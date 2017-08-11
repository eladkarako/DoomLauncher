namespace DoomLauncher
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class FilesCtrl : UserControl
    {
        private ToolStripButton btnAdd;
        private ToolStripButton btnDelete;
        private ToolStripButton btnMoveDown;
        private ToolStripButton btnMoveUp;
        private IContainer components;
        private DataGridView dgvAdditionalFiles;
        private DataGridViewTextBoxColumn FileName;
        private string m_dataProperty;
        private List<object> m_files = new List<object>();
        private string m_keyProperty;
        private TableLayoutPanel tblMain;
        private ToolStrip toolStrip1;

        [field: CompilerGenerated]
        public event AdditionalFilesEventHanlder CellFormatting;

        [field: CompilerGenerated]
        public event AdditionalFilesEventHanlder ItemPriorityDown;

        [field: CompilerGenerated]
        public event AdditionalFilesEventHanlder ItemPriorityUp;

        [field: CompilerGenerated]
        public event AdditionalFilesEventHanlder ItemRemoved;

        [field: CompilerGenerated]
        public event AdditionalFilesEventHanlder ItemRemoving;

        [field: CompilerGenerated]
        public event AdditionalFilesEventHanlder NewItemNeeded;

        public FilesCtrl()
        {
            this.InitializeComponent();
            this.dgvAdditionalFiles.AutoGenerateColumns = false;
            this.dgvAdditionalFiles.RowHeadersVisible = false;
            this.dgvAdditionalFiles.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvAdditionalFiles_CellFormatting);
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            AdditionalFilesEventArgs args = new AdditionalFilesEventArgs(null);
            if (this.NewItemNeeded != null)
            {
                this.NewItemNeeded(this, args);
            }
            if (args.NewItems != null)
            {
                this.m_files.AddRange(args.NewItems);
                this.Rebind();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            object selectedItem = this.SelectedItem;
            if (selectedItem != null)
            {
                AdditionalFilesEventArgs args = new AdditionalFilesEventArgs(selectedItem);
                if (this.ItemRemoving != null)
                {
                    this.ItemRemoving(this, args);
                }
                if (!args.Cancel)
                {
                    this.m_files.Remove(selectedItem);
                    this.Rebind();
                    if (this.ItemRemoved != null)
                    {
                        this.ItemRemoved(this, new AdditionalFilesEventArgs(selectedItem));
                    }
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            this.MoveFile(false);
            object selectedItem = this.SelectedItem;
            if ((selectedItem != null) && (this.ItemPriorityDown != null))
            {
                this.ItemPriorityDown(this, new AdditionalFilesEventArgs(selectedItem));
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            this.MoveFile(true);
            object selectedItem = this.SelectedItem;
            if ((selectedItem != null) && (this.ItemPriorityUp != null))
            {
                this.ItemPriorityUp(this, new AdditionalFilesEventArgs(selectedItem));
            }
        }

        private void dgvAdditionalFiles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.CellFormatting != null)
            {
                AdditionalFilesEventArgs args = new AdditionalFilesEventArgs(this.dgvAdditionalFiles.Rows[e.RowIndex].DataBoundItem, (string) e.Value);
                this.CellFormatting(this, args);
                e.Value = args.DisplayText;
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

        public string GetAdditionalFilesString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in this.m_files)
            {
                PropertyInfo property = obj2.GetType().GetProperty(this.m_dataProperty);
                builder.Append(property.GetValue(obj2).ToString());
                builder.Append(';');
            }
            return builder.ToString();
        }

        private int GetFilePriority(object file)
        {
            int num = 0;
            PropertyInfo property = file.GetType().GetProperty(this.m_keyProperty);
            foreach (object obj2 in this.m_files)
            {
                if (property.GetValue(obj2).Equals(property.GetValue(file)))
                {
                    return num;
                }
                num++;
            }
            return num;
        }

        public List<object> GetFiles() => 
            this.m_files.ToList<object>();

        public void Initialize(string keyProperty, string dataProperty)
        {
            this.dgvAdditionalFiles.Columns[0].DataPropertyName = dataProperty;
            this.dgvAdditionalFiles.Columns[0].Name = dataProperty;
            this.m_keyProperty = keyProperty;
            this.m_dataProperty = dataProperty;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FilesCtrl));
            this.tblMain = new TableLayoutPanel();
            this.dgvAdditionalFiles = new DataGridView();
            this.FileName = new DataGridViewTextBoxColumn();
            this.toolStrip1 = new ToolStrip();
            this.btnMoveUp = new ToolStripButton();
            this.btnMoveDown = new ToolStripButton();
            this.btnAdd = new ToolStripButton();
            this.btnDelete = new ToolStripButton();
            this.tblMain.SuspendLayout();
            ((ISupportInitialize) this.dgvAdditionalFiles).BeginInit();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.dgvAdditionalFiles, 0, 1);
            this.tblMain.Controls.Add(this.toolStrip1, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Margin = new Padding(0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.Size = new Size(0xf7, 0x139);
            this.tblMain.TabIndex = 0x17;
            this.dgvAdditionalFiles.AllowUserToAddRows = false;
            this.dgvAdditionalFiles.BackgroundColor = SystemColors.ControlLightLight;
            this.dgvAdditionalFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewColumn[] dataGridViewColumns = new DataGridViewColumn[] { this.FileName };
            this.dgvAdditionalFiles.Columns.AddRange(dataGridViewColumns);
            this.dgvAdditionalFiles.Dock = DockStyle.Fill;
            this.dgvAdditionalFiles.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dgvAdditionalFiles.Location = new Point(3, 0x1b);
            this.dgvAdditionalFiles.Name = "dgvAdditionalFiles";
            this.dgvAdditionalFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdditionalFiles.Size = new Size(0xf1, 0x11b);
            this.dgvAdditionalFiles.TabIndex = 2;
            this.FileName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.FileName.DataPropertyName = "FileName";
            this.FileName.HeaderText = "File";
            this.FileName.Name = "FileName";
            ToolStripItem[] toolStripItems = new ToolStripItem[] { this.btnAdd, this.btnDelete, this.btnMoveUp, this.btnMoveDown };
            this.toolStrip1.Items.AddRange(toolStripItems);
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0xf7, 0x18);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
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
            this.btnMoveDown.Text = "Move Up";
            this.btnMoveDown.Click += new EventHandler(this.btnMoveDown_Click);
            this.btnAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = (Image) manager.GetObject("btnAdd.Image");
            this.btnAdd.ImageTransparentColor = Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x17, 0x15);
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new EventHandler(this.btnAddFile_Click);
            this.btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = (Image) manager.GetObject("btnDelete.Image");
            this.btnDelete.ImageTransparentColor = Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(0x17, 0x15);
            this.btnDelete.Text = "Remove";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblMain);
            base.Name = "FilesCtrl";
            base.Size = new Size(0xf7, 0x139);
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            ((ISupportInitialize) this.dgvAdditionalFiles).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void MoveFile(bool up)
        {
            if (this.dgvAdditionalFiles.SelectedRows.Count > 0)
            {
                object dataBoundItem = this.dgvAdditionalFiles.SelectedRows[0].DataBoundItem;
                int filePriority = this.GetFilePriority(dataBoundItem);
                if (up && (filePriority > 0))
                {
                    filePriority--;
                }
                if (!up && (filePriority < (this.m_files.Count - 1)))
                {
                    filePriority++;
                }
                this.m_files.Remove(dataBoundItem);
                this.m_files.Insert(filePriority, dataBoundItem);
                this.Rebind();
                this.dgvAdditionalFiles.ClearSelection();
                this.dgvAdditionalFiles.Rows[filePriority].Selected = true;
            }
        }

        private void Rebind()
        {
            this.SetDataSource(this.m_files.ToList<object>());
        }

        public void SetDataSource(object dataSource)
        {
            this.dgvAdditionalFiles.DataSource = null;
            this.dgvAdditionalFiles.DataSource = dataSource;
            this.m_files.Clear();
            foreach (DataGridViewRow row in (IEnumerable) this.dgvAdditionalFiles.Rows)
            {
                this.m_files.Add(row.DataBoundItem);
            }
        }

        private object SelectedItem
        {
            get
            {
                if (this.dgvAdditionalFiles.SelectedRows.Count > 0)
                {
                    return this.dgvAdditionalFiles.SelectedRows[0].DataBoundItem;
                }
                return null;
            }
        }
    }
}

