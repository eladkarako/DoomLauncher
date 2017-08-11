namespace DoomLauncher
{
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

    public class ScreenshotView : BasicFileView
    {
        private IContainer components;
        private FlowLayoutPanel flpScreenshots;
        private double m_aspectHeight = 9.0;
        private double m_aspectWidth = 16.0;
        private Dictionary<PictureBox, IFileDataSource> m_lookup = new Dictionary<PictureBox, IFileDataSource>();
        private List<PictureBox> m_pictureBoxes = new List<PictureBox>();

        [field: CompilerGenerated]
        public event EventHandler<RequestScreenshotsEventArgs> RequestScreenshots;

        public ScreenshotView()
        {
            this.InitializeComponent();
        }

        private PictureBox CreatePictureBox()
        {
            PictureBox box1 = new PictureBox {
                WaitOnLoad = false,
                BackColor = Color.Black,
                Width = 200
            };
            box1.Height = Convert.ToInt32((double) (((double) box1.Width) / (this.m_aspectWidth / this.m_aspectHeight)));
            box1.SizeMode = PictureBoxSizeMode.StretchImage;
            box1.Margin = new Padding(7);
            box1.Click += new EventHandler(this.pbScreen_Click);
            box1.MouseDown += new MouseEventHandler(this.pbScreen_MouseDown);
            box1.LoadCompleted += new AsyncCompletedEventHandler(this.pbScreen_LoadCompleted);
            box1.ContextMenuStrip = base.m_menu;
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

        protected override List<IFileDataSource> GetSelectedFiles()
        {
            List<IFileDataSource> list = new List<IFileDataSource>();
            if (this.SelectedFile != null)
            {
                list.Add(this.SelectedFile);
            }
            return list;
        }

        private void HandleClick(object sender)
        {
            PictureBox key = sender as PictureBox;
            if ((key != null) && this.m_lookup.ContainsKey(key))
            {
                foreach (PictureBox box2 in this.m_lookup.Keys)
                {
                    this.SetSelectedStyle(box2, false);
                }
                this.SelectedFile = this.m_lookup[key];
                this.SetSelectedStyle(key, true);
            }
        }

        private void InitializeComponent()
        {
            this.flpScreenshots = new FlowLayoutPanel();
            base.SuspendLayout();
            this.flpScreenshots.AutoScroll = true;
            this.flpScreenshots.Dock = DockStyle.Fill;
            this.flpScreenshots.Location = new Point(0, 0);
            this.flpScreenshots.Name = "flpScreenshots";
            this.flpScreenshots.Size = new Size(150, 150);
            this.flpScreenshots.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.flpScreenshots);
            base.Name = "ScreenshotView";
            base.ResumeLayout(false);
        }

        private void InitPictureBoxes()
        {
            for (int i = 0; i < 50; i++)
            {
                this.m_pictureBoxes.Add(this.CreatePictureBox());
            }
        }

        private void pbScreen_Click(object sender, EventArgs e)
        {
            this.HandleClick(sender);
        }

        private void pbScreen_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
        }

        private void pbScreen_MouseDown(object sender, MouseEventArgs e)
        {
            this.HandleClick(sender);
        }

        public override void SetData(IGameFileDataSource gameFile)
        {
            if (this.m_pictureBoxes.Count == 0)
            {
                this.InitPictureBoxes();
            }
            this.flpScreenshots.SuspendLayout();
            this.flpScreenshots.Controls.Clear();
            this.flpScreenshots.ResumeLayout();
            this.m_lookup.Clear();
            if (((gameFile != null) && gameFile.GameFileID.HasValue) && (this.RequestScreenshots != null))
            {
                this.RequestScreenshots(this, new RequestScreenshotsEventArgs(gameFile));
            }
        }

        public void SetScreenshots(List<IFileDataSource> screenshots)
        {
            this.flpScreenshots.SuspendLayout();
            List<PictureBox>.Enumerator enumerator = this.m_pictureBoxes.GetEnumerator();
            foreach (IFileDataSource source in screenshots)
            {
                enumerator.MoveNext();
                if (enumerator.Current == null)
                {
                    break;
                }
                PictureBox current = enumerator.Current;
                this.flpScreenshots.Controls.Add(current);
                FileStream stream = null;
                try
                {
                    if (source.IsUrl)
                    {
                        current.CancelAsync();
                        current.Image = null;
                        current.LoadAsync(source.FileName);
                    }
                    else
                    {
                        stream = new FileStream(Path.Combine(base.DataDirectory.GetFullPath(), source.FileName), FileMode.Open, FileAccess.Read);
                        current.Image = Image.FromStream(stream);
                    }
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
                this.m_lookup.Add(current, source);
            }
            this.flpScreenshots.ResumeLayout();
        }

        private void SetSelectedStyle(PictureBox pb, bool selected)
        {
            if (selected)
            {
                pb.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                pb.BorderStyle = BorderStyle.None;
            }
        }

        public override bool EditAllowed =>
            false;

        protected override IFileDataSource[] Files =>
            this.m_lookup.Values.ToArray<IFileDataSource>();

        private IFileDataSource SelectedFile { get; set; }
    }
}

