namespace DoomLauncher.DataSources
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class WadArchiveGameFileDataSource : GameFileDataSource
    {
        public override string FileName
        {
            get
            {
                if ((this.filenames != null) && (this.filenames.Length != 0))
                {
                    return this.filenames[0];
                }
                return null;
            }
            set
            {
                if (this.filenames == null)
                {
                    this.filenames = new string[1];
                }
                this.filenames[0] = value;
            }
        }

        [JsonProperty("filenames")]
        public string[] filenames { get; set; }

        [JsonProperty("size")]
        public override int FileSizeBytes { get; set; }

        [JsonProperty("links")]
        public string[] links { get; set; }

        [JsonProperty("port")]
        public string port { get; set; }

        [JsonProperty("screenshots")]
        public Dictionary<string, string> screenshots { get; set; }

        [JsonProperty("extra")]
        public override string Title { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }
}

