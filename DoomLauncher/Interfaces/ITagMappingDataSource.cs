namespace DoomLauncher.Interfaces
{
    using System;

    public interface ITagMappingDataSource
    {
        int FileID { get; set; }

        int TagID { get; set; }
    }
}

