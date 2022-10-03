using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace APIAlturas
{
    public class ProdutoDao
    {
        private readonly IConfiguration _configuration;

        public ProdutoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Produto GetProdutoPorId(int produtoId)
        {

            var sql = "Select * from Produtos Where produtoId = @produtoId";

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var produto = conn.Query<Produto>(sql, new { produtoId }).FirstOrDefault();
                conn.Close();

                return produto;
            }
        }
        public Produto GetProdutoPorGuid(string id)
        {

            var sql = "Select * from Produtos Where ProdutoGuid = @ProdutoGuid";

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var produto = conn.Query<Produto>(sql, new { ProdutoGuid = id }).FirstOrDefault();
                conn.Close();

                return produto;
            }
        }

        public IEnumerable<ProdutosGrupo> GetByCategorias(string[] categoriaIds, string restauranteId)
        {
            var produtosCategorias = new List<ProdutosGrupo>();

            var sqlCategoria = "Select * from Categorias Where restauranteId = @restauranteId  And CategoriaId in @categoriaIds And Situacao = 1 Order By Sequencia";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var categorias = conexao.Query<Categoria>(sqlCategoria, new { categoriaIds, restauranteId });

                foreach (var categoria in categorias)
                {
                    var produtoCategoria = new ProdutosGrupo();
                    var sqlProdutos =
                        "Select * from Produtos Where Situacao = 1 And restauranteId = @restauranteId And categoriaId = @categoriaId  " +
                        "And ((Isnull(IsControlstock, 0) = 1 and stock > 0) Or (Isnull(IsControlstock,0) = 0)) Order By Sequencia";

                    var produtos = conexao.Query<Produto>(sqlProdutos, new { restauranteId, categoriaId = categoria.CategoriaId });

                    var sqlComplemento = "Select * From Complementos Where categoriaId = @categoriaId";

                    var complemento = conexao.Query<Complemento>(sqlComplemento, new { categoriaId = categoria.CategoriaId });


                    produtoCategoria.categoria = categoria;
                    produtoCategoria.complemento = complemento.ToList();
                    produtoCategoria.produtos = produtos.ToList();

                    produtosCategorias.Add(produtoCategoria);
                }
            }

            return produtosCategorias;
        }

        public ProdutosGrupo GetByCategoriaId(string restauranteId, string categoriaId, int limit, int page)
        {
            var sqlCategoria = "Select * from Categorias Where restauranteId = @restauranteId  And CategoriaId = @categoriaId And Situacao = 1 Order By Sequencia";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var categoria = conexao.Query<Categoria>(sqlCategoria, new { categoriaId, restauranteId }).FirstOrDefault();

                var produtoCategoria = new ProdutosGrupo();
                var sqlProdutos =
                    "Select * from Produtos Where Situacao = 1 And restauranteId = @restauranteId And categoriaId = @categoriaId  " +
                    "And ((Isnull(IsControlstock, 0) = 1 and stock > 0) Or (Isnull(IsControlstock,0) = 0)) Order By Sequencia";

                var produtos = conexao.Query<Produto>(sqlProdutos, new { restauranteId, categoriaId = categoriaId });

                var sqlComplemento = "Select * From Complementos Where categoriaId = @categoriaId";


                produtoCategoria.categoria = categoria;
                produtoCategoria.produtos = produtos.Skip((page - 1) * limit).Take(limit).ToList();

                return produtoCategoria;
            }
        }

        public IEnumerable<ProdutosGrupo> GetProdutos(string restauranteId)
        {
            var produtosCategorias = new List<ProdutosGrupo>();

            var sqlCategoria = "Select * from Categorias Where restauranteId = @restauranteId  And CategoriaId in (select categoriaId From Produtos Where Situacao = 1) And Situacao = 1 Order By Sequencia";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var categorias = conexao.Query<Categoria>(sqlCategoria, new { restauranteId });

                foreach (var categoria in categorias)
                {
                    var produtoCategoria = new ProdutosGrupo();
                    var sqlProdutos =
                        "Select *, " +
                            "(SELECT COUNT(produtoId) FROM produtosOpcoes AS o WHERE o.produtoId = produtos.produtoId) AS optionsCount " +
                        " from Produtos Where Situacao = 1 And restauranteId = @restauranteId And categoriaId = @categoriaId  " +
                        "And ((Isnull(IsControlstock, 0) = 1 and stock > 0) Or (Isnull(IsControlstock,0) = 0)) Order By Sequencia";

                    var produtos = conexao.Query<Produto>(sqlProdutos, new { restauranteId, categoriaId = categoria.CategoriaId });

                    var sqlComplemento = "Select * From Complementos Where categoriaId = @categoriaId";

                    var complemento = conexao.Query<Complemento>(sqlComplemento, new { categoriaId = categoria.CategoriaId });


                    produtoCategoria.categoria = categoria;
                    produtoCategoria.complemento = complemento.ToList();
                    produtoCategoria.produtos = produtos.ToList();

                    produtosCategorias.Add(produtoCategoria);
                }
            }

            return produtosCategorias;
        }

        public IEnumerable<Produto> GetProdutoByCard(string restauranteId)
        {
            var sql = "Select Produtos.*, Categorias.descricao as categoriaNome from Produtos" +
                     " left Join Categorias On Produtos.categoriaId = Categorias.categoriaId" +
                     " Where Produtos.Situacao In (1,2) And Produtos.restauranteId = @restauranteId And Isnull(IsCard,0) = 1";

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var produtos = conn.Query<Produto>(sql, new { restauranteId });
                conn.Close();

                return produtos;
            }
        }
        public IEnumerable<Produto> GetProdutosCadastro(string token)
        {

            var sql = "Select Produtos.*, Categorias.descricao as categoriaNome from Produtos" +
                      " left Join Categorias On Produtos.categoriaId = Categorias.categoriaId" +
                      " Where Produtos.Situacao In (1,2) And Produtos.restauranteId in (Select restauranteId From Restaurantes Where token = @token)";

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var produtos = conn.Query<Produto>(sql, new { token });
                conn.Close();

                return produtos;
            }
        }

        public IEnumerable<Produto> GetByName(string name)
        {
            var nameLike = "%" + name + "%";
            var sql = "Select Produtos.*, Categorias.descricao as categoriaNome from Produtos" +
                      "left Join Categorias On Produtos.categoriaId = Categorias.categoriaId" +
                      "Where Produtos.Situacao In (1,2) And Produtos.nome like @name";

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var produtos = conn.Query<Produto>(sql, new { name = nameLike });
                conn.Close();

                return produtos;
            }
        }


        public IEnumerable<Produto> GetByInsert(ProdutoInsertViewModel produtoInsert)
        {

            var sql = "Select * from Produtos " +
                      "Where restauranteId in (Select restauranteId From Restaurantes Where token = @token) " +
                      "And (referenciaId = @referenciaId Or Upper(nome) like Upper(@nome) )";

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var produtos = conn.Query<Produto>(sql, new
                {
                    produtoInsert.Token,
                    produtoInsert.ReferenciaId,
                    produtoInsert.Nome
                });
                conn.Close();

                return produtos;
            }
        }

        public ValidationResult Insert(ProdutoInsertViewModel produtoInsertView)
        {
            if (!produtoInsertView.IsValid())
                return produtoInsertView.ResultadoValidacao;

            var sql = new StringBuilder();
            sql.AppendLine("Insert Into Produtos (referenciaId,restauranteId,nome,valorVenda,valorRegular,");
            sql.AppendLine("valorPromocao,imagem,avaliacaoRating,categoriaId,descricao,numberMeiomeiom,isPartMeioMeio,");
            sql.AppendLine("IsControlstock, Stock)");
            sql.AppendLine("Values (@referenciaId,@restauranteId,@nome,@valorVenda,@valorRegular,");
            sql.AppendLine("@valorPromocao,@imagem,@avaliacaoRating,@categoriaId,@descricao,@numberMeiomeiom,@isPartMeioMeio,");
            sql.AppendLine("@IsControlstock, @Stock)");

            var parans = new DynamicParameters();
            parans.Add("@referenciaId", produtoInsertView.ReferenciaId);
            parans.Add("@restauranteId", produtoInsertView.RestauranteId);
            parans.Add("@nome", produtoInsertView.Nome);
            parans.Add("@valorVenda", produtoInsertView.ValorVenda);
            parans.Add("@valorRegular", produtoInsertView.ValorRegular);
            parans.Add("@valorPromocao", produtoInsertView.ValorPromocao);
            parans.Add("@imagem", produtoInsertView.Imagem);
            parans.Add("@avaliacaoRating", 1);
            parans.Add("@categoriaId", produtoInsertView.CategoriaId);
            parans.Add("@descricao", produtoInsertView.Descricao);
            parans.Add("@numberMeiomeiom", produtoInsertView.NumberMeioMeio);
            parans.Add("@isPartMeioMeio", produtoInsertView.IsPartMeioMeio);
            parans.Add("@IsControlstock", produtoInsertView.IsControlstock);
            parans.Add("@Stock", produtoInsertView.Stock);

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), parans);
                conn.Close();

                return produtoInsertView.ResultadoValidacao;
            }

        }

        public IEnumerable<Complemento> GetComplementos(string categoriaId)
        {
            var sql = "Select * from Complementos Where categoriaId = @categoriaId";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.Query<Complemento>(sql, new { categoriaId });
            }
        }

        public IEnumerable<Produto> GetMeioMeios(string restauranteId, int produtoId)
        {
            var sql = "Select * from Produtos Where restauranteId = @restauranteId And isPartMeioMeio = 1 " +
                      "And TamanhoId = (Select Top 1 TamanhoId From Produtos Where ProdutoId = @produtoId)";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.Query<Produto>(sql, new { restauranteId, produtoId });
            }
        }
        public IEnumerable<Produto> GetPromocoes(string restauranteId)
        {
            var sql = "Select * from Produtos Where Situacao = 1 And restauranteId = @restauranteId And valorPromocao > 0";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.Query<Produto>(sql, new { restauranteId });
            }
        }

        public IEnumerable<Produto> GetDestaqueByCategorias(string[] categoriaIds, string restauranteId)
        {

            var sql = "Select * from Produtos Where Situacao = 1 And restauranteId = @restauranteId And valorPromocao > 0 And categoriaId In  @categoriaIds Order By Sequencia";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.Query<Produto>(sql, new { restauranteId, categoriaIds });
            }
        }
        public void AddPlayerProduto(PlayersClick playersClick)
        {
            using (var conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Insert Into PlayersProduto(PlayerProdutoId, playersID, DataHora, ProdutoId)");
                sql.AppendLine("Values (@PlayerProdutoId, @playersID, @DataHora, @ProdutoId)");
                var param = new DynamicParameters();
                param.Add("@PlayerProdutoId", Guid.NewGuid());
                param.Add("@playersID", playersClick.PlayersId);
                param.Add("@DataHora", DateTime.Now);
                param.Add("@ProdutoId", playersClick.ProdutoId);

                conexao.Query(sql.ToString(), param);
            }

        }

        public void Incluir(ProdutoAdicionarViewModel produto)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Insert Into Produtos(referenciaId, Situacao, restauranteId, nome, valorVenda, valorRegular, valorPromocao,");
            sql.AppendLine("imagem, avaliacaoRating, categoriaId, descricao, numberMeiomeio, isPartMeioMeio, TamanhoId, ProdutoGuid,");
            sql.AppendLine("IsControlstock, Stock)");
            sql.AppendLine("Values (@referenciaId, @Situacao, @restauranteId, @nome, @valorVenda, @valorRegular, @valorPromocao,");
            sql.AppendLine("@imagem, @avaliacaoRating, @categoriaId, @descricao, @numberMeiomeio, @isPartMeioMeio, @TamanhoId, @ProdutoGuid,");
            sql.AppendLine("@IsControlstock, @Stock)");

            var parms = new DynamicParameters();
            parms.Add("@referenciaId", produto.ReferenciaId);
            parms.Add("@restauranteId", produto.RestauranteId);
            parms.Add("@Situacao", produto.Situacao);
            parms.Add("@nome", produto.Nome);
            parms.Add("@valorVenda", produto.ValorVenda);
            parms.Add("@valorRegular", produto.ValorRegular);
            parms.Add("@valorPromocao", produto.ValorPromocao);
            parms.Add("@imagem", produto.Imagem);
            parms.Add("@avaliacaoRating", 5);
            parms.Add("@categoriaId", produto.CategoriaId);
            parms.Add("@descricao", produto.Descricao);
            parms.Add("@numberMeiomeio", produto.NumberMeiomeio);
            parms.Add("@isPartMeioMeio", produto.IsPartMeioMeio);
            parms.Add("@TamanhoId", produto.TamanhoId);
            parms.Add("@ProdutoGuid", produto.ProdutoGuid);
            parms.Add("@IsControlstock", produto.IsControlstock);
            parms.Add("@Stock", produto.Stock);

            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }

        }
        public void Alterar(ProdutoAdicionarViewModel produto)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update Produtos Set");
            sql.AppendLine("referenciaId = @referenciaId,");
            sql.AppendLine("Situacao = @Situacao,");
            sql.AppendLine("restauranteId = @restauranteId,");
            sql.AppendLine("nome = @nome,");
            sql.AppendLine("valorVenda = @valorVenda,");
            sql.AppendLine("valorRegular = @valorRegular,");
            sql.AppendLine("valorPromocao = @valorPromocao,");
            sql.AppendLine("imagem = @imagem,");
            sql.AppendLine("categoriaId = @categoriaId,");
            sql.AppendLine("descricao = @descricao,");
            sql.AppendLine("numberMeiomeio = @numberMeiomeio,");
            sql.AppendLine("isPartMeioMeio = @isPartMeioMeio,");
            sql.AppendLine("TamanhoId = @TamanhoId,");
            sql.AppendLine("ProdutoGuid = @ProdutoGuid,");
            sql.AppendLine("IsControlstock = @IsControlstock,");
            sql.AppendLine("Stock = @Stock");
            sql.AppendLine("Where ProdutoId = @produtoId");

            var parms = new DynamicParameters();
            parms.Add("@ProdutoId", produto.ProdutoId);
            parms.Add("@referenciaId", produto.ReferenciaId);
            parms.Add("@Situacao", produto.Situacao);
            parms.Add("@restauranteId", produto.RestauranteId);
            parms.Add("@nome", produto.Nome);
            parms.Add("@valorVenda", produto.ValorVenda);
            parms.Add("@valorRegular", produto.ValorRegular);
            parms.Add("@valorPromocao", produto.ValorPromocao);
            parms.Add("@imagem", produto.Imagem);
            parms.Add("@categoriaId", produto.CategoriaId);
            parms.Add("@descricao", produto.Descricao);
            parms.Add("@numberMeiomeio", produto.NumberMeiomeio);
            parms.Add("@isPartMeioMeio", produto.IsPartMeioMeio);
            parms.Add("@TamanhoId", produto.TamanhoId);
            parms.Add("@ProdutoGuid", produto.ProdutoGuid);
            parms.Add("@IsControlstock", produto.IsControlstock);
            parms.Add("@Stock", produto.Stock);

            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }
        }

        public void Excluir(int produtoId)
        {
            var sql = "Update Produtos Set Situacao = 3 Where produtoId = @produtoId";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conexao.Query(sql, new { produtoId });
            }
        }

        public bool ObterPorProdutoIdByRestauranteId(int referenciaId, int restauranteId)
        {
            var sql = "Select * From Produtos Where ReferenciaId = @referenciaId And RestauranteId = @restauranteId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var result = conn.Query<Produto>(sql, new
                {
                    referenciaId,
                    restauranteId
                }).Any();
                conn.Close();

                return result;
            }
        }

        /// <summary>
        /// Retorna os produtos de um restaurante filtrando pelo nome, descrição e tag
        /// </summary>
        public IEnumerable<GrupoProduto> FilterProds(string restauranteId, string filterString)
        {
            // Limpando campos
            Regex rx = new Regex("[^0-9a-zA-Z ]");
            filterString = filterString.Trim();
            filterString = rx.Replace(filterString, "");

            if (filterString.Length == 0) throw new Exception("400");

            Regex rxRestId = new Regex("[^0-9]");
            restauranteId = restauranteId.Trim();
            restauranteId = rxRestId.Replace(restauranteId, "");

            if (restauranteId.Length == 0) throw new Exception("400");

            string sql =
                "SELECT * FROM Grupos AS g " +
                "INNER JOIN Produtos AS p ON (g.GupoId IN (" +
                    "SELECT grupoId FROM GrupoCategoriaRelacao WHERE categoriaId = p.categoriaId)" +
                ") " +
                "WHERE p.restauranteId = @restauranteId " +
                "AND (" +
                    "(p.nome like '%' + @filterString + '%') " +
                    "OR (p.descricao like '%' + @filterString + '%') " +
                    "OR (p.produtoGuid IN (SELECT produtoGuid FROM tags WHERE tag Like '%' + @filterString + '%'))" +
                ")";

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("VipFood")))
            {
                var grupoProdutoDictionary = new Dictionary<Guid, GrupoProduto>();

                conn.Open();
                IEnumerable<GrupoProduto> grupos = conn.Query<GrupoProduto, Produto, GrupoProduto>(
                    sql,
                    (grupoProduto, produto) =>
                    {
                        GrupoProduto grupoProdutoEntry;

                        if(!grupoProdutoDictionary.TryGetValue(grupoProduto.GupoId, out grupoProdutoEntry))
                        {
                            grupoProdutoEntry = grupoProduto;
                            grupoProdutoEntry.Produtos = new List<Produto>();
                            grupoProdutoDictionary.Add(grupoProdutoEntry.GupoId, grupoProdutoEntry);
                        }

                        grupoProdutoEntry.Produtos.Add(produto);
                        return grupoProdutoEntry;
                    },
                    new { restauranteId, filterString },
                    splitOn: "produtoId"
                ).Distinct()
                .ToList();
                return grupos;
            }
        }

        public IEnumerable<Produto> getByCategoryId(int catId)
        {
            string sql =
                "SELECT * FROM produtos " +
                "WHERE categoriaId = @catId";

            using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                IEnumerable<Produto> products = conn.Query<Produto>(sql, new { catId });

                string sqlHasOptions =
                    "SELECT COUNT(ProdutosOpcaoId) FROM ProdutosOpcaoTipoRelacao " +
                    "WHERE produtoId = @prodId";
                foreach(Produto prod in products)
                {
                    int countOptions = conn.Query<int>(sqlHasOptions, new { prodId = prod.ProdutoId }).First();
                    prod.optionsCount = countOptions;
                }
                
                return products;
            }
        }

        public IEnumerable<Produto> getLazy(int restId, int quantity, int catId, string lastProdName)
        {

            using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                // IEnumerable<Produto> products = new List<Produto>();
                /*do
                {*/
                    string sql =
                      "SELECT TOP " + quantity + " *, " +
                        "(SELECT COUNT(o.produtoId) FROM produtosOpcoes AS o WHERE o.produtoId = p.produtoId) AS optionsCount " +
                      "FROM produtos AS p " +
                      "INNER JOIN categorias AS c ON (c.categoriaId = p.categoriaId) " +
                      "WHERE p.restauranteId = @restId " +
                      "AND p.categoriaId = @catId " +
                      (lastProdName == "*" ? "" : "AND p.nome > @lastProdName ") +
                      "ORDER BY nome";
                    IEnumerable<Produto> products = conn.Query<Produto>(sql, new { restId, catId, lastProdName });
                    /*string sqlHasOptions =
                        "SELECT COUNT(produtoId) FROM produtosOpcoes " +
                        "WHERE produtoId = @prodId";*/
                    /*foreach (Produto prod in products)
                    {
                        int countOptions = conn.Query<int>(sqlHasOptions, new { prodId = prod.ProdutoId }).First();
                        prod.HasOptions = countOptions > 0;
                    }*/

                //} while (products.Count() < 15);

                return products;
            }
        }
    }
}
