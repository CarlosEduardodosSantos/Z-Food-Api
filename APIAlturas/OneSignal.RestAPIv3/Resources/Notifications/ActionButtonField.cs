﻿using Newtonsoft.Json;

namespace APIAlturas.OneSignal.RestAPIv3.Resources.Notifications
{
    /// <summary>s
    /// Class used to describe action field.
    /// </summary>
    public class ActionButtonField
    {
        /// <summary>
        /// Action button ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Action button text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Action button icon.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}