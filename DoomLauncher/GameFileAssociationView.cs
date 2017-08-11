namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class GameFileAssociationView : UserControl
    {
        private ToolStripMenuItem addFileToolStripMenuItem;
        private ToolStripButton btnAddFile;
        private ToolStripButton btnCopy;
        private ToolStripButton btnCopyAll;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private ToolStripButton btnMoveDown;
        private ToolStripButton btnMoveUp;
        private ToolStripButton btnOpenFile;
        private ToolStripButton btnSetFirst;
        private IContainer components;
        private ToolStripMenuItem copyAllFilesToolStripMenuItem;
        private ToolStripMenuItem copyFileToolStripMenuItem;
        private GenericFileView ctrlDemoView;
        private GenericFileView ctrlSaveGameView;
        private ScreenshotView ctrlScreenshotView;
        private StatisticsView ctrlViewStats;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem editDetailsToolStripMenuItem;
        private IGameFileDataSource m_gameFile;
        private ContextMenuStrip mnuOptions;
        private ToolStripMenuItem moveDownToolStripMenuItem;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem openFileToolStripMenuItem;
        private ToolStripMenuItem setFirstToolStripMenuItem;
        private TabControl tabControl;
        private TabPage tabPageDemos;
        private TabPage tabPageSaveGames;
        private TabPage tabPageStatistics;
        private TableLayoutPanelDB tblMain;
        private ToolStrip toolStrip1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator7;

        [field: CompilerGenerated]
        public event EventHandler FileDeleted;

        [field: CompilerGenerated]
        public event EventHandler FileOrderChanged;

        [field: CompilerGenerated]
        public event EventHandler<RequestScreenshotsEventArgs> RequestScreenshots;

        public GameFileAssociationView()
        {
            this.InitializeComponent();
            this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);
            this.ctrlScreenshotView.FileType = FileType.Screenshot;
            this.ctrlSaveGameView.FileType = FileType.SaveGame;
            this.ctrlDemoView.FileType = FileType.Demo;
            this.ctrlScreenshotView.RequestScreenshots += new EventHandler<RequestScreenshotsEventArgs>(this.CtrlScreenshotView_RequestScreenshots);
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleAdd();
        }

        private static void AddSeperator(ContextMenuStrip menu)
        {
            if ((menu.Items.Count > 0) && !(menu.Items[menu.Items.Count - 1] is ToolStripSeparator))
            {
                menu.Items.Add(new ToolStripSeparator());
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            this.HandleAdd();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            this.HandleCopy();
        }

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            this.HandleCopyAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.HandleDelete();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.HandleEdit();
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            this.SetFilePriority(false);
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            this.SetFilePriority(true);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            this.HandleView();
        }

        private void btnSetFirst_Click(object sender, EventArgs e)
        {
            this.HandleSetFirst();
        }

        private ContextMenuStrip BuildContextMenuStrip(IFileAssociationView view)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            if (view.CopyAllowed)
            {
                CreateMenuItem(menu, "Copy", new EventHandler(this.copyFileToolStripMenuItem_Click));
                CreateMenuItem(menu, "Copy All", new EventHandler(this.copyAllFilesToolStripMenuItem_Click));
            }
            if (view.DeleteAllowed)
            {
                CreateMenuItem(menu, "Delete", new EventHandler(this.deleteToolStripMenuItem_Click));
            }
            AddSeperator(menu);
            if (view.NewAllowed)
            {
                CreateMenuItem(menu, "Add File...", new EventHandler(this.addFileToolStripMenuItem_Click));
            }
            if (view.ViewAllowed)
            {
                CreateMenuItem(menu, "Open File...", new EventHandler(this.openFileToolStripMenuItem_Click));
            }
            AddSeperator(menu);
            if (view.EditAllowed)
            {
                CreateMenuItem(menu, "Edit Details...", new EventHandler(this.editDetailsToolStripMenuItem_Click));
            }
            AddSeperator(menu);
            if (view.ChangeOrderAllowed)
            {
                CreateMenuItem(menu, "Move Up", new EventHandler(this.moveUpToolStripMenuItem_Click));
                CreateMenuItem(menu, "Move Down", new EventHandler(this.moveDownToolStripMenuItem_Click));
                CreateMenuItem(menu, "Set First", new EventHandler(this.setFirstToolStripMenuItem_Click));
            }
            this.FinalizeMenu(menu);
            return menu;
        }

        private void copyAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleCopyAll();
        }

        private void copyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleCopy();
        }

        private static void CreateMenuItem(ContextMenuStrip menu, string text, EventHandler handler)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text);
            item.Click += handler;
            menu.Items.Add(item);
        }

        private void CtrlScreenshotView_RequestScreenshots(object sender, RequestScreenshotsEventArgs e)
        {
            if (this.RequestScreenshots != null)
            {
                this.RequestScreenshots(this, e);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleDelete();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void editDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleEdit();
        }

        private void FinalizeMenu(ContextMenuStrip menu)
        {
            if ((menu.Items.Count > 0) && (menu.Items[menu.Items.Count - 1] is ToolStripSeparator))
            {
                menu.Items.Remove(menu.Items[menu.Items.Count - 1]);
            }
        }

        private void HandleAdd()
        {
            if (((this.CurrentView != null) && this.CurrentView.NewAllowed) && this.CurrentView.New())
            {
                this.SetData(this.m_gameFile);
            }
        }

        private void HandleCopy()
        {
            IFileAssociationView currentView = this.CurrentView;
            if (currentView != null)
            {
                currentView.CopyToClipboard();
            }
        }

        private void HandleCopyAll()
        {
            if ((this.CurrentView != null) && this.CurrentView.CopyAllowed)
            {
                this.CurrentView.CopyAllToClipboard();
            }
        }

        private void HandleDelete()
        {
            if (((this.CurrentView != null) && this.CurrentView.DeleteAllowed) && (this.CurrentView.Delete() && (this.FileDeleted != null)))
            {
                this.FileDeleted(this, new EventArgs());
            }
        }

        private void HandleEdit()
        {
            if (((this.CurrentView != null) && this.CurrentView.EditAllowed) && this.CurrentView.Edit())
            {
                this.SetData(this.m_gameFile);
            }
        }

        private void HandleSetFirst()
        {
            if (((this.CurrentView != null) && this.CurrentView.ChangeOrderAllowed) && this.CurrentView.SetFileOrderFirst())
            {
                this.SetData(this.m_gameFile);
                if (this.FileOrderChanged != null)
                {
                    this.FileOrderChanged(this, new EventArgs());
                }
            }
        }

        private void HandleView()
        {
            if ((this.CurrentView != null) && this.CurrentView.ViewAllowed)
            {
                this.CurrentView.View();
            }
        }

        public void Initialize(IDataSourceAdapter adapter, LauncherPath screenshotDirectory, LauncherPath demoDirectory, LauncherPath savegameDirectory)
        {
            this.DataSourceAdapter = adapter;
            this.ScreenshotDirectory = screenshotDirectory;
            this.SaveGameDirectory = savegameDirectory;
            this.ctrlScreenshotView.DataSourceAdapter = this.DataSourceAdapter;
            this.ctrlScreenshotView.DataDirectory = this.ScreenshotDirectory;
            this.ctrlScreenshotView.FileType = FileType.Screenshot;
            this.ctrlScreenshotView.SetContextMenu(this.BuildContextMenuStrip(this.ctrlScreenshotView));
            this.ctrlSaveGameView.DataSourceAdapter = this.DataSourceAdapter;
            this.ctrlSaveGameView.DataDirectory = this.SaveGameDirectory;
            this.ctrlSaveGameView.FileType = FileType.SaveGame;
            this.ctrlSaveGameView.SetContextMenu(this.BuildContextMenuStrip(this.ctrlSaveGameView));
            this.ctrlDemoView.DataSourceAdapter = this.DataSourceAdapter;
            this.ctrlDemoView.DataDirectory = demoDirectory;
            this.ctrlDemoView.FileType = FileType.Demo;
            this.ctrlDemoView.SetContextMenu(this.BuildContextMenuStrip(this.ctrlDemoView));
            this.ctrlViewStats.DataSourceAdapter = this.DataSourceAdapter;
            this.ctrlViewStats.SetContextMenu(this.BuildContextMenuStrip(this.ctrlViewStats));
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(GameFileAssociationView));
            this.ctrlScreenshotView = new ScreenshotView();
            this.mnuOptions = new ContextMenuStrip(this.components);
            this.copyFileToolStripMenuItem = new ToolStripMenuItem();
            this.copyAllFilesToolStripMenuItem = new ToolStripMenuItem();
            this.deleteToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.addFileToolStripMenuItem = new ToolStripMenuItem();
            this.openFileToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.editDetailsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.moveUpToolStripMenuItem = new ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new ToolStripMenuItem();
            this.setFirstToolStripMenuItem = new ToolStripMenuItem();
            this.tblMain = new TableLayoutPanelDB();
            this.tabControl = new TabControl();
            this.tabPageDemos = new TabPage();
            this.ctrlDemoView = new GenericFileView();
            this.tabPageSaveGames = new TabPage();
            this.ctrlSaveGameView = new GenericFileView();
            this.tabPageStatistics = new TabPage();
            this.ctrlViewStats = new StatisticsView();
            this.toolStrip1 = new ToolStrip();
            this.btnCopy = new ToolStripButton();
            this.btnCopyAll = new ToolStripButton();
            this.btnDelete = new ToolStripButton();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.btnAddFile = new ToolStripButton();
            this.btnOpenFile = new ToolStripButton();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.btnEdit = new ToolStripButton();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.btnMoveUp = new ToolStripButton();
            this.btnMoveDown = new ToolStripButton();
            this.btnSetFirst = new ToolStripButton();
            TabPage page = new TabPage();
            page.SuspendLayout();
            this.mnuOptions.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageDemos.SuspendLayout();
            this.tabPageSaveGames.SuspendLayout();
            this.tabPageStatistics.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            page.Controls.Add(this.ctrlScreenshotView);
            page.Location = new Point(4, 0x16);
            page.Name = "tabPageScreenshots";
            page.Padding = new Padding(3);
            page.Size = new Size(0x2aa, 0xce);
            page.TabIndex = 0;
            page.Text = "Screenshots";
            page.UseVisualStyleBackColor = true;
            this.ctrlScreenshotView.DataDirectory = null;
            this.ctrlScreenshotView.DataSourceAdapter = null;
            this.ctrlScreenshotView.Dock = DockStyle.Fill;
            this.ctrlScreenshotView.FileType = FileType.Unknown;
            this.ctrlScreenshotView.GameFile = null;
            this.ctrlScreenshotView.Location = new Point(3, 3);
            this.ctrlScreenshotView.Name = "ctrlScreenshotView";
            this.ctrlScreenshotView.Size = new Size(0x2a4, 200);
            this.ctrlScreenshotView.TabIndex = 0;
            ToolStripItem[] toolStripItems = new ToolStripItem[] { this.copyFileToolStripMenuItem, this.copyAllFilesToolStripMenuItem, this.deleteToolStripMenuItem, this.toolStripSeparator1, this.addFileToolStripMenuItem, this.openFileToolStripMenuItem, this.toolStripSeparator2, this.editDetailsToolStripMenuItem, this.toolStripSeparator3, this.moveUpToolStripMenuItem, this.moveDownToolStripMenuItem, this.setFirstToolStripMenuItem };
            this.mnuOptions.Items.AddRange(toolStripItems);
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new Size(0x92, 220);
            this.copyFileToolStripMenuItem.Name = "copyFileToolStripMenuItem";
            this.copyFileToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.copyFileToolStripMenuItem.Text = "Copy File";
            this.copyFileToolStripMenuItem.Click += new EventHandler(this.copyFileToolStripMenuItem_Click);
            this.copyAllFilesToolStripMenuItem.Name = "copyAllFilesToolStripMenuItem";
            this.copyAllFilesToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.copyAllFilesToolStripMenuItem.Text = "Copy All Files";
            this.copyAllFilesToolStripMenuItem.Click += new EventHandler(this.copyAllFilesToolStripMenuItem_Click);
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new EventHandler(this.deleteToolStripMenuItem_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0x8e, 6);
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.addFileToolStripMenuItem.Text = "Add File...";
            this.addFileToolStripMenuItem.Click += new EventHandler(this.addFileToolStripMenuItem_Click);
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.openFileToolStripMenuItem.Text = "Open File...";
            this.openFileToolStripMenuItem.Click += new EventHandler(this.openFileToolStripMenuItem_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0x8e, 6);
            this.editDetailsToolStripMenuItem.Name = "editDetailsToolStripMenuItem";
            this.editDetailsToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.editDetailsToolStripMenuItem.Text = "Edit Details...";
            this.editDetailsToolStripMenuItem.Click += new EventHandler(this.editDetailsToolStripMenuItem_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(0x8e, 6);
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new EventHandler(this.moveUpToolStripMenuItem_Click);
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new EventHandler(this.moveDownToolStripMenuItem_Click);
            this.setFirstToolStripMenuItem.Name = "setFirstToolStripMenuItem";
            this.setFirstToolStripMenuItem.Size = new Size(0x91, 0x16);
            this.setFirstToolStripMenuItem.Text = "Set First";
            this.setFirstToolStripMenuItem.Click += new EventHandler(this.setFirstToolStripMenuItem_Click);
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.tabControl, 0, 1);
            this.tblMain.Controls.Add(this.toolStrip1, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.Size = new Size(0x2b8, 0x106);
            this.tblMain.TabIndex = 1;
            this.tabControl.Controls.Add(page);
            this.tabControl.Controls.Add(this.tabPageDemos);
            this.tabControl.Controls.Add(this.tabPageSaveGames);
            this.tabControl.Controls.Add(this.tabPageStatistics);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(3, 0x1b);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(690, 0xe8);
            this.tabControl.TabIndex = 0;
            this.tabPageDemos.Controls.Add(this.ctrlDemoView);
            this.tabPageDemos.Location = new Point(4, 0x16);
            this.tabPageDemos.Name = "tabPageDemos";
            this.tabPageDemos.Size = new Size(0x2aa, 0xce);
            this.tabPageDemos.TabIndex = 1;
            this.tabPageDemos.Text = "Demos";
            this.tabPageDemos.UseVisualStyleBackColor = true;
            this.ctrlDemoView.DataDirectory = null;
            this.ctrlDemoView.DataSourceAdapter = null;
            this.ctrlDemoView.Dock = DockStyle.Fill;
            this.ctrlDemoView.FileType = FileType.Unknown;
            this.ctrlDemoView.GameFile = null;
            this.ctrlDemoView.Location = new Point(0, 0);
            this.ctrlDemoView.Name = "ctrlDemoView";
            this.ctrlDemoView.Size = new Size(0x2aa, 0xce);
            this.ctrlDemoView.TabIndex = 0;
            this.tabPageSaveGames.Controls.Add(this.ctrlSaveGameView);
            this.tabPageSaveGames.Location = new Point(4, 0x16);
            this.tabPageSaveGames.Margin = new Padding(0);
            this.tabPageSaveGames.Name = "tabPageSaveGames";
            this.tabPageSaveGames.Size = new Size(0x2aa, 0xce);
            this.tabPageSaveGames.TabIndex = 2;
            this.tabPageSaveGames.Text = "Save Games";
            this.tabPageSaveGames.UseVisualStyleBackColor = true;
            this.ctrlSaveGameView.DataDirectory = null;
            this.ctrlSaveGameView.DataSourceAdapter = null;
            this.ctrlSaveGameView.Dock = DockStyle.Fill;
            this.ctrlSaveGameView.FileType = FileType.Unknown;
            this.ctrlSaveGameView.GameFile = null;
            this.ctrlSaveGameView.Location = new Point(0, 0);
            this.ctrlSaveGameView.Name = "ctrlSaveGameView";
            this.ctrlSaveGameView.Size = new Size(0x2aa, 0xce);
            this.ctrlSaveGameView.TabIndex = 1;
            this.tabPageStatistics.Controls.Add(this.ctrlViewStats);
            this.tabPageStatistics.Location = new Point(4, 0x16);
            this.tabPageStatistics.Name = "tabPageStatistics";
            this.tabPageStatistics.Size = new Size(0x2aa, 0xce);
            this.tabPageStatistics.TabIndex = 3;
            this.tabPageStatistics.Text = "Statistics";
            this.tabPageStatistics.UseVisualStyleBackColor = true;
            this.ctrlViewStats.DataSourceAdapter = null;
            this.ctrlViewStats.Dock = DockStyle.Fill;
            this.ctrlViewStats.GameFile = null;
            this.ctrlViewStats.Location = new Point(0, 0);
            this.ctrlViewStats.Name = "ctrlViewStats";
            this.ctrlViewStats.Size = new Size(0x2aa, 0xce);
            this.ctrlViewStats.TabIndex = 0;
            this.toolStrip1.Dock = DockStyle.None;
            ToolStripItem[] itemArray2 = new ToolStripItem[] { this.btnCopy, this.btnCopyAll, this.btnDelete, this.toolStripSeparator7, this.btnAddFile, this.btnOpenFile, this.toolStripSeparator4, this.btnEdit, this.toolStripSeparator5, this.btnMoveUp, this.btnMoveDown, this.btnSetFirst };
            this.toolStrip1.Items.AddRange(itemArray2);
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x10c, 0x18);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.btnCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnCopy.Image = (Image) manager.GetObject("btnCopy.Image");
            this.btnCopy.ImageTransparentColor = Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new Size(0x17, 0x15);
            this.btnCopy.Text = "Copy to Clipboard";
            this.btnCopy.Click += new EventHandler(this.btnCopy_Click);
            this.btnCopyAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnCopyAll.Image = (Image) manager.GetObject("btnCopyAll.Image");
            this.btnCopyAll.ImageTransparentColor = Color.Magenta;
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new Size(0x17, 0x15);
            this.btnCopyAll.Text = "Copy All to Clipboard";
            this.btnCopyAll.Click += new EventHandler(this.btnCopyAll_Click);
            this.btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = (Image) manager.GetObject("btnDelete.Image");
            this.btnDelete.ImageTransparentColor = Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(0x17, 0x15);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(6, 0x18);
            this.btnAddFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnAddFile.Image = (Image) manager.GetObject("btnAddFile.Image");
            this.btnAddFile.ImageTransparentColor = Color.Magenta;
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new Size(0x17, 0x15);
            this.btnAddFile.Text = "Add File";
            this.btnAddFile.Click += new EventHandler(this.btnAddFile_Click);
            this.btnOpenFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnOpenFile.Image = (Image) manager.GetObject("btnOpenFile.Image");
            this.btnOpenFile.ImageTransparentColor = Color.Magenta;
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new Size(0x17, 0x15);
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.Click += new EventHandler(this.btnOpenFile_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x18);
            this.btnEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnEdit.Image = (Image) manager.GetObject("btnEdit.Image");
            this.btnEdit.ImageTransparentColor = Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(0x17, 0x15);
            this.btnEdit.Text = "Edit Details";
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(6, 0x18);
            this.btnMoveUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = (Image) manager.GetObject("btnMoveUp.Image");
            this.btnMoveUp.ImageTransparentColor = Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new Size(0x17, 0x15);
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.Click += new EventHandler(this.btnMoveUp_Click);
            this.btnMoveDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = (Image) manager.GetObject("btnMoveDown.Image");
            this.btnMoveDown.ImageTransparentColor = Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new Size(0x17, 0x15);
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.Click += new EventHandler(this.btnMoveDown_Click);
            this.btnSetFirst.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnSetFirst.Image = (Image) manager.GetObject("btnSetFirst.Image");
            this.btnSetFirst.ImageTransparentColor = Color.Magenta;
            this.btnSetFirst.Name = "btnSetFirst";
            this.btnSetFirst.Size = new Size(0x17, 0x15);
            this.btnSetFirst.Text = "Set First";
            this.btnSetFirst.TextAlign = ContentAlignment.MiddleRight;
            this.btnSetFirst.Click += new EventHandler(this.btnSetFirst_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblMain);
            base.Name = "GameFileAssociationView";
            base.Size = new Size(0x2b8, 0x106);
            page.ResumeLayout(false);
            this.mnuOptions.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageDemos.ResumeLayout(false);
            this.tabPageSaveGames.ResumeLayout(false);
            this.tabPageStatistics.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetFilePriority(false);
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetFilePriority(true);
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleView();
        }

        private void SetButtonsEnabled(IFileAssociationView view)
        {
            this.btnAddFile.Enabled = view.NewAllowed;
            this.btnCopy.Enabled = this.btnCopyAll.Enabled = view.CopyAllowed;
            this.btnDelete.Enabled = view.DeleteAllowed;
            this.btnEdit.Enabled = view.EditAllowed;
            this.btnMoveDown.Enabled = this.btnMoveUp.Enabled = this.btnSetFirst.Enabled = view.ChangeOrderAllowed;
            this.btnOpenFile.Enabled = view.ViewAllowed;
        }

        public void SetData(IGameFileDataSource gameFile)
        {
            this.m_gameFile = gameFile;
            this.SetViewData(this.ctrlScreenshotView, gameFile);
            this.SetViewData(this.ctrlDemoView, gameFile);
            this.SetViewData(this.ctrlSaveGameView, gameFile);
            this.SetViewData(this.ctrlViewStats, gameFile);
        }

        private void SetFilePriority(bool up)
        {
            bool flag = false;
            if ((this.CurrentView != null) && this.CurrentView.ChangeOrderAllowed)
            {
                if (up)
                {
                    flag = this.CurrentView.MoveFileOrderUp();
                }
                else
                {
                    flag = this.CurrentView.MoveFileOrderDown();
                }
            }
            if (flag)
            {
                this.SetData(this.m_gameFile);
                if (this.FileOrderChanged != null)
                {
                    this.FileOrderChanged(this, new EventArgs());
                }
            }
        }

        private void setFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleSetFirst();
        }

        public void SetScreenshots(List<IFileDataSource> screenshots)
        {
            this.ctrlScreenshotView.SetScreenshots(screenshots);
        }

        private void SetViewData(IFileAssociationView view, IGameFileDataSource gameFile)
        {
            view.GameFile = gameFile;
            view.SetData(gameFile);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CurrentView != null)
            {
                this.SetButtonsEnabled(this.CurrentView);
            }
        }

        private IFileAssociationView CurrentView =>
            (this.tabControl.SelectedTab.Controls[0] as IFileAssociationView);

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public LauncherPath SaveGameDirectory { get; set; }

        public LauncherPath ScreenshotDirectory { get; set; }
    }
}

