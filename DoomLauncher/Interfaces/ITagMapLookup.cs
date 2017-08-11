namespace DoomLauncher.Interfaces
{
    using System;

    public interface ITagMapLookup
    {
        ITagDataSource[] GetTags(IGameFileDataSource gameFile);
        void Refresh();
    }
}

