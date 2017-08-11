namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class TabHandler
    {
        private Dictionary<GameFileViewControl, Tuple<ITabView, TabPage>> m_tabLookup = new Dictionary<GameFileViewControl, Tuple<ITabView, TabPage>>();
        private List<ITabView> m_tabs = new List<ITabView>();

        public TabHandler(System.Windows.Forms.TabControl tabControl)
        {
            this.TabControl = tabControl;
        }

        public void AddTab(ITabView tab)
        {
            TabPage page = this.CreateTabPage(tab);
            this.TabControl.TabPages.Add(page);
            this.m_tabLookup.Add(tab.GameFileViewControl, new Tuple<ITabView, TabPage>(tab, page));
            this.m_tabs.Add(tab);
        }

        private TabPage CreateTabPage(ITabView tab)
        {
            Control control = tab as Control;
            TabPage page = new TabPage(tab.Title);
            if (control != null)
            {
                page.Controls.Add(control);
                control.Dock = DockStyle.Fill;
            }
            return page;
        }

        public void InsertTab(int index, ITabView tab)
        {
            TabPage tabPage = this.CreateTabPage(tab);
            this.TabControl.TabPages.Insert(index, tabPage);
            this.m_tabLookup.Add(tab.GameFileViewControl, new Tuple<ITabView, TabPage>(tab, tabPage));
            this.m_tabs.Insert(index, tab);
        }

        public void RemoveTab(ITabView tab)
        {
            if (this.m_tabLookup.ContainsKey(tab.GameFileViewControl))
            {
                Tuple<ITabView, TabPage> tuple = this.m_tabLookup[tab.GameFileViewControl];
                this.m_tabLookup.Remove(tab.GameFileViewControl);
                this.m_tabs.Remove(tuple.Item1);
                this.TabControl.TabPages.Remove(tuple.Item2);
            }
        }

        public void SetTabs(IEnumerable<ITabView> tabs)
        {
            this.m_tabLookup.Clear();
            foreach (ITabView view in tabs)
            {
                this.AddTab(view);
            }
        }

        public ITabView TabViewForControl(GameFileViewControl ctrl)
        {
            if (this.m_tabLookup.ContainsKey(ctrl))
            {
                return this.m_tabLookup[ctrl].Item1;
            }
            return null;
        }

        public void UpdateTabTitle(ITabView tab, string text)
        {
            if (this.m_tabLookup.ContainsKey(tab.GameFileViewControl))
            {
                this.m_tabLookup[tab.GameFileViewControl].Item2.Text = text;
            }
        }

        public System.Windows.Forms.TabControl TabControl { get; private set; }

        public ITabView[] TabViews =>
            this.m_tabs.ToArray();
    }
}

