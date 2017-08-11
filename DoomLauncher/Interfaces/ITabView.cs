namespace DoomLauncher.Interfaces
{
    using DoomLauncher;
    using System;
    using System.Collections.Generic;

    public interface ITabView : ICloneable
    {
        void SetGameFiles();
        void SetGameFiles(IEnumerable<GameFileSearchField> searchFields);
        void SetGameFilesData(IEnumerable<IGameFileDataSource> gameFiles);
        void UpdateDataSourceFile(IGameFileDataSource ds);

        IGameFileDataSourceAdapter Adapter { get; set; }

        DoomLauncher.GameFileViewControl GameFileViewControl { get; }

        bool IsAutoSearchAllowed { get; }

        bool IsDeleteAllowed { get; }

        bool IsEditAllowed { get; }

        bool IsLocal { get; }

        bool IsPlayAllowed { get; }

        bool IsSearchAllowed { get; }

        object Key { get; }

        string Title { get; }
    }
}

