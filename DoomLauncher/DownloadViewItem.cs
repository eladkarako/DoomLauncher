namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class DownloadViewItem : UserControl
    {
        private Button btnCancel;
        private IContainer components;
        private FlowLayoutPanel flp;
        private Label lblText;
        private ProgressBar progressBar;
        private TableLayoutPanel tblMain;

        [field: CompilerGenerated]
        public event EventHandler Cancelled;

        public DownloadViewItem()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.Cancelled != null)
            {
                this.Cancelled(this, new EventArgs());
            }
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
            this.tblMain = new TableLayoutPanel();
            this.progressBar = new ProgressBar();
            this.flp = new FlowLayoutPanel();
            this.btnCancel = new Button();
            this.lblText = new Label();
            this.tblMain.SuspendLayout();
            this.flp.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.progressBar, 0, 1);
            this.tblMain.Controls.Add(this.flp, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.Size = new Size(0x135, 0x74);
            this.tblMain.TabIndex = 0;
            this.progressBar.Dock = DockStyle.Top;
            this.progressBar.Location = new Point(3, 0x23);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(0x12f, 0x17);
            this.progressBar.TabIndex = 2;
            this.flp.Controls.Add(this.btnCancel);
            this.flp.Controls.Add(this.lblText);
            this.flp.Dock = DockStyle.Fill;
            this.flp.Location = new Point(0, 0);
            this.flp.Margin = new Padding(0);
            this.flp.Name = "flp";
            this.flp.Size = new Size(0x135, 0x20);
            this.flp.TabIndex = 3;
            this.btnCancel.Location = new Point(3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.lblText.Anchor = AnchorStyles.Left;
            this.lblText.AutoSize = true;
            this.lblText.Location = new Point(0x54, 8);
            this.lblText.Name = "lblText";
            this.lblText.Size = new Size(0x1c, 13);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "Text";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblMain);
            base.Name = "DownloadViewItem";
            base.Size = new Size(0x135, 0x74);
            this.tblMain.ResumeLayout(false);
            this.flp.ResumeLayout(false);
            this.flp.PerformLayout();
            base.ResumeLayout(false);
        }

        public bool CancelVisible
        {
            get => 
                this.btnCancel.Visible;
            set
            {
                this.btnCancel.Visible = value;
            }
        }

        public string DisplayText
        {
            get => 
                this.lblText.Text;
            set
            {
                this.lblText.Text = value;
            }
        }

        public object Key { get; set; }

        public int ProgressValue
        {
            get => 
                this.progressBar.Value;
            set
            {
                this.progressBar.Value = value;
            }
        }
    }
}

