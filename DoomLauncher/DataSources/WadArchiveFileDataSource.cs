namespace DoomLauncher.DataSources
{
    using DoomLauncher;
    using System;

    internal class WadArchiveFileDataSource : FileDataSource
    {
        public override bool IsUrl =>
            true;
    }
}

