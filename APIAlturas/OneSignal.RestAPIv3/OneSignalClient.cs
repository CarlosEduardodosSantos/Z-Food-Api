using APIAlturas.OneSignal.RestAPIv3.Resources.Devices;
using APIAlturas.OneSignal.RestAPIv3.Resources.Notifications;


namespace APIAlturas.OneSignal.RestAPIv3
{
    public class OneSignalClient : IOneSignalClient
    {
        /// <summary>
        /// Device resources.
        /// </summary>
        public IDevicesResource Devices { get; private set; }

        /// <summary>
        /// Notification resources.
        /// </summary>
        public INotificationsResource Notifications { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="apiKey">Your OneSignal API key</param>
        /// <param name="apiUri">API uri (default is "https://onesignal.com/api/v1")</param>
        public OneSignalClient(string apiKey, string apiUri = "https://onesignal.com/api/v1")
        {
            this.Devices = new DevicesResource(apiKey, apiUri);
            this.Notifications = new NotificationsResource(apiKey, apiUri);
        }
    }
}