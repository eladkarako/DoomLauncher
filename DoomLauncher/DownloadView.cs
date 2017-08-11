namespace DoomLauncher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class DownloadView : UserControl
    {
        private ToolStripMenuItem cancelToolStripMenuItem;
        private IContainer components;
        private HashSet<object> m_cancelledDownloads = new HashSet<object>();
        private Dictionary<object, DownloadViewItem> m_downloadLookup = new Dictionary<object, DownloadViewItem>();
        private ContextMenuStrip menuOptions;
        private ToolStripMenuItem playToolStripMenuItem;
        private ToolStripMenuItem removeFromHistoryToolStripMenuItem;
        private static int s_rowHeight = 0x40;
        private TableLayoutPanel tblMain;

        [field: CompilerGenerated]
        public event EventHandler DownloadCancelled;

        [field: CompilerGenerated]
        public event EventHandler UserPlay;

        public DownloadView()
        {
            this.InitializeComponent();
            this.tblMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tblMain.RowStyles.Clear();
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.UpdateSize();
        }

        public void AddDownload(object key, string text)
        {
            if (this.m_cancelledDownloads.Contains(key))
            {
                this.m_cancelledDownloads.Remove(key);
            }
            if (!this.m_downloadLookup.ContainsKey(key))
            {
                DownloadViewItem item = this.CreateDownloadViewItem(text);
                this.m_downloadLookup.Add(key, item);
                this.tblMain.RowCount++;
                this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                this.tblMain.RowStyles[this.tblMain.RowStyles.Count - 2].SizeType = SizeType.Absolute;
                this.tblMain.RowStyles[this.tblMain.RowStyles.Count - 2].Height = s_rowHeight;
                this.tblMain.Controls.Add(item, 0, this.tblMain.RowStyles.Count - 2);
                this.UpdateSize();
            }
            else
            {
                DownloadViewItem control = this.m_downloadLookup[key];
                DownloadViewItem item3 = this.CreateDownloadViewItem(text);
                TableLayoutPanelCellPosition positionFromControl = this.tblMain.GetPositionFromControl(control);
                this.tblMain.Controls.Remove(control);
                this.tblMain.Controls.Add(item3, positionFromControl.Column, positionFromControl.Row);
                this.m_downloadLookup.Remove(key);
                this.m_downloadLookup.Add(key, item3);
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadViewItem sourceControl = this.menuOptions.SourceControl as DownloadViewItem;
            this.SelectedItem = sourceControl;
            if (sourceControl != null)
            {
                this.HandleItemCancel(sourceControl);
            }
        }

        private DownloadViewItem CreateDownloadViewItem(string text)
        {
            DownloadViewItem item1 = new DownloadViewItem {
                Dock = DockStyle.Fill,
                Key = text,
                DisplayText = text
            };
            item1.Cancelled += new EventHandler(this.item_Cancelled);
            item1.ContextMenuStrip = this.menuOptions;
            return item1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private DownloadViewItem GetItem(int row) => 
            (this.tblMain.GetControlFromPosition(0, row) as DownloadViewItem);

        private object GetKey(DownloadViewItem item)
        {
            foreach (KeyValuePair<object, DownloadViewItem> pair in this.m_downloadLookup)
            {
                if (pair.Value == item)
                {
                    return pair.Key;
                }
            }
            return null;
        }

        private void HandleItemCancel(DownloadViewItem item)
        {
            if (item.ProgressValue != 100)
            {
                item.ProgressValue = 0;
            }
            if (this.DownloadCancelled != null)
            {
                object key = this.GetKey(item);
                if (key != null)
                {
                    this.m_cancelledDownloads.Add(key);
                    this.DownloadCancelled(this, new EventArgs());
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tblMain = new TableLayoutPanel();
            this.menuOptions = new ContextMenuStrip(this.components);
            this.removeFromHistoryToolStripMenuItem = new ToolStripMenuItem();
            this.cancelToolStripMenuItem = new ToolStripMenuItem();
            this.playToolStripMenuItem = new ToolStripMenuItem();
            this.menuOptions.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tblMain.Size = new Size(150, 150);
            this.tblMain.TabIndex = 0;
            ToolStripItem[] toolStripItems = new ToolStripItem[] { this.removeFromHistoryToolStripMenuItem, this.cancelToolStripMenuItem, this.playToolStripMenuItem };
            this.menuOptions.Items.AddRange(toolStripItems);
            this.menuOptions.Name = "contextMenuStrip1";
            this.menuOptions.Size = new Size(0xbc, 0x5c);
            this.removeFromHistoryToolStripMenuItem.Name = "removeFromHistoryToolStripMenuItem";
            this.removeFromHistoryToolStripMenuItem.Size = new Size(0xbb, 0x16);
            this.removeFromHistoryToolStripMenuItem.Text = "Remove from History";
            this.removeFromHistoryToolStripMenuItem.Click += new EventHandler(this.removeFromHistoryToolStripMenuItem_Click);
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new Size(0xbb, 0x16);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new EventHandler(this.cancelToolStripMenuItem_Click);
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new Size(0xbb, 0x16);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new EventHandler(this.playToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblMain);
            base.Name = "DownloadView";
            this.menuOptions.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void item_Cancelled(object sender, EventArgs e)
        {
            DownloadViewItem item = sender as DownloadViewItem;
            if (item != null)
            {
                this.HandleItemCancel(item);
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadViewItem sourceControl = this.menuOptions.SourceControl as DownloadViewItem;
            this.SelectedItem = sourceControl;
            if (((sourceControl != null) && (this.UserPlay != null)) && (sourceControl.ProgressValue == 100))
            {
                this.UserPlay(this, new EventArgs());
            }
        }

        public void RemoveDownload(object key)
        {
            if (this.m_downloadLookup.ContainsKey(key))
            {
                DownloadViewItem item = this.m_downloadLookup[key];
                this.m_downloadLookup.Remove(key);
                this.RemoveDownloadRow(item);
                this.UpdateSize();
            }
        }

        private void RemoveDownloadRow(DownloadViewItem item)
        {
            int row = this.tblMain.GetRow(item);
            if (row > -1)
            {
                this.tblMain.Controls.Remove(item);
                this.tblMain.RowStyles.RemoveAt(row);
                for (int i = row + 1; i < this.tblMain.RowCount; i++)
                {
                    DownloadViewItem item2 = this.GetItem(i);
                    if (item2 != null)
                    {
                        this.tblMain.Controls.Remove(item2);
                        this.tblMain.Controls.Add(item2, 0, i - 1);
                    }
                }
            }
        }

        private void removeFromHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadViewItem sourceControl = this.menuOptions.SourceControl as DownloadViewItem;
            if (sourceControl != null)
            {
                this.RemoveDownloadRow(sourceControl);
                object key = this.GetKey(sourceControl);
                if (key != null)
                {
                    this.m_downloadLookup.Remove(key);
                }
            }
        }

        public void UpdateDownload(object key, int percent)
        {
            if (this.m_downloadLookup.ContainsKey(key))
            {
                DownloadViewItem item = this.m_downloadLookup[key];
                item.ProgressValue = percent;
                if (percent == 100)
                {
                    item.CancelVisible = false;
                }
            }
        }

        public void UpdateDownload(object key, string text)
        {
            if (this.m_downloadLookup.ContainsKey(key))
            {
                this.m_downloadLookup[key].DisplayText = text;
            }
        }

        private void UpdateSize()
        {
            this.Height = this.tblMain.RowStyles.Count * s_rowHeight;
        }

        public object[] CancelledDownloads =>
            this.m_cancelledDownloads.ToArray<object>();

        public int Height { get; set; }

        public DownloadViewItem SelectedItem { get; private set; }
    }
}

