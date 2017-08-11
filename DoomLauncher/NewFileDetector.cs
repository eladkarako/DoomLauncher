namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal class NewFileDetector : INewFileDetector
    {
        public NewFileDetector(string[] extensions, string directory)
        {
            this.Directory = directory;
            this.Extenstions = extensions;
            this.ScanSubDirectories = false;
        }

        public NewFileDetector(string[] extensions, string directory, bool scanSubDirectories)
        {
            this.Directory = directory;
            this.Extenstions = extensions;
            this.ScanSubDirectories = scanSubDirectories;
        }

        private FileInfo[] GetFiles(string directory)
        {
            try
            {
                if (this.ScanSubDirectories)
                {
                    List<FileInfo> list = new List<FileInfo>();
                    list.AddRange(this.GetFilesBase(directory));
                    foreach (DirectoryInfo info in new DirectoryInfo(directory).GetDirectories())
                    {
                        list.AddRange(this.GetFiles(info.FullName));
                    }
                    return list.ToArray();
                }
                return this.GetFilesBase(directory);
            }
            catch
            {
                return new FileInfo[0];
            }
        }

        private FileInfo[] GetFilesBase(string directory)
        {
            try
            {
                return (from item in new DirectoryInfo(directory).GetFiles()
                    where this.Extenstions.Contains<string>(item.Extension)
                    select item).ToArray<FileInfo>();
            }
            catch
            {
                return new FileInfo[0];
            }
        }

        public string[] GetModifiedFiles()
        {
            if ((this.BaseFiles != null) && (this.Directory != null))
            {
                FileInfo[] files = this.GetFiles(this.Directory);
                return (from file in this.BaseFiles
                    join currentFile in files on file.FullName equals currentFile.FullName into CurrentFile
                    where file.LastWriteTime != CurrentFile.LastWriteTime
                    select CurrentFile.FullName).ToArray<string>();
            }
            return new string[0];
        }

        public string[] GetNewFiles()
        {
            if ((this.BaseFiles != null) && (this.Directory != null))
            {
                string[] second = (from x in this.BaseFiles select x.FullName).ToArray<string>();
                return (from x in this.GetFiles(this.Directory) select x.FullName).ToArray<string>().Except<string>(second).ToArray<string>();
            }
            return new string[0];
        }

        public void StartDetection()
        {
            this.BaseFiles = this.GetFiles(this.Directory);
        }

        private FileInfo[] BaseFiles { get; set; }

        public string Directory { get; set; }

        public string[] Extenstions { get; set; }

        public bool ScanSubDirectories { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly NewFileDetector.<>c <>9 = new NewFileDetector.<>c();
            public static Func<FileInfo, string> <>9__3_0;
            public static Func<FileInfo, string> <>9__3_1;
            public static Func<FileInfo, string> <>9__4_0;
            public static Func<FileInfo, string> <>9__4_1;
            public static Func<FileInfo, FileInfo, <>f__AnonymousType1<FileInfo, FileInfo>> <>9__4_2;
            public static Func<<>f__AnonymousType1<FileInfo, FileInfo>, bool> <>9__4_3;
            public static Func<<>f__AnonymousType1<FileInfo, FileInfo>, string> <>9__4_4;

            internal string <GetModifiedFiles>b__4_0(FileInfo file) => 
                file.FullName;

            internal string <GetModifiedFiles>b__4_1(FileInfo currentFile) => 
                currentFile.FullName;

            internal <>f__AnonymousType1<FileInfo, FileInfo> <GetModifiedFiles>b__4_2(FileInfo file, FileInfo currentFile) => 
                new { 
                    OriginalFile = file,
                    CurrentFile = currentFile
                };

            internal bool <GetModifiedFiles>b__4_3(<>f__AnonymousType1<FileInfo, FileInfo> file) => 
                (file.OriginalFile.LastWriteTime != file.CurrentFile.LastWriteTime);

            internal string <GetModifiedFiles>b__4_4(<>f__AnonymousType1<FileInfo, FileInfo> file) => 
                file.CurrentFile.FullName;

            internal string <GetNewFiles>b__3_0(FileInfo x) => 
                x.FullName;

            internal string <GetNewFiles>b__3_1(FileInfo x) => 
                x.FullName;
        }
    }
}

