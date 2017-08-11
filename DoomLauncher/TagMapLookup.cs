namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class TagMapLookup : ITagMapLookup
    {
        private IDataSourceAdapter m_adapter;
        private Dictionary<int, ITagMappingDataSource[]> m_fileTagMapping;
        private Dictionary<int, ITagDataSource> m_tags;

        public TagMapLookup(IDataSourceAdapter adapter)
        {
            this.m_adapter = adapter;
            this.Refresh();
        }

        private Dictionary<int, ITagMappingDataSource[]> BuildTagMappingDictionary(IEnumerable<ITagMappingDataSource> tagMapping)
        {
            Dictionary<int, ITagMappingDataSource[]> dictionary = new Dictionary<int, ITagMappingDataSource[]>();
            int[] numArray = (from item in tagMapping select item.FileID).Distinct<int>().ToArray<int>();
            for (int i = 0; i < numArray.Length; i++)
            {
                int file = numArray[i];
                dictionary.Add(file, (from item in tagMapping
                    where item.FileID == file
                    select item).ToArray<ITagMappingDataSource>());
            }
            return dictionary;
        }

        public ITagDataSource[] GetTags(IGameFileDataSource gameFile)
        {
            if (((gameFile != null) && gameFile.GameFileID.HasValue) && this.m_fileTagMapping.ContainsKey(gameFile.GameFileID.Value))
            {
                return (from k in this.m_fileTagMapping[gameFile.GameFileID.Value]
                    where this.m_tags.ContainsKey(k.TagID)
                    select this.m_tags[k.TagID]).ToArray<ITagDataSource>();
            }
            return new ITagDataSource[0];
        }

        public void Refresh()
        {
            IEnumerable<ITagMappingDataSource> tagMapping = from x in this.m_adapter.GetTagMappings()
                orderby x.FileID
                select x;
            this.m_fileTagMapping = this.BuildTagMappingDictionary(tagMapping);
            this.m_tags = this.m_adapter.GetTags().ToDictionary<ITagDataSource, int, ITagDataSource>(x => x.TagID, x => x);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly TagMapLookup.<>c <>9 = new TagMapLookup.<>c();
            public static Func<ITagMappingDataSource, int> <>9__4_0;
            public static Func<ITagMappingDataSource, int> <>9__5_0;
            public static Func<ITagDataSource, int> <>9__5_1;
            public static Func<ITagDataSource, ITagDataSource> <>9__5_2;

            internal int <BuildTagMappingDictionary>b__4_0(ITagMappingDataSource item) => 
                item.FileID;

            internal int <Refresh>b__5_0(ITagMappingDataSource x) => 
                x.FileID;

            internal int <Refresh>b__5_1(ITagDataSource x) => 
                x.TagID;

            internal ITagDataSource <Refresh>b__5_2(ITagDataSource x) => 
                x;
        }
    }
}

