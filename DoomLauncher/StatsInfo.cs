namespace DoomLauncher
{
    using DoomLauncher.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class StatsInfo : Form
    {
        private Button btnOK;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label lblBoom;
        private Label lblCNDoom;
        private Label lblZdoom;
        private PictureBox pbInfo1;
        private TableLayoutPanel tblInfo;
        private TableLayoutPanel tblInfoOuter;
        private TableLayoutPanel tblInner;
        private TableLayoutPanel tblMain;

        public StatsInfo()
        {
            this.InitializeComponent();
            this.pbInfo1.Image = Resources.bon2b;
            this.lblZdoom.Text = "For all ZDoom based ports. Uses save games to parse statistics. This means statistics cannot be read for the last level of an episode. Items are not available. Statistics will be recorded when the game is saved or when an auto save is generated.";
            this.lblBoom.Text = "Uses the -levelstat parameter and parses the generated levelstat.txt. All statistics will be recorded when the game has exited.";
            this.lblCNDoom.Text = "Uses the -printstats parameter and parses the generated stdout.txt. All statistics will be recorded when the game has exited.";
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
            this.btnOK = new Button();
            this.tblInfoOuter = new TableLayoutPanel();
            this.tblInner = new TableLayoutPanel();
            this.label5 = new Label();
            this.lblCNDoom = new Label();
            this.lblBoom = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.lblZdoom = new Label();
            this.tblInfo = new TableLayoutPanel();
            this.label4 = new Label();
            this.pbInfo1 = new PictureBox();
            this.label3 = new Label();
            this.tblMain.SuspendLayout();
            this.tblInfoOuter.SuspendLayout();
            this.tblInner.SuspendLayout();
            this.tblInfo.SuspendLayout();
            ((ISupportInitialize) this.pbInfo1).BeginInit();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.btnOK, 0, 1);
            this.tblMain.Controls.Add(this.tblInfoOuter, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(400, 330);
            this.tblMain.TabIndex = 0;
            this.btnOK.Anchor = AnchorStyles.Right;
            this.btnOK.Location = new Point(0x142, 0x12e);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.tblInfoOuter.ColumnCount = 1;
            this.tblInfoOuter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblInfoOuter.Controls.Add(this.tblInner, 0, 1);
            this.tblInfoOuter.Controls.Add(this.tblInfo, 0, 0);
            this.tblInfoOuter.Dock = DockStyle.Fill;
            this.tblInfoOuter.Location = new Point(0, 0);
            this.tblInfoOuter.Margin = new Padding(0);
            this.tblInfoOuter.Name = "tblInfoOuter";
            this.tblInfoOuter.RowCount = 2;
            this.tblInfoOuter.RowStyles.Add(new RowStyle(SizeType.Absolute, 80f));
            this.tblInfoOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblInfoOuter.Size = new Size(400, 0x12a);
            this.tblInfoOuter.TabIndex = 1;
            this.tblInner.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tblInner.ColumnCount = 2;
            this.tblInner.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
            this.tblInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblInner.Controls.Add(this.label5, 0, 2);
            this.tblInner.Controls.Add(this.lblCNDoom, 0, 2);
            this.tblInner.Controls.Add(this.lblBoom, 1, 1);
            this.tblInner.Controls.Add(this.label2, 0, 1);
            this.tblInner.Controls.Add(this.label1, 0, 0);
            this.tblInner.Controls.Add(this.lblZdoom, 1, 0);
            this.tblInner.Dock = DockStyle.Fill;
            this.tblInner.Location = new Point(3, 0x53);
            this.tblInner.Name = "tblInner";
            this.tblInner.RowCount = 3;
            this.tblInner.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            this.tblInner.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            this.tblInner.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            this.tblInner.Size = new Size(0x18a, 0xd4);
            this.tblInner.TabIndex = 2;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(4, 0x8d);
            this.label5.Name = "label5";
            this.label5.Size = new Size(50, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "CNDoom";
            this.lblCNDoom.AutoSize = true;
            this.lblCNDoom.Location = new Point(0x55, 0x8d);
            this.lblCNDoom.Name = "lblCNDoom";
            this.lblCNDoom.Size = new Size(0, 13);
            this.lblCNDoom.TabIndex = 4;
            this.lblBoom.AutoSize = true;
            this.lblBoom.Location = new Point(0x55, 0x47);
            this.lblBoom.Name = "lblBoom";
            this.lblBoom.Size = new Size(0, 13);
            this.lblBoom.TabIndex = 3;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(4, 0x47);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x40, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "PrBoomPlus";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(4, 1);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2a, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ZDoom";
            this.lblZdoom.AutoSize = true;
            this.lblZdoom.Location = new Point(0x55, 1);
            this.lblZdoom.Name = "lblZdoom";
            this.lblZdoom.Size = new Size(0, 13);
            this.lblZdoom.TabIndex = 1;
            this.tblInfo.ColumnCount = 2;
            this.tblInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 32f));
            this.tblInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblInfo.Controls.Add(this.label4, 1, 1);
            this.tblInfo.Controls.Add(this.pbInfo1, 0, 0);
            this.tblInfo.Controls.Add(this.label3, 1, 0);
            this.tblInfo.Dock = DockStyle.Fill;
            this.tblInfo.Location = new Point(0, 0);
            this.tblInfo.Margin = new Padding(0);
            this.tblInfo.Name = "tblInfo";
            this.tblInfo.RowCount = 2;
            this.tblInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tblInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tblInfo.Size = new Size(400, 80);
            this.tblInfo.TabIndex = 3;
            this.label4.Anchor = AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x23, 0x35);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x106, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Statistics recording is supported for the following ports:";
            this.pbInfo1.Dock = DockStyle.Fill;
            this.pbInfo1.InitialImage = null;
            this.pbInfo1.Location = new Point(0, 0);
            this.pbInfo1.Margin = new Padding(0);
            this.pbInfo1.Name = "pbInfo1";
            this.pbInfo1.Size = new Size(0x20, 40);
            this.pbInfo1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pbInfo1.TabIndex = 0;
            this.pbInfo1.TabStop = false;
            this.label3.Anchor = AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x23, 7);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x169, 0x1a);
            this.label3.TabIndex = 2;
            this.label3.Text = "The 'Save Statistics' option will become available when a supported source port is selected";
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(400, 330);
            base.Controls.Add(this.tblMain);
            base.Name = "StatsInfo";
            base.ShowIcon = false;
            this.Text = "Save Statistics";
            this.tblMain.ResumeLayout(false);
            this.tblInfoOuter.ResumeLayout(false);
            this.tblInner.ResumeLayout(false);
            this.tblInner.PerformLayout();
            this.tblInfo.ResumeLayout(false);
            this.tblInfo.PerformLayout();
            ((ISupportInitialize) this.pbInfo1).EndInit();
            base.ResumeLayout(false);
        }
    }
}

