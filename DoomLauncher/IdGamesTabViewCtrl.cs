namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class IdGamesTabViewCtrl : OptionsTabViewCtrl
    {
        private IContainer components;
        private bool m_working;

        public IdGamesTabViewCtrl(object key, string title, IGameFileDataSourceAdapter adapter, GameFileFieldType[] selectFields) : base(key, title, adapter, selectFields)
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
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
        }

        public override void SetGameFiles()
        {
            this.SetGameFiles(null);
        }

        public override void SetGameFiles(IEnumerable<GameFileSearchField> searchFields)
        {
            if (!this.m_working)
            {
                this.m_working = true;
                base.SetDisplayText("Searching...");
                BackgroundWorker worker1 = new BackgroundWorker();
                worker1.DoWork += new DoWorkEventHandler(this.UpdateIdGamesView_Worker);
                worker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.UpdateIdGamesViewCompleted);
                worker1.RunWorkerAsync(searchFields);
            }
        }

        private void UpdateIdGamesView_Worker(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.IdGamesDataSource = null;
                IEnumerable<GameFileSearchField> argument = e.Argument as IEnumerable<GameFileSearchField>;
                if ((argument == null) || (argument.Count<GameFileSearchField>() == 0))
                {
                    this.IdGamesDataSource = base.m_adapter.GetGameFiles();
                }
                else
                {
                    IEnumerable<IGameFileDataSource> first = new IGameFileDataSource[0];
                    foreach (GameFileSearchField field in argument)
                    {
                        first = first.Union<IGameFileDataSource>(base.m_adapter.GetGameFiles(new GameFileGetOptions(field)));
                    }
                    this.IdGamesDataSource = first;
                }
            }
            catch
            {
            }
        }

        private void UpdateIdGamesViewCompleted(object sender, EventArgs e)
        {
            this.m_working = false;
            if (this.IdGamesDataSource != null)
            {
                base.SetDataSource(this.IdGamesDataSource.ToList<IGameFileDataSource>());
            }
            else
            {
                base.SetDisplayText("Error retrieving data from id Games");
                MessageBox.Show(this, "There was an error retrieving data from id Games.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        protected override bool FilterIWads =>
            false;

        public IEnumerable<IGameFileDataSource> IdGamesDataSource { get; set; }

        public override bool IsAutoSearchAllowed =>
            false;

        public override bool IsDeleteAllowed =>
            false;

        public override bool IsEditAllowed =>
            false;

        public override bool IsLocal =>
            false;

        public override bool IsPlayAllowed =>
            false;

        public override bool IsSearchAllowed =>
            true;
    }
}

