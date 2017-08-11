namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class DbDataSourceAdapter : IDataSourceAdapter, IGameFileDataSourceAdapter, IIWadDataSourceAdapter, IConfigurationDataSourceAdapter, IStatsDataSourceAdapter
    {
        private static string[] s_opLookup = new string[] { "= ", "<>", "<", ">", "like" };
        private static string[] s_typeLookup = new string[] { "like", "like", "like", "=" };

        public DbDataSourceAdapter(IDatabaseAdapter dbAdapter, string connectionString)
        {
            this.DbAdapter = dbAdapter;
            this.ConnectionString = connectionString;
            this.DataAccess = new DoomLauncher.DataAccess(dbAdapter, connectionString);
        }

        private static object CheckDBNull(object obj, object defaultValue)
        {
            if (obj == DBNull.Value)
            {
                return defaultValue;
            }
            return obj;
        }

        public void DeleteFile(IFileDataSource ds)
        {
            this.DataAccess.ExecuteNonQuery($"delete from Files where FileID = {ds.FileID}");
        }

        public void DeleteFile(IGameFileDataSource ds)
        {
            this.DataAccess.ExecuteNonQuery($"delete from Files where GameFileID = {ds.GameFileID.Value}");
        }

        public void DeleteGameFile(IGameFileDataSource ds)
        {
            if (ds.GameFileID.HasValue)
            {
                this.DataAccess.ExecuteNonQuery($"delete from GameFiles where GameFileID = {ds.GameFileID}");
            }
        }

        public void DeleteIWad(IIWadDataSource ds)
        {
            this.DataAccess.ExecuteNonQuery($"delete from IWads where IWadID = {ds.IWadID}");
        }

        public void DeleteSourcePort(ISourcePortDataSource ds)
        {
            this.DataAccess.ExecuteNonQuery($"delete from SourcePorts where SourcePortID = {ds.SourcePortID}");
        }

        public void DeleteStats(int statID)
        {
            this.DataAccess.ExecuteNonQuery($"delete from Stats where StatID = {statID}");
        }

        public void DeleteStatsByFile(int gameFileID)
        {
            this.DataAccess.ExecuteNonQuery($"delete from Stats where GameFileID = {gameFileID}");
        }

        public void DeleteTag(ITagDataSource ds)
        {
            this.DataAccess.ExecuteNonQuery($"delete from Tags where TagID = {ds.TagID}");
        }

        public void DeleteTagMapping(ITagMappingDataSource ds)
        {
            this.DataAccess.ExecuteNonQuery($"delete from TagMapping where TagID = {ds.TagID} and FileID = {ds.FileID}");
        }

        public void DeleteTagMapping(int tagID)
        {
            this.DataAccess.ExecuteNonQuery($"delete from TagMapping where TagID = {tagID}");
        }

        public IEnumerable<IConfigurationDataSource> GetConfiguration() => 
            ((IEnumerable<IConfigurationDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect("select * from Configuration").Tables[0], typeof(ConfigurationDataSource)).Cast<ConfigurationDataSource>().ToList<ConfigurationDataSource>());

        public IEnumerable<IFileDataSource> GetFiles(IGameFileDataSource gameFile) => 
            ((IEnumerable<IFileDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect($"select * from Files where GameFileID = {gameFile.GameFileID.Value} order by FileOrder, FileID").Tables[0], typeof(FileDataSource)).Cast<FileDataSource>().ToList<FileDataSource>());

        public IEnumerable<IFileDataSource> GetFiles(IGameFileDataSource gameFile, FileType fileTypeID) => 
            ((IEnumerable<IFileDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect($"select * from Files where GameFileID = {gameFile.GameFileID.Value} and FileTypeID = {(int) fileTypeID} order by FileOrder, FileID").Tables[0], typeof(FileDataSource)).Cast<FileDataSource>().ToList<FileDataSource>());

        public IGameFileDataSource GetGameFile(string fileName)
        {
            List<DbParameter> parameters = new List<DbParameter> {
                this.DataAccess.DbAdapter.CreateParameter("FileName", fileName)
            };
            DataTable dt = this.DataAccess.ExecuteSelect("select * from GameFiles where Filename = @FileName", parameters).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Util.TableToStructure(dt, typeof(GameFileDataSource)).Cast<GameFileDataSource>().ToList<GameFileDataSource>()[0];
            }
            return null;
        }

        public IEnumerable<IGameFileDataSource> GetGameFileIWads() => 
            ((IEnumerable<IGameFileDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect("select GameFiles.* from GameFiles join IWads on IWads.GameFileID = GameFiles.GameFileID").Tables[0], typeof(GameFileDataSource)).Cast<GameFileDataSource>());

        public IEnumerable<string> GetGameFileNames()
        {
            DataTable table1 = this.DataAccess.ExecuteSelect("select FileName from GameFiles").Tables[0];
            List<string> list = new List<string>(table1.Rows.Count);
            foreach (DataRow row in table1.Rows)
            {
                list.Add((string) row[0]);
            }
            return list;
        }

        public IEnumerable<IGameFileDataSource> GetGameFiles() => 
            Util.TableToStructure(this.DataAccess.ExecuteSelect("select * from GameFiles").Tables[0], typeof(GameFileDataSource)).Cast<IGameFileDataSource>();

        public IEnumerable<IGameFileDataSource> GetGameFiles(IGameFileGetOptions options) => 
            this.GetGameFiles(options, null);

        public IEnumerable<IGameFileDataSource> GetGameFiles(ITagDataSource tag) => 
            Util.TableToStructure(this.DataAccess.ExecuteSelect($"select Files.* from GameFiles join TagMapping on TagMapping.FileID = GameFiles.GameFileID where TagID = {tag.TagID}").Tables[0], typeof(GameFileDataSource)).Cast<IGameFileDataSource>();

        public IEnumerable<IGameFileDataSource> GetGameFiles(IGameFileGetOptions options, ITagDataSource tag)
        {
            DataTable table;
            string selectFieldString = "GameFiles.*";
            string str2 = string.Empty;
            string str3 = string.Empty;
            if (tag != null)
            {
                str2 = "join TagMapping on TagMapping.FileID = GameFiles.GameFileID";
                str3 = $"TagMapping.TagID = {tag.TagID}";
            }
            if (options.SelectFields != null)
            {
                selectFieldString = this.GetSelectFieldString(options.SelectFields);
            }
            if (options.SearchField != null)
            {
                string str4 = s_opLookup[(int) options.SearchField.SearchOp];
                if (str4 == "like")
                {
                    options.SearchField.SearchText = string.Format("{0}{1}{0}", "%", options.SearchField.SearchText);
                }
                string str5 = options.SearchField.SearchFieldType.ToString("g");
                string str6 = "@search";
                if ((this.DataAccess.DbAdapter is SqliteDatabaseAdapter) && GameFileSearchField.IsDateTimeField(options.SearchField.SearchFieldType))
                {
                    str6 = $"Datetime('{DateTime.Parse(options.SearchField.SearchText).ToString("yyyy-MM-dd")}')";
                }
                if (str3 != string.Empty)
                {
                    str3 = $"and {str3}";
                }
                string sql = string.Format("select {2} from GameFiles {5} where {0} {1} {3} {4} {6}", new object[] { str5, str4, selectFieldString, str6, GetLimitOrderString(options), str2, str3 });
                DbParameter[] parameters = new DbParameter[] { this.DataAccess.DbAdapter.CreateParameter("search", options.SearchField.SearchText) };
                table = this.DataAccess.ExecuteSelect(sql, parameters).Tables[0];
            }
            else
            {
                if (str3 != string.Empty)
                {
                    str3 = $"where {str3}";
                }
                string str8 = string.Format("select {0} from GameFiles {2} {3} {1}", new object[] { selectFieldString, GetLimitOrderString(options), str2, str3 });
                table = this.DataAccess.ExecuteSelect(str8).Tables[0];
            }
            return Util.TableToStructure(table, typeof(GameFileDataSource)).Cast<IGameFileDataSource>();
        }

        public int GetGameFilesCount() => 
            Convert.ToInt32(this.DataAccess.ExecuteSelect("select count(*) from GameFiles").Tables[0].Rows[0][0]);

        public IIWadDataSource GetIWad(int gameFileID) => 
            Util.TableToStructure(this.DataAccess.ExecuteSelect($"select * from IWads where GameFileID = {gameFileID} order by Name collate nocase").Tables[0], typeof(IWadDataSource)).Cast<IWadDataSource>().FirstOrDefault<IWadDataSource>();

        public IEnumerable<IIWadDataSource> GetIWads() => 
            ((IEnumerable<IIWadDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect("select * from IWads order by Name collate nocase").Tables[0], typeof(IWadDataSource)).Cast<IWadDataSource>().ToList<IWadDataSource>());

        private static string GetLimitOrderString(IGameFileGetOptions options)
        {
            string str = string.Empty;
            if (options.OrderBy.HasValue && options.OrderField.HasValue)
            {
                str = str + $"order by {options.OrderField.Value.ToString("g")} {options.OrderBy.Value.ToString("g")}";
            }
            if (options.Limit.HasValue)
            {
                str = str + $" limit {options.Limit.Value}";
            }
            return str;
        }

        private string GetSelectFieldString(GameFileFieldType[] selectFields)
        {
            StringBuilder builder = new StringBuilder();
            foreach (GameFileFieldType type in selectFields)
            {
                builder.Append(type.ToString("g"));
                builder.Append(',');
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public ISourcePortDataSource GetSourcePort(int sourcePortID) => 
            (Util.TableToStructure(this.DataAccess.ExecuteSelect($"select * from SourcePorts where SourcePortID = {sourcePortID}").Tables[0], typeof(SourcePortDataSource)).First<object>() as SourcePortDataSource);

        private List<DbParameter> GetSourcePortParams(ISourcePortDataSource ds) => 
            new List<DbParameter> { 
                this.DataAccess.DbAdapter.CreateParameter("Name", (ds.Name == null) ? string.Empty : ds.Name),
                this.DataAccess.DbAdapter.CreateParameter("Executable", (ds.Executable == null) ? string.Empty : ds.Executable),
                this.DataAccess.DbAdapter.CreateParameter("SupportedExtensions", (ds.SupportedExtensions == null) ? string.Empty : ds.SupportedExtensions),
                this.DataAccess.DbAdapter.CreateParameter("Directory", (ds.Directory == null) ? string.Empty : ds.Directory.GetPossiblyRelativePath()),
                this.DataAccess.DbAdapter.CreateParameter("SettingsFiles", (ds.SettingsFiles == null) ? string.Empty : ds.SettingsFiles),
                this.DataAccess.DbAdapter.CreateParameter("SourcePortID", ds.SourcePortID)
            };

        public IEnumerable<ISourcePortDataSource> GetSourcePorts()
        {
            DataTable table = this.DataAccess.ExecuteSelect("select * from SourcePorts order by Name collate nocase").Tables[0];
            List<ISourcePortDataSource> list = new List<ISourcePortDataSource>();
            foreach (DataRow row in table.Rows)
            {
                SourcePortDataSource item = new SourcePortDataSource {
                    Directory = new LauncherPath((string) row["Directory"]),
                    Executable = (string) row["Executable"],
                    Name = (string) row["Name"]
                };
                if (table.Columns.Contains("SettingsFiles"))
                {
                    item.SettingsFiles = (string) CheckDBNull(row["SettingsFiles"], string.Empty);
                }
                item.SourcePortID = Convert.ToInt32(row["SourcePortID"]);
                item.SupportedExtensions = (string) CheckDBNull(row["SupportedExtensions"], string.Empty);
                list.Add(item);
            }
            return list;
        }

        public IEnumerable<IStatsDataSource> GetStats() => 
            ((IEnumerable<IStatsDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect(string.Format("select * from Stats", new object[0])).Tables[0], typeof(StatsDataSource)).Cast<StatsDataSource>().ToList<StatsDataSource>());

        public IEnumerable<IStatsDataSource> GetStats(int gameFileID) => 
            ((IEnumerable<IStatsDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect($"select * from Stats where GameFileID = {gameFileID}").Tables[0], typeof(StatsDataSource)).Cast<StatsDataSource>().ToList<StatsDataSource>());

        public IEnumerable<ITagMappingDataSource> GetTagMappings() => 
            ((IEnumerable<ITagMappingDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect(string.Format("select * from TagMapping", new object[0])).Tables[0], typeof(TagMappingDataSource)).Cast<TagMappingDataSource>().ToList<TagMappingDataSource>());

        public IEnumerable<ITagMappingDataSource> GetTagMappings(int gameFileID) => 
            ((IEnumerable<ITagMappingDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect($"select * from TagMapping where FileID = {gameFileID}").Tables[0], typeof(TagMappingDataSource)).Cast<TagMappingDataSource>().ToList<TagMappingDataSource>());

        public IEnumerable<ITagDataSource> GetTags() => 
            ((IEnumerable<ITagDataSource>) Util.TableToStructure(this.DataAccess.ExecuteSelect("select * from Tags").Tables[0], typeof(TagDataSource)).Cast<TagDataSource>().ToList<TagDataSource>());

        public void InsertConfiguration(IConfigurationDataSource ds)
        {
            List<DbParameter> list;
            string[] exclude = new string[] { "ConfigID" };
            string sql = this.InsertStatement("Configuration", ds, exclude, out list);
            this.DataAccess.ExecuteNonQuery(sql, list);
        }

        public void InsertFile(IFileDataSource ds)
        {
            List<DbParameter> list;
            string[] exclude = new string[] { "FileID" };
            string sql = this.InsertStatement("Files", ds, exclude, out list);
            this.DataAccess.ExecuteNonQuery(sql, list);
        }

        public void InsertGameFile(IGameFileDataSource ds)
        {
            List<DbParameter> list;
            string[] exclude = new string[] { "GameFileID", "FileSizeBytes" };
            string sql = this.InsertStatement("GameFiles", ds, exclude, out list);
            this.DataAccess.ExecuteNonQuery(sql, list);
        }

        public void InsertIWad(IIWadDataSource ds)
        {
            List<DbParameter> list;
            string[] exclude = new string[] { "IWadID" };
            string sql = this.InsertStatement("IWads", ds, exclude, out list);
            this.DataAccess.ExecuteNonQuery(sql, list);
        }

        public void InsertSourcePort(ISourcePortDataSource ds)
        {
            string sql = "insert into SourcePorts (Name,Executable,SupportedExtensions,Directory,SettingsFiles) \r\n                values(@Name,@Executable,@SupportedExtensions,@Directory,@SettingsFiles)";
            this.DataAccess.ExecuteNonQuery(sql, this.GetSourcePortParams(ds));
        }

        private string InsertStatement(string tableName, object obj, out List<DbParameter> parameters) => 
            this.InsertStatement(tableName, obj, new string[0], out parameters);

        private string InsertStatement(string tableName, object obj, string[] exclude, out List<DbParameter> parameters)
        {
            StringBuilder builder = new StringBuilder("insert into ");
            builder.Append(tableName);
            builder.Append(" (");
            parameters = new List<DbParameter>();
            PropertyInfo[] infoArray = (from p in obj.GetType().GetProperties()
                where ((p.GetSetMethod() != null) && (p.GetGetMethod() != null)) && !exclude.Contains<string>(p.Name)
                select p).ToArray<PropertyInfo>();
            foreach (PropertyInfo info in infoArray)
            {
                builder.Append(info.Name);
                builder.Append(',');
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append(") values(");
            foreach (PropertyInfo info2 in infoArray)
            {
                builder.Append("@");
                builder.Append(info2.Name);
                builder.Append(',');
                object obj2 = info2.GetValue(obj);
                if (obj2 == null)
                {
                    obj2 = DBNull.Value;
                }
                parameters.Add(this.DataAccess.DbAdapter.CreateParameter(info2.Name, obj2));
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append(")");
            return builder.ToString();
        }

        public void InsertStats(IStatsDataSource ds)
        {
            List<DbParameter> list;
            string[] exclude = new string[] { "StatID", "SaveFile" };
            string sql = this.InsertStatement("Stats", ds, exclude, out list);
            this.DataAccess.ExecuteNonQuery(sql, list);
        }

        public void InsertTag(ITagDataSource ds)
        {
            List<DbParameter> list;
            string[] exclude = new string[] { "TagID" };
            string sql = this.InsertStatement("Tags", ds, exclude, out list);
            this.DataAccess.ExecuteNonQuery(sql, list);
        }

        public void InsertTagMapping(ITagMappingDataSource ds)
        {
            List<DbParameter> list;
            string sql = this.InsertStatement("TagMapping", ds, out list);
            this.DataAccess.ExecuteNonQuery(sql, list);
        }

        public void UpdateConfiguration(IConfigurationDataSource ds)
        {
            string sql = "update Configuration set \r\n            Name = @Name, Value = @Value, AvailableValues = @AvailableValues\r\n            where ConfigID = @ConfigID";
            List<DbParameter> parameters = new List<DbParameter> {
                this.DataAccess.DbAdapter.CreateParameter("Name", (ds.Name == null) ? string.Empty : ds.Name),
                this.DataAccess.DbAdapter.CreateParameter("Value", (ds.Value == null) ? string.Empty : ds.Value),
                this.DataAccess.DbAdapter.CreateParameter("AvailableValues", (ds.AvailableValues == null) ? string.Empty : ds.AvailableValues),
                this.DataAccess.DbAdapter.CreateParameter("ConfigID", ds.ConfigID)
            };
            this.DataAccess.ExecuteNonQuery(sql, parameters);
        }

        public void UpdateFile(IFileDataSource ds)
        {
            string sql = "update Files set \r\n            SourcePortID = @SourcePortID, Description = @Description, FileOrder = @FileOrder\r\n            where FileID = @FileID";
            List<DbParameter> parameters = new List<DbParameter> {
                this.DataAccess.DbAdapter.CreateParameter("SourcePortID", ds.SourcePortID),
                this.DataAccess.DbAdapter.CreateParameter("Description", ds.Description),
                this.DataAccess.DbAdapter.CreateParameter("FileID", ds.FileID),
                this.DataAccess.DbAdapter.CreateParameter("FileOrder", ds.FileOrder)
            };
            this.DataAccess.ExecuteNonQuery(sql, parameters);
        }

        public void UpdateFiles(int sourcePortID_Where, int? sourcePortID_Set)
        {
            DbParameter[] parameters = new DbParameter[] { this.DataAccess.DbAdapter.CreateParameter("id", sourcePortID_Set.HasValue ? ((object) sourcePortID_Set.Value) : ((object) DBNull.Value)) };
            this.DataAccess.ExecuteNonQuery($"update Files set SourcePortID = @id where SourcePortID = {sourcePortID_Where}", parameters);
        }

        public void UpdateGameFile(IGameFileDataSource ds)
        {
            this.UpdateGameFile(ds, null);
        }

        public void UpdateGameFile(IGameFileDataSource ds, GameFileFieldType[] updateFields)
        {
            StringBuilder builder = new StringBuilder("update GameFiles set ");
            if ((updateFields != null) && (updateFields.Length != 0))
            {
                foreach (GameFileFieldType type in updateFields)
                {
                    builder.Append(type.ToString());
                    builder.Append(" = @");
                    builder.Append(type.ToString());
                    builder.Append(",");
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append(" where GameFileID = @gameFileID");
            }
            else
            {
                builder = new StringBuilder("update GameFiles set Title = @Title, Author = @Author, ReleaseDate = @ReleaseDate,\r\n                    Description = @Description, Map = @Map, SourcePortID = @SourcePortID,\r\n                    Thumbnail = @Thumbnail, Comments = @Comments, Rating = @Rating,\r\n                    IWadID = @IWadID, LastPlayed = @LastPlayed, Downloaded = @Downloaded, \r\n                    SettingsMap = @SettingsMap, SettingsSkill = @SettingsSkill, SettingsExtraParams = @SettingsExtraParams, SettingsFiles = @SettingsFiles,\r\n                    SettingsSpecificFiles = @SettingsSpecificFiles, FileName = @FileName\r\n                    where GameFileID = @gameFileID");
            }
            List<DbParameter> parameters = new List<DbParameter> {
                this.DataAccess.DbAdapter.CreateParameter("Title", (ds.Title == null) ? ((object) DBNull.Value) : ((object) ds.Title)),
                this.DataAccess.DbAdapter.CreateParameter("Author", (ds.Author == null) ? ((object) DBNull.Value) : ((object) ds.Author)),
                this.DataAccess.DbAdapter.CreateParameter("ReleaseDate", !ds.ReleaseDate.HasValue ? ((object) DBNull.Value) : ((object) ds.ReleaseDate.Value)),
                this.DataAccess.DbAdapter.CreateParameter("Description", (ds.Description == null) ? ((object) DBNull.Value) : ((object) ds.Description)),
                this.DataAccess.DbAdapter.CreateParameter("Map", (ds.Map == null) ? ((object) DBNull.Value) : ((object) ds.Map)),
                this.DataAccess.DbAdapter.CreateParameter("SourcePortID", !ds.SourcePortID.HasValue ? ((object) DBNull.Value) : ((object) ds.SourcePortID.Value)),
                this.DataAccess.DbAdapter.CreateParameter("Thumbnail", (ds.Thumbnail == null) ? ((object) DBNull.Value) : ((object) ds.Thumbnail)),
                this.DataAccess.DbAdapter.CreateParameter("Comments", (ds.Comments == null) ? ((object) DBNull.Value) : ((object) ds.Comments)),
                this.DataAccess.DbAdapter.CreateParameter("Rating", !ds.Rating.HasValue ? ((object) DBNull.Value) : ((object) ds.Rating)),
                this.DataAccess.DbAdapter.CreateParameter("IWadID", !ds.IWadID.HasValue ? ((object) DBNull.Value) : ((object) ds.IWadID)),
                this.DataAccess.DbAdapter.CreateParameter("GameFileID", ds.GameFileID.Value),
                this.DataAccess.DbAdapter.CreateParameter("LastPlayed", !ds.LastPlayed.HasValue ? ((object) DBNull.Value) : ((object) ds.LastPlayed)),
                this.DataAccess.DbAdapter.CreateParameter("Downloaded", !ds.Downloaded.HasValue ? ((object) DBNull.Value) : ((object) ds.Downloaded)),
                this.DataAccess.DbAdapter.CreateParameter("SettingsMap", (ds.SettingsMap == null) ? ((object) DBNull.Value) : ((object) ds.SettingsMap)),
                this.DataAccess.DbAdapter.CreateParameter("SettingsSkill", (ds.SettingsSkill == null) ? ((object) DBNull.Value) : ((object) ds.SettingsSkill)),
                this.DataAccess.DbAdapter.CreateParameter("SettingsExtraParams", (ds.SettingsExtraParams == null) ? ((object) DBNull.Value) : ((object) ds.SettingsExtraParams)),
                this.DataAccess.DbAdapter.CreateParameter("SettingsFiles", (ds.SettingsFiles == null) ? ((object) DBNull.Value) : ((object) ds.SettingsFiles)),
                this.DataAccess.DbAdapter.CreateParameter("SettingsSpecificFiles", (ds.SettingsSpecificFiles == null) ? ((object) DBNull.Value) : ((object) ds.SettingsSpecificFiles)),
                this.DataAccess.DbAdapter.CreateParameter("MapCount", !ds.MapCount.HasValue ? ((object) DBNull.Value) : ((object) ds.MapCount)),
                this.DataAccess.DbAdapter.CreateParameter("MinutesPlayed", ds.MinutesPlayed),
                this.DataAccess.DbAdapter.CreateParameter("FileName", ds.FileName)
            };
            this.DataAccess.ExecuteNonQuery(builder.ToString(), parameters);
        }

        public void UpdateGameFiles(GameFileFieldType ftWhere, GameFileFieldType ftSet, object fWhere, object fSet)
        {
            List<DbParameter> parameters = new List<DbParameter> {
                this.DataAccess.DbAdapter.CreateParameter("set1", (fSet == null) ? DBNull.Value : fSet),
                this.DataAccess.DbAdapter.CreateParameter("where1", (fWhere == null) ? DBNull.Value : fWhere)
            };
            this.DataAccess.ExecuteNonQuery($"update GameFiles set {ftWhere.ToString("g")} = @set1 where {ftSet.ToString("g")} = @where1", parameters);
        }

        public void UpdateIWad(IIWadDataSource ds)
        {
            string sql = string.Format("update IWads set FileName = @FileName, Name = @Name, GameFileID = @GameFileID where IWadID = @IWadID", new object[0]);
            List<DbParameter> parameters = new List<DbParameter> {
                this.DataAccess.DbAdapter.CreateParameter("IWadID", ds.IWadID),
                this.DataAccess.DbAdapter.CreateParameter("FileName", ds.FileName),
                this.DataAccess.DbAdapter.CreateParameter("Name", ds.Name),
                this.DataAccess.DbAdapter.CreateParameter("GameFileID", ds.GameFileID.HasValue ? ((object) ds.GameFileID) : ((object) DBNull.Value))
            };
            this.DataAccess.ExecuteNonQuery(sql, parameters);
        }

        public void UpdateSourcePort(ISourcePortDataSource ds)
        {
            string sql = "update SourcePorts set \r\n            Name = @Name, Executable = @Executable, SupportedExtensions = @SupportedExtensions,\r\n            Directory = @Directory, SettingsFiles = @SettingsFiles\r\n            where SourcePortID = @sourcePortID";
            this.DataAccess.ExecuteNonQuery(sql, this.GetSourcePortParams(ds));
        }

        public void UpdateTag(ITagDataSource ds)
        {
            string sql = "update Tags set \r\n            Name = @Name, HasTab = @HasTab, HasColor = @HasColor, Color = @Color\r\n            where TagID = @TagID";
            List<DbParameter> parameters = new List<DbParameter> {
                this.DataAccess.DbAdapter.CreateParameter("Name", (ds.Name == null) ? string.Empty : ds.Name),
                this.DataAccess.DbAdapter.CreateParameter("HasTab", ds.HasTab),
                this.DataAccess.DbAdapter.CreateParameter("HasColor", ds.HasColor),
                this.DataAccess.DbAdapter.CreateParameter("Color", ds.Color.HasValue ? ((object) ds.Color) : ((object) DBNull.Value)),
                this.DataAccess.DbAdapter.CreateParameter("TagID", ds.TagID)
            };
            this.DataAccess.ExecuteNonQuery(sql, parameters);
        }

        public string ConnectionString { get; private set; }

        private DoomLauncher.DataAccess DataAccess { get; set; }

        public IDatabaseAdapter DbAdapter { get; private set; }
    }
}

