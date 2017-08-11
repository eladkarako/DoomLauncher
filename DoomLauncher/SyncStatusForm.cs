namespace DoomLauncher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SyncStatusForm : Form
    {
        private Button btnContinue;
        private Button btnSelectAll;
        private ComboBox cmbOptions;
        private IContainer components;
        private CDataGridView dgvFiles;
        private DataGridViewTextBoxColumn FileName;
        private FlowLayoutPanel flpBottom;
        private Label lblHeader;
        private DataGridViewCheckBoxColumn Selected;
        private TableLayoutPanel tblMain;

        public SyncStatusForm()
        {
            this.InitializeComponent();
            this.dgvFiles.Columns[this.dgvFiles.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvFiles.RowHeadersVisible = false;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            List<SyncFileData> dataSource = this.dgvFiles.DataSource as List<SyncFileData>;
            if (dataSource.Count > 0)
            {
                bool flag = !dataSource[0].Selected;
                using (List<SyncFileData>.Enumerator enumerator = dataSource.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.Selected = flag;
                    }
                }
                this.dgvFiles.DataSource = null;
                this.dgvFiles.DataSource = dataSource;
                this.dgvFiles.Columns[this.dgvFiles.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dgvFiles.Columns[this.dgvFiles.Columns.Count - 1].HeaderText = string.Empty;
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

        public List<string> GetSelectedFiles() => 
            (from x in this.dgvFiles.DataSource as List<SyncFileData>
                where x.Selected
                select x.FileName).ToList<string>();

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            this.tblMain = new TableLayoutPanel();
            this.btnSelectAll = new Button();
            this.flpBottom = new FlowLayoutPanel();
            this.cmbOptions = new ComboBox();
            this.btnContinue = new Button();
            this.lblHeader = new Label();
            this.dgvFiles = new CDataGridView();
            this.FileName = new DataGridViewTextBoxColumn();
            this.Selected = new DataGridViewCheckBoxColumn();
            this.tblMain.SuspendLayout();
            this.flpBottom.SuspendLayout();
            ((ISupportInitialize) this.dgvFiles).BeginInit();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.dgvFiles, 0, 1);
            this.tblMain.Controls.Add(this.btnSelectAll, 0, 2);
            this.tblMain.Controls.Add(this.flpBottom, 0, 3);
            this.tblMain.Controls.Add(this.lblHeader, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 4;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0x16d, 0x137);
            this.tblMain.TabIndex = 0;
            this.btnSelectAll.Anchor = AnchorStyles.Right;
            this.btnSelectAll.Location = new Point(0x11f, 0xfb);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new Size(0x4b, 0x17);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
            this.flpBottom.Controls.Add(this.btnContinue);
            this.flpBottom.Controls.Add(this.cmbOptions);
            this.flpBottom.Dock = DockStyle.Fill;
            this.flpBottom.FlowDirection = FlowDirection.RightToLeft;
            this.flpBottom.Location = new Point(0, 0x117);
            this.flpBottom.Margin = new Padding(0);
            this.flpBottom.Name = "flpBottom";
            this.flpBottom.Size = new Size(0x16d, 0x20);
            this.flpBottom.TabIndex = 4;
            this.cmbOptions.Anchor = AnchorStyles.None;
            this.cmbOptions.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbOptions.FormattingEnabled = true;
            this.cmbOptions.Location = new Point(0x9d, 4);
            this.cmbOptions.Margin = new Padding(3, 3, 6, 3);
            this.cmbOptions.Name = "cmbOptions";
            this.cmbOptions.Size = new Size(0x79, 0x15);
            this.cmbOptions.TabIndex = 0;
            this.btnContinue.DialogResult = DialogResult.OK;
            this.btnContinue.Location = new Point(0x11f, 3);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new Size(0x4b, 0x17);
            this.btnContinue.TabIndex = 1;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.lblHeader.Anchor = AnchorStyles.Left;
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new Point(3, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new Size(13, 13);
            this.lblHeader.TabIndex = 5;
            this.lblHeader.Text = "  ";
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.AllowUserToOrderColumns = true;
            style.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.dgvFiles.AlternatingRowsDefaultCellStyle = style;
            this.dgvFiles.BackgroundColor = SystemColors.ControlLightLight;
            this.dgvFiles.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewColumn[] dataGridViewColumns = new DataGridViewColumn[] { this.FileName, this.Selected };
            this.dgvFiles.Columns.AddRange(dataGridViewColumns);
            this.dgvFiles.Dock = DockStyle.Fill;
            this.dgvFiles.EditMode = DataGridViewEditMode.EditOnEnter;
            this.dgvFiles.GridColor = SystemColors.ActiveBorder;
            this.dgvFiles.Location = new Point(3, 0x23);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvFiles.Size = new Size(0x167, 0xd1);
            this.dgvFiles.TabIndex = 2;
            this.FileName.DataPropertyName = "FileName";
            this.FileName.HeaderText = "FileName";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.Selected.DataPropertyName = "Selected";
            this.Selected.HeaderText = "";
            this.Selected.Name = "Selected";
            base.AcceptButton = this.btnContinue;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x16d, 0x137);
            base.Controls.Add(this.tblMain);
            base.Name = "SyncStatusFile";
            base.ShowIcon = false;
            this.Text = "SyncStatusFile";
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.flpBottom.ResumeLayout(false);
            ((ISupportInitialize) this.dgvFiles).EndInit();
            base.ResumeLayout(false);
        }

        public void SetData(IEnumerable<string> files, IEnumerable<string> dropDownOptions)
        {
            List<SyncFileData> list = (from file in files
                select new SyncFileData(file) into x
                orderby x.FileName
                select x).ToList<SyncFileData>();
            this.dgvFiles.DataSource = list;
            this.cmbOptions.DataSource = dropDownOptions.ToList<string>();
        }

        public string HeaderText
        {
            set
            {
                this.lblHeader.Text = value;
            }
        }

        public int SelectedOptionIndex =>
            this.cmbOptions.SelectedIndex;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SyncStatusForm.<>c <>9 = new SyncStatusForm.<>c();
            public static Func<string, SyncFileData> <>9__3_0;
            public static Func<SyncFileData, string> <>9__3_1;
            public static Func<SyncFileData, bool> <>9__4_0;
            public static Func<SyncFileData, string> <>9__4_1;

            internal bool <GetSelectedFiles>b__4_0(SyncFileData x) => 
                x.Selected;

            internal string <GetSelectedFiles>b__4_1(SyncFileData x) => 
                x.FileName;

            internal SyncFileData <SetData>b__3_0(string file) => 
                new SyncFileData(file);

            internal string <SetData>b__3_1(SyncFileData x) => 
                x.FileName;
        }
    }
}

