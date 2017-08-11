namespace DoomLauncher.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IConfigurationDataSourceAdapter
    {
        void DeleteTag(ITagDataSource ds);
        void DeleteTagMapping(ITagMappingDataSource ds);
        void DeleteTagMapping(int tagID);
        IEnumerable<IConfigurationDataSource> GetConfiguration();
        IEnumerable<ITagMappingDataSource> GetTagMappings();
        IEnumerable<ITagMappingDataSource> GetTagMappings(int gameFileID);
        IEnumerable<ITagDataSource> GetTags();
        void InsertConfiguration(IConfigurationDataSource ds);
        void InsertTag(ITagDataSource ds);
        void InsertTagMapping(ITagMappingDataSource ds);
        void UpdateConfiguration(IConfigurationDataSource ds);
        void UpdateTag(ITagDataSource ds);
    }
}

