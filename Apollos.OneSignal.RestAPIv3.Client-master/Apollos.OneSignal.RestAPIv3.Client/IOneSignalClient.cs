using Apollos.OneSignal.RestAPIv3.Client.Resources.Notifications;
using Apollos.OneSignal.RestAPIv3.Client.Resources.Devices;

namespace Apollos.OneSignal.RestAPIv3.Client
{
    /// <summary>
    /// OneSignal client interface.
    /// </summary>
    public interface IOneSignalClient
    {
        /// <summary>
        /// Device resources.
        /// </summary>
        IDevicesResource Devices { get; }

        /// <summary>
        /// Notification resources.
        /// </summary>
        INotificationsResource Notifications { get; }
    }
}
