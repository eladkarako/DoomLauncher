namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextBoxForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer components;
        private FlowLayoutPanel flpButtons;
        private GrowLabel lblHeader;
        private TableLayoutPanel tblMain;
        private TextBox txtText;

        public TextBoxForm() : this(true, MessageBoxButtons.OK)
        {
        }

        public TextBoxForm(bool multiline, MessageBoxButtons buttons)
        {
            this.InitializeComponent();
            if ((buttons != MessageBoxButtons.OK) && (buttons != MessageBoxButtons.OKCancel))
            {
                throw new NotSupportedException(buttons.ToString() + " not supported");
            }
            this.btnCancel.Visible = buttons == MessageBoxButtons.OKCancel;
            this.HeaderText = string.Empty;
            this.txtText.Multiline = multiline;
            if (!multiline)
            {
                base.Height = 100;
                base.Width = 300;
            }
        }

        public void AppendText(string text)
        {
            this.txtText.Text = this.txtText.Text + text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.Close();
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
            this.txtText = new TextBox();
            this.lblHeader = new GrowLabel();
            this.flpButtons = new FlowLayoutPanel();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.tblMain.SuspendLayout();
            this.flpButtons.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.txtText, 0, 1);
            this.tblMain.Controls.Add(this.lblHeader, 0, 0);
            this.tblMain.Controls.Add(this.flpButtons, 0, 2);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0x158, 0x142);
            this.tblMain.TabIndex = 0;
            this.txtText.Dock = DockStyle.Fill;
            this.txtText.Location = new Point(3, 0x23);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.ScrollBars = ScrollBars.Vertical;
            this.txtText.Size = new Size(0x152, 0xfc);
            this.txtText.TabIndex = 0;
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new Point(3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new Size(0x3e, 13);
            this.lblHeader.TabIndex = 2;
            this.lblHeader.Text = "growLabel1";
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = DockStyle.Fill;
            this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
            this.flpButtons.Location = new Point(0, 290);
            this.flpButtons.Margin = new Padding(0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new Size(0x158, 0x20);
            this.flpButtons.TabIndex = 3;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0xb9, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.Location = new Point(0x10a, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x158, 0x142);
            base.Controls.Add(this.tblMain);
            base.Name = "TextBoxForm";
            base.ShowIcon = false;
            this.Text = "TextBoxForm";
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void SelectDisplayText(int start, int length)
        {
            this.txtText.Select(start, length);
        }

        public void SetMaxLength(int length)
        {
            this.txtText.MaxLength = length;
        }

        public string DisplayText
        {
            get => 
                this.txtText.Text;
            set
            {
                this.txtText.Text = value;
            }
        }

        public string HeaderText
        {
            get => 
                this.lblHeader.Text;
            set
            {
                this.lblHeader.Text = value;
                this.tblMain.RowStyles[0].Height = this.lblHeader.Height;
            }
        }
    }
}

