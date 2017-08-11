namespace DoomLauncher.DataSources
{
    using Newtonsoft.Json;
    using System;
    using System.Runtime.CompilerServices;

    internal class ScreenshotItem
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("value")]
        public string value { get; set; }
    }
}

