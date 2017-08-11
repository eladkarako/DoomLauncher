namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class DemoHandler
    {
        public DemoHandler(IDataSourceAdapter adapter, LauncherPath demoDirectory)
        {
            this.DataAdapter = adapter;
            this.DemoDirectory = demoDirectory;
        }

        public IEnumerable<IFileDataSource> HandleNewDemo(ISourcePortDataSource sourcePort, IGameFileDataSource gameFile, string demoFile, string descriptionText)
        {
            FileInfo info = new FileInfo(demoFile);
            if (gameFile.GameFileID.HasValue && info.Exists)
            {
                info.CopyTo(Path.Combine(this.DemoDirectory.GetFullPath(), info.Name));
                FileDataSource ds = new FileDataSource {
                    FileName = info.Name,
                    GameFileID = gameFile.GameFileID.Value,
                    SourcePortID = sourcePort.SourcePortID,
                    FileTypeID = 2,
                    Description = descriptionText
                };
                this.DataAdapter.InsertFile(ds);
            }
            return new List<IFileDataSource>();
        }

        public IDataSourceAdapter DataAdapter { get; set; }

        public LauncherPath DemoDirectory { get; set; }
    }
}

