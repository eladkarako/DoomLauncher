namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Timers;

    internal class ZDoomStatsReader : IStatisticsReader
    {
        private System.Timers.Timer m_checkTimer;
        private NewFileDetector m_detector;
        private string m_dir;
        private List<string> m_errors = new List<string>();
        private IGameFileDataSource m_gameFile;
        private List<IStatsDataSource> m_statistics = new List<IStatsDataSource>();
        private static int s_endianCheck = 0xffff;

        [field: CompilerGenerated]
        public event NewStatisticsEventHandler NewStastics;

        public ZDoomStatsReader(IGameFileDataSource gameFile, string directory, IEnumerable<IStatsDataSource> existingStats)
        {
            this.m_dir = directory;
            string[] extensions = new string[] { ".zds" };
            this.m_detector = new NewFileDetector(extensions, directory, true);
            this.m_statistics = existingStats.ToList<IStatsDataSource>();
            this.m_gameFile = gameFile;
        }

        private LevelCount CheckLevelCount(LevelCount count)
        {
            if (count.levelcount > s_endianCheck)
            {
                count.levelcount = ReverseBytes(count.levelcount);
            }
            return count;
        }

        private LevelStats CheckStats(LevelStats stats)
        {
            if (stats.killcount > s_endianCheck)
            {
                stats.killcount = ReverseBytes(stats.killcount);
            }
            if (stats.leveltime > s_endianCheck)
            {
                stats.leveltime = ReverseBytes(stats.leveltime);
            }
            if (stats.secretcount > s_endianCheck)
            {
                stats.secretcount = ReverseBytes(stats.secretcount);
            }
            if (stats.totalkills > s_endianCheck)
            {
                stats.totalkills = ReverseBytes(stats.totalkills);
            }
            if (stats.totalsecrets > s_endianCheck)
            {
                stats.totalsecrets = ReverseBytes(stats.totalsecrets);
            }
            return stats;
        }

        public static IStatisticsReader CreateDefault(IGameFileDataSource gameFile, string directory, IEnumerable<IStatsDataSource> existingStats) => 
            new ZDoomStatsReader(gameFile, directory, existingStats);

        private static StatsDataSource CreateStatsDataSource(uint totalkills, uint killcount, uint totalsecrets, uint secretcount, uint leveltime, string name)
        {
            float num = Convert.ToSingle(leveltime) / 35f;
            return new StatsDataSource { 
                RecordTime = DateTime.Now,
                TotalKills = (int) totalkills,
                KillCount = (int) killcount,
                TotalSecrets = (int) totalsecrets,
                SecretCount = (int) secretcount,
                LevelTime = num,
                MapName = name
            };
        }

        private void HandleSaveFile(string file)
        {
            try
            {
                try
                {
                    this.ReadSaveFile(file);
                }
                catch (IOException)
                {
                    Thread.Sleep(200);
                    this.ReadSaveFile(file);
                }
            }
            catch (FileNotFoundException exception)
            {
                this.m_errors.Add($"The file {exception.FileName} was not found and could not be parsed.");
            }
            catch (Exception exception2)
            {
                this.m_errors.Add($"An unexpected error occurred with {file}: {exception2.Message} {Environment.NewLine} {exception2.StackTrace}");
            }
        }

        private void HandleStatsData(StatsDataSource statsData)
        {
            statsData.GameFileID = this.m_gameFile.GameFileID.Value;
            (from x in this.m_statistics
                where x is StatsDataSource
                select x).Cast<StatsDataSource>();
            if (!this.m_statistics.Contains(statsData))
            {
                this.m_statistics.Add(statsData);
                if (this.NewStastics != null)
                {
                    this.NewStastics(this, new NewStatisticsEventArgs(statsData));
                }
            }
        }

        private void m_checkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.m_checkTimer.Stop();
            string[] strArray = this.m_detector.GetNewFiles().Union<string>(this.m_detector.GetModifiedFiles()).ToArray<string>();
            if (strArray.Length != 0)
            {
                foreach (string str in strArray)
                {
                    this.HandleSaveFile(str);
                }
                string[] extensions = new string[] { ".zds" };
                this.m_detector = new NewFileDetector(extensions, this.m_dir, true);
                this.m_detector.StartDetection();
            }
            this.m_checkTimer.Start();
        }

        private bool ParseBinary(string file)
        {
            bool flag = false;
            string str = "sTat";
            byte[] buffer = new byte[str.Length];
            MemoryStream ms = new MemoryStream(File.ReadAllBytes(file));
            int num = 0;
            while ((num + str.Length) < ms.Length)
            {
                ms.Read(buffer, 0, buffer.Length);
                if (Encoding.ASCII.GetString(buffer).Equals(str))
                {
                    this.ReadStatistics(ms);
                    flag = true;
                    break;
                }
                ms.Position = ++num;
            }
            if (!flag)
            {
                this.m_errors.Add($"Unable to find statistics in the save file {file}.");
            }
            return flag;
        }

        private bool ParseJson(string file)
        {
            bool flag = false;
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(file))
                {
                    ZipArchiveEntry zae = (from x in archive.Entries
                        where x.Name.Equals("globals.json")
                        select x).FirstOrDefault<ZipArchiveEntry>();
                    if (zae == null)
                    {
                        return flag;
                    }
                    List<StatsDataSource> list = null;
                    using (StreamReader reader = new StreamReader(zae.Open()))
                    {
                        list = ParseJsonStats(zae, reader);
                    }
                    if (list == null)
                    {
                        return flag;
                    }
                    foreach (StatsDataSource source in list)
                    {
                        this.HandleStatsData(source);
                    }
                    return true;
                }
            }
            catch
            {
            }
            return flag;
        }

        private static List<StatsDataSource> ParseJsonStats(ZipArchiveEntry zae, StreamReader stream)
        {
            List<StatsDataSource> list = new List<StatsDataSource>();
            JObject obj2 = (JsonConvert.DeserializeObject(stream.ReadToEnd()) as JObject).GetValue("statistics") as JObject;
            if (obj2 != null)
            {
                JToken token = obj2.GetValue("levels");
                if (token == null)
                {
                    return list;
                }
                foreach (JObject obj1 in (IEnumerable<JToken>) token)
                {
                    uint totalkills = Convert.ToUInt32(obj1.GetValue("totalkills"));
                    uint killcount = Convert.ToUInt32(obj1.GetValue("killcount"));
                    uint totalsecrets = Convert.ToUInt32(obj1.GetValue("totalsecrets"));
                    uint secretcount = Convert.ToUInt32(obj1.GetValue("secretcount"));
                    Convert.ToUInt32(obj1.GetValue("totalitems"));
                    Convert.ToUInt32(obj1.GetValue("itemcount"));
                    uint leveltime = Convert.ToUInt32(obj1.GetValue("leveltime"));
                    string name = obj1.GetValue("levelname").ToString();
                    list.Add(CreateStatsDataSource(totalkills, killcount, totalsecrets, secretcount, leveltime, name));
                }
            }
            return list;
        }

        private static int ReadCount(MemoryStream ms)
        {
            byte[] buffer = new byte[1];
            int num = 0;
            int num2 = 0;
            do
            {
                ms.Read(buffer, 0, 1);
                num |= (buffer[0] & 0x7f) << num2;
                num2 += 7;
            }
            while ((buffer[0] & 0x80) != 0);
            return num;
        }

        public void ReadNow()
        {
            throw new NotSupportedException();
        }

        private void ReadSaveFile(string file)
        {
            if (!this.ParseJson(file))
            {
                this.ParseBinary(file);
            }
        }

        private void ReadStatistics(MemoryStream ms)
        {
            LevelCount count = ReadStuctureFromFile<LevelCount>(ms);
            count = this.CheckLevelCount(count);
            int num = ReadCount(ms);
            ms.Position += num - 1;
            for (int i = 0; i < count.levelcount; i++)
            {
                LevelStats stats = ReadStuctureFromFile<LevelStats>(ms);
                stats = this.CheckStats(stats);
                ms.Position += 1L;
                num = ReadCount(ms) - 1;
                byte[] buffer = new byte[num];
                ms.Read(buffer, 0, buffer.Length);
                string name = Encoding.ASCII.GetString(buffer).ToLower();
                StatsDataSource statsData = CreateStatsDataSource(stats.totalkills, stats.killcount, stats.totalsecrets, stats.secretcount, stats.leveltime, name);
                this.HandleStatsData(statsData);
            }
        }

        private static T ReadStuctureFromFile<T>(MemoryStream ms)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            ms.Read(buffer, 0, buffer.Length);
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            handle.Free();
            return (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        }

        private static uint ReverseBytes(uint value) => 
            ((uint) (((((value & 0xff) << 0x18) | ((value & 0xff00) << 8)) | ((value & 0xff0000) >> 8)) | ((value & -16777216) >> 0x18)));

        public void Start()
        {
            if (this.m_checkTimer == null)
            {
                this.m_detector.StartDetection();
                this.m_checkTimer = new System.Timers.Timer(1000.0);
                this.m_checkTimer.Elapsed += new ElapsedEventHandler(this.m_checkTimer_Elapsed);
                this.m_checkTimer.Start();
            }
        }

        public void Stop()
        {
            if (this.m_checkTimer != null)
            {
                this.m_checkTimer.Stop();
            }
        }

        public static bool Supported(ISourcePortDataSource sourcePort)
        {
            string str = sourcePort.Executable.ToLower();
            return (str.Contains("zdoom.exe") || str.Contains("zandronum.exe"));
        }

        public string[] Errors =>
            this.m_errors.ToArray();

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
            string.Empty;

        public bool ReadOnClose =>
            false;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ZDoomStatsReader.<>c <>9 = new ZDoomStatsReader.<>c();
            public static Func<ZipArchiveEntry, bool> <>9__29_0;
            public static Func<IStatsDataSource, bool> <>9__33_0;

            internal bool <HandleStatsData>b__33_0(IStatsDataSource x) => 
                (x is StatsDataSource);

            internal bool <ParseJson>b__29_0(ZipArchiveEntry x) => 
                x.Name.Equals("globals.json");
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LevelCount
        {
            public uint levelcount;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LevelStats
        {
            public uint totalkills;
            public uint killcount;
            public uint totalsecrets;
            public uint secretcount;
            public uint leveltime;
        }
    }
}

