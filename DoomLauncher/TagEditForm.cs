namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TagEditForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private TagEdit m_tagEdit;
        private TableLayoutPanel tblMain;

        public TagEditForm()
        {
            this.InitializeComponent();
            this.m_tagEdit = new TagEdit();
            this.m_tagEdit.Dock = DockStyle.Fill;
            this.tblMain.Controls.Add(this.m_tagEdit, 0, 0);
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
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.tblMain.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tblMain.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0xd4, 0x9d);
            this.tblMain.TabIndex = 0;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnOK);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new Point(0, 0x7d);
            this.flowLayoutPanel1.Margin = new Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(0xd4, 0x20);
            this.flowLayoutPanel1.TabIndex = 0;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x86, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x35, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0xd4, 0x9d);
            base.Controls.Add(this.tblMain);
            base.Name = "TagEditForm";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            this.Text = "Tag";
            this.tblMain.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public TagEdit TagEditControl =>
            this.m_tagEdit;
    }
}

