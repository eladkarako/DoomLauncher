namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using DoomLauncher.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class SourcePortEditForm : Form
    {
        private Button btnSave;
        private Button button1;
        private IContainer components;
        private FilesCtrl ctrlFiles;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label lblInfo;
        private IDataSourceAdapter m_adapter;
        private ITabView[] m_additionalFileViews;
        private PictureBox pbInfo;
        private SourcePortEdit sourcePortEdit1;
        private TableLayoutPanel tblMain;

        public SourcePortEditForm()
        {
            this.InitializeComponent();
            this.pbInfo.Image = Resources.bon2b;
            this.lblInfo.Text = $"These files will automatically be added when this source port{Environment.NewLine} is selected.";
            this.ctrlFiles.Initialize("GameFileID", "FileName");
            this.ctrlFiles.NewItemNeeded += new AdditionalFilesEventHanlder(this.ctrlFiles_NewItemNeeded);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string str = null;
            if (string.IsNullOrEmpty(this.sourcePortEdit1.SourcePortName))
            {
                str = "Please enter a name for the source port.";
            }
            if (string.IsNullOrEmpty(this.sourcePortEdit1.SourcePortExec))
            {
                str = "Please select an executable for the source port.";
            }
            if (!string.IsNullOrEmpty(str))
            {
                MessageBox.Show(this, str, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.DialogResult = DialogResult.None;
            }
            else
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void ctrlFiles_NewItemNeeded(object sender, AdditionalFilesEventArgs e)
        {
            using (FileSelectForm form = new FileSelectForm())
            {
                form.Initialize(this.m_adapter, this.m_additionalFileViews);
                form.MultiSelect = true;
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    IGameFileDataSource[] selectedFiles = form.SelectedFiles;
                    if (selectedFiles.Length != 0)
                    {
                        e.NewItems = selectedFiles.Cast<object>().ToList<object>();
                    }
                }
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

        public void Initialize(IDataSourceAdapter adapter, ITabView[] additionalTabViews)
        {
            this.m_adapter = adapter;
            this.m_additionalFileViews = additionalTabViews;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SourcePortEditForm));
            this.tblMain = new TableLayoutPanel();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.button1 = new Button();
            this.btnSave = new Button();
            this.groupBox1 = new GroupBox();
            this.lblInfo = new Label();
            this.pbInfo = new PictureBox();
            this.ctrlFiles = new FilesCtrl();
            this.groupBox2 = new GroupBox();
            this.sourcePortEdit1 = new SourcePortEdit();
            this.tblMain.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize) this.pbInfo).BeginInit();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tblMain.Controls.Add(this.groupBox1, 0, 1);
            this.tblMain.Controls.Add(this.groupBox2, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 200f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 200f));
            this.tblMain.Size = new Size(0x164, 0x14e);
            this.tblMain.TabIndex = 2;
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new Point(0, 300);
            this.flowLayoutPanel1.Margin = new Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(0x164, 200);
            this.flowLayoutPanel1.TabIndex = 1;
            this.button1.DialogResult = DialogResult.Cancel;
            this.button1.Location = new Point(0x116, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.btnSave.DialogResult = DialogResult.OK;
            this.btnSave.Location = new Point(0xc5, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x4b, 0x17);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.groupBox1.Controls.Add(this.lblInfo);
            this.groupBox1.Controls.Add(this.pbInfo);
            this.groupBox1.Controls.Add(this.ctrlFiles);
            this.groupBox1.Location = new Point(3, 0x67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(350, 0xc2);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Additional Files";
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new Point(0x1f, 0x13);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new Size(0x1d, 13);
            this.lblInfo.TabIndex = 0x17;
            this.lblInfo.Text = "label";
            this.pbInfo.Location = new Point(9, 0x13);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new Size(0x10, 0x10);
            this.pbInfo.TabIndex = 0x16;
            this.pbInfo.TabStop = false;
            this.ctrlFiles.Location = new Point(9, 0x33);
            this.ctrlFiles.Name = "ctrlFiles";
            this.ctrlFiles.Size = new Size(0x14c, 0x89);
            this.ctrlFiles.TabIndex = 0x15;
            this.groupBox2.Controls.Add(this.sourcePortEdit1);
            this.groupBox2.Location = new Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(350, 0x5e);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Properties";
            this.sourcePortEdit1.Location = new Point(6, 13);
            this.sourcePortEdit1.Name = "sourcePortEdit1";
            this.sourcePortEdit1.Size = new Size(0x152, 0x4b);
            this.sourcePortEdit1.TabIndex = 3;
            base.AcceptButton = this.btnSave;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.button1;
            base.ClientSize = new Size(0x164, 0x14e);
            base.Controls.Add(this.tblMain);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "SourcePortEditForm";
            base.ShowIcon = false;
            this.Text = "Source Port";
            this.tblMain.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize) this.pbInfo).EndInit();
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void SetDataSource(ISourcePortDataSource ds)
        {
            this.sourcePortEdit1.SetDataSource(ds);
            this.ctrlFiles.SetDataSource(DoomLauncher.Util.GetAdditionalFiles(this.m_adapter, ds));
        }

        public void SetSupportedExtensions(string text)
        {
            this.sourcePortEdit1.SetSupportedExtensions(text);
        }

        public void UpdateDataSource(ISourcePortDataSource ds)
        {
            this.sourcePortEdit1.UpdateDataSource(ds);
            ds.SettingsFiles = this.ctrlFiles.GetAdditionalFilesString();
        }
    }
}

