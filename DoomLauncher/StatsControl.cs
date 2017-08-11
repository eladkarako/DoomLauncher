namespace DoomLauncher
{
    using DoomLauncher.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class StatsControl : UserControl
    {
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lblItems;
        private Label lblKills;
        private Label lblSecrets;
        private IStatsDataSource m_stats;
        private PictureBox pbItems;
        private PictureBox pbKills;
        private PictureBox pbSecrets;
        private TableLayoutPanel tblStats;

        public StatsControl()
        {
            this.InitializeComponent();
            this.pbItems.Image = Resources.bon2b;
            this.pbKills.Image = Resources.kill;
            this.pbSecrets.Image = Resources.secret;
            this.lblItems.Text = this.lblKills.Text = this.lblSecrets.Text = string.Empty;
            this.lblItems.Visible = this.lblKills.Visible = this.lblSecrets.Visible = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawProgress(Point pt, int count, int total, string text)
        {
            int width = 160;
            int height = 20;
            Graphics graphics = this.tblStats.CreateGraphics();
            Pen pen = new Pen(Color.Black, 1f);
            Rectangle rect = new Rectangle(pt, new Size(width, height));
            graphics.DrawRectangle(pen, rect);
            double percent = 0.0;
            if (total > 0)
            {
                percent = ((double) count) / ((double) total);
            }
            width = (int) ((width - 1) * percent);
            pt.Offset(1, 1);
            rect = new Rectangle(pt, new Size(rect.Width - 1, rect.Height - 1));
            Brush brush = new LinearGradientBrush(rect, Color.DarkGray, Color.LightGray, 90f);
            Rectangle rectangle2 = new Rectangle(rect.Location, new Size(width, rect.Height));
            Brush percentBrush = GetPercentBrush(rect, percent);
            graphics.FillRectangle(brush, rect);
            if (percent > 0.0)
            {
                graphics.FillRectangle(percentBrush, rectangle2);
                pt.Offset(-1, -1);
                graphics.DrawRectangle(GetPrecentPen(percent), new Rectangle(pt, new Size(rectangle2.Width + 1, rectangle2.Height + 1)));
            }
            Brush brush3 = new SolidBrush(Color.Black);
            graphics.DrawString(text, new Font(FontFamily.GenericSansSerif, 10f), brush3, new PointF((float) (pt.X + 3), (float) (pt.Y + 3)));
        }

        private static Brush GetPercentBrush(Rectangle rect, double percent)
        {
            if (percent >= 1.0)
            {
                return new LinearGradientBrush(rect, Color.LightGreen, Color.Green, LinearGradientMode.ForwardDiagonal);
            }
            return new LinearGradientBrush(rect, Color.LightBlue, Color.Blue, LinearGradientMode.ForwardDiagonal);
        }

        private static Pen GetPrecentPen(double percent)
        {
            if (percent >= 1.0)
            {
                return new Pen(Color.Green);
            }
            return new Pen(Color.Blue);
        }

        private Point GetProgressDrawPoint(Control ctrl)
        {
            Point location = ctrl.Location;
            location.Offset(-4, -4);
            return location;
        }

        private void InitializeComponent()
        {
            this.tblStats = new TableLayoutPanel();
            this.lblItems = new Label();
            this.lblSecrets = new Label();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.lblKills = new Label();
            this.pbKills = new PictureBox();
            this.pbSecrets = new PictureBox();
            this.pbItems = new PictureBox();
            this.tblStats.SuspendLayout();
            ((ISupportInitialize) this.pbKills).BeginInit();
            ((ISupportInitialize) this.pbSecrets).BeginInit();
            ((ISupportInitialize) this.pbItems).BeginInit();
            base.SuspendLayout();
            this.tblStats.ColumnCount = 3;
            this.tblStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f));
            this.tblStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 76f));
            this.tblStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 136f));
            this.tblStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tblStats.Controls.Add(this.lblItems, 2, 2);
            this.tblStats.Controls.Add(this.lblSecrets, 2, 1);
            this.tblStats.Controls.Add(this.label1, 1, 0);
            this.tblStats.Controls.Add(this.label2, 1, 1);
            this.tblStats.Controls.Add(this.label3, 1, 2);
            this.tblStats.Controls.Add(this.lblKills, 2, 0);
            this.tblStats.Controls.Add(this.pbKills, 0, 0);
            this.tblStats.Controls.Add(this.pbSecrets, 0, 1);
            this.tblStats.Controls.Add(this.pbItems, 0, 2);
            this.tblStats.Dock = DockStyle.Fill;
            this.tblStats.Location = new Point(0, 0);
            this.tblStats.Margin = new Padding(0);
            this.tblStats.Name = "tblStats";
            this.tblStats.RowCount = 4;
            this.tblStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblStats.Size = new Size(0x106, 150);
            this.tblStats.TabIndex = 7;
            this.tblStats.Paint += new PaintEventHandler(this.tblStats_Paint);
            this.lblItems.Anchor = AnchorStyles.Left;
            this.lblItems.AutoSize = true;
            this.lblItems.Location = new Point(0x81, 0x35);
            this.lblItems.Name = "lblItems";
            this.lblItems.Size = new Size(30, 13);
            this.lblItems.TabIndex = 5;
            this.lblItems.Text = "0 / 0";
            this.lblSecrets.Anchor = AnchorStyles.Left;
            this.lblSecrets.AutoSize = true;
            this.lblSecrets.Location = new Point(0x81, 0x1d);
            this.lblSecrets.Name = "lblSecrets";
            this.lblSecrets.Size = new Size(30, 13);
            this.lblSecrets.TabIndex = 4;
            this.lblSecrets.Text = "0 / 0";
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x35, 5);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1c, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kills:";
            this.label2.Anchor = AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x35, 0x1d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2e, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Secrets:";
            this.label3.Anchor = AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x35, 0x35);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x23, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Items:";
            this.lblKills.Anchor = AnchorStyles.Left;
            this.lblKills.AutoSize = true;
            this.lblKills.Location = new Point(0x81, 5);
            this.lblKills.Name = "lblKills";
            this.lblKills.Size = new Size(30, 13);
            this.lblKills.TabIndex = 3;
            this.lblKills.Text = "0 / 0";
            this.pbKills.BackgroundImageLayout = ImageLayout.Center;
            this.pbKills.Dock = DockStyle.Fill;
            this.pbKills.Location = new Point(0, 0);
            this.pbKills.Margin = new Padding(0);
            this.pbKills.Name = "pbKills";
            this.pbKills.Size = new Size(50, 0x18);
            this.pbKills.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pbKills.TabIndex = 6;
            this.pbKills.TabStop = false;
            this.pbSecrets.BackgroundImageLayout = ImageLayout.Center;
            this.pbSecrets.Dock = DockStyle.Fill;
            this.pbSecrets.Location = new Point(0, 0x18);
            this.pbSecrets.Margin = new Padding(0);
            this.pbSecrets.Name = "pbSecrets";
            this.pbSecrets.Size = new Size(50, 0x18);
            this.pbSecrets.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pbSecrets.TabIndex = 7;
            this.pbSecrets.TabStop = false;
            this.pbItems.BackgroundImageLayout = ImageLayout.Center;
            this.pbItems.Dock = DockStyle.Fill;
            this.pbItems.Location = new Point(0, 0x30);
            this.pbItems.Margin = new Padding(0);
            this.pbItems.Name = "pbItems";
            this.pbItems.Size = new Size(50, 0x18);
            this.pbItems.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pbItems.TabIndex = 8;
            this.pbItems.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblStats);
            base.Name = "StatsControl";
            base.Size = new Size(0x106, 150);
            this.tblStats.ResumeLayout(false);
            this.tblStats.PerformLayout();
            ((ISupportInitialize) this.pbKills).EndInit();
            ((ISupportInitialize) this.pbSecrets).EndInit();
            ((ISupportInitialize) this.pbItems).EndInit();
            base.ResumeLayout(false);
        }

        public void SetStatistics(IEnumerable<IStatsDataSource> stats)
        {
            StatsDataSource source = new StatsDataSource();
            foreach (IStatsDataSource source2 in stats)
            {
                source.KillCount += source2.KillCount;
                source.TotalKills += source2.TotalKills;
                source.SecretCount += source2.SecretCount;
                source.TotalSecrets += source2.TotalSecrets;
                source.ItemCount += source2.ItemCount;
                source.TotalItems += source2.TotalItems;
                source.LevelTime += source2.LevelTime;
            }
            this.m_stats = source;
        }

        private void tblStats_Paint(object sender, PaintEventArgs e)
        {
            if (this.m_stats != null)
            {
                this.DrawProgress(this.GetProgressDrawPoint(this.lblKills), this.m_stats.KillCount, this.m_stats.TotalKills, this.m_stats.FormattedKills);
                this.DrawProgress(this.GetProgressDrawPoint(this.lblSecrets), this.m_stats.SecretCount, this.m_stats.TotalSecrets, this.m_stats.FormattedSecrets);
                this.DrawProgress(this.GetProgressDrawPoint(this.lblItems), this.m_stats.ItemCount, this.m_stats.TotalItems, this.m_stats.FormattedItems);
            }
        }
    }
}

