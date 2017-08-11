namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class OptionsTabViewCtrl : LocalTabViewCtrl
    {
        private IContainer components;

        public OptionsTabViewCtrl(object key, string title, IGameFileDataSourceAdapter adapter, GameFileFieldType[] selectFields) : base(key, title, adapter, selectFields, null)
        {
            this.InitializeComponent();
        }

        public OptionsTabViewCtrl(object key, string title, IGameFileDataSourceAdapter adapter, GameFileFieldType[] selectFields, ITagMapLookup lookup) : base(key, title, adapter, selectFields, lookup)
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
        }

        public override void SetGameFiles()
        {
            if (this.Options != null)
            {
                this.SetGameFiles(null);
            }
            else
            {
                base.SetGameFiles();
            }
        }

        public override void SetGameFiles(IEnumerable<GameFileSearchField> searchFields)
        {
            if (this.Options != null)
            {
                GameFileGetOptions options = new GameFileGetOptions {
                    Limit = 0x19,
                    OrderBy = 1,
                    OrderField = 7,
                    SelectFields = base.m_selectFields
                };
                if ((searchFields != null) && (searchFields.Count<GameFileSearchField>() > 0))
                {
                    IEnumerable<IGameFileDataSource> first = new IGameFileDataSource[0];
                    foreach (GameFileSearchField field in searchFields)
                    {
                        options.SearchField = field;
                        first = first.Union<IGameFileDataSource>(base.m_adapter.GetGameFiles(options));
                    }
                    base.SetDataSource(first);
                }
                else
                {
                    base.SetDataSource(base.m_adapter.GetGameFiles(options));
                }
            }
            else
            {
                base.SetGameFiles(searchFields);
            }
        }

        public GameFileGetOptions Options { get; set; }
    }
}

