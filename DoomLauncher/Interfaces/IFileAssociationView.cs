namespace DoomLauncher.Interfaces
{
    using System;
    using System.Windows.Forms;

    internal interface IFileAssociationView
    {
        void CopyAllToClipboard();
        void CopyToClipboard();
        bool Delete();
        bool Edit();
        bool MoveFileOrderDown();
        bool MoveFileOrderUp();
        bool New();
        void SetContextMenu(ContextMenuStrip menu);
        void SetData(IGameFileDataSource gameFile);
        bool SetFileOrderFirst();
        void View();

        bool ChangeOrderAllowed { get; }

        bool CopyAllowed { get; }

        bool DeleteAllowed { get; }

        bool EditAllowed { get; }

        IGameFileDataSource GameFile { get; set; }

        bool NewAllowed { get; }

        bool ViewAllowed { get; }
    }
}

