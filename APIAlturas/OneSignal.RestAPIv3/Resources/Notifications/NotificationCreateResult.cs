﻿using RestSharp.Deserializers;

namespace APIAlturas.OneSignal.RestAPIv3.Resources.Notifications
{
    /// <summary>
    /// Result of notification create operation.
    /// </summary>
    public class NotificationCreateResult
    {
        /// <summary>
        /// Returns the number of recepients who received the message.
        /// </summary>
        [DeserializeAs(Name = "recipients")]
        public int Recipients { get; set; }

        /// <summary>
        /// Returns the id of the result.
        /// </summary>
        [DeserializeAs(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Returns an error.
        /// </summary>
        [DeserializeAs( Name = "error" )]
        public string Error { get; set; }
    }
}
