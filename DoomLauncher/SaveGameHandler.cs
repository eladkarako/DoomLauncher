namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal class SaveGameHandler
    {
        public SaveGameHandler(IDataSourceAdapter adapter, LauncherPath savegameDirectory)
        {
            this.DataSourceAdapter = adapter;
            this.SaveGameDirectory = savegameDirectory;
        }

        public void CopySaveGamesToSourcePort(ISourcePortDataSource sourcePort, IFileDataSource[] files)
        {
            files = (from file in files
                where file.SourcePortID == sourcePort.SourcePortID
                select file).ToArray<IFileDataSource>();
            foreach (IFileDataSource source in files)
            {
                string destFileName = Path.Combine(sourcePort.Directory.GetFullPath(), source.OriginalFileName);
                FileInfo info = new FileInfo(Path.Combine(this.SaveGameDirectory.GetFullPath(), source.FileName));
                try
                {
                    if (info.Exists)
                    {
                        DirectoryInfo info2 = new DirectoryInfo(Path.Combine(sourcePort.Directory.GetFullPath(), source.OriginalFilePath));
                        if (!info2.Exists)
                        {
                            info2.Create();
                        }
                        info.CopyTo(destFileName);
                    }
                }
                catch
                {
                }
            }
        }

        public IEnumerable<IFileDataSource> HandleNewSaveGames(ISourcePortDataSource sourcePort, IGameFileDataSource gameFile, string[] files)
        {
            List<IFileDataSource> list = new List<IFileDataSource>();
            if ((gameFile != null) && gameFile.GameFileID.HasValue)
            {
                foreach (string str in files)
                {
                    try
                    {
                        FileInfo info = new FileInfo(str);
                        string str2 = Guid.NewGuid().ToString() + info.Extension;
                        info.CopyTo(Path.Combine(this.SaveGameDirectory.GetFullPath(), str2));
                        FileDataSource ds = new FileDataSource {
                            Description = info.Name,
                            OriginalFileName = info.Name,
                            FileName = str2,
                            GameFileID = gameFile.GameFileID.Value,
                            SourcePortID = sourcePort.SourcePortID,
                            FileTypeID = 3
                        };
                        this.DataSourceAdapter.InsertFile(ds);
                        list.Add(ds);
                    }
                    catch
                    {
                    }
                }
            }
            return list;
        }

        public void HandleUpdateSaveGames(ISourcePortDataSource sourcePort, IGameFileDataSource gameFile, IFileDataSource[] files)
        {
            foreach (IFileDataSource source in files)
            {
                FileInfo info = new FileInfo(Path.Combine(sourcePort.Directory.GetFullPath(), source.OriginalFileName));
                if (info.Exists)
                {
                    try
                    {
                        info.CopyTo(Path.Combine(this.SaveGameDirectory.GetFullPath(), source.FileName), true);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public LauncherPath SaveGameDirectory { get; set; }
    }
}

