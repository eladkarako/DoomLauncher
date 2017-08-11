namespace DoomLauncher
{
    using DoomLauncher.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class RatingControl : UserControl
    {
        private IContainer components;
        private int m_columnCount = 5;
        private List<PictureBox> m_pictures = new List<PictureBox>();
        private int m_selectedRating;
        private TableLayoutPanelDB tblMain;

        public RatingControl()
        {
            this.InitializeComponent();
            this.InitPictures();
        }

        private PictureBox CreatePictureBox(Image img)
        {
            PictureBox box1 = new PictureBox {
                Image = img,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            box1.MouseHover += new EventHandler(this.pb_MouseEnter);
            box1.MouseDown += new MouseEventHandler(this.pb_MouseDown);
            box1.Margin = new Padding(2, 2, 2, 2);
            box1.Dock = DockStyle.Fill;
            return box1;
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
            this.tblMain = new TableLayoutPanelDB();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 5;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 1;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.Size = new Size(0x89, 0x1d);
            this.tblMain.TabIndex = 0;
            this.tblMain.MouseLeave += new EventHandler(this.tblMain_MouseLeave);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblMain);
            base.Name = "RatingControl";
            base.Size = new Size(0x89, 0x1d);
            base.ResumeLayout(false);
        }

        private void InitPictures()
        {
            foreach (PictureBox box in this.m_pictures)
            {
                this.tblMain.Controls.Remove(box);
                box.Dispose();
            }
            this.m_pictures.Clear();
            for (int i = 0; i < this.m_columnCount; i++)
            {
                PictureBox item = null;
                if (i <= this.m_selectedRating)
                {
                    item = this.CreatePictureBox(Resources.bon2b);
                }
                else
                {
                    item = this.CreatePictureBox(Resources.bon2a);
                }
                this.m_pictures.Add(item);
                this.tblMain.Controls.Add(item, i, 0);
            }
        }

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            for (int i = 0; i < this.m_pictures.Count; i++)
            {
                if (this.m_pictures[i].Equals(box))
                {
                    this.m_selectedRating = i;
                    break;
                }
            }
            this.InitPictures();
        }

        private void pb_MouseEnter(object sender, EventArgs e)
        {
        }

        private void tblMain_MouseLeave(object sender, EventArgs e)
        {
        }

        public int RatingCount =>
            this.m_columnCount;

        public int SelectedRating
        {
            get => 
                (this.m_selectedRating + 1);
            set
            {
                this.m_selectedRating = value - 1;
                this.InitPictures();
            }
        }
    }
}

