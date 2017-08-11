namespace DoomLauncher.Interfaces
{
    using DoomLauncher;
    using System;
    using System.Collections.Generic;

    public interface IStatsDataSourceAdapter
    {
        void DeleteStats(int statID);
        void DeleteStatsByFile(int gameFileID);
        IEnumerable<IStatsDataSource> GetStats();
        IEnumerable<IStatsDataSource> GetStats(int gameFileID);
        void InsertStats(IStatsDataSource ds);
    }
}

