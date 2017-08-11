namespace DoomLauncher
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GameFileEditForm : Form
    {
        private Button btnCancel;
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private GameFileEdit gameFileEdit1;
        private TableLayoutPanel tableLayoutPanel1;

        public GameFileEditForm()
        {
            this.InitializeComponent();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(GameFileEditForm));
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.gameFileEdit1 = new GameFileEdit();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.btnCancel = new Button();
            Button button = new Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.gameFileEdit1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tableLayoutPanel1.Size = new Size(430, 490);
            this.tableLayoutPanel1.TabIndex = 1;
            this.gameFileEdit1.AuthorChecked = true;
            this.gameFileEdit1.CommentsChecked = true;
            this.gameFileEdit1.DescriptionChecked = true;
            this.gameFileEdit1.Dock = DockStyle.Fill;
            this.gameFileEdit1.Location = new Point(3, 3);
            this.gameFileEdit1.Name = "gameFileEdit1";
            this.gameFileEdit1.RatingChecked = true;
            this.gameFileEdit1.ReleaseDateChecked = true;
            this.gameFileEdit1.Size = new Size(0x1a8, 0x1c4);
            this.gameFileEdit1.TabIndex = 0;
            this.gameFileEdit1.TitleChecked = true;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(button);
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
            button.DialogResult = DialogResult.OK;
            button.Location = new Point(0x10f, 3);
            button.Name = "btnSave";
            button.Size = new Size(0x4b, 0x17);
            button.TabIndex = 1;
            button.Text = "Save";
            button.UseVisualStyleBackColor = true;
            base.AcceptButton = button;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(430, 490);
            base.Controls.Add(this.tableLayoutPanel1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "GameFileEditForm";
            base.ShowIcon = false;
            this.Text = "Edit";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public GameFileEdit EditControl =>
            this.gameFileEdit1;
    }
}

