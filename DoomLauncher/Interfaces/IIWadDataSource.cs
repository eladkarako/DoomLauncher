namespace DoomLauncher.Interfaces
{
    using System;

    public interface IIWadDataSource
    {
        string FileName { get; set; }

        int? GameFileID { get; set; }

        int IWadID { get; set; }

        string Name { get; set; }
    }
}

