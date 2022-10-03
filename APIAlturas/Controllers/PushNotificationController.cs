using System;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/Notificacao")]
    public class PushNotificationController : Controller
    {
        
        [HttpPost("enviar")]
        public object Enviar([FromBody]PushNotificationModel pushNotificationModel,
            [FromServices]OneSignalService oneSignalService,
            [FromServices] RestauranteDAO restauranteDao)
        {
            try
            {
                //var restaurante = restauranteDao.FindByToken(token.ToString());
                var result = oneSignalService.PushNotificationAll(pushNotificationModel);

                
                return new
                {
                    errors = false,
                    message = "Mensagem enviada com sucesso."
                };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public string Get()
        {
            var smsService = new TwilioService();
            var msg =
                $"Panificadora Paulista: Pedido 102030 esta em preparo.\nSua senha é 123\nAcompanhe seu pedido: http://www.zipsoftware2.ddns.com.br:8991/32759021000181/1314";

            var to = "16988351041";
            var result = smsService.EnviarSms(msg, to);

            return result;
        }
    }
}