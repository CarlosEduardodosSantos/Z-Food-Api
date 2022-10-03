using System;
using System.Collections.Generic;
using System.Linq;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{

    [Produces("application/json")]
    [Route("api/Pedido")]
    public class PedidoController : Controller
    {
        private readonly PedidoDAO _pedidosDao;
        //private readonly OneSignalService _oneSignalService;

        public PedidoController([FromServices]PedidoDAO pedidosDao)
        {
            _pedidosDao = pedidosDao;
        }

        [HttpGet("pendentes/{token}/{limit}/{page}")]
        public RootResult Get(Guid token, int limit, int page)
        {
            var data = _pedidosDao.GetPedidosByRestaurateId(token).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }
        [HttpGet("byId/{id}/{token}")]
        public object Get(string id, Guid token)
        {
            var data = _pedidosDao.GetPedidoById(token, id);

            return new 
            {
                id = data.PedidoId,
                shortReference = data.PedidoId,
                reference = data.PedidoGuid,
                createdAt = data.DataHora,
                scheduled = false,
                observations = data.Observacao,
                type = data.IsEntrega ? "DELIVERY" : "  RETIRA",
                customer = new
                {
                    id = data.User.UserID,
                    name = $"{data.User.Nome} {data.User.Sobrenome}",
                    phone = data.User.Fone,
                    email = data.User.Email,
                    taxPayerIdentificationNumber = data.Cpf
                },
                items = (from item in data.PedidoItens
                         select new
                         {
                             name = item.Produto.Nome,
                             tamanho = item.Produto.TamanhoId == 1 ? "Grande" : item.Produto.TamanhoId == 2 ? "Média" : "Pequeno",
                             quantity = Convert.ToInt32(item.Quantidade),
                             price = item.Valorprodutos,
                             totalPrice = item.Valorprodutos,
                             externalCode = item.Produto.ReferenciaId,
                             observations = item.Observacao,
                             subItems = (from subItem in item.PedidoComplementos
                                         select new
                                         {
                                             name = subItem.Descricao,
                                             quantity = 1,
                                             price = subItem.Valor,
                                             totalPrice = subItem.Valor
                                         }),
                             subOpcoes = (from subOpcao in item.PedidoItemOpcoes
                                 select new
                                 {
                                     name = subOpcao.Nome,
                                     tipo = subOpcao.Tipo,
                                     quantity = subOpcao.Quantidade,
                                     price = subOpcao.Valor,
                                     totalPrice = subOpcao.Valor
                                 }).OrderBy(o => o.tipo),
                             subMeioMeios = (from subMeioMeio in item.PedidoMeioMeios
                                 select new
                                 {
                                     produtoId = subMeioMeio.ProdutoId,
                                     name = subMeioMeio.Descricao,
                                     quantity = 1,
                                     price = subMeioMeio.Valor,
                                     totalPrice = subMeioMeio.Valor
                                 })
                         }),
                subTotal = data.Subtotal,
                totalPrice = data.Total,
                deliveryFee = data.EntregaTotal,
                deliveryAddress = !data.IsEntrega ? null : new
                {
                    formattedAddress = $"{data.PedidoEntrega.Logradouro}, {data.PedidoEntrega.Numero}",
                    state = data.PedidoEntrega.Uf,
                    city = data.PedidoEntrega.Localidade,
                    neighborhood = data.PedidoEntrega.Bairro,
                    postalCode = data.PedidoEntrega.Cep,
                    complement = data.PedidoEntrega.Complemento
                },
                deliveryDateTime = data.DataHora.AddHours(1),
                preparationStartDateTime = data.DataHora,
                payments = new List<object>
                {
                    new  {
                        name = data.FormaPagamento,
                        value = data.Total,
                        prepaid = data.IsPagamentoOnline,
                        changeFor = data.Troco
                        }
                }

            };

        }

        [HttpGet("polling/{token}")]
        public List<PollingModel> Polling(Guid token)
        {
            var pollings = _pedidosDao.ObterPollingByRestaurante(token).ToList();
            return pollings;
        }
        [HttpGet("meus/{token}/{limit}/{page}")]
        public RootResult GetUsuario(Guid token, int limit, int page)
        {
            var data = _pedidosDao.GetPedidosByUsuario(token).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }
        [HttpGet("obterStatus/{pedidoId}")]
        public object GetStatusByPedidoId(string pedidoId)
        {
            try
            {
                var data = _pedidosDao.ObterStatusById(pedidoId);

                return data;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPost("alterar")]
        public object AlterarSituacao([FromBody] PedidoModify pedidoModify,
            [FromServices]OneSignalService oneSignalService,
            [FromServices]UsersDAO usersDao)
        {
            try
            {
                /*
                _pedidosDao.Modificar(pedidoModify);

                //Enviar PushNotification
                if (pedidoModify.EnviaNotification)
                {
                    var usuario = usersDao.GetByPlayerId(pedidoModify.PlayersId);
                    oneSignalService.PushNotificationByPlayId(usuario, pedidoModify.Mensagem);
                }
                */
                return new
                {
                    errors = false,
                    message = "Pedido alterado com sucesso."
                };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("statuss/{rederence}/{status}/{token}")]
        public object AlterarStatuss(string rederence, string status, Guid token,
            [FromServices] OneSignalService oneSignalService,
            [FromServices] UsersDAO usersDao,
            [FromServices] RestauranteDAO restauranteDao)
        {
            try
            {
                var situacaoParceiro = (PedidoStatusParceiroEnumView)Enum.Parse(typeof(PedidoStatusParceiroEnumView), status, true);
                var situacao = (PedidoStatusEnumView) (int) situacaoParceiro;

                var restaurante = restauranteDao.FindByToken(token.ToString());
                var pedido = _pedidosDao.GetPedidoById(token, rederence);
                var usuario = usersDao.GetById(pedido.UsuarioGuid.ToString());

                _pedidosDao.AtrualizaStatus(situacao, rederence, token.ToString());

                if (situacao == PedidoStatusEnumView.CONFIRMED)
                {
                    oneSignalService.PushNotificationByPlayId(restaurante, usuario, "Seu pedido foi recebido com sucesso e já estamos preparando.");
                } else if (situacao == PedidoStatusEnumView.DISPATCHED)
                {
                    oneSignalService.PushNotificationByPlayId(restaurante, usuario, "Seu pedido já esta com nosso entregador. Logo chegará até você.");
                }
                else if (situacao == PedidoStatusEnumView.CANCELLED)
                {
                    oneSignalService.PushNotificationByPlayId(restaurante, usuario, "Seu pedido foi cancelado. Verifique com o restaurante.");
                }
                else if (situacao == PedidoStatusEnumView.READYTODELIVER)
                {
                    oneSignalService.PushNotificationByPlayId(restaurante, usuario, "Seu pedido foi ja esta pronto para retirada.");
                }

                return new
                {
                    errors = false,
                    message = "Pedido alterado com sucesso."
                };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ficha/{comandaId}")]
        public object getOrdersByCommand(string comandaId)
        {
            /*try
            {*/
                return _pedidosDao.getOrderByComandaId(comandaId);
            /*}
            catch (Exception e)
            {
                return BadRequest(e.Message);
            };*/
        }

        [HttpGet("history/{userId}/{limit}/{lastId}")]
        public ActionResult<Pedido[]> getOrderHistory(string userId, int limit, int lastId)
        {
            try
            {
                return Ok(this._pedidosDao.getOrderHistory(userId, limit, lastId));
            } catch(Exception err)
            {
                return StatusCode(500, err);
            }
        }

        [HttpGet("itensByMesa/{restauranteId}/{mesaId}")]
        public ActionResult<PedidoItem[]> getItensByMesa(int restauranteId, int mesaId)
        {
            try
            {
                return Ok(this._pedidosDao.GetPedidoItensByMesaId(restauranteId, mesaId));
            }
            catch (Exception err)
            {
                return StatusCode(500, err);
            }
        }
    }

}