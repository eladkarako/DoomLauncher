namespace DoomLauncher
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    internal class StatsDataSource : IStatsDataSource
    {
        public override bool Equals(object obj)
        {
            IStatsDataSource source = obj as IStatsDataSource;
            if (source == null)
            {
                return false;
            }
            return (((((this.GameFileID == source.GameFileID) && (this.KillCount == source.KillCount)) && ((this.TotalKills == source.TotalKills) && (this.SecretCount == source.SecretCount))) && (((this.TotalSecrets == source.TotalSecrets) && (this.ItemCount == source.ItemCount)) && ((this.TotalItems == source.TotalItems) && (this.LevelTime == source.LevelTime)))) && (this.MapName.ToLower() == source.MapName.ToLower()));
        }

        public override int GetHashCode() => 
            $"{this.GameFileID},{this.KillCount},{this.TotalKills},{this.SecretCount},{this.TotalSecrets},{this.ItemCount},{this.TotalItems},{this.LevelTime},{this.MapName.ToLower()}".GetHashCode();

        public string FormattedItems =>
            $"{this.ItemCount.ToString("N0", CultureInfo.InvariantCulture)} / {this.TotalItems.ToString("N0", CultureInfo.InvariantCulture)}";

        public string FormattedKills =>
            $"{this.KillCount.ToString("N0", CultureInfo.InvariantCulture)} / {this.TotalKills.ToString("N0", CultureInfo.InvariantCulture)}";

        public string FormattedSecrets =>
            $"{this.SecretCount.ToString("N0", CultureInfo.InvariantCulture)} / {this.TotalSecrets.ToString("N0", CultureInfo.InvariantCulture)}";

        public string FormattedTime
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds((double) this.LevelTime);
                return $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}:{span.Milliseconds:D3}ms";
            }
        }

        public int GameFileID { get; set; }

        public int ItemCount { get; set; }

        public int KillCount { get; set; }

        public float LevelTime { get; set; }

        public string MapName { get; set; }

        public DateTime RecordTime { get; set; }

        public string SaveFile { get; set; }

        public int SecretCount { get; set; }

        public int SourcePortID { get; set; }

        public int StatID { get; set; }

        public int TotalItems { get; set; }

        public int TotalKills { get; set; }

        public int TotalSecrets { get; set; }
    }
}

