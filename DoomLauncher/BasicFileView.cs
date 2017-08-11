namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public abstract class BasicFileView : UserControl, IFileAssociationView
    {
        private IFileDataSource[] m_files = new IFileDataSource[0];
        protected ContextMenuStrip m_menu;

        protected BasicFileView()
        {
        }

        public void CopyAllToClipboard()
        {
            StringCollection filePaths = new StringCollection();
            foreach (IFileDataSource source in this.Files)
            {
                if (!source.IsUrl)
                {
                    if (new FileInfo(Path.Combine(this.DataDirectory.GetFullPath(), source.FileName)).Exists)
                    {
                        filePaths.Add(Path.Combine(this.DataDirectory.GetFullPath(), source.FileName));
                    }
                    if (filePaths.Count > 0)
                    {
                        Clipboard.SetFileDropList(filePaths);
                    }
                }
            }
        }

        public void CopyToClipboard()
        {
            List<IFileDataSource> selectedFiles = this.GetSelectedFiles();
            if (selectedFiles.Count > 0)
            {
                StringCollection filePaths = new StringCollection();
                foreach (IFileDataSource source in selectedFiles)
                {
                    if (source.IsUrl)
                    {
                        Process.Start(source.FileName);
                    }
                    else
                    {
                        if (new FileInfo(Path.Combine(this.DataDirectory.GetFullPath(), source.FileName)).Exists)
                        {
                            filePaths.Add(Path.Combine(this.DataDirectory.GetFullPath(), source.FileName));
                        }
                        if (filePaths.Count > 0)
                        {
                            Clipboard.SetFileDropList(filePaths);
                        }
                    }
                }
            }
        }

        private FileDataSource CreateNewFileDataSource(FileDetailsEditForm detailsForm, FileInfo fi) => 
            new FileDataSource { 
                FileName = Guid.NewGuid() + fi.Extension,
                FileTypeID = (int) this.FileType,
                GameFileID = this.GameFile.GameFileID.Value,
                Description = detailsForm.Description,
                SourcePortID = detailsForm.SourcePort.SourcePortID
            };

        public bool Delete()
        {
            List<IFileDataSource> selectedFiles = this.GetSelectedFiles();
            if ((selectedFiles.Count <= 0) || (MessageBox.Show(this, "Delete selected file(s)?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.OK))
            {
                return false;
            }
            foreach (IFileDataSource source in selectedFiles)
            {
                if (!source.IsUrl)
                {
                    FileInfo info = new FileInfo(Path.Combine(this.DataDirectory.GetFullPath(), source.FileName));
                    this.DataSourceAdapter.DeleteFile(source);
                    try
                    {
                        info.Delete();
                    }
                    catch
                    {
                    }
                }
            }
            return true;
        }

        public bool Edit()
        {
            List<IFileDataSource> selectedFiles = this.GetSelectedFiles();
            if (selectedFiles.Count > 0)
            {
                IFileDataSource file = selectedFiles.First<IFileDataSource>();
                FileDetailsEditForm form = new FileDetailsEditForm();
                form.Initialize(this.DataSourceAdapter, file);
                form.StartPosition = FormStartPosition.CenterParent;
                if (((form.ShowDialog(this) == DialogResult.OK) && (form.SourcePort != null)) && !file.IsUrl)
                {
                    file.SourcePortID = form.SourcePort.SourcePortID;
                    file.Description = form.Description;
                    this.DataSourceAdapter.UpdateFile(file);
                    return true;
                }
                if (form.SourcePort == null)
                {
                    MessageBox.Show(this, "A source port must be selected.", "Error", MessageBoxButtons.OK);
                }
            }
            return false;
        }

        protected abstract List<IFileDataSource> GetSelectedFiles();
        public bool MoveFileOrderDown() => 
            this.SetFilePriority(false);

        public bool MoveFileOrderUp() => 
            this.SetFilePriority(true);

        public bool New()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }
            FileDetailsEditForm detailsForm = new FileDetailsEditForm();
            detailsForm.Initialize(this.DataSourceAdapter);
            detailsForm.StartPosition = FormStartPosition.CenterParent;
            if ((detailsForm.ShowDialog(this) == DialogResult.OK) && (detailsForm.SourcePort != null))
            {
                this.GameFile = this.DataSourceAdapter.GetGameFile(this.GameFile.FileName);
                string[] fileNames = dialog.FileNames;
                for (int i = 0; i < fileNames.Length; i++)
                {
                    FileInfo fi = new FileInfo(fileNames[i]);
                    FileDataSource ds = this.CreateNewFileDataSource(detailsForm, fi);
                    fi.CopyTo(Path.Combine(this.DataDirectory.GetFullPath(), ds.FileName));
                    this.DataSourceAdapter.InsertFile(ds);
                }
            }
            else if (detailsForm.SourcePort == null)
            {
                this.ShowSourcePortError();
            }
            return true;
        }

        public void SetContextMenu(ContextMenuStrip menu)
        {
            this.m_menu = menu;
        }

        public abstract void SetData(IGameFileDataSource gameFile);
        protected void SetData(DataGridView dgvMain, IGameFileDataSource gameFile)
        {
            if ((this.GameFile != null) && this.GameFile.GameFileID.HasValue)
            {
                IEnumerable<IFileDataSource> files = this.DataSourceAdapter.GetFiles(gameFile, this.FileType);
                List<ISourcePortDataSource> sourcePortsData = DoomLauncher.Util.GetSourcePortsData(this.DataSourceAdapter);
                var source = from file in files
                    join sp in sourcePortsData on file.SourcePortID equals sp.SourcePortID
                    select new { 
                        Description = file.Description,
                        DateCreated = file.DateCreated,
                        SourcePortName = sp.Name,
                        FileDataSource = file
                    };
                dgvMain.DataSource = source.ToList();
                dgvMain.ContextMenuStrip = this.m_menu;
                this.m_files = files.ToArray<IFileDataSource>();
            }
            else
            {
                dgvMain.DataSource = null;
                this.m_files = new IFileDataSource[0];
            }
        }

        public bool SetFileOrderFirst()
        {
            List<IFileDataSource> selectedFiles = this.GetSelectedFiles();
            if (selectedFiles.Count > 0)
            {
                IFileDataSource item = selectedFiles.First<IFileDataSource>();
                if (!item.IsUrl)
                {
                    List<IFileDataSource> files = this.Files.ToList<IFileDataSource>();
                    files.Remove(item);
                    files.Insert(0, item);
                    this.SetFilePriorities(files);
                    foreach (IFileDataSource source2 in files)
                    {
                        this.DataSourceAdapter.UpdateFile(source2);
                    }
                    return true;
                }
            }
            return false;
        }

        private void SetFilePriorities(List<IFileDataSource> files)
        {
            int num = 0;
            using (List<IFileDataSource>.Enumerator enumerator = files.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.FileOrder = num++;
                }
            }
        }

        private bool SetFilePriority(bool up)
        {
            List<IFileDataSource> selectedFiles = this.GetSelectedFiles();
            if (selectedFiles.Count > 0)
            {
                IFileDataSource item = selectedFiles.First<IFileDataSource>();
                if (!item.IsUrl)
                {
                    List<IFileDataSource> files = this.Files.ToList<IFileDataSource>();
                    this.SetFilePriorities(files);
                    if (up && (item.FileOrder > 0))
                    {
                        item.FileOrder--;
                    }
                    if (!up && (item.FileOrder < files.Count))
                    {
                        item.FileOrder++;
                    }
                    files.Remove(item);
                    files.Insert(item.FileOrder, item);
                    this.SetFilePriorities(files);
                    foreach (IFileDataSource source2 in files)
                    {
                        this.DataSourceAdapter.UpdateFile(source2);
                    }
                    return true;
                }
            }
            return false;
        }

        private void ShowSourcePortError()
        {
            MessageBox.Show(this, "A source port must be selected.", "Error", MessageBoxButtons.OK);
        }

        public void View()
        {
            List<IFileDataSource> selectedFiles = this.GetSelectedFiles();
            if (selectedFiles.Count > 0)
            {
                IFileDataSource source = selectedFiles.First<IFileDataSource>();
                if (source.IsUrl)
                {
                    Process.Start(source.FileName);
                }
                else if (File.Exists(Path.Combine(this.DataDirectory.GetFullPath(), source.FileName)))
                {
                    Process.Start(Path.Combine(this.DataDirectory.GetFullPath(), source.FileName));
                }
            }
        }

        public virtual bool ChangeOrderAllowed =>
            true;

        public virtual bool CopyAllowed =>
            true;

        public LauncherPath DataDirectory { get; set; }

        public IDataSourceAdapter DataSourceAdapter { get; set; }

        public virtual bool DeleteAllowed =>
            true;

        public virtual bool EditAllowed =>
            true;

        protected virtual IFileDataSource[] Files =>
            this.m_files;

        public DoomLauncher.FileType FileType { get; set; }

        public IGameFileDataSource GameFile { get; set; }

        public virtual bool NewAllowed =>
            true;

        public virtual bool ViewAllowed =>
            true;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly BasicFileView.<>c <>9 = new BasicFileView.<>c();
            public static Func<IFileDataSource, int> <>9__3_0;
            public static Func<ISourcePortDataSource, int> <>9__3_1;
            public static Func<IFileDataSource, ISourcePortDataSource, <>f__AnonymousType0<string, DateTime, string, IFileDataSource>> <>9__3_2;

            internal int <SetData>b__3_0(IFileDataSource file) => 
                file.SourcePortID;

            internal int <SetData>b__3_1(ISourcePortDataSource sp) => 
                sp.SourcePortID;

            internal <>f__AnonymousType0<string, DateTime, string, IFileDataSource> <SetData>b__3_2(IFileDataSource file, ISourcePortDataSource sp) => 
                new { 
                    Description = file.Description,
                    DateCreated = file.DateCreated,
                    SourcePortName = sp.Name,
                    FileDataSource = file
                };
        }
    }
}

