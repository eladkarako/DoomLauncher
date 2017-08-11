namespace DoomLauncher
{
    using System;

    public interface IGameFileGetOptions
    {
        int? Limit { get; set; }

        OrderType? OrderBy { get; set; }

        GameFileFieldType? OrderField { get; set; }

        GameFileSearchField SearchField { get; set; }

        GameFileFieldType[] SelectFields { get; set; }
    }
}

