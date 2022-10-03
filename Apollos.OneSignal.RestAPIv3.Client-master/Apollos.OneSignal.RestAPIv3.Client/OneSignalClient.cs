using Apollos.OneSignal.RestAPIv3.Client.Resources.Devices;
using Apollos.OneSignal.RestAPIv3.Client.Resources.Notifications;

namespace Apollos.OneSignal.RestAPIv3.Client
{
    /// <summary>
    /// OneSignal client 
    /// </summary>
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
