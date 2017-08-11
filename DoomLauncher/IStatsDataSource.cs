namespace DoomLauncher
{
    using System;

    public interface IStatsDataSource
    {
        string FormattedItems { get; }

        string FormattedKills { get; }

        string FormattedSecrets { get; }

        string FormattedTime { get; }

        int GameFileID { get; set; }

        int ItemCount { get; set; }

        int KillCount { get; set; }

        float LevelTime { get; set; }

        string MapName { get; set; }

        DateTime RecordTime { get; set; }

        int SecretCount { get; set; }

        int SourcePortID { get; set; }

        int StatID { get; set; }

        int TotalItems { get; set; }

        int TotalKills { get; set; }

        int TotalSecrets { get; set; }
    }
}

