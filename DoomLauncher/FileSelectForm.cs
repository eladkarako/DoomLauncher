namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using Equin.ApplicationFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class FileSelectForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private Button btnSearch;
        private IContainer components;
        private SearchControl ctrlSearch;
        private FlowLayoutPanel flpButtons;
        private FlowLayoutPanel flpSearch;
        private Label lblText;
        private bool m_bOverrideInit;
        private bool m_multiSelect;
        private TabHandler m_tabHandler;
        private TabControl tabControl;
        private TableLayoutPanelDB tblMain;

        public FileSelectForm()
        {
            this.InitializeComponent();
            this.SetupSearchFilters();
            this.m_multiSelect = this.m_bOverrideInit = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GameFileViewControl currentGameFileControl = this.CurrentGameFileControl;
            if (currentGameFileControl != null)
            {
                this.HandleSearch(currentGameFileControl);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void HandleSearch(GameFileViewControl ctrl)
        {
            if (!this.m_bOverrideInit)
            {
                ITabView view = this.m_tabHandler.TabViewForControl(ctrl);
                if (view != null)
                {
                    if (!string.IsNullOrEmpty(this.ctrlSearch.SearchText))
                    {
                        view.SetGameFiles(DoomLauncher.Util.SearchFieldsFromSearchCtrl(this.ctrlSearch));
                    }
                    else
                    {
                        view.SetGameFiles();
                    }
                }
            }
        }

        public void Initialize(IDataSourceAdapter adapter, IEnumerable<ITabView> views)
        {
            this.DataSourceAdapter = adapter;
            this.m_tabHandler = new TabHandler(this.tabControl);
            using (IEnumerator<ITabView> enumerator = views.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ITabView tab = (ITabView) enumerator.Current.Clone();
                    tab.GameFileViewControl.MultiSelect = this.MultiSelect;
                    this.m_tabHandler.AddTab(tab);
                }
            }
        }

        public void Initialize(IDataSourceAdapter adapter, ITabView tabView, IEnumerable<IGameFileDataSource> gameFilesBind)
        {
            ITabView[] views = new ITabView[] { tabView };
            this.Initialize(adapter, views);
            this.m_tabHandler.TabViews[0].SetGameFilesData(gameFilesBind);
            this.m_bOverrideInit = true;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FileSelectForm));
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.tblMain = new TableLayoutPanelDB();
            this.flpButtons = new FlowLayoutPanel();
            this.flpSearch = new FlowLayoutPanel();
            this.ctrlSearch = new SearchControl();
            this.lblText = new Label();
            this.btnSearch = new Button();
            this.tabControl = new TabControl();
            this.tblMain.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.flpSearch.SuspendLayout();
            base.SuspendLayout();
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x1d1, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x222, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.flpButtons, 0, 2);
            this.tblMain.Controls.Add(this.flpSearch, 0, 0);
            this.tblMain.Controls.Add(this.tabControl, 0, 1);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Margin = new Padding(0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tblMain.Size = new Size(0x270, 0x1ba);
            this.tblMain.TabIndex = 0;
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = DockStyle.Fill;
            this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
            this.flpButtons.Location = new Point(0, 410);
            this.flpButtons.Margin = new Padding(0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new Size(0x270, 0x20);
            this.flpButtons.TabIndex = 1;
            this.flpSearch.Controls.Add(this.ctrlSearch);
            this.flpSearch.Controls.Add(this.lblText);
            this.flpSearch.Controls.Add(this.btnSearch);
            this.flpSearch.Dock = DockStyle.Fill;
            this.flpSearch.Location = new Point(0, 0);
            this.flpSearch.Margin = new Padding(0);
            this.flpSearch.Name = "flpSearch";
            this.flpSearch.Size = new Size(0x270, 0x20);
            this.flpSearch.TabIndex = 2;
            this.ctrlSearch.Location = new Point(6, 4);
            this.ctrlSearch.Margin = new Padding(6, 4, 3, 3);
            this.ctrlSearch.Name = "ctrlSearch";
            this.ctrlSearch.SearchText = "";
            this.ctrlSearch.Size = new Size(0x8e, 0x17);
            this.ctrlSearch.TabIndex = 2;
            this.lblText.Anchor = AnchorStyles.Left;
            this.lblText.AutoSize = true;
            this.lblText.Location = new Point(0x9a, 8);
            this.lblText.Name = "lblText";
            this.lblText.Size = new Size(0, 13);
            this.lblText.TabIndex = 4;
            this.btnSearch.BackgroundImageLayout = ImageLayout.None;
            this.btnSearch.FlatAppearance.BorderColor = Color.Silver;
            this.btnSearch.Image = (Image) manager.GetObject("btnSearch.Image");
            this.btnSearch.ImageAlign = ContentAlignment.TopCenter;
            this.btnSearch.Location = new Point(0x9d, 2);
            this.btnSearch.Margin = new Padding(0, 2, 3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(80, 0x18);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(3, 0x23);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x26a, 0x174);
            this.tabControl.TabIndex = 3;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x270, 0x1ba);
            base.Controls.Add(this.tblMain);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "FileSelectForm";
            base.ShowIcon = false;
            this.Text = "Select File";
            this.tblMain.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            this.flpSearch.ResumeLayout(false);
            this.flpSearch.PerformLayout();
            base.ResumeLayout(false);
        }

        protected override void OnShown(EventArgs e)
        {
            foreach (ITabView view in this.m_tabHandler.TabViews)
            {
                this.HandleSearch(view.GameFileViewControl);
            }
        }

        public void SetDisplayText(string text)
        {
            this.lblText.Text = text;
        }

        public void SetShowCancel(bool set)
        {
            this.btnCancel.Visible = set;
        }

        private void SetupSearchFilters()
        {
            DoomLauncher.Util.SetDefaultSearchFields(this.ctrlSearch);
        }

        public void ShowSearchControl(bool set)
        {
            this.ctrlSearch.Visible = set;
            this.btnSearch.Visible = set;
        }

        private GameFileViewControl CurrentGameFileControl
        {
            get
            {
                ITabView view = this.tabControl.SelectedTab.Controls[0] as ITabView;
                if (view != null)
                {
                    return view.GameFileViewControl;
                }
                return null;
            }
        }

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public bool MultiSelect
        {
            get => 
                this.m_multiSelect;
            set
            {
                this.m_multiSelect = value;
                if (this.m_tabHandler != null)
                {
                    ITabView[] tabViews = this.m_tabHandler.TabViews;
                    for (int i = 0; i < tabViews.Length; i++)
                    {
                        tabViews[i].GameFileViewControl.MultiSelect = this.MultiSelect;
                    }
                }
            }
        }

        public IGameFileDataSource[] SelectedFiles
        {
            get
            {
                object[] selectedItems = this.CurrentGameFileControl.SelectedItems;
                List<IGameFileDataSource> list = new List<IGameFileDataSource>(selectedItems.Length);
                foreach (object obj2 in selectedItems)
                {
                    list.Add(((ObjectView<GameFileDataSource>) obj2).Object);
                }
                return list.ToArray();
            }
        }
    }
}

