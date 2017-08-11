namespace DoomLauncher
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameFileGetOptions : IGameFileGetOptions
    {
        public GameFileGetOptions()
        {
        }

        public GameFileGetOptions(GameFileSearchField sf)
        {
            this.SearchField = sf;
        }

        public GameFileGetOptions(GameFileFieldType[] selectFields)
        {
            this.SelectFields = selectFields;
        }

        public GameFileGetOptions(int limit)
        {
            this.Limit = new int?(limit);
        }

        public GameFileGetOptions(GameFileFieldType[] selectFields, GameFileSearchField searchField)
        {
            this.SelectFields = selectFields;
            this.SearchField = searchField;
        }

        public int? Limit { get; set; }

        public OrderType? OrderBy { get; set; }

        public GameFileFieldType? OrderField { get; set; }

        public GameFileSearchField SearchField { get; set; }

        public GameFileFieldType[] SelectFields { get; set; }
    }
}

