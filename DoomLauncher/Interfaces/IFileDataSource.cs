namespace DoomLauncher.Interfaces
{
    using System;

    public interface IFileDataSource
    {
        DateTime DateCreated { get; set; }

        string Description { get; set; }

        int? FileID { get; set; }

        string FileName { get; set; }

        int FileOrder { get; set; }

        int FileTypeID { get; set; }

        int GameFileID { get; set; }

        bool IsUrl { get; }

        string OriginalFileName { get; set; }

        string OriginalFilePath { get; set; }

        int SourcePortID { get; set; }
    }
}

