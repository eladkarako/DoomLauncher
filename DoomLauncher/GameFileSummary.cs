namespace DoomLauncher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class GameFileSummary : UserControl
    {
        private IContainer components;
        private StatsControl ctrlStats;
        private GrowLabel lblTags;
        private Label lblTimePlayed;
        private GrowLabel lblTitle;
        private double m_aspectHeight = 9.0;
        private double m_aspectWidth = 16.0;
        private float m_imageHeight;
        private float m_labelHeight;
        private bool m_setting;
        private PictureBox pbImage;
        private TableLayoutPanelDB tblMain;
        private TextBox txtDescription;

        public GameFileSummary()
        {
            this.InitializeComponent();
            this.m_labelHeight = this.tblMain.RowStyles[0].Height;
            this.m_imageHeight = this.tblMain.RowStyles[1].Height;
        }

        public void ClearPreviewImage()
        {
            this.pbImage.Image = null;
            this.ShowImageSection(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GameFileSummary_ClientSizeChanged(object sender, EventArgs e)
        {
            double num = ((double) base.Width) / (this.m_aspectWidth / this.m_aspectHeight);
            this.m_imageHeight = Convert.ToSingle(num);
            if (this.pbImage.Image != null)
            {
                this.ShowImageSection(true);
            }
        }

        private void InitializeComponent()
        {
            this.tblMain = new TableLayoutPanelDB();
            this.lblTimePlayed = new Label();
            this.lblTitle = new GrowLabel();
            this.txtDescription = new TextBox();
            this.pbImage = new PictureBox();
            this.lblTags = new GrowLabel();
            this.ctrlStats = new StatsControl();
            this.tblMain.SuspendLayout();
            ((ISupportInitialize) this.pbImage).BeginInit();
            base.SuspendLayout();
            this.tblMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.lblTimePlayed, 0, 2);
            this.tblMain.Controls.Add(this.lblTitle, 0, 0);
            this.tblMain.Controls.Add(this.txtDescription, 0, 5);
            this.tblMain.Controls.Add(this.pbImage, 0, 1);
            this.tblMain.Controls.Add(this.lblTags, 0, 4);
            this.tblMain.Controls.Add(this.ctrlStats, 0, 3);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Margin = new Padding(0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 6;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 200f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 78f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.Size = new Size(0xcc, 0x28e);
            this.tblMain.TabIndex = 0;
            this.lblTimePlayed.Anchor = AnchorStyles.Left;
            this.lblTimePlayed.AutoSize = true;
            this.lblTimePlayed.Location = new Point(4, 0x100);
            this.lblTimePlayed.Name = "lblTimePlayed";
            this.lblTimePlayed.Size = new Size(0x44, 13);
            this.lblTimePlayed.TabIndex = 7;
            this.lblTimePlayed.Text = "Time Played:";
            this.lblTitle.Anchor = AnchorStyles.None;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblTitle.Location = new Point(0x51, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(0x29, 0x13);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Title";
            this.lblTitle.UseMnemonic = false;
            this.txtDescription.BackColor = SystemColors.Window;
            this.txtDescription.Dock = DockStyle.Fill;
            this.txtDescription.Location = new Point(4, 0x197);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new Size(0xc4, 0xf3);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.TextChanged += new EventHandler(this.txtDescription_TextChanged);
            this.pbImage.Dock = DockStyle.Fill;
            this.pbImage.Location = new Point(4, 0x2d);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new Size(0xc4, 0xc2);
            this.pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 2;
            this.pbImage.TabStop = false;
            this.lblTags.Anchor = AnchorStyles.Left;
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new Point(4, 0x178);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new Size(0x22, 13);
            this.lblTags.TabIndex = 5;
            this.lblTags.Text = "Tags:";
            this.ctrlStats.Dock = DockStyle.Fill;
            this.ctrlStats.Location = new Point(1, 0x11c);
            this.ctrlStats.Margin = new Padding(0);
            this.ctrlStats.Name = "ctrlStats";
            this.ctrlStats.Size = new Size(0xca, 0x4e);
            this.ctrlStats.TabIndex = 8;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblMain);
            base.Name = "GameFileSummary";
            base.Size = new Size(0xcc, 0x28e);
            base.ClientSizeChanged += new EventHandler(this.GameFileSummary_ClientSizeChanged);
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            ((ISupportInitialize) this.pbImage).EndInit();
            base.ResumeLayout(false);
        }

        private void SetImageFromFile(string source)
        {
            try
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream(source, FileMode.Open, FileAccess.Read);
                    this.pbImage.Image = Image.FromStream(stream);
                }
                catch
                {
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            catch
            {
                this.pbImage.Image = null;
            }
        }

        public void SetPreviewImage(string source, bool isUrl)
        {
            this.pbImage.CancelAsync();
            if (isUrl)
            {
                this.pbImage.LoadAsync(source);
            }
            else
            {
                this.SetImageFromFile(source);
            }
            this.ShowImageSection(true);
        }

        public void SetStatistics(IEnumerable<IStatsDataSource> stats)
        {
            if (stats.Count<IStatsDataSource>() > 0)
            {
                this.ctrlStats.Visible = true;
                this.tblMain.RowStyles[3].Height = 92f;
                this.ctrlStats.SetStatistics(stats);
            }
            else
            {
                this.ctrlStats.Visible = false;
                this.tblMain.RowStyles[3].Height = 0f;
            }
        }

        public void SetTimePlayed(int minutes)
        {
            this.lblTimePlayed.Text = DoomLauncher.Util.GetTimePlayedString(minutes);
        }

        private void ShowImageSection(bool bShow)
        {
            if (bShow)
            {
                this.tblMain.RowStyles[1].Height = this.m_imageHeight;
            }
            else
            {
                this.tblMain.RowStyles[1].Height = 0f;
            }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (!this.m_setting)
            {
                this.m_setting = true;
                TextBox box = sender as TextBox;
                Size size = TextRenderer.MeasureText(box.Text, box.Font);
                bool flag = box.ClientSize.Height < (size.Height + Convert.ToInt32(box.Font.Size));
                bool flag2 = box.ClientSize.Width < size.Width;
                if (flag & flag2)
                {
                    box.ScrollBars = ScrollBars.Both;
                }
                else if (!flag && !flag2)
                {
                    box.ScrollBars = ScrollBars.None;
                }
                else if (flag && !flag2)
                {
                    box.ScrollBars = ScrollBars.Vertical;
                }
                else if (!flag & flag2)
                {
                    box.ScrollBars = ScrollBars.Horizontal;
                }
                sender = box;
                this.m_setting = false;
            }
        }

        public string Description
        {
            get => 
                this.txtDescription.Text;
            set
            {
                this.txtDescription.Clear();
                this.txtDescription.Visible = false;
                char[] separator = new char[] { '\n' };
                StringBuilder builder = new StringBuilder();
                foreach (string str in value.Split(separator))
                {
                    builder.Append(Regex.Replace(str, @"\s+", " "));
                    builder.Append(Environment.NewLine);
                }
                this.txtDescription.Text = builder.ToString();
                this.txtDescription.Visible = true;
            }
        }

        public string TagText
        {
            get => 
                this.lblTags.Text;
            set
            {
                this.lblTags.Text = value;
            }
        }

        public string Title
        {
            get => 
                this.lblTitle.Text;
            set
            {
                this.lblTitle.Text = value;
                this.tblMain.RowStyles[0].Height = this.lblTitle.Height + 6;
                if (this.tblMain.RowStyles[0].Height < this.m_labelHeight)
                {
                    this.tblMain.RowStyles[0].Height = this.m_labelHeight;
                }
            }
        }
    }
}

