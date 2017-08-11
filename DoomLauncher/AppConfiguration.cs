namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class AppConfiguration
    {
        public AppConfiguration(IDataSourceAdapter adapter)
        {
            this.DataSourceAdapter = adapter;
            this.Refresh(false);
        }

        public string GetConfigValue(ConfigType ct)
        {
            IConfigurationDataSource source = (from x in this.DataSourceAdapter.GetConfiguration()
                where x.Name == ct.ToString("g")
                select x).FirstOrDefault<IConfigurationDataSource>();
            if (source != null)
            {
                return source.Value;
            }
            return null;
        }

        public object GetTypedConfigValue(ConfigType ct, System.Type type) => 
            GetValueFromConfig(this.GetConfigValue(ct), type);

        private static object GetValueFromConfig(string value, System.Type type)
        {
            if (type == typeof(string))
            {
                return value;
            }
            if (type == typeof(int))
            {
                int num = -1;
                int.TryParse(value, out num);
                return num;
            }
            if (type != typeof(bool))
            {
                throw new NotSupportedException($"Type {type.ToString()} not supported ");
            }
            bool result = false;
            bool.TryParse(value, out result);
            return result;
        }

        public void Refresh()
        {
            this.Refresh(true);
        }

        private void Refresh(bool throwErrors)
        {
            IEnumerable<IConfigurationDataSource> configuration = this.DataSourceAdapter.GetConfiguration();
            this.IdGamesUrl = (from item in configuration
                where item.Name == "IdGamesUrl"
                select item).First<IConfigurationDataSource>().Value;
            this.ApiPage = (from item in configuration
                where item.Name == "ApiPage"
                select item).First<IConfigurationDataSource>().Value;
            this.MirrorUrl = (from item in configuration
                where item.Name == "MirrorUrl"
                select item).First<IConfigurationDataSource>().Value;
            this.CleanTemp = Convert.ToBoolean((from item in configuration
                where item.Name == "CleanTemp"
                select item).First<IConfigurationDataSource>().Value);
            this.SetChildDirectories(configuration);
            this.SplitTopBottom = Convert.ToInt32((from item in configuration
                where item.Name == "SplitTopBottom"
                select item).First<IConfigurationDataSource>().Value);
            this.SplitLeftRight = Convert.ToInt32((from item in configuration
                where item.Name == "SplitLeftRight"
                select item).First<IConfigurationDataSource>().Value);
            this.AppWidth = Convert.ToInt32((from item in configuration
                where item.Name == "AppWidth"
                select item).First<IConfigurationDataSource>().Value);
            this.AppHeight = Convert.ToInt32((from item in configuration
                where item.Name == "AppHeight"
                select item).First<IConfigurationDataSource>().Value);
            this.AppX = Convert.ToInt32((from item in configuration
                where item.Name == "AppX"
                select item).First<IConfigurationDataSource>().Value);
            this.AppY = Convert.ToInt32((from item in configuration
                where item.Name == "AppY"
                select item).First<IConfigurationDataSource>().Value);
            this.WindowState = (FormWindowState) Enum.Parse(typeof(FormWindowState), (from item in configuration
                where item.Name == "WindowState"
                select item).First<IConfigurationDataSource>().Value);
            this.ColumnConfig = (from item in configuration
                where item.Name == "ColumnConfig"
                select item).First<IConfigurationDataSource>().Value;
            this.DateParseFormats = this.SplitString((from item in configuration
                where item.Name == "DateParseFormats"
                select item).First<IConfigurationDataSource>().Value);
            this.ScreenshotCaptureDirectories = this.SplitString((from item in configuration
                where item.Name == "ScreenshotCaptureDirectories"
                select item).First<IConfigurationDataSource>().Value);
            this.VerifyPaths(throwErrors);
        }

        public void RefreshColumnConfig()
        {
            IEnumerable<IConfigurationDataSource> configuration = this.DataSourceAdapter.GetConfiguration();
            this.ColumnConfig = (from item in configuration
                where item.Name == "ColumnConfig"
                select item).First<IConfigurationDataSource>().Value;
        }

        private void SetChildDirectories(IEnumerable<IConfigurationDataSource> config)
        {
            string gameFileDirectory = (from item in config
                where item.Name == "GameFileDirectory"
                select item).First<IConfigurationDataSource>().Value;
            this.ScreenshotDirectory = this.SetChildDirectory(gameFileDirectory, "Screenshots");
            this.TempDirectory = this.SetChildDirectory(gameFileDirectory, "Temp");
            this.DemoDirectory = this.SetChildDirectory(gameFileDirectory, "Demos");
            this.SaveGameDirectory = this.SetChildDirectory(gameFileDirectory, "SaveGames");
            this.GameFileDirectory = new LauncherPath(gameFileDirectory);
        }

        private LauncherPath SetChildDirectory(string gameFileDirectory, string childDirectory)
        {
            if (!childDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                childDirectory = childDirectory + Path.DirectorySeparatorChar.ToString();
            }
            return new LauncherPath(Path.Combine(gameFileDirectory, childDirectory));
        }

        private string[] SplitString(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                char[] separator = new char[] { ';' };
                return item.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }

        private void VerifyPath(LauncherPath path, bool throwErrors)
        {
            if (throwErrors && !Directory.Exists(path.GetFullPath()))
            {
                throw new DirectoryNotFoundException(path.GetPossiblyRelativePath());
            }
        }

        private void VerifyPaths(bool throwErrors)
        {
            this.VerifyPath(this.GameFileDirectory, throwErrors);
            this.VerifyPath(this.ScreenshotDirectory, throwErrors);
            this.VerifyPath(this.TempDirectory, throwErrors);
            this.VerifyPath(this.DemoDirectory, throwErrors);
            this.VerifyPath(this.SaveGameDirectory, throwErrors);
        }

        public string ApiPage { get; private set; }

        public int AppHeight { get; private set; }

        public int AppWidth { get; private set; }

        public int AppX { get; private set; }

        public int AppY { get; private set; }

        public bool CleanTemp { get; private set; }

        public string ColumnConfig { get; private set; }

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public string[] DateParseFormats { get; private set; }

        public LauncherPath DemoDirectory { get; private set; }

        public LauncherPath GameFileDirectory { get; private set; }

        public string IdGamesUrl { get; private set; }

        public string MirrorUrl { get; private set; }

        public LauncherPath SaveGameDirectory { get; private set; }

        public string[] ScreenshotCaptureDirectories { get; private set; }

        public LauncherPath ScreenshotDirectory { get; private set; }

        public int SplitLeftRight { get; private set; }

        public int SplitTopBottom { get; private set; }

        public LauncherPath TempDirectory { get; private set; }

        public FormWindowState WindowState { get; private set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly AppConfiguration.<>c <>9 = new AppConfiguration.<>c();
            public static Func<IConfigurationDataSource, bool> <>9__5_0;
            public static Func<IConfigurationDataSource, bool> <>9__5_1;
            public static Func<IConfigurationDataSource, bool> <>9__5_10;
            public static Func<IConfigurationDataSource, bool> <>9__5_11;
            public static Func<IConfigurationDataSource, bool> <>9__5_12;
            public static Func<IConfigurationDataSource, bool> <>9__5_13;
            public static Func<IConfigurationDataSource, bool> <>9__5_2;
            public static Func<IConfigurationDataSource, bool> <>9__5_3;
            public static Func<IConfigurationDataSource, bool> <>9__5_4;
            public static Func<IConfigurationDataSource, bool> <>9__5_5;
            public static Func<IConfigurationDataSource, bool> <>9__5_6;
            public static Func<IConfigurationDataSource, bool> <>9__5_7;
            public static Func<IConfigurationDataSource, bool> <>9__5_8;
            public static Func<IConfigurationDataSource, bool> <>9__5_9;
            public static Func<IConfigurationDataSource, bool> <>9__6_0;
            public static Func<IConfigurationDataSource, bool> <>9__8_0;

            internal bool <Refresh>b__5_0(IConfigurationDataSource item) => 
                (item.Name == "IdGamesUrl");

            internal bool <Refresh>b__5_1(IConfigurationDataSource item) => 
                (item.Name == "ApiPage");

            internal bool <Refresh>b__5_10(IConfigurationDataSource item) => 
                (item.Name == "WindowState");

            internal bool <Refresh>b__5_11(IConfigurationDataSource item) => 
                (item.Name == "ColumnConfig");

            internal bool <Refresh>b__5_12(IConfigurationDataSource item) => 
                (item.Name == "DateParseFormats");

            internal bool <Refresh>b__5_13(IConfigurationDataSource item) => 
                (item.Name == "ScreenshotCaptureDirectories");

            internal bool <Refresh>b__5_2(IConfigurationDataSource item) => 
                (item.Name == "MirrorUrl");

            internal bool <Refresh>b__5_3(IConfigurationDataSource item) => 
                (item.Name == "CleanTemp");

            internal bool <Refresh>b__5_4(IConfigurationDataSource item) => 
                (item.Name == "SplitTopBottom");

            internal bool <Refresh>b__5_5(IConfigurationDataSource item) => 
                (item.Name == "SplitLeftRight");

            internal bool <Refresh>b__5_6(IConfigurationDataSource item) => 
                (item.Name == "AppWidth");

            internal bool <Refresh>b__5_7(IConfigurationDataSource item) => 
                (item.Name == "AppHeight");

            internal bool <Refresh>b__5_8(IConfigurationDataSource item) => 
                (item.Name == "AppX");

            internal bool <Refresh>b__5_9(IConfigurationDataSource item) => 
                (item.Name == "AppY");

            internal bool <RefreshColumnConfig>b__8_0(IConfigurationDataSource item) => 
                (item.Name == "ColumnConfig");

            internal bool <SetChildDirectories>b__6_0(IConfigurationDataSource item) => 
                (item.Name == "GameFileDirectory");
        }
    }
}

