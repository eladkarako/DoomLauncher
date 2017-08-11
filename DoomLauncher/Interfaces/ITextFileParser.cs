namespace DoomLauncher.Interfaces
{
    using System;

    public interface ITextFileParser
    {
        string Author { get; set; }

        string Description { get; set; }

        DateTime? ReleaseDate { get; set; }

        string Title { get; set; }
    }
}

