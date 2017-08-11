namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;

    internal class DownloadHandler
    {
        private List<IGameFileDownloadable> m_currentDownloads = new List<IGameFileDownloadable>();

        public DownloadHandler(LauncherPath downloadDirectory, DoomLauncher.DownloadView view)
        {
            this.DownloadDirectory = downloadDirectory;
            this.DownloadView = view;
            view.DownloadCancelled += new EventHandler(this.view_DownloadCancelled);
        }

        private void dlItem_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IGameFileDownloadable item = sender as IGameFileDownloadable;
            if ((this.DownloadView != null) && (item != null))
            {
                this.DownloadView.UpdateDownload(sender, $"{item.FileName} ({e.Cancelled ? "Cancelled" : "Complete"})");
                this.m_currentDownloads.Remove(item);
            }
        }

        private void dlItem_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            IGameFileDownloadable downloadable = sender as IGameFileDownloadable;
            if ((this.DownloadView != null) && (downloadable != null))
            {
                this.DownloadView.UpdateDownload(sender, e.ProgressPercentage);
                this.DownloadView.UpdateDownload(sender, $"{downloadable.FileName} - {Math.Round((double) ((((double) e.TotalBytesToReceive) / 1024.0) / 1024.0), 2)}MB");
            }
        }

        public void Download(IGameFileDataSourceAdapter adapter, IGameFileDownloadable dlItem)
        {
            if ((dlItem != null) && !this.IsDownloading(dlItem))
            {
                try
                {
                    this.m_currentDownloads.Add(dlItem);
                    dlItem.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.dlItem_DownloadProgressChanged);
                    dlItem.DownloadCompleted += new AsyncCompletedEventHandler(this.dlItem_DownloadCompleted);
                    if (this.DownloadView != null)
                    {
                        this.DownloadView.AddDownload(dlItem, dlItem.FileName);
                    }
                    dlItem.Download(adapter, Path.Combine(this.DownloadDirectory.GetFullPath(), dlItem.FileName));
                }
                catch
                {
                }
            }
        }

        public bool IsDownloading(IGameFileDownloadable dlItem) => 
            this.m_currentDownloads.Contains(dlItem);

        private void view_DownloadCancelled(object sender, EventArgs e)
        {
            object[] cancelledDownloads = this.DownloadView.CancelledDownloads;
            for (int i = 0; i < cancelledDownloads.Length; i++)
            {
                IGameFileDownloadable item = cancelledDownloads[i] as IGameFileDownloadable;
                if (item != null)
                {
                    item.Cancel();
                    this.m_currentDownloads.Remove(item);
                }
            }
        }

        public LauncherPath DownloadDirectory { get; set; }

        public DoomLauncher.DownloadView DownloadView { get; set; }
    }
}

