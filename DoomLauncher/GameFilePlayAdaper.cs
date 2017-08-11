namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class GameFilePlayAdaper
    {
        [field: CompilerGenerated]
        public event EventHandler ProcessExited;

        private bool AssertFile(string path, string filename, string displayTypeName)
        {
            if (!new FileInfo(Path.Combine(path, filename)).Exists)
            {
                this.LastError = $"Failed to find the {displayTypeName}: {filename}";
                return false;
            }
            return true;
        }

        private static string BuildWarpParamter(string map)
        {
            List<string> source = new List<string>();
            string item = string.Empty;
            for (int i = 0; i < map.Length; i++)
            {
                if (char.IsDigit(map[i]))
                {
                    item = item + map[i].ToString();
                }
                else
                {
                    if (item != string.Empty)
                    {
                        source.Add(item);
                    }
                    item = string.Empty;
                }
            }
            if (item != string.Empty)
            {
                source.Add(item);
            }
            StringBuilder builder = new StringBuilder();
            foreach (string str2 in source)
            {
                builder.Append(Convert.ToInt32(str2));
                builder.Append(' ');
            }
            if (source.Count<string>() > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        public string GetLaunchParameters(LauncherPath gameFileDirectory, LauncherPath tempDirectory, IGameFileDataSource gameFile, ISourcePortDataSource sourcePort, bool isGameFileIwad)
        {
            StringBuilder sb = new StringBuilder();
            List<IGameFileDataSource> list = this.AdditionalFiles.ToList<IGameFileDataSource>();
            if (isGameFileIwad)
            {
                list.Remove(gameFile);
            }
            if (this.AdditionalFiles != null)
            {
                foreach (IGameFileDataSource source in this.AdditionalFiles)
                {
                    if (!this.AssertFile(gameFileDirectory.GetFullPath(), source.FileName, "game file"))
                    {
                        return null;
                    }
                    if (!this.HandleGameFile(source, sb, gameFileDirectory, tempDirectory, sourcePort, true))
                    {
                        return null;
                    }
                }
            }
            if (this.IWad != null)
            {
                if (!this.AssertFile(gameFileDirectory.GetFullPath(), gameFile.FileName, "game file"))
                {
                    return null;
                }
                if (!this.HandleGameFileIWad(this.IWad, sb, gameFileDirectory, tempDirectory))
                {
                    return null;
                }
            }
            if (this.Map != null)
            {
                sb.Append($" -warp {BuildWarpParamter(this.Map)}");
                if (this.Skill != null)
                {
                    sb.Append($" -skill {this.Skill}");
                }
            }
            if (this.Record)
            {
                this.RecordedFileName = Path.Combine(tempDirectory.GetFullPath(), Guid.NewGuid().ToString());
                sb.Append($" -record "{this.RecordedFileName}"");
            }
            if (this.PlayDemo && (this.PlayDemoFile != null))
            {
                if (!this.AssertFile(this.PlayDemoFile, "", "demo file"))
                {
                    return null;
                }
                sb.Append($" -playdemo "{this.PlayDemoFile}"");
            }
            if (this.ExtraParameters != null)
            {
                sb.Append(" " + this.ExtraParameters);
            }
            return sb.ToString();
        }

        private bool HandleGameFile(IGameFileDataSource gameFile, StringBuilder sb, LauncherPath gameFileDirectory, LauncherPath tempDirectory, ISourcePortDataSource sourcePort, bool checkSpecific)
        {
            try
            {
                char[] separator = new char[] { ',' };
                string[] extensions = sourcePort.SupportedExtensions.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                List<string> parameters = new List<string>();
                using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(gameFileDirectory.GetFullPath(), gameFile.FileName)))
                {
                    foreach (ZipArchiveEntry entry in from item in archive.Entries
                        where (!string.IsNullOrEmpty(item.Name) && item.Name.Contains<char>('.')) && extensions.Any<string>(x => x.Equals(new FileInfo(item.Name).Extension, StringComparison.OrdinalIgnoreCase))
                        select item)
                    {
                        bool flag = true;
                        if ((checkSpecific && (this.SpecificFiles != null)) && (this.SpecificFiles.Length != 0))
                        {
                            flag = this.SpecificFiles.Contains<string>(entry.Name);
                        }
                        if (flag)
                        {
                            string destinationFileName = Path.Combine(tempDirectory.GetFullPath(), entry.Name);
                            entry.ExtractToFile(destinationFileName, true);
                            parameters.Add(destinationFileName);
                        }
                    }
                }
                parameters = this.SortParameters(parameters, extensions).ToList<string>();
                string[] source = new string[] { ".deh", ".bex" };
                List<string> list2 = new List<string>();
                if (parameters.Count > 0)
                {
                    sb.Append(" -file ");
                    foreach (string str2 in parameters)
                    {
                        FileInfo info = new FileInfo(str2);
                        if (!source.Contains<string>(info.Extension.ToLower()))
                        {
                            sb.Append($""{str2}" ");
                        }
                        else
                        {
                            list2.Add(str2);
                        }
                    }
                }
                if (list2.Count > 0)
                {
                    sb.Append(" -deh ");
                    foreach (string str3 in list2)
                    {
                        sb.Append($""{str3}" ");
                    }
                }
            }
            catch (FileNotFoundException exception1)
            {
                string message = exception1.Message;
                this.LastError = $"The game file was not found: {gameFile.FileName}";
                return false;
            }
            catch (InvalidDataException exception2)
            {
                string text2 = exception2.Message;
                this.LastError = $"The game file does not appear to be a valid zip file: {gameFile.FileName}";
                return false;
            }
            return true;
        }

        private bool HandleGameFileIWad(IGameFileDataSource gameFile, StringBuilder sb, LauncherPath gameFileDirectory, LauncherPath tempDirectory)
        {
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(gameFileDirectory.GetFullPath(), gameFile.FileName)))
                {
                    ZipArchiveEntry source = archive.Entries.First<ZipArchiveEntry>();
                    string destinationFileName = Path.Combine(tempDirectory.GetFullPath(), source.Name);
                    source.ExtractToFile(destinationFileName, true);
                    sb.Append($" -iwad "{destinationFileName}" ");
                }
            }
            catch
            {
                this.LastError = $"There was an issue with the IWad: {gameFile.FileName}";
                return false;
            }
            return true;
        }

        public bool Launch(LauncherPath gameFileDirectory, LauncherPath tempDirectory, IGameFileDataSource gameFile, ISourcePortDataSource sourcePort, bool isGameFileIwad)
        {
            if (!Directory.Exists(sourcePort.Directory.GetFullPath()))
            {
                this.LastError = "The source port directory does not exist:" + Environment.NewLine + Environment.NewLine + sourcePort.Directory.GetPossiblyRelativePath();
                return false;
            }
            this.GameFile = gameFile;
            this.SourcePort = sourcePort;
            string str = this.GetLaunchParameters(gameFileDirectory, tempDirectory, gameFile, sourcePort, isGameFileIwad);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            Directory.SetCurrentDirectory(sourcePort.Directory.GetFullPath());
            try
            {
                Process process1 = Process.Start(sourcePort.GetFullExecutablePath(), str);
                process1.EnableRaisingEvents = true;
                process1.Exited += new EventHandler(this.proc_Exited);
            }
            catch
            {
                this.LastError = "Failed to execute the source port process.";
                return false;
            }
            return true;
        }

        private void proc_Exited(object sender, EventArgs e)
        {
            if (this.ProcessExited != null)
            {
                this.ProcessExited(this, new EventArgs());
            }
        }

        private IEnumerable<string> SortParameters(IEnumerable<string> parameters, string[] extensionOrder)
        {
            List<string> list = new List<string>();
            string[] strArray = extensionOrder;
            for (int i = 0; i < strArray.Length; i++)
            {
                string ext = strArray[i];
                list.AddRange(from item in parameters
                    where item.ToLower().Contains(ext.ToLower())
                    select item);
            }
            return list;
        }

        public IGameFileDataSource[] AdditionalFiles { get; set; }

        public string ExtraParameters { get; set; }

        public IGameFileDataSource GameFile { get; private set; }

        public IGameFileDataSource IWad { get; set; }

        public string LastError { get; private set; }

        public string Map { get; set; }

        public bool PlayDemo { get; set; }

        public string PlayDemoFile { get; set; }

        public bool Record { get; set; }

        public string RecordedFileName { get; private set; }

        public string Skill { get; set; }

        public ISourcePortDataSource SourcePort { get; private set; }

        public string[] SpecificFiles { get; set; }
    }
}

