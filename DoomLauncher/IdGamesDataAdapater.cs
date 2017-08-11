namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;

    internal class IdGamesDataAdapater : IGameFileDataSourceAdapter
    {
        private static string[] s_queryLookup = new string[] { "action=search&type=filename&query={0}", "action=search&type=title&query={0}", "action=search&type=author&query={0}", "action=search&type=descrption&query={0}", string.Empty, "action=get&id={0}" };

        public IdGamesDataAdapater(string url, string apiPage, string mirrorUrl)
        {
            this.Url = url;
            this.ApiPage = apiPage;
            this.MirrorUrl = mirrorUrl;
        }

        public void DeleteGameFile(IGameFileDataSource ds)
        {
            throw new NotSupportedException();
        }

        private IEnumerable<IGameFileDataSource> GetFiles(string query, string itemName)
        {
            WebRequest request1 = WebRequest.Create(string.Format(this.Url + this.ApiPage + "?" + query, new object[0]));
            request1.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse) request1.GetResponse();
            StreamReader reader1 = new StreamReader(response.GetResponseStream());
            string s = reader1.ReadToEnd();
            reader1.Close();
            response.Close();
            StringReader reader = new StringReader(s);
            DataSet set = new DataSet();
            set.ReadXml(reader);
            reader.Dispose();
            if (set.Tables.Contains("warning") && (set.Tables["warning"].Rows[0]["type"].ToString() == "No Results"))
            {
                return new List<IGameFileDataSource>();
            }
            IEnumerable<IdGamesGameFileDataSource> source = Util.TableToStructure(set.Tables[itemName], typeof(IdGamesGameFileDataSource)).Cast<IdGamesGameFileDataSource>();
            foreach (IdGamesGameFileDataSource local1 in source)
            {
                local1.Description = local1.Description.Replace("<br>", "\n");
            }
            return source.Cast<IGameFileDataSource>();
        }

        public IGameFileDataSource GetGameFile(string fileName)
        {
            GameFileSearchField sf = new GameFileSearchField(GameFileFieldType.Filename, fileName);
            return this.GetGameFiles(new GameFileGetOptions(sf)).FirstOrDefault<IGameFileDataSource>();
        }

        public IEnumerable<IGameFileDataSource> GetGameFileIWads()
        {
            throw new NotSupportedException();
        }

        public IEnumerable<string> GetGameFileNames()
        {
            throw new NotSupportedException();
        }

        public IEnumerable<IGameFileDataSource> GetGameFiles() => 
            this.GetGameFiles(null);

        public IEnumerable<IGameFileDataSource> GetGameFiles(IGameFileGetOptions options)
        {
            if (options == null)
            {
                int num = ((options != null) && options.Limit.HasValue) ? options.Limit.Value : 0x19;
                return this.GetFiles($"action=latestfiles&limit={num}", "file");
            }
            if (string.IsNullOrEmpty(s_queryLookup[(int) options.SearchField.SearchFieldType]))
            {
                return new List<IGameFileDataSource>();
            }
            return this.GetFiles(string.Format(s_queryLookup[(int) options.SearchField.SearchFieldType], options.SearchField.SearchText), (options.SearchField.SearchFieldType == GameFileFieldType.GameFileID) ? "content" : "file");
        }

        public IEnumerable<IGameFileDataSource> GetGameFilesByName(string fileName)
        {
            GameFileSearchField sf = new GameFileSearchField(GameFileFieldType.Filename, fileName);
            return this.GetGameFiles(new GameFileGetOptions(sf));
        }

        public int GetGameFilesCount() => 
            0;

        public void InsertGameFile(IGameFileDataSource ds)
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

        public string ApiPage { get; set; }

        public string MirrorUrl { get; set; }

        public string Url { get; set; }
    }
}

