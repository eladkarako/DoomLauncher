namespace DoomLauncher
{
    using System;
    using System.Runtime.CompilerServices;

    public class ColumnConfig
    {
        public ColumnConfig()
        {
        }

        public ColumnConfig(string parent, string column, int width)
        {
            this.Parent = parent;
            this.Column = column;
            this.Width = width;
        }

        public string Column { get; set; }

        public string Parent { get; set; }

        public int Width { get; set; }
    }
}

