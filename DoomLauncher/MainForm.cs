namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Forms;
    using DoomLauncher.Interfaces;
    using DoomLauncher.Properties;
    using Equin.ApplicationFramework;
    using Microsoft.CSharp.RuntimeBinder;
    using PresentationControls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Media;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class MainForm : Form
    {
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem addFilesToolStripMenuItem;
        private ToolStripMenuItem addIWADsToolStripMenuItem;
        private Button btnDownloads;
        private Button btnPlay;
        private Button btnSearch;
        private CheckBox chkAutoSearch;
        private IContainer components;
        private GameFileAssociationView ctrlAssociationView;
        private SearchControl ctrlSearch;
        private GameFileSummary ctrlSummary;
        private ToolStripMenuItem cumulativeStatisticsToolStripMenuItem;
        private ToolStripMenuItem cumulativeStatisticsToolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem downloadToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private FlowLayoutPanel flpSearch;
        private ToolStripMenuItem generateTextFileToolStripMenuItem;
        private ToolStripMenuItem generateTextFileToolStripMenuItem1;
        private bool m_cancelMetaUpdate;
        private IGameFileDataSource m_currentPlayFile;
        private PlayForm m_currentPlayForm;
        private static TextBoxForm m_debugForm;
        private DownloadHandler m_downloadHandler;
        private DownloadView m_downloadView;
        private DateTime m_dtStartPlay;
        private bool m_idGamesLoaded;
        private IGameFileDataSource m_lastSelectedItem;
        private string m_launchFile;
        private IGameFileDataSource[] m_pendingZdlFiles;
        private bool m_playInProgress;
        private ProgressBarForm m_progressBarSync;
        private ProgressBarForm m_progressBarUpdate;
        private List<INewFileDetector> m_saveFileDetectors;
        private IFileDataSource[] m_saveGames;
        private List<INewFileDetector> m_screenshotDetectors;
        private SplashScreen m_splash;
        private IStatisticsReader m_statsReader;
        private TabHandler m_tabHandler;
        private string m_typeSearch;
        private DateTime m_typeSearchLastPress;
        private VersionHandler m_versionHandler;
        private string m_workingDirectory;
        private List<InvalidFile> m_zdlInvalidFiles = new List<InvalidFile>();
        private ToolStripMenuItem manageTagsToolStripMenuItem;
        private ToolStripMenuItem manageTagsToolStripMenuItem1;
        private ContextMenuStrip mnuIdGames;
        private ContextMenuStrip mnuLocal;
        private ToolStripMenuItem newTagToolStripMenuItem;
        private ToolStripMenuItem openZipFileToolStripMenuItem;
        private ToolStripMenuItem playNowToolStripMenuItem;
        private ToolStripMenuItem playRandomToolStripMenuItem;
        private ToolStripMenuItem playToolStripMenuItem;
        private ToolStripMenuItem removeTagToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private static MainForm s_instance;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripMenuItem sourcePortsToolStripMenuItem;
        private SplitContainer splitLeftRight;
        private SplitContainer splitTopBottom;
        private DraggableTabControl tabControl;
        private ToolStripMenuItem tagToolStripMenuItem;
        private TableLayoutPanel tblDataView;
        private TableLayoutPanel tblMain;
        private TableLayoutPanel tblTop;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private ToolStripMenuItem updateMetadataToolStripMenuItem;
        private ToolStripMenuItem viewTextFileToolStripMenuItem;
        private ToolStripMenuItem viewWebPageToolStripMenuItem;

        public MainForm(string launchFile)
        {
            s_instance = this;
            this.m_launchFile = launchFile;
            this.m_splash = new SplashScreen();
            this.m_splash.StartPosition = FormStartPosition.CenterScreen;
            this.m_splash.Show();
            this.m_splash.Invalidate();
            this.InitializeComponent();
            this.m_workingDirectory = Directory.GetCurrentDirectory();
            bool flag = false;
            if (this.VerifyDatabase())
            {
                string str = Path.Combine(Directory.GetCurrentDirectory(), "DoomLauncher.sqlite");
                this.DataSourceAdapter = new DbDataSourceAdapter(new SqliteDatabaseAdapter(), $"Data Source={str}");
                this.AppConfiguration = new DoomLauncher.AppConfiguration(this.DataSourceAdapter);
                this.BackupDatabase(str);
                this.CreateSendToLink();
                this.KillRunningApps();
                if (this.VerifyGameFilesDirectory())
                {
                    this.HandleVersionUpdate();
                    flag = true;
                }
            }
            if (!flag)
            {
                base.Close();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleAddFiles();
        }

        private void addIWADsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleAddIWads();
        }

        private bool AssertCurrentViewItem(IGameFileDataSource item)
        {
            if ((item == null) || ((this.m_lastSelectedItem != null) && this.m_lastSelectedItem.Equals(item)))
            {
                return false;
            }
            this.m_lastSelectedItem = item;
            return true;
        }

        private bool AssertFile(string file)
        {
            FileInfo info1 = new FileInfo(file);
            if (!info1.Exists)
            {
                MessageBox.Show(this, $"The file {file} does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return info1.Exists;
        }

        private void BackupDatabase(string dataSource)
        {
            FileInfo fi = new FileInfo(dataSource);
            if (fi.Exists)
            {
                Directory.CreateDirectory("Backup");
                string backupFileName = this.GetBackupFileName(fi);
                if (!new FileInfo(backupFileName).Exists)
                {
                    fi.CopyTo(backupFileName);
                }
                this.CleanupBackupDirectory();
            }
        }

        private void btnDownloads_Click(object sender, EventArgs e)
        {
            this.DisplayDownloads();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.HandlePlay();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.HandleSearch();
        }

        private string BuildColumnConfig()
        {
            if (this.m_tabHandler != null)
            {
                List<ColumnConfig> list = new List<ColumnConfig>();
                foreach (ITabView view in this.m_tabHandler.TabViews)
                {
                    foreach (string str in view.GameFileViewControl.GetColumnKeyOrder())
                    {
                        ColumnConfig item = new ColumnConfig(view.Title, str, view.GameFileViewControl.GetColumnWidth(str));
                        list.Add(item);
                    }
                }
                try
                {
                    StringWriter writer = new StringWriter();
                    new XmlSerializer(typeof(ColumnConfig[])).Serialize((TextWriter) writer, list.ToArray());
                    return writer.ToString();
                }
                catch (Exception exception)
                {
                    this.DisplayException(exception);
                }
            }
            return string.Empty;
        }

        private string BuildTagText(IGameFileDataSource gameFile)
        {
            if (!gameFile.GameFileID.HasValue)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder("Tags: ");
            IEnumerable<ITagMappingDataSource> tagMappings = this.DataSourceAdapter.GetTagMappings(gameFile.GameFileID.Value);
            IEnumerable<ITagDataSource> enumerable2 = from tag in this.DataSourceAdapter.GetTags()
                join map in tagMappings on tag.TagID equals map.TagID
                select tag;
            if (enumerable2.Count<ITagDataSource>() > 0)
            {
                foreach (ITagDataSource source in enumerable2)
                {
                    builder.Append(source.Name);
                    builder.Append(", ");
                }
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }

        private void BuildTagToolStrip(ToolStripMenuItem tagToolStrip, IEnumerable<ITagDataSource> tags, EventHandler handler)
        {
            while (tagToolStrip.DropDownItems.Count > 2)
            {
                tagToolStrip.DropDownItems.RemoveAt(tagToolStrip.DropDownItems.Count - 1);
            }
            foreach (ITagDataSource source in tags)
            {
                tagToolStrip.DropDownItems.Add(source.Name, null, handler);
            }
        }

        private static bool CheckEdit(ITabView tabView, IGameFileDataSource[] gameFiles) => 
            (((gameFiles.Length != 0) && (tabView != null)) && tabView.IsEditAllowed);

        private void CleanTempDirectory()
        {
            DirectoryInfo info = new DirectoryInfo(this.AppConfiguration.TempDirectory.GetFullPath());
            if (info.Exists)
            {
                foreach (FileInfo info2 in info.GetFiles())
                {
                    try
                    {
                        info2.Delete();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void CleanupBackupDirectory()
        {
            List<FileInfo> filesInfo = new List<FileInfo>();
            Array.ForEach<string>(Directory.GetFiles("Backup", "*.sqlite"), x => filesInfo.Add(new FileInfo(x)));
            List<FileInfo> source = (from x in filesInfo
                orderby x.CreationTime
                select x).ToList<FileInfo>();
            while (source.Count<FileInfo>() > 10)
            {
                source.First<FileInfo>().Delete();
                source.RemoveAt(0);
            }
        }

        private void CopyFiles(string[] files, string directory, ProgressBarForm progressBar)
        {
            HashSet<string> set = new HashSet<string>();
            List<string> list = files.ToList<string>();
            list.Sort();
            int num = 0;
            foreach (string str in list)
            {
                this.UpdateProgressBar(progressBar, $"Copying {str}...", Convert.ToInt32((double) ((((double) num) / ((double) list.Count)) * 100.0)));
                FileInfo fi = new FileInfo(str);
                string item = fi.Name.Replace(fi.Extension, string.Empty);
                if (!IsZipFile(fi) && set.Contains(item))
                {
                    string path = Path.Combine(directory, item, ".zip");
                    if (File.Exists(path))
                    {
                        using (ZipArchive archive = ZipFile.Open(path, ZipArchiveMode.Update))
                        {
                            if (archive.Entries.Count<ZipArchiveEntry>(x => (x.Name == fi.Name)) == 0)
                            {
                                archive.CreateEntryFromFile(str, fi.Name);
                            }
                        }
                    }
                }
                if (!set.Contains(item))
                {
                    set.Add(item);
                }
                if (!IsZipFile(fi))
                {
                    string str4 = directory + item + ".zip";
                    if (File.Exists(str4))
                    {
                        goto Label_01A7;
                    }
                    using (ZipArchive archive2 = ZipFile.Open(str4, ZipArchiveMode.Create))
                    {
                        archive2.CreateEntryFromFile(str, fi.Name);
                        goto Label_01A7;
                    }
                }
                if (!File.Exists(Path.Combine(directory, fi.Name)))
                {
                    fi.CopyTo(Path.Combine(directory, fi.Name));
                }
            Label_01A7:
                num++;
            }
        }

        private void CopySaveGames(IGameFileDataSource gameFile, ISourcePortDataSource ds)
        {
            this.m_saveGames = this.DataSourceAdapter.GetFiles(gameFile, FileType.SaveGame).ToArray<IFileDataSource>();
            new SaveGameHandler(this.DataSourceAdapter, this.AppConfiguration.SaveGameDirectory).CopySaveGamesToSourcePort(ds, this.m_saveGames);
        }

        private void CopySaveGames(IGameFileDataSource gameFile, ISourcePortDataSource sourcePort, GameFilePlayAdaper playAdapter)
        {
            if (gameFile != null)
            {
                this.CopySaveGames(gameFile, sourcePort);
            }
            else if (this.IsGameFileIwad(gameFile))
            {
                gameFile = this.GetGameFileForIWad(playAdapter, gameFile);
                this.CopySaveGames(gameFile, sourcePort);
            }
        }

        [Conditional("DEBUG_OUTPUT")]
        private void CreateDebugForm()
        {
            m_debugForm = new TextBoxForm();
            m_debugForm.Text = "Debug";
            m_debugForm.HeaderText = "Debug output:";
            m_debugForm.Show();
        }

        private List<INewFileDetector> CreateDefaulSaveGameDetectors() => 
            new List<INewFileDetector>();

        private List<INewFileDetector> CreateDefaultScreenshotDetectors()
        {
            List<INewFileDetector> list = new List<INewFileDetector>();
            foreach (string str in this.AppConfiguration.ScreenshotCaptureDirectories)
            {
                if (new DirectoryInfo(str).Exists)
                {
                    list.Add(this.CreateScreenshotDetector(str));
                }
            }
            return list;
        }

        private void CreateFileDetectors(ISourcePortDataSource ds)
        {
            this.m_screenshotDetectors = this.CreateDefaultScreenshotDetectors();
            this.m_screenshotDetectors.Add(this.CreateScreenshotDetector(ds.Directory.GetFullPath()));
            Array.ForEach<INewFileDetector>(this.m_screenshotDetectors.ToArray(), x => x.StartDetection());
            this.m_saveFileDetectors = this.CreateDefaulSaveGameDetectors();
            this.m_saveFileDetectors.Add(this.CreateSaveGameDetector(ds.Directory.GetFullPath()));
            Array.ForEach<INewFileDetector>(this.m_saveFileDetectors.ToArray(), x => x.StartDetection());
        }

        private MetaDataForm CreateMetaForm()
        {
            MetaDataForm form1 = new MetaDataForm {
                StartPosition = FormStartPosition.CenterParent
            };
            form1.GameFileEdit.SetCheckBoxesChecked(true);
            form1.GameFileEdit.CommentsChecked = false;
            return form1;
        }

        private static GameFilePlayAdaper CreatePlayAdapter(PlayForm form, EventHandler processExited, DoomLauncher.AppConfiguration appConfig)
        {
            GameFilePlayAdaper adaper = new GameFilePlayAdaper {
                IWad = form.SelectedIWad,
                Map = form.SelectedMap,
                Skill = form.SelectedSkill,
                Record = form.Record,
                SpecificFiles = form.SpecificFiles,
                AdditionalFiles = form.GetAdditionalFiles().ToArray(),
                PlayDemo = form.PlayDemo,
                ExtraParameters = form.ExtraParameters
            };
            adaper.ProcessExited += processExited;
            if (form.SelectedDemo != null)
            {
                adaper.PlayDemoFile = Path.Combine(appConfig.DemoDirectory.GetFullPath(), form.SelectedDemo.FileName);
            }
            return adaper;
        }

        private ProgressBarForm CreateProgressBar(string text, ProgressBarStyle style)
        {
            ProgressBarForm form = new ProgressBarForm {
                StartPosition = FormStartPosition.CenterParent,
                DisplayText = text
            };
            if (style == ProgressBarStyle.Marquee)
            {
                form.ProgressBarStyle = ProgressBarStyle.Marquee;
                return form;
            }
            form.Minimum = 0;
            form.Maximum = 100;
            return form;
        }

        private INewFileDetector CreateSaveGameDetector(string dir) => 
            new NewFileDetector(new string[] { ".zds", ".dsg", ".esg" }, dir, true);

        private INewFileDetector CreateScreenshotDetector(string dir) => 
            new NewFileDetector(new string[] { ".png", ".jpg", ".bmp" }, dir, true);

        private void CreateSendToLink()
        {
            object obj2 = Activator.CreateInstance(System.Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
            try
            {
                string str = Environment.ExpandEnvironmentVariables(@"%AppData%\Microsoft\Windows\SendTo");
                if (<>o__27.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__27.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CreateShortcut", null, typeof(MainForm), argumentInfo));
                }
                object obj3 = <>o__27.<>p__0.Target(<>o__27.<>p__0, obj2, Path.Combine(str, "DoomLauncher.lnk"));
                try
                {
                    if (<>o__27.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] infoArray2 = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__27.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TargetPath", typeof(MainForm), infoArray2));
                    }
                    <>o__27.<>p__1.Target(<>o__27.<>p__1, obj3, Path.Combine(Directory.GetCurrentDirectory(), "DoomLauncher.exe"));
                    if (<>o__27.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] infoArray3 = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__27.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "IconLocation", typeof(MainForm), infoArray3));
                    }
                    <>o__27.<>p__2.Target(<>o__27.<>p__2, obj3, string.Format(Path.Combine(Directory.GetCurrentDirectory(), "DoomLauncher.ico"), new object[0]));
                    if (<>o__27.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] infoArray4 = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__27.<>p__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Save", null, typeof(MainForm), infoArray4));
                    }
                    <>o__27.<>p__3.Target(<>o__27.<>p__3, obj3);
                }
                finally
                {
                    if (<>o__27.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] infoArray5 = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__27.<>p__4 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FinalReleaseComObject", null, typeof(MainForm), infoArray5));
                    }
                    <>o__27.<>p__4.Target(<>o__27.<>p__4, typeof(Marshal), obj3);
                }
            }
            finally
            {
                if (<>o__27.<>p__5 == null)
                {
                    CSharpArgumentInfo[] infoArray6 = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__27.<>p__5 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FinalReleaseComObject", null, typeof(MainForm), infoArray6));
                }
                <>o__27.<>p__5.Target(<>o__27.<>p__5, typeof(Marshal), obj2);
            }
        }

        private IStatisticsReader CreateStatisticsReader(ISourcePortDataSource sourcePort, GameFilePlayAdaper playAdapter, IGameFileDataSource gameFile)
        {
            IStatisticsReader reader = null;
            if (BoomStatsReader.Supported(sourcePort))
            {
                return BoomStatsReader.CreateDefault(gameFile, sourcePort.Directory.GetFullPath());
            }
            if (CNDoomStatsReader.Supported(sourcePort))
            {
                return CNDoomStatsReader.CreateDefault(gameFile, sourcePort.Directory.GetFullPath());
            }
            if (!ZDoomStatsReader.Supported(sourcePort))
            {
                return reader;
            }
            List<IStatsDataSource> existingStats = new List<IStatsDataSource>();
            if ((gameFile != null) && gameFile.GameFileID.HasValue)
            {
                existingStats = this.DataSourceAdapter.GetStats(gameFile.GameFileID.Value).ToList<IStatsDataSource>();
            }
            return ZDoomStatsReader.CreateDefault(gameFile, sourcePort.Directory.GetFullPath(), existingStats);
        }

        private TagTabView CreateTagTab(Tuple<string, string>[] columnTextFields, ColumnConfig[] colConfig, string name, ITagDataSource tag)
        {
            ColumnConfig[] array = (from item in colConfig
                where item.Parent == "Local"
                select item).ToArray<ColumnConfig>();
            Array.ForEach<ColumnConfig>(array, delegate (ColumnConfig x) {
                x.Parent = tag.Name;
            });
            columnTextFields = SortColumns(tag.Name, columnTextFields, colConfig);
            TagTabView view = new TagTabView(tag.TagID, name, this.DataSourceAdapter, this.DefaultGameFileSelectFields, tag);
            view.GameFileViewControl.SetColumnFields(columnTextFields);
            view.GameFileViewControl.SetColumnFormat("ReleaseDate", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            view.GameFileViewControl.SetColumnFormat("Downloaded", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            view.GameFileViewControl.SetColumnFormat("LastPlayed", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            view.GameFileViewControl.SetContextMenuStrip(this.mnuLocal);
            view.GameFileViewControl.AllowDrop = true;
            this.SetGameFileViewEvents(view.GameFileViewControl, true);
            SetColumnWidths(name, view.GameFileViewControl, array);
            return view;
        }

        private List<ITabView> CreateTagTabs(Tuple<string, string>[] columnTextFields, ColumnConfig[] colConfig)
        {
            List<ITabView> list = new List<ITabView>();
            IEnumerable<ITagDataSource> enumerable = from x in this.DataSourceAdapter.GetTags()
                where x.HasTab
                orderby x.Name
                select x;
            this.Tags = enumerable.ToArray<ITagDataSource>();
            (from item in this.mnuLocal.Items.Cast<ToolStripItem>()
                where item.Text == "Tag"
                select item).FirstOrDefault<ToolStripItem>();
            foreach (ITagDataSource source in enumerable)
            {
                list.Add(this.CreateTagTab(columnTextFields, colConfig, source.Name, source));
            }
            return list;
        }

        private void ctrlAssociationView_FileDeleted(object sender, EventArgs e)
        {
            this.HandleSelectionChange(this.GetCurrentViewControl(), true);
        }

        private void ctrlAssociationView_FileOrderChanged(object sender, EventArgs e)
        {
            this.HandleSelectionChange(this.GetCurrentViewControl(), true);
        }

        private void CtrlAssociationView_RequestScreenshots(object sender, RequestScreenshotsEventArgs e)
        {
            List<IFileDataSource> screenshots = this.DataSourceAdapter.GetFiles(e.GameFile, FileType.Screenshot).ToList<IFileDataSource>();
            this.ctrlAssociationView.SetScreenshots(screenshots);
            if (screenshots.Count > 0)
            {
                this.SetPreviewImage(screenshots.First<IFileDataSource>());
            }
            else
            {
                this.ctrlSummary.ClearPreviewImage();
            }
        }

        private void ctrlSearch_SearchTextChanged(object sender, EventArgs e)
        {
            if ((this.chkAutoSearch.Checked && (this.GetCurrentTabView() != null)) && (this.GetCurrentTabView().GetType() != typeof(IdGamesTabViewCtrl)))
            {
                this.HandleSearch();
            }
        }

        private void ctrlView_DragDrop(object sender, DragEventArgs e)
        {
            GameFileViewControl control = sender as GameFileViewControl;
            string[] data = e.Data.GetData(DataFormats.FileDrop) as string[];
            if ((control != null) && (data != null))
            {
                if ((control.DoomLauncherParent != null) && (control.DoomLauncherParent is IWadTabViewCtrl))
                {
                    this.HandleAddGameFiles(AddFileType.IWad, data);
                }
                else
                {
                    this.HandleAddGameFiles(AddFileType.GameFile, data);
                }
            }
        }

        private void ctrlView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void ctrlView_GridKeyPress(object sender, KeyPressEventArgs e)
        {
            this.HandleKeyPress(e);
        }

        private void ctrlView_RowDoubleClicked(object sender, EventArgs e)
        {
            this.HandleRowDoubleClicked(sender as GameFileViewControl);
        }

        private void ctrlView_SelectionChange(object sender, EventArgs e)
        {
            this.HandleSelectionChange(sender, false);
        }

        private void ctrlView_ToolTipTextNeeded(object sender, AddingNewEventArgs e)
        {
            GameFileViewControl control1 = sender as GameFileViewControl;
            object obj2 = control1.ItemForRow(control1.ToolTipRowIndex);
            if (obj2 != null)
            {
                IGameFileDataSource item = ((ObjectView<GameFileDataSource>) obj2).Object;
                if (item != null)
                {
                    e.NewObject = new ToolTipHandler().GetToolTipText(this.Font, item);
                }
            }
        }

        private void cumulativeStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleCumulativeStatistics();
        }

        private void cumulativeStatisticsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.HandleCumulativeStatistics();
        }

        private static void DebugFormTrace(string text)
        {
            if (s_instance.InvokeRequired)
            {
                object[] args = new object[] { text };
                s_instance.Invoke(new Action<string>(MainForm.DebugFormTrace), args);
            }
            else if ((m_debugForm != null) && !m_debugForm.IsDisposed)
            {
                m_debugForm.AppendText(text + Environment.NewLine);
            }
        }

        [Conditional("DEBUG_OUTPUT")]
        public static void DebugTrace(string text)
        {
            DebugFormTrace(text);
        }

        private void DeleteGameFileAndAssociations(IGameFileDataSource gameFile)
        {
            this.DeleteLocalFileAssociations(gameFile);
            IIWadDataSource iWad = this.DataSourceAdapter.GetIWad(gameFile.GameFileID.Value);
            if (iWad != null)
            {
                this.DataSourceAdapter.DeleteIWad(iWad);
            }
            this.DirectoryDataSourceAdapter.DeleteGameFile(gameFile);
            this.DataSourceAdapter.DeleteGameFile(gameFile);
            if (gameFile.GameFileID.HasValue)
            {
                this.DataSourceAdapter.DeleteStatsByFile(gameFile.GameFileID.Value);
            }
            this.HandleSelectionChange(this.GetCurrentViewControl());
        }

        private void DeleteLibraryGameFiles(IEnumerable<string> files)
        {
            foreach (string str in files)
            {
                IGameFileDataSource gameFile = this.DataSourceAdapter.GetGameFile(str);
                if ((gameFile != null) && gameFile.GameFileID.HasValue)
                {
                    this.DeleteGameFileAndAssociations(gameFile);
                }
            }
        }

        private void DeleteLocalFileAssociations(IGameFileDataSource ds)
        {
            foreach (IFileDataSource source in this.DataSourceAdapter.GetFiles(ds))
            {
                FileInfo info = new FileInfo(Path.Combine(this.DirectoryForFileType(source.FileTypeID).GetFullPath(), source.FileName));
                if (info.Exists)
                {
                    info.Delete();
                }
                this.DataSourceAdapter.DeleteFile(source);
            }
        }

        private void DeleteLocalGameFiles(IEnumerable<string> files)
        {
            foreach (string str in files)
            {
                try
                {
                    File.Delete(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), str));
                }
                catch
                {
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleDelete();
        }

        private LauncherPath DirectoryForFileType(int fileTypeID)
        {
            switch (fileTypeID)
            {
                case 1:
                    return this.AppConfiguration.ScreenshotDirectory;

                case 2:
                    return this.AppConfiguration.DemoDirectory;

                case 3:
                    return this.AppConfiguration.SaveGameDirectory;
            }
            throw new NotImplementedException();
        }

        private void DisplayDownloads()
        {
            new Popup(this.m_downloadView) { 
                Width = 300,
                Height = this.m_downloadView.Height
            }.Show(this.btnDownloads);
        }

        private void DisplayException(Exception ex)
        {
            if (base.InvokeRequired)
            {
                object[] args = new object[] { ex };
                base.Invoke(new Action<Exception>(this.DisplayException), args);
            }
            else
            {
                TextBoxForm form = new TextBoxForm {
                    Text = "Unexpected Error",
                    HeaderText = "An unexpected error occurred. Please submit the following error report:"
                };
                string[] textArray1 = new string[] { ex.Message, Environment.NewLine, "------------------------------------------------------------", Environment.NewLine, ex.StackTrace };
                form.DisplayText = string.Concat(textArray1);
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
        }

        private void DisplayFilesNotFound(IEnumerable<string> files, List<IGameFileDataSource> gameFiles)
        {
            IEnumerable<string> source = files.Except<string>(from x in gameFiles select x.FileName);
            if (source.Count<string>() > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string str in source)
                {
                    builder.Append(str);
                    builder.Append(Environment.NewLine);
                }
                new TextBoxForm(true, MessageBoxButtons.OK) { 
                    Text = "Files Not Found",
                    HeaderText = "The following files were not found in the idgames database:",
                    DisplayText = builder.ToString()
                }.ShowDialog(this);
            }
        }

        private void DisplayInvalidFilesError(IEnumerable<InvalidFile> invalidFiles)
        {
            StringBuilder builder = new StringBuilder();
            foreach (InvalidFile file in invalidFiles)
            {
                builder.Append(file.FileName);
                builder.Append(":\t\t");
                builder.Append(file.Reason);
                builder.Append('\n');
            }
            this.ShowTextBoxForm("Processing Errors", "The information on these files may be incomplete.\n\nFor ZDL files adding the missing Source Port/IWAD name and re-adding will update the information.\n\nFor zip archive errors: Doom Launcher uses a zip library that has very strict zip rules that not all applications respect.\n\nVerify the zip by opening it with your favorite zip application. Create a new zip file and extract the files from the original zip into the newly created one. Then add the newly created zip to Doom Launcher.", builder.ToString(), true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dlItem_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                IGameFileDownloadable dlFile = sender as IGameFileDownloadable;
                if (e.Error != null)
                {
                    MessageBox.Show(this, e.Error.Message + "\n\nIf this error keeps occuring try chaning your mirror in the Settings menu.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else if (dlFile != null)
                {
                    this.WriteDownloadFile(dlFile);
                }
                try
                {
                    IDisposable disposable = sender as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    DoomLauncher.Util.DisplayUnexpectedException(this, exception);
                }
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleDownload(this.AppConfiguration.TempDirectory);
        }

        private void DownloadView_UserPlay(object sender, EventArgs e)
        {
            if (this.m_downloadView.SelectedItem != null)
            {
                IGameFileDataSource gameFile = this.DataSourceAdapter.GetGameFile(this.m_downloadView.SelectedItem.Key.ToString());
                if (gameFile != null)
                {
                    IGameFileDataSource[] gameFiles = new IGameFileDataSource[] { gameFile };
                    this.HandlePlay(gameFiles);
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleEdit();
        }

        private SyncLibraryHandler ExecuteSyncHandler() => 
            this.ExecuteSyncHandler(null);

        private SyncLibraryHandler ExecuteSyncHandler(string[] files)
        {
            SyncLibraryHandler handler = null;
            try
            {
                handler = new SyncLibraryHandler(this.DataSourceAdapter, this.DirectoryDataSourceAdapter, this.AppConfiguration.GameFileDirectory, this.AppConfiguration.TempDirectory);
                handler.SyncFileChange += new EventHandler(this.syncHandler_SyncFileChange);
                handler.GameFileDataNeeded += new EventHandler(this.syncHandler_GameFileDataNeeded);
                if (files == null)
                {
                    handler.Execute();
                }
                else
                {
                    handler.Execute(files);
                }
                if (this.m_pendingZdlFiles != null)
                {
                    this.SyncPendingZdlFiles();
                    this.m_pendingZdlFiles = null;
                }
            }
            catch (Exception exception)
            {
                this.DisplayException(exception);
            }
            return handler;
        }

        private void ExecuteVersionUpdate()
        {
            this.m_versionHandler.HandleVersionUpdate();
        }

        private void FillFileSize(IGameFileDataSource localFile)
        {
            FileInfo info = new FileInfo(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), localFile.FileName));
            if (info.Exists)
            {
                localFile.FileSizeBytes = Convert.ToInt32(info.Length);
            }
        }

        private void FillIwadData(IGameFileDataSource ds)
        {
            GameFileFieldType[] typeArray1;
            FileInfo info = new FileInfo(ds.FileName);
            if (!string.IsNullOrEmpty(ds.Title))
            {
                return;
            }
            string s = ds.FileName.Replace(info.Extension, string.Empty).ToUpper();
            switch (<PrivateImplementationDetails>.ComputeStringHash(s))
            {
                case 0x3c9b958f:
                    if (s == "DOOM1")
                    {
                        ds.Title = "Doom Shareware";
                        goto Label_01C2;
                    }
                    break;

                case 0x3d9b9722:
                    if (s == "DOOM2")
                    {
                        ds.Title = "Doom II: Hell on Earth";
                        goto Label_01C2;
                    }
                    break;

                case 0x2b4e3619:
                    if (s == "TNT")
                    {
                        ds.Title = "Final Doom: TNT: Evilution";
                        goto Label_01C2;
                    }
                    break;

                case 0x38aea989:
                    if (s == "HEXEN")
                    {
                        ds.Title = "Hexen: Beyond Heretic";
                        goto Label_01C2;
                    }
                    break;

                case 0x98083d5d:
                    if (s == "HERETIC")
                    {
                        ds.Title = "Shadow of the Serpent Riders";
                        goto Label_01C2;
                    }
                    break;

                case 0xa81e1366:
                    if (s == "STRIFE")
                    {
                        ds.Title = "Strife";
                        goto Label_01C2;
                    }
                    break;

                case 0xc41189a4:
                    if (s == "DOOM")
                    {
                        ds.Title = "The Ultimate Doom";
                        goto Label_01C2;
                    }
                    break;

                case 0xf59fe0a3:
                    if (s == "PLUTONIA")
                    {
                        ds.Title = "Final Doom: The Plutonia Experiment";
                        goto Label_01C2;
                    }
                    break;
            }
            ds.Title = ds.FileName.Replace(info.Extension, string.Empty);
        Label_01C2:
            typeArray1 = new GameFileFieldType[] { GameFileFieldType.Title };
            this.DataSourceAdapter.UpdateGameFile(ds, typeArray1);
        }

        private List<IGameFileDataSource> FindIdGamesFiles(IEnumerable<string> files)
        {
            List<IGameFileDataSource> list = new List<IGameFileDataSource>();
            foreach (string str in files)
            {
                IGameFileDataSource gameFile = this.IdGamesDataSourceAdapter.GetGameFile(str);
                if (gameFile != null)
                {
                    list.Add(gameFile);
                }
            }
            return list;
        }

        private void form_SourcePortLaunched(object sender, EventArgs e)
        {
            SourcePortViewForm form = sender as SourcePortViewForm;
            if ((form != null) && (form.SelectedSourcePort != null))
            {
                this.HandlePlay(null, form.SelectedSourcePort);
            }
        }

        private void generateTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGameFileDataSource file = this.SelectedItems(this.GetCurrentViewControl()).FirstOrDefault<IGameFileDataSource>();
            file = this.DataSourceAdapter.GetGameFile(file.FileName);
            if (file != null)
            {
                this.ShowTextFileGenerator(file);
            }
        }

        private void generateTextFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.ShowTextFileGenerator(null);
        }

        private List<ITabView> GetAdditionalTabViews()
        {
            List<ITabView> list1 = new List<ITabView>();
            list1.AddRange(from view in this.m_tabHandler.TabViews
                where view.Title == "Local"
                select view);
            list1.AddRange(from view in this.m_tabHandler.TabViews
                where view is TagTabView
                select view);
            return list1;
        }

        private string GetBackupFileName(FileInfo fi)
        {
            string newValue = DateTime.Now.ToString("yyyy_MM_dd") + fi.Extension;
            return Path.Combine(fi.DirectoryName, "Backup", fi.Name.Replace(fi.Extension, newValue));
        }

        private ColumnConfig[] GetColumnConfig()
        {
            try
            {
                StringReader textReader = new StringReader(this.AppConfiguration.ColumnConfig);
                ColumnConfig[] configArray = new XmlSerializer(typeof(ColumnConfig[])).Deserialize(textReader) as ColumnConfig[];
                if (configArray != null)
                {
                    return configArray;
                }
            }
            catch
            {
            }
            return new ColumnConfig[0];
        }

        private ITabView GetCurrentTabView() => 
            (this.tabControl.SelectedTab.Controls[0] as ITabView);

        private GameFileViewControl GetCurrentViewControl()
        {
            ITabView currentTabView = this.GetCurrentTabView();
            if (currentTabView != null)
            {
                return currentTabView.GameFileViewControl;
            }
            return null;
        }

        private string GetDialogFilter(string name, string[] extensions)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in extensions)
            {
                builder.Append($"*.{str.ToLower()};");
            }
            builder.Remove(builder.Length - 1, 1);
            return string.Format("{0} ({1})|{1}|All Files (*.*)|*.*", name, builder.ToString());
        }

        private IGameFileDataSource GetGameFileForIWad(GameFilePlayAdaper adapter, IGameFileDataSource gameFile) => 
            (from x in this.DataSourceAdapter.GetGameFileIWads()
                where x.GameFileID.Value == gameFile.GameFileID.Value
                select x).FirstOrDefault<IGameFileDataSource>();

        private IEnumerable<IGameFileDataSource> GetMetaFiles(IdGamesDataAdapater adapter, string metaFileName) => 
            adapter.GetGameFilesByName(metaFileName);

        private string[] GetNewSaveGames()
        {
            IEnumerable<string> first = new string[0];
            foreach (INewFileDetector detector in this.m_saveFileDetectors)
            {
                first = first.Union<string>(detector.GetNewFiles());
            }
            IEnumerable<string> enumerable2 = new string[0];
            foreach (INewFileDetector detector2 in this.m_saveFileDetectors)
            {
                enumerable2 = enumerable2.Union<string>(detector2.GetModifiedFiles());
            }
            IEnumerable<string> source = from x in this.m_saveGames select x.OriginalFileName;
            List<string> list = first.ToList<string>();
            foreach (string str in enumerable2)
            {
                FileInfo info = new FileInfo(str);
                if (!source.Contains<string>(info.Name) && !list.Contains(info.Name))
                {
                    list.Add(str);
                }
            }
            return list.ToArray();
        }

        private string[] GetNewScreenshots()
        {
            IEnumerable<string> first = new string[0];
            foreach (INewFileDetector detector in this.m_screenshotDetectors)
            {
                first = first.Union<string>(detector.GetNewFiles());
            }
            return first.ToArray<string>();
        }

        private IGameFileDataSource GetSecondaryFile(IGameFileDataSource gameFile, WadArchiveDataAdapter funzies)
        {
            ZipArchiveEntry source = (from x in ZipFile.OpenRead(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), gameFile.FileName)).Entries
                where x.Name.Contains(".wad")
                select x).FirstOrDefault<ZipArchiveEntry>();
            if (source != null)
            {
                try
                {
                    string path = Path.Combine("GameFiles", "Temp", source.Name);
                    if (!File.Exists(path))
                    {
                        source.ExtractToFile(path, true);
                    }
                    GameFileGetOptions options = new GameFileGetOptions(new GameFileSearchField(GameFileFieldType.MD5, path));
                    return funzies.GetGameFiles(options).FirstOrDefault<IGameFileDataSource>();
                }
                catch
                {
                }
            }
            return null;
        }

        private IEnumerable<string> GetZdlFiles(string[] files) => 
            (from file in files
                where new FileInfo(file).Extension == ".zdl"
                select file);

        private void HandleAddFiles()
        {
            string[] extensions = new string[] { "Zip", "WAD", "pk3", "txt", "zdl" };
            this.HandleAddFiles(AddFileType.GameFile, extensions, "Select Game Files");
        }

        private string[] HandleAddFiles(AddFileType type, string[] extensions, string dialogTitle)
        {
            OpenFileDialog dialog = new OpenFileDialog {
                Title = dialogTitle,
                Multiselect = true,
                Filter = this.GetDialogFilter("Game Files", extensions)
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.HandleAddGameFiles(type, dialog.FileNames);
                return dialog.FileNames;
            }
            return new string[0];
        }

        private void HandleAddGameFiles(AddFileType type, string[] files)
        {
            List<string> first = new List<string>(files);
            string[] second = this.GetZdlFiles(files).ToArray<string>();
            first = first.Except<string>(second).ToList<string>();
            string[] collection = this.HandleZdlFiles(second);
            first.AddRange(collection);
            if (((this.m_launchFile == null) && (collection.Length == 1)) && (first.Count<string>() == 1))
            {
                FileInfo info = new FileInfo(collection[0]);
                this.m_launchFile = info.Name.Replace(info.Extension, ".zip");
            }
            string[] array = (from item in first
                where !File.Exists(item)
                select item).ToArray<string>();
            if (array.Length != 0)
            {
                StringBuilder sb = new StringBuilder();
                Array.ForEach<string>(array, x => sb.Append(x + "\n"));
                MessageBox.Show(this, "The following files were not found and will not be added:" + Environment.NewLine + Environment.NewLine + sb.ToString(), "Files Not Found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                first = first.Except<string>(array).ToList<string>();
            }
            if (first.Count > 0)
            {
                this.HandleCopyFiles(type, first.ToArray());
            }
            else if (this.m_zdlInvalidFiles.Count > 0)
            {
                this.DisplayInvalidFilesError(this.m_zdlInvalidFiles);
            }
        }

        private void HandleAddIWads()
        {
            string[] extensions = new string[] { "WAD" };
            this.HandleAddFiles(AddFileType.IWad, extensions, "Select IWADs");
        }

        [AsyncStateMachine(typeof(<HandleCopyFiles>d__201))]
        private void HandleCopyFiles(AddFileType type, string[] fileNames)
        {
            <HandleCopyFiles>d__201 d__;
            d__.<>4__this = this;
            d__.type = type;
            d__.fileNames = fileNames;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<HandleCopyFiles>d__201>(ref d__);
        }

        private void HandleCumulativeStatistics()
        {
            if (this.GetCurrentViewControl() != null)
            {
                List<IStatsDataSource> stats = new List<IStatsDataSource>();
                foreach (ObjectView<GameFileDataSource> view2 in (BindingListView<GameFileDataSource>) this.GetCurrentViewControl().DataSource)
                {
                    if (view2.Object.GameFileID.HasValue)
                    {
                        stats.AddRange(this.DataSourceAdapter.GetStats(view2.Object.GameFileID.Value));
                    }
                }
                ITabView view = this.m_tabHandler.TabViewForControl(this.GetCurrentViewControl());
                string str = (view == null) ? string.Empty : view.Title;
                CumulativeStats stats1 = new CumulativeStats {
                    Text = $"Cumulative Stats - {str}"
                };
                stats1.SetStatistics(stats);
                stats1.StartPosition = FormStartPosition.CenterParent;
                stats1.ShowDialog(this);
            }
        }

        private void HandleDelete()
        {
            if (this.GetCurrentViewControl() != null)
            {
                MessageCheckBox box = null;
                ITabView view = this.m_tabHandler.TabViewForControl(this.GetCurrentViewControl());
                bool flag = true;
                bool flag2 = false;
                if (((view != null) && view.IsDeleteAllowed) && view.IsLocal)
                {
                    IGameFileDataSource[] sourceArray = this.SelectedItems(this.GetCurrentViewControl());
                    foreach (IGameFileDataSource source in sourceArray)
                    {
                        if (flag)
                        {
                            box = new MessageCheckBox("Confirm", $"Delete {source.FileName} and all associated data?", $"Do this for all {sourceArray.Length} items", SystemIcons.Question, MessageBoxButtons.OKCancel);
                            box.SetShowCheckBox(sourceArray.Length > 1);
                        }
                        if ((source != null) && (!flag || (box.ShowDialog(this) == DialogResult.OK)))
                        {
                            try
                            {
                                this.DeleteGameFileAndAssociations(source);
                                flag2 = true;
                            }
                            catch
                            {
                                MessageBox.Show(this, $"The file {source.FileName} appears to be in use and could not be deleted.", "In Use", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            }
                        }
                        if ((box != null) && box.Checked)
                        {
                            flag = false;
                        }
                        if (((box != null) && box.Checked) && (box.DialogResult == DialogResult.Cancel))
                        {
                            break;
                        }
                    }
                    if (flag2)
                    {
                        this.UpdateLocal();
                    }
                }
            }
        }

        private void HandleDetectorFiles(GameFilePlayAdaper adapter, IGameFileDataSource gameFile)
        {
            new ScreenshotHandler(this.DataSourceAdapter, this.AppConfiguration.ScreenshotDirectory).HandleNewScreenshots(adapter.SourcePort, gameFile, this.GetNewScreenshots());
            SaveGameHandler handler1 = new SaveGameHandler(this.DataSourceAdapter, this.AppConfiguration.SaveGameDirectory);
            handler1.HandleNewSaveGames(adapter.SourcePort, gameFile, this.GetNewSaveGames());
            handler1.HandleUpdateSaveGames(adapter.SourcePort, gameFile, this.m_saveGames);
        }

        private void HandleDownload(LauncherPath directory)
        {
            ITabView view = (from x in this.m_tabHandler.TabViews
                where x is IdGamesTabViewCtrl
                select x).FirstOrDefault<ITabView>();
            bool flag = false;
            if (view != null)
            {
                IGameFileDataSource[] dsItems = this.SelectedItems(view.GameFileViewControl);
                bool showAlreadyDownloading = true;
                bool doForAll = false;
                bool flag4 = true;
                foreach (IGameFileDataSource source in dsItems)
                {
                    IGameFileDownloadable dlItem = source as IGameFileDownloadable;
                    if ((source != null) && (dlItem != null))
                    {
                        GameFileGetOptions options = new GameFileGetOptions(new GameFileSearchField(GameFileFieldType.GameFileID, ((IdGamesGameFileDataSource) source).id.ToString()));
                        IGameFileDataSource dsItemFull = view.Adapter.GetGameFiles(options).FirstOrDefault<IGameFileDataSource>();
                        dlItem = dsItemFull as IGameFileDownloadable;
                        if (!doForAll)
                        {
                            flag4 = this.PromptUserDownload(dsItems, ref showAlreadyDownloading, ref doForAll, dlItem, dsItemFull, dsItems.Length > 1);
                        }
                        if ((dlItem > null) & flag4)
                        {
                            this.CurrentDownloadFile = dsItemFull;
                            dlItem.DownloadCompleted += new AsyncCompletedEventHandler(this.dlItem_DownloadCompleted);
                            this.m_downloadHandler.DownloadDirectory = directory;
                            this.m_downloadHandler.Download(view.Adapter, dlItem);
                            flag = true;
                        }
                    }
                }
            }
            if (flag)
            {
                this.DisplayDownloads();
            }
        }

        private void HandleEdit()
        {
            GameFileViewControl currentViewControl = this.GetCurrentViewControl();
            if (currentViewControl != null)
            {
                ITabView tabView = this.m_tabHandler.TabViewForControl(currentViewControl);
                IGameFileDataSource[] gameFiles = this.SelectedItems(currentViewControl);
                if (CheckEdit(tabView, gameFiles))
                {
                    IGameFileDataSource ds = gameFiles.First<IGameFileDataSource>();
                    IEnumerable<ITagMappingDataSource> tagMappings = this.DataSourceAdapter.GetTagMappings(ds.GameFileID.Value);
                    IEnumerable<ITagDataSource> tags = from tag in this.DataSourceAdapter.GetTags()
                        join map in tagMappings on tag.TagID equals map.TagID
                        select tag;
                    GameFileEditForm form = new GameFileEditForm {
                        StartPosition = FormStartPosition.CenterParent
                    };
                    form.EditControl.SetShowCheckBoxes(false);
                    form.EditControl.SetDataSource(ds, tags);
                    if (gameFiles.Length > 1)
                    {
                        form.Text = "*** Multiple Edit";
                        form.EditControl.SetShowCheckBoxes(true);
                        form.EditControl.SetCheckBoxesChecked(false);
                    }
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        foreach (IGameFileDataSource source2 in gameFiles)
                        {
                            form.EditControl.UpdateDataSource(source2);
                            tabView.Adapter.UpdateGameFile(source2, this.DefaultGameFileUpdateFields);
                            this.UpdateDataSourceViews(source2);
                        }
                        if (gameFiles.Count<IGameFileDataSource>() > 0)
                        {
                            this.HandleSelectionChange(currentViewControl);
                        }
                    }
                }
            }
        }

        private void HandleEditSourcePorts(bool initSetup)
        {
            SourcePortViewForm form = new SourcePortViewForm();
            form.Initialize(this.DataSourceAdapter, this.GetAdditionalTabViews().ToArray());
            form.StartPosition = FormStartPosition.CenterParent;
            form.SourcePortLaunched += new EventHandler(this.form_SourcePortLaunched);
            if (initSetup)
            {
                form.DisplayInitSetupButton(true);
            }
            form.ShowDialog(this);
        }

        private void HandleFormClosing()
        {
            if (this.DataSourceAdapter != null)
            {
                IEnumerable<IConfigurationDataSource> configuration = this.DataSourceAdapter.GetConfiguration();
                if (base.WindowState != FormWindowState.Minimized)
                {
                    this.UpdateConfig(configuration, "SplitTopBottom", this.splitTopBottom.SplitterDistance.ToString());
                    this.UpdateConfig(configuration, "SplitLeftRight", this.splitLeftRight.SplitterDistance.ToString());
                    this.UpdateConfig(configuration, "AppWidth", base.Size.Width.ToString());
                    this.UpdateConfig(configuration, "AppHeight", base.Size.Height.ToString());
                    this.UpdateConfig(configuration, "AppX", base.Location.X.ToString());
                    this.UpdateConfig(configuration, "AppY", base.Location.Y.ToString());
                    this.UpdateConfig(configuration, "WindowState", base.WindowState.ToString());
                }
                this.UpdateConfig(configuration, "ColumnConfig", this.BuildColumnConfig());
                this.UpdateConfig(configuration, ConfigType.AutoSearch.ToString("g"), this.chkAutoSearch.Checked.ToString());
            }
        }

        private void HandleGameFileDataNeeded(SyncLibraryHandler handler)
        {
            if ((this.CurrentDownloadFile != null) && (this.CurrentDownloadFile.FileName == handler.CurrentGameFile.FileName))
            {
                handler.CurrentGameFile.Title = this.CurrentDownloadFile.Title;
                handler.CurrentGameFile.Author = this.CurrentDownloadFile.Author;
                handler.CurrentGameFile.ReleaseDate = this.CurrentDownloadFile.ReleaseDate;
            }
        }

        private void HandleKeyPress(KeyPressEventArgs e)
        {
            if (this.GetCurrentViewControl() != null)
            {
                char.ToLower(e.KeyChar);
                this.SelectedItems(this.GetCurrentViewControl()).FirstOrDefault<IGameFileDataSource>();
                if ((this.m_typeSearch != null) && (DateTime.Now.Subtract(this.m_typeSearchLastPress).TotalMilliseconds > 700.0))
                {
                    this.m_typeSearch = null;
                }
                if (this.m_typeSearch == null)
                {
                    this.m_typeSearch = char.ToLower(e.KeyChar).ToString();
                }
                else
                {
                    this.m_typeSearch = this.m_typeSearch + char.ToLower(e.KeyChar).ToString();
                }
                this.m_typeSearchLastPress = DateTime.Now;
                if (!this.SelectItem(this.GetCurrentViewControl(), this.m_typeSearch))
                {
                    SystemSounds.Beep.Play();
                }
            }
        }

        private void HandleManageTags()
        {
            int num;
            TagForm form = new TagForm();
            form.Init(this.DataSourceAdapter);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog(this);
            this.RebuildTagToolStrip();
            this.TagMapLookup.Refresh();
            if (form.TagControl.AddedTags.Length != 0)
            {
                this.UpdateColumnConfig();
                this.AppConfiguration.RefreshColumnConfig();
            }
            ITagDataSource[] addedTags = form.TagControl.AddedTags;
            for (num = 0; num < addedTags.Length; num++)
            {
                ITagDataSource source = addedTags[num];
                if (source.HasTab)
                {
                    this.OrderedTagTabInsert(source);
                }
            }
            addedTags = form.TagControl.EditedTags;
            for (num = 0; num < addedTags.Length; num++)
            {
                ITagDataSource tag = addedTags[num];
                ITabView tab = (from item in this.m_tabHandler.TabViews
                    where item.Key.Equals(tag.TagID) && (item is TagTabView)
                    select item).FirstOrDefault<ITabView>();
                if (tab != null)
                {
                    if (tag.HasTab)
                    {
                        this.m_tabHandler.UpdateTabTitle(tab, tag.Name);
                    }
                    else
                    {
                        this.m_tabHandler.RemoveTab(tab);
                    }
                }
                else if (tag.HasTab)
                {
                    this.OrderedTagTabInsert(tag);
                }
            }
            addedTags = form.TagControl.DeletedTags;
            for (num = 0; num < addedTags.Length; num++)
            {
                ITagDataSource tag = addedTags[num];
                ITabView view2 = (from item in this.m_tabHandler.TabViews
                    where item.Key.Equals(tag.TagID) && (item is TagTabView)
                    select item).FirstOrDefault<ITabView>();
                if (view2 != null)
                {
                    this.m_tabHandler.RemoveTab(view2);
                }
            }
            this.HandleSelectionChange(this.GetCurrentViewControl(), false);
        }

        private bool HandleMetaError(IGameFileDataSource localFile)
        {
            MessageCheckBox box1 = new MessageCheckBox("Meta", $"Failed to find {localFile.FileName} from the id games mirror.

If you are sure this file should exist try chaning your mirror in the Settings menu.", "Don't show this error again", SystemIcons.Error) {
                StartPosition = FormStartPosition.CenterParent
            };
            box1.ShowDialog(this);
            return !box1.Checked;
        }

        private IGameFileDataSource HandleMultipleMetaFilesFound(IGameFileDataSource localFile, IEnumerable<IGameFileDataSource> remoteFiles)
        {
            if (remoteFiles.Count<IGameFileDataSource>() == 1)
            {
                return remoteFiles.First<IGameFileDataSource>();
            }
            this.FillFileSize(localFile);
            IEnumerable<IGameFileDataSource> source = from file in remoteFiles
                where file.FileSizeBytes == localFile.FileSizeBytes
                select file;
            if (source.Count<IGameFileDataSource>() == 1)
            {
                return source.First<IGameFileDataSource>();
            }
            FileSelectForm form = new FileSelectForm();
            form.Initialize(this.DataSourceAdapter, (from view in this.m_tabHandler.TabViews
                where view.Key.Equals("Id Games")
                select view).First<ITabView>(), remoteFiles);
            form.ShowSearchControl(false);
            string fileName = localFile.FileName;
            if (!string.IsNullOrEmpty(localFile.Title))
            {
                fileName = $"{localFile.Title}({localFile.FileName})";
            }
            form.SetDisplayText($"Multiple files found for {fileName}. Please select intended file.");
            form.MultiSelect = false;
            form.StartPosition = FormStartPosition.CenterParent;
            if (form.ShowDialog() != DialogResult.Cancel)
            {
                IGameFileDataSource[] selectedFiles = form.SelectedFiles;
                if (selectedFiles.Length != 0)
                {
                    return selectedFiles.First<IGameFileDataSource>();
                }
            }
            return null;
        }

        private void HandleMultiSelectPlay(IGameFileDataSource firstGameFile, IEnumerable<IGameFileDataSource> gameFiles)
        {
            StringBuilder builder = new StringBuilder();
            foreach (IGameFileDataSource source in gameFiles)
            {
                builder.Append(source.FileName);
                builder.Append(';');
            }
            firstGameFile.SettingsFiles = builder.ToString();
        }

        private void HandleOpenZipFile()
        {
            if (this.GetCurrentViewControl() != null)
            {
                foreach (IGameFileDataSource source in this.SelectedItems(this.GetCurrentViewControl()))
                {
                    if ((source != null) && this.AssertFile(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), source.FileName)))
                    {
                        Process.Start(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), source.FileName));
                    }
                }
            }
        }

        private void HandlePlay()
        {
            if (this.GetCurrentViewControl() != null)
            {
                this.HandlePlay(this.SelectedItems(this.GetCurrentViewControl()));
            }
        }

        private void HandlePlay(IEnumerable<IGameFileDataSource> gameFiles)
        {
            this.HandlePlay(gameFiles, null);
        }

        private void HandlePlay(IEnumerable<IGameFileDataSource> gameFiles, ISourcePortDataSource sourcePort)
        {
            IGameFileDataSource firstGameFile = null;
            if (gameFiles != null)
            {
                if (gameFiles.Count<IGameFileDataSource>() > 1)
                {
                    bool flag;
                    firstGameFile = this.PromptUserMainFile(gameFiles, out flag);
                    if (!flag)
                    {
                        return;
                    }
                }
                else
                {
                    firstGameFile = gameFiles.First<IGameFileDataSource>();
                }
            }
            if (this.m_playInProgress)
            {
                MessageBox.Show(this, "There is already a game in progress. Please exit that game first.", "Already Playing", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.DataSourceAdapter.GetSourcePorts().Count<ISourcePortDataSource>() == 0)
            {
                MessageBox.Show(this, "You must have at least one source port configured to play! Click the settings menu on the top left and select Source Ports to configure.", "No Source Ports", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.DataSourceAdapter.GetIWads().Count<IIWadDataSource>() == 0)
            {
                MessageBox.Show(this, "You must have at least one IWAD configured to play! Click the settings menu on the top left and select IWads to configure.", "No IWADs", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if ((firstGameFile != null) && (this.GetCurrentViewControl() != null))
                {
                    if (this.m_tabHandler.TabViewForControl(this.GetCurrentViewControl()) != null)
                    {
                        firstGameFile = this.DataSourceAdapter.GetGameFile(firstGameFile.FileName);
                    }
                    if (gameFiles.Count<IGameFileDataSource>() > 1)
                    {
                        IGameFileDataSource[] second = new IGameFileDataSource[] { firstGameFile };
                        this.HandleMultiSelectPlay(firstGameFile, gameFiles.Except<IGameFileDataSource>(second));
                        List<IGameFileDataSource> gameFilesList = new List<IGameFileDataSource> {
                            firstGameFile
                        };
                        Array.ForEach<IGameFileDataSource>(gameFiles.Skip<IGameFileDataSource>(1).ToArray<IGameFileDataSource>(), x => gameFilesList.Add(x));
                        gameFiles = gameFilesList;
                    }
                }
                this.SetupSourcePortForm(firstGameFile, this.AppConfiguration);
                if (sourcePort != null)
                {
                    this.m_currentPlayForm.SelectedSourcePort = sourcePort;
                }
                if (this.m_currentPlayForm.ShowDialog(this) == DialogResult.OK)
                {
                    this.HandlePlaySettings(this.m_currentPlayForm, firstGameFile);
                    if (this.m_currentPlayForm.SelectedSourcePort != null)
                    {
                        this.m_playInProgress = this.StartPlay(firstGameFile, this.m_currentPlayForm.SelectedSourcePort);
                    }
                }
            }
        }

        private void HandlePlaySettings(PlayForm form, IGameFileDataSource gameFile)
        {
            if (form.RememberSettings && (gameFile != null))
            {
                int? nullable = null;
                gameFile.IWadID = nullable;
                gameFile.SourcePortID = nullable;
                StringBuilder sbAdditionalFiles = new StringBuilder();
                StringBuilder sbSpecificFiles = new StringBuilder();
                if (form.SelectedSourcePort != null)
                {
                    gameFile.SourcePortID = new int?(form.SelectedSourcePort.SourcePortID);
                }
                if (form.SelectedIWad != null)
                {
                    gameFile.IWadID = form.SelectedIWad.IWadID;
                }
                if (form.SelectedMap != null)
                {
                    gameFile.SettingsMap = form.SelectedMap;
                }
                else
                {
                    gameFile.SettingsMap = string.Empty;
                }
                if (form.SelectedSkill != null)
                {
                    gameFile.SettingsSkill = form.SelectedSkill;
                }
                if (form.ExtraParameters != null)
                {
                    gameFile.SettingsExtraParams = form.ExtraParameters;
                }
                form.GetAdditionalFiles().ForEach(x => sbAdditionalFiles.Append(x.FileName + ";"));
                gameFile.SettingsFiles = sbAdditionalFiles.ToString();
                if (form.SpecificFiles != null)
                {
                    Array.ForEach<string>(form.SpecificFiles, x => sbSpecificFiles.Append(x + ";"));
                    gameFile.SettingsSpecificFiles = sbSpecificFiles.ToString();
                }
                else
                {
                    gameFile.SettingsSpecificFiles = string.Empty;
                }
                this.DataSourceAdapter.UpdateGameFile(gameFile, new GameFileFieldType[] { GameFileFieldType.SourcePortID });
            }
        }

        private void HandleProcessExited(object sender)
        {
            GameFilePlayAdaper adapter = sender as GameFilePlayAdaper;
            DateTime now = DateTime.Now;
            Directory.SetCurrentDirectory(this.m_workingDirectory);
            this.m_playInProgress = false;
            if (adapter.SourcePort != null)
            {
                IGameFileDataSource gameFile = adapter.GameFile;
                if (gameFile != null)
                {
                    this.SetMinutesPlayed(now, gameFile);
                }
                if (!string.IsNullOrEmpty(adapter.RecordedFileName))
                {
                    this.HandleRecordedDemo(adapter, gameFile);
                }
                this.HandleDetectorFiles(adapter, gameFile);
                if (this.m_statsReader != null)
                {
                    this.m_statsReader.Stop();
                    if (this.m_statsReader.ReadOnClose)
                    {
                        this.m_statsReader.ReadNow();
                    }
                    if (this.m_statsReader.Errors.Length != 0)
                    {
                        this.HandleStatReaderErrors(this.m_statsReader);
                    }
                    this.m_statsReader = null;
                }
            }
            if (this.GetCurrentViewControl() != null)
            {
                this.HandleSelectionChange(this.GetCurrentViewControl());
            }
        }

        private void handler_UpdateProgress(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Action(this.UpdateVersionProgress));
            }
            else
            {
                this.UpdateVersionProgress();
            }
        }

        private void HandleRecordedDemo(GameFilePlayAdaper adapter, IGameFileDataSource gameFile)
        {
            FileInfo fiTemp = new FileInfo(adapter.RecordedFileName);
            FileInfo info = (from file in new DirectoryInfo(this.AppConfiguration.TempDirectory.GetFullPath()).GetFiles()
                where file.Name.Contains(fiTemp.Name)
                select file).FirstOrDefault<FileInfo>();
            if ((info != null) && info.Exists)
            {
                new DemoHandler(this.DataSourceAdapter, this.AppConfiguration.DemoDirectory).HandleNewDemo(adapter.SourcePort, gameFile, info.FullName, this.m_currentPlayForm.RecordDescriptionText);
            }
            else
            {
                MessageBox.Show(this, "Could not find the demo file. Does this source port support recording?", "Demo Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void HandleRename()
        {
            IGameFileDataSource gameFile = this.SelectedItems(this.GetCurrentViewControl()).FirstOrDefault<IGameFileDataSource>();
            if (gameFile != null)
            {
                bool flag = false;
                TextBoxForm form = new TextBoxForm(false, MessageBoxButtons.OKCancel);
                form.SetMaxLength(0x30);
                form.DisplayText = gameFile.FileName;
                form.StartPosition = FormStartPosition.CenterParent;
                form.Text = $"Rename {gameFile.FileName}";
                int index = form.DisplayText.IndexOf('.');
                if (index != -1)
                {
                    form.SelectDisplayText(0, index);
                }
                while (!flag && (form.ShowDialog(this) == DialogResult.OK))
                {
                    flag = this.RenameGameFile(gameFile, form.DisplayText);
                    index = form.DisplayText.IndexOf('.');
                    if (index != -1)
                    {
                        form.SelectDisplayText(0, index);
                    }
                }
            }
        }

        private string HandleRenameFile(IGameFileDataSource gameFile, string fileName, string error)
        {
            FileInfo info = new FileInfo(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), gameFile.FileName));
            if (info.Exists)
            {
                if (!new FileInfo(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), fileName)).Exists)
                {
                    info.MoveTo(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), fileName));
                    IGameFileDataSource ds = this.DataSourceAdapter.GetGameFile(gameFile.FileName);
                    gameFile.FileName = fileName;
                    ds.FileName = fileName;
                    this.DataSourceAdapter.UpdateGameFile(ds);
                    this.HandleSelectionChange(this.GetCurrentViewControl());
                    return error;
                }
                error = $"The file {fileName} already exists.";
                return error;
            }
            error = $"Could not find {gameFile.FileName} to rename.";
            return error;
        }

        private void HandleRowDoubleClicked(GameFileViewControl ctrl)
        {
            if (ctrl != null)
            {
                ITabView view = this.m_tabHandler.TabViewForControl(ctrl);
                if ((view != null) && (view is IdGamesTabViewCtrl))
                {
                    this.HandleDownload(this.AppConfiguration.TempDirectory);
                }
                else if ((view != null) && view.IsPlayAllowed)
                {
                    this.HandlePlay();
                }
            }
        }

        private void HandleSearch()
        {
            if (this.GetCurrentViewControl() != null)
            {
                ITabView view = this.m_tabHandler.TabViewForControl(this.GetCurrentViewControl());
                if ((view != null) && view.IsSearchAllowed)
                {
                    if (string.IsNullOrEmpty(this.ctrlSearch.SearchText.Trim()))
                    {
                        view.SetGameFiles();
                    }
                    else
                    {
                        view.SetGameFiles(DoomLauncher.Util.SearchFieldsFromSearchCtrl(this.ctrlSearch));
                    }
                }
            }
        }

        private void HandleSelectionChange(object sender)
        {
            this.HandleSelectionChange(sender, true);
        }

        private void HandleSelectionChange(object sender, bool forceChange)
        {
            if (sender is GameFileViewControl)
            {
                IGameFileDataSource item = null;
                IGameFileDataSource[] source = this.SelectedItems(this.GetCurrentViewControl());
                if (source.Length != 0)
                {
                    item = source.First<IGameFileDataSource>();
                }
                if (forceChange || this.AssertCurrentViewItem(item))
                {
                    this.ctrlAssociationView.SetData(item);
                    if (item != null)
                    {
                        this.SetSummary(item);
                        IFileDataSource imgFile = null;
                        IEnumerable<IStatsDataSource> stats = new IStatsDataSource[0];
                        if (item.GameFileID.HasValue)
                        {
                            imgFile = this.DataSourceAdapter.GetFiles(item, FileType.Screenshot).FirstOrDefault<IFileDataSource>();
                            stats = this.DataSourceAdapter.GetStats(item.GameFileID.Value);
                        }
                        if ((imgFile != null) && !string.IsNullOrEmpty(imgFile.FileName))
                        {
                            this.SetPreviewImage(imgFile);
                        }
                        this.ctrlSummary.SetStatistics(stats);
                    }
                    else
                    {
                        this.btnPlay.Enabled = false;
                    }
                    this.ctrlSummary.Visible = true;
                    this.GetCurrentViewControl().Refresh();
                }
            }
        }

        private void HandleStatReaderErrors(IStatisticsReader m_statsReader)
        {
            TextBoxForm form = new TextBoxForm {
                StartPosition = FormStartPosition.CenterParent,
                Text = "Statistic Reader Errors",
                HeaderText = "The following errors were reported by the statistics reader." + Environment.NewLine + "The statistics may be incomplete or missing."
            };
            StringBuilder builder = new StringBuilder();
            foreach (string str in m_statsReader.Errors)
            {
                builder.Append(str);
                builder.Append(Environment.NewLine);
            }
            form.DisplayText = builder.ToString();
            form.ShowDialog(this);
        }

        [AsyncStateMachine(typeof(<HandleSyncStatus>d__54))]
        private void HandleSyncStatus()
        {
            <HandleSyncStatus>d__54 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<HandleSyncStatus>d__54>(ref d__);
        }

        [AsyncStateMachine(typeof(<HandleSyncStatusGameFilesOption>d__55))]
        private Task HandleSyncStatusGameFilesOption(SyncFileOption option, IEnumerable<string> files)
        {
            <HandleSyncStatusGameFilesOption>d__55 d__;
            d__.<>4__this = this;
            d__.option = option;
            d__.files = files;
            d__.<>t__builder = AsyncTaskMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<HandleSyncStatusGameFilesOption>d__55>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<HandleSyncStatusLibraryOptions>d__57))]
        private Task HandleSyncStatusLibraryOptions(SyncFileOption option, IEnumerable<string> files)
        {
            <HandleSyncStatusLibraryOptions>d__57 d__;
            d__.<>4__this = this;
            d__.option = option;
            d__.files = files;
            d__.<>t__builder = AsyncTaskMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<HandleSyncStatusLibraryOptions>d__57>(ref d__);
            return d__.<>t__builder.Task;
        }

        private void HandleTabSelectionChange()
        {
            if (this.tabControl.SelectedTab != null)
            {
                ITabView currentTabView = this.GetCurrentTabView();
                if (currentTabView != null)
                {
                    this.btnSearch.Enabled = currentTabView.IsSearchAllowed;
                    this.btnPlay.Enabled = currentTabView.IsPlayAllowed;
                    this.chkAutoSearch.Enabled = currentTabView.IsAutoSearchAllowed;
                    if ((currentTabView is IdGamesTabViewCtrl) && !this.m_idGamesLoaded)
                    {
                        currentTabView.SetGameFiles();
                        this.m_idGamesLoaded = true;
                    }
                    this.HandleSelectionChange(this.GetCurrentViewControl(), false);
                    currentTabView.GameFileViewControl.Focus();
                }
            }
        }

        private bool HandleUpdateMetaFields(IGameFileDataSource localFile, List<GameFileFieldType> fields)
        {
            this.DataSourceAdapter.UpdateGameFile(localFile, fields.ToArray());
            this.UpdateDataSourceViews(localFile);
            return true;
        }

        [AsyncStateMachine(typeof(<HandleVersionUpdate>d__20))]
        private void HandleVersionUpdate()
        {
            <HandleVersionUpdate>d__20 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<HandleVersionUpdate>d__20>(ref d__);
        }

        private void HandleViewTextFile()
        {
            if (this.GetCurrentViewControl() != null)
            {
                foreach (IGameFileDataSource source in this.SelectedItems(this.GetCurrentViewControl()))
                {
                    if ((source != null) && this.AssertFile(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), source.FileName)))
                    {
                        ZipArchiveEntry entry = (from zipItem in ZipFile.OpenRead(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), source.FileName)).Entries
                            where zipItem.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                            select zipItem).FirstOrDefault<ZipArchiveEntry>();
                        if (entry != null)
                        {
                            string destinationFileName = Path.Combine(this.AppConfiguration.TempDirectory.GetFullPath(), entry.Name);
                            entry.ExtractToFile(destinationFileName, true);
                            Process.Start(destinationFileName);
                        }
                        else
                        {
                            MessageBox.Show(this, "No text file found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                }
            }
        }

        private void HandleViewWebPage()
        {
            GameFileViewControl currentViewControl = this.GetCurrentViewControl();
            if (currentViewControl != null)
            {
                IGameFileDataSource source = this.SelectedItems(currentViewControl).FirstOrDefault<IGameFileDataSource>();
                if ((source != null) && (source is IdGamesGameFileDataSource))
                {
                    GameFileGetOptions options = new GameFileGetOptions(new GameFileSearchField(GameFileFieldType.GameFileID, ((IdGamesGameFileDataSource) source).id.ToString()));
                    IdGamesGameFileDataSource source2 = this.IdGamesDataSourceAdapter.GetGameFiles(options).FirstOrDefault<IGameFileDataSource>() as IdGamesGameFileDataSource;
                    if (source2 != null)
                    {
                        Process.Start($"{this.AppConfiguration.IdGamesUrl}?file={source2.dir}{source2.FileName}");
                    }
                }
            }
        }

        private string[] HandleZdlFiles(string[] files)
        {
            this.m_zdlInvalidFiles = new List<InvalidFile>();
            List<string> list = new List<string>();
            List<IGameFileDataSource> list2 = new List<IGameFileDataSource>();
            ZdlParser parser = new ZdlParser(this.DataSourceAdapter.GetSourcePorts(), this.DataSourceAdapter.GetIWads());
            foreach (string str in files)
            {
                IGameFileDataSource[] sourceArray = parser.Parse(str);
                foreach (IGameFileDataSource source in sourceArray)
                {
                    FileInfo info = new FileInfo(source.FileName);
                    list.Add(source.FileName);
                    source.FileName = info.Name.Replace(info.Extension, ".zip");
                    list2.Add(source);
                    if ((sourceArray.Count<IGameFileDataSource>() > 1) && (source == sourceArray.First<IGameFileDataSource>()))
                    {
                        StringBuilder sb = new StringBuilder();
                        Array.ForEach<string>((from item in sourceArray.Skip<IGameFileDataSource>(1) select new FileInfo(item.FileName).Name).ToArray<string>(), delegate (string x) {
                            sb.Append(x + ";");
                        });
                        sb.Remove(sb.Length - 1, 1);
                        source.SettingsFiles = sb.ToString();
                    }
                }
                if (parser.Errors.Length != 0)
                {
                    this.m_zdlInvalidFiles.Add(new InvalidFile(str, string.Join(", ", parser.Errors)));
                }
            }
            this.m_pendingZdlFiles = list2.ToArray();
            return list.ToArray();
        }

        private bool InitFileCheck(string initFile, string file, bool directory)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), initFile);
            string str2 = Path.Combine(Directory.GetCurrentDirectory(), file);
            if (directory)
            {
                DirectoryInfo info = new DirectoryInfo(str2);
                if (!new DirectoryInfo(path).Exists)
                {
                    if (!info.Exists)
                    {
                        return false;
                    }
                    info.MoveTo(path);
                }
                else if (info.Exists)
                {
                    info.Delete(true);
                }
            }
            else
            {
                FileInfo info2 = new FileInfo(str2);
                if (File.Exists(path))
                {
                    if (info2.Exists)
                    {
                        info2.Delete();
                    }
                }
                else if (info2.Exists)
                {
                    info2.MoveTo(path);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void Initialize()
        {
            try
            {
                if (ValidatePosition(this.AppConfiguration, this))
                {
                    base.StartPosition = FormStartPosition.Manual;
                    base.Location = new Point(this.AppConfiguration.AppX, this.AppConfiguration.AppY);
                    base.WindowState = this.AppConfiguration.WindowState;
                    base.Width = this.AppConfiguration.AppWidth;
                    base.Height = this.AppConfiguration.AppHeight;
                }
                this.splitTopBottom.SplitterDistance = this.AppConfiguration.SplitTopBottom;
                this.splitLeftRight.SplitterDistance = this.AppConfiguration.SplitLeftRight;
            }
            catch (DirectoryNotFoundException exception)
            {
                MessageBox.Show(this, $"The directory specified in your settings was incorrect: '{exception.Message}'", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.tblMain.Enabled = false;
                return;
            }
            if (this.AppConfiguration.CleanTemp)
            {
                this.CleanTempDirectory();
            }
            this.DirectoryDataSourceAdapter = new DoomLauncher.DirectoryDataSourceAdapter(this.AppConfiguration.GameFileDirectory, this.AppConfiguration.DateParseFormats);
            this.SetupTabs();
            this.RebuildTagToolStrip();
            this.m_downloadView = new DownloadView();
            this.m_downloadView.UserPlay += new EventHandler(this.DownloadView_UserPlay);
            this.m_downloadHandler = new DownloadHandler(this.AppConfiguration.TempDirectory, this.m_downloadView);
            this.ctrlAssociationView.Initialize(this.DataSourceAdapter, this.AppConfiguration.ScreenshotDirectory, this.AppConfiguration.DemoDirectory, this.AppConfiguration.SaveGameDirectory);
            this.ctrlAssociationView.FileDeleted += new EventHandler(this.ctrlAssociationView_FileDeleted);
            this.ctrlAssociationView.FileOrderChanged += new EventHandler(this.ctrlAssociationView_FileOrderChanged);
            this.ctrlAssociationView.RequestScreenshots += new EventHandler<RequestScreenshotsEventArgs>(this.CtrlAssociationView_RequestScreenshots);
            this.SetIWadGameFiles();
            if (this.DataSourceAdapter.GetSourcePorts().Count<ISourcePortDataSource>() == 0)
            {
                this.HandleEditSourcePorts(true);
            }
            if (this.DataSourceAdapter.GetIWads().Count<IIWadDataSource>() == 0)
            {
                this.m_splash.Close();
                this.HandleAddIWads();
                this.UpdateLocal();
            }
            else
            {
                this.UpdateLocal();
                this.m_splash.Close();
            }
            this.SetupSearchFilters();
            this.HandleTabSelectionChange();
            if (this.m_launchFile != null)
            {
                string launchFile = this.m_launchFile;
                this.m_launchFile = null;
                string[] files = new string[] { launchFile };
                this.HandleAddGameFiles(AddFileType.GameFile, files);
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MainForm));
            this.mnuLocal = new ContextMenuStrip(this.components);
            this.viewTextFileToolStripMenuItem = new ToolStripMenuItem();
            this.openZipFileToolStripMenuItem = new ToolStripMenuItem();
            this.playToolStripMenuItem = new ToolStripMenuItem();
            this.editToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.updateMetadataToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator8 = new ToolStripSeparator();
            this.tagToolStripMenuItem = new ToolStripMenuItem();
            this.newTagToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.removeTagToolStripMenuItem = new ToolStripMenuItem();
            this.manageTagsToolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.deleteToolStripMenuItem = new ToolStripMenuItem();
            this.renameToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator9 = new ToolStripSeparator();
            this.generateTextFileToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator11 = new ToolStripSeparator();
            this.cumulativeStatisticsToolStripMenuItem1 = new ToolStripMenuItem();
            this.tblMain = new TableLayoutPanel();
            this.tblTop = new TableLayoutPanel();
            this.flpSearch = new FlowLayoutPanel();
            this.toolStrip1 = new ToolStrip();
            this.toolStripDropDownButton1 = new ToolStripDropDownButton();
            this.addFilesToolStripMenuItem = new ToolStripMenuItem();
            this.addIWADsToolStripMenuItem = new ToolStripMenuItem();
            this.sourcePortsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.settingsToolStripMenuItem = new ToolStripMenuItem();
            this.manageTagsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.playNowToolStripMenuItem = new ToolStripMenuItem();
            this.playRandomToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.showToolStripMenuItem = new ToolStripMenuItem();
            this.generateTextFileToolStripMenuItem1 = new ToolStripMenuItem();
            this.cumulativeStatisticsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator10 = new ToolStripSeparator();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.ctrlSearch = new SearchControl();
            this.chkAutoSearch = new CheckBox();
            this.btnSearch = new Button();
            this.btnPlay = new Button();
            this.btnDownloads = new Button();
            this.tblDataView = new TableLayoutPanel();
            this.splitLeftRight = new SplitContainer();
            this.splitTopBottom = new SplitContainer();
            this.tabControl = new DraggableTabControl();
            this.ctrlAssociationView = new GameFileAssociationView();
            this.ctrlSummary = new GameFileSummary();
            this.toolTip1 = new ToolTip(this.components);
            this.mnuIdGames = new ContextMenuStrip(this.components);
            this.downloadToolStripMenuItem = new ToolStripMenuItem();
            this.viewWebPageToolStripMenuItem = new ToolStripMenuItem();
            this.toolTip2 = new ToolTip(this.components);
            this.mnuLocal.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.tblTop.SuspendLayout();
            this.flpSearch.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tblDataView.SuspendLayout();
            this.splitLeftRight.BeginInit();
            this.splitLeftRight.Panel1.SuspendLayout();
            this.splitLeftRight.Panel2.SuspendLayout();
            this.splitLeftRight.SuspendLayout();
            this.splitTopBottom.BeginInit();
            this.splitTopBottom.Panel1.SuspendLayout();
            this.splitTopBottom.Panel2.SuspendLayout();
            this.splitTopBottom.SuspendLayout();
            this.mnuIdGames.SuspendLayout();
            base.SuspendLayout();
            ToolStripItem[] toolStripItems = new ToolStripItem[] { this.viewTextFileToolStripMenuItem, this.openZipFileToolStripMenuItem, this.playToolStripMenuItem, this.editToolStripMenuItem, this.toolStripSeparator1, this.updateMetadataToolStripMenuItem, this.toolStripSeparator8, this.tagToolStripMenuItem, this.removeTagToolStripMenuItem, this.toolStripSeparator5, this.deleteToolStripMenuItem, this.renameToolStripMenuItem, this.toolStripSeparator9, this.generateTextFileToolStripMenuItem, this.toolStripSeparator11, this.cumulativeStatisticsToolStripMenuItem1 };
            this.mnuLocal.Items.AddRange(toolStripItems);
            this.mnuLocal.Name = "mnuGrid";
            this.mnuLocal.Size = new Size(0xc2, 0x114);
            this.viewTextFileToolStripMenuItem.Name = "viewTextFileToolStripMenuItem";
            this.viewTextFileToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.viewTextFileToolStripMenuItem.Text = "View Text File...";
            this.viewTextFileToolStripMenuItem.Click += new EventHandler(this.viewTextFileToolStripMenuItem_Click);
            this.openZipFileToolStripMenuItem.Name = "openZipFileToolStripMenuItem";
            this.openZipFileToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.openZipFileToolStripMenuItem.Text = "Open Zip File...";
            this.openZipFileToolStripMenuItem.Click += new EventHandler(this.openZipFileToolStripMenuItem_Click);
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.playToolStripMenuItem.Text = "Play...";
            this.playToolStripMenuItem.Click += new EventHandler(this.playToolStripMenuItem_Click);
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.editToolStripMenuItem.Text = "Edit...";
            this.editToolStripMenuItem.Click += new EventHandler(this.editToolStripMenuItem_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(190, 6);
            this.updateMetadataToolStripMenuItem.Name = "updateMetadataToolStripMenuItem";
            this.updateMetadataToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.updateMetadataToolStripMenuItem.Text = "Update metadata...";
            this.updateMetadataToolStripMenuItem.Click += new EventHandler(this.updateMetadataToolStripMenuItem_Click);
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new Size(190, 6);
            ToolStripItem[] itemArray2 = new ToolStripItem[] { this.newTagToolStripMenuItem, this.toolStripSeparator6 };
            this.tagToolStripMenuItem.DropDownItems.AddRange(itemArray2);
            this.tagToolStripMenuItem.Name = "tagToolStripMenuItem";
            this.tagToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.tagToolStripMenuItem.Text = "Tag";
            this.newTagToolStripMenuItem.Name = "newTagToolStripMenuItem";
            this.newTagToolStripMenuItem.Size = new Size(0x99, 0x16);
            this.newTagToolStripMenuItem.Text = "Manage Tags...";
            this.newTagToolStripMenuItem.Click += new EventHandler(this.newTagToolStripMenuItem_Click);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(150, 6);
            ToolStripItem[] itemArray3 = new ToolStripItem[] { this.manageTagsToolStripMenuItem1, this.toolStripSeparator7 };
            this.removeTagToolStripMenuItem.DropDownItems.AddRange(itemArray3);
            this.removeTagToolStripMenuItem.Name = "removeTagToolStripMenuItem";
            this.removeTagToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.removeTagToolStripMenuItem.Text = "Remove Tag";
            this.manageTagsToolStripMenuItem1.Name = "manageTagsToolStripMenuItem1";
            this.manageTagsToolStripMenuItem1.Size = new Size(0x99, 0x16);
            this.manageTagsToolStripMenuItem1.Text = "Manage Tags...";
            this.manageTagsToolStripMenuItem1.Click += new EventHandler(this.manageTagsToolStripMenuItem1_Click);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(150, 6);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(190, 6);
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new EventHandler(this.deleteToolStripMenuItem_Click);
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.renameToolStripMenuItem.Text = "Rename...";
            this.renameToolStripMenuItem.Click += new EventHandler(this.renameToolStripMenuItem_Click);
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new Size(190, 6);
            this.generateTextFileToolStripMenuItem.Name = "generateTextFileToolStripMenuItem";
            this.generateTextFileToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.generateTextFileToolStripMenuItem.Text = "Generate Text File...";
            this.generateTextFileToolStripMenuItem.Click += new EventHandler(this.generateTextFileToolStripMenuItem_Click);
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new Size(190, 6);
            this.cumulativeStatisticsToolStripMenuItem1.Name = "cumulativeStatisticsToolStripMenuItem1";
            this.cumulativeStatisticsToolStripMenuItem1.Size = new Size(0xc1, 0x16);
            this.cumulativeStatisticsToolStripMenuItem1.Text = "Cumulative Statistics...";
            this.cumulativeStatisticsToolStripMenuItem1.Click += new EventHandler(this.cumulativeStatisticsToolStripMenuItem1_Click);
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblMain.Controls.Add(this.tblTop, 0, 0);
            this.tblMain.Controls.Add(this.tblDataView, 0, 1);
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.Location = new Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tblMain.Size = new Size(0x3f0, 730);
            this.tblMain.TabIndex = 1;
            this.tblTop.ColumnCount = 2;
            this.tblTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tblTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tblTop.Controls.Add(this.flpSearch, 0, 0);
            this.tblTop.Controls.Add(this.btnDownloads, 1, 0);
            this.tblTop.Dock = DockStyle.Fill;
            this.tblTop.Location = new Point(0, 0);
            this.tblTop.Margin = new Padding(0);
            this.tblTop.Name = "tblTop";
            this.tblTop.RowCount = 1;
            this.tblTop.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            this.tblTop.Size = new Size(0x3f0, 0x20);
            this.tblTop.TabIndex = 1;
            this.flpSearch.AutoSize = true;
            this.flpSearch.Controls.Add(this.toolStrip1);
            this.flpSearch.Controls.Add(this.ctrlSearch);
            this.flpSearch.Controls.Add(this.chkAutoSearch);
            this.flpSearch.Controls.Add(this.btnSearch);
            this.flpSearch.Controls.Add(this.btnPlay);
            this.flpSearch.Dock = DockStyle.Fill;
            this.flpSearch.Location = new Point(0, 0);
            this.flpSearch.Margin = new Padding(0);
            this.flpSearch.Name = "flpSearch";
            this.flpSearch.Size = new Size(0x1f8, 0x20);
            this.flpSearch.TabIndex = 0;
            this.toolStrip1.Anchor = AnchorStyles.Left;
            this.toolStrip1.Dock = DockStyle.None;
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ToolStripItem[] itemArray4 = new ToolStripItem[] { this.toolStripDropDownButton1 };
            this.toolStrip1.Items.AddRange(itemArray4);
            this.toolStrip1.Location = new Point(4, 2);
            this.toolStrip1.Margin = new Padding(4, 0, 0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x41, 0x19);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "Options";
            this.toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripItem[] itemArray5 = new ToolStripItem[] { this.addFilesToolStripMenuItem, this.addIWADsToolStripMenuItem, this.sourcePortsToolStripMenuItem, this.toolStripSeparator2, this.settingsToolStripMenuItem, this.manageTagsToolStripMenuItem, this.toolStripSeparator3, this.playNowToolStripMenuItem, this.playRandomToolStripMenuItem, this.toolStripSeparator4, this.showToolStripMenuItem, this.generateTextFileToolStripMenuItem1, this.cumulativeStatisticsToolStripMenuItem, this.toolStripSeparator10, this.aboutToolStripMenuItem };
            this.toolStripDropDownButton1.DropDownItems.AddRange(itemArray5);
            this.toolStripDropDownButton1.Image = (Image) manager.GetObject("toolStripDropDownButton1.Image");
            this.toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripDropDownButton1.Margin = new Padding(2, 1, 0, 2);
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new Size(0x1d, 0x16);
            this.toolStripDropDownButton1.Text = "Options";
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.addFilesToolStripMenuItem.Text = "Add Files...";
            this.addFilesToolStripMenuItem.Click += new EventHandler(this.addFilesToolStripMenuItem_Click);
            this.addIWADsToolStripMenuItem.Name = "addIWADsToolStripMenuItem";
            this.addIWADsToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.addIWADsToolStripMenuItem.Text = "Add IWADs...";
            this.addIWADsToolStripMenuItem.Click += new EventHandler(this.addIWADsToolStripMenuItem_Click);
            this.sourcePortsToolStripMenuItem.Name = "sourcePortsToolStripMenuItem";
            this.sourcePortsToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.sourcePortsToolStripMenuItem.Text = "Source Ports...";
            this.sourcePortsToolStripMenuItem.Click += new EventHandler(this.sourcePortsToolStripMenuItem_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(190, 6);
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new EventHandler(this.settingsToolStripMenuItem_Click);
            this.manageTagsToolStripMenuItem.Name = "manageTagsToolStripMenuItem";
            this.manageTagsToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.manageTagsToolStripMenuItem.Text = "Manage Tags...";
            this.manageTagsToolStripMenuItem.Click += new EventHandler(this.manageTagsToolStripMenuItem_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(190, 6);
            this.playNowToolStripMenuItem.Name = "playNowToolStripMenuItem";
            this.playNowToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.playNowToolStripMenuItem.Text = "Play Now";
            this.playNowToolStripMenuItem.Click += new EventHandler(this.playNowToolStripMenuItem_Click);
            this.playRandomToolStripMenuItem.Name = "playRandomToolStripMenuItem";
            this.playRandomToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.playRandomToolStripMenuItem.Text = "Play Random!";
            this.playRandomToolStripMenuItem.Click += new EventHandler(this.playRandomToolStripMenuItem_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(190, 6);
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.showToolStripMenuItem.Text = "Sync Status...";
            this.showToolStripMenuItem.Click += new EventHandler(this.showToolStripMenuItem_Click);
            this.generateTextFileToolStripMenuItem1.Name = "generateTextFileToolStripMenuItem1";
            this.generateTextFileToolStripMenuItem1.Size = new Size(0xc1, 0x16);
            this.generateTextFileToolStripMenuItem1.Text = "Generate Text File...";
            this.generateTextFileToolStripMenuItem1.Click += new EventHandler(this.generateTextFileToolStripMenuItem1_Click);
            this.cumulativeStatisticsToolStripMenuItem.Name = "cumulativeStatisticsToolStripMenuItem";
            this.cumulativeStatisticsToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.cumulativeStatisticsToolStripMenuItem.Text = "Cumulative Statistics...";
            this.cumulativeStatisticsToolStripMenuItem.Click += new EventHandler(this.cumulativeStatisticsToolStripMenuItem_Click);
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new Size(190, 6);
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(0xc1, 0x16);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
            this.ctrlSearch.Anchor = AnchorStyles.Left;
            this.ctrlSearch.Location = new Point(0x48, 4);
            this.ctrlSearch.Margin = new Padding(3, 2, 3, 3);
            this.ctrlSearch.Name = "ctrlSearch";
            this.ctrlSearch.SearchText = "";
            this.ctrlSearch.Size = new Size(0x8e, 20);
            this.ctrlSearch.TabIndex = 0;
            this.chkAutoSearch.Anchor = AnchorStyles.Left;
            this.chkAutoSearch.AutoSize = true;
            this.chkAutoSearch.Location = new Point(220, 6);
            this.chkAutoSearch.Name = "chkAutoSearch";
            this.chkAutoSearch.Size = new Size(0x55, 0x11);
            this.chkAutoSearch.TabIndex = 7;
            this.chkAutoSearch.Text = "Auto Search";
            this.chkAutoSearch.UseVisualStyleBackColor = true;
            this.btnSearch.BackgroundImageLayout = ImageLayout.None;
            this.btnSearch.FlatAppearance.BorderColor = Color.Silver;
            this.btnSearch.Image = (Image) manager.GetObject("btnSearch.Image");
            this.btnSearch.ImageAlign = ContentAlignment.TopCenter;
            this.btnSearch.Location = new Point(0x137, 2);
            this.btnSearch.Margin = new Padding(3, 2, 3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(80, 0x18);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnPlay.BackgroundImageLayout = ImageLayout.None;
            this.btnPlay.Enabled = false;
            this.btnPlay.FlatAppearance.BorderColor = Color.Silver;
            this.btnPlay.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnPlay.Image = (Image) manager.GetObject("btnPlay.Image");
            this.btnPlay.Location = new Point(0x18d, 2);
            this.btnPlay.Margin = new Padding(3, 2, 3, 3);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new Size(0x4b, 0x18);
            this.btnPlay.TabIndex = 6;
            this.btnPlay.Text = "Play";
            this.btnPlay.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new EventHandler(this.btnPlay_Click);
            this.btnDownloads.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnDownloads.BackgroundImageLayout = ImageLayout.None;
            this.btnDownloads.FlatAppearance.BorderSize = 0;
            this.btnDownloads.FlatStyle = FlatStyle.Flat;
            this.btnDownloads.Image = Resources.th;
            this.btnDownloads.ImageAlign = ContentAlignment.TopCenter;
            this.btnDownloads.Location = new Point(0x38f, 2);
            this.btnDownloads.Margin = new Padding(0, 2, 1, 0);
            this.btnDownloads.Name = "btnDownloads";
            this.btnDownloads.Size = new Size(0x60, 0x16);
            this.btnDownloads.TabIndex = 1;
            this.btnDownloads.Text = "Downloads";
            this.btnDownloads.TextImageRelation = TextImageRelation.TextBeforeImage;
            this.btnDownloads.UseVisualStyleBackColor = true;
            this.btnDownloads.Click += new EventHandler(this.btnDownloads_Click);
            this.tblDataView.ColumnCount = 1;
            this.tblDataView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tblDataView.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tblDataView.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tblDataView.Controls.Add(this.splitLeftRight, 0, 0);
            this.tblDataView.Dock = DockStyle.Fill;
            this.tblDataView.Location = new Point(3, 0x20);
            this.tblDataView.Margin = new Padding(3, 0, 3, 0);
            this.tblDataView.Name = "tblDataView";
            this.tblDataView.RowCount = 1;
            this.tblDataView.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tblDataView.Size = new Size(0x3ea, 0x2ba);
            this.tblDataView.TabIndex = 4;
            this.splitLeftRight.Dock = DockStyle.Fill;
            this.splitLeftRight.Location = new Point(3, 6);
            this.splitLeftRight.Margin = new Padding(3, 6, 3, 3);
            this.splitLeftRight.Name = "splitLeftRight";
            this.splitLeftRight.Panel1.Controls.Add(this.splitTopBottom);
            this.splitLeftRight.Panel2.Controls.Add(this.ctrlSummary);
            this.splitLeftRight.Size = new Size(0x3e4, 0x2b1);
            this.splitLeftRight.SplitterDistance = 680;
            this.splitLeftRight.TabIndex = 9;
            this.splitTopBottom.Dock = DockStyle.Fill;
            this.splitTopBottom.Location = new Point(0, 0);
            this.splitTopBottom.Margin = new Padding(0);
            this.splitTopBottom.Name = "splitTopBottom";
            this.splitTopBottom.Orientation = Orientation.Horizontal;
            this.splitTopBottom.Panel1.Controls.Add(this.tabControl);
            this.splitTopBottom.Panel2.Controls.Add(this.ctrlAssociationView);
            this.splitTopBottom.Size = new Size(680, 0x2b1);
            this.splitTopBottom.SplitterDistance = 0x1db;
            this.splitTopBottom.TabIndex = 8;
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.HotTrack = true;
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(680, 0x1db);
            this.tabControl.TabIndex = 6;
            this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);
            this.ctrlAssociationView.DataSourceAdapter = null;
            this.ctrlAssociationView.Dock = DockStyle.Fill;
            this.ctrlAssociationView.Location = new Point(0, 0);
            this.ctrlAssociationView.Name = "ctrlAssociationView";
            this.ctrlAssociationView.SaveGameDirectory = null;
            this.ctrlAssociationView.ScreenshotDirectory = null;
            this.ctrlAssociationView.Size = new Size(680, 210);
            this.ctrlAssociationView.TabIndex = 7;
            this.ctrlSummary.Description = manager.GetString("ctrlSummary.Description");
            this.ctrlSummary.Dock = DockStyle.Fill;
            this.ctrlSummary.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.ctrlSummary.Location = new Point(0, 0);
            this.ctrlSummary.Margin = new Padding(3, 0, 3, 3);
            this.ctrlSummary.Name = "ctrlSummary";
            this.ctrlSummary.Padding = new Padding(0, 20, 0, 0);
            this.ctrlSummary.Size = new Size(0x138, 0x2b1);
            this.ctrlSummary.TabIndex = 4;
            this.ctrlSummary.TagText = "Tags:";
            this.ctrlSummary.Title = "";
            ToolStripItem[] itemArray6 = new ToolStripItem[] { this.downloadToolStripMenuItem, this.viewWebPageToolStripMenuItem };
            this.mnuIdGames.Items.AddRange(itemArray6);
            this.mnuIdGames.Name = "mnuIdGames";
            this.mnuIdGames.Size = new Size(0xa5, 0x30);
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new Size(0xa4, 0x16);
            this.downloadToolStripMenuItem.Text = "Download...";
            this.downloadToolStripMenuItem.Click += new EventHandler(this.downloadToolStripMenuItem_Click);
            this.viewWebPageToolStripMenuItem.Name = "viewWebPageToolStripMenuItem";
            this.viewWebPageToolStripMenuItem.Size = new Size(0xa4, 0x16);
            this.viewWebPageToolStripMenuItem.Text = "View Web Page...";
            this.viewWebPageToolStripMenuItem.Click += new EventHandler(this.viewWebPageToolStripMenuItem_Click);
            base.AcceptButton = this.btnSearch;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x3f0, 730);
            base.Controls.Add(this.tblMain);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MinimumSize = new Size(800, 600);
            base.Name = "MainForm";
            this.Text = "Doom Launcher";
            base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            this.mnuLocal.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.tblTop.ResumeLayout(false);
            this.tblTop.PerformLayout();
            this.flpSearch.ResumeLayout(false);
            this.flpSearch.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tblDataView.ResumeLayout(false);
            this.splitLeftRight.Panel1.ResumeLayout(false);
            this.splitLeftRight.Panel2.ResumeLayout(false);
            this.splitLeftRight.EndInit();
            this.splitLeftRight.ResumeLayout(false);
            this.splitTopBottom.Panel1.ResumeLayout(false);
            this.splitTopBottom.Panel2.ResumeLayout(false);
            this.splitTopBottom.EndInit();
            this.splitTopBottom.ResumeLayout(false);
            this.mnuIdGames.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private ProgressBarForm InitMetaProgressBar()
        {
            ProgressBarForm form1 = new ProgressBarForm {
                Text = "Fetching data...",
                Minimum = 0,
                Maximum = 0
            };
            form1.SetCancelAllowed(false);
            return form1;
        }

        private bool IsGameFileIwad(IGameFileDataSource gameFile) => 
            this.DataSourceAdapter.GetGameFileIWads().Any<IGameFileDataSource>(x => (x.GameFileID.Value == gameFile.GameFileID.Value));

        private static bool IsZipFile(FileInfo fi) => 
            fi.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase);

        private void KillRunningApps()
        {
            Func<Process, bool> <>9__0;
            Process currentProc = Process.GetCurrentProcess();
            using (IEnumerator<Process> enumerator = Process.GetProcessesByName("DoomLauncher").Where<Process>(((Func<Process, bool>) (<>9__0 ?? (<>9__0 = x => x.Id != currentProc.Id)))).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Kill();
                }
            }
        }

        private void m_currentPlayForm_SaveSettings(object sender, EventArgs e)
        {
            this.HandlePlaySettings(this.m_currentPlayForm, this.m_currentPlayForm.GameFile);
        }

        private void m_progressBarFormCopy_Cancelled(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void m_statsReader_NewStastics(object sender, NewStatisticsEventArgs e)
        {
            if (((e.Statistics != null) && (this.m_currentPlayFile != null)) && this.m_currentPlayFile.GameFileID.HasValue)
            {
                e.Statistics.MapName = e.Statistics.MapName.ToUpper();
                e.Statistics.GameFileID = this.m_currentPlayFile.GameFileID.Value;
                e.Statistics.SourcePortID = this.m_currentPlayForm.SelectedSourcePort.SourcePortID;
                if (e.Update)
                {
                    IStatsDataSource source = (from x in this.DataSourceAdapter.GetStats(e.Statistics.GameFileID)
                        where x.MapName == e.Statistics.MapName
                        select x).LastOrDefault<IStatsDataSource>();
                    if (source != null)
                    {
                        this.DataSourceAdapter.DeleteStats(source.StatID);
                    }
                }
                this.DataSourceAdapter.InsertStats(e.Statistics);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.HandleFormClosing();
        }

        private void manageTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleManageTags();
        }

        private void manageTagsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.HandleManageTags();
        }

        private void newTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleManageTags();
        }

        private void openZipFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleOpenZipFile();
        }

        private void OrderedTagTabInsert(ITagDataSource tag)
        {
            int index = this.m_tabHandler.TabViews.Length - 1;
            int num2 = 1;
            ITabView[] tabViews = this.m_tabHandler.TabViews;
            while (((index > num2) && (tabViews[index] is TagTabView)) && (tabViews[index].Title.CompareTo(tag.Name) > 0))
            {
                index--;
            }
            this.m_tabHandler.InsertTab(index + 1, this.CreateTagTab(this.DefaultColumnTextFields, this.GetColumnConfig(), tag.Name, tag));
        }

        private void playAdapter_ProcessExited(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                object[] args = new object[] { sender };
                base.Invoke(new Action<object>(this.HandleProcessExited), args);
            }
            else
            {
                this.HandleProcessExited(sender);
            }
        }

        private void playNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandlePlay(null, null);
        }

        private void playRandomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameFileFieldType[] selectFields = new GameFileFieldType[] { GameFileFieldType.GameFileID };
            IGameFileGetOptions options = new GameFileGetOptions(selectFields);
            IEnumerable<IGameFileDataSource> gameFiles = this.DataSourceAdapter.GetGameFiles(options);
            if (gameFiles.Count<IGameFileDataSource>() > 0)
            {
                int index = new Random(DateTime.Now.Millisecond).Next() % gameFiles.Count<IGameFileDataSource>();
                IGameFileDataSource source = gameFiles.ElementAt<IGameFileDataSource>(index);
                options = new GameFileGetOptions {
                    SearchField = new GameFileSearchField(GameFileFieldType.GameFileID, source.GameFileID.ToString())
                };
                source = this.DataSourceAdapter.GetGameFiles(options).First<IGameFileDataSource>();
                IGameFileDataSource[] sourceArray1 = new IGameFileDataSource[] { source };
                this.HandlePlay(sourceArray1);
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandlePlay();
        }

        private void ProgressBarEnd(ProgressBarForm form)
        {
            if (base.InvokeRequired)
            {
                object[] args = new object[] { form };
                base.Invoke(new Action<ProgressBarForm>(this.ProgressBarEnd), args);
            }
            else
            {
                base.Enabled = true;
                form.Close();
            }
        }

        private void ProgressBarForm_Cancelled(object sender, EventArgs e)
        {
            base.Enabled = true;
            base.BringToFront();
        }

        private void ProgressBarStart(ProgressBarForm form)
        {
            if (base.InvokeRequired)
            {
                object[] args = new object[] { form };
                base.Invoke(new Action<ProgressBarForm>(this.ProgressBarStart), args);
            }
            else
            {
                base.Enabled = false;
                form.Show(this);
            }
        }

        private void ProgressBarUpdate(SyncLibraryHandler handler)
        {
            if (this.m_progressBarSync != null)
            {
                this.m_progressBarSync.Maximum = handler.SyncFileCount;
                this.m_progressBarSync.Value = handler.SyncFileCurrent;
                this.m_progressBarSync.DisplayText = $"Reading {handler.CurrentSyncFileName}...";
            }
        }

        private bool PromptUserDownload(IGameFileDataSource[] dsItems, ref bool showAlreadyDownloading, ref bool doForAll, IGameFileDownloadable dlItem, IGameFileDataSource dsItemFull, bool showCheckBox)
        {
            if (showAlreadyDownloading && this.m_downloadHandler.IsDownloading(dlItem))
            {
                MessageCheckBox box = this.ShowAlreadyDownloading(dlItem, showCheckBox);
                showAlreadyDownloading = !box.Checked;
            }
            if (!doForAll)
            {
                IGameFileDataSource gameFile = this.DataSourceAdapter.GetGameFile(dsItemFull.FileName);
                if (gameFile != null)
                {
                    MessageCheckBox box2 = this.ShowAlreadyExists(dsItems, gameFile, showCheckBox);
                    doForAll = box2.Checked;
                    if (box2.DialogResult == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private IGameFileDataSource PromptUserMainFile(IEnumerable<IGameFileDataSource> gameFiles, out bool accepted)
        {
            accepted = false;
            FileSelectForm form = new FileSelectForm();
            ITabView tabView = (from tab in this.m_tabHandler.TabViews
                where tab.Key.Equals("Local")
                select tab).FirstOrDefault<ITabView>();
            form.Initialize(this.DataSourceAdapter, tabView, gameFiles);
            form.StartPosition = FormStartPosition.CenterParent;
            form.SetDisplayText("Please select the main file that all data will be associated with. (Screenshots, demos, save games, etc.)");
            form.MultiSelect = false;
            form.ShowSearchControl(false);
            if ((form.ShowDialog(this) == DialogResult.OK) && (form.SelectedFiles.Length != 0))
            {
                accepted = true;
                return form.SelectedFiles[0];
            }
            return gameFiles.First<IGameFileDataSource>();
        }

        private void RebuildTagToolStrip()
        {
            IEnumerable<ITagDataSource> source = from x in this.DataSourceAdapter.GetTags()
                orderby x.Name
                select x;
            this.Tags = source.ToArray<ITagDataSource>();
            ToolStripMenuItem tagToolStrip = (from item in this.mnuLocal.Items.Cast<ToolStripItem>()
                where item.Text == "Tag"
                select item).FirstOrDefault<ToolStripItem>() as ToolStripMenuItem;
            ToolStripMenuItem item2 = (from item in this.mnuLocal.Items.Cast<ToolStripItem>()
                where item.Text == "Remove Tag"
                select item).FirstOrDefault<ToolStripItem>() as ToolStripMenuItem;
            if (tagToolStrip != null)
            {
                this.BuildTagToolStrip(tagToolStrip, source, new EventHandler(this.tagToolStripItem_Click));
                this.BuildTagToolStrip(item2, source, new EventHandler(this.removeTagToolStripItem_Click));
            }
        }

        private void RefreshConfigItems()
        {
            this.IdGamesDataSourceAdapter = new IdGamesDataAdapater(this.AppConfiguration.IdGamesUrl, this.AppConfiguration.ApiPage, this.AppConfiguration.MirrorUrl);
            if (this.m_tabHandler != null)
            {
                (from x in this.m_tabHandler.TabViews
                    where x is IdGamesTabViewCtrl
                    select x).FirstOrDefault<ITabView>().Adapter = this.IdGamesDataSourceAdapter;
            }
            this.ctrlAssociationView.Initialize(this.DataSourceAdapter, this.AppConfiguration.ScreenshotDirectory, this.AppConfiguration.DemoDirectory, this.AppConfiguration.SaveGameDirectory);
        }

        private void removeTagToolStripItem_Click(object sender, EventArgs e)
        {
            ToolStripItem strip = sender as ToolStripItem;
            IGameFileDataSource[] sourceArray = this.SelectedItems(this.GetCurrentViewControl());
            if (strip != null)
            {
                ITagDataSource tag = (from item in this.Tags
                    where item.Name == strip.Text
                    select item).FirstOrDefault<ITagDataSource>();
                if (tag != null)
                {
                    TagMappingDataSource ds = new TagMappingDataSource();
                    foreach (IGameFileDataSource source3 in sourceArray)
                    {
                        ds.TagID = tag.TagID;
                        ds.FileID = source3.GameFileID.Value;
                        this.DataSourceAdapter.DeleteTagMapping(ds);
                    }
                    this.TagMapLookup.Refresh();
                    this.UpdateTagTabData(tag);
                    this.HandleTabSelectionChange();
                }
            }
        }

        private bool RenameGameFile(IGameFileDataSource gameFile, string fileName)
        {
            string error = null;
            bool flag = VerifyFileName(fileName);
            try
            {
                if (flag)
                {
                    if (!string.IsNullOrEmpty(fileName) && (fileName != gameFile.FileName))
                    {
                        error = this.HandleRenameFile(gameFile, fileName, error);
                    }
                    else
                    {
                        error = "The new file name must be different and not empty.";
                    }
                }
                else
                {
                    error = "The entered file name is invalid.";
                }
            }
            catch (Exception exception)
            {
                DoomLauncher.Util.DisplayUnexpectedException(this, exception);
            }
            if (error != null)
            {
                MessageBox.Show(this, error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            return true;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleRename();
        }

        private IGameFileDataSource[] SelectedItems(GameFileViewControl ctrl)
        {
            object[] selectedItems = ctrl.SelectedItems;
            List<IGameFileDataSource> list = new List<IGameFileDataSource>(selectedItems.Length);
            foreach (object obj2 in selectedItems)
            {
                list.Add(((ObjectView<GameFileDataSource>) obj2).Object);
            }
            return list.ToArray();
        }

        private bool SelectItem(GameFileViewControl ctrl, string search)
        {
            bool flag = false;
            bool flag2 = false;
            ITabView view = this.m_tabHandler.TabViewForControl(ctrl);
            if ((view != null) && (view is IdGamesTabViewCtrl))
            {
                flag2 = true;
            }
            foreach (ObjectView<GameFileDataSource> view2 in (BindingListView<GameFileDataSource>) this.GetCurrentViewControl().DataSource)
            {
                if (flag2)
                {
                    flag = view2.Object.Title.ToLower().StartsWith(search);
                }
                else
                {
                    flag = view2.Object.FileName.ToLower().StartsWith(search);
                }
                if (flag)
                {
                    this.SetSelectedItem(this.GetCurrentViewControl(), view2.Object);
                    return flag;
                }
            }
            return flag;
        }

        private static void SetColumnWidths(string tab, GameFileViewControl ctrl, ColumnConfig[] config)
        {
            foreach (ColumnConfig config2 in from item in config
                where item.Parent == tab
                select item)
            {
                ctrl.SetColumnWidth(config2.Column, config2.Width);
            }
        }

        private void SetDefaultSelections(DoomLauncher.AppConfiguration appConfig)
        {
            int port = (int) this.AppConfiguration.GetTypedConfigValue(ConfigType.DefaultSourcePort, typeof(int));
            int iwad = (int) this.AppConfiguration.GetTypedConfigValue(ConfigType.DefaultIWad, typeof(int));
            string typedConfigValue = (string) this.AppConfiguration.GetTypedConfigValue(ConfigType.DefaultSkill, typeof(string));
            ISourcePortDataSource source = (from x in this.DataSourceAdapter.GetSourcePorts()
                where x.SourcePortID == port
                select x).FirstOrDefault<ISourcePortDataSource>();
            if (source != null)
            {
                this.m_currentPlayForm.SelectedSourcePort = source;
            }
            IIWadDataSource source2 = (from x in this.DataSourceAdapter.GetIWads()
                where x.IWadID == Convert.ToInt32(iwad)
                select x).FirstOrDefault<IIWadDataSource>();
            if (source2 != null)
            {
                GameFileGetOptions options = new GameFileGetOptions(new GameFileSearchField(GameFileFieldType.GameFileID, source2.GameFileID.Value.ToString()));
                IEnumerable<IGameFileDataSource> gameFiles = this.DataSourceAdapter.GetGameFiles(options);
                if (gameFiles.Count<IGameFileDataSource>() > 0)
                {
                    this.m_currentPlayForm.SelectedIWad = gameFiles.First<IGameFileDataSource>();
                }
            }
            if (typedConfigValue != null)
            {
                this.m_currentPlayForm.SelectedSkill = typedConfigValue;
            }
        }

        private void SetGameFileViewEvents(GameFileViewControl ctrl, bool dragDrop)
        {
            ctrl.ToolTipTextNeeded += new AddingNewEventHandler(this.ctrlView_ToolTipTextNeeded);
            ctrl.RowDoubleClicked += new EventHandler(this.ctrlView_RowDoubleClicked);
            ctrl.SelectionChange += new EventHandler(this.ctrlView_SelectionChange);
            ctrl.GridKeyPress += new KeyPressEventHandler(this.ctrlView_GridKeyPress);
            if (dragDrop)
            {
                ctrl.DragDrop += new DragEventHandler(this.ctrlView_DragDrop);
                ctrl.DragEnter += new DragEventHandler(this.ctrlView_DragEnter);
            }
        }

        private void SetIWadGameFiles()
        {
            IEnumerable<IIWadDataSource> iWads = this.DataSourceAdapter.GetIWads();
            List<IGameFileDataSource> list = new List<IGameFileDataSource>();
            if (iWads.Count<IIWadDataSource>() > 0)
            {
                GameFileFieldType[] selectFields = new GameFileFieldType[2];
                selectFields[0] = GameFileFieldType.GameFileID;
                new GameFileGetOptions(selectFields);
                IEnumerable<IGameFileDataSource> gameFiles = this.DataSourceAdapter.GetGameFiles();
                using (IEnumerator<IIWadDataSource> enumerator = iWads.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        IIWadDataSource iwad = enumerator.Current;
                        IGameFileDataSource ds = (from x in gameFiles
                            where x.FileName.ToLower() == iwad.FileName.ToLower().Replace(".wad", ".zip")
                            select x).FirstOrDefault<IGameFileDataSource>();
                        if (ds != null)
                        {
                            if (!ds.IWadID.HasValue)
                            {
                                this.FillIwadData(ds);
                                list.Add(ds);
                                ds.IWadID = new int?(iwad.IWadID);
                                GameFileFieldType[] updateFields = new GameFileFieldType[] { GameFileFieldType.IWadID };
                                this.DataSourceAdapter.UpdateGameFile(ds, updateFields);
                            }
                            if (!iwad.GameFileID.HasValue)
                            {
                                iwad.GameFileID = ds.GameFileID;
                                this.DataSourceAdapter.UpdateIWad(iwad);
                            }
                        }
                    }
                }
            }
        }

        private void SetMinutesPlayed(DateTime dtExit, IGameFileDataSource gameFile)
        {
            gameFile.MinutesPlayed += Convert.ToInt32(dtExit.Subtract(this.m_dtStartPlay).TotalMinutes);
            GameFileFieldType[] updateFields = new GameFileFieldType[] { GameFileFieldType.MinutesPlayed };
            this.DataSourceAdapter.UpdateGameFile(gameFile, updateFields);
            this.UpdateDataSourceViews(gameFile);
        }

        private void SetPreviewImage(IFileDataSource imgFile)
        {
            try
            {
                FileStream stream = null;
                try
                {
                    if (imgFile.IsUrl)
                    {
                        this.ctrlSummary.SetPreviewImage(imgFile.FileName, true);
                    }
                    else
                    {
                        this.ctrlSummary.SetPreviewImage(Path.Combine(this.AppConfiguration.ScreenshotDirectory.GetFullPath(), imgFile.FileName), false);
                    }
                }
                catch
                {
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            catch
            {
                this.ctrlSummary.ClearPreviewImage();
            }
        }

        private void SetSelectedItem(GameFileViewControl ctrl, IGameFileDataSource ds)
        {
            foreach (ObjectView<GameFileDataSource> view in (BindingListView<GameFileDataSource>) ctrl.DataSource)
            {
                if (view.Object.Equals(ds))
                {
                    ctrl.SelectedItem = view;
                    break;
                }
            }
        }

        private void SetSummary(IGameFileDataSource item)
        {
            this.ctrlSummary.Visible = false;
            this.ctrlSummary.Title = item.Title;
            this.ctrlSummary.Description = item.Description;
            this.ctrlSummary.ClearPreviewImage();
            this.ctrlSummary.TagText = this.BuildTagText(item);
            this.ctrlSummary.SetTimePlayed(item.MinutesPlayed);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool flag = false;
            bool allowCancel = true;
            do
            {
                DialogResult result;
                flag = this.ShowSettings(allowCancel, out result);
                allowCancel = false;
            }
            while (!flag);
        }

        private void SetupSearchFilters()
        {
            this.chkAutoSearch.Checked = (bool) this.AppConfiguration.GetTypedConfigValue(ConfigType.AutoSearch, typeof(bool));
            this.ctrlSearch.SearchTextChanged += new EventHandler(this.ctrlSearch_SearchTextChanged);
            DoomLauncher.Util.SetDefaultSearchFields(this.ctrlSearch);
        }

        private void SetupSourcePortForm(IGameFileDataSource gameFile, DoomLauncher.AppConfiguration appConfig)
        {
            this.m_currentPlayForm = new PlayForm();
            this.m_currentPlayForm.SaveSettings += new EventHandler(this.m_currentPlayForm_SaveSettings);
            this.m_currentPlayForm.StartPosition = FormStartPosition.CenterParent;
            List<ITabView> additionalTabViews = this.GetAdditionalTabViews();
            this.m_currentPlayForm.Initialize(additionalTabViews, this.AppConfiguration.GameFileDirectory, this.DataSourceAdapter, (gameFile == null) ? null : this.DataSourceAdapter.GetGameFile(gameFile.FileName));
            this.SetDefaultSelections(appConfig);
            if (gameFile != null)
            {
                if (this.DataSourceAdapter.GetIWad(gameFile.GameFileID.Value) != null)
                {
                    this.m_currentPlayForm.SelectedIWad = gameFile;
                }
                if (gameFile.SourcePortID.HasValue)
                {
                    this.m_currentPlayForm.SelectedSourcePort = this.DataSourceAdapter.GetSourcePorts().Where<ISourcePortDataSource>(delegate (ISourcePortDataSource item) {
                        int? sourcePortID = gameFile.SourcePortID;
                        if (item.SourcePortID != sourcePortID.GetValueOrDefault())
                        {
                            return false;
                        }
                        return sourcePortID.HasValue;
                    }).FirstOrDefault<ISourcePortDataSource>();
                }
                if (gameFile.IWadID.HasValue)
                {
                    this.m_currentPlayForm.SelectedIWad = gameFile;
                    this.m_currentPlayForm.SelectedIWad = this.DataSourceAdapter.GetGameFileIWads().Where<IGameFileDataSource>(delegate (IGameFileDataSource item) {
                        int? iWadID = item.IWadID;
                        int? nullable2 = gameFile.IWadID;
                        if (iWadID.GetValueOrDefault() != nullable2.GetValueOrDefault())
                        {
                            return false;
                        }
                        return (iWadID.HasValue == nullable2.HasValue);
                    }).FirstOrDefault<IGameFileDataSource>();
                }
                if (!string.IsNullOrEmpty(gameFile.SettingsMap))
                {
                    this.m_currentPlayForm.SelectedMap = gameFile.SettingsMap;
                }
                if (!string.IsNullOrEmpty(gameFile.SettingsSkill))
                {
                    this.m_currentPlayForm.SelectedSkill = gameFile.SettingsSkill;
                }
                if (!string.IsNullOrEmpty(gameFile.SettingsExtraParams))
                {
                    this.m_currentPlayForm.ExtraParameters = gameFile.SettingsExtraParams;
                }
                if (!string.IsNullOrEmpty(gameFile.SettingsSpecificFiles))
                {
                    char[] separator = new char[] { ';' };
                    this.m_currentPlayForm.SpecificFiles = gameFile.SettingsSpecificFiles.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                }
                List<IGameFileDataSource> gameFiles = new List<IGameFileDataSource>();
                if (!string.IsNullOrEmpty(gameFile.SettingsFiles))
                {
                    gameFiles = (from x in DoomLauncher.Util.GetAdditionalFiles(this.DataSourceAdapter, gameFile)
                        where x > null
                        select x).ToList<IGameFileDataSource>();
                }
                this.m_currentPlayForm.SetAdditionalFiles(gameFiles);
                this.m_currentPlayForm.InitializeComplete();
            }
        }

        private void SetupStatsReader(ISourcePortDataSource sourcePort, GameFilePlayAdaper playAdapter, IGameFileDataSource gameFile)
        {
            this.m_statsReader = this.CreateStatisticsReader(sourcePort, playAdapter, gameFile);
            if (this.m_statsReader != null)
            {
                if (!string.IsNullOrEmpty(this.m_statsReader.LaunchParameter))
                {
                    playAdapter.ExtraParameters = playAdapter.ExtraParameters + this.m_statsReader.LaunchParameter;
                }
                this.m_statsReader.NewStastics += new NewStatisticsEventHandler(this.m_statsReader_NewStastics);
                this.m_statsReader.Start();
            }
        }

        private void SetupTabs()
        {
            List<ITabView> tabs = new List<ITabView>();
            ColumnConfig[] columnConfig = this.GetColumnConfig();
            this.TagMapLookup = new DoomLauncher.TagMapLookup(this.DataSourceAdapter);
            Tuple<string, string>[] defaultColumnTextFields = this.DefaultColumnTextFields;
            defaultColumnTextFields = SortColumns("Recent", defaultColumnTextFields, columnConfig);
            OptionsTabViewCtrl item = new OptionsTabViewCtrl("Recent", "Recent", this.DataSourceAdapter, this.DefaultGameFileSelectFields, this.TagMapLookup);
            item.GameFileViewControl.SetColumnFields(defaultColumnTextFields);
            item.GameFileViewControl.SetColumnFormat("ReleaseDate", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            item.GameFileViewControl.SetColumnFormat("Downloaded", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            item.GameFileViewControl.SetColumnFormat("LastPlayed", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            item.GameFileViewControl.SetContextMenuStrip(this.mnuLocal);
            item.GameFileViewControl.AllowDrop = true;
            item.Options = new GameFileGetOptions();
            item.Options.Limit = 10;
            item.Options.OrderBy = 1;
            item.Options.OrderField = 7;
            this.SetGameFileViewEvents(item.GameFileViewControl, true);
            SetColumnWidths("Recent", item.GameFileViewControl, columnConfig);
            tabs.Add(item);
            defaultColumnTextFields = SortColumns("Local", defaultColumnTextFields, columnConfig);
            LocalTabViewCtrl ctrl2 = new LocalTabViewCtrl("Local", "Local", this.DataSourceAdapter, this.DefaultGameFileSelectFields, this.TagMapLookup);
            ctrl2.GameFileViewControl.SetColumnFields(defaultColumnTextFields);
            ctrl2.GameFileViewControl.SetColumnFormat("ReleaseDate", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            ctrl2.GameFileViewControl.SetColumnFormat("Downloaded", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            ctrl2.GameFileViewControl.SetColumnFormat("LastPlayed", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            ctrl2.GameFileViewControl.SetContextMenuStrip(this.mnuLocal);
            ctrl2.GameFileViewControl.AllowDrop = true;
            this.SetGameFileViewEvents(ctrl2.GameFileViewControl, true);
            SetColumnWidths("Local", ctrl2.GameFileViewControl, columnConfig);
            tabs.Add(ctrl2);
            defaultColumnTextFields = new Tuple<string, string>[] { new Tuple<string, string>("FileName", "File"), new Tuple<string, string>("Title", "Title"), new Tuple<string, string>("LastPlayed", "Last Played") };
            defaultColumnTextFields = SortColumns("IWads", defaultColumnTextFields, columnConfig);
            IWadTabViewCtrl ctrl3 = new IWadTabViewCtrl("IWads", "IWads", this.DataSourceAdapter, this.DefaultGameFileSelectFields, this.TagMapLookup);
            ctrl3.GameFileViewControl.SetColumnFields(defaultColumnTextFields);
            ctrl3.GameFileViewControl.SetColumnFormat("LastPlayed", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            ctrl3.GameFileViewControl.SetContextMenuStrip(this.mnuLocal);
            ctrl3.GameFileViewControl.AllowDrop = true;
            this.SetGameFileViewEvents(ctrl3.GameFileViewControl, true);
            SetColumnWidths("IWads", ctrl3.GameFileViewControl, columnConfig);
            tabs.Add(ctrl3);
            defaultColumnTextFields = new Tuple<string, string>[] { new Tuple<string, string>("Title", "Title"), new Tuple<string, string>("Author", "Author"), new Tuple<string, string>("Description", "Description"), new Tuple<string, string>("Rating", "Rating") };
            defaultColumnTextFields = SortColumns("Id Games", defaultColumnTextFields, columnConfig);
            this.IdGamesDataSourceAdapter = new IdGamesDataAdapater(this.AppConfiguration.IdGamesUrl, this.AppConfiguration.ApiPage, this.AppConfiguration.MirrorUrl);
            IdGamesTabViewCtrl ctrl4 = new IdGamesTabViewCtrl("Id Games", "Id Games", this.IdGamesDataSourceAdapter, this.DefaultGameFileSelectFields);
            ctrl4.GameFileViewControl.SetColumnFields(defaultColumnTextFields);
            ctrl4.GameFileViewControl.SetContextMenuStrip(this.mnuIdGames);
            this.SetGameFileViewEvents(ctrl4.GameFileViewControl, false);
            SetColumnWidths("Id Games", ctrl4.GameFileViewControl, columnConfig);
            tabs.Add(ctrl4);
            tabs.AddRange(this.CreateTagTabs(this.DefaultColumnTextFields, columnConfig));
            this.m_tabHandler = new TabHandler(this.tabControl);
            this.m_tabHandler.SetTabs(tabs);
        }

        private MessageCheckBox ShowAlreadyDownloading(IGameFileDownloadable dlItem, bool showCheckBox)
        {
            MessageCheckBox box1 = new MessageCheckBox("Already Downloading", $"The file {dlItem.FileName} is already downloading", "Do not show this message again", SystemIcons.Error, MessageBoxButtons.OK);
            box1.SetShowCheckBox(showCheckBox);
            box1.ShowDialog(this);
            return box1;
        }

        private MessageCheckBox ShowAlreadyExists(IGameFileDataSource[] dsItems, IGameFileDataSource dsCheck, bool showCheckBox)
        {
            MessageCheckBox box1 = new MessageCheckBox("Already Exists", $"The file {dsCheck.FileName} already exists in the library. Continue Download?", $"Do this for all {dsItems.Length} items", SystemIcons.Warning, MessageBoxButtons.OKCancel);
            box1.SetShowCheckBox(showCheckBox);
            box1.ShowDialog(this);
            return box1;
        }

        private void ShowLaunchParameters(GameFilePlayAdaper playAdapter, IGameFileDataSource gameFile, ISourcePortDataSource sourcePort)
        {
            TextBoxForm form = new TextBoxForm {
                Text = "Launch Parameters",
                StartPosition = FormStartPosition.CenterParent
            };
            string str = playAdapter.GetLaunchParameters(this.AppConfiguration.GameFileDirectory, this.AppConfiguration.TempDirectory, gameFile, sourcePort, !this.IsGameFileIwad(gameFile));
            if (str != null)
            {
                string str2 = string.Empty;
                if ((this.m_currentPlayForm.SpecificFiles != null) && (this.m_currentPlayForm.SpecificFiles.Length != 0))
                {
                    str2 = Environment.NewLine + $"Selected Files: {string.Join(", ", this.m_currentPlayForm.SpecificFiles)}";
                }
                string[] textArray1 = new string[] { str, Environment.NewLine, Environment.NewLine, $"Supported Extensions: {sourcePort.SupportedExtensions}", str2, Environment.NewLine, Environment.NewLine, "*** If files appear to be missing check the 'Select Individual Files' option and supported extensions options in the Source Port form of the selected source port." };
                form.DisplayText = string.Concat(textArray1);
            }
            else
            {
                form.DisplayText = "Failed to generate launch parameters";
            }
            form.ShowDialog(this);
        }

        private bool ShowSettings(bool allowCancel, out DialogResult result)
        {
            SettingsForm form = new SettingsForm();
            form.SetData(this.DataSourceAdapter, this.AppConfiguration);
            form.SetCancelAllowed(allowCancel);
            form.StartPosition = FormStartPosition.CenterParent;
            result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                try
                {
                    this.AppConfiguration.Refresh();
                }
                catch (DirectoryNotFoundException exception)
                {
                    MessageBox.Show(this, $"The directory {exception.Message} was not found. DoomLauncher will not operate correctly with invalid paths. Make sure the directory you are setting contains all folders required (GameWads, Screenshots, SaveGames, Demos, Temp)", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                catch (Exception exception2)
                {
                    DoomLauncher.Util.DisplayUnexpectedException(this, exception2);
                    return false;
                }
                this.RefreshConfigItems();
                this.HandleSelectionChange(this.GetCurrentViewControl(), true);
            }
            return true;
        }

        private SyncStatusForm ShowSyncStatusForm(string title, string header, IEnumerable<string> files, IEnumerable<string> dropDownOptions)
        {
            SyncStatusForm form1 = new SyncStatusForm {
                Text = title,
                HeaderText = header
            };
            form1.SetData(files, dropDownOptions);
            form1.StartPosition = FormStartPosition.CenterParent;
            form1.ShowDialog(this);
            return form1;
        }

        private void ShowTextBoxForm(string title, string header, string text, bool dialog)
        {
            TextBoxForm form = new TextBoxForm {
                StartPosition = FormStartPosition.CenterParent,
                Text = title,
                HeaderText = header,
                DisplayText = text
            };
            if (dialog)
            {
                form.Show(this);
            }
            else
            {
                form.ShowDialog(this);
            }
        }

        private void ShowTextFileGenerator(IGameFileDataSource file)
        {
            TxtGenerator generator1 = new TxtGenerator();
            generator1.SetData(this.DataSourceAdapter, file);
            generator1.StartPosition = FormStartPosition.CenterParent;
            generator1.ShowDialog(this);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleSyncStatus();
        }

        private static Tuple<string, string>[] SortColumns(string tab, Tuple<string, string>[] items, ColumnConfig[] config)
        {
            List<Tuple<string, string>> first = new List<Tuple<string, string>>();
            bool flag = (config.Length != 0) && ((from item in config
                where item.Column == "MapCount"
                select item).Count<ColumnConfig>() == 0);
            ColumnConfig[] configArray = config;
            for (int i = 0; i < configArray.Length; i++)
            {
                ColumnConfig configItem = configArray[i];
                Tuple<string, string> tuple = (from x in items
                    where (configItem.Parent == tab) && x.Item1.Equals(configItem.Column, StringComparison.InvariantCultureIgnoreCase)
                    select x).FirstOrDefault<Tuple<string, string>>();
                if (tuple != null)
                {
                    first.Add(tuple);
                }
            }
            if (flag && (first.Count > 4))
            {
                Tuple<string, string> tuple2 = (from x in items
                    where x.Item2.Equals("maps", StringComparison.InvariantCultureIgnoreCase)
                    select x).FirstOrDefault<Tuple<string, string>>();
                if (tuple2 != null)
                {
                    first.Remove(tuple2);
                    first.Insert(4, tuple2);
                }
            }
            return first.Union<Tuple<string, string>>(items).ToArray<Tuple<string, string>>();
        }

        private void sourcePortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleEditSourcePorts(false);
        }

        private bool StartPlay(IGameFileDataSource gameFile, ISourcePortDataSource sourcePort)
        {
            GameFilePlayAdaper playAdapter = CreatePlayAdapter(this.m_currentPlayForm, new EventHandler(this.playAdapter_ProcessExited), this.AppConfiguration);
            this.m_saveGames = new IFileDataSource[0];
            this.CopySaveGames(gameFile, sourcePort, playAdapter);
            this.CreateFileDetectors(sourcePort);
            if (this.m_currentPlayForm.PreviewLaunchParameters)
            {
                this.ShowLaunchParameters(playAdapter, gameFile, sourcePort);
            }
            bool isGameFileIwad = this.IsGameFileIwad(gameFile);
            if (this.m_currentPlayForm.SaveStatistics)
            {
                this.SetupStatsReader(sourcePort, playAdapter, gameFile);
            }
            if (playAdapter.Launch(this.AppConfiguration.GameFileDirectory, this.AppConfiguration.TempDirectory, gameFile, sourcePort, isGameFileIwad))
            {
                this.m_currentPlayFile = gameFile;
                if (gameFile != null)
                {
                    gameFile.LastPlayed = new DateTime?(DateTime.Now);
                    this.m_dtStartPlay = DateTime.Now;
                    GameFileFieldType[] updateFields = new GameFileFieldType[] { GameFileFieldType.LastPlayed };
                    this.DataSourceAdapter.UpdateGameFile(gameFile, updateFields);
                    this.UpdateDataSourceViews(gameFile);
                }
                return true;
            }
            MessageBox.Show(this, playAdapter.LastError, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
        }

        private void syncHandler_GameFileDataNeeded(object sender, EventArgs e)
        {
            SyncLibraryHandler handler = sender as SyncLibraryHandler;
            if (handler != null)
            {
                if (base.InvokeRequired)
                {
                    object[] args = new object[] { handler };
                    base.Invoke(new Action<SyncLibraryHandler>(this.HandleGameFileDataNeeded), args);
                }
                else
                {
                    this.HandleGameFileDataNeeded(handler);
                }
            }
        }

        private void syncHandler_SyncFileChange(object sender, EventArgs e)
        {
            SyncLibraryHandler handler = sender as SyncLibraryHandler;
            if (handler != null)
            {
                if (base.InvokeRequired)
                {
                    object[] args = new object[] { handler };
                    base.Invoke(new Action<SyncLibraryHandler>(this.ProgressBarUpdate), args);
                }
                else
                {
                    this.ProgressBarUpdate(handler);
                }
            }
        }

        private void SyncIWads(string[] files)
        {
            IEnumerable<string> second = from x in this.DataSourceAdapter.GetIWads() select x.Name;
            foreach (string str in files.Except<string>(second))
            {
                try
                {
                    IWadDataSource ds = new IWadDataSource();
                    ds.FileName = ds.Name = str;
                    this.DataSourceAdapter.InsertIWad(ds);
                }
                catch (Exception exception)
                {
                    DoomLauncher.Util.DisplayUnexpectedException(this, exception);
                }
            }
        }

        [AsyncStateMachine(typeof(<SyncLocalDatabase>d__40))]
        private void SyncLocalDatabase(string[] fileNames, bool async)
        {
            <SyncLocalDatabase>d__40 d__;
            d__.<>4__this = this;
            d__.fileNames = fileNames;
            d__.async = async;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SyncLocalDatabase>d__40>(ref d__);
        }

        private void SyncLocalDatabaseComplete(SyncLibraryHandler handler)
        {
            this.SetIWadGameFiles();
            this.UpdateLocal();
            this.HandleTabSelectionChange();
            if ((handler != null) && ((handler.InvalidFiles.Length != 0) || (this.m_zdlInvalidFiles.Count > 0)))
            {
                this.DisplayInvalidFilesError(handler.InvalidFiles.Union<InvalidFile>(this.m_zdlInvalidFiles));
            }
            else if (this.m_launchFile != null)
            {
                IGameFileDataSource gameFile = this.DataSourceAdapter.GetGameFile(new FileInfo(this.m_launchFile).Name);
                this.m_launchFile = null;
                if (gameFile != null)
                {
                    IGameFileDataSource[] gameFiles = new IGameFileDataSource[] { gameFile };
                    this.HandlePlay(gameFiles);
                }
            }
        }

        private void SyncPendingZdlFiles()
        {
            foreach (IGameFileDataSource source in this.m_pendingZdlFiles)
            {
                IGameFileDataSource gameFile = this.DataSourceAdapter.GetGameFile(source.FileName);
                if (gameFile != null)
                {
                    gameFile.SettingsSkill = source.SettingsSkill;
                    gameFile.SettingsMap = source.SettingsMap;
                    gameFile.SettingsExtraParams = source.SettingsExtraParams;
                    gameFile.SourcePortID = source.SourcePortID;
                    gameFile.IWadID = source.IWadID;
                    gameFile.SettingsSkill = source.SettingsSkill;
                    gameFile.SettingsFiles = source.SettingsFiles;
                    if (string.IsNullOrEmpty(gameFile.Comments))
                    {
                        gameFile.Comments = source.Comments;
                    }
                    this.DataSourceAdapter.UpdateGameFile(gameFile);
                }
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HandleTabSelectionChange();
        }

        private void tagToolStripItem_Click(object sender, EventArgs e)
        {
            ToolStripItem strip = sender as ToolStripItem;
            IGameFileDataSource[] sourceArray = this.SelectedItems(this.GetCurrentViewControl());
            if (strip != null)
            {
                ITagDataSource tag = (from item in this.Tags
                    where item.Name == strip.Text
                    select item).FirstOrDefault<ITagDataSource>();
                if (tag != null)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (IGameFileDataSource source2 in sourceArray)
                    {
                        TagMappingDataSource source3 = new TagMappingDataSource {
                            FileID = source2.GameFileID.Value,
                            TagID = tag.TagID
                        };
                        if (!this.DataSourceAdapter.GetTagMappings(source3.FileID).Contains<ITagMappingDataSource>(source3))
                        {
                            this.DataSourceAdapter.InsertTagMapping(source3);
                        }
                        else
                        {
                            builder.Append(source2.FileName);
                            builder.Append(", ");
                        }
                    }
                    this.TagMapLookup.Refresh();
                    this.UpdateTagTabData(tag);
                    this.HandleTabSelectionChange();
                    if (builder.Length > 0)
                    {
                        builder.Remove(builder.Length - 2, 2);
                        builder.Insert(0, "The file(s) ");
                        builder.Append(" already have the tag ");
                        builder.Append(tag.Name);
                        MessageBox.Show(this, builder.ToString(), "Already Tagged", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private static string ToZipExtension(FileInfo fi) => 
            fi.Name.Replace(fi.Extension, ".zip");

        private void UpdateColumnConfig()
        {
            IEnumerable<IConfigurationDataSource> configuration = this.DataSourceAdapter.GetConfiguration();
            this.UpdateConfig(configuration, "ColumnConfig", this.BuildColumnConfig());
        }

        private void UpdateConfig(IEnumerable<IConfigurationDataSource> config, string name, string value)
        {
            IConfigurationDataSource ds = (from item in config
                where item.Name == name
                select item).FirstOrDefault<IConfigurationDataSource>();
            if (ds == null)
            {
                ds = new ConfigurationDataSource {
                    Name = name,
                    Value = value,
                    UserCanModify = false
                };
                this.DataSourceAdapter.InsertConfiguration(ds);
            }
            else
            {
                ds.Value = value;
                this.DataSourceAdapter.UpdateConfiguration(ds);
            }
        }

        private void UpdateDataSourceViews(IGameFileDataSource ds)
        {
            ITabView[] tabViews = this.m_tabHandler.TabViews;
            for (int i = 0; i < tabViews.Length; i++)
            {
                tabViews[i].UpdateDataSourceFile(ds);
            }
        }

        private void UpdateLocal()
        {
            foreach (ITabView view in this.m_tabHandler.TabViews)
            {
                if (view.IsLocal)
                {
                    view.SetGameFiles();
                }
            }
        }

        [AsyncStateMachine(typeof(<updateMetadataToolStripMenuItem_Click>d__99))]
        private void updateMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            <updateMetadataToolStripMenuItem_Click>d__99 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<updateMetadataToolStripMenuItem_Click>d__99>(ref d__);
        }

        private void UpdateProgressBar(ProgressBarForm form, string text, int value)
        {
            if (base.InvokeRequired)
            {
                object[] args = new object[] { form, text, value };
                base.Invoke(new Action<ProgressBarForm, string, int>(this.UpdateProgressBar), args);
            }
            else
            {
                form.DisplayText = text;
                form.Value = value;
            }
        }

        private void UpdateTagTabData(ITagDataSource tag)
        {
            ITabView view = (from item in this.m_tabHandler.TabViews
                where item.Key.Equals(tag.TagID) && (item is TagTabView)
                select item).FirstOrDefault<ITabView>();
            if (view != null)
            {
                view.SetGameFiles();
            }
        }

        private void UpdateVersionProgress()
        {
            this.m_progressBarUpdate.Value = this.m_versionHandler.ProgressPercent;
        }

        private static bool ValidatePosition(DoomLauncher.AppConfiguration config, Form form)
        {
            if (config.WindowState != FormWindowState.Minimized)
            {
                Rectangle formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);
                return ((from item in Screen.AllScreens
                    where item.WorkingArea.Contains(formRectangle)
                    select item).Count<Screen>() > 0);
            }
            return true;
        }

        private bool VerifyDatabase()
        {
            bool flag = false;
            try
            {
                flag = this.InitFileCheck("DoomLauncher.sqlite", "DoomLauncher_.sqlite", false);
                if (!flag)
                {
                    MessageBox.Show(this, "Initialization failure. Could not find DoomLauncher database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception exception)
            {
                DoomLauncher.Util.DisplayUnexpectedException(this, exception);
            }
            return flag;
        }

        private static bool VerifyFileName(string fileName) => 
            (fileName.Except<char>(Path.GetInvalidFileNameChars()).Count<char>() == fileName.Distinct<char>().Count<char>());

        private bool VerifyGameFilesDirectory()
        {
            bool flag = false;
            try
            {
                flag = this.InitFileCheck("GameFiles", "GameFiles_", true);
                if (flag)
                {
                    return flag;
                }
                MessageBox.Show(this, "Initialization failure. Could not find DoomLauncher GameFiles directory. Please update your settings to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                DialogResult oK = DialogResult.OK;
                bool flag2 = false;
                do
                {
                    flag2 = this.ShowSettings(true, out oK);
                }
                while ((oK != DialogResult.Cancel) && !flag2);
                flag = flag2;
            }
            catch (Exception exception)
            {
                DoomLauncher.Util.DisplayUnexpectedException(this, exception);
            }
            return flag;
        }

        private void viewTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleViewTextFile();
        }

        private void viewWebPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.HandleViewWebPage();
        }

        private void wadarchiveStripMenuItem_Click(object sender, EventArgs e)
        {
            IGameFileDataSource source = this.SelectedItems(this.GetCurrentViewControl()).FirstOrDefault<IGameFileDataSource>();
            if (source != null)
            {
                ZipArchiveEntry entry = ZipFile.OpenRead(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), source.FileName)).Entries.First<ZipArchiveEntry>();
                string destinationFileName = Path.Combine(this.AppConfiguration.TempDirectory.GetFullPath(), entry.Name);
                entry.ExtractToFile(destinationFileName, true);
                MessageBox.Show(new WadArchiveDataAdapter().Test(Path.Combine(this.AppConfiguration.TempDirectory.GetFullPath(), destinationFileName)).FileName);
            }
        }

        private void WriteDownloadFile(IGameFileDownloadable dlFile)
        {
            try
            {
                FileInfo info = new FileInfo(Path.Combine(this.AppConfiguration.TempDirectory.GetFullPath(), dlFile.FileName));
                info.CopyTo(Path.Combine(this.AppConfiguration.GameFileDirectory.GetFullPath(), dlFile.FileName), true);
                info.Delete();
                string[] fileNames = new string[] { info.Name };
                this.SyncLocalDatabase(fileNames, true);
                this.UpdateLocal();
            }
            catch (IOException)
            {
                MessageBox.Show(this, $"The file {dlFile.FileName} is in use and cannot be written.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (Exception exception)
            {
                DoomLauncher.Util.DisplayUnexpectedException(this, exception);
            }
        }

        private DoomLauncher.AppConfiguration AppConfiguration { get; set; }

        private IGameFileDataSource CurrentDownloadFile { get; set; }

        private IDataSourceAdapter DataSourceAdapter { get; set; }

        private Tuple<string, string>[] DefaultColumnTextFields =>
            new Tuple<string, string>[] { new Tuple<string, string>("FileName", "File"), new Tuple<string, string>("Title", "Title"), new Tuple<string, string>("Author", "Author"), new Tuple<string, string>("ReleaseDate", "Release Date"), new Tuple<string, string>("MapCount", "Maps"), new Tuple<string, string>("Comments", "Comments"), new Tuple<string, string>("Rating", "Rating"), new Tuple<string, string>("Downloaded", "Downloaded"), new Tuple<string, string>("LastPlayed", "Last Played") };

        private GameFileFieldType[] DefaultGameFileSelectFields =>
            new GameFileFieldType[] { GameFileFieldType.GameFileID };

        private GameFileFieldType[] DefaultGameFileUpdateFields =>
            new GameFileFieldType[] { GameFileFieldType.Author };

        private IGameFileDataSourceAdapter DirectoryDataSourceAdapter { get; set; }

        private IGameFileDataSourceAdapter IdGamesDataSourceAdapter { get; set; }

        private ITagMapLookup TagMapLookup { get; set; }

        private ITagDataSource[] Tags { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly MainForm.<>c <>9 = new MainForm.<>c();
            public static Func<ITabView, bool> <>9__102_1;
            public static Func<ITagDataSource, bool> <>9__12_0;
            public static Func<ITagDataSource, string> <>9__12_1;
            public static Func<ToolStripItem, bool> <>9__12_2;
            public static Func<ColumnConfig, bool> <>9__13_0;
            public static Func<ITagDataSource, string> <>9__14_0;
            public static Func<ToolStripItem, bool> <>9__14_1;
            public static Func<ToolStripItem, bool> <>9__14_2;
            public static Func<ZipArchiveEntry, bool> <>9__148_0;
            public static Func<ITagDataSource, int> <>9__159_0;
            public static Func<ITagMappingDataSource, int> <>9__159_1;
            public static Func<ITagDataSource, ITagMappingDataSource, ITagDataSource> <>9__159_2;
            public static Func<ITabView, bool> <>9__168_0;
            public static Func<ColumnConfig, bool> <>9__17_0;
            public static Func<Tuple<string, string>, bool> <>9__17_2;
            public static Func<ITagDataSource, int> <>9__172_0;
            public static Func<ITagMappingDataSource, int> <>9__172_1;
            public static Func<ITagDataSource, ITagMappingDataSource, ITagDataSource> <>9__172_2;
            public static Func<ITabView, bool> <>9__190_0;
            public static Func<ZipArchiveEntry, bool> <>9__192_0;
            public static Func<string, bool> <>9__199_0;
            public static Func<string, string> <>9__201_1;
            public static Func<string, string> <>9__201_2;
            public static Func<string, string> <>9__201_3;
            public static Func<IGameFileDataSource, string> <>9__204_0;
            public static Func<string, bool> <>9__205_0;
            public static Func<IIWadDataSource, string> <>9__53_0;
            public static Func<IGameFileDataSource, string> <>9__58_0;
            public static Func<ITabView, bool> <>9__66_0;
            public static Func<IGameFileDataSource, bool> <>9__69_2;
            public static Func<ITabView, bool> <>9__71_0;
            public static Func<ITabView, bool> <>9__71_1;
            public static Action<INewFileDetector> <>9__80_0;
            public static Action<INewFileDetector> <>9__80_1;
            public static Func<FileInfo, DateTime> <>9__9_1;
            public static Func<IFileDataSource, string> <>9__97_0;

            internal int <BuildTagText>b__159_0(ITagDataSource tag) => 
                tag.TagID;

            internal int <BuildTagText>b__159_1(ITagMappingDataSource map) => 
                map.TagID;

            internal ITagDataSource <BuildTagText>b__159_2(ITagDataSource tag, ITagMappingDataSource map) => 
                tag;

            internal DateTime <CleanupBackupDirectory>b__9_1(FileInfo x) => 
                x.CreationTime;

            internal void <CreateFileDetectors>b__80_0(INewFileDetector x)
            {
                x.StartDetection();
            }

            internal void <CreateFileDetectors>b__80_1(INewFileDetector x)
            {
                x.StartDetection();
            }

            internal bool <CreateTagTab>b__13_0(ColumnConfig item) => 
                (item.Parent == "Local");

            internal bool <CreateTagTabs>b__12_0(ITagDataSource item) => 
                item.HasTab;

            internal string <CreateTagTabs>b__12_1(ITagDataSource x) => 
                x.Name;

            internal bool <CreateTagTabs>b__12_2(ToolStripItem item) => 
                (item.Text == "Tag");

            internal string <DisplayFilesNotFound>b__58_0(IGameFileDataSource x) => 
                x.FileName;

            internal bool <GetAdditionalTabViews>b__71_0(ITabView view) => 
                (view.Title == "Local");

            internal bool <GetAdditionalTabViews>b__71_1(ITabView view) => 
                (view is TagTabView);

            internal string <GetNewSaveGames>b__97_0(IFileDataSource x) => 
                x.OriginalFileName;

            internal bool <GetSecondaryFile>b__192_0(ZipArchiveEntry x) => 
                x.Name.Contains(".wad");

            internal bool <GetZdlFiles>b__205_0(string file) => 
                (new FileInfo(file).Extension == ".zdl");

            internal bool <HandleAddGameFiles>b__199_0(string item) => 
                !File.Exists(item);

            internal string <HandleCopyFiles>b__201_1(string x) => 
                MainForm.ToZipExtension(new FileInfo(x));

            internal string <HandleCopyFiles>b__201_2(string x) => 
                new FileInfo(x).Name;

            internal string <HandleCopyFiles>b__201_3(string x) => 
                MainForm.ToZipExtension(new FileInfo(x));

            internal bool <HandleDownload>b__168_0(ITabView x) => 
                (x is IdGamesTabViewCtrl);

            internal int <HandleEdit>b__172_0(ITagDataSource tag) => 
                tag.TagID;

            internal int <HandleEdit>b__172_1(ITagMappingDataSource map) => 
                map.TagID;

            internal ITagDataSource <HandleEdit>b__172_2(ITagDataSource tag, ITagMappingDataSource map) => 
                tag;

            internal bool <HandleMultipleMetaFilesFound>b__102_1(ITabView view) => 
                view.Key.Equals("Id Games");

            internal bool <HandleViewTextFile>b__148_0(ZipArchiveEntry zipItem) => 
                zipItem.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase);

            internal string <HandleZdlFiles>b__204_0(IGameFileDataSource item) => 
                new FileInfo(item.FileName).Name;

            internal bool <PromptUserMainFile>b__66_0(ITabView tab) => 
                tab.Key.Equals("Local");

            internal string <RebuildTagToolStrip>b__14_0(ITagDataSource x) => 
                x.Name;

            internal bool <RebuildTagToolStrip>b__14_1(ToolStripItem item) => 
                (item.Text == "Tag");

            internal bool <RebuildTagToolStrip>b__14_2(ToolStripItem item) => 
                (item.Text == "Remove Tag");

            internal bool <RefreshConfigItems>b__190_0(ITabView x) => 
                (x is IdGamesTabViewCtrl);

            internal bool <SetupSourcePortForm>b__69_2(IGameFileDataSource x) => 
                (x > null);

            internal bool <SortColumns>b__17_0(ColumnConfig item) => 
                (item.Column == "MapCount");

            internal bool <SortColumns>b__17_2(Tuple<string, string> x) => 
                x.Item2.Equals("maps", StringComparison.InvariantCultureIgnoreCase);

            internal string <SyncIWads>b__53_0(IIWadDataSource x) => 
                x.Name;
        }

        [CompilerGenerated]
        private static class <>o__27
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Action<CallSite, object>> <>p__3;
            public static CallSite<Action<CallSite, System.Type, object>> <>p__4;
            public static CallSite<Action<CallSite, System.Type, object>> <>p__5;
        }

        [CompilerGenerated]
        private struct <HandleCopyFiles>d__201 : IAsyncStateMachine
        {
            public int <>1__state;
            public MainForm <>4__this;
            private MainForm.<>c__DisplayClass201_0 <>8__1;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter <>u__1;
            public string[] fileNames;
            public AddFileType type;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<>8__1 = new MainForm.<>c__DisplayClass201_0();
                        this.<>8__1.<>4__this = this.<>4__this;
                        this.<>8__1.fileNames = this.fileNames;
                        this.<>8__1.progressBar = this.<>4__this.CreateProgressBar("Copying...", ProgressBarStyle.Marquee);
                        this.<>8__1.progressBar.Cancelled += new EventHandler(this.<>4__this.m_progressBarFormCopy_Cancelled);
                        this.<>4__this.ProgressBarStart(this.<>8__1.progressBar);
                        awaiter = Task.Run(new Action(this.<>8__1.<HandleCopyFiles>b__0)).GetAwaiter();
                        if (!awaiter.IsCompleted)
                        {
                            this.<>1__state = num = 0;
                            this.<>u__1 = awaiter;
                            this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MainForm.<HandleCopyFiles>d__201>(ref awaiter, ref this);
                            return;
                        }
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                    }
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    this.<>4__this.ProgressBarEnd(this.<>8__1.progressBar);
                    switch (this.type)
                    {
                        case AddFileType.GameFile:
                        {
                            string[] fileNames = this.<>8__1.fileNames.Select<string, string>((MainForm.<>c.<>9__201_1 ?? (MainForm.<>c.<>9__201_1 = new Func<string, string>(MainForm.<>c.<>9.<HandleCopyFiles>b__201_1)))).ToArray<string>();
                            this.<>4__this.SyncLocalDatabase(fileNames, true);
                            break;
                        }
                        case AddFileType.IWad:
                            goto Label_0170;
                    }
                    goto Label_0212;
                Label_0170:;
                    string[] files = this.<>8__1.fileNames.Select<string, string>((MainForm.<>c.<>9__201_2 ?? (MainForm.<>c.<>9__201_2 = new Func<string, string>(MainForm.<>c.<>9.<HandleCopyFiles>b__201_2)))).ToArray<string>();
                    this.<>4__this.SyncIWads(files);
                    files = this.<>8__1.fileNames.Select<string, string>((MainForm.<>c.<>9__201_3 ?? (MainForm.<>c.<>9__201_3 = new Func<string, string>(MainForm.<>c.<>9.<HandleCopyFiles>b__201_3)))).ToArray<string>();
                    this.<>4__this.SyncLocalDatabase(files, true);
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
            Label_0212:
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <HandleSyncStatus>d__54 : IAsyncStateMachine
        {
            public int <>1__state;
            public MainForm <>4__this;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter <>u__1;
            private IEnumerable<string> <dbFiles>5__1;
            private IEnumerable<string> <dsFiles>5__2;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    IEnumerable<string> enumerable;
                    SyncStatusForm form;
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        if (num != 1)
                        {
                            this.<dsFiles>5__2 = this.<>4__this.DirectoryDataSourceAdapter.GetGameFileNames();
                            this.<dbFiles>5__1 = this.<>4__this.DataSourceAdapter.GetGameFileNames();
                            enumerable = this.<dsFiles>5__2.Except<string>(this.<dbFiles>5__1);
                            string[] textArray1 = new string[] { "Do Nothing", "Add to Library", "Delete" };
                            form = this.<>4__this.ShowSyncStatusForm("Sync Status", "Files that exist in the GameFiles directory but not the Database:", enumerable, textArray1);
                            awaiter = this.<>4__this.HandleSyncStatusGameFilesOption((SyncFileOption) form.SelectedOptionIndex, form.GetSelectedFiles()).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                this.<>1__state = num = 0;
                                this.<>u__1 = awaiter;
                                this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MainForm.<HandleSyncStatus>d__54>(ref awaiter, ref this);
                                return;
                            }
                            goto Label_00EC;
                        }
                        goto Label_0188;
                    }
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter();
                    this.<>1__state = num = -1;
                Label_00EC:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    enumerable = this.<dbFiles>5__1.Except<string>(this.<dsFiles>5__2);
                    string[] dropDownOptions = new string[] { "Do Nothing", "Find in idgames", "Delete" };
                    form = this.<>4__this.ShowSyncStatusForm("Sync Status", "Files that exist in the Database but not the GameFiles directory:", enumerable, dropDownOptions);
                    awaiter = this.<>4__this.HandleSyncStatusLibraryOptions((SyncFileOption) form.SelectedOptionIndex, form.GetSelectedFiles()).GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_01A4;
                    }
                    this.<>1__state = num = 1;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MainForm.<HandleSyncStatus>d__54>(ref awaiter, ref this);
                    return;
                Label_0188:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter();
                    this.<>1__state = num = -1;
                Label_01A4:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <HandleSyncStatusGameFilesOption>d__55 : IAsyncStateMachine
        {
            public int <>1__state;
            public MainForm <>4__this;
            private MainForm.<>c__DisplayClass55_0 <>8__2;
            public AsyncTaskMethodBuilder <>t__builder;
            private TaskAwaiter<SyncLibraryHandler> <>u__1;
            private TaskAwaiter <>u__2;
            private ProgressBarForm <form>5__1;
            public IEnumerable<string> files;
            public SyncFileOption option;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter<SyncLibraryHandler> awaiter;
                    switch (num)
                    {
                        case 0:
                            break;

                        case 1:
                            goto Label_01AF;

                        default:
                        {
                            this.<>8__2 = new MainForm.<>c__DisplayClass55_0();
                            this.<>8__2.<>4__this = this.<>4__this;
                            this.<>8__2.files = this.files;
                            this.<form>5__1 = this.<>4__this.CreateProgressBar(string.Empty, ProgressBarStyle.Marquee);
                            SyncFileOption option = this.option;
                            if (option != SyncFileOption.Add)
                            {
                                if (option == SyncFileOption.Delete)
                                {
                                    goto Label_013C;
                                }
                                goto Label_0207;
                            }
                            this.<>4__this.m_progressBarSync = this.<>4__this.CreateProgressBar("Updating...", ProgressBarStyle.Continuous);
                            this.<>4__this.ProgressBarStart(this.<>4__this.m_progressBarSync);
                            awaiter = Task.Run<SyncLibraryHandler>(new Func<SyncLibraryHandler>(this.<>8__2.<HandleSyncStatusGameFilesOption>b__0)).GetAwaiter();
                            if (awaiter.IsCompleted)
                            {
                                goto Label_0105;
                            }
                            this.<>1__state = num = 0;
                            this.<>u__1 = awaiter;
                            this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<SyncLibraryHandler>, MainForm.<HandleSyncStatusGameFilesOption>d__55>(ref awaiter, ref this);
                            return;
                        }
                    }
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<SyncLibraryHandler>();
                    this.<>1__state = num = -1;
                Label_0105:
                    SyncLibraryHandler introduced6 = awaiter.GetResult();
                    awaiter = new TaskAwaiter<SyncLibraryHandler>();
                    SyncLibraryHandler handler = introduced6;
                    this.<>4__this.ProgressBarEnd(this.<>4__this.m_progressBarSync);
                    this.<>4__this.SyncLocalDatabaseComplete(handler);
                    goto Label_0207;
                Label_013C:
                    this.<form>5__1.DisplayText = string.Format("Deleting...", new object[0]);
                    this.<>4__this.ProgressBarStart(this.<form>5__1);
                    TaskAwaiter awaiter2 = Task.Run(new Action(this.<>8__2.<HandleSyncStatusGameFilesOption>b__1)).GetAwaiter();
                    if (awaiter2.IsCompleted)
                    {
                        goto Label_01CC;
                    }
                    this.<>1__state = num = 1;
                    this.<>u__2 = awaiter2;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MainForm.<HandleSyncStatusGameFilesOption>d__55>(ref awaiter2, ref this);
                    return;
                Label_01AF:
                    awaiter2 = this.<>u__2;
                    this.<>u__2 = new TaskAwaiter();
                    this.<>1__state = num = -1;
                Label_01CC:
                    awaiter2.GetResult();
                    awaiter2 = new TaskAwaiter();
                    this.<>4__this.ProgressBarEnd(this.<form>5__1);
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
            Label_0207:
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <HandleSyncStatusLibraryOptions>d__57 : IAsyncStateMachine
        {
            public int <>1__state;
            public MainForm <>4__this;
            private MainForm.<>c__DisplayClass57_0 <>8__2;
            public AsyncTaskMethodBuilder <>t__builder;
            private TaskAwaiter<List<IGameFileDataSource>> <>u__1;
            private TaskAwaiter <>u__2;
            private ProgressBarForm <form>5__1;
            public IEnumerable<string> files;
            public SyncFileOption option;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter<List<IGameFileDataSource>> awaiter;
                    switch (num)
                    {
                        case 0:
                            break;

                        case 1:
                            goto Label_0218;

                        default:
                        {
                            this.<>8__2 = new MainForm.<>c__DisplayClass57_0();
                            this.<>8__2.<>4__this = this.<>4__this;
                            this.<>8__2.files = this.files;
                            this.<form>5__1 = new ProgressBarForm();
                            this.<form>5__1.ProgressBarStyle = ProgressBarStyle.Marquee;
                            SyncFileOption option = this.option;
                            if (option != SyncFileOption.Add)
                            {
                                if (option == SyncFileOption.Delete)
                                {
                                    goto Label_01A5;
                                }
                                goto Label_027B;
                            }
                            this.<form>5__1.DisplayText = string.Format("Searching...", new object[0]);
                            this.<>4__this.ProgressBarStart(this.<form>5__1);
                            awaiter = Task.Run<List<IGameFileDataSource>>(new Func<List<IGameFileDataSource>>(this.<>8__2.<HandleSyncStatusLibraryOptions>b__0)).GetAwaiter();
                            if (awaiter.IsCompleted)
                            {
                                goto Label_00FF;
                            }
                            this.<>1__state = num = 0;
                            this.<>u__1 = awaiter;
                            this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<IGameFileDataSource>>, MainForm.<HandleSyncStatusLibraryOptions>d__57>(ref awaiter, ref this);
                            return;
                        }
                    }
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<List<IGameFileDataSource>>();
                    this.<>1__state = num = -1;
                Label_00FF:
                    List<IGameFileDataSource> introduced8 = awaiter.GetResult();
                    awaiter = new TaskAwaiter<List<IGameFileDataSource>>();
                    List<IGameFileDataSource> gameFiles = introduced8;
                    List<IGameFileDataSource>.Enumerator enumerator = gameFiles.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            IGameFileDataSource current = enumerator.Current;
                            this.<>4__this.m_downloadHandler.Download(this.<>4__this.IdGamesDataSourceAdapter, current as IGameFileDownloadable);
                        }
                    }
                    finally
                    {
                        if (num < 0)
                        {
                            enumerator.Dispose();
                        }
                    }
                    this.<>4__this.ProgressBarEnd(this.<form>5__1);
                    this.<>4__this.DisplayFilesNotFound(this.<>8__2.files, gameFiles);
                    if (gameFiles.Count > 0)
                    {
                        this.<>4__this.DisplayDownloads();
                    }
                    goto Label_027B;
                Label_01A5:
                    this.<form>5__1.DisplayText = string.Format("Deleting...", new object[0]);
                    this.<>4__this.ProgressBarStart(this.<form>5__1);
                    TaskAwaiter awaiter2 = Task.Run(new Action(this.<>8__2.<HandleSyncStatusLibraryOptions>b__1)).GetAwaiter();
                    if (awaiter2.IsCompleted)
                    {
                        goto Label_0235;
                    }
                    this.<>1__state = num = 1;
                    this.<>u__2 = awaiter2;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MainForm.<HandleSyncStatusLibraryOptions>d__57>(ref awaiter2, ref this);
                    return;
                Label_0218:
                    awaiter2 = this.<>u__2;
                    this.<>u__2 = new TaskAwaiter();
                    this.<>1__state = num = -1;
                Label_0235:
                    awaiter2.GetResult();
                    awaiter2 = new TaskAwaiter();
                    this.<>4__this.ProgressBarEnd(this.<form>5__1);
                    this.<>4__this.UpdateLocal();
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
            Label_027B:
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <HandleVersionUpdate>d__20 : IAsyncStateMachine
        {
            public int <>1__state;
            public MainForm <>4__this;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter <>u__1;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        string str = Path.Combine(Directory.GetCurrentDirectory(), "DoomLauncher.sqlite");
                        DataAccess access = new DataAccess(new SqliteDatabaseAdapter(), $"Data Source={str}");
                        this.<>4__this.m_versionHandler = new VersionHandler(access, this.<>4__this.DataSourceAdapter, this.<>4__this.AppConfiguration);
                        if (this.<>4__this.m_versionHandler.UpdateRequired())
                        {
                            this.<>4__this.m_versionHandler.UpdateProgress += new EventHandler(this.<>4__this.handler_UpdateProgress);
                            this.<>4__this.m_progressBarUpdate = this.<>4__this.CreateProgressBar("Updating...", ProgressBarStyle.Continuous);
                            this.<>4__this.ProgressBarStart(this.<>4__this.m_progressBarUpdate);
                            awaiter = Task.Run(new Action(this.<>4__this.<HandleVersionUpdate>b__20_0)).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                this.<>1__state = num = 0;
                                this.<>u__1 = awaiter;
                                this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MainForm.<HandleVersionUpdate>d__20>(ref awaiter, ref this);
                                return;
                            }
                            goto Label_0123;
                        }
                        goto Label_0148;
                    }
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter();
                    this.<>1__state = num = -1;
                Label_0123:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    this.<>4__this.ProgressBarEnd(this.<>4__this.m_progressBarUpdate);
                Label_0148:
                    this.<>4__this.Initialize();
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SyncLocalDatabase>d__40 : IAsyncStateMachine
        {
            public int <>1__state;
            public MainForm <>4__this;
            private MainForm.<>c__DisplayClass40_0 <>8__1;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter<SyncLibraryHandler> <>u__1;
            public bool async;
            public string[] fileNames;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter<SyncLibraryHandler> awaiter;
                    SyncLibraryHandler handler2;
                    if (num != 0)
                    {
                        this.<>8__1 = new MainForm.<>c__DisplayClass40_0();
                        this.<>8__1.<>4__this = this.<>4__this;
                        this.<>8__1.fileNames = this.fileNames;
                        if (this.async)
                        {
                            this.<>4__this.m_progressBarSync = this.<>4__this.CreateProgressBar("Updating...", ProgressBarStyle.Continuous);
                            this.<>4__this.ProgressBarStart(this.<>4__this.m_progressBarSync);
                            awaiter = Task.Run<SyncLibraryHandler>(new Func<SyncLibraryHandler>(this.<>8__1.<SyncLocalDatabase>b__0)).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                this.<>1__state = num = 0;
                                this.<>u__1 = awaiter;
                                this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<SyncLibraryHandler>, MainForm.<SyncLocalDatabase>d__40>(ref awaiter, ref this);
                                return;
                            }
                            goto Label_00DB;
                        }
                        goto Label_010F;
                    }
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<SyncLibraryHandler>();
                    this.<>1__state = num = -1;
                Label_00DB:
                    SyncLibraryHandler introduced5 = awaiter.GetResult();
                    awaiter = new TaskAwaiter<SyncLibraryHandler>();
                    SyncLibraryHandler handler = introduced5;
                    this.<>4__this.ProgressBarEnd(this.<>4__this.m_progressBarSync);
                    this.<>4__this.SyncLocalDatabaseComplete(handler);
                    goto Label_014D;
                Label_010F:
                    handler2 = this.<>4__this.ExecuteSyncHandler(this.<>8__1.fileNames);
                    this.<>4__this.SyncLocalDatabaseComplete(handler2);
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
            Label_014D:
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <updateMetadataToolStripMenuItem_Click>d__99 : IAsyncStateMachine
        {
            public int <>1__state;
            public MainForm <>4__this;
            private IGameFileDataSource[] <>7__wrap1;
            private int <>7__wrap2;
            private MainForm.<>c__DisplayClass99_0 <>8__1;
            private MainForm.<>c__DisplayClass99_1 <>8__4;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter<IEnumerable<IGameFileDataSource>> <>u__1;
            private MetaDataForm <form>5__5;
            private ProgressBarForm <progress>5__2;
            private DialogResult <result>5__7;
            private bool <showError>5__3;
            private bool <showForm>5__6;
            private bool <updateView>5__8;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    if (num == 0)
                    {
                        goto Label_0118;
                    }
                    this.<>8__1 = new MainForm.<>c__DisplayClass99_0();
                    this.<>8__1.<>4__this = this.<>4__this;
                    this.<>8__1.adapter = new IdGamesDataAdapater(this.<>4__this.AppConfiguration.IdGamesUrl, this.<>4__this.AppConfiguration.ApiPage, this.<>4__this.AppConfiguration.MirrorUrl);
                    IGameFileDataSource[] sourceArray = this.<>4__this.SelectedItems(this.<>4__this.GetCurrentViewControl());
                    if (sourceArray.Length != 0)
                    {
                        this.<showForm>5__6 = true;
                        this.<showError>5__3 = true;
                        this.<updateView>5__8 = false;
                        this.<>4__this.m_cancelMetaUpdate = false;
                        this.<result>5__7 = DialogResult.Cancel;
                        this.<form>5__5 = this.<>4__this.CreateMetaForm();
                        this.<progress>5__2 = this.<>4__this.InitMetaProgressBar();
                        this.<>7__wrap1 = sourceArray;
                        this.<>7__wrap2 = 0;
                        while (this.<>7__wrap2 < this.<>7__wrap1.Length)
                        {
                            this.<>8__4 = new MainForm.<>c__DisplayClass99_1();
                            this.<>8__4.CS$<>8__locals1 = this.<>8__1;
                            this.<>8__4.localFile = this.<>7__wrap1[this.<>7__wrap2];
                        Label_0118:;
                            try
                            {
                                TaskAwaiter<IEnumerable<IGameFileDataSource>> awaiter;
                                if (num != 0)
                                {
                                    this.<>4__this.Enabled = false;
                                    this.<progress>5__2.DisplayText = $"Searching for {this.<>8__4.localFile.FileName}...";
                                    this.<progress>5__2.Show(this.<>4__this);
                                    awaiter = Task.Run<IEnumerable<IGameFileDataSource>>(new Func<IEnumerable<IGameFileDataSource>>(this.<>8__4.<updateMetadataToolStripMenuItem_Click>b__0)).GetAwaiter();
                                    if (!awaiter.IsCompleted)
                                    {
                                        this.<>1__state = num = 0;
                                        this.<>u__1 = awaiter;
                                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IEnumerable<IGameFileDataSource>>, MainForm.<updateMetadataToolStripMenuItem_Click>d__99>(ref awaiter, ref this);
                                        return;
                                    }
                                }
                                else
                                {
                                    awaiter = this.<>u__1;
                                    this.<>u__1 = new TaskAwaiter<IEnumerable<IGameFileDataSource>>();
                                    this.<>1__state = num = -1;
                                }
                                IEnumerable<IGameFileDataSource> result = awaiter.GetResult();
                                awaiter = new TaskAwaiter<IEnumerable<IGameFileDataSource>>();
                                IEnumerable<IGameFileDataSource> enumerable = result;
                                this.<>4__this.Enabled = true;
                                this.<progress>5__2.Hide();
                                if ((enumerable == null) || this.<>4__this.m_cancelMetaUpdate)
                                {
                                    break;
                                }
                                if (enumerable.Count<IGameFileDataSource>() == 0)
                                {
                                    if (this.<showError>5__3)
                                    {
                                        this.<showError>5__3 = this.<>4__this.HandleMetaError(this.<>8__4.localFile);
                                    }
                                }
                                else
                                {
                                    IGameFileDataSource ds = this.<>4__this.HandleMultipleMetaFilesFound(this.<>8__4.localFile, enumerable);
                                    if (ds != null)
                                    {
                                        this.<form>5__5.GameFileEdit.SetDataSource(ds, new ITagDataSource[0]);
                                        if (this.<showForm>5__6)
                                        {
                                            this.<result>5__7 = this.<form>5__5.ShowDialog(this.<>4__this);
                                        }
                                        if (this.<result>5__7 != DialogResult.Cancel)
                                        {
                                            List<GameFileFieldType> fields = this.<form>5__5.GameFileEdit.UpdateDataSource(this.<>8__4.localFile);
                                            this.<showForm>5__6 = this.<result>5__7 == DialogResult.OK;
                                            if (fields.Count > 0)
                                            {
                                                this.<updateView>5__8 = this.<>4__this.HandleUpdateMetaFields(this.<>8__4.localFile, fields);
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                this.<>4__this.Enabled = true;
                                this.<progress>5__2.Hide();
                                MessageBox.Show(this.<>4__this, "Failed to fetch metadeta from the id games mirror.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                break;
                            }
                            this.<>8__4 = null;
                            this.<>7__wrap2++;
                        }
                        this.<>7__wrap1 = null;
                        if (this.<updateView>5__8)
                        {
                            this.<>4__this.HandleSelectionChange(this.<>4__this.GetCurrentViewControl());
                        }
                        this.<form>5__5 = null;
                        this.<progress>5__2 = null;
                    }
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        private delegate void GameFileDataSourceUpdate(IEnumerable<GameFileSearchField> searchFields);
    }
}

