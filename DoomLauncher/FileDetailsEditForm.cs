namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class FileDetailsEditForm : Form
    {
        private Button btnSave;
        private Button button1;
        private ComboBox cmbSourcePort;
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private Label label2;
        private TableLayoutPanelDB tblMain;
        private TextBox txtDescription;

        public FileDetailsEditForm()
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

        public void Initialize(IDataSourceAdapter adapter)
        {
            this.Initialize(adapter, null);
        }

        public void Initialize(IDataSourceAdapter adapter, IFileDataSource file)
        {
            this.DataSourceAdapter = adapter;
            this.cmbSourcePort.DisplayMember = "Name";
            this.cmbSourcePort.ValueMember = "SourcePortID";
            this.cmbSourcePort.DataSource = adapter.GetSourcePorts();
            if (file != null)
            {
                this.cmbSourcePort.SelectedValue = file.SourcePortID;
                this.txtDescription.Text = file.Description;
            }
        }

        private void InitializeComponent()
        {
            this.btnSave = new Button();
            this.button1 = new Button();
            this.tblMain = new TableLayoutPanelDB();
            this.label1 = new Label();
            this.label2 = new Label();
            this.txtDescription = new TextBox();
            this.cmbSourcePort = new ComboBox();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.tblMain.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.btnSave.DialogResult = DialogResult.OK;
            this.btnSave.Location = new Point(0x7d, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x4b, 0x17);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.button1.DialogResult = DialogResult.Cancel;
            this.button1.Location = new Point(0xce, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.tblMain.ColumnCount = 2;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.label1, 0, 0);
            this.tblMain.Controls.Add(this.label2, 0, 1);
            this.tblMain.Controls.Add(this.txtDescription, 1, 1);
            this.tblMain.Controls.Add(this.cmbSourcePort, 1, 0);
            this.tblMain.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0x180, 0x106);
            this.tblMain.TabIndex = 0;
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x3f, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source Port";
            this.label2.Anchor = AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 0x7c);
            this.label2.Name = "label2";
            this.label2.Size = new Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description";
            this.txtDescription.Dock = DockStyle.Fill;
            this.txtDescription.Location = new Point(0x67, 0x23);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new Size(0x116, 0xc0);
            this.txtDescription.TabIndex = 2;
            this.cmbSourcePort.Dock = DockStyle.Fill;
            this.cmbSourcePort.FormattingEnabled = true;
            this.cmbSourcePort.Location = new Point(0x67, 3);
            this.cmbSourcePort.Name = "cmbSourcePort";
            this.cmbSourcePort.Size = new Size(0x116, 0x15);
            this.cmbSourcePort.TabIndex = 3;
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new Point(100, 230);
            this.flowLayoutPanel1.Margin = new Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(0x11c, 0x20);
            this.flowLayoutPanel1.TabIndex = 4;
            base.AcceptButton = this.btnSave;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.button1;
            base.ClientSize = new Size(0x180, 0x106);
            base.Controls.Add(this.tblMain);
            base.Name = "FileDetailsEditForm";
            base.ShowIcon = false;
            this.Text = "Details";
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public string Description
        {
            get => 
                this.txtDescription.Text;
            set
            {
                this.txtDescription.Text = value;
            }
        }

        public ISourcePortDataSource SourcePort
        {
            get => 
                (this.cmbSourcePort.SelectedItem as ISourcePortDataSource);
            set
            {
                this.cmbSourcePort.SelectedValue = value.SourcePortID;
            }
        }
    }
}

