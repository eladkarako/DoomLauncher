namespace DoomLauncher
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class GameFileSearchField
    {
        private static GameFileFieldType[] s_dateTimeFields = new GameFileFieldType[] { GameFileFieldType.ReleaseDate };

        public GameFileSearchField(GameFileFieldType type, string search)
        {
            this.SearchFieldType = type;
            this.SearchText = search;
            this.SearchOp = GameFileSearchOp.Equal;
        }

        public GameFileSearchField(GameFileFieldType type, GameFileSearchOp op, string search)
        {
            this.SearchFieldType = type;
            this.SearchText = search;
            this.SearchOp = op;
        }

        public static bool IsDateTimeField(GameFileFieldType field) => 
            s_dateTimeFields.ToList<GameFileFieldType>().Contains(field);

        public GameFileFieldType SearchFieldType { get; set; }

        public GameFileSearchOp SearchOp { get; set; }

        public string SearchText { get; set; }
    }
}

