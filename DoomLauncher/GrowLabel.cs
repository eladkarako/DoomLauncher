namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GrowLabel : Label
    {
        private IContainer components;
        private bool mGrowing;

        public GrowLabel()
        {
            this.AutoSize = false;
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
            this.components = new Container();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.resizeLabel();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.resizeLabel();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.resizeLabel();
        }

        private void resizeLabel()
        {
            if (!this.mGrowing)
            {
                try
                {
                    this.mGrowing = true;
                    Size proposedSize = new Size(base.Width, 0x7fffffff);
                    proposedSize = TextRenderer.MeasureText(this.Text, this.Font, proposedSize, TextFormatFlags.WordBreak);
                    base.Height = proposedSize.Height;
                }
                finally
                {
                    this.mGrowing = false;
                }
            }
        }
    }
}

