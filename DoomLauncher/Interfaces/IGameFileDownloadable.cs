namespace DoomLauncher.Interfaces
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.CompilerServices;

    internal interface IGameFileDownloadable
    {
        event AsyncCompletedEventHandler DownloadCompleted;

        event DownloadProgressChangedEventHandler DownloadProgressChanged;

        void Cancel();
        void Download(IGameFileDataSourceAdapter adapter, string filename);

        string FileName { get; set; }
    }
}

