namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TagForm : Form
    {
        private IContainer components;
        private DoomLauncher.TagControl m_tabCtrl;

        public TagForm()
        {
            this.InitializeComponent();
            this.m_tabCtrl = new DoomLauncher.TagControl();
            this.m_tabCtrl.Dock = DockStyle.Fill;
            base.Controls.Add(this.m_tabCtrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Init(IDataSourceAdapter adapter)
        {
            this.m_tabCtrl.Init(adapter);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x180, 0x106);
            base.Name = "TagForm";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            this.Text = "Manage Tags";
            base.ResumeLayout(false);
        }

        public DoomLauncher.TagControl TagControl =>
            this.m_tabCtrl;
    }
}

