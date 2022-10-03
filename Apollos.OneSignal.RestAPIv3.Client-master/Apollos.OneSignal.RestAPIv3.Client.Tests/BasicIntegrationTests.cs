using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Apollos.OneSignal.RestAPIv3.Client.Resources.Notifications;
using Apollos.OneSignal.RestAPIv3.Client.Resources;

namespace Apollos.OneSignal.RestAPIv3.Client.Tests
{
    [TestClass]
    public class BasicIntegrationTests
    {
        [TestMethod]
        public void TestASimpleCall()
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
                    "000000" // Whatever your custom id is
                }
            };
            options.Headings.Add(LanguageCodes.English, "New Notification!");
            options.Contents.Add(LanguageCodes.English, "This will push a real notification directly to your device.");

            client.Notifications.Create(options);
        }
    }
}
