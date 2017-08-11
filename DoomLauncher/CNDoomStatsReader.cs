namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;

    internal class CNDoomStatsReader : StatFileScanner, IStatisticsReader
    {
        private IGameFileDataSource m_gameFile;
        private List<IStatsDataSource> m_statistics;
        private static StatFileScanner.ParseItem[] s_regexItems = new StatFileScanner.ParseItem[] { new StatFileScanner.ParseItem(@"\w+", string.Empty, "MapName"), new StatFileScanner.ParseItem(@"\d+:\d+.\d+", string.Empty, "LevelTime"), new StatFileScanner.ParseItem(@":\d+", ":", "KillCount"), new StatFileScanner.ParseItem(@"/\d+", "/", "TotalKills"), new StatFileScanner.ParseItem(@":\d+", ":", "ItemCount"), new StatFileScanner.ParseItem(@"/\d+", "/", "TotalItems"), new StatFileScanner.ParseItem(@":\d+", ":", "SecretCount"), new StatFileScanner.ParseItem(@"/\d+", "/", "TotalSecrets") };

        [field: CompilerGenerated]
        public event NewStatisticsEventHandler NewStastics;

        public CNDoomStatsReader(IGameFileDataSource gameFile, string stdoutFile) : base(stdoutFile)
        {
            this.m_statistics = new List<IStatsDataSource>();
            this.m_gameFile = gameFile;
        }

        public static CNDoomStatsReader CreateDefault(IGameFileDataSource gameFile, string directory) => 
            new CNDoomStatsReader(gameFile, Path.Combine(directory, "stdout.txt"));

        private IStatsDataSource ParseLine(string line)
        {
            StatsDataSource stats = new StatsDataSource();
            foreach (StatFileScanner.ParseItem item in s_regexItems)
            {
                Match match = Regex.Match(line, item.RegexInput);
                if (match.Success)
                {
                    line = ReplaceFirst(line, match.Value, string.Empty);
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
                string format = @"#+\w+#+Time:\d+:\d+.\d+Kills:\d+/\d+#+Items:\d+/\d+Secrets:\d+/\d+";
                MatchCollection matchs = Regex.Matches(File.ReadAllText(base.StatFile).Replace(" ", string.Empty).Replace(Environment.NewLine, string.Empty), string.Format(format, new object[0]), RegexOptions.Singleline);
                foreach (Match match in matchs)
                {
                    IStatsDataSource item = null;
                    if (match.Success)
                    {
                        item = this.ParseLine(match.Value);
                    }
                    if (item != null)
                    {
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
                if (matchs.Count == 0)
                {
                    base.m_errors.Add($"The file {base.StatFile} did not contain any statistics information.");
                }
            }
            catch (FileNotFoundException exception)
            {
                base.m_errors.Add($"The file {exception.FileName} was not found and could not be parsed.");
            }
            catch (Exception exception2)
            {
                base.m_errors.Add($"Unexpected exception: {exception2.Message}{Environment.NewLine}{exception2.StackTrace}");
            }
        }

        private static string ReplaceFirst(string text, string oldValue, string newValue)
        {
            int index = text.IndexOf(oldValue);
            if (index != -1)
            {
                return (text.Substring(0, index) + text.Substring(index + oldValue.Length));
            }
            return text;
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public static bool Supported(ISourcePortDataSource sourcePort) => 
            sourcePort.Executable.ToLower().Contains("cndoom.exe");

        public void Test()
        {
            this.ReadStatistics();
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
            "-printstats";

        public bool ReadOnClose =>
            true;
    }
}

