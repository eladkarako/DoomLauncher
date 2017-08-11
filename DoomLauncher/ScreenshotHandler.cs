namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class ScreenshotHandler
    {
        public ScreenshotHandler(IDataSourceAdapter adapter, LauncherPath screenshotDirectory)
        {
            this.DataSourceAdapter = adapter;
            this.ScreenshotDirectory = screenshotDirectory;
        }

        public IEnumerable<IFileDataSource> HandleNewScreenshots(ISourcePortDataSource sourcePort, IGameFileDataSource gameFile, string[] files)
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
                        info.CopyTo(Path.Combine(this.ScreenshotDirectory.GetFullPath(), str2));
                        FileDataSource ds = new FileDataSource {
                            FileName = str2,
                            GameFileID = gameFile.GameFileID.Value,
                            SourcePortID = sourcePort.SourcePortID,
                            FileTypeID = 1
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

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public LauncherPath ScreenshotDirectory { get; set; }
    }
}

