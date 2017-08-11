namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;

    internal class BoomStatsReader : StatFileScanner, IStatisticsReader
    {
        private IGameFileDataSource m_gameFile;
        private List<IStatsDataSource> m_statistics;
        private static StatFileScanner.ParseItem[] s_regexItems = new StatFileScanner.ParseItem[] { new StatFileScanner.ParseItem(@"^\S+-", "-", "MapName"), new StatFileScanner.ParseItem(@"-\S+:\S+\(", "-(", "LevelTime"), new StatFileScanner.ParseItem(@"K:\d+/", "K:/", "KillCount"), new StatFileScanner.ParseItem(@"/\d+I:", "I:/", "TotalKills"), new StatFileScanner.ParseItem(@"I:\d+/", "I:/", "ItemCount"), new StatFileScanner.ParseItem(@"/\d+S:", "S:/", "TotalItems"), new StatFileScanner.ParseItem(@"S:\d+/", "S:/", "SecretCount"), new StatFileScanner.ParseItem(@"/\d+$", "/", "TotalSecrets") };

        [field: CompilerGenerated]
        public event NewStatisticsEventHandler NewStastics;

        public BoomStatsReader(IGameFileDataSource gameFile, string statFile) : base(statFile)
        {
            this.m_statistics = new List<IStatsDataSource>();
            this.m_gameFile = gameFile;
        }

        public static IStatisticsReader CreateDefault(IGameFileDataSource gameFile, string directory) => 
            new BoomStatsReader(gameFile, Path.Combine(directory, "levelstat.txt"));

        private IStatsDataSource ParseLine(string line)
        {
            line = line.Replace(" ", string.Empty);
            StatsDataSource stats = new StatsDataSource();
            foreach (StatFileScanner.ParseItem item in s_regexItems)
            {
                Match match = Regex.Match(line, item.RegexInput);
                if (match.Success)
                {
                    base.SetStatProperty(stats, item, match.Value);
                }
                else
                {
                    base.m_errors.Add($"Failed to parse {item.DataSourceProperty} from levelstat file.");
                }
            }
            return stats;
        }

        public void ReadNow()
        {
            this.ReadStatistics();
        }

        private void ReadStatistics()
        {
            try
            {
                foreach (string str in from line in File.ReadLines(base.StatFile)
                    where !string.IsNullOrEmpty(line)
                    select line)
                {
                    IStatsDataSource item = this.ParseLine(str.Trim());
                    item.RecordTime = DateTime.Now;
                    item.GameFileID = this.m_gameFile.GameFileID.Value;
                    if (!this.m_statistics.Contains(item))
                    {
                        this.m_statistics.Add(item);
                        if (this.NewStastics != null)
                        {
                            this.NewStastics(this, new NewStatisticsEventArgs(item));
                        }
                    }
                }
            }
            catch (FileNotFoundException exception)
            {
                base.m_errors.Add($"The file {exception.FileName} was not found and could not be parsed.");
            }
        }

        public void Start()
        {
            try
            {
                FileInfo info = new FileInfo(base.StatFile);
                if (info.Exists)
                {
                    info.Delete();
                }
            }
            catch
            {
            }
        }

        public void Stop()
        {
        }

        public static bool Supported(ISourcePortDataSource sourcePort)
        {
            string str = sourcePort.Executable.ToLower();
            if (!str.Contains("prboom"))
            {
                return str.Contains("glboom");
            }
            return true;
        }

        public string[] Errors =>
            base.m_errors.ToArray();

        public IGameFileDataSource GameFile
        {
            get => 
                this.m_gameFile;
            set
            {
                this.m_gameFile = value;
            }
        }

        public string LaunchParameter =>
            "-levelstat";

        public bool ReadOnClose =>
            true;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly BoomStatsReader.<>c <>9 = new BoomStatsReader.<>c();
            public static Func<string, bool> <>9__20_0;

            internal bool <ReadStatistics>b__20_0(string line) => 
                !string.IsNullOrEmpty(line);
        }
    }
}

