namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using DoomLauncher.TextFileParsers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class DirectoryDataSourceAdapter : IGameFileDataSourceAdapter, IIWadDataSourceAdapter
    {
        public DirectoryDataSourceAdapter(LauncherPath gameFileDirectory, string[] dateParseFormats)
        {
            this.GameFileDirectory = gameFileDirectory;
            this.DateParseFormats = dateParseFormats.ToArray<string>();
        }

        public void DeleteGameFile(IGameFileDataSource ds)
        {
            HandleDelete(this.GameFileDirectory.GetFullPath(), ds.FileName);
        }

        public void DeleteIWad(IIWadDataSource ds)
        {
            throw new NotSupportedException();
        }

        private IGameFileDataSource FromZipFile(FileInfo fi)
        {
            IGameFileDataSource source = new GameFileDataSource {
                FileName = fi.Name
            };
            ZipArchive archive = null;
            try
            {
                archive = ZipFile.OpenRead(fi.FullName);
            }
            catch
            {
                return null;
            }
            bool flag = false;
            try
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (new FileInfo(entry.FullName).Extension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] buffer = new byte[entry.Length];
                        entry.Open().Read(buffer, 0, Convert.ToInt32(entry.Length));
                        IdGamesTextFileParser parser = new IdGamesTextFileParser(Encoding.UTF7.GetString(buffer), this.DateParseFormats);
                        source.Title = parser.Title;
                        source.Author = parser.Author;
                        source.ReleaseDate = parser.ReleaseDate;
                        source.Description = parser.Description;
                        if (string.IsNullOrEmpty(source.Title))
                        {
                            source.Title = fi.Name;
                        }
                        flag = (!string.IsNullOrEmpty(source.Title) || !string.IsNullOrEmpty(source.Author)) || !string.IsNullOrEmpty(source.Description);
                    }
                    if (flag)
                    {
                        return source;
                    }
                }
                return source;
            }
            catch (InvalidDataException exception1)
            {
                source = null;
                string message = exception1.Message;
            }
            return source;
        }

        public IGameFileDataSource GetGameFile(string fileName)
        {
            FileInfo fi = new FileInfo(Path.Combine(this.GameFileDirectory.GetFullPath(), fileName));
            if (fi.Exists)
            {
                return this.FromZipFile(fi);
            }
            return null;
        }

        public IEnumerable<IGameFileDataSource> GetGameFileIWads()
        {
            throw new NotSupportedException();
        }

        public IEnumerable<string> GetGameFileNames() => 
            (from name in new DirectoryInfo(this.GameFileDirectory.GetFullPath()).GetFiles() select name.Name);

        public IEnumerable<IGameFileDataSource> GetGameFiles() => 
            this.GetGameFiles(null);

        public IEnumerable<IGameFileDataSource> GetGameFiles(IGameFileGetOptions options)
        {
            int num = options.Limit.HasValue ? options.Limit.Value : -1;
            List<IGameFileDataSource> list = new List<IGameFileDataSource>();
            int num2 = 0;
            foreach (FileInfo info in new DirectoryInfo(this.GameFileDirectory.GetFullPath()).GetFiles())
            {
                num2++;
                list.Add(this.FromZipFile(info));
                if ((num > -1) && (num2 == num))
                {
                    return list;
                }
            }
            return list;
        }

        public int GetGameFilesCount() => 
            new DirectoryInfo(this.GameFileDirectory.GetFullPath()).GetFiles().Length;

        public IIWadDataSource GetIWad(int gameFileID)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<IIWadDataSource> GetIWads()
        {
            throw new NotSupportedException();
        }

        private static void HandleDelete(string directory, string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                FileInfo info = new FileInfo(Path.Combine(directory, filename));
                if (info.Exists)
                {
                    info.Delete();
                }
            }
        }

        public void InsertGameFile(IGameFileDataSource ds)
        {
            throw new NotSupportedException();
        }

        public void InsertIWad(IIWadDataSource ds)
        {
            throw new NotSupportedException();
        }

        public void UpdateGameFile(IGameFileDataSource ds)
        {
            throw new NotSupportedException();
        }

        public void UpdateGameFile(IGameFileDataSource ds, GameFileFieldType[] updateFields)
        {
            throw new NotSupportedException();
        }

        public void UpdateIWad(IIWadDataSource ds)
        {
            throw new NotSupportedException();
        }

        public string[] DateParseFormats { get; set; }

        public LauncherPath GameFileDirectory { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly DirectoryDataSourceAdapter.<>c <>9 = new DirectoryDataSourceAdapter.<>c();
            public static Func<FileInfo, string> <>9__2_0;

            internal string <GetGameFileNames>b__2_0(FileInfo name) => 
                name.Name;
        }
    }
}

