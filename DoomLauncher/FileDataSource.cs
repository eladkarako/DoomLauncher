namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Runtime.CompilerServices;

    internal class FileDataSource : IFileDataSource
    {
        public FileDataSource()
        {
            this.FileOrder = 0x7fffffff;
            this.DateCreated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        public DateTime DateCreated { get; set; }

        public string Description { get; set; }

        public int? FileID { get; set; }

        public string FileName { get; set; }

        public int FileOrder { get; set; }

        public int FileTypeID { get; set; }

        public int GameFileID { get; set; }

        public virtual bool IsUrl =>
            false;

        public string OriginalFileName { get; set; }

        public string OriginalFilePath { get; set; }

        public int SourcePortID { get; set; }
    }
}

