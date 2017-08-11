namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    internal class AboutBox : Form
    {
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private Label labelProductName;
        private Label labelVersion;
        private Label lblAuthor;
        private LinkLabel lnkThread;
        private LinkLabel lnkThread2;
        private PictureBox logoPictureBox;
        private Button okButton;
        private TableLayoutPanel tableLayoutPanel;

        public AboutBox()
        {
            this.InitializeComponent();
            this.Text = $"About {this.AssemblyTitle}";
            this.labelProductName.Text = this.AssemblyProduct;
            this.labelVersion.Text = $"Version {this.AssemblyVersion}";
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AboutBox));
            this.tableLayoutPanel = new TableLayoutPanel();
            this.logoPictureBox = new PictureBox();
            this.labelProductName = new Label();
            this.labelVersion = new Label();
            this.okButton = new Button();
            this.label1 = new Label();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.lnkThread = new LinkLabel();
            this.lnkThread2 = new LinkLabel();
            this.lblAuthor = new Label();
            this.tableLayoutPanel.SuspendLayout();
            ((ISupportInitialize) this.logoPictureBox).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67f));
            this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.okButton, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.label1, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.flowLayoutPanel1, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.lblAuthor, 1, 2);
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Location = new Point(9, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tableLayoutPanel.Size = new Size(0x1a1, 0x109);
            this.tableLayoutPanel.TabIndex = 0;
            this.logoPictureBox.Anchor = AnchorStyles.Top;
            this.logoPictureBox.Image = (Image) manager.GetObject("logoPictureBox.Image");
            this.logoPictureBox.Location = new Point(4, 3);
            this.logoPictureBox.Name = "logoPictureBox";
            this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 7);
            this.logoPictureBox.Size = new Size(0x80, 0x80);
            this.logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 12;
            this.logoPictureBox.TabStop = false;
            this.labelProductName.Dock = DockStyle.Fill;
            this.labelProductName.Location = new Point(0x8f, 0);
            this.labelProductName.Margin = new Padding(6, 0, 3, 0);
            this.labelProductName.MaximumSize = new Size(0, 0x11);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new Size(0x10f, 0x11);
            this.labelProductName.TabIndex = 0x13;
            this.labelProductName.Text = "Product Name";
            this.labelProductName.TextAlign = ContentAlignment.MiddleLeft;
            this.labelVersion.BackColor = Color.Transparent;
            this.labelVersion.Dock = DockStyle.Fill;
            this.labelVersion.Location = new Point(0x8f, 0x20);
            this.labelVersion.Margin = new Padding(6, 0, 3, 0);
            this.labelVersion.MaximumSize = new Size(0, 0x11);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new Size(0x10f, 0x11);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Version";
            this.labelVersion.TextAlign = ContentAlignment.MiddleLeft;
            this.okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.okButton.DialogResult = DialogResult.Cancel;
            this.okButton.Location = new Point(0x153, 0xef);
            this.okButton.Name = "okButton";
            this.okButton.Size = new Size(0x4b, 0x17);
            this.okButton.TabIndex = 0x18;
            this.okButton.Text = "&OK";
            this.label1.AutoSize = true;
            this.label1.Dock = DockStyle.Fill;
            this.label1.Location = new Point(140, 0x60);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x112, 0x20);
            this.label1.TabIndex = 0x1a;
            this.label1.Text = "Please check out the thread to post questions, comments, bugs, and feature requests!\r\n";
            this.label1.UseMnemonic = false;
            this.flowLayoutPanel1.Controls.Add(this.lnkThread);
            this.flowLayoutPanel1.Controls.Add(this.lnkThread2);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.Location = new Point(0x89, 0x84);
            this.flowLayoutPanel1.Margin = new Padding(0, 4, 0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(280, 0x1c);
            this.flowLayoutPanel1.TabIndex = 0x1b;
            this.lnkThread.Anchor = AnchorStyles.Left;
            this.lnkThread.AutoSize = true;
            this.lnkThread.Location = new Point(3, 0);
            this.lnkThread.Name = "lnkThread";
            this.lnkThread.Size = new Size(0x61, 13);
            this.lnkThread.TabIndex = 0x1a;
            this.lnkThread.TabStop = true;
            this.lnkThread.Text = "Doomworld Thread";
            this.lnkThread.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkThread_LinkClicked);
            this.lnkThread2.AutoSize = true;
            this.lnkThread2.Location = new Point(0x6a, 0);
            this.lnkThread2.Name = "lnkThread2";
            this.lnkThread2.Size = new Size(90, 13);
            this.lnkThread2.TabIndex = 0x1b;
            this.lnkThread2.TabStop = true;
            this.lnkThread2.Text = "Realm 667 Forum";
            this.lnkThread2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkThread2_LinkClicked);
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Dock = DockStyle.Fill;
            this.lblAuthor.Location = new Point(0x8f, 0x40);
            this.lblAuthor.Margin = new Padding(6, 0, 3, 0);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new Size(0x10f, 0x20);
            this.lblAuthor.TabIndex = 0x1c;
            this.lblAuthor.Text = "Author: hobomaster22";
            this.lblAuthor.TextAlign = ContentAlignment.MiddleLeft;
            base.AcceptButton = this.okButton;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1b3, 0x11b);
            base.Controls.Add(this.tableLayoutPanel);
            this.DoubleBuffered = true;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AboutBox";
            base.Padding = new Padding(9);
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "AboutBox";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((ISupportInitialize) this.logoPictureBox).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lnkThread_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.doomworld.com/vb/doom-general/69346-doom-launcher-doom-frontend-database/");
        }

        private void lnkThread2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://realm667.com/index.php/en/kunena/doom-launcher");
        }

        public string AssemblyCompany
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute) customAttributes[0]).Company;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute) customAttributes[0]).Product;
            }
        }

        public string AssemblyTitle
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (customAttributes.Length != 0)
                {
                    AssemblyTitleAttribute attribute = (AssemblyTitleAttribute) customAttributes[0];
                    if (attribute.Title != "")
                    {
                        return attribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion =>
            Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}

