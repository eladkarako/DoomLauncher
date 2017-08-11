namespace DoomLauncher
{
    using System;
    using System.IO;

    public class LauncherPath
    {
        private string m_fullPath;
        private string m_path;

        public LauncherPath(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path = path + Path.DirectorySeparatorChar.ToString();
            }
            this.m_path = path;
            this.m_fullPath = this.m_path;
            if (!Path.IsPathRooted(this.m_fullPath))
            {
                this.m_fullPath = Path.Combine(Directory.GetCurrentDirectory(), this.m_fullPath);
            }
            else
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                if (this.m_path.StartsWith(currentDirectory))
                {
                    this.m_path = this.m_path.Replace(currentDirectory, string.Empty);
                }
            }
        }

        public string GetFullPath() => 
            this.m_fullPath;

        public string GetPossiblyRelativePath() => 
            this.m_path;

        public static implicit operator string(LauncherPath p) => 
            p.GetFullPath();
    }
}

