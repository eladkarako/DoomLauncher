namespace DoomLauncher.Interfaces
{
    using System;

    public interface ITagDataSource
    {
        int? Color { get; set; }

        bool HasColor { get; set; }

        bool HasTab { get; set; }

        string Name { get; set; }

        int TagID { get; set; }
    }
}

