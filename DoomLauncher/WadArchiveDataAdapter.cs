namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;

    internal class WadArchiveDataAdapter : IDataSourceAdapter, IGameFileDataSourceAdapter, IIWadDataSourceAdapter, IConfigurationDataSourceAdapter, IStatsDataSourceAdapter
    {
        private string m_urlFilename = "http://www.wad-archive.com/wadseeker/";
        private string m_urlMD5 = "http://www.wad-archive.com/api/latest/";

        public void DeleteFile(IFileDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(IGameFileDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void DeleteGameFile(IGameFileDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void DeleteIWad(IIWadDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void DeleteSourcePort(ISourcePortDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void DeleteStats(int statID)
        {
            throw new NotImplementedException();
        }

        public void DeleteStatsByFile(int gameFileID)
        {
            throw new NotImplementedException();
        }

        public void DeleteTag(ITagDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void DeleteTagMapping(ITagMappingDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void DeleteTagMapping(int tagID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConfigurationDataSource> GetConfiguration()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileDataSource> GetFiles(IGameFileDataSource gameFile)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileDataSource> GetFiles(IGameFileDataSource gameFile, FileType fileTypeID)
        {
            WadArchiveGameFileDataSource source = gameFile as WadArchiveGameFileDataSource;
            if (source == null)
            {
                throw new ArgumentException("Parameter gameFile must be of type WadArchiveGameFileDataSource");
            }
            List<WadArchiveFileDataSource> list = new List<WadArchiveFileDataSource>();
            if (source.screenshots != null)
            {
                foreach (KeyValuePair<string, string> pair in source.screenshots)
                {
                    WadArchiveFileDataSource item = new WadArchiveFileDataSource {
                        FileName = pair.Value
                    };
                    list.Add(item);
                }
            }
            return (IEnumerable<IFileDataSource>) list;
        }

        public IGameFileDataSource GetGameFile(string fileName)
        {
            throw new NotImplementedException();
        }

        private WadArchiveGameFileDataSource GetGameFileByName(string name) => 
            this.GetGameFileRequest(this.m_urlFilename + name);

        private WadArchiveGameFileDataSource GetGameFileFromMD5(string filePath)
        {
            MD5 md = MD5.Create();
            byte[] buffer = null;
            using (FileStream stream = System.IO.File.OpenRead(filePath))
            {
                buffer = md.ComputeHash(stream);
            }
            string str = BitConverter.ToString(buffer).Replace("-", string.Empty).ToLower();
            return this.GetGameFileRequest(this.m_urlMD5 + str);
        }

        public IEnumerable<IGameFileDataSource> GetGameFileIWads()
        {
            throw new NotSupportedException();
        }

        public IEnumerable<string> GetGameFileNames()
        {
            throw new NotImplementedException();
        }

        private WadArchiveGameFileDataSource GetGameFileRequest(string url)
        {
            WebRequest request1 = WebRequest.Create(string.Format(url, new object[0]));
            request1.Credentials = CredentialCache.DefaultCredentials;
            string str = new StreamReader(((HttpWebResponse) request1.GetResponse()).GetResponseStream()).ReadToEnd();
            if (str == "[]")
            {
                return null;
            }
            if (str.EndsWith(",\"screenshots\":[]}"))
            {
                str = str.Replace(",\"screenshots\":[]", string.Empty);
            }
            JsonSerializerSettings settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<WadArchiveGameFileDataSource>(str, settings);
        }

        public IEnumerable<IGameFileDataSource> GetGameFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameFileDataSource> GetGameFiles(IGameFileGetOptions options)
        {
            if (options.SearchField.SearchFieldType != GameFileFieldType.MD5)
            {
                throw new NotSupportedException("Only GamefileFieldType.MD5 is supported.");
            }
            return new WadArchiveGameFileDataSource[] { this.GetGameFileFromMD5(options.SearchField.SearchText) };
        }

        public IEnumerable<IGameFileDataSource> GetGameFiles(ITagDataSource tag)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameFileDataSource> GetGameFiles(IGameFileGetOptions options, ITagDataSource tag)
        {
            throw new NotImplementedException();
        }

        public int GetGameFilesCount() => 
            0;

        public IIWadDataSource GetIWad(int gameFileID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IIWadDataSource> GetIWads()
        {
            throw new NotImplementedException();
        }

        public ISourcePortDataSource GetSourcePort(int sourcePortID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISourcePortDataSource> GetSourcePorts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStatsDataSource> GetStats()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStatsDataSource> GetStats(int gameFileID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITagMappingDataSource> GetTagMappings()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITagMappingDataSource> GetTagMappings(int gameFileID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITagDataSource> GetTags()
        {
            throw new NotImplementedException();
        }

        public void InsertConfiguration(IConfigurationDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void InsertFile(IFileDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void InsertGameFile(IGameFileDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void InsertIWad(IIWadDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void InsertSourcePort(ISourcePortDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void InsertStats(IStatsDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void InsertTag(ITagDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void InsertTagMapping(ITagMappingDataSource ds)
        {
            throw new NotImplementedException();
        }

        public WadArchiveGameFileDataSource Test(string file) => 
            this.GetGameFileFromMD5(file);

        public void UpdateConfiguration(IConfigurationDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void UpdateFile(IFileDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void UpdateFiles(int sourcePortID_Where, int? sourcePortID_Set)
        {
            throw new NotImplementedException();
        }

        public void UpdateGameFile(IGameFileDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void UpdateGameFile(IGameFileDataSource ds, GameFileFieldType[] updateFields)
        {
            throw new NotImplementedException();
        }

        public void UpdateGameFiles(GameFileFieldType ftWhere, GameFileFieldType ftSet, object fWhere, object fSet)
        {
            throw new NotImplementedException();
        }

        public void UpdateIWad(IIWadDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void UpdateSourcePort(ISourcePortDataSource ds)
        {
            throw new NotImplementedException();
        }

        public void UpdateTag(ITagDataSource ds)
        {
            throw new NotImplementedException();
        }
    }
}

