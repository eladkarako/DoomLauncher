namespace DoomLauncher.Interfaces
{
    using System;

    public interface IConfigurationDataSource
    {
        string AvailableValues { get; set; }

        int ConfigID { get; set; }

        string Name { get; set; }

        bool UserCanModify { get; set; }

        string Value { get; set; }
    }
}

