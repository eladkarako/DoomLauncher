namespace DoomLauncher
{
    using System;
    using System.Runtime.CompilerServices;

    internal class InvalidFile
    {
        public InvalidFile(string filename, string reason)
        {
            this.FileName = filename;
            this.Reason = reason;
        }

        public string FileName { get; set; }

        public string Reason { get; set; }
    }
}

