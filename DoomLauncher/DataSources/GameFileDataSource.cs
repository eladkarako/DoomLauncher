namespace DoomLauncher.DataSources
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Runtime.CompilerServices;

    internal class GameFileDataSource : IGameFileDataSource
    {
        public GameFileDataSource()
        {
            string str4;
            string str5;
            string str6;
            string str7;
            string str8;
            string str9;
            string str10;
            string str11;
            this.SettingsSpecificFiles = str11 = string.Empty;
            this.SettingsFiles = str10 = str11;
            this.SettingsExtraParams = str9 = str10;
            this.SettingsSkill = str8 = str9;
            this.SettingsMap = str7 = str8;
            this.Map = str6 = str7;
            this.Comments = str5 = str6;
            this.Thumbnail = str4 = str5;
            this.FileName = this.Title = this.Author = this.Description = str4;
        }

        public override bool Equals(object obj) => 
            ((obj is IGameFileDataSource) && (((IGameFileDataSource) obj).FileName == this.FileName));

        public override int GetHashCode()
        {
            if (this.FileName != null)
            {
                return this.FileName.GetHashCode();
            }
            return 0;
        }

        public virtual string Author { get; set; }

        public string Comments { get; set; }

        public virtual string Description { get; set; }

        public DateTime? Downloaded { get; set; }

        public virtual string FileName { get; set; }

        public virtual int FileSizeBytes { get; set; }

        public int? GameFileID { get; set; }

        public int? IWadID { get; set; }

        public DateTime? LastPlayed { get; set; }

        public string Map { get; set; }

        public int? MapCount { get; set; }

        public int MinutesPlayed { get; set; }

        public virtual double? Rating { get; set; }

        public virtual DateTime? ReleaseDate { get; set; }

        public string SettingsExtraParams { get; set; }

        public string SettingsFiles { get; set; }

        public string SettingsMap { get; set; }

        public string SettingsSkill { get; set; }

        public string SettingsSpecificFiles { get; set; }

        public int? SourcePortID { get; set; }

        public string Thumbnail { get; set; }

        public virtual string Title { get; set; }
    }
}

