using APIAlturas.OneSignal.RestAPIv3.Resources.Devices;
using APIAlturas.OneSignal.RestAPIv3.Resources.Notifications;

namespace APIAlturas.OneSignal.RestAPIv3
{
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
