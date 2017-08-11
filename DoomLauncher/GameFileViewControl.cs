namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using Equin.ApplicationFramework;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;

    public class GameFileViewControl : UserControl
    {
        private IContainer components;
        private CDataGridView dgvMain;
        private BindingListView<GameFileDataSource> m_datasource;
        private Label m_label = new Label();
        private Dictionary<string, DataGridViewColumn> m_orderLookup = new Dictionary<string, DataGridViewColumn>();
        private Dictionary<int, PropertyInfo> m_properties = new Dictionary<int, PropertyInfo>();
        private bool m_setting;
        private ToolTip toolTip1;

        [field: CompilerGenerated]
        public event CancelEventHandler CustomRowPaint;

        [field: CompilerGenerated]
        public event KeyPressEventHandler GridKeyPress;

        [field: CompilerGenerated]
        public event EventHandler RowContentDoubleClicked;

        [field: CompilerGenerated]
        public event EventHandler RowDoubleClicked;

        [field: CompilerGenerated]
        public event EventHandler SelectionChange;

        [field: CompilerGenerated]
        public event AddingNewEventHandler ToolTipTextNeeded;

        public GameFileViewControl()
        {
            this.InitializeComponent();
            this.SetupGridView();
            this.m_label.AutoSize = true;
            this.m_label.Visible = false;
            this.m_label.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.m_label.Margin = new Padding(12);
            this.m_label.Font = new Font(this.m_label.Font.FontFamily, 12f, FontStyle.Bold);
            base.Controls.Add(this.m_label);
        }

        private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.m_setting && (this.SelectionChange != null))
            {
                this.SelectionChange(this, new EventArgs());
            }
        }

        private void dgvMain_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.RowContentDoubleClicked != null)
            {
                this.RowContentDoubleClicked(this, new EventArgs());
            }
        }

        private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.RowDoubleClicked != null)
            {
                this.RowDoubleClicked(this, new EventArgs());
            }
        }

        private void dgvMain_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (((e.RowIndex > -1) && (e.Button == MouseButtons.Right)) && (this.dgvMain.SelectedRows.Count < 2))
            {
                this.m_setting = true;
                this.dgvMain.ClearSelection();
                this.dgvMain.Rows[e.RowIndex].Selected = true;
                this.dgvMain.Rows[e.RowIndex].Cells[0].Selected = true;
                if (this.SelectionChange != null)
                {
                    this.SelectionChange(this, new EventArgs());
                }
                this.m_setting = false;
            }
        }

        private void dgvMain_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.ToolTipTimer.Stop();
            if (e.RowIndex != this.ToolTipRowIndex)
            {
                this.toolTip1.Hide(this.dgvMain);
                this.ToolTipRowIndex = e.RowIndex;
                this.ToolTipTimer.Interval = 500.0;
                this.ToolTipTimer.Start();
            }
        }

        private void dgvMain_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.ToolTipTimer.Stop();
            this.toolTip1.Hide(this.dgvMain);
            this.ToolTipRowIndex = -1;
        }

        private void dgvMain_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if ((this.m_datasource != null) && (this.m_datasource.Count > e.RowIndex))
            {
                GameFileDataSource source = this.m_datasource[e.RowIndex].Object;
                if (!this.m_properties.ContainsKey(e.ColumnIndex))
                {
                    this.m_properties.Add(e.ColumnIndex, source.GetType().GetProperty(this.dgvMain.Columns[e.ColumnIndex].DataPropertyName));
                }
                e.Value = this.m_properties[e.ColumnIndex].GetValue(source);
            }
        }

        private void dgvMain_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.ColumnReorderIndex = e.Column.DisplayIndex;
        }

        private void dgvMain_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.m_datasource != null)
            {
                string str = null;
                DataGridViewColumn column = this.dgvMain.Columns[e.ColumnIndex];
                foreach (DataGridViewColumn column2 in this.dgvMain.Columns)
                {
                    if (column2 != column)
                    {
                        column2.HeaderCell.SortGlyphDirection = SortOrder.None;
                    }
                }
                if (column.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    column.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    str = " ASC";
                }
                else
                {
                    column.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    str = " DESC";
                }
                BindingListView<GameFileDataSource> datasource = this.m_datasource;
                if (datasource != null)
                {
                    datasource.ApplySort(column.Name + " " + str);
                    this.dgvMain.Invalidate();
                }
            }
        }

        private void dgvMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.GridKeyPress != null)
            {
                this.GridKeyPress(this, e);
            }
        }

        private void dgvMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (this.CustomRowColorPaint && (this.CustomRowPaint != null))
            {
                this.CustomRowPaintDataBoundItem = this.ItemForRow(e.RowIndex);
                CancelEventArgs args = new CancelEventArgs();
                this.CustomRowPaint(this, args);
                if (!args.Cancel)
                {
                    this.dgvMain.Rows[e.RowIndex].DefaultCellStyle.ForeColor = this.CustomRowPaintForeColor;
                }
            }
        }

        private void dgvMain_SelectionChanged(object sender, EventArgs e)
        {
            if (!this.m_setting && (this.SelectionChange != null))
            {
                this.SelectionChange(this, new EventArgs());
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

        public List<Tuple<string, string>> GetColumnFormats()
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            foreach (DataGridViewColumn column in this.dgvMain.Columns)
            {
                if (!string.IsNullOrEmpty(column.DefaultCellStyle.Format))
                {
                    list.Add(new Tuple<string, string>(column.Name, column.DefaultCellStyle.Format));
                }
            }
            return list;
        }

        public string[] GetColumnKeyOrder()
        {
            Func<KeyValuePair<string, DataGridViewColumn>, bool> <>9__0;
            string[] strArray = new string[this.m_orderLookup.Count];
            int count = 0;
            using (Dictionary<string, DataGridViewColumn>.Enumerator enumerator = this.m_orderLookup.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, DataGridViewColumn> current = enumerator.Current;
                    strArray[count] = (from col in this.m_orderLookup.Where<KeyValuePair<string, DataGridViewColumn>>((Func<KeyValuePair<string, DataGridViewColumn>, bool>) (<>9__0 ?? (<>9__0 = col => col.Value.DisplayIndex == count))) select col.Key).First<string>();
                    int num = count;
                    count = num + 1;
                }
            }
            return strArray;
        }

        public int GetColumnWidth(string key)
        {
            key = key.ToLower();
            return this.m_orderLookup[key].Width;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            this.toolTip1 = new ToolTip(this.components);
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
            this.dgvMain.TabIndex = 1;
            this.dgvMain.CellContentDoubleClick += new DataGridViewCellEventHandler(this.dgvMain_CellContentDoubleClick);
            this.dgvMain.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvMain_CellDoubleClick);
            this.dgvMain.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvMain_CellMouseDown);
            this.dgvMain.CellMouseEnter += new DataGridViewCellEventHandler(this.dgvMain_CellMouseEnter);
            this.dgvMain.CellMouseLeave += new DataGridViewCellEventHandler(this.dgvMain_CellMouseLeave);
            this.dgvMain.RowPostPaint += new DataGridViewRowPostPaintEventHandler(this.dgvMain_RowPostPaint);
            this.dgvMain.KeyPress += new KeyPressEventHandler(this.dgvMain_KeyPress);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.dgvMain);
            base.Name = "GameFileViewControl";
            ((ISupportInitialize) this.dgvMain).EndInit();
            base.ResumeLayout(false);
        }

        public object ItemForRow(int rowIndex)
        {
            if ((rowIndex > -1) && (rowIndex < this.dgvMain.Rows.Count))
            {
                return this.m_datasource[rowIndex];
            }
            return null;
        }

        public void SetColumnFields(IEnumerable<Tuple<string, string>> columnFields)
        {
            this.m_orderLookup.Clear();
            this.ColumnFields = columnFields.ToArray<Tuple<string, string>>();
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
                    this.m_orderLookup.Add(tuple.Item1.ToLower(), dataGridViewColumn);
                }
                this.dgvMain.Columns[this.dgvMain.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        public void SetColumnFormat(string colName, string format)
        {
            this.dgvMain.Columns[colName].DefaultCellStyle.Format = format;
        }

        public void SetColumnWidth(string key, int width)
        {
            key = key.ToLower();
            this.m_orderLookup[key].Width = width;
        }

        public void SetContextMenuStrip(ContextMenuStrip menu)
        {
            this.dgvMain.ContextMenuStrip = menu;
        }

        public void SetDisplayText(string text)
        {
            this.m_label.Text = text;
            this.m_label.Visible = true;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Remove(this.dgvMain);
        }

        private void SetToolTipText()
        {
            if (this.ToolTipTextNeeded != null)
            {
                AddingNewEventArgs e = new AddingNewEventArgs(string.Empty);
                this.ToolTipTextNeeded(this, e);
                this.toolTip1.SetToolTip(this.dgvMain, e.NewObject.ToString());
            }
        }

        private void SetupGridView()
        {
            this.dgvMain.DefaultCellStyle.NullValue = "N/A";
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.AutoGenerateColumns = false;
            this.dgvMain.ShowCellToolTips = false;
            this.dgvMain.DefaultCellStyle.SelectionBackColor = Color.Gray;
            this.dgvMain.SelectionChanged += new EventHandler(this.dgvMain_SelectionChanged);
            this.dgvMain.CellClick += new DataGridViewCellEventHandler(this.dgvMain_CellClick);
            this.dgvMain.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(this.dgvMain_ColumnDisplayIndexChanged);
            this.dgvMain.VirtualMode = true;
            this.dgvMain.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvMain_CellValueNeeded);
            this.toolTip1.AutoPopDelay = 0x7fff;
            this.ToolTipTimer = new System.Timers.Timer(500.0);
            this.ToolTipTimer.Elapsed += new ElapsedEventHandler(this.ToolTipTimer_Elapsed);
            this.dgvMain.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgvMain_ColumnHeaderMouseClick);
        }

        private void ToolTipTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.ToolTipTimer.Enabled && base.InvokeRequired)
            {
                base.Invoke(new Action(this.SetToolTipText));
                this.ToolTipTimer.Stop();
            }
        }

        public Tuple<string, string>[] ColumnFields { get; private set; }

        public int ColumnReorderIndex { get; private set; }

        public bool CustomRowColorPaint { get; set; }

        public object CustomRowPaintDataBoundItem { get; private set; }

        public Color CustomRowPaintForeColor { get; set; }

        public object DataSource
        {
            get => 
                this.m_datasource;
            set
            {
                this.m_datasource = (BindingListView<GameFileDataSource>) value;
                if (this.m_datasource == null)
                {
                    this.dgvMain.RowCount = 0;
                }
                else
                {
                    this.dgvMain.RowCount = this.m_datasource.Count;
                }
                if (!base.Controls.Contains(this.dgvMain))
                {
                    base.Controls.Add(this.dgvMain);
                    this.m_label.Visible = false;
                    base.BorderStyle = BorderStyle.None;
                }
            }
        }

        public object DoomLauncherParent { get; set; }

        public bool MultiSelect
        {
            get => 
                this.dgvMain.MultiSelect;
            set
            {
                this.dgvMain.MultiSelect = value;
            }
        }

        public object SelectedItem
        {
            get
            {
                if (this.dgvMain.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = this.dgvMain.SelectedRows[0];
                    return this.m_datasource[row.Index];
                }
                return null;
            }
            set
            {
                this.dgvMain.ClearSelection();
                int num = 0;
                foreach (DataGridViewRow row in (IEnumerable) this.dgvMain.Rows)
                {
                    if (this.m_datasource[row.Index].Equals(value))
                    {
                        row.Selected = true;
                        row.Cells[0].Selected = true;
                        this.dgvMain.FirstDisplayedScrollingRowIndex = num;
                        break;
                    }
                    num++;
                }
            }
        }

        public object[] SelectedItems
        {
            get
            {
                if ((this.m_datasource == null) || (this.dgvMain.SelectedRows.Count <= 0))
                {
                    return new object[0];
                }
                List<object> list = new List<object>(this.dgvMain.Rows.Count);
                foreach (DataGridViewRow row in this.dgvMain.SelectedRows)
                {
                    if (this.m_datasource.Count > row.Index)
                    {
                        list.Add(this.m_datasource[row.Index]);
                    }
                }
                return list.ToArray();
            }
        }

        public int ToolTipRowIndex { get; private set; }

        private System.Timers.Timer ToolTipTimer { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly GameFileViewControl.<>c <>9 = new GameFileViewControl.<>c();
            public static Func<KeyValuePair<string, DataGridViewColumn>, string> <>9__55_1;

            internal string <GetColumnKeyOrder>b__55_1(KeyValuePair<string, DataGridViewColumn> col) => 
                col.Key;
        }
    }
}

