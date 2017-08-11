namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using Equin.ApplicationFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class StatisticsView : UserControl, IFileAssociationView
    {
        private IContainer components;
        private CDataGridView dgvMain;
        private ContextMenuStrip m_menu;

        public StatisticsView()
        {
            this.InitializeComponent();
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.AutoGenerateColumns = false;
            this.dgvMain.ShowCellToolTips = false;
            this.dgvMain.DefaultCellStyle.SelectionBackColor = Color.Gray;
            Tuple<string, string>[] columnFields = new Tuple<string, string>[] { new Tuple<string, string>("MapName", "Map"), new Tuple<string, string>("FormattedKills", "Kills"), new Tuple<string, string>("FormattedSecrets", "Secrets"), new Tuple<string, string>("FormattedItems", "Items"), new Tuple<string, string>("FormattedTime", "Time"), new Tuple<string, string>("RecordTime", "Date"), new Tuple<string, string>("SourcePort", "SourcePort") };
            this.SetColumnFields(columnFields);
            this.dgvMain.Columns[this.dgvMain.Columns.Count - 2].DefaultCellStyle.Format = $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} {CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern}";
        }

        public void CopyAllToClipboard()
        {
            this.dgvMain.SelectAll();
            Clipboard.SetDataObject(this.dgvMain.GetClipboardContent(), true);
        }

        public void CopyToClipboard()
        {
            Clipboard.SetDataObject(this.dgvMain.GetClipboardContent(), true);
        }

        public bool Delete()
        {
            if ((this.dgvMain.SelectedRows.Count <= 0) || (MessageBox.Show(this, "Delete selected statistic(s)?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.OK))
            {
                return false;
            }
            foreach (DataGridViewRow row in this.dgvMain.SelectedRows)
            {
                IStatsDataSource statsFromGridRow = this.GetStatsFromGridRow(row);
                this.DataSourceAdapter.DeleteStats(statsFromGridRow.StatID);
            }
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Edit()
        {
            throw new NotSupportedException();
        }

        private List<StatsBind> GetStatsBind(IEnumerable<IStatsDataSource> stats)
        {
            List<ISourcePortDataSource> sourcePortsData = DoomLauncher.Util.GetSourcePortsData(this.DataSourceAdapter);
            return (from stat in stats
                join sp in sourcePortsData on stat.SourcePortID equals sp.SourcePortID
                select new StatsBind(stat.MapName, stat.FormattedKills, stat.FormattedSecrets, stat.FormattedItems, TimeSpan.FromSeconds((double) stat.LevelTime), stat.RecordTime, sp.Name, stat)).ToList<StatsBind>();
        }

        private IStatsDataSource GetStatsFromGridRow(DataGridViewRow row) => 
            ((ObjectView<StatsBind>) row.DataBoundItem).Object.StatsDataSource;

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
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.dgvMain);
            base.Name = "StatisticsView";
            ((ISupportInitialize) this.dgvMain).EndInit();
            base.ResumeLayout(false);
        }

        public bool MoveFileOrderDown()
        {
            throw new NotSupportedException();
        }

        public bool MoveFileOrderUp()
        {
            throw new NotSupportedException();
        }

        public bool New()
        {
            throw new NotSupportedException();
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

        public void SetContextMenu(ContextMenuStrip menu)
        {
            this.m_menu = menu;
        }

        public void SetData(IGameFileDataSource gameFile)
        {
            if ((gameFile != null) && gameFile.GameFileID.HasValue)
            {
                IEnumerable<IStatsDataSource> stats = this.DataSourceAdapter.GetStats(gameFile.GameFileID.Value);
                this.dgvMain.DataSource = new BindingListView<StatsBind>(this.GetStatsBind(stats));
                this.dgvMain.ContextMenuStrip = this.m_menu;
            }
            else
            {
                this.dgvMain.DataSource = null;
            }
        }

        public bool SetFileOrderFirst()
        {
            throw new NotSupportedException();
        }

        public void View()
        {
            throw new NotSupportedException();
        }

        public bool ChangeOrderAllowed =>
            false;

        public bool CopyAllowed =>
            false;

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public bool DeleteAllowed =>
            true;

        public bool EditAllowed =>
            false;

        public IGameFileDataSource GameFile { get; set; }

        public bool NewAllowed =>
            false;

        public bool ViewAllowed =>
            false;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly StatisticsView.<>c <>9 = new StatisticsView.<>c();
            public static Func<IStatsDataSource, int> <>9__4_0;
            public static Func<ISourcePortDataSource, int> <>9__4_1;
            public static Func<IStatsDataSource, ISourcePortDataSource, StatisticsView.StatsBind> <>9__4_2;

            internal int <GetStatsBind>b__4_0(IStatsDataSource stat) => 
                stat.SourcePortID;

            internal int <GetStatsBind>b__4_1(ISourcePortDataSource sp) => 
                sp.SourcePortID;

            internal StatisticsView.StatsBind <GetStatsBind>b__4_2(IStatsDataSource stat, ISourcePortDataSource sp) => 
                new StatisticsView.StatsBind(stat.MapName, stat.FormattedKills, stat.FormattedSecrets, stat.FormattedItems, TimeSpan.FromSeconds((double) stat.LevelTime), stat.RecordTime, sp.Name, stat);
        }

        private class StatsBind
        {
            public StatsBind(string map, string kills, string secrets, string items, TimeSpan time, DateTime recordtime, string sourceport, IStatsDataSource statsdatasource)
            {
                this.MapName = map;
                this.FormattedKills = kills;
                this.FormattedSecrets = secrets;
                this.FormattedItems = items;
                this.FormattedTime = time;
                this.RecordTime = recordtime;
                this.SourcePort = sourceport;
                this.StatsDataSource = statsdatasource;
            }

            public string FormattedItems { get; set; }

            public string FormattedKills { get; set; }

            public string FormattedSecrets { get; set; }

            public TimeSpan FormattedTime { get; set; }

            public string MapName { get; set; }

            public DateTime RecordTime { get; set; }

            public string SourcePort { get; set; }

            public IStatsDataSource StatsDataSource { get; set; }
        }
    }
}

