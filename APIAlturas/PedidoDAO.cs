using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class PedidoDAO
    {
        private readonly IConfiguration _configuration;
        private readonly ProdutoDao _produtoDao;
        private readonly UsersDAO _userDao;

        public PedidoDAO(IConfiguration configuration, ProdutoDao produtoDAO, UsersDAO userDAO)
        {
            _configuration = configuration;
            _produtoDao = produtoDAO;
            _userDao = userDAO;
        }

        public Guid InsertToCart(Carrinho carrinho)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                try
                {
                    if (carrinho.IsEntrega == true)
                    {
                        carrinho.OrderType = "DELIVERY";
                    } else if (carrinho.IsEntrega == false)
                    {
                        carrinho.OrderType = "TAKEOUT";
                    }

                    var sqlPedido = new StringBuilder();
                    sqlPedido.AppendLine(
                        "Insert Into Pedidos(PedidoGuid, UsuarioGuid, DataHora, RestauranteId, Subtotal, DescontoTotal, EntregaTotal,");
                    sqlPedido.AppendLine("Total, Coupom, Cpf, IsEntrega, Troco, FormaPagamento, Observacao, Token, IsPagamentoOnline,");
                    sqlPedido.AppendLine("PaymentNsu, TotalOnline, RepresentanteId, Referencia,  OrderType, TotenId, ComandaId)");
                    sqlPedido.AppendLine("Values (@PedidoGuid, @UsuarioGuid, @DataHora, @RestauranteId, @Subtotal, @DescontoTotal, @EntregaTotal,");
                    sqlPedido.AppendLine("@Total, @Coupom, @Cpf, @IsEntrega, @Troco, @FormaPagamento, @Observacao, @Token, @IsPagamentoOnline,");
                    sqlPedido.AppendLine("@PaymentNsu, @TotalOnline, @RepresentanteId, @Referencia, @OrderType, @TotenId, @ComandaId)");


                    var parms = new DynamicParameters();
                    carrinho.CarrinhoIdGuid = Guid.NewGuid();
                    parms.Add("@PedidoGuid", carrinho.CarrinhoIdGuid);
                    parms.Add("@UsuarioGuid", carrinho.UsuarioGuid);
                    parms.Add("@DataHora", DateTime.Now);
                    parms.Add("@RestauranteId", carrinho.Restaurante.RestauranteId);
                    parms.Add("@Subtotal", carrinho.CartTotal.SubTotal);
                    parms.Add("@DescontoTotal", carrinho.CartTotal.DiscountTotal);
                    parms.Add("@EntregaTotal", carrinho.CartTotal.TaxTotal);
                    parms.Add("@Total", carrinho.CartTotal.Total);
                    parms.Add("@Coupom", carrinho.Coupom);
                    parms.Add("@Cpf", carrinho.Cpf);
                    parms.Add("@IsEntrega", carrinho.IsEntrega);
                    parms.Add("@Troco", carrinho.Troco);
                    parms.Add("@FormaPagamento", carrinho.FormaPagamento);
                    parms.Add("@Observacao", carrinho.Observacao);
                    parms.Add("@Token", carrinho.Restaurante.Token);
                    parms.Add("@IsPagamentoOnline", carrinho.IsPagamentoOnline);
                    parms.Add("@PaymentNsu", carrinho.PaymentNsu);
                    parms.Add("@TotalOnline", carrinho.CartTotal.TotalOnline);
                    parms.Add("@RepresentanteId", carrinho.RepresentanteId);
                    parms.Add("@Referencia", carrinho.Referencia);
                    parms.Add("@OrderType", carrinho.OrderType);
                    parms.Add("@ComandaId", carrinho.ComandaId);

                    var numAleatorio = new Random();
                    parms.Add("@TotenId", numAleatorio.Next(0, 999));

                    conn.Query(sqlPedido.ToString(), parms);

                    //Itens
                    var sqlPedidoItens = new StringBuilder();
                    sqlPedidoItens.AppendLine(
                        "Insert Into PedidoItens(PedidoItemGuid, PedidoGuid, ProdutoId, Quantidade,");
                    sqlPedidoItens.AppendLine("Observacao, Valorprodutos, Valortotal) Values (");
                    sqlPedidoItens.AppendLine("@PedidoItemGuid, @PedidoGuid, @ProdutoId, @Quantidade,");
                    sqlPedidoItens.AppendLine("@Observacao, @Valorprodutos, @Valortotal)");

                    foreach (var itemCartIten in carrinho.CartItens)
                    {
                        var parmsItens = new DynamicParameters();

                        itemCartIten.PedidoItemGuid = Guid.NewGuid();
                        parmsItens.Add("@PedidoItemGuid", itemCartIten.PedidoItemGuid);
                        parmsItens.Add("@PedidoGuid", carrinho.CarrinhoIdGuid);
                        parmsItens.Add("@ProdutoId", itemCartIten.Produto.ProdutoId);

                        //Ate achar qual o problema no app esse if acerta quanto a qtde vem 2 e os valores de produto e total igual
                        if (itemCartIten.Valorprodutos == itemCartIten.Valortotal)
                            itemCartIten.Quantidade = 1;

                        parmsItens.Add("@Quantidade", itemCartIten.Quantidade);
                        parmsItens.Add("@Observacao", itemCartIten.Observacao);
                        parmsItens.Add("@Valorprodutos", itemCartIten.Valorprodutos);
                        parmsItens.Add("@Valortotal", itemCartIten.Valortotal);

                        conn.Query(sqlPedidoItens.ToString(), parmsItens);

                        //Verifica se produto controla estoque
                        if (itemCartIten.Produto.IsControlstock)
                        {
                            var sqlStock =
                                "Update Produtos set Stock = Isnull(Stock,0) - @stock Where ProdutoId = @produtoId";
                            conn.Query(sqlStock,
                                new { stock = itemCartIten.Quantidade, produtoId = itemCartIten.Produto.ProdutoId });

                        }

                        //Pedido Item Opções 
                        if (itemCartIten.CartItemOpcoes == null) itemCartIten.CartItemOpcoes = new List<CartItemOpcao>();
                        foreach (var cartItemOpcao in itemCartIten.CartItemOpcoes)
                        {
                            var sqlOpcoes = new StringBuilder();
                            sqlOpcoes.AppendLine("Insert Into PedidoItemOpcoes(PedidoItemOpcaoId, PedidoItemGuid, PedidoGuid, Tipo, Nome, Quantidade, Valor, ProdutoPdv)");
                            sqlOpcoes.AppendLine("Values(@PedidoItemOpcaoId, @PedidoItemGuid, @PedidoGuid, @Tipo, @Nome, @Quantidade, @Valor, @ProdutoPdv)");

                            var parmsOpcoes = new DynamicParameters();
                            parmsOpcoes.Add("@PedidoItemOpcaoId", Guid.NewGuid());
                            parmsOpcoes.Add("@PedidoItemGuid", itemCartIten.PedidoItemGuid);
                            parmsOpcoes.Add("@PedidoGuid", carrinho.CarrinhoIdGuid);
                            parmsOpcoes.Add("@Tipo", cartItemOpcao.Tipo);
                            parmsOpcoes.Add("@Nome", cartItemOpcao.Nome);
                            parmsOpcoes.Add("@Quantidade", cartItemOpcao.Quantidade);
                            parmsOpcoes.Add("@Valor", cartItemOpcao.Valor);
                            parmsOpcoes.Add("@ProdutoPdv", cartItemOpcao.ProdutoPdv);

                            conn.Query(sqlOpcoes.ToString(), parmsOpcoes);
                        }

                        //Complementos
                        if (itemCartIten.CartComplementos == null) itemCartIten.CartComplementos = new List<CartComplemento>();
                        foreach (var cartComplemento in itemCartIten.CartComplementos)
                        {

                            var sqlComplementos = new StringBuilder();
                            sqlComplementos.AppendLine("Insert Into PedidoComplementos (PedidoItemGuid, PedidoGuid, ComplementoId, Descricao, Valor) Values (");
                            sqlComplementos.AppendLine("@PedidoItemGuid, @PedidoGuid, @ComplementoId, @Descricao, @Valor)");

                            var parmsComplemento = new DynamicParameters();
                            parmsComplemento.Add("@PedidoItemGuid", itemCartIten.PedidoItemGuid);
                            parmsComplemento.Add("@PedidoGuid", carrinho.CarrinhoIdGuid);
                            parmsComplemento.Add("@ComplementoId", 0);
                            parmsComplemento.Add("@Descricao", cartComplemento.Descricao);
                            parmsComplemento.Add("@Valor", cartComplemento.Valor);

                            conn.Query(sqlComplementos.ToString(), parmsComplemento);
                        }
                        //Pedidos Meio/Meio
                        if (itemCartIten.CartMeioMeios == null) itemCartIten.CartMeioMeios = new List<CartMeioMeio>();
                        foreach (var cartComplemento in itemCartIten.CartMeioMeios)
                        {


                            var sqlComplementos = new StringBuilder();
                            sqlComplementos.AppendLine("Insert Into PedidoMeioMeios (PedidoItemGuid, PedidoGuid, ProdutoId, Descricao, Valor, MeioMeioGuid) Values (");
                            sqlComplementos.AppendLine("@PedidoItemGuid, @PedidoGuid, @ProdutoId, @Descricao, @Valor, @MeioMeioGuid)");

                            var parmsComplemento = new DynamicParameters();
                            cartComplemento.MeioMeioGuid = Guid.NewGuid();
                            parmsComplemento.Add("@PedidoItemGuid", itemCartIten.PedidoItemGuid);
                            parmsComplemento.Add("@PedidoGuid", carrinho.CarrinhoIdGuid);
                            parmsComplemento.Add("@ProdutoId", cartComplemento.ProdutoId);
                            parmsComplemento.Add("@Descricao", cartComplemento.Descricao);
                            parmsComplemento.Add("@Valor", cartComplemento.Valor);
                            parmsComplemento.Add("@MeioMeioGuid", cartComplemento.MeioMeioGuid);

                            conn.Query(sqlComplementos.ToString(), parmsComplemento);


                            // Complementos do Meio/Meio

                            if (cartComplemento.CartComplementosMeioMeios == null) cartComplemento.CartComplementosMeioMeios = new List<CartComplementosMeioMeio>();
                            foreach (var CartComplementoMeioMeio in cartComplemento.CartComplementosMeioMeios)
                            {
                                var sqlComplementos2 = new StringBuilder();
                                sqlComplementos2.AppendLine("Insert Into PedidoMeioMeioComplementos (MeioMeioGuid, Complemento, Valor, Quantidade, PedidoGuid) Values (");
                                sqlComplementos2.AppendLine("@MeioMeioGuid, @Complemento, @Valor, @Quantidade, @PedidoGuid)");

                                var parmsComplemento2 = new DynamicParameters();
                                parmsComplemento2.Add("@MeioMeioGuid", cartComplemento.MeioMeioGuid);
                                parmsComplemento2.Add("@PedidoGuid", carrinho.CarrinhoIdGuid);
                                parmsComplemento2.Add("@Complemento", CartComplementoMeioMeio.Complemento);
                                parmsComplemento2.Add("@Quantidade", CartComplementoMeioMeio.Quantidade);
                                parmsComplemento2.Add("@Valor", CartComplementoMeioMeio.Valor);

                                conn.Query(sqlComplementos2.ToString(), parmsComplemento2);
                            }

                        }
                    }

                    if (carrinho.Adress != null)
                    {
                        if (carrinho.Adress.bairro != null && carrinho.Adress.uf != null && carrinho.Adress.cep != null && carrinho.Adress.logradouro != null && carrinho.Adress.localidade != null)
                        {
                            //Endereço entrega
                            if (string.IsNullOrEmpty(carrinho.Adress?.cep) && carrinho.IsEntrega)
                                throw new Exception("Encontramos problema no endereço.");


                            var sqlEndereco = new StringBuilder();
                            sqlEndereco.AppendLine("Insert Into PedidoEntrega(PedidoEntregaGuid, PedidoGuid, UsuarioGuid, cep, logradouro, complemento,");
                            sqlEndereco.AppendLine("bairro, localidade, uf, unidade, numero) Values(@PedidoEntregaGuid, @PedidoGuid, @UsuarioGuid,");
                            sqlEndereco.AppendLine("@cep, @logradouro, @complemento, @bairro, @localidade, @uf, @unidade, @numero)");

                            var parmsEndereco = new DynamicParameters();
                            parmsEndereco.Add("@PedidoEntregaGuid", Guid.NewGuid());
                            parmsEndereco.Add("@PedidoGuid", carrinho.CarrinhoIdGuid);
                            parmsEndereco.Add("@UsuarioGuid", carrinho.UsuarioGuid);
                            parmsEndereco.Add("@cep", carrinho.Adress.cep);
                            parmsEndereco.Add("@logradouro", carrinho.Adress.logradouro);
                            parmsEndereco.Add("@complemento", carrinho.Adress.complemento);
                            parmsEndereco.Add("@bairro", carrinho.Adress.bairro);
                            parmsEndereco.Add("@localidade", carrinho.Adress.localidade);
                            parmsEndereco.Add("@uf", carrinho.Adress.uf);
                            parmsEndereco.Add("@unidade", carrinho.Adress.unidade);
                            parmsEndereco.Add("@numero", carrinho.Adress.numero);

                            conn.Query(sqlEndereco.ToString(), parmsEndereco);
                        }
                    }

                    //Grava Cupom
                    if (carrinho.AppliedCoupons)
                    {
                        var sqlCupom = new StringBuilder();
                        sqlCupom.AppendLine("Insert Into CupomMovimentacoes(CupomMovimentacaoId, CupomId, UserId, DataHora, PedidoId, Valor)");
                        sqlCupom.AppendLine("Values (@CupomMovimentacaoId, @CupomId, @UserId, @DataHora, @PedidoId, @Valor)");
                        var parmsCupom = new DynamicParameters();
                        parmsCupom.Add("CupomMovimentacaoId", Guid.NewGuid());
                        parmsCupom.Add("@CupomId", carrinho.Cupom.CupomId);
                        parmsCupom.Add("@UserId", carrinho.UsuarioGuid);
                        parmsCupom.Add("@DataHora", carrinho.Cupom.DataHora);
                        parmsCupom.Add("@PedidoId", carrinho.CarrinhoIdGuid);
                        parmsCupom.Add("@Valor", carrinho.Cupom.Valor);

                        conn.Query(sqlCupom.ToString(), parmsCupom);
                    }
                    //Verifica se pedido é um pedido de mesa
                    if (carrinho.OrderType == "TABLE")
                    {
                        var sqlMesa = new StringBuilder();
                        sqlMesa.AppendLine("Select * from PedidoMesas Where Situacao = 1 and RestauranteId = @restauranteId And MesaId = @idmesa");
                        var pedidoMesa = conn.Query<PedidoMesa>(sqlMesa.ToString(), 
                            new
                            {
                                idmesa = carrinho.Referencia,
                                restauranteId = carrinho.Restaurante.RestauranteId
                            }).FirstOrDefault();

                        if (pedidoMesa == null)
                        {
                            pedidoMesa = new PedidoMesa()
                            {
                                MesaId = carrinho.Referencia,
                                RestauranteId = carrinho.Restaurante.RestauranteId
                            };

                            sqlMesa = new StringBuilder();
                            sqlMesa.AppendLine(
                                "Insert Into PedidoMesas(PedidoMesaId, MesaId, RestauranteId, DataHora, Situacao)");
                            sqlMesa.AppendLine("Values (@PedidoMesaId, @MesaId, @RestauranteId, @DataHora, @Situacao)");

                            var parmsMesa = new DynamicParameters();
                            parmsMesa.Add("@PedidoMesaId", pedidoMesa.PedidoMesaId);
                            parmsMesa.Add("@MesaId", pedidoMesa.MesaId);
                            parmsMesa.Add("@RestauranteId", pedidoMesa.RestauranteId);
                            parmsMesa.Add("@DataHora", pedidoMesa.DataHora);
                            parmsMesa.Add("@Situacao", pedidoMesa.Situacao);
                            conn.Query(sqlMesa.ToString(), parmsMesa);


                        }
                        var sqlMesaItem = new StringBuilder();
                        sqlMesaItem.AppendLine("Insert Into PedidoMesaItens(PedidoMesaItemId, PedidoMesaId, PedidoId, DataHora)");
                        sqlMesaItem.AppendLine("Values (@PedidoMesaItemId, @PedidoMesaId, @PedidoId, @DataHora)");
                        var parmsMesaItem = new DynamicParameters();
                        parmsMesaItem.Add("PedidoMesaItemId", Guid.NewGuid());
                        parmsMesaItem.Add("@PedidoMesaId", pedidoMesa.PedidoMesaId);
                        parmsMesaItem.Add("@PedidoId", carrinho.CarrinhoIdGuid);
                        parmsMesaItem.Add("@DataHora", DateTime.Now);

                        conn.Query(sqlMesaItem.ToString(), parmsMesaItem);


                    }
                    //Grava ZCash
                    var sqlZCash = new StringBuilder();
                    sqlZCash.AppendLine("Insert Into ZCashMovimentacao(ZCashMovimentacaoId, PedidoGuid, RestauranteId, UsuarioId, TipoOperacao, Operacao, DataHora, ValorPedido)");
                    sqlZCash.AppendLine("Values (@ZCashMovimentacaoId, @PedidoGuid, @RestauranteId, @UsuarioId, @TipoOperacao, @Operacao, @DataHora, @ValorPedido)");
                    var parmsZCash = new DynamicParameters();
                    parmsZCash.Add("ZCashMovimentacaoId", Guid.NewGuid());
                    parmsZCash.Add("@PedidoGuid", carrinho.CarrinhoIdGuid);
                    parmsZCash.Add("@RestauranteId", carrinho.Restaurante.RestauranteId);
                    parmsZCash.Add("@UsuarioId", carrinho.UsuarioGuid);
                    parmsZCash.Add("@TipoOperacao", 1); // Credito
                    parmsZCash.Add("@Operacao", (int)OperacaoEnumView.PedidoApp);
                    parmsZCash.Add("@DataHora", DateTime.Now);
                    parmsZCash.Add("@ValorPedido", carrinho.CartTotal.Total);

                    conn.Query(sqlZCash.ToString(), parmsZCash);
                    //Aualiza o saldo de pontos do cliente
                    conn.Query("Update Users Set ZCash = Isnull(ZCash,0) + @valor Where userID = @UsuarioId",
                        new { valor = carrinho.CartTotal.Total, UsuarioId = carrinho.UsuarioGuid });

                    //Grava Pollings 
                    var sqlPoolins = new StringBuilder();
                    sqlPoolins.AppendLine("Insert Into Pollings(Id, token, statuss, correlationId, createdAt, historico)");
                    sqlPoolins.AppendLine("Values (@Id, @token, @statuss, @correlationId, @createdAt, @historico)");
                    var parmsPoolins = new DynamicParameters();
                    parmsPoolins.Add("@Id", Guid.NewGuid());
                    parmsPoolins.Add("token", carrinho.Restaurante.Token);
                    parmsPoolins.Add("correlationId", carrinho.CarrinhoIdGuid);
                    parmsPoolins.Add("statuss", (int)PedidoStatusEnumView.PLACED);
                    parmsPoolins.Add("@createdAt", DateTime.Now);
                    parmsPoolins.Add("@historico", "Inclusão do pedido");

                    conn.Query(sqlPoolins.ToString(), parmsPoolins);

                    return carrinho.CarrinhoIdGuid;
                }
                catch (Exception e)
                {
                    var sqlException = new StringBuilder();
                    sqlException.AppendLine("Delete From Pedidos Where PedidoGuid = @PedidoGuid");
                    sqlException.AppendLine("Delete From PedidoItens Where PedidoGuid = @PedidoGuid");
                    sqlException.AppendLine("Delete From PedidoComplementos Where PedidoGuid = @PedidoGuid");

                    conn.Query(sqlException.ToString(), new { PedidoGuid = carrinho.CarrinhoIdGuid });

                    throw new Exception("Erro ao gravar o pedido. Tente novamente mais tarde.");
                }
            }
        }

        public IEnumerable<Pedido> GetPedidosByRestaurateId(Guid token)
        {
            var sql = new StringBuilder();
            sql.AppendLine("select *,");
            sql.AppendLine("(SELECT statuss FROM pollings WHERE correlationId = Pedidos.pedidoGuid) AS pedidoStatus");
            sql.AppendLine(" FROM Pedidos");
            sql.AppendLine("Inner Join Users On Pedidos.UsuarioGuid = Users.UserID");
            sql.AppendLine("Left Join PedidoEntrega On Pedidos.PedidoGuid = PedidoEntrega.PedidoGuid");
            sql.AppendLine("Where Pedidos.Situacao in (0,1) And Pedidos.restauranteId In (Select restauranteId From Restaurantes Where token = @token)");
            sql.AppendLine("Order By Pedidos.DataHora");


            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var pedidos = conn.Query<Pedido, User, PedidoEntrega, Pedido>(sql.ToString(),
                    (p, user, entrega) =>
                    {
                        p.User = user;
                        p.PedidoEntrega = entrega;
                        return p;
                    }, new { token }, splitOn: "PedidoGuid, UserID, PedidoEntregaGuid").ToList();


                foreach (var pedido in pedidos)
                {
                    var sqlItens = new StringBuilder();
                    sqlItens.AppendLine("select * from PedidoItens");
                    sqlItens.AppendLine("Inner Join Produtos On PedidoItens.produtoId = Produtos.produtoId");
                    sqlItens.AppendLine("Left Join PedidoComplementos On PedidoItens.PedidoItemGuid = PedidoComplementos.PedidoItemGuid");
                    sqlItens.AppendLine("Left Join PedidoItemOpcoes On PedidoItens.PedidoItemGuid = PedidoItemOpcoes.PedidoItemGuid");
                    sqlItens.AppendLine("Left Join PedidoMeioMeios On PedidoItens.PedidoItemGuid = PedidoMeioMeios.PedidoItemGuid");
                    sqlItens.AppendLine("Where  PedidoItens.PedidoGuid = @pedidoGuid");

                    var lookup = new Dictionary<Guid, PedidoItem>();
                    conn.Query<PedidoItem, Produto, PedidoComplemento, PedidoItemOpcao, PedidoMeioMeio, PedidoItem>(sqlItens.ToString(),
                            (s, a, b, io, mm) =>
                            {
                                PedidoItem shop;
                                if (!lookup.TryGetValue(s.PedidoItemGuid, out shop))
                                {
                                    lookup.Add(s.PedidoItemGuid, shop = s);
                                }
                                shop.Produto = a;
                                if (shop.PedidoComplementos == null)
                                    shop.PedidoComplementos = new List<PedidoComplemento>();
                                if (shop.PedidoMeioMeios == null)
                                    shop.PedidoMeioMeios = new List<PedidoMeioMeio>();
                                if (shop.PedidoItemOpcoes == null)
                                    shop.PedidoItemOpcoes = new List<PedidoItemOpcao>();

                                if (b != null)
                                    shop.PedidoComplementos.Add(b);
                                if (mm != null)
                                    shop.PedidoMeioMeios.Add(mm);
                                if (io != null)
                                {
                                    if (!shop.PedidoItemOpcoes.Any(t => t.PedidoItemOpcaoId == io.PedidoItemOpcaoId))
                                        shop.PedidoItemOpcoes.Add(io);
                                }


                                return shop;
                            }, new { pedidoGuid = pedido.PedidoGuid },
                            splitOn: "PedidoItemGuid, ProdutoId, PedidoComplementoId, PedidoItemOpcaoId,  PedidoMeioMeioId")
                        .AsQueryable();
                    var resultList = lookup.Values;

                    pedido.PedidoItens = resultList;
                }
                conn.Close();

                return pedidos;
            }

        }

        public Pedido GetPedidoById(Guid token, string id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("select * from Pedidos");
            sql.AppendLine("Left Join Users On Pedidos.UsuarioGuid = Users.UserID");
            sql.AppendLine("Left Join PedidoEntrega On Pedidos.PedidoGuid = PedidoEntrega.PedidoGuid");
            sql.AppendLine(
                "Where  Pedidos.restauranteId In (Select restauranteId From Restaurantes Where token = @token)");
            sql.AppendLine("And Pedidos.PedidoGuid = @id");
            sql.AppendLine("Order By Pedidos.DataHora");


            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var pedido = conn.Query<Pedido, User, PedidoEntrega, Pedido>(sql.ToString(),
                    (p, user, entrega) =>
                    {
                        p.User = user;
                        p.PedidoEntrega = entrega;
                        return p;
                    }, new { token, id }, splitOn: "PedidoGuid, UserID, PedidoEntregaGuid").FirstOrDefault();


                var sqlItens = new StringBuilder();
                sqlItens.AppendLine("select * from PedidoItens");
                sqlItens.AppendLine("Inner Join Produtos On PedidoItens.produtoId = Produtos.produtoId");
                sqlItens.AppendLine("Left Join PedidoComplementos On PedidoItens.PedidoItemGuid = PedidoComplementos.PedidoItemGuid");
                sqlItens.AppendLine("Left Join PedidoItemOpcoes On PedidoItens.PedidoItemGuid = PedidoItemOpcoes.PedidoItemGuid");
                sqlItens.AppendLine("Left Join PedidoMeioMeios On PedidoItens.PedidoItemGuid = PedidoMeioMeios.PedidoItemGuid");
                sqlItens.AppendLine("Where  PedidoItens.PedidoGuid = @pedidoGuid");

                var lookup = new Dictionary<Guid, PedidoItem>();
                conn.Query<PedidoItem, Produto, PedidoComplemento, PedidoItemOpcao, PedidoMeioMeio, PedidoItem>(sqlItens.ToString(),
                        (s, a, b, io, mm) =>
                        {
                            PedidoItem shop;
                            if (!lookup.TryGetValue(s.PedidoItemGuid, out shop))
                            {
                                lookup.Add(s.PedidoItemGuid, shop = s);
                            }
                            shop.Produto = a;
                            if (shop.PedidoComplementos == null)
                                shop.PedidoComplementos = new List<PedidoComplemento>();
                            if (shop.PedidoMeioMeios == null)
                                shop.PedidoMeioMeios = new List<PedidoMeioMeio>();
                            if (shop.PedidoItemOpcoes == null)
                                shop.PedidoItemOpcoes = new List<PedidoItemOpcao>();

                            if (b != null)
                                shop.PedidoComplementos.Add(b);
                            if (mm != null)
                            {
                                shop.PedidoMeioMeios.Add(mm);

                                if (mm.PedidoMeioMeioComplementos == null)
                                mm.PedidoMeioMeioComplementos = new List<PedidoMeioMeioComplemento>();


                                var MeioMeioGuid = mm.MeioMeioGuid;

                                using (var conn2 = new SqlConnection(
                                _configuration.GetConnectionString("ViPFood")))
                                {
                                    conn2.Open();
                                    var prop = conn
                                        .Query<PedidoMeioMeioComplemento>("select * from PedidoMeioMeioComplementos where MeioMeioGuid = @MeioMeioGuid", new { MeioMeioGuid })
                                        .ToList();
                                    conn2.Close();

                                    if (prop.Count != 0)
                                    {
                                        for (int i = 0; i < prop.Count; i++)
                                        {
                                            var teste = prop[i];
                                            mm.PedidoMeioMeioComplementos.Add(teste);
                                        }
                                    }
                                }
                            }

                            if (io != null)
                            {
                                if (!shop.PedidoItemOpcoes.Any(t => t.PedidoItemOpcaoId == io.PedidoItemOpcaoId))
                                    shop.PedidoItemOpcoes.Add(io);
                            }

                            return shop;
                        }, new { pedidoGuid = pedido.PedidoGuid },
                        splitOn: "PedidoItemGuid, ProdutoId, PedidoComplementoId, PedidoItemOpcaoId,  PedidoMeioMeioId")
                    .AsQueryable();
                var resultList = lookup.Values;

                pedido.PedidoItens = resultList;

                conn.Close();
                return pedido;
            }
        }
        public IEnumerable<Pedido> GetPedidosByUsuario(Guid token)
        {
            var sql = new StringBuilder();
            sql.AppendLine("select * from Pedidos");
            sql.AppendLine("Inner Join Users On Pedidos.UsuarioGuid = Users.UserID");
            sql.AppendLine("Inner Join Restaurantes On Pedidos.RestauranteId = Restaurantes.restauranteId");
            sql.AppendLine("Left Join PedidoEntrega On Pedidos.PedidoGuid = PedidoEntrega.PedidoGuid");
            sql.AppendLine("Where Pedidos.UsuarioGuid = @token");
            sql.AppendLine("Order By Pedidos.DataHora Desc");


            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var pedidos = conn.Query<Pedido, User, Restaurante, PedidoEntrega, Pedido>(sql.ToString(),
                    (p, user, restaurante, entrega) =>
                    {
                        p.User = user;
                        p.Restaurante = restaurante;
                        p.PedidoEntrega = entrega;
                        return p;
                    }, new { token }, splitOn: "PedidoGuid, UserID, restauranteId, PedidoEntregaGuid").ToList();


                foreach (var pedido in pedidos)
                {
                    var sqlItens = new StringBuilder();
                    sqlItens.AppendLine("select * from PedidoItens");
                    sqlItens.AppendLine("Inner Join Produtos On PedidoItens.produtoId = Produtos.produtoId");
                    sqlItens.AppendLine("Left Join PedidoComplementos On PedidoItens.PedidoItemGuid = PedidoComplementos.PedidoItemGuid");
                    sqlItens.AppendLine("Left Join PedidoItemOpcoes On PedidoItens.PedidoItemGuid = PedidoItemOpcoes.PedidoItemGuid");
                    sqlItens.AppendLine("Left Join PedidoMeioMeios On PedidoItens.PedidoItemGuid = PedidoMeioMeios.PedidoItemGuid");
                    sqlItens.AppendLine("Where  PedidoItens.PedidoGuid = @pedidoGuid");

                    var lookup = new Dictionary<Guid, PedidoItem>();
                    conn.Query<PedidoItem, Produto, PedidoComplemento, PedidoItemOpcao, PedidoMeioMeio, PedidoItem>(sqlItens.ToString(),
                            (s, a, b, io, mm) =>
                            {
                                PedidoItem shop;
                                if (!lookup.TryGetValue(s.PedidoItemGuid, out shop))
                                {
                                    lookup.Add(s.PedidoItemGuid, shop = s);
                                }
                                shop.Produto = a;
                                if (shop.PedidoComplementos == null)
                                    shop.PedidoComplementos = new List<PedidoComplemento>();
                                if (shop.PedidoMeioMeios == null)
                                    shop.PedidoMeioMeios = new List<PedidoMeioMeio>();
                                if (shop.PedidoItemOpcoes == null)
                                    shop.PedidoItemOpcoes = new List<PedidoItemOpcao>();

                                if (b != null)
                                    shop.PedidoComplementos.Add(b);
                                if (mm != null)
                                    shop.PedidoMeioMeios.Add(mm);
                                if (io != null)
                                {
                                    if (!shop.PedidoItemOpcoes.Any(t => t.PedidoItemOpcaoId == io.PedidoItemOpcaoId))
                                        shop.PedidoItemOpcoes.Add(io);
                                }



                                return shop;
                            }, new { pedidoGuid = pedido.PedidoGuid },
                            splitOn: "PedidoItemGuid, ProdutoId, PedidoComplementoId, PedidoItemOpcaoId,  PedidoMeioMeioId")
                        .AsQueryable();
                    var resultList = lookup.Values;

                    pedido.PedidoItens = resultList;
                }
                conn.Close();

                return pedidos;
            }

        }

        public IEnumerable<Pedido> GetByEnactment(Guid token)
        {
            var sql = new StringBuilder();
            sql.AppendLine("select * from Pedidos");
            sql.AppendLine("Inner Join Users On Pedidos.UsuarioGuid = Users.UserID");
            sql.AppendLine("Inner Join Restaurantes On Pedidos.RestauranteId = Restaurantes.restauranteId");
            sql.AppendLine("Left Join PedidoEntrega On Pedidos.PedidoGuid = PedidoEntrega.PedidoGuid");
            sql.AppendLine("Where Pedidos.UsuarioGuid = @token And Pedidos.Situacao In (1,2,3,5,8) ");
            sql.AppendLine("Order By Pedidos.DataHora Desc");


            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var pedidos = conn.Query<Pedido, User, Restaurante, PedidoEntrega, Pedido>(sql.ToString(),
                    (p, user, restaurante, entrega) =>
                    {
                        p.User = user;
                        p.Restaurante = restaurante;
                        p.PedidoEntrega = entrega;
                        return p;
                    }, new { token }, splitOn: "PedidoGuid, UserID, restauranteId, PedidoEntregaGuid").ToList();


                foreach (var pedido in pedidos)
                {
                    var sqlItens = new StringBuilder();
                    sqlItens.AppendLine("select * from PedidoItens");
                    sqlItens.AppendLine("Inner Join Produtos On PedidoItens.produtoId = Produtos.produtoId");
                    sqlItens.AppendLine("Left Join PedidoComplementos On PedidoItens.PedidoItemGuid = PedidoComplementos.PedidoItemGuid");
                    sqlItens.AppendLine("Left Join PedidoItemOpcoes On PedidoItens.PedidoItemGuid = PedidoItemOpcoes.PedidoItemGuid");
                    sqlItens.AppendLine("Left Join PedidoMeioMeios On PedidoItens.PedidoItemGuid = PedidoMeioMeios.PedidoItemGuid");
                    sqlItens.AppendLine("Where  PedidoItens.PedidoGuid = @pedidoGuid");

                    var lookup = new Dictionary<Guid, PedidoItem>();
                    conn.Query<PedidoItem, Produto, PedidoComplemento, PedidoItemOpcao, PedidoMeioMeio, PedidoItem>(sqlItens.ToString(),
                            (s, a, b, io, mm) =>
                            {
                                PedidoItem shop;
                                if (!lookup.TryGetValue(s.PedidoItemGuid, out shop))
                                {
                                    lookup.Add(s.PedidoItemGuid, shop = s);
                                }
                                shop.Produto = a;
                                if (shop.PedidoComplementos == null)
                                    shop.PedidoComplementos = new List<PedidoComplemento>();
                                if (shop.PedidoMeioMeios == null)
                                    shop.PedidoMeioMeios = new List<PedidoMeioMeio>();
                                if (shop.PedidoItemOpcoes == null)
                                    shop.PedidoItemOpcoes = new List<PedidoItemOpcao>();

                                if (b != null)
                                    shop.PedidoComplementos.Add(b);
                                if (mm != null)
                                    shop.PedidoMeioMeios.Add(mm);
                                if (io != null)
                                {
                                    if (!shop.PedidoItemOpcoes.Any(t => t.PedidoItemOpcaoId == io.PedidoItemOpcaoId))
                                        shop.PedidoItemOpcoes.Add(io);
                                }

                                return shop;
                            }, new { pedidoGuid = pedido.PedidoGuid },
                            splitOn: "PedidoItemGuid, ProdutoId, PedidoComplementoId, PedidoItemOpcaoId,  PedidoMeioMeioId")
                        .AsQueryable();
                    var resultList = lookup.Values;

                    pedido.PedidoItens = resultList;
                }
                conn.Close();

                return pedidos;
            }

        }

        public int ObterStatusById(string pedidoId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("select Isnull(Situacao,0) as Situacao from Pedidos Where PedidoGuid = @pedidoId");
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var status = conn.Query<int>(sql.ToString(), new { pedidoId }).FirstOrDefault();
                conn.Close();

                return status;


            }
        }

        public void Modificar(PedidoModify pedidoModify)
        {
            var sql = $"Update Pedidos Set Situacao = @situacao Where PedidoGuid = @pedidoGuid";
            var parms = new DynamicParameters();
            parms.Add("@pedidoGuid", pedidoModify.PedidoGuid);
            parms.Add("@situacao", pedidoModify.Situacao);
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Query(sql, parms);
            }

        }

        public IEnumerable<PollingModel> ObterPollingByRestaurante(Guid token)
        {
            //var sql = "select * from Pollings Where statuss In (1, 4) And   token = @token";
            var sql = "select * from Pollings Where statuss In (1) And   token = @token";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var pollings = conn.Query<PollingModel>(sql, new { token });

                conn.Query("Update Restaurantes Set DataHoraPooling = GetDate(), Situacao = 1 Where token = @token And Situacao = 0", new { token });

                conn.Close();




                return pollings;
            }
        }

        public bool AtrualizaStatus(PedidoStatusEnumView statuss, string rederence, string token)
        {
            var sql = "Update Pollings set statuss = @statuss Where correlationId = @rederence And token = @token;";
            sql += " Update Pedidos Set Situacao = @statuss Where PedidoGuid = @rederence";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql, new { statuss, rederence, token });
                conn.Close();
            }
            return true;
        }

        public IEnumerable<Pedido> getOrderByComandaId(string comandaId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM pedidos");
            sql.AppendLine("INNER JOIN pedidoItens ON pedidos.pedidoGuid = pedidoItens.PedidoGuid");
            sql.AppendLine("INNER JOIN produtos ON produtos.produtoId = PedidoItens.ProdutoId");
            sql.AppendLine("LEFT JOIN PedidoItemOpcoes ON PedidoItens.PedidoItemGuid = PedidoItemOpcoes.PedidoItemGuid");
            sql.AppendLine("LEFT JOIN PedidoMeioMeios ON PedidoMeioMeios.PedidoItemGuid = PedidoItens.pedidoItemGuid");
            sql.AppendLine("WHERE comandaId like @comandaId");

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                //conn.StatisticsEnabled = true;
                var lookup = new Dictionary<Guid, Pedido>();
                //var lookupOrderItem = new Dictionary<Guid, PedidoItem>();
                conn.Open();
                var orders = conn.Query<Pedido, PedidoItem, Produto, PedidoItemOpcao, PedidoMeioMeio, Pedido>(
                    sql: sql.ToString(),
                    map: (order, orderItem, itemProd, itemOpt, itemHalf) =>
                    {
                        Pedido ord;
                        if (!lookup.TryGetValue(order.PedidoGuid, out ord)) lookup.Add(order.PedidoGuid, ord = order);
                        //if(!lookupOrderItem.TryGetValue(orderItens.PedidoItemGuid, out orderItens)) lookup.

                        if (ord.PedidoItens == null) ord.PedidoItens = new List<PedidoItem>();
                        if (orderItem.PedidoItemOpcoes == null) orderItem.PedidoItemOpcoes = new List<PedidoItemOpcao>();
                        if (orderItem.PedidoMeioMeios == null) orderItem.PedidoMeioMeios = new List<PedidoMeioMeio>();
                        orderItem.Produto = itemProd;
                        if (itemOpt != null) orderItem.PedidoItemOpcoes.Add(itemOpt);
                        if (itemHalf != null) orderItem.PedidoMeioMeios.Add(itemHalf);
                        /*if (orderItens != null)*/
                        ord.PedidoItens.Add(orderItem);

                        //if (!order.PedidoItens.Any(ord => order.PedidoGuid == ord.PedidoGuid)) order.PedidoItens.Append(orderItens);

                        return ord;
                    },
                    param: new { comandaId },
                    splitOn: "PedidoGuid, PedidoItemGuid, ProdutoId, PedidoItemOpcaoId, PedidoMeioMeioId"
                );
                //var stats = conn.RetrieveStatistics();
                conn.Close();
                return lookup.Values;
            }

        }

        public IEnumerable<Pedido> getOrderHistory(string userId, int limit, int lastId)
        {
            string sql =
                "SELECT TOP " + (limit + " * ") +
                "FROM pedidos AS p " +
                "WHERE p.UsuarioGuid = @userId " +
                (lastId == 0 ? "" : "AND p.pedidoId < @lastId ") +
                "ORDER BY p.dataHora DESC";

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                IEnumerable<Pedido> pedidos = conn.Query<Pedido>(sql, new { userId, lastId });

                foreach (Pedido pedido in pedidos)
                {
                    string sqlItem =
                        "SELECT * FROM pedidoItens " +
                        "WHERE pedidoGuid = @pedidoGuid";

                    IEnumerable<PedidoItem> pedidoItens = conn.Query<PedidoItem>(sqlItem, new { pedido.PedidoGuid });

                    foreach (PedidoItem pedidoItem in pedidoItens)
                    {
                        string sqlPedidoItemOptions =
                            "SELECT * FROM pedidoItemOpcoes " +
                            "WHERE pedidoItemGuid = @pedidoItemGuid";

                        IEnumerable<PedidoItemOpcao> pedidoItemOptions = conn.Query<PedidoItemOpcao>(sqlPedidoItemOptions, new { pedidoItem.PedidoItemGuid });
                        pedidoItem.PedidoItemOpcoes = pedidoItemOptions.ToList();

                        string sqlPedidoItemHalf =
                            "SELECT * FROM pedidoMeioMeios " +
                            "WHERE pedidoItemGuid = @pedidoItemGuid";

                        IEnumerable<PedidoMeioMeio> pedidoItemHalf = conn.Query<PedidoMeioMeio>(sqlPedidoItemHalf, new { pedidoItem.PedidoItemGuid });
                        pedidoItem.PedidoMeioMeios = pedidoItemHalf.ToList();

                        pedidoItem.Produto = conn.Query<Produto>("SELECT * FROM produtos WHERE produtoId = @produtoId", new { pedidoItem.ProdutoId }).First();
                    }

                    pedido.PedidoItens = pedidoItens.ToArray();

                    pedido.User = conn.Query<User>("SELECT * FROM users WHERE userID = @userId", new { userId }).First();

                    pedido.Restaurante = conn.Query<Restaurante>("SELECT * FROM restaurantes WHERE restauranteId = @restauranteId", new { pedido.RestauranteId }).First();

                }

                return pedidos;
            }
        }

        public IEnumerable<PedidoItem> GetPedidoItensByMesaId(int restauranteId, int mesaId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select * from PedidoItens");
            sql.AppendLine("Inner Join PedidoMesaItens On PedidoItens.PedidoGuid = PedidoMesaItens.PedidoId");
            sql.AppendLine("Inner Join PedidoMesas On PedidoMesaItens.PedidoMesaId = PedidoMesas.PedidoMesaId");
            sql.AppendLine("Where PedidoMesas.Situacao = 1 And PedidoMesas.RestauranteId = @restauranteId And PedidoMesas.MesaId = @mesaId");
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var pedidoItens = conn.Query<PedidoItem>(sql.ToString(), new {restauranteId, mesaId});
                conn.Close();


                foreach (var pedidoItem in pedidoItens)
                {
                    string sqlPedidoItemOptions =
                        "SELECT * FROM pedidoItemOpcoes " +
                        "WHERE pedidoItemGuid = @pedidoItemGuid";

                    IEnumerable<PedidoItemOpcao> pedidoItemOptions = conn.Query<PedidoItemOpcao>(sqlPedidoItemOptions, new { pedidoItem.PedidoItemGuid });
                    pedidoItem.PedidoItemOpcoes = pedidoItemOptions.ToList();

                    string sqlPedidoItemHalf =
                        "SELECT * FROM pedidoMeioMeios " +
                        "WHERE pedidoItemGuid = @pedidoItemGuid";

                    IEnumerable<PedidoMeioMeio> pedidoItemHalf = conn.Query<PedidoMeioMeio>(sqlPedidoItemHalf, new { pedidoItem.PedidoItemGuid });
                    pedidoItem.PedidoMeioMeios = pedidoItemHalf.ToList();

                    pedidoItem.Produto = conn.Query<Produto>("SELECT * FROM produtos WHERE produtoId = @produtoId", new { pedidoItem.ProdutoId }).First();
                }


                return pedidoItens;
            }

        }

        //Busca todos os pedidos do restaurantes que não foram concluídos ou cancelados
        public IEnumerable<Pedido> GetOrdersByRestId(int restId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    $"SELECT * FROM pedidos  WHERE restauranteId = @restId  AND situacao NOT IN({ (int)PedidoSituacaoViewEnum.Cancelado },{ (int)PedidoSituacaoViewEnum.Finalizado})";

                conn.Open();
                IEnumerable<Pedido> orders = conn.Query<Pedido>(sql, new { restId });

                foreach(Pedido order in orders)
                {
                    //order.PedidoItens = GetOrderItens(order.PedidoGuid).ToList();
                    if(order.IsEntrega) order.User = _userDao.GetById(order.UsuarioGuid.ToString());
                }

                return orders;
            }
        }

        public IEnumerable<PedidoItem> GetOrderItens(Guid pedidoGuid) {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "SELECT * " +
                    "FROM pedidoItens WHERE pedidoGuid = @pedidoGuid";

                conn.Open();
                IEnumerable<PedidoItem> orderItens = conn.Query<PedidoItem>(sql, new { pedidoGuid });

                foreach(PedidoItem orderItem in orderItens)
                {
                    orderItem.Produto = _produtoDao.GetProdutoPorId(orderItem.ProdutoId);
                    orderItem.PedidoItemOpcoes = GetOrderItemOptions(orderItem.PedidoItemGuid).ToList();
                    orderItem.PedidoMeioMeios = GetOrderItemHalfs(orderItem.PedidoItemGuid).ToList();
                }

                return orderItens;
            }
        }

        public IEnumerable<PedidoItemOpcao> GetOrderItemOptions(Guid pedidoItemGuid)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "SELECT * FROM pedidoItemOpcoes " +
                    "WHERE pedidoItemGuid = @pedidoItemGuid";
                conn.Open();
                return conn.Query<PedidoItemOpcao>(sql, new { pedidoItemGuid });
            }
        }

        public IEnumerable<PedidoMeioMeio> GetOrderItemHalfs(Guid pedidoItemGuid)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "SELECT * FROM pedidoMeioMeios " +
                    "WHERE pedidoItemGuid = @pedidoItemGuid";
                conn.Open();
                return conn.Query<PedidoMeioMeio>(sql, new { pedidoItemGuid });
            }
        }
    }
}
