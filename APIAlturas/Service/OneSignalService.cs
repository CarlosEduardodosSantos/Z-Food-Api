using System;
using System.Collections.Generic;
using System.Linq;
using APIAlturas.OneSignal.RestAPIv3;
using APIAlturas.OneSignal.RestAPIv3.Resources;
using APIAlturas.OneSignal.RestAPIv3.Resources.Notifications;
using APIAlturas.ViewModels;
using Microsoft.Extensions.Configuration;


namespace APIAlturas.Service
{
    public class OneSignalService
    {
        private readonly IConfiguration _configuration;
        private readonly RestauranteDAO _restauranteDao;
        private readonly UsersDAO _users;

        public OneSignalService(IConfiguration configuration, UsersDAO users, RestauranteDAO restauranteDao)
        {
            _configuration = configuration;
            _users = users;
            _restauranteDao = restauranteDao;
        }

        public void PushNotificationByPlayId(Restaurante restaurante, User usuario, string mensage)
        {
            //var appId = _configuration.GetValue<string>("OneSignal:AppId");

            var client = new OneSignalClient(restaurante.OnSignalAppKey); // Use your Api Key
            var options = new NotificationCreateOptions
            {
                AppId = new Guid(restaurante.OneSignalAppId),   // Use your AppId
                IncludePlayerIds = new List<string>()
                {
                    usuario.PlayersId // Use your playerId
                }
            };


            if (string.IsNullOrEmpty(mensage))
                mensage = "Seu pedido foi recebido com sucesso. E assim que estiver tudo pronto, vamos te avisar!";

            options.Headings.Add(LanguageCodes.English, $"Olá {usuario.Nome}");
            options.Contents.Add(LanguageCodes.English, mensage);
            options.SmallAndroidIcon = "https://i.imgur.com/9QFB20F.png";

            client.Notifications.Create(options);
        }

        public NotificationCreateResult PushNotificationAll(PushNotificationModel pushNotificationModel)
        {
            var restaurante = _restauranteDao.FindByToken(pushNotificationModel.RestauranteToken);
            
            var includePlayerIds = new List<string>();
            var usres = _users.Find(restaurante.RestauranteId);
            foreach (var user in usres)
            {
                if (string.IsNullOrEmpty(user.PlayersId)) continue;


                if (includePlayerIds.All(t => t != user.PlayersId))
                    includePlayerIds.Add(user.PlayersId);
            }

            var client = new OneSignalClient(restaurante.OnSignalAppKey); // Use your Api Key
            var options = new NotificationCreateOptions
            {
                AppId = new Guid(restaurante.OneSignalAppId),   // Use your AppId
                IncludePlayerIds = includePlayerIds
            };
            options.Headings.Add(LanguageCodes.English, pushNotificationModel.Cabecalho);
            options.Contents.Add(LanguageCodes.English, pushNotificationModel.Mensagem);
            options.SmallAndroidIcon = "https://i.imgur.com/9QFB20F.png";

            var result =  client.Notifications.Create(options);

            return result;

        }
    }
}