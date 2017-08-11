namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Runtime.CompilerServices;

    public class RequestScreenshotsEventArgs : EventArgs
    {
        public RequestScreenshotsEventArgs(IGameFileDataSource gameFile)
        {
            this.GameFile = gameFile;
        }

        public IGameFileDataSource GameFile { get; private set; }
    }
}

