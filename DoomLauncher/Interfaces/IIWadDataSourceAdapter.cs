namespace DoomLauncher.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IIWadDataSourceAdapter
    {
        void DeleteIWad(IIWadDataSource ds);
        IIWadDataSource GetIWad(int gameFileID);
        IEnumerable<IIWadDataSource> GetIWads();
        void InsertIWad(IIWadDataSource ds);
        void UpdateIWad(IIWadDataSource ds);
    }
}

