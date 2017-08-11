namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using WadReader;

    internal class SyncLibraryHandler
    {
        private List<InvalidFile> m_invalidFiles = new List<InvalidFile>();

        [field: CompilerGenerated]
        public event EventHandler GameFileDataNeeded;

        [field: CompilerGenerated]
        public event EventHandler SyncFileChange;

        public SyncLibraryHandler(IGameFileDataSourceAdapter dbDataSource, IGameFileDataSourceAdapter syncDataSource, LauncherPath gameFileDirectory, LauncherPath tempDirectory)
        {
            this.DbDataSource = dbDataSource;
            this.SyncDataSource = syncDataSource;
            this.GameFileDirectory = gameFileDirectory;
            this.TempDirectory = tempDirectory;
            this.SyncFileCurrent = this.SyncFileCount = 0;
        }

        private int CompareMapLump(FileLump lump1, FileLump lump2) => 
            lump1.Name.CompareTo(lump2.Name);

        public void Execute()
        {
            this.Execute(null);
        }

        public void Execute(string[] syncFiles)
        {
            this.m_invalidFiles.Clear();
            IEnumerable<string> enumerable2 = this.SyncDataSource.GetGameFileNames().Except<string>(this.DbDataSource.GetGameFileNames());
            if (syncFiles != null)
            {
                enumerable2 = syncFiles;
            }
            this.SyncFileCount = enumerable2.Count<string>();
            this.SyncFileCurrent = 0;
            foreach (string str in enumerable2)
            {
                if (this.SyncFileChange != null)
                {
                    this.CurrentSyncFileName = str;
                    this.SyncFileChange(this, new EventArgs());
                }
                IGameFileDataSource gameFile = this.SyncDataSource.GetGameFile(str);
                if (gameFile != null)
                {
                    this.CurrentGameFile = gameFile;
                    if (this.GameFileDataNeeded != null)
                    {
                        this.GameFileDataNeeded(this, new EventArgs());
                    }
                    gameFile.Downloaded = new DateTime?(DateTime.Now);
                    try
                    {
                        gameFile.Map = this.GetMaps(Path.Combine(this.GameFileDirectory.GetFullPath(), gameFile.FileName));
                        if (!string.IsNullOrEmpty(gameFile.Map))
                        {
                            gameFile.MapCount = new int?(gameFile.Map.Count<char>(x => (x == ',')) + 1);
                        }
                    }
                    catch (Exception exception1)
                    {
                        string message = exception1.Message;
                        gameFile.Map = string.Empty;
                        this.m_invalidFiles.Add(new InvalidFile(str, "Zip archive contained an improper pk3"));
                    }
                    this.DbDataSource.InsertGameFile(gameFile);
                }
                else
                {
                    this.m_invalidFiles.Add(new InvalidFile(str, "Not a valid zip archive"));
                    try
                    {
                        FileInfo info = new FileInfo(Path.Combine(this.GameFileDirectory.GetFullPath(), str));
                        if (info.Exists)
                        {
                            info.Delete();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                int syncFileCurrent = this.SyncFileCurrent;
                this.SyncFileCurrent = syncFileCurrent + 1;
            }
        }

        private IEnumerable<ZipArchiveEntry> GetEntriesByExtension(ZipArchive za, string ext) => 
            (from item in za.Entries
                where item.Name.Contains<char>('.') && new FileInfo(item.Name).Extension.Equals(ext, StringComparison.OrdinalIgnoreCase)
                select item);

        private string GetMaps(string filename)
        {
            using (ZipArchive archive = ZipFile.OpenRead(filename))
            {
                IEnumerable<ZipArchiveEntry> source = from x in this.GetEntriesByExtension(archive, ".txt")
                    where x.Name.Equals("mapinfo.txt", StringComparison.InvariantCultureIgnoreCase)
                    select x;
                if (source.Count<ZipArchiveEntry>() > 0)
                {
                    return this.MapStringFromMapInfo(source.First<ZipArchiveEntry>());
                }
                StringBuilder builder = new StringBuilder();
                builder.Append(this.MapStringFromGameFile(archive));
                foreach (ZipArchiveEntry entry in this.GetEntriesByExtension(archive, ".pk3"))
                {
                    string destinationFileName = Path.Combine(this.TempDirectory.GetFullPath(), entry.Name);
                    entry.ExtractToFile(destinationFileName);
                    builder.Append(this.GetMaps(destinationFileName));
                }
                return builder.ToString();
            }
        }

        private string MapStringFromGameFile(ZipArchive za)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ZipArchiveEntry entry in this.GetEntriesByExtension(za, ".wad"))
            {
                string destinationFileName = Path.Combine(this.TempDirectory.GetFullPath(), entry.Name);
                entry.ExtractToFile(destinationFileName, true);
                builder.Append(DoomLauncher.Util.GetMapStringFromWad(destinationFileName));
            }
            if (builder.Length > 2)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }

        private string MapStringFromMapInfo(ZipArchiveEntry zae)
        {
            string destinationFileName = Path.Combine(this.TempDirectory.GetFullPath(), zae.Name);
            zae.ExtractToFile(destinationFileName, true);
            StringBuilder builder = new StringBuilder();
            if (File.Exists(destinationFileName))
            {
                foreach (System.Text.RegularExpressions.Match match in new Regex(@"\s*map\s+\w+").Matches(File.ReadAllText(destinationFileName)))
                {
                    builder.Append(match.Value.Trim().Substring(3).Trim());
                    builder.Append(", ");
                }
                new FileInfo(destinationFileName).Delete();
            }
            if (builder.Length > 2)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }

        public IGameFileDataSource CurrentGameFile { get; set; }

        public string CurrentSyncFileName { get; private set; }

        public IGameFileDataSourceAdapter DbDataSource { get; set; }

        public LauncherPath GameFileDirectory { get; set; }

        public InvalidFile[] InvalidFiles =>
            this.m_invalidFiles.ToArray();

        public IGameFileDataSourceAdapter SyncDataSource { get; set; }

        public int SyncFileCount { get; private set; }

        public int SyncFileCurrent { get; private set; }

        public LauncherPath TempDirectory { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SyncLibraryHandler.<>c <>9 = new SyncLibraryHandler.<>c();
            public static Func<ZipArchiveEntry, bool> <>9__11_0;
            public static Func<char, bool> <>9__9_0;

            internal bool <Execute>b__9_0(char x) => 
                (x == ',');

            internal bool <GetMaps>b__11_0(ZipArchiveEntry x) => 
                x.Name.Equals("mapinfo.txt", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

