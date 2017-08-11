namespace DoomLauncher.DataSources
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Runtime.CompilerServices;

    internal class TagMappingDataSource : ITagMappingDataSource
    {
        public override bool Equals(object obj)
        {
            ITagMappingDataSource source = obj as ITagMappingDataSource;
            return ((source?.FileID == this.FileID) && (source.TagID == this.TagID));
        }

        public override int GetHashCode() => 
            $"{this.FileID},{this.TagID}".GetHashCode();

        public int FileID { get; set; }

        public int TagID { get; set; }
    }
}

