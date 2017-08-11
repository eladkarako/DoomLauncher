namespace DoomLauncher
{
    using System;
    using System.Runtime.CompilerServices;

    internal class SyncFileData
    {
        public SyncFileData(string filename)
        {
            this.FileName = filename;
        }

        public string FileName { get; set; }

        public bool Selected { get; set; }
    }
}

