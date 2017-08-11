namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using DoomLauncher.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class PlayForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private Button btnSaveSettings;
        private CheckBox chkDemo;
        private CheckBox chkMap;
        private CheckBox chkPreview;
        private CheckBox chkRecord;
        private CheckBox chkRemember;
        private CheckBox chkSaveStats;
        private ComboBox cmbDemo;
        private ComboBox cmbIwad;
        private ComboBox cmbMap;
        private ComboBox cmbSkill;
        private ComboBox cmbSourcePorts;
        private IContainer components;
        private FilesCtrl ctrlFiles;
        private FlowLayoutPanel flpButtons;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lblInfo;
        private LinkLabel lnkMore;
        private LinkLabel lnkSpecific;
        private ITabView[] m_additionalFileViews;
        private bool m_init;
        private List<IGameFileDataSource> m_iwadAdditionalFiles = new List<IGameFileDataSource>();
        private List<IGameFileDataSource> m_sourcePortAdditionalFiles = new List<IGameFileDataSource>();
        private Panel panel1;
        private PictureBox pbInfo;
        private Panel pnlBottom;
        private Panel pnlLeft;
        private TableLayoutPanel tblFiles;
        private TableLayoutPanel tblInner;
        private TableLayoutPanel tblMain;
        private TextBox txtDescription;
        private TextBox txtParameters;

        [field: CompilerGenerated]
        public event EventHandler SaveSettings;

        public PlayForm()
        {
            this.InitializeComponent();
            this.ctrlFiles.Initialize("GameFileID", "FileName");
            this.ctrlFiles.CellFormatting += new AdditionalFilesEventHanlder(this.ctrlFiles_CellFormatting);
            this.ctrlFiles.NewItemNeeded += new AdditionalFilesEventHanlder(this.ctrlFiles_NewItemNeeded);
            this.ctrlFiles.ItemRemoving += new AdditionalFilesEventHanlder(this.CtrlFiles_ItemRemoving);
        }

        private void AddExtraAdditionalFiles(AddFilesType type)
        {
            ISourcePortDataSource selectedItem = this.cmbSourcePorts.SelectedItem as ISourcePortDataSource;
            IGameFileDataSource selectedIWad = this.SelectedIWad;
            if (((selectedIWad != null) && this.ShouldAddExtraAdditionalFiles()) && this.AssertTypeObject(type, selectedIWad, selectedItem))
            {
                List<IGameFileDataSource> source = this.ctrlFiles.GetFiles().Cast<IGameFileDataSource>().ToList<IGameFileDataSource>();
                List<IGameFileDataSource> second = source.ToList<IGameFileDataSource>();
                List<IGameFileDataSource> newTypeFiles = this.GetAdditionalFiles(type, selectedIWad, selectedItem);
                List<IGameFileDataSource> memberTypeFiles = this.FileListMemberForType(type);
                memberTypeFiles.RemoveAll(x => newTypeFiles.Contains(x));
                source.RemoveAll(x => memberTypeFiles.Contains(x));
                memberTypeFiles.Clear();
                memberTypeFiles.AddRange(newTypeFiles);
                source.AddRange(memberTypeFiles);
                this.ctrlFiles.SetDataSource(source.Distinct<IGameFileDataSource>().ToList<IGameFileDataSource>());
                this.ResetSpecificFilesSelections(source.Except<IGameFileDataSource>(second).ToArray<IGameFileDataSource>());
            }
        }

        private bool AssertTypeObject(AddFilesType type, IGameFileDataSource iwad, ISourcePortDataSource sourcePort)
        {
            if (type != AddFilesType.SourcePort)
            {
                return ((type == AddFilesType.IWAD) && (iwad > null));
            }
            return (sourcePort > null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string text = null;
            if (this.chkRecord.Checked && string.IsNullOrEmpty(this.txtDescription.Text))
            {
                text = "Please enter a name for the demo to record.";
            }
            if (text == null)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(this, text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.DialogResult = DialogResult.None;
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (this.SaveSettings != null)
            {
                this.SaveSettings(this, new EventArgs());
            }
        }

        private void chkDemo_CheckedChanged(object sender, EventArgs e)
        {
            this.txtDescription.Enabled = false;
            this.cmbDemo.Enabled = this.chkDemo.Checked;
            this.chkRecord.CheckedChanged -= new EventHandler(this.chkRecord_CheckedChanged);
            this.chkRecord.Checked = false;
            this.chkRecord.CheckedChanged += new EventHandler(this.chkRecord_CheckedChanged);
        }

        private void chkMap_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbMap.Enabled = this.cmbSkill.Enabled = this.chkMap.Checked;
        }

        private void chkRecord_CheckedChanged(object sender, EventArgs e)
        {
            this.txtDescription.Enabled = this.chkRecord.Checked;
            this.cmbDemo.Enabled = false;
            this.chkDemo.CheckedChanged -= new EventHandler(this.chkDemo_CheckedChanged);
            this.chkDemo.Checked = false;
            this.chkDemo.CheckedChanged += new EventHandler(this.chkDemo_CheckedChanged);
        }

        private void cmbIwad_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.AddExtraAdditionalFiles(AddFilesType.IWAD);
        }

        private void cmbSourcePorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.cmbSourcePorts.SelectedItem != null) && (this.GameFile != null))
            {
                ISourcePortDataSource sourcePort = this.cmbSourcePorts.SelectedItem as ISourcePortDataSource;
                if (this.GameFile.GameFileID.HasValue)
                {
                    IEnumerable<IFileDataSource> source = from x in this.DataSourceAdapter.GetFiles(this.GameFile, FileType.Demo)
                        where x.SourcePortID == sourcePort.SourcePortID
                        select x;
                    this.cmbDemo.DataSource = source.ToList<IFileDataSource>();
                }
                this.chkSaveStats.Enabled = this.SaveStatisticsSupported(sourcePort);
                this.AddExtraAdditionalFiles(AddFilesType.SourcePort);
            }
        }

        private void ctrlFiles_CellFormatting(object sender, AdditionalFilesEventArgs e)
        {
            IGameFileDataSource item = e.Item as IGameFileDataSource;
            IGameFileDataSource selectedIWad = this.SelectedIWad;
            ISourcePortDataSource selectedValue = this.cmbSourcePorts.SelectedValue as ISourcePortDataSource;
            if (this.m_iwadAdditionalFiles.Contains(item))
            {
                e.DisplayText = $"{item.FileName} ({selectedIWad.FileName})";
            }
            if (this.m_sourcePortAdditionalFiles.Contains(item))
            {
                e.DisplayText = $"{item.FileName} ({selectedValue.Name})";
            }
        }

        private void CtrlFiles_ItemRemoving(object sender, AdditionalFilesEventArgs e)
        {
            if (e.Item.Equals(this.GameFile))
            {
                MessageBox.Show(this, $"Cannot remove {this.GameFile.FileName}. This is the file you will be launching!", "Cannot Remove", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                e.Cancel = true;
            }
        }

        private void ctrlFiles_NewItemNeeded(object sender, AdditionalFilesEventArgs e)
        {
            using (FileSelectForm form = new FileSelectForm())
            {
                form.Initialize(this.DataSourceAdapter, this.m_additionalFileViews);
                form.MultiSelect = true;
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    IGameFileDataSource[] second = new IGameFileDataSource[] { this.GameFile };
                    IGameFileDataSource[] source = form.SelectedFiles.Except<IGameFileDataSource>(second).ToArray<IGameFileDataSource>();
                    if (source.Length != 0)
                    {
                        e.NewItems = source.Cast<object>().ToList<object>();
                        try
                        {
                            IGameFileDataSource[] selectedFiles = new IGameFileDataSource[] { source.First<IGameFileDataSource>() };
                            this.ResetSpecificFilesSelections(selectedFiles);
                        }
                        catch (FileNotFoundException exception)
                        {
                            MessageBox.Show(this, $"The Game File {exception.FileName} is missing from the library.", "File Not Found");
                        }
                        catch (Exception exception2)
                        {
                            DoomLauncher.Util.DisplayUnexpectedException(this, exception2);
                        }
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

        private List<IGameFileDataSource> FileListMemberForType(AddFilesType type)
        {
            if (type != AddFilesType.SourcePort)
            {
                if (type == AddFilesType.IWAD)
                {
                    return this.m_iwadAdditionalFiles;
                }
                return null;
            }
            return this.m_sourcePortAdditionalFiles;
        }

        public List<IGameFileDataSource> GetAdditionalFiles() => 
            this.ctrlFiles.GetFiles().Cast<IGameFileDataSource>().ToList<IGameFileDataSource>();

        private List<IGameFileDataSource> GetAdditionalFiles(AddFilesType type, IGameFileDataSource gameIwad, ISourcePortDataSource sourcePort)
        {
            if (type != AddFilesType.SourcePort)
            {
                if (type == AddFilesType.IWAD)
                {
                    return DoomLauncher.Util.GetAdditionalFiles(this.DataSourceAdapter, gameIwad);
                }
                return null;
            }
            return DoomLauncher.Util.GetAdditionalFiles(this.DataSourceAdapter, sourcePort);
        }

        private IGameFileDataSource GetSelectedIWad()
        {
            IIWadDataSource selectedItem = this.cmbIwad.SelectedItem as IIWadDataSource;
            if (selectedItem != null)
            {
                GameFileGetOptions options = new GameFileGetOptions(new GameFileSearchField(GameFileFieldType.GameFileID, selectedItem.GameFileID.ToString()));
                return this.DataSourceAdapter.GetGameFiles(options).FirstOrDefault<IGameFileDataSource>();
            }
            return null;
        }

        private string[] GetSupportedExtensions()
        {
            string[] strArray = new string[0];
            if (this.SelectedSourcePort != null)
            {
                string[] separator = new string[] { ", ", "," };
                strArray = this.SelectedSourcePort.SupportedExtensions.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }
            return strArray;
        }

        public void Initialize(IEnumerable<ITabView> additionalFileViews, LauncherPath gameFileDirectory, IDataSourceAdapter adapter, IGameFileDataSource gameFile)
        {
            this.m_init = true;
            this.m_additionalFileViews = additionalFileViews.ToArray<ITabView>();
            this.GameFileDirectory = gameFileDirectory;
            this.DataSourceAdapter = adapter;
            this.GameFile = gameFile;
            if (gameFile != null)
            {
                this.Text = "Run - " + (string.IsNullOrEmpty(gameFile.Title) ? gameFile.FileName : gameFile.Title);
            }
            this.cmbSourcePorts.DataSource = adapter.GetSourcePorts();
            this.cmbIwad.DataSource = adapter.GetIWads();
            if ((gameFile != null) && !string.IsNullOrEmpty(gameFile.Map))
            {
                string[] separator = new string[] { ", ", "," };
                this.cmbMap.DataSource = gameFile.Map.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }
            this.cmbSkill.DataSource = DoomLauncher.Util.GetSkills();
            this.cmbSkill.SelectedItem = "3";
            if ((gameFile != null) && this.IsIwad(gameFile))
            {
                this.pbInfo.Image = Resources.bon2b;
                this.lblInfo.Text = $"These files will automatically be added{Environment.NewLine} when this IWAD is selected for play.";
            }
            else
            {
                this.tblFiles.RowStyles[0].Height = 0f;
            }
        }

        public void InitializeComplete()
        {
            IGameFileDataSource selectedIWad = this.SelectedIWad;
            if ((selectedIWad != null) && selectedIWad.Equals(this.GameFile))
            {
                this.cmbIwad.Enabled = false;
            }
            this.AddExtraAdditionalFiles(AddFilesType.SourcePort);
            this.AddExtraAdditionalFiles(AddFilesType.IWAD);
            this.SetExtraAdditionalFilesFromSettings();
            this.m_init = false;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PlayForm));
            this.cmbSourcePorts = new ComboBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.cmbIwad = new ComboBox();
            this.chkRemember = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.label2 = new Label();
            this.label1 = new Label();
            this.cmbMap = new ComboBox();
            this.label4 = new Label();
            this.cmbSkill = new ComboBox();
            this.chkRecord = new CheckBox();
            this.txtDescription = new TextBox();
            this.cmbDemo = new ComboBox();
            this.chkDemo = new CheckBox();
            this.chkMap = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.label3 = new Label();
            this.txtParameters = new TextBox();
            this.chkPreview = new CheckBox();
            this.lnkMore = new LinkLabel();
            this.chkSaveStats = new CheckBox();
            this.groupBox4 = new GroupBox();
            this.lnkSpecific = new LinkLabel();
            this.groupBox3 = new GroupBox();
            this.tblFiles = new TableLayoutPanel();
            this.panel1 = new Panel();
            this.lblInfo = new Label();
            this.pbInfo = new PictureBox();
            this.tblMain = new TableLayoutPanel();
            this.tblInner = new TableLayoutPanel();
            this.pnlLeft = new Panel();
            this.pnlBottom = new Panel();
            this.btnSaveSettings = new Button();
            this.flpButtons = new FlowLayoutPanel();
            this.ctrlFiles = new FilesCtrl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tblFiles.SuspendLayout();
            this.panel1.SuspendLayout();
            ((ISupportInitialize) this.pbInfo).BeginInit();
            this.tblMain.SuspendLayout();
            this.tblInner.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.flpButtons.SuspendLayout();
            base.SuspendLayout();
            this.cmbSourcePorts.DisplayMember = "Name";
            this.cmbSourcePorts.FormattingEnabled = true;
            this.cmbSourcePorts.Location = new Point(0x2f, 0x13);
            this.cmbSourcePorts.Name = "cmbSourcePorts";
            this.cmbSourcePorts.Size = new Size(0xc5, 0x15);
            this.cmbSourcePorts.TabIndex = 0;
            this.cmbSourcePorts.SelectedIndexChanged += new EventHandler(this.cmbSourcePorts_SelectedIndexChanged);
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x29, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x7a, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.cmbIwad.DisplayMember = "FileName";
            this.cmbIwad.FormattingEnabled = true;
            this.cmbIwad.Location = new Point(0x2f, 0x2e);
            this.cmbIwad.Name = "cmbIwad";
            this.cmbIwad.Size = new Size(0xc5, 0x15);
            this.cmbIwad.TabIndex = 3;
            this.cmbIwad.ValueMember = "GameFileID";
            this.cmbIwad.SelectedIndexChanged += new EventHandler(this.cmbIwad_SelectedIndexChanged);
            this.chkRemember.AutoSize = true;
            this.chkRemember.Checked = true;
            this.chkRemember.CheckState = CheckState.Checked;
            this.chkRemember.Location = new Point(9, 9);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new Size(0x76, 0x11);
            this.chkRemember.TabIndex = 4;
            this.chkRemember.Text = "Remember Settings";
            this.chkRemember.UseVisualStyleBackColor = true;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbSourcePorts);
            this.groupBox1.Controls.Add(this.cmbIwad);
            this.groupBox1.Location = new Point(3, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xfc, 80);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 0x31);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "IWAD";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 0x16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1a, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Port";
            this.cmbMap.DisplayMember = "Name";
            this.cmbMap.Enabled = false;
            this.cmbMap.FormattingEnabled = true;
            this.cmbMap.Location = new Point(0x52, 0x13);
            this.cmbMap.Name = "cmbMap";
            this.cmbMap.Size = new Size(160, 0x15);
            this.cmbMap.TabIndex = 6;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x17, 0x31);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1a, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Skill";
            this.cmbSkill.DisplayMember = "Name";
            this.cmbSkill.Enabled = false;
            this.cmbSkill.FormattingEnabled = true;
            this.cmbSkill.Location = new Point(0x52, 0x2e);
            this.cmbSkill.Name = "cmbSkill";
            this.cmbSkill.Size = new Size(160, 0x15);
            this.cmbSkill.TabIndex = 10;
            this.chkRecord.AutoSize = true;
            this.chkRecord.Location = new Point(7, 0x67);
            this.chkRecord.Name = "chkRecord";
            this.chkRecord.Size = new Size(0x3d, 0x11);
            this.chkRecord.TabIndex = 12;
            this.chkRecord.Text = "Record";
            this.chkRecord.UseVisualStyleBackColor = true;
            this.chkRecord.CheckedChanged += new EventHandler(this.chkRecord_CheckedChanged);
            this.txtDescription.Enabled = false;
            this.txtDescription.Location = new Point(0x52, 100);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new Size(160, 20);
            this.txtDescription.TabIndex = 13;
            this.cmbDemo.DisplayMember = "Description";
            this.cmbDemo.Enabled = false;
            this.cmbDemo.FormattingEnabled = true;
            this.cmbDemo.Location = new Point(0x52, 0x49);
            this.cmbDemo.Name = "cmbDemo";
            this.cmbDemo.Size = new Size(160, 0x15);
            this.cmbDemo.TabIndex = 14;
            this.chkDemo.AutoSize = true;
            this.chkDemo.Location = new Point(7, 0x4b);
            this.chkDemo.Name = "chkDemo";
            this.chkDemo.Size = new Size(0x4d, 0x11);
            this.chkDemo.TabIndex = 15;
            this.chkDemo.Text = "Play Demo";
            this.chkDemo.UseVisualStyleBackColor = true;
            this.chkDemo.CheckedChanged += new EventHandler(this.chkDemo_CheckedChanged);
            this.chkMap.AutoSize = true;
            this.chkMap.Location = new Point(7, 0x15);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new Size(0x2f, 0x11);
            this.chkMap.TabIndex = 0x10;
            this.chkMap.Text = "Map";
            this.chkMap.UseVisualStyleBackColor = true;
            this.chkMap.CheckedChanged += new EventHandler(this.chkMap_CheckedChanged);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtParameters);
            this.groupBox2.Controls.Add(this.cmbMap);
            this.groupBox2.Controls.Add(this.chkMap);
            this.groupBox2.Controls.Add(this.cmbSkill);
            this.groupBox2.Controls.Add(this.chkDemo);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbDemo);
            this.groupBox2.Controls.Add(this.chkRecord);
            this.groupBox2.Controls.Add(this.txtDescription);
            this.groupBox2.Location = new Point(3, 0x5d);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x101, 0x9b);
            this.groupBox2.TabIndex = 0x11;
            this.groupBox2.TabStop = false;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(6, 0x81);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x45, 13);
            this.label3.TabIndex = 0x12;
            this.label3.Text = "Extra Params";
            this.txtParameters.Location = new Point(0x52, 0x7e);
            this.txtParameters.Name = "txtParameters";
            this.txtParameters.Size = new Size(160, 20);
            this.txtParameters.TabIndex = 0x11;
            this.chkPreview.AutoSize = true;
            this.chkPreview.Location = new Point(6, 0x2b);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new Size(0x9f, 0x11);
            this.chkPreview.TabIndex = 0x15;
            this.chkPreview.Text = "Preview Launch Parameters";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.lnkMore.AutoSize = true;
            this.lnkMore.Location = new Point(0x6c, 20);
            this.lnkMore.Name = "lnkMore";
            this.lnkMore.Size = new Size(0x3d, 13);
            this.lnkMore.TabIndex = 20;
            this.lnkMore.TabStop = true;
            this.lnkMore.Text = "More Info...";
            this.lnkMore.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkMore_LinkClicked);
            this.chkSaveStats.AutoSize = true;
            this.chkSaveStats.Checked = true;
            this.chkSaveStats.CheckState = CheckState.Checked;
            this.chkSaveStats.Location = new Point(6, 20);
            this.chkSaveStats.Name = "chkSaveStats";
            this.chkSaveStats.Size = new Size(0x60, 0x11);
            this.chkSaveStats.TabIndex = 0x13;
            this.chkSaveStats.Text = "Save Statistics";
            this.chkSaveStats.UseVisualStyleBackColor = true;
            this.groupBox4.Controls.Add(this.chkPreview);
            this.groupBox4.Controls.Add(this.chkSaveStats);
            this.groupBox4.Controls.Add(this.lnkMore);
            this.groupBox4.Location = new Point(3, 0xfe);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x101, 0x44);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.lnkSpecific.Anchor = AnchorStyles.Left;
            this.lnkSpecific.AutoSize = true;
            this.lnkSpecific.Location = new Point(3, 0x125);
            this.lnkSpecific.Name = "lnkSpecific";
            this.lnkSpecific.Size = new Size(0x76, 13);
            this.lnkSpecific.TabIndex = 0x13;
            this.lnkSpecific.TabStop = true;
            this.lnkSpecific.Text = "Select Individual Files...";
            this.lnkSpecific.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkSpecific_LinkClicked);
            this.groupBox3.Controls.Add(this.tblFiles);
            this.groupBox3.Dock = DockStyle.Fill;
            this.groupBox3.Location = new Point(0x109, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xec, 0x14b);
            this.groupBox3.TabIndex = 0x12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Additional Files / Load Order";
            this.tblFiles.ColumnCount = 1;
            this.tblFiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblFiles.Controls.Add(this.ctrlFiles, 0, 1);
            this.tblFiles.Controls.Add(this.lnkSpecific, 0, 2);
            this.tblFiles.Controls.Add(this.panel1, 0, 0);
            this.tblFiles.Dock = DockStyle.Fill;
            this.tblFiles.Location = new Point(3, 0x10);
            this.tblFiles.Margin = new Padding(0);
            this.tblFiles.Name = "tblFiles";
            this.tblFiles.RowCount = 3;
            this.tblFiles.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            this.tblFiles.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblFiles.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblFiles.Size = new Size(230, 0x138);
            this.tblFiles.TabIndex = 0;
            this.panel1.Controls.Add(this.lblInfo);
            this.panel1.Controls.Add(this.pbInfo);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Margin = new Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(230, 40);
            this.panel1.TabIndex = 0x15;
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new Point(0x19, 7);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new Size(0x1d, 13);
            this.lblInfo.TabIndex = 0x19;
            this.lblInfo.Text = "label";
            this.pbInfo.Location = new Point(3, 7);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new Size(0x10, 0x10);
            this.pbInfo.TabIndex = 0x18;
            this.pbInfo.TabStop = false;
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.tblInner, 0, 0);
            this.tblMain.Controls.Add(this.pnlBottom, 0, 1);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.Size = new Size(0x1f8, 0x171);
            this.tblMain.TabIndex = 0x15;
            this.tblInner.ColumnCount = 2;
            this.tblInner.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 262f));
            this.tblInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblInner.Controls.Add(this.pnlLeft, 0, 0);
            this.tblInner.Controls.Add(this.groupBox3, 1, 0);
            this.tblInner.Dock = DockStyle.Fill;
            this.tblInner.Location = new Point(0, 0);
            this.tblInner.Margin = new Padding(0);
            this.tblInner.Name = "tblInner";
            this.tblInner.RowCount = 1;
            this.tblInner.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblInner.Size = new Size(0x1f8, 0x151);
            this.tblInner.TabIndex = 0;
            this.pnlLeft.Controls.Add(this.groupBox1);
            this.pnlLeft.Controls.Add(this.groupBox4);
            this.pnlLeft.Controls.Add(this.groupBox2);
            this.pnlLeft.Dock = DockStyle.Fill;
            this.pnlLeft.Location = new Point(0, 0);
            this.pnlLeft.Margin = new Padding(0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new Size(0x106, 0x151);
            this.pnlLeft.TabIndex = 0;
            this.pnlBottom.Controls.Add(this.btnSaveSettings);
            this.pnlBottom.Controls.Add(this.flpButtons);
            this.pnlBottom.Controls.Add(this.chkRemember);
            this.pnlBottom.Dock = DockStyle.Fill;
            this.pnlBottom.Location = new Point(0, 0x151);
            this.pnlBottom.Margin = new Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new Size(0x1f8, 0x20);
            this.pnlBottom.TabIndex = 1;
            this.btnSaveSettings.Location = new Point(0x85, 5);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new Size(0x52, 0x17);
            this.btnSaveSettings.TabIndex = 6;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new EventHandler(this.btnSaveSettings_Click);
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnOK);
            this.flpButtons.Dock = DockStyle.Right;
            this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
            this.flpButtons.Location = new Point(0x130, 0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new Padding(0, 2, 0, 0);
            this.flpButtons.Size = new Size(200, 0x20);
            this.flpButtons.TabIndex = 5;
            this.ctrlFiles.Dock = DockStyle.Fill;
            this.ctrlFiles.Location = new Point(3, 0x2b);
            this.ctrlFiles.Name = "ctrlFiles";
            this.ctrlFiles.Size = new Size(0xe0, 0xf2);
            this.ctrlFiles.TabIndex = 20;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x1f8, 0x171);
            base.Controls.Add(this.tblMain);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "PlayForm";
            base.ShowIcon = false;
            this.Text = "Run";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tblFiles.ResumeLayout(false);
            this.tblFiles.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((ISupportInitialize) this.pbInfo).EndInit();
            this.tblMain.ResumeLayout(false);
            this.tblInner.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private bool IsIwad(IGameFileDataSource gameFile) => 
            (gameFile.GameFileID.HasValue && (this.DataSourceAdapter.GetIWad(gameFile.GameFileID.Value) > null));

        private void lnkMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new StatsInfo { StartPosition = FormStartPosition.CenterParent }.ShowDialog(this);
        }

        private void lnkSpecific_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SpecificFilesForm form = new SpecificFilesForm {
                StartPosition = FormStartPosition.CenterParent
            };
            List<IGameFileDataSource> gameFiles = new List<IGameFileDataSource> {
                this.GameFile
            };
            gameFiles.AddRange(this.GetAdditionalFiles());
            form.Initialize(this.GameFileDirectory, gameFiles, this.GetSupportedExtensions(), this.SpecificFiles);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                this.SpecificFiles = form.SpecificFiles;
            }
        }

        private void ResetSpecificFilesSelections(IGameFileDataSource[] selectedFiles)
        {
            foreach (IGameFileDataSource source in selectedFiles)
            {
                if (this.SpecificFiles == null)
                {
                    this.SpecificFiles = SpecificFilesForm.GetSupportedFiles(this.GameFileDirectory.GetFullPath(), this.GameFile, this.GetSupportedExtensions());
                }
                this.SpecificFiles = this.SpecificFiles.Union<string>(SpecificFilesForm.GetSupportedFiles(this.GameFileDirectory.GetFullPath(), source, this.GetSupportedExtensions())).ToArray<string>();
            }
        }

        private bool SaveStatisticsSupported(ISourcePortDataSource sourcePort) => 
            (BoomStatsReader.Supported(sourcePort) || (ZDoomStatsReader.Supported(sourcePort) || CNDoomStatsReader.Supported(sourcePort)));

        public void SetAdditionalFiles(IEnumerable<IGameFileDataSource> gameFiles)
        {
            IGameFileDataSource selectedIWad = this.SelectedIWad;
            if ((!gameFiles.Contains<IGameFileDataSource>(this.GameFile) && (selectedIWad != null)) && !selectedIWad.Equals(this.GameFile))
            {
                List<IGameFileDataSource> list1 = new List<IGameFileDataSource>();
                list1.AddRange(gameFiles);
                list1.Add(this.GameFile);
                gameFiles = list1;
            }
            this.ctrlFiles.SetDataSource(gameFiles.ToArray<IGameFileDataSource>());
        }

        private void SetExtraAdditionalFilesFromSettings()
        {
            ISourcePortDataSource selectedItem = this.cmbSourcePorts.SelectedItem as ISourcePortDataSource;
            IGameFileDataSource selectedIWad = this.SelectedIWad;
            if (selectedIWad != null)
            {
                List<IGameFileDataSource> files = this.GetAdditionalFiles();
                if (!selectedIWad.Equals(this.GameFile))
                {
                    this.m_iwadAdditionalFiles = this.GetAdditionalFiles(AddFilesType.IWAD, selectedIWad, selectedItem).FindAll(x => files.Contains(x));
                }
                this.m_sourcePortAdditionalFiles = this.GetAdditionalFiles(AddFilesType.SourcePort, selectedIWad, selectedItem).FindAll(x => files.Contains(x));
            }
        }

        private bool ShouldAddExtraAdditionalFiles()
        {
            if (this.m_init)
            {
                return !this.GameFile.LastPlayed.HasValue;
            }
            return true;
        }

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public string ExtraParameters
        {
            get => 
                this.txtParameters.Text;
            set
            {
                this.txtParameters.Text = value;
            }
        }

        public IGameFileDataSource GameFile { get; set; }

        public LauncherPath GameFileDirectory { get; set; }

        public bool PlayDemo =>
            this.chkDemo.Checked;

        public bool PreviewLaunchParameters =>
            this.chkPreview.Checked;

        public bool Record =>
            this.chkRecord.Checked;

        public string RecordDescriptionText =>
            this.txtDescription.Text;

        public bool RememberSettings =>
            this.chkRemember.Checked;

        public bool SaveStatistics =>
            (this.chkSaveStats.Enabled && this.chkSaveStats.Checked);

        public IFileDataSource SelectedDemo
        {
            get => 
                (this.cmbDemo.SelectedItem as IFileDataSource);
            set
            {
                this.cmbDemo.SelectedItem = value;
            }
        }

        public IGameFileDataSource SelectedIWad
        {
            get
            {
                if (this.cmbIwad.SelectedItem != null)
                {
                    return this.DataSourceAdapter.GetGameFileIWads().Where<IGameFileDataSource>(delegate (IGameFileDataSource x) {
                        int? gameFileID = x.GameFileID;
                        int? nullable2 = ((IIWadDataSource) this.cmbIwad.SelectedItem).GameFileID;
                        if (gameFileID.GetValueOrDefault() != nullable2.GetValueOrDefault())
                        {
                            return false;
                        }
                        return (gameFileID.HasValue == nullable2.HasValue);
                    }).FirstOrDefault<IGameFileDataSource>();
                }
                return null;
            }
            set
            {
                this.cmbIwad.SelectedItem = this.DataSourceAdapter.GetIWads().Where<IIWadDataSource>(delegate (IIWadDataSource x) {
                    int? iWadID = value.IWadID;
                    if (x.IWadID != iWadID.GetValueOrDefault())
                    {
                        return false;
                    }
                    return iWadID.HasValue;
                }).FirstOrDefault<IIWadDataSource>();
            }
        }

        public string SelectedMap
        {
            get
            {
                if (!this.chkMap.Checked)
                {
                    return null;
                }
                return (this.cmbMap.SelectedItem as string);
            }
            set
            {
                if (value == null)
                {
                    this.chkMap.Checked = false;
                }
                else
                {
                    this.chkMap.Checked = true;
                    this.cmbMap.SelectedItem = value;
                }
            }
        }

        public string SelectedSkill
        {
            get => 
                (this.cmbSkill.SelectedItem as string);
            set
            {
                this.cmbSkill.SelectedItem = value;
            }
        }

        public ISourcePortDataSource SelectedSourcePort
        {
            get => 
                (this.cmbSourcePorts.SelectedItem as ISourcePortDataSource);
            set
            {
                this.cmbSourcePorts.SelectedItem = value;
            }
        }

        public string[] SpecificFiles { get; set; }

        private enum AddFilesType
        {
            SourcePort,
            IWAD
        }
    }
}

