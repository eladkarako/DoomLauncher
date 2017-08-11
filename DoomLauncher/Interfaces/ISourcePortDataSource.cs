namespace DoomLauncher.Interfaces
{
    using DoomLauncher;
    using System;

    public interface ISourcePortDataSource
    {
        string GetFullExecutablePath();

        LauncherPath Directory { get; set; }

        string Executable { get; set; }

        string Name { get; set; }

        string SettingsFiles { get; set; }

        int SourcePortID { get; set; }

        string SupportedExtensions { get; set; }
    }
}

