namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class VersionHandler
    {
        private IDataSourceAdapter m_adapter;
        private AppConfiguration m_appConfig;

        [field: CompilerGenerated]
        public event EventHandler UpdateComplete;

        [field: CompilerGenerated]
        public event EventHandler UpdateProgress;

        public VersionHandler(DoomLauncher.DataAccess access, IDataSourceAdapter adapter, AppConfiguration appConfig)
        {
            this.DataAccess = access;
            this.m_adapter = adapter;
            this.m_appConfig = appConfig;
        }

        private void ExecuteUpdate(Action update, AppVersion version)
        {
            if ((this.GetVersion() < version) || (version == AppVersion.Unknown))
            {
                update();
                if (version != AppVersion.Unknown)
                {
                    this.WriteVersion(version);
                }
            }
        }

        private AppVersion GetVersion()
        {
            IConfigurationDataSource source = (from x in this.m_adapter.GetConfiguration()
                where x.Name == "Version"
                select x).FirstOrDefault<IConfigurationDataSource>();
            if (source != null)
            {
                return (AppVersion) Convert.ToInt32(source.Value);
            }
            return AppVersion.Unknown;
        }

        public void HandleVersionUpdate()
        {
            if (this.UpdateRequired())
            {
                this.ExecuteUpdate(new Action(this.Pre_0_9_2), AppVersion.Unknown);
                this.ExecuteUpdate(new Action(this.Pre_1_0_0), AppVersion.Version_1_0_0);
                this.ExecuteUpdate(new Action(this.Pre_1_1_0), AppVersion.Version_1_1_0);
                this.ExecuteUpdate(new Action(this.Pre_1_2_0), AppVersion.Version_1_2_0);
                this.ExecuteUpdate(new Action(this.Pre_2_1_0), AppVersion.Version_2_1_0);
                this.ExecuteUpdate(new Action(this.Pre_2_2_0), AppVersion.Version_2_2_0);
                this.ExecuteUpdate(new Action(this.Pre_2_2_1), AppVersion.Version_2_2_1);
                this.ExecuteUpdate(new Action(this.Pre_2_3_0), AppVersion.Version_2_3_0);
                this.ExecuteUpdate(new Action(this.Pre_2_4_0), AppVersion.Version_2_4_0);
                this.ExecuteUpdate(new Action(this.Pre_2_4_1), AppVersion.Version_2_4_1);
            }
        }

        public void Pre_0_9_2()
        {
            if (this.DataAccess.ExecuteSelect("select name from sqlite_master where type='table' and name='Configuration';").Tables[0].Rows.Count == 0)
            {
                string sql = "CREATE TABLE 'Configuration' (\r\n\t                'ConfigID'\tINTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n\t                'Name'\tTEXT NOT NULL,\r\n\t                'Value'\tTEXT NOT NULL,\r\n\t                'AvailableValues'\tTEXT NOT NULL,\r\n\t                'UserCanModify'\tINTEGER);";
                this.DataAccess.ExecuteNonQuery(sql);
                sql = $"insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('IdGamesUrl', 'http://www.doomworld.com/idgames/', '', 1);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('ApiPage', 'api/api.php', '', 1);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('MirrorUrl', 'ftp://mancubus.net/pub/idgames/', 'Germany;ftp://ftp.fu-berlin.de/pc/games/idgames/;Idaho;ftp://mirrors.syringanetworks.net/idgames/;Greece;ftp://ftp.ntua.gr/pub/vendors/idgames/;Texas;ftp://mancubus.net/pub/idgames/;New York;http://youfailit.net/pub/idgames/;Florida;http://www.gamers.org/pub/idgames/;', 1);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('ScreenshotCaptureDirectories', '', '', 1);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('DateParseFormats', 'dd/M/yy;dd/MM/yyyy;dd MMMM yyyy;', '', 0);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('CleanTemp', 'true', '', 0);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('GameFileDirectory', '{ConfigurationManager.AppSettings["GameFileDirectory"]}', '', 0);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('ScreenshotDirectory', '{ConfigurationManager.AppSettings["ScreenshotDirectory"]}', '', 0);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('TempDirectory', '{ConfigurationManager.AppSettings["TempDirectory"]}', '', 0);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('GameWadDirectory', '{ConfigurationManager.AppSettings["GameWadDirectory"]}', '', 0);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('DemoDirectory', '{ConfigurationManager.AppSettings["DemoDirectory"]}', '', 0);
                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('SaveGameDirectory', '{ConfigurationManager.AppSettings["GameFileDirectory"] + @"SaveGames\"}', '', 0);";
                this.DataAccess.ExecuteNonQuery(sql);
                DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\" + ConfigurationManager.AppSettings["GameFileDirectory"] + @"SaveGames\");
                if (!info.Exists)
                {
                    info.Create();
                }
            }
            if (this.DataAccess.ExecuteSelect("pragma table_info(Files);").Tables[0].Select("name = 'OriginalFileName'").Count<DataRow>() == 0)
            {
                string str2 = "alter table Files add column 'OriginalFileName' TEXT;\r\n                alter table Files add column 'OriginalFilePath' TEXT;";
                this.DataAccess.ExecuteNonQuery(str2);
            }
        }

        public void Pre_1_0_0()
        {
            if (this.DataAccess.ExecuteSelect("pragma table_info(GameFiles);").Tables[0].Select("name = 'SettingsMap'").Count<DataRow>() == 0)
            {
                string sql = "alter table GameFiles add column 'SettingsMap' TEXT;\r\n                alter table GameFiles add column 'SettingsSkill' TEXT;\r\n                alter table GameFiles add column 'SettingsExtraParams' TEXT;\r\n                alter table GameFiles add column 'SettingsFiles' TEXT;";
                this.DataAccess.ExecuteNonQuery(sql);
            }
            if (this.DataAccess.ExecuteSelect("select * from Configuration where Name = 'ColumnConfig'").Tables[0].Rows.Count == 0)
            {
                string str2 = "insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('SplitTopBottom', '475', '', 0);\r\n                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('SplitLeftRight', '680', '', 0);\r\n                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('AppWidth', '1024', '', 0);\r\n                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('AppHeight', '768', '', 0);\r\n                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('AppX', '0', '', 0);\r\n                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('AppY', '0', '', 0);\r\n                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('WindowState', 'Normal', '', 0);\r\n                insert into Configuration (Name, Value, AvailableValues, UserCanModify) values('ColumnConfig', '', '', 0);";
                this.DataAccess.ExecuteNonQuery(str2);
            }
        }

        public void Pre_1_1_0()
        {
            DataTable table = this.DataAccess.ExecuteSelect("select name from sqlite_master where type='table' and name='Tags';").Tables[0];
            if (table.Rows.Count == 0)
            {
                string sql = "CREATE TABLE 'Tags' (\r\n\t            'TagID'\tINTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n\t            'Name'\tTEXT NOT NULL,\r\n\t            'HasTab'\tINTEGER NOT NULL);";
                this.DataAccess.ExecuteSelect(sql);
                sql = "CREATE TABLE 'TagMapping' (\r\n\t            'FileID'\tINTEGER NOT NULL,\r\n\t            'TagID'\tINTEGER NOT NULL,\r\n\t            PRIMARY KEY(FileID,TagID));";
                this.DataAccess.ExecuteSelect(sql);
            }
            table = this.DataAccess.ExecuteSelect("pragma table_info(Tags);").Tables[0];
            if (table.Select("name = 'Color'").Count<DataRow>() == 0)
            {
                this.DataAccess.ExecuteNonQuery("alter table Tags add column 'HasColor' int;");
                this.DataAccess.ExecuteNonQuery("alter table Tags add column 'Color' int;");
            }
            table = this.DataAccess.ExecuteSelect("pragma table_info(GameFiles);").Tables[0];
            if (table.Select("name = 'MapCount'").Count<DataRow>() == 0)
            {
                new FileInfo("DoomLauncher.sqlite").CopyTo($"DoomLauncher_{Guid.NewGuid().ToString()}.sqlite.bak");
                this.DataAccess.ExecuteNonQuery("alter table GameFiles add column 'MapCount' int;");
                GameFileGetOptions options = new GameFileGetOptions();
                options.SelectFields = new GameFileFieldType[] { GameFileFieldType.GameFileID, GameFileFieldType.Map };
                IEnumerable<IGameFileDataSource> gameFiles = this.m_adapter.GetGameFiles(options);
                GameFileFieldType[] updateFields = new GameFileFieldType[] { GameFileFieldType.MapCount };
                float num = gameFiles.Count<IGameFileDataSource>();
                int num2 = 0;
                foreach (IGameFileDataSource source in gameFiles)
                {
                    if (this.UpdateProgress != null)
                    {
                        this.ProgressPercent = Convert.ToInt32((float) ((((float) num2) / num) * 100f));
                        this.UpdateProgress(this, new EventArgs());
                    }
                    if (source.Map != null)
                    {
                        source.MapCount = new int?(source.Map.Count<char>(x => (x == ',')) + 1);
                        this.m_adapter.UpdateGameFile(source, updateFields);
                    }
                    num2++;
                }
            }
            if (table.Select("name = 'SettingsSpecificFiles'").Count<DataRow>() == 0)
            {
                this.DataAccess.ExecuteNonQuery("alter table GameFiles add column 'SettingsSpecificFiles' TEXT;");
            }
            table = this.DataAccess.ExecuteSelect("select * from Configuration where Name = 'Version'").Tables[0];
            this.WriteVersion(AppVersion.Version_1_1_0);
            if (this.UpdateComplete != null)
            {
                this.UpdateComplete(this, new EventArgs());
            }
        }

        private void Pre_1_2_0()
        {
        }

        private void Pre_2_1_0()
        {
            if (this.DataAccess.ExecuteSelect("pragma table_info(Files);").Tables[0].Select("name = 'FileOrder'").Count<DataRow>() == 0)
            {
                this.DataAccess.ExecuteNonQuery("alter table Files add column 'FileOrder' int;");
                this.DataAccess.ExecuteNonQuery("update Files set FileOrder = 2");
            }
            if (this.DataAccess.ExecuteSelect("pragma table_info(GameFiles);").Tables[0].Select("name = 'MinutesPlayed'").Count<DataRow>() == 0)
            {
                this.DataAccess.ExecuteNonQuery("alter table GameFiles add column 'MinutesPlayed' int;");
                this.DataAccess.ExecuteNonQuery("update GameFiles set MinutesPlayed = 0");
            }
        }

        private void Pre_2_2_0()
        {
            if (this.DataAccess.ExecuteSelect("select name from sqlite_master where type='table' and name='Stats';").Tables[0].Rows.Count == 0)
            {
                this.DataAccess.ExecuteNonQuery("update Configuration set UserCanModify = 1 where Name = 'GameFileDirectory'");
                string sql = "CREATE TABLE 'Stats' (\r\n\t            'StatID'\tINTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n\t            'GameFileID'\tINTEGER NOT NULL,\r\n                'KillCount'\tINTEGER NOT NULL,\r\n                'TotalKills'\tINTEGER NOT NULL,\r\n                'SecretCount'\tINTEGER NOT NULL,\r\n                'TotalSecrets'\tINTEGER NOT NULL,\r\n                'LevelTime'\tREAL NOT NULL,\r\n                'ItemCount'\tINTEGER NOT NULL,\r\n                'TotalItems'\tINTEGER NOT NULL,\r\n                'SourcePortID'\tINTEGER NOT NULL,\r\n                'MapName'\tTEXT NOT NULL,\r\n                'RecordTime'\tTEXT NOT NULL);";
                this.DataAccess.ExecuteNonQuery(sql);
            }
        }

        private void Pre_2_2_1()
        {
            foreach (ISourcePortDataSource source in this.m_adapter.GetSourcePorts())
            {
                if (source.SupportedExtensions.Contains(".pk3"))
                {
                    source.SupportedExtensions = source.SupportedExtensions.Replace(".pk3", ".pk3,.pk7");
                }
                List<DbParameter> parameters = new List<DbParameter> {
                    this.DataAccess.DbAdapter.CreateParameter("ext", source.SupportedExtensions),
                    this.DataAccess.DbAdapter.CreateParameter("SourcePortID", source.SourcePortID)
                };
                this.DataAccess.ExecuteNonQuery("update SourcePorts set SupportedExtensions = @ext where SourcePortID = @SourcePortID", parameters);
            }
        }

        private void Pre_2_3_0()
        {
            HashSet<IStatsDataSource> set = new HashSet<IStatsDataSource>();
            foreach (IStatsDataSource source in this.m_adapter.GetStats())
            {
                if (!set.Contains(source))
                {
                    set.Add(source);
                }
                else
                {
                    this.m_adapter.DeleteStats(source.StatID);
                }
            }
        }

        private void Pre_2_4_0()
        {
            if (this.DataAccess.ExecuteSelect("pragma table_info(IWads);").Tables[0].Select("name = 'GameFileID'").Count<DataRow>() == 0)
            {
                this.DataAccess.ExecuteNonQuery("alter table IWads add column 'GameFileID' int;");
                IEnumerable<IGameFileDataSource> gameFiles = this.m_adapter.GetGameFiles();
                using (IEnumerator<IIWadDataSource> enumerator = this.m_adapter.GetIWads().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        IIWadDataSource iwad = enumerator.Current;
                        IGameFileDataSource source = (from x in gameFiles
                            where x.FileName.ToLower() == iwad.FileName.ToLower().Replace(".wad", ".zip")
                            select x).FirstOrDefault<IGameFileDataSource>();
                        if (source != null)
                        {
                            iwad.GameFileID = source.GameFileID;
                            this.m_adapter.UpdateIWad(iwad);
                        }
                    }
                }
            }
            if (this.DataAccess.ExecuteSelect("pragma table_info(SourcePorts);").Tables[0].Select("name = 'SettingsFiles'").Count<DataRow>() == 0)
            {
                this.DataAccess.ExecuteNonQuery("alter table SourcePorts add column 'SettingsFiles' TEXT;");
            }
            this.DataAccess.ExecuteNonQuery("update GameFiles set MapCount = null where Map is null or length(Map) = 0");
        }

        private void Pre_2_4_1()
        {
            Thread.Sleep(0x4e20);
            string[] textArray1 = new string[] { "*.zds", "*.dsg", "*.esg" };
            foreach (string str in textArray1)
            {
                string[] files = Directory.GetFiles(this.m_appConfig.DemoDirectory.GetFullPath(), str);
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo info = new FileInfo(files[i]);
                    FileInfo info2 = new FileInfo(Path.Combine(this.m_appConfig.SaveGameDirectory.GetFullPath(), info.Name));
                    if (info2.Exists)
                    {
                        info2.Delete();
                    }
                    info.MoveTo(info.FullName);
                }
            }
        }

        public bool UpdateRequired()
        {
            AppVersion version = this.GetVersion();
            if ((version != AppVersion.Unknown) && (version >= AppVersion.Version_2_4_1))
            {
                return false;
            }
            return true;
        }

        private void WriteVersion(AppVersion version)
        {
            IConfigurationDataSource ds = (from x in this.m_adapter.GetConfiguration()
                where x.Name == "Version"
                select x).FirstOrDefault<IConfigurationDataSource>();
            if (ds != null)
            {
                ds.Value = Convert.ToInt32(version).ToString();
                this.m_adapter.UpdateConfiguration(ds);
            }
            else
            {
                ConfigurationDataSource source2 = new ConfigurationDataSource {
                    Name = "Version",
                    UserCanModify = false,
                    Value = Convert.ToInt32(version).ToString()
                };
                this.m_adapter.InsertConfiguration(source2);
            }
        }

        public DoomLauncher.DataAccess DataAccess { get; set; }

        public int ProgressPercent { get; private set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly VersionHandler.<>c <>9 = new VersionHandler.<>c();
            public static Func<IConfigurationDataSource, bool> <>9__16_0;
            public static Func<IConfigurationDataSource, bool> <>9__17_0;
            public static Func<char, bool> <>9__20_0;

            internal bool <GetVersion>b__16_0(IConfigurationDataSource x) => 
                (x.Name == "Version");

            internal bool <Pre_1_1_0>b__20_0(char x) => 
                (x == ',');

            internal bool <WriteVersion>b__17_0(IConfigurationDataSource x) => 
                (x.Name == "Version");
        }
    }
}

