namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using DoomLauncher.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SettingsForm : Form
    {
        private Button btnCancel;
        private Button btnSave;
        private ComboBox cmbIwad;
        private ComboBox cmbSkill;
        private ComboBox cmbSourcePorts;
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label lblLaunchSettings;
        private List<Tuple<IConfigurationDataSource, object>> m_configValues = new List<Tuple<IConfigurationDataSource, object>>();
        private TextBox m_gameFileDirectory;
        private PictureBox pictureBox1;
        private TabControl tabControl;
        private TabPage tabPageConfig;
        private TabPage tabPageDefault;
        private TableLayoutPanel tblOuter;

        public SettingsForm()
        {
            this.InitializeComponent();
            this.lblLaunchSettings.Text = "These are the default settings for a game file" + Environment.NewLine + " that does not have a specific configuration saved.";
        }

        private static string AddSpaceBetweenWords(string item)
        {
            for (int i = 0; i < item.Length; i++)
            {
                if (char.IsUpper(item[i]) && (i != 0))
                {
                    item = item.Insert(i, " ");
                    i++;
                }
            }
            return item;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            LauncherPath path = new LauncherPath(this.m_gameFileDirectory.Text);
            FolderBrowserDialog dialog = new FolderBrowserDialog {
                SelectedPath = path.GetFullPath()
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.m_gameFileDirectory.Text = new LauncherPath(dialog.SelectedPath).GetPossiblyRelativePath();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (Tuple<IConfigurationDataSource, object> tuple in this.m_configValues)
            {
                tuple.Item1.Value = this.GetValue(tuple.Item1, tuple.Item2);
                this.DataSourceAdapter.UpdateConfiguration(tuple.Item1);
            }
            this.HandleLaunchSettings();
            base.Close();
        }

        private static IConfigurationDataSource CreateConfig(string configName, string configValue) => 
            new ConfigurationDataSource { 
                Name = configName,
                Value = configValue,
                UserCanModify = false,
                AvailableValues = string.Empty
            };

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetValue(IConfigurationDataSource ds, object value)
        {
            if (!string.IsNullOrEmpty(ds.AvailableValues))
            {
                ComboBox box = value as ComboBox;
                if ((box != null) && (box.SelectedItem != null))
                {
                    Tuple<string, string> selectedItem = box.SelectedItem as Tuple<string, string>;
                    if (selectedItem != null)
                    {
                        return selectedItem.Item2;
                    }
                }
            }
            else
            {
                TextBox box2 = value as TextBox;
                if (box2 != null)
                {
                    return box2.Text;
                }
            }
            return string.Empty;
        }

        private void HandleLaunchSettings()
        {
            string[] configNames = new string[] { ConfigType.DefaultSourcePort.ToString("g"), ConfigType.DefaultIWad.ToString("g"), ConfigType.DefaultSkill.ToString("g") };
            string[] strArray = new string[] { (this.cmbSourcePorts.SelectedItem == null) ? null : ((ISourcePortDataSource) this.cmbSourcePorts.SelectedItem).SourcePortID.ToString(), (this.cmbIwad.SelectedItem == null) ? null : ((IIWadDataSource) this.cmbIwad.SelectedItem).IWadID.ToString(), this.cmbSkill.SelectedItem?.ToString() };
            IEnumerable<IConfigurationDataSource> enumerable = from x in this.DataSourceAdapter.GetConfiguration()
                where configNames.Contains<string>(x.Name)
                select x;
            for (int i = 0; i < configNames.Length; i++)
            {
                string configName = configNames[i];
                string configValue = strArray[i];
                IConfigurationDataSource ds = (from x in enumerable
                    where x.Name == configName
                    select x).FirstOrDefault<IConfigurationDataSource>();
                if (configValue != null)
                {
                    if (ds == null)
                    {
                        ds = CreateConfig(configName, configValue);
                        this.DataSourceAdapter.InsertConfiguration(ds);
                    }
                    else
                    {
                        ds.Value = configValue;
                        this.DataSourceAdapter.UpdateConfiguration(ds);
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SettingsForm));
            this.tblOuter = new TableLayoutPanel();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.btnCancel = new Button();
            this.btnSave = new Button();
            this.tabControl = new TabControl();
            this.tabPageConfig = new TabPage();
            this.tabPageDefault = new TabPage();
            this.cmbSkill = new ComboBox();
            this.label4 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.cmbSourcePorts = new ComboBox();
            this.cmbIwad = new ComboBox();
            this.lblLaunchSettings = new Label();
            this.pictureBox1 = new PictureBox();
            this.tblOuter.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageDefault.SuspendLayout();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.tblOuter.ColumnCount = 1;
            this.tblOuter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblOuter.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tblOuter.Controls.Add(this.tabControl, 0, 0);
            this.tblOuter.Dock = DockStyle.Fill;
            this.tblOuter.Location = new Point(0, 0);
            this.tblOuter.Name = "tblOuter";
            this.tblOuter.RowCount = 2;
            this.tblOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblOuter.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblOuter.Size = new Size(0x199, 0xd3);
            this.tblOuter.TabIndex = 0;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new Point(0, 0xb3);
            this.flowLayoutPanel1.Margin = new Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(0x199, 0x20);
            this.flowLayoutPanel1.TabIndex = 0;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x14b, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnSave.DialogResult = DialogResult.OK;
            this.btnSave.Location = new Point(250, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x4b, 0x17);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.tabControl.Controls.Add(this.tabPageConfig);
            this.tabControl.Controls.Add(this.tabPageDefault);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x193, 0xad);
            this.tabControl.TabIndex = 1;
            this.tabPageConfig.BackColor = SystemColors.Control;
            this.tabPageConfig.Location = new Point(4, 0x16);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new Padding(3);
            this.tabPageConfig.Size = new Size(0x18b, 0x93);
            this.tabPageConfig.TabIndex = 0;
            this.tabPageConfig.Text = "Configuration";
            this.tabPageDefault.BackColor = SystemColors.Control;
            this.tabPageDefault.Controls.Add(this.pictureBox1);
            this.tabPageDefault.Controls.Add(this.lblLaunchSettings);
            this.tabPageDefault.Controls.Add(this.cmbSkill);
            this.tabPageDefault.Controls.Add(this.label4);
            this.tabPageDefault.Controls.Add(this.label2);
            this.tabPageDefault.Controls.Add(this.label1);
            this.tabPageDefault.Controls.Add(this.cmbSourcePorts);
            this.tabPageDefault.Controls.Add(this.cmbIwad);
            this.tabPageDefault.Location = new Point(4, 0x16);
            this.tabPageDefault.Margin = new Padding(0);
            this.tabPageDefault.Name = "tabPageDefault";
            this.tabPageDefault.Padding = new Padding(3);
            this.tabPageDefault.Size = new Size(0x18b, 0x93);
            this.tabPageDefault.TabIndex = 1;
            this.tabPageDefault.Text = "Launch Settings";
            this.cmbSkill.DisplayMember = "Name";
            this.cmbSkill.FormattingEnabled = true;
            this.cmbSkill.Location = new Point(0x30, 0x43);
            this.cmbSkill.Name = "cmbSkill";
            this.cmbSkill.Size = new Size(0xc5, 0x15);
            this.cmbSkill.TabIndex = 13;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(8, 70);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1a, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Skill";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(7, 0x2b);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "IWAD";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(7, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1a, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Port";
            this.cmbSourcePorts.DisplayMember = "Name";
            this.cmbSourcePorts.FormattingEnabled = true;
            this.cmbSourcePorts.Location = new Point(0x30, 13);
            this.cmbSourcePorts.Name = "cmbSourcePorts";
            this.cmbSourcePorts.Size = new Size(0xc5, 0x15);
            this.cmbSourcePorts.TabIndex = 9;
            this.cmbSourcePorts.ValueMember = "SourcePortID";
            this.cmbIwad.DisplayMember = "Name";
            this.cmbIwad.FormattingEnabled = true;
            this.cmbIwad.Location = new Point(0x30, 40);
            this.cmbIwad.Name = "cmbIwad";
            this.cmbIwad.Size = new Size(0xc5, 0x15);
            this.cmbIwad.TabIndex = 10;
            this.cmbIwad.ValueMember = "IWadID";
            this.lblLaunchSettings.AutoSize = true;
            this.lblLaunchSettings.Location = new Point(0x25, 0x65);
            this.lblLaunchSettings.Name = "lblLaunchSettings";
            this.lblLaunchSettings.Size = new Size(0x1c, 13);
            this.lblLaunchSettings.TabIndex = 15;
            this.lblLaunchSettings.Text = "Text";
            this.pictureBox1.Image = Resources.bon2b;
            this.pictureBox1.Location = new Point(10, 0x65);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x15, 0x12);
            this.pictureBox1.TabIndex = 0x10;
            this.pictureBox1.TabStop = false;
            base.AcceptButton = this.btnSave;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x199, 0xd3);
            base.Controls.Add(this.tblOuter);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "SettingsForm";
            base.ShowIcon = false;
            this.Text = "Settings";
            this.tblOuter.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageDefault.ResumeLayout(false);
            this.tabPageDefault.PerformLayout();
            ((ISupportInitialize) this.pictureBox1).EndInit();
            base.ResumeLayout(false);
        }

        private void PopulateConfiguration()
        {
            TableLayoutPanel panel = new TableLayoutPanel {
                Dock = DockStyle.Top
            };
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 8f));
            int num = 8;
            using (IEnumerator<IConfigurationDataSource> enumerator = (from item in this.DataSourceAdapter.GetConfiguration()
                where item.UserCanModify
                select item).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    IConfigurationDataSource ds = enumerator.Current;
                    GrowLabel control = new GrowLabel {
                        Anchor = AnchorStyles.Left,
                        Text = AddSpaceBetweenWords(ds.Name)
                    };
                    panel.RowStyles.Add(new RowStyle(SizeType.Absolute, (control.Height < 0x20) ? ((float) 0x20) : ((float) control.Height)));
                    panel.Controls.Add(control, 0, panel.RowStyles.Count - 1);
                    if (!string.IsNullOrEmpty(ds.AvailableValues))
                    {
                        ComboBox box = new ComboBox {
                            Dock = DockStyle.Fill
                        };
                        char[] separator = new char[] { ';' };
                        string[] strArray = ds.AvailableValues.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        List<Tuple<string, string>> list = new List<Tuple<string, string>>();
                        for (int i = 0; i < (strArray.Length - 1); i += 2)
                        {
                            list.Add(new Tuple<string, string>(strArray[i], strArray[i + 1]));
                        }
                        box.ValueMember = "Item1";
                        box.DataSource = list;
                        box.BindingContext = new BindingContext();
                        box.SelectedItem = (from item in list
                            where item.Item2 == ds.Value
                            select item).FirstOrDefault<Tuple<string, string>>();
                        panel.Controls.Add(box, 1, panel.RowStyles.Count - 1);
                        this.m_configValues.Add(new Tuple<IConfigurationDataSource, object>(ds, box));
                    }
                    else
                    {
                        TextBox box2 = new TextBox {
                            Dock = DockStyle.Fill,
                            Text = ds.Value
                        };
                        this.m_configValues.Add(new Tuple<IConfigurationDataSource, object>(ds, box2));
                        if (ds.Name == "GameFileDirectory")
                        {
                            this.m_gameFileDirectory = box2;
                            this.m_gameFileDirectory.Width = 170;
                            FlowLayoutPanel panel2 = new FlowLayoutPanel {
                                Dock = DockStyle.Fill,
                                Controls = { this.m_gameFileDirectory }
                            };
                            Button button = new Button {
                                Text = "Browse..."
                            };
                            button.Click += new EventHandler(this.browseButton_Click);
                            panel2.Controls.Add(button);
                            panel.Controls.Add(panel2, 1, panel.RowStyles.Count - 1);
                        }
                        else
                        {
                            panel.Controls.Add(box2, 1, panel.RowStyles.Count - 1);
                        }
                    }
                    num += 0x20;
                }
            }
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            panel.Height = num;
            this.tabControl.TabPages[0].Controls.Add(panel);
            base.Height = num + 110;
        }

        private void PopulateDefaultSettings(IDataSourceAdapter adapter)
        {
            this.cmbSourcePorts.DataSource = adapter.GetSourcePorts();
            this.cmbIwad.DataSource = adapter.GetIWads();
            this.cmbSkill.DataSource = DoomLauncher.Util.GetSkills();
            this.cmbSourcePorts.SelectedValue = this.AppConfiguration.GetTypedConfigValue(ConfigType.DefaultSourcePort, typeof(int));
            this.cmbIwad.SelectedValue = this.AppConfiguration.GetTypedConfigValue(ConfigType.DefaultIWad, typeof(int));
            this.cmbSkill.SelectedItem = this.AppConfiguration.GetTypedConfigValue(ConfigType.DefaultSkill, typeof(string));
        }

        public void SetCancelAllowed(bool set)
        {
            this.btnCancel.Visible = set;
            base.ControlBox = set;
        }

        public void SetData(IDataSourceAdapter adapter, DoomLauncher.AppConfiguration appConfig)
        {
            this.DataSourceAdapter = adapter;
            this.AppConfiguration = appConfig;
            this.PopulateDefaultSettings(adapter);
            this.PopulateConfiguration();
        }

        public DoomLauncher.AppConfiguration AppConfiguration { get; set; }

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SettingsForm.<>c <>9 = new SettingsForm.<>c();
            public static Func<IConfigurationDataSource, bool> <>9__5_0;

            internal bool <PopulateConfiguration>b__5_0(IConfigurationDataSource item) => 
                item.UserCanModify;
        }
    }
}

