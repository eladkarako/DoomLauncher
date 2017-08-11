namespace DoomLauncher.DataSources
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Runtime.CompilerServices;

    internal class TagDataSource : ITagDataSource
    {
        public override bool Equals(object obj)
        {
            ITagDataSource source = obj as ITagDataSource;
            return ((source != null) && (source.TagID == this.TagID));
        }

        public override int GetHashCode() => 
            this.TagID;

        public int? Color { get; set; }

        public bool HasColor { get; set; }

        public bool HasTab { get; set; }

        public string Name { get; set; }

        public int TagID { get; set; }
    }
}

