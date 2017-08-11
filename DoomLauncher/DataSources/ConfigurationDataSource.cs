namespace DoomLauncher.DataSources
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Runtime.CompilerServices;

    internal class ConfigurationDataSource : IConfigurationDataSource
    {
        public ConfigurationDataSource()
        {
            this.AvailableValues = string.Empty;
            this.Value = string.Empty;
        }

        public string AvailableValues { get; set; }

        public int ConfigID { get; set; }

        public string Name { get; set; }

        public bool UserCanModify { get; set; }

        public string Value { get; set; }
    }
}

