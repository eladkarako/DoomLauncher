namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DownloadForm : Form
    {
        private IContainer components;
        private DoomLauncher.DownloadView downloadView;

        public DownloadForm()
        {
            this.InitializeComponent();
            base.FormClosing += new FormClosingEventHandler(this.DownloadForm_FormClosing);
            base.Shown += new EventHandler(this.DownloadForm_Shown);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Showing = false;
            base.Hide();
        }

        private void DownloadForm_Shown(object sender, EventArgs e)
        {
            this.Showing = true;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DownloadForm));
            this.downloadView = new DoomLauncher.DownloadView();
            base.SuspendLayout();
            this.downloadView.Dock = DockStyle.Fill;
            this.downloadView.Location = new Point(0, 0);
            this.downloadView.Name = "downloadView";
            this.downloadView.Size = new Size(0x11c, 0x106);
            this.downloadView.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x11c, 0x106);
            base.Controls.Add(this.downloadView);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "DownloadForm";
            base.ShowIcon = false;
            this.Text = "Downloads";
            base.ResumeLayout(false);
        }

        public DoomLauncher.DownloadView DownloadView =>
            this.downloadView;

        public bool Showing { get; private set; }
    }
}

