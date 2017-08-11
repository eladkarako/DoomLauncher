namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Runtime.CompilerServices;

    internal class IWadDataSource : IIWadDataSource
    {
        public override bool Equals(object obj)
        {
            IIWadDataSource source = obj as IIWadDataSource;
            return ((source != null) && (source.FileName == this.FileName));
        }

        public override int GetHashCode() => 
            this.FileName.GetHashCode();

        public string FileName { get; set; }

        public int? GameFileID { get; set; }

        public int IWadID { get; set; }

        public string Name { get; set; }
    }
}

