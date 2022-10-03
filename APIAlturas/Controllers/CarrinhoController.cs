using System;
using System.Linq;
using System.Threading.Tasks;
using APIAlturas.ExtensionLogger;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CarrinhoController : Controller
    {
        private readonly PedidoDAO _pedidoDao;
        private readonly RepositorioLogger _logger;
        private readonly ProdutoDao _produtoDao;
        private readonly PedidoUserZimmerDao _pedidoUserZimmerDao;

        public CarrinhoController(PedidoDAO pedidoDao, RepositorioLogger logger, ProdutoDao produtoDao, PedidoUserZimmerDao pedidoUserZimmerDao)
        {
            _pedidoDao = pedidoDao;
            _logger = logger;
            _produtoDao = produtoDao;
            _pedidoUserZimmerDao = pedidoUserZimmerDao;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarrinho([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("1");
            }

            var carrinho = new Carrinho();
            return Ok(carrinho);
        }

        [HttpPost]
        public async Task<IActionResult> PostCarrinho([FromBody] Carrinho carrinho)
        {
            try
            {
                if (carrinho.Restaurante.ValorEntrega > 0 && carrinho.IsEntrega && carrinho.CartTotal.TaxTotal == 0)
                    return BadRequest("Pedido não pode ser finalizado, ocorreu um erro com a versão do APP.\nÉ necessário atualizar o APP.");

                //Confirma se produto trabalha com estoque e se tem a quantidade disponivel
                if (carrinho.CartItens.Any(t => t.Produto.IsControlstock))
                {
                    foreach (var carrinhoCartIten in carrinho.CartItens)
                    {
                        if(!carrinhoCartIten.Produto.IsControlstock)continue;


                        var produto = _produtoDao.GetProdutoPorId(carrinhoCartIten.Produto.ProdutoId);

                        if(produto.Stock <= 0)
                            return BadRequest($"Infelizmente o produto {produto.Nome} nao esta disponivel.");

                        if(produto.Stock < carrinhoCartIten.Quantidade)
                            return BadRequest($"Infelizmente temos apenas ({produto.Stock}) em estoque do produto {produto.Nome}.");

                    }
                }



                var id = _pedidoDao.InsertToCart(carrinho);
                var pedido = _pedidoDao.GetPedidoById(carrinho.Restaurante.Token, id.ToString());

                try
                {
                    if (carrinho.UserZimmer != null)
                    {
                        _pedidoUserZimmerDao.Insert(carrinho.UserZimmer, id);

                        var smsService = new TwilioService();
                        var msg =
                            //$"{carrinho.Restaurante.Nome} o pedido {pedido.PedidoId} esta em preparo.\nSua senha é {pedido.PedidoId}\nAcompanhe seu pedido: http://www.zipsoftware2.ddns.com.br:8991/pedido/{pedido.PedidoGuid}";
                            $"{carrinho.Restaurante.Nome} o pedido {pedido.PedidoId} esta em preparo.\nSua senha é {pedido.PedidoId}.";
                        var fone = carrinho.UserZimmer.Fone;

                        smsService.EnviarSms(msg, fone);


                    }
                }
                catch (Exception e)
                {
                    _logger.InsertLog(new LogEvento("Carrinho SMS", e.Message));
                }



                return CreatedAtAction("GetCarrinho",
                    id,
                    pedido
                    );
            }
            catch (Exception e)
            {
                _logger.InsertLog(new LogEvento("Carrinho", e.Message));
                return BadRequest(e.Message);
            }
        }

    }
}