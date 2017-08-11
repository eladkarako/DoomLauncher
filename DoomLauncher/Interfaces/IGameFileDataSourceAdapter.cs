namespace DoomLauncher.Interfaces
{
    using DoomLauncher;
    using System;
    using System.Collections.Generic;

    public interface IGameFileDataSourceAdapter
    {
        void DeleteGameFile(IGameFileDataSource ds);
        IGameFileDataSource GetGameFile(string fileName);
        IEnumerable<IGameFileDataSource> GetGameFileIWads();
        IEnumerable<string> GetGameFileNames();
        IEnumerable<IGameFileDataSource> GetGameFiles();
        IEnumerable<IGameFileDataSource> GetGameFiles(IGameFileGetOptions options);
        int GetGameFilesCount();
        void InsertGameFile(IGameFileDataSource ds);
        void UpdateGameFile(IGameFileDataSource ds);
        void UpdateGameFile(IGameFileDataSource ds, GameFileFieldType[] updateFields);
    }
}

