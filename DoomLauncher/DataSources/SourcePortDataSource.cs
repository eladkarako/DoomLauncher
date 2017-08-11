namespace DoomLauncher.DataSources
{
    using DoomLauncher;
    using DoomLauncher.Interfaces;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class SourcePortDataSource : ISourcePortDataSource
    {
        public override bool Equals(object obj)
        {
            ISourcePortDataSource source = obj as ISourcePortDataSource;
            return ((source != null) && (source.SourcePortID == this.SourcePortID));
        }

        public string GetFullExecutablePath() => 
            Path.Combine(this.Directory.GetFullPath(), this.Executable);

        public override int GetHashCode() => 
            this.SourcePortID;

        public LauncherPath Directory { get; set; }

        public string Executable { get; set; }

        public string Name { get; set; }

        public string SettingsFiles { get; set; }

        public int SourcePortID { get; set; }

        public string SupportedExtensions { get; set; }
    }
}

