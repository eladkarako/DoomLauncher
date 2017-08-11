namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Windows.Forms;

    public class SpecificFilesForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private CheckBox chkSupported;
        private CheckedListBox clbFiles;
        private IContainer components;
        private FlowLayoutPanel flpButtons;
        private FlowLayoutPanel flpLinks;
        private LinkLabel lnkSelect;
        private LauncherPath m_directory;
        private List<IGameFileDataSource> m_gameFiles;
        private bool m_select;
        private string[] m_specificFiles = new string[0];
        private string[] m_supportedExtensions = new string[0];
        private TableLayoutPanel tblMain;

        public SpecificFilesForm()
        {
            this.InitializeComponent();
        }

        private void chkSupported_CheckedChanged(object sender, EventArgs e)
        {
            this.SetGrid();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static string[] GetSupportedFiles(string gameFileDirectory, IGameFileDataSource gameFile, string[] supportedExtensions)
        {
            List<string> list = new List<string>();
            using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(gameFileDirectory, gameFile.FileName)))
            {
                from item in archive.Entries
                    where (!string.IsNullOrEmpty(item.Name) && item.Name.Contains<char>('.')) && supportedExtensions.Any<string>(x => x.Equals(new FileInfo(item.Name).Extension, StringComparison.OrdinalIgnoreCase))
                    select item;
                using (IEnumerator<ZipArchiveEntry> enumerator = archive.Entries.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ZipArchiveEntry zae = enumerator.Current;
                        if (!string.IsNullOrEmpty(zae.Name) && supportedExtensions.Any<string>(x => x.Equals(new FileInfo(zae.Name).Extension, StringComparison.OrdinalIgnoreCase)))
                        {
                            list.Add(zae.Name);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        private void HandleAddItem(string name, bool isChecked)
        {
            if (this.chkSupported.Checked)
            {
                if (this.m_supportedExtensions.Any<string>(x => x.Equals(new FileInfo(name).Extension, StringComparison.OrdinalIgnoreCase)))
                {
                    this.clbFiles.Items.Add(name, isChecked);
                }
            }
            else
            {
                this.clbFiles.Items.Add(name, isChecked);
            }
        }

        public void Initialize(LauncherPath gameFileDirectory, IEnumerable<IGameFileDataSource> gameFiles, string[] supportedExtensions, string[] specificFiles)
        {
            if (specificFiles != null)
            {
                this.m_specificFiles = specificFiles.ToArray<string>();
            }
            this.m_supportedExtensions = supportedExtensions.ToArray<string>();
            this.m_gameFiles = gameFiles.ToList<IGameFileDataSource>();
            this.m_directory = gameFileDirectory;
            try
            {
                this.SetGrid();
            }
            catch (Exception exception)
            {
                DoomLauncher.Util.DisplayUnexpectedException(this, exception);
            }
        }

        private void InitializeComponent()
        {
            this.tblMain = new TableLayoutPanel();
            this.flpButtons = new FlowLayoutPanel();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.clbFiles = new CheckedListBox();
            this.lnkSelect = new LinkLabel();
            this.flpLinks = new FlowLayoutPanel();
            this.chkSupported = new CheckBox();
            this.tblMain.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.flpLinks.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.flpButtons, 0, 2);
            this.tblMain.Controls.Add(this.clbFiles, 0, 1);
            this.tblMain.Controls.Add(this.flpLinks, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0x11c, 0x106);
            this.tblMain.TabIndex = 0;
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = DockStyle.Fill;
            this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
            this.flpButtons.Location = new Point(0, 230);
            this.flpButtons.Margin = new Padding(0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new Size(0x11c, 0x20);
            this.flpButtons.TabIndex = 0;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xce, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x7d, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.clbFiles.Dock = DockStyle.Fill;
            this.clbFiles.FormattingEnabled = true;
            this.clbFiles.Location = new Point(3, 0x1b);
            this.clbFiles.Name = "clbFiles";
            this.clbFiles.Size = new Size(0x116, 200);
            this.clbFiles.TabIndex = 1;
            this.lnkSelect.Anchor = AnchorStyles.Left;
            this.lnkSelect.AutoSize = true;
            this.lnkSelect.Location = new Point(3, 2);
            this.lnkSelect.Name = "lnkSelect";
            this.lnkSelect.Size = new Size(0x62, 13);
            this.lnkSelect.TabIndex = 2;
            this.lnkSelect.TabStop = true;
            this.lnkSelect.Text = "Select/Unselect All";
            this.lnkSelect.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkSelect_LinkClicked);
            this.flpLinks.Controls.Add(this.lnkSelect);
            this.flpLinks.Controls.Add(this.chkSupported);
            this.flpLinks.Dock = DockStyle.Fill;
            this.flpLinks.Location = new Point(0, 4);
            this.flpLinks.Margin = new Padding(0, 4, 0, 0);
            this.flpLinks.Name = "flpLinks";
            this.flpLinks.Size = new Size(0x11c, 20);
            this.flpLinks.TabIndex = 3;
            this.chkSupported.AutoSize = true;
            this.chkSupported.Location = new Point(0x80, 0);
            this.chkSupported.Margin = new Padding(0x18, 0, 0, 0);
            this.chkSupported.Name = "chkSupported";
            this.chkSupported.Size = new Size(0x99, 0x11);
            this.chkSupported.TabIndex = 3;
            this.chkSupported.Text = "Supported Extensions Only";
            this.chkSupported.UseVisualStyleBackColor = true;
            this.chkSupported.CheckedChanged += new EventHandler(this.chkSupported_CheckedChanged);
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x11c, 0x106);
            base.Controls.Add(this.tblMain);
            base.Name = "SpecificFilesForm";
            base.ShowIcon = false;
            this.Text = "Select Files";
            this.tblMain.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            this.flpLinks.ResumeLayout(false);
            this.flpLinks.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lnkSelect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < this.clbFiles.Items.Count; i++)
            {
                this.clbFiles.SetItemChecked(i, this.m_select);
            }
            this.m_select = !this.m_select;
        }

        private void SetGrid()
        {
            this.clbFiles.Items.Clear();
            foreach (IGameFileDataSource source in this.m_gameFiles)
            {
                string path = Path.Combine(this.m_directory.GetFullPath(), source.FileName);
                if (File.Exists(path))
                {
                    using (ZipArchive archive = ZipFile.OpenRead(path))
                    {
                        if ((this.m_specificFiles == null) || (this.m_specificFiles.Length == 0))
                        {
                            IEnumerable<ZipArchiveEntry> enumerable = from item in archive.Entries
                                where (!string.IsNullOrEmpty(item.Name) && item.Name.Contains<char>('.')) && this.m_supportedExtensions.Any<string>(x => x.Equals(new FileInfo(item.Name).Extension, StringComparison.OrdinalIgnoreCase))
                                select item;
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (!string.IsNullOrEmpty(entry.Name))
                                {
                                    this.HandleAddItem(entry.Name, enumerable.Contains<ZipArchiveEntry>(entry));
                                }
                            }
                        }
                        else
                        {
                            foreach (ZipArchiveEntry entry2 in archive.Entries)
                            {
                                if (!string.IsNullOrEmpty(entry2.Name))
                                {
                                    this.HandleAddItem(entry2.Name, this.m_specificFiles.Contains<string>(entry2.Name));
                                }
                            }
                        }
                    }
                }
            }
        }

        public string[] SpecificFiles =>
            this.clbFiles.CheckedItems.Cast<string>().ToArray<string>();
    }
}

