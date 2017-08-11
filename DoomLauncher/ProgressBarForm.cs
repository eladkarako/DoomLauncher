namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class ProgressBarForm : Form
    {
        private Button btnCancel;
        private IContainer components;
        private Label lblProcess;
        private Panel panel1;
        private ProgressBar progressBar1;
        private TableLayoutPanel tblMain;

        [field: CompilerGenerated]
        public event EventHandler Cancelled;

        public ProgressBarForm()
        {
            this.InitializeComponent();
            base.Load += new EventHandler(this.ProgressBarForm_Load);
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ProgressBarForm));
            this.tblMain = new TableLayoutPanel();
            this.btnCancel = new Button();
            this.panel1 = new Panel();
            this.lblProcess = new Label();
            this.progressBar1 = new ProgressBar();
            this.tblMain.SuspendLayout();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.btnCancel, 0, 1);
            this.tblMain.Controls.Add(this.panel1, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0x11c, 0x5b);
            this.tblMain.TabIndex = 0;
            this.btnCancel.Anchor = AnchorStyles.Right;
            this.btnCancel.Location = new Point(0xce, 0x3f);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.panel1.Controls.Add(this.lblProcess);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Margin = new Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x11c, 0x3b);
            this.panel1.TabIndex = 1;
            this.lblProcess.AutoSize = true;
            this.lblProcess.Location = new Point(12, 7);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new Size(0x44, 13);
            this.lblProcess.TabIndex = 1;
            this.lblProcess.Text = "Processing...";
            this.progressBar1.Location = new Point(12, 0x17);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(260, 0x17);
            this.progressBar1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x11c, 0x5b);
            base.Controls.Add(this.tblMain);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "ProgressBarForm";
            base.ShowIcon = false;
            this.Text = "Progress";
            this.tblMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void ProgressBarForm_Load(object sender, EventArgs e)
        {
            if (base.Owner != null)
            {
                base.Location = new Point((base.Owner.Location.X + (base.Owner.Width / 2)) - (base.Width / 2), (base.Owner.Location.Y + (base.Owner.Height / 2)) - (base.Height / 2));
            }
        }

        public void SetCancelAllowed(bool set)
        {
            this.btnCancel.Visible = set;
        }

        public string DisplayText
        {
            get => 
                this.lblProcess.Text;
            set
            {
                this.lblProcess.Text = value;
            }
        }

        public int Maximum
        {
            get => 
                this.progressBar1.Maximum;
            set
            {
                this.progressBar1.Maximum = value;
            }
        }

        public int Minimum
        {
            get => 
                this.progressBar1.Minimum;
            set
            {
                this.progressBar1.Minimum = value;
            }
        }

        public System.Windows.Forms.ProgressBarStyle ProgressBarStyle
        {
            get => 
                this.progressBar1.Style;
            set
            {
                this.progressBar1.Style = value;
            }
        }

        public int Value
        {
            get => 
                this.progressBar1.Value;
            set
            {
                this.progressBar1.Value = value;
            }
        }
    }
}

