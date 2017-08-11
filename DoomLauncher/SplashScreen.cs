namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SplashScreen : Form
    {
        private IContainer components;

        public SplashScreen()
        {
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            base.TransparencyKey = Color.Magenta;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SplashScreen));
            base.SuspendLayout();
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(0x100, 0x100);
            base.ControlBox = false;
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "SplashScreen";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            this.Text = "SplashScreen";
            base.ResumeLayout(false);
        }
    }
}

