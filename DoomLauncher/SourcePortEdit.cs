namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    public class SourcePortEdit : UserControl
    {
        private Button btnBrowse;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private string m_directory;
        private string m_exec;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tblExec;
        private TextBox txtExec;
        private TextBox txtExtensions;
        private TextBox txtName;

        public SourcePortEdit()
        {
            this.InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                string relativeDirectory = this.GetRelativeDirectory(dialog.FileName);
                this.m_exec = this.m_directory = string.Empty;
                this.m_exec = new FileInfo(relativeDirectory).Name;
                this.m_directory = relativeDirectory.Replace(this.m_exec, string.Empty);
                this.txtExec.Text = this.m_exec;
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

        private string GetRelativeDirectory(string file)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!file.Contains(currentDirectory))
            {
                return file;
            }
            char[] separator = new char[] { Path.DirectorySeparatorChar };
            string[] first = file.Split(separator);
            char[] chArray2 = new char[] { Path.DirectorySeparatorChar };
            string[] second = currentDirectory.Split(chArray2);
            StringBuilder builder = new StringBuilder();
            foreach (string str2 in first.Except<string>(second).ToArray<string>())
            {
                builder.Append(str2);
                builder.Append(Path.DirectorySeparatorChar);
            }
            if (builder.Length > 1)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.label1 = new Label();
            this.txtName = new TextBox();
            this.tblExec = new TableLayoutPanel();
            this.txtExec = new TextBox();
            this.btnBrowse = new Button();
            this.label3 = new Label();
            this.label2 = new Label();
            this.txtExtensions = new TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tblExec.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tblExec, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtExtensions, 1, 2);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.Size = new Size(310, 0x1d7);
            this.tableLayoutPanel1.TabIndex = 1;
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            this.txtName.Dock = DockStyle.Fill;
            this.txtName.Location = new Point(0x7b, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(0xb8, 20);
            this.txtName.TabIndex = 6;
            this.tblExec.ColumnCount = 2;
            this.tblExec.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblExec.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
            this.tblExec.Controls.Add(this.txtExec, 0, 0);
            this.tblExec.Controls.Add(this.btnBrowse, 1, 0);
            this.tblExec.Dock = DockStyle.Fill;
            this.tblExec.Location = new Point(120, 0x18);
            this.tblExec.Margin = new Padding(0);
            this.tblExec.Name = "tblExec";
            this.tblExec.RowCount = 1;
            this.tblExec.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblExec.Size = new Size(190, 0x18);
            this.tblExec.TabIndex = 8;
            this.txtExec.Dock = DockStyle.Fill;
            this.txtExec.Location = new Point(3, 3);
            this.txtExec.Name = "txtExec";
            this.txtExec.Size = new Size(0x68, 20);
            this.txtExec.TabIndex = 0;
            this.btnBrowse.Location = new Point(110, 0);
            this.btnBrowse.Margin = new Padding(0);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new Size(0x42, 0x18);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
            this.label3.Anchor = AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 0x1d);
            this.label3.Name = "label3";
            this.label3.Size = new Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Executable";
            this.label2.Anchor = AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 0x35);
            this.label2.Name = "label2";
            this.label2.Size = new Size(110, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Supported Extensions";
            this.txtExtensions.Dock = DockStyle.Fill;
            this.txtExtensions.Location = new Point(0x7b, 0x33);
            this.txtExtensions.Name = "txtExtensions";
            this.txtExtensions.Size = new Size(0xb8, 20);
            this.txtExtensions.TabIndex = 7;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tableLayoutPanel1);
            base.Name = "SourcePortEdit";
            base.Size = new Size(310, 0x1d7);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tblExec.ResumeLayout(false);
            this.tblExec.PerformLayout();
            base.ResumeLayout(false);
        }

        public void SetDataSource(ISourcePortDataSource ds)
        {
            this.m_directory = ds.Directory.GetPossiblyRelativePath();
            this.m_exec = ds.Executable;
            if (!string.IsNullOrEmpty(ds.Name))
            {
                this.txtName.Text = ds.Name;
            }
            if ((ds.Directory != null) && (ds.Executable != null))
            {
                this.txtExec.Text = ds.Executable;
            }
            if (!string.IsNullOrEmpty(ds.SupportedExtensions))
            {
                this.txtExtensions.Text = ds.SupportedExtensions;
            }
        }

        public void SetSupportedExtensions(string text)
        {
            this.txtExtensions.Text = text;
        }

        public void UpdateDataSource(ISourcePortDataSource ds)
        {
            ds.Name = this.txtName.Text;
            ds.Directory = new LauncherPath(this.m_directory);
            ds.Executable = this.m_exec;
            ds.SupportedExtensions = this.txtExtensions.Text;
        }

        public string SourcePortExec =>
            this.txtExec.Text;

        public string SourcePortName =>
            this.txtName.Text;
    }
}

