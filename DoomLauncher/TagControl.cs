namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TagControl : UserControl
    {
        private Button btnAdd;
        private Button btnDelete;
        private Button btnEdit;
        private IContainer components;
        private DataGridView dgvTags;
        private FlowLayoutPanel flowLayoutPanel1;
        private IDataSourceAdapter m_adapter;
        private List<ITagDataSource> m_addTags = new List<ITagDataSource>();
        private List<ITagDataSource> m_deleteTags = new List<ITagDataSource>();
        private List<ITagDataSource> m_editTags = new List<ITagDataSource>();
        private TableLayoutPanel tblMain;

        public TagControl()
        {
            this.InitializeComponent();
            this.dgvTags.RowHeadersVisible = false;
            this.dgvTags.AutoGenerateColumns = false;
            this.dgvTags.DefaultCellStyle.SelectionBackColor = Color.Gray;
            DataGridViewColumn dataGridViewColumn = new DataGridViewTextBoxColumn {
                HeaderText = "Name",
                Name = "Name",
                DataPropertyName = "Name"
            };
            this.dgvTags.Columns.Add(dataGridViewColumn);
            dataGridViewColumn = new DataGridViewTextBoxColumn {
                HeaderText = "Display Tab",
                Name = "HasTab",
                DataPropertyName = "HasTab"
            };
            this.dgvTags.Columns.Add(dataGridViewColumn);
            dataGridViewColumn = new DataGridViewTextBoxColumn {
                HeaderText = "Display Color",
                Name = "HasColor",
                DataPropertyName = "HasColor"
            };
            this.dgvTags.Columns.Add(dataGridViewColumn);
            dataGridViewColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TagEditForm form = new TagEditForm {
                StartPosition = FormStartPosition.CenterParent
            };
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                TagDataSource ds = new TagDataSource();
                form.TagEditControl.GetDataSource(ds);
                if (!this.IsTagNameUnique(ds))
                {
                    MessageBox.Show(this, "Tag name must be unique and not empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    this.m_adapter.InsertTag(ds);
                    this.Init(this.m_adapter);
                    IEnumerable<ITagDataSource> source = from item in this.m_adapter.GetTags()
                        where item.Name == ds.Name
                        select item;
                    if (source.Count<ITagDataSource>() > 0)
                    {
                        if (this.m_addTags.Contains(source.First<ITagDataSource>()))
                        {
                            this.m_addTags.Remove(source.First<ITagDataSource>());
                        }
                        this.m_addTags.Add(source.First<ITagDataSource>());
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((this.dgvTags.SelectedRows.Count > 0) && (MessageBox.Show(this, "Are you sure you want to delete this tag?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK))
            {
                ITagDataSource dataBoundItem = this.dgvTags.SelectedRows[0].DataBoundItem as ITagDataSource;
                if (dataBoundItem != null)
                {
                    this.m_adapter.DeleteTag(dataBoundItem);
                    this.m_adapter.DeleteTagMapping(dataBoundItem.TagID);
                    this.Init(this.m_adapter);
                    if (this.m_deleteTags.Contains(dataBoundItem))
                    {
                        this.m_deleteTags.Remove(dataBoundItem);
                    }
                    this.m_deleteTags.Add(dataBoundItem);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvTags.SelectedRows.Count > 0)
            {
                ITagDataSource dataBoundItem = this.dgvTags.SelectedRows[0].DataBoundItem as ITagDataSource;
                if (dataBoundItem != null)
                {
                    TagEditForm form = new TagEditForm();
                    form.TagEditControl.SetDataSource(dataBoundItem);
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!this.IsTagNameUnique(dataBoundItem))
                        {
                            MessageBox.Show(this, "Tag name must be unique and not empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                        else
                        {
                            form.TagEditControl.GetDataSource(dataBoundItem);
                            this.m_adapter.UpdateTag(dataBoundItem);
                            this.Init(this.m_adapter);
                            if (this.m_editTags.Contains(dataBoundItem))
                            {
                                this.m_editTags.Remove(dataBoundItem);
                            }
                            this.m_editTags.Add(dataBoundItem);
                        }
                    }
                }
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

        public void Init(IDataSourceAdapter adapter)
        {
            this.m_adapter = adapter;
            this.dgvTags.DataSource = (from x in adapter.GetTags()
                orderby x.Name
                select x).ToList<ITagDataSource>();
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            this.tblMain = new TableLayoutPanel();
            this.dgvTags = new DataGridView();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.tblMain.SuspendLayout();
            ((ISupportInitialize) this.dgvTags).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.dgvTags, 0, 0);
            this.tblMain.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0x128, 150);
            this.tblMain.TabIndex = 0;
            this.dgvTags.AllowUserToAddRows = false;
            this.dgvTags.AllowUserToDeleteRows = false;
            style.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.dgvTags.AlternatingRowsDefaultCellStyle = style;
            this.dgvTags.BackgroundColor = SystemColors.ControlLightLight;
            this.dgvTags.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvTags.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTags.Dock = DockStyle.Fill;
            this.dgvTags.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dgvTags.GridColor = SystemColors.ActiveBorder;
            this.dgvTags.Location = new Point(3, 3);
            this.dgvTags.Name = "dgvTags";
            this.dgvTags.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvTags.Size = new Size(290, 0x70);
            this.dgvTags.TabIndex = 0;
            this.flowLayoutPanel1.Controls.Add(this.btnAdd);
            this.flowLayoutPanel1.Controls.Add(this.btnEdit);
            this.flowLayoutPanel1.Controls.Add(this.btnDelete);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.Location = new Point(0, 0x76);
            this.flowLayoutPanel1.Margin = new Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(0x128, 0x20);
            this.flowLayoutPanel1.TabIndex = 1;
            this.btnAdd.Location = new Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x4b, 0x17);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnEdit.Location = new Point(0x54, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(0x4b, 0x17);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.btnDelete.Location = new Point(0xa5, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(0x4b, 0x17);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblMain);
            base.Name = "TagControl";
            base.Size = new Size(0x128, 150);
            this.tblMain.ResumeLayout(false);
            ((ISupportInitialize) this.dgvTags).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private bool IsTagNameUnique(ITagDataSource ds)
        {
            IEnumerable<ITagDataSource> source = from item in this.m_adapter.GetTags()
                where item.Name.Equals(ds.Name, StringComparison.CurrentCultureIgnoreCase) && !item.Equals(ds)
                select item;
            string[] strArray = new string[] { "recent", "local", "iwads", "id games" };
            return ((!string.IsNullOrEmpty(ds.Name) && (source.Count<ITagDataSource>() <= 0)) && !strArray.Contains<string>(ds.Name.ToLower()));
        }

        public ITagDataSource[] AddedTags =>
            this.m_addTags.ToArray();

        public ITagDataSource[] DeletedTags =>
            this.m_deleteTags.ToArray();

        public ITagDataSource[] EditedTags =>
            this.m_editTags.ToArray();

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly TagControl.<>c <>9 = new TagControl.<>c();
            public static Func<ITagDataSource, string> <>9__5_0;

            internal string <Init>b__5_0(ITagDataSource x) => 
                x.Name;
        }
    }
}

