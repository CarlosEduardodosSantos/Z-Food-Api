using System;
using System.Collections.Generic;
using System.Linq;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{

    [Produces("application/json")]
    [Route("api/v3/Pedido")]
    public class PedidoV3Controller : Controller
    {
        private readonly PedidoDAO _pedidosDao;
        //private readonly OneSignalService _oneSignalService;

        public PedidoV3Controller([FromServices] PedidoDAO pedidosDao)
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
            try
            {
                return new
                {
                    id = data.PedidoGuid,
                    shortReference = data.PedidoId,
                    displayId = data.PedidoId,
                    orderTiming = "IMMEDIATE",
                    reference = data.Referencia,
                    zReference = data.Referencia, // Reference da tabela de Pedidos do ViPFood. Específico da ZIP`, não vem no JSON do Ifood
                    createdAt = data.DataHora,
                    dataPedido = data.DataHora,
                    scheduled = false,
                    isSchedule = false,
                    observations = data.Observacao,
                    //ordertypes precisam ser atualizados e INDOOR precisa ser incluso
                    type = !string.IsNullOrEmpty(data.OrderType) ? data.OrderType : data.IsEntrega ? "DELIVERY" : "TAKEOUT",
                    orderType = !string.IsNullOrEmpty(data.OrderType) ? data.OrderType : data.IsEntrega ? "DELIVERY" : "TAKEOUT",

                    /*merchant = new
                    {
                        id = data.RestauranteId,
                        name = data.Restaurante.Nome,
                        adress = data.Restaurante.Logradouro,
                    },*/

                    customer = new
                    {
                        id = data.User.UserID,
                        name = $"{data.User.Nome} {data.User.Sobrenome}",
                        phone = new
                        {
                            number = data.User.Fone,
                            //localizer = 
                            //localizerExpiration = 
                        },
                        email = data.User.Email,
                        taxPayerIdentificationNumber = data.Cpf,
                        documentNumber = data.Cpf

                    },

                    // ordersCountOnMerchant =

                    items = (from item in data.PedidoItens
                             select new
                             {
                                 name = item.Produto.Nome,
                                 namePrincipal = item.Produto.Nome,
                                 tamanho = item.Produto.TamanhoId == 1 ? "Grande" : item.Produto.TamanhoId == 2 ? "Média" : "Pequeno",
                                 quantity = Convert.ToInt32(item.Quantidade),
                                 unitPriceCalc = item.ValorUnitario,
                                 unitPrice = item.ValorUnitario,
                                 optionsPrice = item.PedidoItemOpcoes.Sum(t => t.Valor),
                                 price = item.Valorprodutos,
                                 totalItem = item.TotalSubs,
                                 totalPrice = item.Valorprodutos,
                                 //addition = item.PedidoComplementos,
                                 discount = 0,

                                 options = (from Opcoes in item.PedidoItemOpcoes
                                            select new
                                            {
                                                id = Opcoes.PedidoItemOpcaoId,
                                                name = Opcoes.Nome,
                                                //unit =  
                                                quantity = Convert.ToInt32(Opcoes.Quantidade),
                                                unitPrice = Opcoes.Valor,
                                                //addition =
                                                //price = 
                                                //externalCode = 

                                            }),
                                 externalCode = item.Produto.ReferenciaId,
                                 externalCodeCalc = item.Produto.ReferenciaId,
                                 observations = item.Observacao,
                                 subItemsPrice = item.PedidoItemOpcoes.Sum(t => t.Valor),
                                 subItems = (from subOpcao in item.PedidoItemOpcoes.GroupBy(g => g)
                                             select new
                                             {
                                                 name = subOpcao.Key.Nome,
                                                 namePrincipal = subOpcao.Key.Nome,
                                                 tipo = subOpcao.Key.Tipo,
                                                 quantity = 1,
                                                 price = subOpcao.Key.Valor,
                                                 totalPrice = subOpcao.Key.Valor,
                                                 externalCode = subOpcao.Key.ProdutoPdv
                                             }).OrderBy(o => o.tipo),
                                 subMeioMeios = (from subMeioMeios in item.PedidoMeioMeios
                                                 select new
                                                 {
                                                     PedidoMeioMeioId = subMeioMeios.PedidoMeioMeioId,
                                                     PedidoGuid = subMeioMeios.PedidoGuid,
                                                     PedidoItemGuid = subMeioMeios.PedidoItemGuid,
                                                     produtoId = subMeioMeios.ProdutoId,
                                                     name = subMeioMeios.Descricao,
                                                     quantity = 1,
                                                     price = subMeioMeios.Valor,
                                                     totalPrice = subMeioMeios.Valor,
                                                     MeioMeioGuid = subMeioMeios.MeioMeioGuid,
                                                     complementosMeioMeio = (from complementosMeioMeio in subMeioMeios.PedidoMeioMeioComplementos
                                                                             select new
                                                                             {
                                                                                 PedidoMeioMeioComplementoID = complementosMeioMeio.PedidoMeioMeioComplementoID,
                                                                                 MeioMeioGuid = complementosMeioMeio.MeioMeioGuid,
                                                                                 Complemento = complementosMeioMeio.Complemento,
                                                                                 Valor = complementosMeioMeio.Valor,
                                                                                 Quantidade = complementosMeioMeio.Quantidade,
                                                                                 PedidoGuid = complementosMeioMeio.PedidoGuid
                                                                             })
                                                 })

                             }),
                    salesChannel = "IFOOD",
                    total = new
                    {
                        subTotal = data.Subtotal,
                        deliveryFee = data.EntregaTotal,
                        //benefits = ,
                        orderAmount = data.Total,
                        //additionalFees = 0,
                    },


                    valorTotal = data.Total,
                    valorServico = 0,
                    trocoPara = data.Troco,
                    subTotal = data.Subtotal,
                    totalPrice = data.Total,
                    deliveryFee = data.EntregaTotal,
                    deliveryAddress = !data.IsEntrega ? null : new
                    {
                        formattedAddress = $"{data.PedidoEntrega.Logradouro}, {data.PedidoEntrega.Numero}",
                        streetName = data.PedidoEntrega.Logradouro,
                        streetNumber = data.PedidoEntrega.Numero,
                        state = data.PedidoEntrega.Uf,
                        city = data.PedidoEntrega.Localidade,
                        neighborhood = data.PedidoEntrega.Bairro,
                        postalCode = data.PedidoEntrega.Cep,
                        complement = data.PedidoEntrega.Complemento
                    },

                    deliveryDateTime = data.DataHora.AddHours(1),
                    preparationStartDateTime = data.DataHora,
                    DataEntrega = data.DataHora.AddHours(1),
                    DataPreparação = data.DataHora,
                    //localizer = ,
                    preparationTimeInSeconds = 0,
                    //isTest = ,
                    tipoCupom = data.Coupom,
                    sitEvent = data.PedidoStatus,
                    //observations ja existe,
                    //aplicacao = ,
                    vendaId = data.PedidoId,
                    senhaId = data.ComandaId,
                    //extraInfo = ,
                    //qrcodePix = ,
                    //additionalFees = 0,

                    delivery = new
                    {
                        deliveryMode = "DEFAULT",
                        mode = "DEFAULT",
                        //deliveredBy = data.Restaurante.Nome,
                        deliveryDateTime = data.DataHora.AddHours(1),
                        deliveryAddress = !data.IsEntrega ? null : new
                        {
                            formattedAddress = $"{data.PedidoEntrega.Logradouro}, {data.PedidoEntrega.Numero}",
                            country = "BR",
                            state = data.PedidoEntrega.Uf,
                            city = data.PedidoEntrega.Localidade,
                            neighborhood = data.PedidoEntrega.Bairro,
                            postalCode = data.PedidoEntrega.Cep,
                            reference = data.PedidoEntrega.Complemento,
                            complement = data.PedidoEntrega.Complemento,
                            streetName = data.PedidoEntrega.Logradouro,
                            streetNumber = data.PedidoEntrega.Numero,
                        }
                    },

                    payments = new

                    {
                        name = data.FormaPagamento,
                        pending = 0,
                        formaPagamento = data.FormaPagamento,
                        value = data.TotalOnline > 0 ? data.TotalOnline : data.Total,
                        //prepaid = data.IsPagamentoOnline,
                        type = data.IsPagamentoOnline,
                        changeFor = data.Troco,
                        methods = new List<object>
                    {
                        new  {
                            name = data.FormaPagamento,
                            formaPagamento = data.FormaPagamento,
                            method = data.FormaPagamento, 
                            //cash = 
                            currency = "BRL",
                            value = data.TotalOnline > 0 ? data.TotalOnline : data.Total,
                            prepaid = data.IsPagamentoOnline,
                            type = data.IsPagamentoOnline,
                            changeFor = data.Troco
                        }

                    },
                        benefits = new List<object>
                    {
                        new
                        {
                            value = data.DescontoTotal,
                            sponsorshipValues = new {
                                IFOOD = 0,
                                MERCHANT = data.DescontoTotal
                            },
                            target = "CART"
                        }

                    },





                    },


                };
            }
            catch (Exception e)
            {
                return new
                {
                    sucess = false,
                    mensage = e.Message
                };

            }


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

        [HttpGet("getByEnactment/{userId}/{limit}/{page}")]
        public RootResult GetByEnactment(Guid userId, int limit, int page)
        {
            var data = _pedidosDao.GetByEnactment(userId).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }

        [HttpPost("alterar")]
        public object AlterarSituacao([FromBody] PedidoModify pedidoModify,
            [FromServices] OneSignalService oneSignalService,
            [FromServices] UsersDAO usersDao)
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
                var situacao = (PedidoStatusEnumView)(int)situacaoParceiro;

                var restaurante = restauranteDao.FindByToken(token.ToString());
                var pedido = _pedidosDao.GetPedidoById(token, rederence);
                var usuario = usersDao.GetById(pedido.UsuarioGuid.ToString());

                _pedidosDao.AtrualizaStatus(situacao, rederence, token.ToString());

                if (situacao == PedidoStatusEnumView.CONFIRMED)
                {
                    oneSignalService.PushNotificationByPlayId(restaurante, usuario, "Seu pedido foi recebido com sucesso e já estamos preparando.");
                }
                else if (situacao == PedidoStatusEnumView.DISPATCHED)
                {
                    oneSignalService.PushNotificationByPlayId(restaurante, usuario, "Seu pedido já esta com nosso entregador. Logo chegará até você.");
                }
                else if (situacao == PedidoStatusEnumView.CANCELLED)
                {
                    oneSignalService.PushNotificationByPlayId(restaurante, usuario, "Seu pedido foi cancelado. Verifique com o restaurante.");
                }
                else if (situacao == PedidoStatusEnumView.READYTODELIVER)
                {
                    oneSignalService.PushNotificationByPlayId(restaurante, usuario, "Seu pedido ja esta pronto para retirada.");
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

        [HttpGet("ByRestId/{restId}")]
        public IActionResult GetOrdersByRestId(int restId)
        {
            try
            {
                return Ok(_pedidosDao.GetOrdersByRestId(restId));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("OrderItens/{orderGuid}")]
        public IActionResult GetOrderItens(Guid orderGuid)
        {
            try
            {
                return Ok(_pedidosDao.GetOrderItens(orderGuid));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }

}