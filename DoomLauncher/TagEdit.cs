namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TagEdit : UserControl
    {
        private Button btnSelect;
        private ComboBox cmbColor;
        private ComboBox cmbTab;
        private IContainer components;
        private FlowLayoutPanel flp;
        private Label label1;
        private Label label2;
        private Label label3;
        private Color? m_color;
        private Panel pnlColor;
        private TableLayoutPanel tblMain;
        private TextBox txtName;

        public TagEdit()
        {
            this.InitializeComponent();
            this.cmbTab.SelectedIndex = 0;
            this.cmbColor.SelectedIndex = 1;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.pnlColor.BackColor = dialog.Color;
                this.m_color = new Color?(dialog.Color);
            }
        }

        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnSelect.Visible = this.pnlColor.Visible = this.cmbColor.SelectedIndex == 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void GetDataSource(ITagDataSource ds)
        {
            ds.Name = this.txtName.Text;
            ds.HasTab = this.cmbTab.SelectedIndex == 0;
            ds.HasColor = this.cmbColor.SelectedIndex == 0;
            if (this.m_color.HasValue)
            {
                ds.Color = new int?(this.m_color.Value.ToArgb());
            }
            else
            {
                ds.Color = null;
            }
        }

        private void InitializeComponent()
        {
            this.tblMain = new TableLayoutPanel();
            this.label1 = new Label();
            this.label2 = new Label();
            this.txtName = new TextBox();
            this.cmbTab = new ComboBox();
            this.label3 = new Label();
            this.btnSelect = new Button();
            this.cmbColor = new ComboBox();
            this.flp = new FlowLayoutPanel();
            this.pnlColor = new Panel();
            this.tblMain.SuspendLayout();
            this.flp.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 2;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.label1, 0, 0);
            this.tblMain.Controls.Add(this.label2, 0, 1);
            this.tblMain.Controls.Add(this.txtName, 1, 0);
            this.tblMain.Controls.Add(this.cmbTab, 1, 1);
            this.tblMain.Controls.Add(this.label3, 0, 2);
            this.tblMain.Controls.Add(this.cmbColor, 1, 2);
            this.tblMain.Controls.Add(this.flp, 1, 3);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 5;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tblMain.Size = new Size(0xfb, 0x89);
            this.tblMain.TabIndex = 0;
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            this.label2.Anchor = AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 0x20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Show Tab";
            this.txtName.Dock = DockStyle.Fill;
            this.txtName.Location = new Point(0x67, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(0x91, 20);
            this.txtName.TabIndex = 2;
            this.cmbTab.Dock = DockStyle.Fill;
            this.cmbTab.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTab.FormattingEnabled = true;
            object[] items = new object[] { "Yes", "No" };
            this.cmbTab.Items.AddRange(items);
            this.cmbTab.Location = new Point(0x67, 0x1d);
            this.cmbTab.Name = "cmbTab";
            this.cmbTab.Size = new Size(0x91, 0x15);
            this.cmbTab.TabIndex = 3;
            this.label3.Anchor = AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 0x3a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4b, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Highlight Color";
            this.btnSelect.Location = new Point(30, 0);
            this.btnSelect.Margin = new Padding(3, 0, 0, 0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new Size(0x4b, 0x17);
            this.btnSelect.TabIndex = 5;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
            this.cmbColor.Dock = DockStyle.Fill;
            this.cmbColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbColor.FormattingEnabled = true;
            object[] objArray2 = new object[] { "Yes", "No" };
            this.cmbColor.Items.AddRange(objArray2);
            this.cmbColor.Location = new Point(0x67, 0x37);
            this.cmbColor.Name = "cmbColor";
            this.cmbColor.Size = new Size(0x91, 0x15);
            this.cmbColor.TabIndex = 6;
            this.cmbColor.SelectedIndexChanged += new EventHandler(this.cmbColor_SelectedIndexChanged);
            this.flp.Controls.Add(this.pnlColor);
            this.flp.Controls.Add(this.btnSelect);
            this.flp.Dock = DockStyle.Fill;
            this.flp.Location = new Point(100, 0x4e);
            this.flp.Margin = new Padding(0);
            this.flp.Name = "flp";
            this.flp.Size = new Size(0x97, 0x1a);
            this.flp.TabIndex = 7;
            this.pnlColor.BackColor = SystemColors.ActiveCaptionText;
            this.pnlColor.BorderStyle = BorderStyle.FixedSingle;
            this.pnlColor.Location = new Point(3, 0);
            this.pnlColor.Margin = new Padding(3, 0, 0, 0);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new Size(0x18, 0x18);
            this.pnlColor.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tblMain);
            base.Name = "TagEdit";
            base.Size = new Size(0xfb, 0x89);
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.flp.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void SetDataSource(ITagDataSource ds)
        {
            this.txtName.Text = ds.Name;
            this.cmbTab.SelectedIndex = ds.HasTab ? 0 : 1;
            this.cmbColor.SelectedIndex = ds.HasColor ? 0 : 1;
            if (ds.HasColor && ds.Color.HasValue)
            {
                this.m_color = new Color?(this.pnlColor.BackColor = Color.FromArgb(ds.Color.Value));
            }
        }
    }
}

