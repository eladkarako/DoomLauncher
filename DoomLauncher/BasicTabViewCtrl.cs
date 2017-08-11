namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using Equin.ApplicationFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    public class BasicTabViewCtrl : UserControl, ITabView, ICloneable
    {
        private IContainer components;
        private DoomLauncher.GameFileViewControl ctrlView;
        protected IGameFileDataSourceAdapter m_adapter;
        protected object m_key;
        protected GameFileFieldType[] m_selectFields;
        protected string m_title;

        public BasicTabViewCtrl(object key, string title, IGameFileDataSourceAdapter adapter, GameFileFieldType[] selectFields)
        {
            this.InitializeComponent();
            this.m_key = key;
            this.m_title = title;
            this.m_adapter = adapter;
            this.m_selectFields = selectFields.ToArray<GameFileFieldType>();
        }

        public virtual object Clone()
        {
            BasicTabViewCtrl view = new BasicTabViewCtrl(this.m_key, this.m_title, this.m_adapter, this.m_selectFields);
            this.SetBaseCloneProperties(view);
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

        protected IGameFileDataSource FromDataBoundItem(object item) => 
            ((ObjectView<GameFileDataSource>) item).Object;

        private void InitializeComponent()
        {
            this.ctrlView = new DoomLauncher.GameFileViewControl();
            base.SuspendLayout();
            this.ctrlView.DataSource = null;
            this.ctrlView.Dock = DockStyle.Fill;
            this.ctrlView.Location = new Point(0, 0);
            this.ctrlView.Name = "ctrlView";
            this.ctrlView.SelectedItem = null;
            this.ctrlView.Size = new Size(150, 150);
            this.ctrlView.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.ctrlView);
            base.Name = "BasicTabViewCtrl";
            base.ResumeLayout(false);
        }

        protected void SetBaseCloneProperties(ITabView view)
        {
            string[] columnKeyOrder = this.GameFileViewControl.GetColumnKeyOrder();
            Tuple<string, string>[] columnFields = this.GameFileViewControl.ColumnFields;
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            string[] strArray2 = columnKeyOrder;
            for (int i = 0; i < strArray2.Length; i++)
            {
                string key = strArray2[i];
                list.AddRange(from x in columnFields
                    where x.Item1.Equals(key, StringComparison.InvariantCultureIgnoreCase)
                    select x);
            }
            view.GameFileViewControl.SetColumnFields(list);
            foreach (string str in columnKeyOrder)
            {
                view.GameFileViewControl.SetColumnWidth(str, this.GameFileViewControl.GetColumnWidth(str));
            }
            foreach (Tuple<string, string> tuple in this.GameFileViewControl.GetColumnFormats())
            {
                view.GameFileViewControl.SetColumnFormat(tuple.Item1, tuple.Item2);
            }
        }

        protected void SetDataSource(IEnumerable<IGameFileDataSource> ds)
        {
            if (this.FilterIWads)
            {
                ds = ds.Except<IGameFileDataSource>(this.m_adapter.GetGameFileIWads());
            }
            if (ds.Count<IGameFileDataSource>() == 0)
            {
                this.ctrlView.DataSource = null;
                this.ctrlView.SetDisplayText("No Results Found");
            }
            else
            {
                this.ctrlView.DataSource = new BindingListView<GameFileDataSource>(ds.Cast<GameFileDataSource>().ToList<GameFileDataSource>());
            }
        }

        protected void SetDisplayText(string text)
        {
            this.ctrlView.SetDisplayText(text);
        }

        public virtual void SetGameFiles()
        {
            GameFileGetOptions options = new GameFileGetOptions {
                SelectFields = this.m_selectFields
            };
            this.SetDataSource(this.m_adapter.GetGameFiles(options));
        }

        public virtual void SetGameFiles(IEnumerable<GameFileSearchField> searchFields)
        {
            IEnumerable<IGameFileDataSource> first = new IGameFileDataSource[0];
            foreach (GameFileSearchField field in searchFields)
            {
                first = first.Union<IGameFileDataSource>(this.m_adapter.GetGameFiles(new GameFileGetOptions(this.m_selectFields, field)));
            }
            this.SetDataSource(first);
        }

        public void SetGameFilesData(IEnumerable<IGameFileDataSource> gameFiles)
        {
            this.SetDataSource(gameFiles);
        }

        public virtual void UpdateDataSourceFile(IGameFileDataSource ds)
        {
            if (this.ctrlView.DataSource != null)
            {
                foreach (ObjectView<GameFileDataSource> view in (BindingListView<GameFileDataSource>) this.ctrlView.DataSource)
                {
                    if (view.Object.Equals(ds))
                    {
                        IGameFileDataSource dsSet = view.Object;
                        Array.ForEach<PropertyInfo>(dsSet.GetType().GetProperties(), x => x.SetValue(dsSet, x.GetValue(ds)));
                        this.ctrlView.Invalidate(true);
                        break;
                    }
                }
            }
        }

        public virtual IGameFileDataSourceAdapter Adapter
        {
            get => 
                this.m_adapter;
            set
            {
                this.m_adapter = value;
            }
        }

        protected virtual bool FilterIWads =>
            true;

        public DoomLauncher.GameFileViewControl GameFileViewControl =>
            this.ctrlView;

        public virtual bool IsAutoSearchAllowed =>
            true;

        public virtual bool IsDeleteAllowed =>
            true;

        public virtual bool IsEditAllowed =>
            true;

        public virtual bool IsLocal =>
            true;

        public virtual bool IsPlayAllowed =>
            true;

        public virtual bool IsSearchAllowed =>
            true;

        public object Key =>
            this.m_key;

        public string Title =>
            this.m_title;
    }
}

