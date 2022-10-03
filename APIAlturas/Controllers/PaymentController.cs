using System;
using System.Threading.Tasks;
using APIAlturas.Service.Payment;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private IPaymentBase _payment;
        private readonly PaymentAutenticacaoDao _paymentAutenticacao;
        private readonly PaymentRetornoDao _paymentRetornoDao;

        public PaymentController(PaymentAutenticacaoDao paymentAutenticacao, PaymentRetornoDao paymentRetornoDao)
        {
            _paymentAutenticacao = paymentAutenticacao;
            _paymentRetornoDao = paymentRetornoDao;
        }

        [HttpGet("doSession/{restauranteId}")]
        public object GetSession([FromServices]IHostingEnvironment hostingEnv, string restauranteId)
        {
            var paymentRestaurante = _paymentAutenticacao.ObterPorRestauranteId(restauranteId);

            if (paymentRestaurante == null)
                return new
                {
                    errors = true,
                    message = ""
                }; 

            _payment = new PagSeguroService(paymentRestaurante, hostingEnv);

            var resulta = _payment.GetSession();

            return new
            {
                errors = true,
                message = resulta
            };
        }
        [HttpPost("doPayment")]
        public object DoPayment([FromBody] PaymentModelView pagamento, [FromServices]IHostingEnvironment hostingEnv)
        {
            try
            {

                var paymentRestaurante = _paymentAutenticacao.ObterPorRestauranteId(pagamento.restauranteId);

                switch (paymentRestaurante.Operadora)
                {
                    case OperadoraPagamento.Rede:
                        _payment = new RedePagamentoService(paymentRestaurante);
                        break;
                    case OperadoraPagamento.PagSeguro:
                        _payment = new PagSeguroService(paymentRestaurante, hostingEnv);
                        break;
                    case OperadoraPagamento.Cielo:
                        _payment = new CieloPagamentoService();
                        break;
                }

                var resultConsulta = _payment.Pagar(pagamento);


                if (string.IsNullOrEmpty(resultConsulta.CodigoAutorizacao))
                {
                    return new
                    {
                        errors = true,
                        message = "Algo deu errado, tente novamente mais tarde!"
                    };

                }

                _paymentRetornoDao.Adicionar(resultConsulta);

                if (!resultConsulta.Pendente)
                {
                    return new
                    {
                        errors = !resultConsulta.Autorizado,
                        message = resultConsulta.Menssage,
                        nsu = resultConsulta.Nsu
                    };

                }
                else
                {
                    var resultNovaConsulta = new PaymentResult();
                    int maxTask = 0;
                    while (maxTask <= 3)
                    {
                        Task.Delay(2000).Wait(); // Wait 2 seconds with blocking

                        resultNovaConsulta = _payment.Consulta(resultConsulta);

                        if (!resultNovaConsulta.Pendente)
                        {
                            return new
                            {
                                errors = !resultNovaConsulta.Autorizado,
                                message = resultNovaConsulta.Menssage,
                                nsu = resultNovaConsulta.Nsu
                            };
                        }
                        maxTask++;
                    }

                    return new
                    {
                        errors = !resultNovaConsulta.Autorizado,
                        message = resultNovaConsulta.Menssage,
                        nsu = resultNovaConsulta.Nsu
                    };

                }

            }
            catch (Exception e)
            {
                return new
                {
                    errors = true,
                    message = e.Message,
                    nsu = ""
                };
            }




        }
    }
}