namespace DoomLauncher
{
    using System;
    using System.Runtime.CompilerServices;

    public class NewStatisticsEventArgs : EventArgs
    {
        public NewStatisticsEventArgs(IStatsDataSource stats)
        {
            this.Statistics = stats;
            this.Update = false;
        }

        public NewStatisticsEventArgs(IStatsDataSource stats, bool update)
        {
            this.Statistics = stats;
            this.Update = update;
        }

        public IStatsDataSource Statistics { get; set; }

        public bool Update { get; set; }
    }
}

