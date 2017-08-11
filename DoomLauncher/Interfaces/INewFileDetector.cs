namespace DoomLauncher.Interfaces
{
    using System;

    public interface INewFileDetector
    {
        string[] GetModifiedFiles();
        string[] GetNewFiles();
        void StartDetection();
    }
}

