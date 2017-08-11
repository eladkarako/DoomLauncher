namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class IWadTabViewCtrl : LocalTabViewCtrl
    {
        private IContainer components;
        private IDataSourceAdapter m_dsAdapter;

        public IWadTabViewCtrl(object key, string title, IDataSourceAdapter adapter, GameFileFieldType[] selectFields, ITagMapLookup lookup) : base(key, title, adapter, selectFields, lookup)
        {
            this.InitializeComponent();
            this.m_dsAdapter = adapter;
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
            base.SetDataSource(this.m_dsAdapter.GetGameFileIWads());
        }

        public override void SetGameFiles(IEnumerable<GameFileSearchField> searchFields)
        {
            throw new NotSupportedException();
        }

        protected override bool FilterIWads =>
            false;

        public override bool IsSearchAllowed =>
            false;
    }
}

