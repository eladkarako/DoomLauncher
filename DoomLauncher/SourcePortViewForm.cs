namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class SourcePortViewForm : Form
    {
        private Button btnDelete;
        private Button btnEdit;
        private Button btnLaunch;
        private Button btnNew;
        private DataGridViewTextBoxColumn ColName;
        private IContainer components;
        private DataGridView dgvSourcePorts;
        private DataGridViewTextBoxColumn Directory;
        private DataGridViewTextBoxColumn Executable;
        private FlowLayoutPanel flpButtons;
        private ITabView[] m_tabViews;
        private TableLayoutPanelDB tableLayoutPanelDB1;
        private TableLayoutPanel tblMain;

        [field: CompilerGenerated]
        public event EventHandler SourcePortLaunched;

        public SourcePortViewForm()
        {
            this.InitializeComponent();
            this.dgvSourcePorts.DefaultCellStyle.NullValue = "N/A";
            this.dgvSourcePorts.RowHeadersVisible = false;
            this.dgvSourcePorts.AutoGenerateColumns = false;
            this.dgvSourcePorts.DefaultCellStyle.SelectionBackColor = Color.Gray;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((this.SelectedItem != null) && (MessageBox.Show(this, "Deleting this source port will orphan demo files and save games associated with it. Are you sure you want to continue?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK))
            {
                try
                {
                    this.DataAdapter.UpdateGameFiles(GameFileFieldType.SourcePortID, GameFileFieldType.SourcePortID, this.SelectedItem.SourcePortID, null);
                    this.DataAdapter.UpdateFiles(this.SelectedItem.SourcePortID, -1);
                    this.DataAdapter.DeleteSourcePort(this.SelectedItem);
                }
                catch (IOException)
                {
                    MessageBox.Show(this, "This source port appears to be in use and cannot be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                catch (Exception exception)
                {
                    DoomLauncher.Util.DisplayUnexpectedException(this, exception);
                }
                this.SetDataSource(this.DataAdapter.GetSourcePorts());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.HandleEdit();
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            if (this.IsInitSetup)
            {
                base.Close();
            }
            else if (this.SourcePortLaunched != null)
            {
                this.SourcePortLaunched(this, new EventArgs());
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SourcePortEditForm form = new SourcePortEditForm();
            form.Initialize(this.DataAdapter, this.m_tabViews);
            form.SetSupportedExtensions(".wad,.pk3,.pk7,.deh,.bex");
            form.StartPosition = FormStartPosition.CenterParent;
            form.StartPosition = FormStartPosition.CenterParent;
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                SourcePortDataSource ds = new SourcePortDataSource();
                form.UpdateDataSource(ds);
                this.DataAdapter.InsertSourcePort(ds);
                this.SetDataSource(this.DataAdapter.GetSourcePorts());
            }
        }

        private void dgvSourcePorts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.HandleEdit();
        }

        public void DisplayInitSetupButton(bool set)
        {
            this.IsInitSetup = true;
            if (set)
            {
                this.btnLaunch.Text = "Next";
            }
            else
            {
                this.btnLaunch.Text = "Play";
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

        private void HandleEdit()
        {
            if (this.SelectedItem != null)
            {
                SourcePortEditForm form = new SourcePortEditForm();
                form.Initialize(this.DataAdapter, this.m_tabViews);
                ISourcePortDataSource selectedItem = this.SelectedItem;
                form.SetDataSource(selectedItem);
                form.StartPosition = FormStartPosition.CenterParent;
                form.Initialize(this.DataAdapter, this.m_tabViews);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    form.UpdateDataSource(selectedItem);
                    this.DataAdapter.UpdateSourcePort(selectedItem);
                    this.SetDataSource(this.DataAdapter.GetSourcePorts());
                }
            }
        }

        public void Initialize(IDataSourceAdapter adapter, ITabView[] tabViews)
        {
            this.DataAdapter = adapter;
            this.m_tabViews = tabViews;
            this.SetDataSource(adapter.GetSourcePorts());
            this.dgvSourcePorts.Columns[this.dgvSourcePorts.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SourcePortViewForm));
            this.tblMain = new TableLayoutPanel();
            this.dgvSourcePorts = new DataGridView();
            this.ColName = new DataGridViewTextBoxColumn();
            this.Executable = new DataGridViewTextBoxColumn();
            this.Directory = new DataGridViewTextBoxColumn();
            this.tableLayoutPanelDB1 = new TableLayoutPanelDB();
            this.flpButtons = new FlowLayoutPanel();
            this.btnNew = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnLaunch = new Button();
            this.tblMain.SuspendLayout();
            ((ISupportInitialize) this.dgvSourcePorts).BeginInit();
            this.tableLayoutPanelDB1.SuspendLayout();
            this.flpButtons.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.dgvSourcePorts, 0, 0);
            this.tblMain.Controls.Add(this.tableLayoutPanelDB1, 0, 1);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0x270, 0x1ba);
            this.tblMain.TabIndex = 0;
            this.dgvSourcePorts.AllowUserToAddRows = false;
            this.dgvSourcePorts.AllowUserToDeleteRows = false;
            style.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.dgvSourcePorts.AlternatingRowsDefaultCellStyle = style;
            this.dgvSourcePorts.BackgroundColor = SystemColors.ControlLightLight;
            this.dgvSourcePorts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSourcePorts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewColumn[] dataGridViewColumns = new DataGridViewColumn[] { this.ColName, this.Executable, this.Directory };
            this.dgvSourcePorts.Columns.AddRange(dataGridViewColumns);
            this.dgvSourcePorts.Dock = DockStyle.Fill;
            this.dgvSourcePorts.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dgvSourcePorts.GridColor = SystemColors.ActiveBorder;
            this.dgvSourcePorts.Location = new Point(3, 3);
            this.dgvSourcePorts.Name = "dgvSourcePorts";
            this.dgvSourcePorts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvSourcePorts.Size = new Size(0x26a, 0x194);
            this.dgvSourcePorts.TabIndex = 0;
            this.dgvSourcePorts.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvSourcePorts_CellDoubleClick);
            this.ColName.DataPropertyName = "Name";
            this.ColName.HeaderText = "Name";
            this.ColName.Name = "ColName";
            this.Executable.DataPropertyName = "Executable";
            this.Executable.HeaderText = "Executable";
            this.Executable.Name = "Executable";
            this.Directory.DataPropertyName = "Directory";
            this.Directory.HeaderText = "Directory";
            this.Directory.Name = "Directory";
            this.tableLayoutPanelDB1.ColumnCount = 2;
            this.tableLayoutPanelDB1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanelDB1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanelDB1.Controls.Add(this.flpButtons, 0, 0);
            this.tableLayoutPanelDB1.Controls.Add(this.btnLaunch, 1, 0);
            this.tableLayoutPanelDB1.Dock = DockStyle.Fill;
            this.tableLayoutPanelDB1.Location = new Point(0, 410);
            this.tableLayoutPanelDB1.Margin = new Padding(0);
            this.tableLayoutPanelDB1.Name = "tableLayoutPanelDB1";
            this.tableLayoutPanelDB1.RowCount = 1;
            this.tableLayoutPanelDB1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanelDB1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tableLayoutPanelDB1.Size = new Size(0x270, 0x20);
            this.tableLayoutPanelDB1.TabIndex = 1;
            this.flpButtons.Controls.Add(this.btnNew);
            this.flpButtons.Controls.Add(this.btnEdit);
            this.flpButtons.Controls.Add(this.btnDelete);
            this.flpButtons.Dock = DockStyle.Fill;
            this.flpButtons.Location = new Point(0, 0);
            this.flpButtons.Margin = new Padding(0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new Size(0x138, 0x20);
            this.flpButtons.TabIndex = 2;
            this.btnNew.Location = new Point(3, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new Size(0x4b, 0x17);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new EventHandler(this.btnNew_Click);
            this.btnEdit.Location = new Point(0x54, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(0x4b, 0x17);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.btnDelete.Location = new Point(0xa5, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(0x4b, 0x17);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnLaunch.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnLaunch.Location = new Point(0x21f, 3);
            this.btnLaunch.Margin = new Padding(3, 3, 6, 3);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new Size(0x4b, 0x17);
            this.btnLaunch.TabIndex = 3;
            this.btnLaunch.Text = "Play";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new EventHandler(this.btnLaunch_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x270, 0x1ba);
            base.Controls.Add(this.tblMain);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "SourcePortViewForm";
            base.ShowIcon = false;
            this.Text = "Source Ports";
            this.tblMain.ResumeLayout(false);
            ((ISupportInitialize) this.dgvSourcePorts).EndInit();
            this.tableLayoutPanelDB1.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void SetDataSource(IEnumerable<ISourcePortDataSource> sourcePorts)
        {
            this.dgvSourcePorts.DataSource = (from item in sourcePorts select new { 
                SourcePortID = item.SourcePortID,
                Name = item.Name,
                Executable = item.Executable,
                Directory = item.Directory.GetPossiblyRelativePath(),
                SourcePort = item
            }).ToList();
        }

        public IDataSourceAdapter DataAdapter { get; set; }

        private bool IsInitSetup { get; set; }

        private ISourcePortDataSource SelectedItem
        {
            get
            {
                if (this.dgvSourcePorts.SelectedRows.Count == 0)
                {
                    return null;
                }
                object dataBoundItem = this.dgvSourcePorts.SelectedRows[0].DataBoundItem;
                return (dataBoundItem.GetType().GetProperty("SourcePort").GetValue(dataBoundItem) as ISourcePortDataSource);
            }
        }

        public ISourcePortDataSource SelectedSourcePort =>
            this.SelectedItem;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SourcePortViewForm.<>c <>9 = new SourcePortViewForm.<>c();
            public static Func<ISourcePortDataSource, <>f__AnonymousType2<int, string, string, string, ISourcePortDataSource>> <>9__11_0;

            internal <>f__AnonymousType2<int, string, string, string, ISourcePortDataSource> <SetDataSource>b__11_0(ISourcePortDataSource item) => 
                new { 
                    SourcePortID = item.SourcePortID,
                    Name = item.Name,
                    Executable = item.Executable,
                    Directory = item.Directory.GetPossiblyRelativePath(),
                    SourcePort = item
                };
        }
    }
}

