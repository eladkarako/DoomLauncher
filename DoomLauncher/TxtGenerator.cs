namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TxtGenerator : Form
    {
        private Button btnCancel;
        private Button btnGenerate;
        private CheckBox chkDehacked;
        private CheckBox chkDemos;
        private CheckBox chkDifficulty;
        private CheckBox chkGraphics;
        private CheckBox chkMusic;
        private CheckBox chkOther;
        private CheckBox chkSounds;
        private ComboBox cmbBase;
        private ComboBox cmbCoop;
        private ComboBox cmbDeathmatch;
        private ComboBox cmbEngine;
        private ComboBox cmbGame;
        private ComboBox cmbPermission;
        private ComboBox cmbPrimaryPurpose;
        private ComboBox cmbSingle;
        private IContainer components;
        private DateTimePicker dtRelease;
        private FlowLayoutPanel flpButtons;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label25;
        private Label label26;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label3;
        private Label label30;
        private Label label31;
        private Label label32;
        private Label label33;
        private Label label34;
        private Label label35;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private IDataSourceAdapter m_adapter;
        private NumericUpDown numLevels;
        private TabControl tabControl;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TableLayoutPanelDB tblLayout1;
        private TableLayoutPanel tblLayout2;
        private TableLayoutPanelDB tblMain;
        private TextBox txtAdditionalCredits;
        private TextBox txtAuthor;
        private TextBox txtBuildTime;
        private TextBox txtDescription;
        private TextBox txtEditorsUsed;
        private TextBox txtEmail;
        private TextBox txtFilename;
        private TextBox txtFtpSites;
        private TextBox txtKnownBugs;
        private TextBox txtMaps;
        private TextBox txtMayNotRun;
        private TextBox txtMiscAuthor;
        private TextBox txtOtherFiles;
        private TextBox txtOtherFilesRequired;
        private TextBox txtOtherGameStyles;
        private TextBox txtTestedWith;
        private TextBox txtTile;
        private TextBox txtWebSites;

        public TxtGenerator()
        {
            this.InitializeComponent();
            PopulatePlayInfoCombo(this.cmbSingle);
            PopulatePlayInfoCombo(this.cmbCoop);
            PopulatePlayInfoCombo(this.cmbDeathmatch);
            this.cmbSingle.SelectedIndex = this.cmbCoop.SelectedIndex = this.cmbDeathmatch.SelectedIndex = this.cmbBase.SelectedIndex = this.cmbPermission.SelectedIndex = this.cmbPrimaryPurpose.SelectedIndex = 0;
            this.dtRelease.Format = DateTimePickerFormat.Custom;
            this.dtRelease.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.cmbGame.SelectedItem == null)
            {
                MessageBox.Show(this, "A game must be selected.", "Game", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                string contents = this.CreateTextFile();
                string path = "outgen.txt";
                try
                {
                    File.WriteAllText(path, contents);
                    Process.Start(path);
                }
                catch (Exception exception)
                {
                    DoomLauncher.Util.DisplayUnexpectedException(this, exception);
                }
            }
        }

        private string CreateTextFile()
        {
            string str = "You MAY not distribute this file in any format.";
            string str2 = "You MAY distribute this file, provided you include this text file, with\r\nno modifications.  You may distribute this file in any electronic\r\nformat (BBS, Diskette, CD, etc) as long as you include this file \r\nintact.  I have received permission from the original authors of any\r\nmodified or included content in this file to allow further distribution.";
            return string.Format("===========================================================================\r\nAdvanced engine needed  : {34}\r\nPrimary purpose         : {35}\r\n===========================================================================\r\nTitle                   : {0}\r\nFilename                : {1}\r\nRelease date            : {2}\r\nAuthor                  : {3}\r\nEmail Address           : {4}\r\nOther Files By Author   : {5}\r\nMisc. Author Info       : {6}\r\n\r\nDescription             : {7}\r\n\r\nAdditional Credits to   : {8}\r\n===========================================================================\r\n* What is included *\r\n\r\nNew levels              : {9}\r\nSounds                  : {10}\r\nMusic                   : {11}\r\nGraphics                : {12}\r\nDehacked/BEX Patch      : {13}\r\nDemos                   : {14}\r\nOther                   : {15}\r\nOther files required    : {16}\r\n\r\n\r\n* Play Information *\r\n\r\nGame                    : {17}\r\nMap #                   : {18}\r\nSingle Player           : {19}\r\nCooperative 2-4 Player  : {20}\r\nDeathmatch 2-4 Player   : {21}\r\nOther game styles       : {22}\r\nDifficulty Settings     : {23}\r\n\r\n\r\n* Construction *\r\n\r\nBase                    : {24}\r\nBuild Time              : {25}\r\nEditor(s) used          : {26}\r\nKnown Bugs              : {27}\r\nMay Not Run With        : {28}\r\nTested With             : {29}\r\n\r\n\r\n\r\n* Copyright / Permissions *\r\n\r\nAuthors {30} use the contents of this file as a base for\r\nmodification or reuse.  Permissions have been obtained from original \r\nauthors for any of their resources modified or included in this file.\r\n\r\n{31}\r\n\r\n* Where to get the file that this text file describes *\r\n\r\nThe Usual: ftp://archives.3dgamers.com/pub/idgames/ and mirrors\r\nWeb sites: {32}\r\nFTP sites: {33}", new object[] { 
                this.txtTile.Text, this.txtFilename.Text, this.dtRelease.Value.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern), this.txtAuthor.Text, this.txtEmail.Text, this.txtOtherFiles.Text, this.txtMiscAuthor.Text, this.txtDescription.Text, this.txtAdditionalCredits.Text, Convert.ToInt32(this.numLevels.Value), GetIncluded(this.chkSounds), GetIncluded(this.chkMusic), GetIncluded(this.chkGraphics), GetIncluded(this.chkDehacked), GetIncluded(this.chkDemos), GetIncluded(this.chkOther),
                this.txtOtherFilesRequired.Text, this.GetGameName(), this.txtMaps.Text, this.cmbSingle.SelectedItem.ToString(), this.cmbCoop.SelectedItem.ToString(), this.cmbDeathmatch.SelectedItem.ToString(), this.txtOtherGameStyles.Text, GetIncluded(this.chkDifficulty), GetBase(this.cmbBase), this.txtBuildTime.Text, this.txtEditorsUsed.Text, this.txtKnownBugs.Text, this.txtMayNotRun.Text, this.txtTestedWith.Text, (this.cmbPermission.SelectedIndex == 0) ? "MAY" : "may NOT", (this.cmbPermission.SelectedIndex == 0) ? str : str2,
                this.txtWebSites.Text, this.txtFtpSites.Text, ((ISourcePortDataSource) this.cmbEngine.SelectedItem).Name, this.cmbPrimaryPurpose.SelectedItem.ToString()
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private static string GetBase(ComboBox cmbBase)
        {
            if (cmbBase.SelectedIndex == 0)
            {
                return cmbBase.SelectedItem.ToString();
            }
            return $"{cmbBase.SelectedItem.ToString()} (-)";
        }

        private string GetGameName() => 
            (this.cmbGame.SelectedItem as IIWadDataSource).Name;

        private static string GetIncluded(CheckBox chk)
        {
            if (!chk.Checked)
            {
                return "No";
            }
            return "Yes";
        }

        private static string GetMapString(string mapString)
        {
            char[] separator = new char[] { ',' };
            string[] source = mapString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (source.Length == 1)
            {
                return source.First<string>();
            }
            if (source.Length > 1)
            {
                List<Tuple<int, string>> list = new List<Tuple<int, string>>();
                foreach (string str in source)
                {
                    int result = 0;
                    if (int.TryParse(new string((from x in str
                        where char.IsDigit(x)
                        select x).ToArray<char>()), out result))
                    {
                        list.Add(new Tuple<int, string>(result, str));
                    }
                }
                if (list.Count > 0)
                {
                    int min = list.Min<Tuple<int, string>>((Func<Tuple<int, string>, int>) (x => x.Item1));
                    int max = list.Max<Tuple<int, string>>((Func<Tuple<int, string>, int>) (x => x.Item1));
                    return $"{(from x in list
                        where x.Item1 == min
                        select x).First<Tuple<int, string>>().Item2}-{(from x in list
                        where x.Item1 == max
                        select x).First<Tuple<int, string>>().Item2}";
                }
            }
            return string.Empty;
        }

        private void InitializeComponent()
        {
            this.btnGenerate = new Button();
            this.btnCancel = new Button();
            this.tblMain = new TableLayoutPanelDB();
            this.flpButtons = new FlowLayoutPanel();
            this.tabControl = new TabControl();
            this.tabPage1 = new TabPage();
            this.tblLayout1 = new TableLayoutPanelDB();
            this.cmbPrimaryPurpose = new ComboBox();
            this.txtAdditionalCredits = new TextBox();
            this.txtDescription = new TextBox();
            this.txtMiscAuthor = new TextBox();
            this.txtOtherFiles = new TextBox();
            this.txtEmail = new TextBox();
            this.txtAuthor = new TextBox();
            this.txtFilename = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.label9 = new Label();
            this.txtTile = new TextBox();
            this.dtRelease = new DateTimePicker();
            this.label35 = new Label();
            this.cmbEngine = new ComboBox();
            this.label34 = new Label();
            this.tabPage2 = new TabPage();
            this.tblLayout2 = new TableLayoutPanel();
            this.chkGraphics = new CheckBox();
            this.chkMusic = new CheckBox();
            this.label12 = new Label();
            this.label10 = new Label();
            this.numLevels = new NumericUpDown();
            this.label11 = new Label();
            this.chkSounds = new CheckBox();
            this.label13 = new Label();
            this.label14 = new Label();
            this.label15 = new Label();
            this.label16 = new Label();
            this.label17 = new Label();
            this.chkDehacked = new CheckBox();
            this.chkDemos = new CheckBox();
            this.chkOther = new CheckBox();
            this.txtOtherFilesRequired = new TextBox();
            this.tabPage3 = new TabPage();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.txtOtherGameStyles = new TextBox();
            this.cmbDeathmatch = new ComboBox();
            this.cmbCoop = new ComboBox();
            this.cmbSingle = new ComboBox();
            this.label18 = new Label();
            this.label19 = new Label();
            this.label20 = new Label();
            this.label21 = new Label();
            this.label22 = new Label();
            this.label23 = new Label();
            this.label24 = new Label();
            this.cmbGame = new ComboBox();
            this.txtMaps = new TextBox();
            this.chkDifficulty = new CheckBox();
            this.tabPage4 = new TabPage();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.label26 = new Label();
            this.label30 = new Label();
            this.label29 = new Label();
            this.label28 = new Label();
            this.label25 = new Label();
            this.label27 = new Label();
            this.cmbBase = new ComboBox();
            this.txtBuildTime = new TextBox();
            this.txtEditorsUsed = new TextBox();
            this.txtKnownBugs = new TextBox();
            this.txtMayNotRun = new TextBox();
            this.txtTestedWith = new TextBox();
            this.tabPage5 = new TabPage();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.label31 = new Label();
            this.cmbPermission = new ComboBox();
            this.label32 = new Label();
            this.label33 = new Label();
            this.txtWebSites = new TextBox();
            this.txtFtpSites = new TextBox();
            this.tblMain.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tblLayout1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tblLayout2.SuspendLayout();
            this.numLevels.BeginInit();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            base.SuspendLayout();
            this.btnGenerate.Location = new Point(250, 3);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new Size(0x4b, 0x17);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x14b, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.flpButtons, 0, 1);
            this.tblMain.Controls.Add(this.tabControl, 0, 0);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            this.tblMain.Size = new Size(0x19f, 0x1ed);
            this.tblMain.TabIndex = 0;
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Controls.Add(this.btnGenerate);
            this.flpButtons.Dock = DockStyle.Fill;
            this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
            this.flpButtons.Location = new Point(3, 0x1c8);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new Size(0x199, 0x22);
            this.flpButtons.TabIndex = 1;
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Controls.Add(this.tabPage5);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x199, 0x1bf);
            this.tabControl.TabIndex = 2;
            this.tabPage1.Controls.Add(this.tblLayout1);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0x191, 0x1a5);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tblLayout1.ColumnCount = 2;
            this.tblLayout1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f));
            this.tblLayout1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblLayout1.Controls.Add(this.cmbPrimaryPurpose, 1, 1);
            this.tblLayout1.Controls.Add(this.txtAdditionalCredits, 1, 10);
            this.tblLayout1.Controls.Add(this.txtDescription, 1, 9);
            this.tblLayout1.Controls.Add(this.txtMiscAuthor, 1, 8);
            this.tblLayout1.Controls.Add(this.txtOtherFiles, 1, 7);
            this.tblLayout1.Controls.Add(this.txtEmail, 1, 6);
            this.tblLayout1.Controls.Add(this.txtAuthor, 1, 5);
            this.tblLayout1.Controls.Add(this.txtFilename, 1, 3);
            this.tblLayout1.Controls.Add(this.label1, 0, 2);
            this.tblLayout1.Controls.Add(this.label2, 0, 3);
            this.tblLayout1.Controls.Add(this.label3, 0, 4);
            this.tblLayout1.Controls.Add(this.label4, 0, 5);
            this.tblLayout1.Controls.Add(this.label5, 0, 6);
            this.tblLayout1.Controls.Add(this.label6, 0, 7);
            this.tblLayout1.Controls.Add(this.label7, 0, 8);
            this.tblLayout1.Controls.Add(this.label8, 0, 9);
            this.tblLayout1.Controls.Add(this.label9, 0, 10);
            this.tblLayout1.Controls.Add(this.txtTile, 1, 2);
            this.tblLayout1.Controls.Add(this.dtRelease, 1, 4);
            this.tblLayout1.Controls.Add(this.label35, 0, 1);
            this.tblLayout1.Controls.Add(this.cmbEngine, 1, 0);
            this.tblLayout1.Controls.Add(this.label34, 0, 0);
            this.tblLayout1.Dock = DockStyle.Fill;
            this.tblLayout1.Location = new Point(3, 3);
            this.tblLayout1.Name = "tblLayout1";
            this.tblLayout1.RowCount = 13;
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 160f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout1.Size = new Size(0x18b, 0x19f);
            this.tblLayout1.TabIndex = 1;
            this.cmbPrimaryPurpose.Dock = DockStyle.Fill;
            this.cmbPrimaryPurpose.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPrimaryPurpose.FormattingEnabled = true;
            object[] items = new object[] { "Single+Coop play", "Deathmatch", "No levels included" };
            this.cmbPrimaryPurpose.Items.AddRange(items);
            this.cmbPrimaryPurpose.Location = new Point(0xa3, 0x1b);
            this.cmbPrimaryPurpose.Name = "cmbPrimaryPurpose";
            this.cmbPrimaryPurpose.Size = new Size(0xe5, 0x15);
            this.cmbPrimaryPurpose.TabIndex = 0x16;
            this.txtAdditionalCredits.Dock = DockStyle.Fill;
            this.txtAdditionalCredits.Location = new Point(0xa3, 0x17b);
            this.txtAdditionalCredits.Name = "txtAdditionalCredits";
            this.txtAdditionalCredits.Size = new Size(0xe5, 20);
            this.txtAdditionalCredits.TabIndex = 0x11;
            this.txtDescription.Dock = DockStyle.Fill;
            this.txtDescription.Location = new Point(0xa3, 0xdb);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new Size(0xe5, 0x9a);
            this.txtDescription.TabIndex = 0x10;
            this.txtMiscAuthor.Dock = DockStyle.Fill;
            this.txtMiscAuthor.Location = new Point(0xa3, 0xc3);
            this.txtMiscAuthor.Name = "txtMiscAuthor";
            this.txtMiscAuthor.Size = new Size(0xe5, 20);
            this.txtMiscAuthor.TabIndex = 15;
            this.txtOtherFiles.Dock = DockStyle.Fill;
            this.txtOtherFiles.Location = new Point(0xa3, 0xab);
            this.txtOtherFiles.Name = "txtOtherFiles";
            this.txtOtherFiles.Size = new Size(0xe5, 20);
            this.txtOtherFiles.TabIndex = 14;
            this.txtEmail.Dock = DockStyle.Fill;
            this.txtEmail.Location = new Point(0xa3, 0x93);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new Size(0xe5, 20);
            this.txtEmail.TabIndex = 13;
            this.txtAuthor.Dock = DockStyle.Fill;
            this.txtAuthor.Location = new Point(0xa3, 0x7b);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new Size(0xe5, 20);
            this.txtAuthor.TabIndex = 12;
            this.txtFilename.Dock = DockStyle.Fill;
            this.txtFilename.Location = new Point(0xa3, 0x4b);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new Size(0xe5, 20);
            this.txtFilename.TabIndex = 10;
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 0x35);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1b, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            this.label2.Anchor = AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 0x4d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Filename";
            this.label3.Anchor = AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 0x65);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Release Date";
            this.label4.Anchor = AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(3, 0x7d);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x26, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Author";
            this.label5.Anchor = AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(3, 0x95);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x49, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Email Address";
            this.label6.Anchor = AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(3, 0xad);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x6a, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Other Files By Author";
            this.label7.Anchor = AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(3, 0xc5);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x57, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Misc. Author Info";
            this.label8.Anchor = AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(3, 0x121);
            this.label8.Name = "label8";
            this.label8.Size = new Size(60, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Description";
            this.label9.Anchor = AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(3, 0x17d);
            this.label9.Name = "label9";
            this.label9.Size = new Size(100, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Additional Credits to";
            this.txtTile.Dock = DockStyle.Fill;
            this.txtTile.Location = new Point(0xa3, 0x33);
            this.txtTile.Name = "txtTile";
            this.txtTile.Size = new Size(0xe5, 20);
            this.txtTile.TabIndex = 9;
            this.dtRelease.Dock = DockStyle.Fill;
            this.dtRelease.Location = new Point(0xa3, 0x63);
            this.dtRelease.Name = "dtRelease";
            this.dtRelease.Size = new Size(0xe5, 20);
            this.dtRelease.TabIndex = 0x12;
            this.label35.Anchor = AnchorStyles.Left;
            this.label35.AutoSize = true;
            this.label35.Location = new Point(3, 0x1d);
            this.label35.Name = "label35";
            this.label35.Size = new Size(0x53, 13);
            this.label35.TabIndex = 20;
            this.label35.Text = "Primary Purpose";
            this.cmbEngine.Dock = DockStyle.Fill;
            this.cmbEngine.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEngine.FormattingEnabled = true;
            this.cmbEngine.Location = new Point(0xa3, 3);
            this.cmbEngine.Name = "cmbEngine";
            this.cmbEngine.Size = new Size(0xe5, 0x15);
            this.cmbEngine.TabIndex = 0x15;
            this.label34.Anchor = AnchorStyles.Left;
            this.label34.AutoSize = true;
            this.label34.Location = new Point(3, 5);
            this.label34.Name = "label34";
            this.label34.Size = new Size(0x8a, 13);
            this.label34.TabIndex = 0x17;
            this.label34.Text = "Advanced Engine Required";
            this.tabPage2.Controls.Add(this.tblLayout2);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0x191, 0x1a5);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Included";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tblLayout2.ColumnCount = 2;
            this.tblLayout2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f));
            this.tblLayout2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblLayout2.Controls.Add(this.chkGraphics, 1, 3);
            this.tblLayout2.Controls.Add(this.chkMusic, 1, 2);
            this.tblLayout2.Controls.Add(this.label12, 0, 2);
            this.tblLayout2.Controls.Add(this.label10, 0, 0);
            this.tblLayout2.Controls.Add(this.numLevels, 1, 0);
            this.tblLayout2.Controls.Add(this.label11, 0, 1);
            this.tblLayout2.Controls.Add(this.chkSounds, 1, 1);
            this.tblLayout2.Controls.Add(this.label13, 0, 3);
            this.tblLayout2.Controls.Add(this.label14, 0, 4);
            this.tblLayout2.Controls.Add(this.label15, 0, 5);
            this.tblLayout2.Controls.Add(this.label16, 0, 6);
            this.tblLayout2.Controls.Add(this.label17, 0, 7);
            this.tblLayout2.Controls.Add(this.chkDehacked, 1, 4);
            this.tblLayout2.Controls.Add(this.chkDemos, 1, 5);
            this.tblLayout2.Controls.Add(this.chkOther, 1, 6);
            this.tblLayout2.Controls.Add(this.txtOtherFilesRequired, 1, 7);
            this.tblLayout2.Dock = DockStyle.Fill;
            this.tblLayout2.Location = new Point(3, 3);
            this.tblLayout2.Name = "tblLayout2";
            this.tblLayout2.RowCount = 11;
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 140f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tblLayout2.Size = new Size(0x18b, 0x19f);
            this.tblLayout2.TabIndex = 0;
            this.chkGraphics.AutoSize = true;
            this.chkGraphics.Location = new Point(0xa3, 0x4b);
            this.chkGraphics.Name = "chkGraphics";
            this.chkGraphics.Size = new Size(0x43, 0x11);
            this.chkGraphics.TabIndex = 0x1f;
            this.chkGraphics.Text = "Included";
            this.chkGraphics.UseVisualStyleBackColor = true;
            this.chkMusic.AutoSize = true;
            this.chkMusic.Location = new Point(0xa3, 0x33);
            this.chkMusic.Name = "chkMusic";
            this.chkMusic.Size = new Size(0x43, 0x11);
            this.chkMusic.TabIndex = 30;
            this.chkMusic.Text = "Included";
            this.chkMusic.UseVisualStyleBackColor = true;
            this.label12.Anchor = AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new Point(3, 0x35);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x23, 13);
            this.label12.TabIndex = 0x18;
            this.label12.Text = "Music";
            this.label10.Anchor = AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new Point(3, 5);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x3f, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "New Levels";
            this.numLevels.Location = new Point(0xa3, 3);
            this.numLevels.Name = "numLevels";
            this.numLevels.Size = new Size(0x29, 20);
            this.numLevels.TabIndex = 0x15;
            this.label11.Anchor = AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new Point(3, 0x1d);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x2b, 13);
            this.label11.TabIndex = 0x16;
            this.label11.Text = "Sounds";
            this.chkSounds.AutoSize = true;
            this.chkSounds.Location = new Point(0xa3, 0x1b);
            this.chkSounds.Name = "chkSounds";
            this.chkSounds.Size = new Size(0x43, 0x11);
            this.chkSounds.TabIndex = 0x17;
            this.chkSounds.Text = "Included";
            this.chkSounds.UseVisualStyleBackColor = true;
            this.label13.Anchor = AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new Point(3, 0x4d);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x31, 13);
            this.label13.TabIndex = 0x19;
            this.label13.Text = "Graphics";
            this.label14.Anchor = AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Location = new Point(3, 0x65);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x72, 13);
            this.label14.TabIndex = 0x1a;
            this.label14.Text = "Dehacked/BEX Patch";
            this.label14.TextAlign = ContentAlignment.MiddleLeft;
            this.label15.Anchor = AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.Location = new Point(3, 0x7d);
            this.label15.Name = "label15";
            this.label15.Size = new Size(40, 13);
            this.label15.TabIndex = 0x1b;
            this.label15.Text = "Demos";
            this.label15.TextAlign = ContentAlignment.MiddleLeft;
            this.label16.Anchor = AnchorStyles.Left;
            this.label16.AutoSize = true;
            this.label16.Location = new Point(3, 0x95);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x21, 13);
            this.label16.TabIndex = 0x1c;
            this.label16.Text = "Other";
            this.label16.TextAlign = ContentAlignment.MiddleLeft;
            this.label17.Anchor = AnchorStyles.Left;
            this.label17.AutoSize = true;
            this.label17.Location = new Point(3, 0xe7);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x67, 13);
            this.label17.TabIndex = 0x1d;
            this.label17.Text = "Other Files Required";
            this.label17.TextAlign = ContentAlignment.MiddleLeft;
            this.chkDehacked.AutoSize = true;
            this.chkDehacked.Location = new Point(0xa3, 0x63);
            this.chkDehacked.Name = "chkDehacked";
            this.chkDehacked.Size = new Size(0x43, 0x11);
            this.chkDehacked.TabIndex = 0x20;
            this.chkDehacked.Text = "Included";
            this.chkDehacked.UseVisualStyleBackColor = true;
            this.chkDemos.AutoSize = true;
            this.chkDemos.Location = new Point(0xa3, 0x7b);
            this.chkDemos.Name = "chkDemos";
            this.chkDemos.Size = new Size(0x43, 0x11);
            this.chkDemos.TabIndex = 0x21;
            this.chkDemos.Text = "Included";
            this.chkDemos.UseVisualStyleBackColor = true;
            this.chkOther.AutoSize = true;
            this.chkOther.Location = new Point(0xa3, 0x93);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new Size(0x43, 0x11);
            this.chkOther.TabIndex = 0x22;
            this.chkOther.Text = "Included";
            this.chkOther.UseVisualStyleBackColor = true;
            this.txtOtherFilesRequired.Dock = DockStyle.Fill;
            this.txtOtherFilesRequired.Location = new Point(0xa3, 0xab);
            this.txtOtherFilesRequired.Multiline = true;
            this.txtOtherFilesRequired.Name = "txtOtherFilesRequired";
            this.txtOtherFilesRequired.Size = new Size(0xe5, 0x86);
            this.txtOtherFilesRequired.TabIndex = 0x23;
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(0x191, 0x1a5);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Play Info";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.txtOtherGameStyles, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbDeathmatch, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmbCoop, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbSingle, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label18, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label20, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label21, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label22, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label23, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label24, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.cmbGame, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtMaps, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkDifficulty, 1, 6);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 140f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.Size = new Size(0x191, 0x1a5);
            this.tableLayoutPanel1.TabIndex = 1;
            this.txtOtherGameStyles.Dock = DockStyle.Fill;
            this.txtOtherGameStyles.Location = new Point(0xa3, 0x7b);
            this.txtOtherGameStyles.Name = "txtOtherGameStyles";
            this.txtOtherGameStyles.Size = new Size(0xeb, 20);
            this.txtOtherGameStyles.TabIndex = 0x29;
            this.cmbDeathmatch.Dock = DockStyle.Fill;
            this.cmbDeathmatch.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDeathmatch.FormattingEnabled = true;
            this.cmbDeathmatch.Location = new Point(0xa3, 0x63);
            this.cmbDeathmatch.Name = "cmbDeathmatch";
            this.cmbDeathmatch.Size = new Size(0xeb, 0x15);
            this.cmbDeathmatch.TabIndex = 40;
            this.cmbCoop.Dock = DockStyle.Fill;
            this.cmbCoop.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCoop.FormattingEnabled = true;
            this.cmbCoop.Location = new Point(0xa3, 0x4b);
            this.cmbCoop.Name = "cmbCoop";
            this.cmbCoop.Size = new Size(0xeb, 0x15);
            this.cmbCoop.TabIndex = 0x27;
            this.cmbSingle.Dock = DockStyle.Fill;
            this.cmbSingle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSingle.FormattingEnabled = true;
            this.cmbSingle.Location = new Point(0xa3, 0x33);
            this.cmbSingle.Name = "cmbSingle";
            this.cmbSingle.Size = new Size(0xeb, 0x15);
            this.cmbSingle.TabIndex = 0x26;
            this.label18.Anchor = AnchorStyles.Left;
            this.label18.AutoSize = true;
            this.label18.Location = new Point(3, 0x35);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x44, 13);
            this.label18.TabIndex = 0x18;
            this.label18.Text = "Single Player";
            this.label19.Anchor = AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new Point(3, 5);
            this.label19.Name = "label19";
            this.label19.Size = new Size(0x23, 13);
            this.label19.TabIndex = 20;
            this.label19.Text = "Game";
            this.label20.Anchor = AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.label20.Location = new Point(3, 0x1d);
            this.label20.Name = "label20";
            this.label20.Size = new Size(0x21, 13);
            this.label20.TabIndex = 0x16;
            this.label20.Text = "Maps";
            this.label21.Anchor = AnchorStyles.Left;
            this.label21.AutoSize = true;
            this.label21.Location = new Point(3, 0x4d);
            this.label21.Name = "label21";
            this.label21.Size = new Size(0x72, 13);
            this.label21.TabIndex = 0x19;
            this.label21.Text = "Cooperative 2-4 Player";
            this.label22.Anchor = AnchorStyles.Left;
            this.label22.AutoSize = true;
            this.label22.Location = new Point(3, 0x65);
            this.label22.Name = "label22";
            this.label22.Size = new Size(0x73, 13);
            this.label22.TabIndex = 0x1a;
            this.label22.Text = "Deathmatch 2-4 Player";
            this.label22.TextAlign = ContentAlignment.MiddleLeft;
            this.label23.Anchor = AnchorStyles.Left;
            this.label23.AutoSize = true;
            this.label23.Location = new Point(3, 0x7d);
            this.label23.Name = "label23";
            this.label23.Size = new Size(0x5b, 13);
            this.label23.TabIndex = 0x1b;
            this.label23.Text = "Other game styles";
            this.label23.TextAlign = ContentAlignment.MiddleLeft;
            this.label24.Anchor = AnchorStyles.Left;
            this.label24.AutoSize = true;
            this.label24.Location = new Point(3, 0x95);
            this.label24.Name = "label24";
            this.label24.Size = new Size(0x58, 13);
            this.label24.TabIndex = 0x1c;
            this.label24.Text = "Difficulty Settings";
            this.label24.TextAlign = ContentAlignment.MiddleLeft;
            this.cmbGame.Dock = DockStyle.Fill;
            this.cmbGame.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGame.FormattingEnabled = true;
            this.cmbGame.Location = new Point(0xa3, 3);
            this.cmbGame.Name = "cmbGame";
            this.cmbGame.Size = new Size(0xeb, 0x15);
            this.cmbGame.TabIndex = 0x24;
            this.txtMaps.Dock = DockStyle.Fill;
            this.txtMaps.Location = new Point(0xa3, 0x1b);
            this.txtMaps.Name = "txtMaps";
            this.txtMaps.Size = new Size(0xeb, 20);
            this.txtMaps.TabIndex = 0x25;
            this.chkDifficulty.AutoSize = true;
            this.chkDifficulty.Location = new Point(0xa3, 0x93);
            this.chkDifficulty.Name = "chkDifficulty";
            this.chkDifficulty.Size = new Size(0x2c, 0x11);
            this.chkDifficulty.TabIndex = 0x2a;
            this.chkDifficulty.Text = "Yes";
            this.chkDifficulty.UseVisualStyleBackColor = true;
            this.tabPage4.Controls.Add(this.tableLayoutPanel2);
            this.tabPage4.Location = new Point(4, 0x16);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new Size(0x191, 0x1a5);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Construction";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.Controls.Add(this.label26, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label30, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.label29, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label28, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label25, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label27, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbBase, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtBuildTime, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtEditorsUsed, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtKnownBugs, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtMayNotRun, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtTestedWith, 1, 6);
            this.tableLayoutPanel2.Dock = DockStyle.Fill;
            this.tableLayoutPanel2.Location = new Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 11;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 140f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel2.Size = new Size(0x191, 0x1a5);
            this.tableLayoutPanel2.TabIndex = 2;
            this.label26.Anchor = AnchorStyles.Left;
            this.label26.AutoSize = true;
            this.label26.Location = new Point(3, 5);
            this.label26.Name = "label26";
            this.label26.Size = new Size(0x1f, 13);
            this.label26.TabIndex = 20;
            this.label26.Text = "Base";
            this.label30.Anchor = AnchorStyles.Left;
            this.label30.AutoSize = true;
            this.label30.Location = new Point(3, 0x95);
            this.label30.Name = "label30";
            this.label30.Size = new Size(0x41, 13);
            this.label30.TabIndex = 0x1b;
            this.label30.Text = "Tested With";
            this.label30.TextAlign = ContentAlignment.MiddleLeft;
            this.label29.Anchor = AnchorStyles.Left;
            this.label29.AutoSize = true;
            this.label29.Location = new Point(3, 0x7d);
            this.label29.Name = "label29";
            this.label29.Size = new Size(0x5f, 13);
            this.label29.TabIndex = 0x1a;
            this.label29.Text = "May Not Run With";
            this.label29.TextAlign = ContentAlignment.MiddleLeft;
            this.label28.Anchor = AnchorStyles.Left;
            this.label28.AutoSize = true;
            this.label28.Location = new Point(3, 0x65);
            this.label28.Name = "label28";
            this.label28.Size = new Size(0x43, 13);
            this.label28.TabIndex = 0x19;
            this.label28.Text = "Known Bugs";
            this.label25.Anchor = AnchorStyles.Left;
            this.label25.AutoSize = true;
            this.label25.Location = new Point(3, 0x4d);
            this.label25.Name = "label25";
            this.label25.Size = new Size(0x47, 13);
            this.label25.TabIndex = 0x18;
            this.label25.Text = "Editor(s) used";
            this.label27.Anchor = AnchorStyles.Left;
            this.label27.AutoSize = true;
            this.label27.Location = new Point(3, 0x35);
            this.label27.Name = "label27";
            this.label27.Size = new Size(0x38, 13);
            this.label27.TabIndex = 0x16;
            this.label27.Text = "Build Time";
            this.cmbBase.Dock = DockStyle.Fill;
            this.cmbBase.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbBase.FormattingEnabled = true;
            object[] objArray2 = new object[] { "New from scratch", "Modified" };
            this.cmbBase.Items.AddRange(objArray2);
            this.cmbBase.Location = new Point(0xa3, 3);
            this.cmbBase.Name = "cmbBase";
            this.cmbBase.Size = new Size(0xeb, 0x15);
            this.cmbBase.TabIndex = 0x1c;
            this.txtBuildTime.Dock = DockStyle.Fill;
            this.txtBuildTime.Location = new Point(0xa3, 0x33);
            this.txtBuildTime.Name = "txtBuildTime";
            this.txtBuildTime.Size = new Size(0xeb, 20);
            this.txtBuildTime.TabIndex = 0x1d;
            this.txtEditorsUsed.Dock = DockStyle.Fill;
            this.txtEditorsUsed.Location = new Point(0xa3, 0x4b);
            this.txtEditorsUsed.Name = "txtEditorsUsed";
            this.txtEditorsUsed.Size = new Size(0xeb, 20);
            this.txtEditorsUsed.TabIndex = 30;
            this.txtKnownBugs.Dock = DockStyle.Fill;
            this.txtKnownBugs.Location = new Point(0xa3, 0x63);
            this.txtKnownBugs.Name = "txtKnownBugs";
            this.txtKnownBugs.Size = new Size(0xeb, 20);
            this.txtKnownBugs.TabIndex = 0x1f;
            this.txtMayNotRun.Dock = DockStyle.Fill;
            this.txtMayNotRun.Location = new Point(0xa3, 0x7b);
            this.txtMayNotRun.Name = "txtMayNotRun";
            this.txtMayNotRun.Size = new Size(0xeb, 20);
            this.txtMayNotRun.TabIndex = 0x20;
            this.txtTestedWith.Dock = DockStyle.Fill;
            this.txtTestedWith.Location = new Point(0xa3, 0x93);
            this.txtTestedWith.Name = "txtTestedWith";
            this.txtTestedWith.Size = new Size(0xeb, 20);
            this.txtTestedWith.TabIndex = 0x21;
            this.tabPage5.Controls.Add(this.tableLayoutPanel3);
            this.tabPage5.Location = new Point(4, 0x16);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new Size(0x191, 0x1a5);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Permissions";
            this.tabPage5.UseVisualStyleBackColor = true;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f));
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel3.Controls.Add(this.label31, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbPermission, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label32, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label33, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtWebSites, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtFtpSites, 1, 2);
            this.tableLayoutPanel3.Dock = DockStyle.Fill;
            this.tableLayoutPanel3.Location = new Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 11;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 140f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel3.Size = new Size(0x191, 0x1a5);
            this.tableLayoutPanel3.TabIndex = 3;
            this.label31.Anchor = AnchorStyles.Left;
            this.label31.AutoSize = true;
            this.label31.Location = new Point(3, 5);
            this.label31.Name = "label31";
            this.label31.Size = new Size(0x39, 13);
            this.label31.TabIndex = 20;
            this.label31.Text = "Permission";
            this.cmbPermission.Dock = DockStyle.Fill;
            this.cmbPermission.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPermission.FormattingEnabled = true;
            object[] objArray3 = new object[] { "May distribute", "May not distribute" };
            this.cmbPermission.Items.AddRange(objArray3);
            this.cmbPermission.Location = new Point(0xa3, 3);
            this.cmbPermission.Name = "cmbPermission";
            this.cmbPermission.Size = new Size(0xeb, 0x15);
            this.cmbPermission.TabIndex = 0x1c;
            this.label32.Anchor = AnchorStyles.Left;
            this.label32.AutoSize = true;
            this.label32.Location = new Point(3, 0x1d);
            this.label32.Name = "label32";
            this.label32.Size = new Size(0x36, 13);
            this.label32.TabIndex = 0x1d;
            this.label32.Text = "Web sites";
            this.label33.Anchor = AnchorStyles.Left;
            this.label33.AutoSize = true;
            this.label33.Location = new Point(3, 0x35);
            this.label33.Name = "label33";
            this.label33.Size = new Size(0x33, 13);
            this.label33.TabIndex = 30;
            this.label33.Text = "FTP sites";
            this.txtWebSites.Dock = DockStyle.Fill;
            this.txtWebSites.Location = new Point(0xa3, 0x1b);
            this.txtWebSites.Name = "txtWebSites";
            this.txtWebSites.Size = new Size(0xeb, 20);
            this.txtWebSites.TabIndex = 0x1f;
            this.txtFtpSites.Dock = DockStyle.Fill;
            this.txtFtpSites.Location = new Point(0xa3, 0x33);
            this.txtFtpSites.Name = "txtFtpSites";
            this.txtFtpSites.Size = new Size(0xeb, 20);
            this.txtFtpSites.TabIndex = 0x20;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x19f, 0x1ed);
            base.Controls.Add(this.tblMain);
            base.Name = "TxtGenerator";
            base.ShowIcon = false;
            this.Text = "Text File Generator";
            this.tblMain.ResumeLayout(false);
            this.flpButtons.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tblLayout1.ResumeLayout(false);
            this.tblLayout1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tblLayout2.ResumeLayout(false);
            this.tblLayout2.PerformLayout();
            this.numLevels.EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            base.ResumeLayout(false);
        }

        private static void PopulatePlayInfoCombo(ComboBox cmb)
        {
            cmb.Items.Add("Designed for");
            cmb.Items.Add("Player starts only");
            cmb.Items.Add("No");
        }

        public void SetData(IDataSourceAdapter adapter)
        {
            this.SetData(adapter, null);
        }

        public void SetData(IDataSourceAdapter adapter, IGameFileDataSource ds)
        {
            this.m_adapter = adapter;
            this.cmbEngine.ValueMember = "SourcePortID";
            this.cmbEngine.DisplayMember = "Name";
            List<ISourcePortDataSource> list = adapter.GetSourcePorts().ToList<ISourcePortDataSource>();
            SourcePortDataSource item = new SourcePortDataSource {
                SourcePortID = 0,
                Name = "N/A"
            };
            list.Insert(0, item);
            this.cmbEngine.DataSource = list;
            this.cmbGame.ValueMember = "IWadID";
            this.cmbGame.DisplayMember = "Name";
            this.cmbGame.DataSource = adapter.GetIWads();
            this.cmbGame.SelectedIndex = 0;
            if (ds != null)
            {
                if (ds.SourcePortID.HasValue)
                {
                    this.cmbEngine.SelectedValue = ds.SourcePortID.Value;
                }
                this.txtTile.Text = ds.Title;
                this.txtFilename.Text = ds.FileName;
                if (ds.ReleaseDate.HasValue)
                {
                    this.dtRelease.Value = ds.ReleaseDate.Value;
                }
                this.txtAuthor.Text = ds.Author;
                this.txtDescription.Text = ds.Description;
                if (ds.MapCount.HasValue)
                {
                    this.numLevels.Value = ds.MapCount.Value;
                }
                if (ds.Map != null)
                {
                    this.txtMaps.Text = GetMapString(ds.Map);
                }
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly TxtGenerator.<>c <>9 = new TxtGenerator.<>c();
            public static Func<char, bool> <>9__6_0;
            public static Func<Tuple<int, string>, int> <>9__6_1;
            public static Func<Tuple<int, string>, int> <>9__6_2;

            internal bool <GetMapString>b__6_0(char x) => 
                char.IsDigit(x);

            internal int <GetMapString>b__6_1(Tuple<int, string> x) => 
                x.Item1;

            internal int <GetMapString>b__6_2(Tuple<int, string> x) => 
                x.Item1;
        }
    }
}

