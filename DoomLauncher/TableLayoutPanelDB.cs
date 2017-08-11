namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class TableLayoutPanelDB : TableLayoutPanel
    {
        private IContainer components;

        public TableLayoutPanelDB()
        {
            this.DoubleBuffered = true;
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
    }
}

