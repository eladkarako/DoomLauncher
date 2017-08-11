namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TagTabView : BasicTabViewCtrl
    {
        private IContainer components;
        private IDataSourceAdapter m_tagAdapter;

        public TagTabView(object key, string title, IDataSourceAdapter adapter, GameFileFieldType[] selectFields, ITagDataSource tag) : base(key, title, adapter, selectFields)
        {
            this.InitializeComponent();
            this.TagDataSource = tag;
            this.m_tagAdapter = adapter;
        }

        public override object Clone()
        {
            TagTabView view = new TagTabView(base.m_key, base.m_title, this.m_tagAdapter, base.m_selectFields, this.TagDataSource);
            base.SetBaseCloneProperties(view);
            return view;
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
            this.Text = "TagTabView";
        }

        public override void SetGameFiles()
        {
            GameFileGetOptions options = new GameFileGetOptions {
                SelectFields = base.m_selectFields
            };
            base.SetDataSource(this.m_tagAdapter.GetGameFiles(options, this.TagDataSource));
        }

        public override void SetGameFiles(IEnumerable<GameFileSearchField> searchFields)
        {
            IEnumerable<IGameFileDataSource> first = new IGameFileDataSource[0];
            foreach (GameFileSearchField field in searchFields)
            {
                first = first.Union<IGameFileDataSource>(this.m_tagAdapter.GetGameFiles(new GameFileGetOptions(base.m_selectFields, field), this.TagDataSource));
            }
            base.SetDataSource(first);
        }

        public ITagDataSource TagDataSource { get; private set; }
    }
}

