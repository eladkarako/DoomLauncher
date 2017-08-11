namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MetaDataForm : Form
    {
        private Button btnAccept;
        private Button btnCancel;
        private Button button1;
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private DoomLauncher.GameFileEdit gameFileEdit1;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;

        public MetaDataForm()
        {
            this.InitializeComponent();
            this.gameFileEdit1.SetShowCheckBoxes(true);
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
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.gameFileEdit1 = new DoomLauncher.GameFileEdit();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.btnCancel = new Button();
            this.btnAccept = new Button();
            this.label1 = new Label();
            this.button1 = new Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.gameFileEdit1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tableLayoutPanel1.Size = new Size(430, 490);
            this.tableLayoutPanel1.TabIndex = 2;
            this.gameFileEdit1.AuthorChecked = false;
            this.gameFileEdit1.CommentsChecked = true;
            this.gameFileEdit1.DescriptionChecked = false;
            this.gameFileEdit1.Dock = DockStyle.Fill;
            this.gameFileEdit1.Location = new Point(3, 3);
            this.gameFileEdit1.Name = "gameFileEdit1";
            this.gameFileEdit1.RatingChecked = false;
            this.gameFileEdit1.ReleaseDateChecked = false;
            this.gameFileEdit1.Size = new Size(0x1a8, 0x19c);
            this.gameFileEdit1.TabIndex = 0;
            this.gameFileEdit1.TitleChecked = false;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.btnAccept);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new Point(0, 0x1ca);
            this.flowLayoutPanel1.Margin = new Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(430, 0x20);
            this.flowLayoutPanel1.TabIndex = 1;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x160, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnAccept.DialogResult = DialogResult.OK;
            this.btnAccept.Location = new Point(190, 3);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new Size(0x4b, 0x17);
            this.btnAccept.TabIndex = 1;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(3, 0x1a9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1a5, 0x1a);
            this.label1.TabIndex = 2;
            this.label1.Text = "The fields from id games are displayed above. To accept meta changes, check the box next to the items you wish to update and click accept.";
            this.button1.DialogResult = DialogResult.Yes;
            this.button1.Location = new Point(0x10f, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "Accept All";
            this.button1.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnAccept;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(430, 490);
            base.Controls.Add(this.tableLayoutPanel1);
            base.Name = "MetaDataForm";
            this.Text = "Metadata";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public DoomLauncher.GameFileEdit GameFileEdit =>
            this.gameFileEdit1;
    }
}

