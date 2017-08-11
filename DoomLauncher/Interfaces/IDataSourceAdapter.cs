namespace DoomLauncher.Interfaces
{
    using DoomLauncher;
    using System;
    using System.Collections.Generic;

    public interface IDataSourceAdapter : IGameFileDataSourceAdapter, IIWadDataSourceAdapter, IConfigurationDataSourceAdapter, IStatsDataSourceAdapter
    {
        void DeleteFile(IFileDataSource ds);
        void DeleteFile(IGameFileDataSource ds);
        void DeleteSourcePort(ISourcePortDataSource ds);
        IEnumerable<IFileDataSource> GetFiles(IGameFileDataSource gameFile);
        IEnumerable<IFileDataSource> GetFiles(IGameFileDataSource gameFile, FileType fileTypeID);
        IEnumerable<IGameFileDataSource> GetGameFiles(ITagDataSource tag);
        IEnumerable<IGameFileDataSource> GetGameFiles(IGameFileGetOptions options, ITagDataSource tag);
        ISourcePortDataSource GetSourcePort(int sourcePortID);
        IEnumerable<ISourcePortDataSource> GetSourcePorts();
        void InsertFile(IFileDataSource ds);
        void InsertSourcePort(ISourcePortDataSource ds);
        void UpdateFile(IFileDataSource ds);
        void UpdateFiles(int sourcePortID_Where, int? sourcePortID_Set);
        void UpdateGameFiles(GameFileFieldType ftWhere, GameFileFieldType ftSet, object fWhere, object fSet);
        void UpdateSourcePort(ISourcePortDataSource ds);
    }
}

