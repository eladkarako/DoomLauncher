namespace DoomLauncher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CumulativeStats : Form
    {
        private Button btnOK;
        private IContainer components;
        private StatsControl ctrlStats;
        private Label label1;
        private Label lblTimePlayed;
        private TableLayoutPanelDB tblMain;

        public CumulativeStats()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnOK = new Button();
            this.tblMain = new TableLayoutPanelDB();
            this.lblTimePlayed = new Label();
            this.ctrlStats = new StatsControl();
            this.label1 = new Label();
            this.tblMain.SuspendLayout();
            base.SuspendLayout();
            this.btnOK.Anchor = AnchorStyles.Right;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x109, 0xb3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.tblMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.lblTimePlayed, 0, 1);
            this.tblMain.Controls.Add(this.ctrlStats, 0, 2);
            this.tblMain.Controls.Add(this.label1, 0, 0);
            this.tblMain.Controls.Add(this.btnOK, 0, 3);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Margin = new Padding(0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 4;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 96f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.Size = new Size(0x158, 0xd3);
            this.tblMain.TabIndex = 2;
            this.lblTimePlayed.Anchor = AnchorStyles.Left;
            this.lblTimePlayed.AutoSize = true;
            this.lblTimePlayed.Location = new Point(4, 0x2f);
            this.lblTimePlayed.Name = "lblTimePlayed";
            this.lblTimePlayed.Size = new Size(0x44, 13);
            this.lblTimePlayed.TabIndex = 7;
            this.lblTimePlayed.Text = "Time Played:";
            this.ctrlStats.Dock = DockStyle.Fill;
            this.ctrlStats.Location = new Point(4, 0x4e);
            this.ctrlStats.Margin = new Padding(3, 3, 0, 0);
            this.ctrlStats.Name = "ctrlStats";
            this.ctrlStats.Size = new Size(0x153, 0x5d);
            this.ctrlStats.TabIndex = 8;
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x7d, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Note: Search filters apply";
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x158, 0xd3);
            base.Controls.Add(this.tblMain);
            base.Name = "CumulativeStats";
            base.ShowIcon = false;
            this.Text = "Cumulative Stats";
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            base.ResumeLayout(false);
        }

        public void SetStatistics(IEnumerable<IStatsDataSource> stats)
        {
            int minutes = 0;
            foreach (IStatsDataSource source in stats)
            {
                minutes += (int) (((double) source.LevelTime) / 60.0);
            }
            this.lblTimePlayed.Text = DoomLauncher.Util.GetTimePlayedString(minutes);
            this.ctrlStats.SetStatistics(stats);
        }
    }
}

