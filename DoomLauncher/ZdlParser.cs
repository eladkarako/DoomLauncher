namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class ZdlParser
    {
        private List<string> m_errors = new List<string>();
        private IEnumerable<IIWadDataSource> m_iwads;
        private IEnumerable<ISourcePortDataSource> m_sourcePorts;
        private static string s_regex = "{0}=";
        private static string s_regexFull = "{0}=.*";

        public ZdlParser(IEnumerable<ISourcePortDataSource> sourcePorts, IEnumerable<IIWadDataSource> iwads)
        {
            this.m_sourcePorts = sourcePorts;
            this.m_iwads = iwads;
        }

        private string FindValue(string text, string category, string regexFull, bool ignoreCase)
        {
            Match match = new Regex(string.Format(regexFull, category), RegexOptions.Multiline).Match(text);
            if (match.Success)
            {
                return match.Value.Substring(new Regex(string.Format(s_regex, category)).Match(match.Value).Value.Length);
            }
            return string.Empty;
        }

        private int? GetIWad(string iwad)
        {
            iwad = iwad.ToLower();
            IIWadDataSource source = (from item in this.m_iwads
                where item.FileName.ToLower().Contains(iwad)
                select item).FirstOrDefault<IIWadDataSource>();
            if (source == null)
            {
                this.m_errors.Add($"Could not find IWAD - {iwad}");
            }
            if (source != null)
            {
                return new int?(source.IWadID);
            }
            return null;
        }

        private int? GetSourcePort(string port)
        {
            ISourcePortDataSource source = (from item in this.m_sourcePorts
                where item.Name == port
                select item).FirstOrDefault<ISourcePortDataSource>();
            if (source == null)
            {
                this.m_errors.Add($"Could not find Source Port - {port}");
            }
            if (source != null)
            {
                return new int?(source.SourcePortID);
            }
            return null;
        }

        public IGameFileDataSource[] Parse(string file)
        {
            this.m_errors.Clear();
            List<IGameFileDataSource> list = new List<IGameFileDataSource>();
            string text = File.ReadAllText(file).Replace("\r\n", "\n");
            if (!text.StartsWith("[zdl.save]"))
            {
                this.m_errors.Add("Not a valid zdl file");
                return new IGameFileDataSource[0];
            }
            string str2 = null;
            int num = 0;
            do
            {
                str2 = this.FindValue(text, $"file{num}", s_regexFull, true).Trim();
                num++;
                if (!string.IsNullOrEmpty(str2))
                {
                    GameFileDataSource item = new GameFileDataSource {
                        FileName = str2
                    };
                    list.Add(item);
                }
            }
            while (!string.IsNullOrEmpty(str2));
            if (list.Count > 0)
            {
                string str3 = this.FindValue(text, "skill", s_regexFull, true);
                string port = this.FindValue(text, "port", s_regexFull, true);
                string str5 = this.FindValue(text, "warp", s_regexFull, true);
                string str6 = this.FindValue(text, "iwad", s_regexFull, true);
                string str7 = this.FindValue(text, "extra", s_regexFull, true);
                IGameFileDataSource local1 = list[0];
                local1.SettingsSkill = str3;
                local1.SettingsMap = str5;
                local1.Map = str5;
                local1.SettingsExtraParams = str7;
                local1.SourcePortID = this.GetSourcePort(port);
                local1.IWadID = this.GetIWad(str6 + ".wad");
            }
            else
            {
                this.m_errors.Add("Did not contain any files (e.g. file=0)");
            }
            return list.ToArray();
        }

        public string[] Errors =>
            this.m_errors.ToArray();
    }
}

