using Newtonsoft.Json;
using Apollos.OneSignal.RestAPIv3.Client.Resources;
using Apollos.OneSignal.RestAPIv3.Client.Resources.Notifications;
using System;
using System.Collections.Generic;

namespace Apollos.OneSignal.RestAPIv3.Client.Sample.ConsoleApp.NetFramework452
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new OneSignalClient(""); // Use your Api Key

            var options = new NotificationCreateOptions
            {
                AppId = new Guid(""),   // Use your AppId
                IncludePlayerIds = new List<string>()
                {
                    "00000000-0000-0000-0000-000000000000" // Use your playerId
                },
                // ... OR ...
                IncludeExternalUserIds = new List<string>()
                {
                  "000000" // whatever your custom id is
                }
            };
            options.Headings.Add(LanguageCodes.English, "New Notification!");
            options.Contents.Add(LanguageCodes.English, "This will push a real notification directly to your device.");

            var result = client.Notifications.Create(options);

            Console.WriteLine(JsonConvert.SerializeObject(result));
            Console.ReadLine();
        }
    }
}
