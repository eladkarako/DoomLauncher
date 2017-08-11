namespace DoomLauncher.Interfaces
{
    using System;

    public interface IGameFileDataSource
    {
        string Author { get; set; }

        string Comments { get; set; }

        string Description { get; set; }

        DateTime? Downloaded { get; set; }

        string FileName { get; set; }

        int FileSizeBytes { get; set; }

        int? GameFileID { get; set; }

        int? IWadID { get; set; }

        DateTime? LastPlayed { get; set; }

        string Map { get; set; }

        int? MapCount { get; set; }

        int MinutesPlayed { get; set; }

        double? Rating { get; set; }

        DateTime? ReleaseDate { get; set; }

        string SettingsExtraParams { get; set; }

        string SettingsFiles { get; set; }

        string SettingsMap { get; set; }

        string SettingsSkill { get; set; }

        string SettingsSpecificFiles { get; set; }

        int? SourcePortID { get; set; }

        string Thumbnail { get; set; }

        string Title { get; set; }
    }
}

