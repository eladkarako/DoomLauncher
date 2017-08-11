namespace DoomLauncher.DataSources
{
    using DoomLauncher;
    using DoomLauncher.Interfaces;
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class IdGamesGameFileDataSource : GameFileDataSource, IGameFileDownloadable, IDisposable
    {
        private WebClient m_webClient;

        [field: CompilerGenerated]
        public event AsyncCompletedEventHandler DownloadCompleted;

        [field: CompilerGenerated]
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;

        public void Cancel()
        {
            if (this.m_webClient != null)
            {
                this.m_webClient.CancelAsync();
            }
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.m_webClient.Dispose();
            this.m_webClient = null;
            if (this.DownloadCompleted != null)
            {
                this.DownloadCompleted(this, e);
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (this.DownloadProgressChanged != null)
            {
                this.DownloadProgressChanged(this, e);
            }
        }

        public void Dispose()
        {
            if (this.m_webClient != null)
            {
                this.m_webClient.Dispose();
            }
        }

        public void Download(IGameFileDataSourceAdapter adapter, string dlFilename)
        {
            IdGamesDataAdapater adapater = adapter as IdGamesDataAdapater;
            if (adapater != null)
            {
                this.m_webClient = new WebClient();
                this.m_webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.client_DownloadProgressChanged);
                this.m_webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.client_DownloadFileCompleted);
                this.m_webClient.DownloadFileAsync(new Uri(adapater.MirrorUrl + this.dir + this.filename), dlFilename, this.id);
            }
        }

        public override bool Equals(object obj) => 
            ((obj is IdGamesGameFileDataSource) && (this.id == ((IdGamesGameFileDataSource) obj).id));

        public override int GetHashCode() => 
            this.id;

        public string author { get; set; }

        public override string Author
        {
            get => 
                this.author;
            set
            {
                this.author = value;
            }
        }

        public DateTime? date { get; set; }

        public string description { get; set; }

        public override string Description
        {
            get => 
                this.description;
            set
            {
                this.description = value;
            }
        }

        public string dir { get; set; }

        public string filename { get; set; }

        public override int FileSizeBytes
        {
            get => 
                this.size;
            set
            {
                this.size = value;
            }
        }

        public int id { get; set; }

        public double rating { get; set; }

        public override double? Rating
        {
            get => 
                new double?(this.rating);
            set
            {
                if (value.HasValue)
                {
                    this.rating = value.Value;
                }
            }
        }

        public override DateTime? ReleaseDate
        {
            get => 
                this.date;
            set
            {
                if (value.HasValue)
                {
                    this.date = new DateTime?(value.Value);
                }
            }
        }

        public int size { get; set; }

        public string title { get; set; }

        public override string Title
        {
            get => 
                this.title;
            set
            {
                this.title = value;
            }
        }
    }
}

