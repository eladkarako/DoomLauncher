namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    public class GenericFileView : BasicFileView
    {
        private IContainer components;
        private CDataGridView dgvMain;

        public GenericFileView()
        {
            this.InitializeComponent();
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.AutoGenerateColumns = false;
            this.dgvMain.ShowCellToolTips = false;
            this.dgvMain.DefaultCellStyle.SelectionBackColor = Color.Gray;
            Tuple<string, string>[] columnFields = new Tuple<string, string>[] { new Tuple<string, string>("Description", "Description"), new Tuple<string, string>("DateCreated", "Created"), new Tuple<string, string>("SourcePortName", "SourcePort") };
            this.SetColumnFields(columnFields);
        }

        private void dgvMain_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (((e.RowIndex > -1) && (e.Button == MouseButtons.Right)) && (this.dgvMain.SelectedRows.Count < 2))
            {
                this.dgvMain.ClearSelection();
                this.dgvMain.Rows[e.RowIndex].Selected = true;
                this.dgvMain.Rows[e.RowIndex].Cells[0].Selected = true;
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

        protected override List<IFileDataSource> GetSelectedFiles()
        {
            List<IFileDataSource> list = new List<IFileDataSource>();
            if (this.dgvMain.SelectedRows.Count > 0)
            {
                PropertyInfo property = this.dgvMain.SelectedRows[0].DataBoundItem.GetType().GetProperty("FileDataSource");
                foreach (DataGridViewRow row in this.dgvMain.SelectedRows)
                {
                    list.Add(property.GetValue(row.DataBoundItem) as IFileDataSource);
                }
            }
            return list;
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            this.dgvMain = new CDataGridView();
            ((ISupportInitialize) this.dgvMain).BeginInit();
            base.SuspendLayout();
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToOrderColumns = true;
            style.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.dgvMain.AlternatingRowsDefaultCellStyle = style;
            this.dgvMain.BackgroundColor = SystemColors.ControlLightLight;
            this.dgvMain.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Dock = DockStyle.Fill;
            this.dgvMain.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dgvMain.GridColor = SystemColors.ActiveBorder;
            this.dgvMain.Location = new Point(0, 0);
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new Size(150, 150);
            this.dgvMain.TabIndex = 2;
            this.dgvMain.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvMain_CellMouseDown);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.dgvMain);
            base.Name = "DemoView";
            ((ISupportInitialize) this.dgvMain).EndInit();
            base.ResumeLayout(false);
        }

        private void SetColumnFields(IEnumerable<Tuple<string, string>> columnFields)
        {
            if (columnFields.Count<Tuple<string, string>>() > 0)
            {
                foreach (Tuple<string, string> tuple in columnFields)
                {
                    DataGridViewColumn dataGridViewColumn = new DataGridViewTextBoxColumn {
                        HeaderText = tuple.Item2,
                        Name = tuple.Item1,
                        DataPropertyName = tuple.Item1
                    };
                    this.dgvMain.Columns.Add(dataGridViewColumn);
                }
                this.dgvMain.Columns[this.dgvMain.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        public override void SetData(IGameFileDataSource gameFile)
        {
            base.SetData(this.dgvMain, gameFile);
        }
    }
}

