namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using Equin.ApplicationFramework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal class GameFileViewHandler
    {
        public GameFileViewHandler(DoomLauncher.SearchControl ctrlSearch, GameFileFieldType[] selectFields)
        {
            this.SearchControl = ctrlSearch;
            this.DefaultGameFileSelectFields = selectFields;
        }

        public GameFileSearchField[] GetSelectedSearchFields() => 
            this.GetSelectedSearchFields(this.SearchControl.SearchText);

        private GameFileSearchField[] GetSelectedSearchFields(string searchText)
        {
            List<GameFileSearchField> list = new List<GameFileSearchField>();
            string[] selectedSearchFilters = this.SearchControl.GetSelectedSearchFilters();
            for (int i = 0; i < selectedSearchFilters.Length; i++)
            {
                GameFileFieldType type;
                if (Enum.TryParse<GameFileFieldType>(selectedSearchFilters[i], out type))
                {
                    list.Add(new GameFileSearchField(type, GameFileSearchOp.Like, searchText));
                }
            }
            return list.ToArray();
        }

        private void SetDataSource(GameFileViewControl ctrl, IEnumerable<IGameFileDataSource> ds)
        {
            if (ds.Count<IGameFileDataSource>() == 0)
            {
                ctrl.DataSource = null;
                ctrl.SetDisplayText("No Results Found");
            }
            else
            {
                ctrl.DataSource = new BindingListView<GameFileDataSource>(ds.Cast<GameFileDataSource>().ToList<GameFileDataSource>());
            }
        }

        private void Update(IGameFileDataSourceAdapter adapter, GameFileViewControl ctrl, GameFileGetOptions options, IEnumerable<GameFileSearchField> searchFields)
        {
            if ((searchFields == null) || (searchFields.Count<GameFileSearchField>() == 0))
            {
                if (options == null)
                {
                    this.SetDataSource(ctrl, adapter.GetGameFiles().ToList<IGameFileDataSource>());
                }
                else
                {
                    this.SetDataSource(ctrl, adapter.GetGameFiles(options).ToList<IGameFileDataSource>());
                }
            }
            else
            {
                this.UpdateBase(adapter, ctrl, options, searchFields);
            }
        }

        private void UpdateBase(IGameFileDataSourceAdapter adapter, GameFileViewControl ctrl, GameFileGetOptions options, IEnumerable<GameFileSearchField> searchFields)
        {
            IEnumerable<IGameFileDataSource> first = new IGameFileDataSource[0];
            GameFileFieldType[] defaultGameFileSelectFields = this.DefaultGameFileSelectFields;
            if (options == null)
            {
                foreach (GameFileSearchField field in searchFields)
                {
                    first = first.Union<IGameFileDataSource>(adapter.GetGameFiles(new GameFileGetOptions(defaultGameFileSelectFields, field)));
                }
            }
            else
            {
                foreach (GameFileSearchField field2 in searchFields)
                {
                    options.SearchField = field2;
                    first = first.Union<IGameFileDataSource>(adapter.GetGameFiles(options));
                }
                if (options.SearchField != null)
                {
                    first = first.Union<IGameFileDataSource>(adapter.GetGameFiles(new GameFileGetOptions(defaultGameFileSelectFields, options.SearchField)));
                }
            }
            this.SetDataSource(ctrl, first.ToList<IGameFileDataSource>());
        }

        public void UpdateGameFileView(IGameFileDataSourceAdapter adapter, GameFileViewControl ctrl)
        {
            this.Update(adapter, ctrl, null, null);
        }

        public void UpdateGameFileViewSearch(IGameFileDataSourceAdapter adapter, GameFileViewControl ctrl)
        {
            string searchText = this.SearchControl.SearchText;
            if (!string.IsNullOrEmpty(searchText.Trim()))
            {
                this.Update(adapter, ctrl, null, this.GetSelectedSearchFields(searchText));
            }
            else
            {
                this.Update(adapter, ctrl, null, null);
            }
        }

        public void UpdateGameFileViewSearch(IGameFileDataSourceAdapter adapter, GameFileViewControl ctrl, GameFileGetOptions options)
        {
            this.Update(adapter, ctrl, options, null);
        }

        public GameFileFieldType[] DefaultGameFileSelectFields { get; set; }

        public DoomLauncher.SearchControl SearchControl { get; set; }
    }
}

