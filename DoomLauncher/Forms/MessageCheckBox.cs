namespace DoomLauncher.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MessageCheckBox : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private CheckBox checkBox1;
        private IContainer components;
        private FlowLayoutPanel flpButtons;
        private Label lblText;
        private int m_height;
        private Icon m_icon;
        private TableLayoutPanel tblMain;
        private TableLayoutPanel tblMessage;

        public MessageCheckBox(string title, string text, string checkBoxText, Icon icon) : this(title, text, checkBoxText, icon, MessageBoxButtons.OK)
        {
        }

        public MessageCheckBox(string title, string text, string checkBoxText, Icon icon, MessageBoxButtons buttons)
        {
            this.InitializeComponent();
            if ((buttons != MessageBoxButtons.OK) && (buttons != MessageBoxButtons.OKCancel))
            {
                throw new NotSupportedException("Only MessageBoxButtons OK or OKCancel are supported");
            }
            base.StartPosition = FormStartPosition.CenterParent;
            if (buttons == MessageBoxButtons.OK)
            {
                this.btnCancel.Visible = false;
            }
            this.Text = title;
            this.lblText.Text = text;
            this.checkBox1.Text = checkBoxText;
            this.m_icon = icon;
            this.tblMessage.Paint += new PaintEventHandler(this.tblMessage_Paint);
            this.lblText.Anchor = AnchorStyles.Left;
            this.m_height = base.Height;
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
            this.checkBox1 = new CheckBox();
            this.tblMessage = new TableLayoutPanel();
            this.lblText = new Label();
            this.flpButtons = new FlowLayoutPanel();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.tblMain.SuspendLayout();
            this.tblMessage.SuspendLayout();
            this.flpButtons.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.checkBox1, 0, 2);
            this.tblMain.Controls.Add(this.tblMessage, 0, 0);
            this.tblMain.Controls.Add(this.flpButtons, 0, 1);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 64f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(400, 0x7e);
            this.tblMain.TabIndex = 0;
            this.checkBox1.Anchor = AnchorStyles.Left;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(3, 0x67);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x97, 0x11);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Don't show this error again";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.tblMessage.ColumnCount = 2;
            this.tblMessage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 64f));
            this.tblMessage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMessage.Controls.Add(this.lblText, 1, 0);
            this.tblMessage.Dock = DockStyle.Fill;
            this.tblMessage.Location = new Point(3, 3);
            this.tblMessage.Name = "tblMessage";
            this.tblMessage.RowCount = 1;
            this.tblMessage.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMessage.Size = new Size(0x18a, 0x3a);
            this.tblMessage.TabIndex = 2;
            this.lblText.AutoSize = true;
            this.lblText.Location = new Point(0x43, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new Size(0, 13);
            this.lblText.TabIndex = 0;
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = DockStyle.Fill;
            this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
            this.flpButtons.Location = new Point(0, 0x40);
            this.flpButtons.Margin = new Padding(0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new Size(400, 0x20);
            this.flpButtons.TabIndex = 3;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x142, 6);
            this.btnCancel.Margin = new Padding(3, 6, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = AnchorStyles.None;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0xf1, 6);
            this.btnOK.Margin = new Padding(3, 6, 3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(400, 0x7e);
            base.Controls.Add(this.tblMain);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MessageCheckBox";
            base.ShowIcon = false;
            this.Text = "MessageCheckBox";
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.tblMessage.ResumeLayout(false);
            this.tblMessage.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void SetShowCheckBox(bool set)
        {
            this.checkBox1.Visible = set;
            if (set)
            {
                base.Height = this.m_height;
            }
            else
            {
                base.Height -= 0x18;
            }
        }

        private void tblMessage_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawIcon(this.m_icon, 12, 12);
        }

        public bool Checked =>
            this.checkBox1.Checked;
    }
}

