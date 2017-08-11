namespace DoomLauncher
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Timers;

    internal class StatFileScanner
    {
        private System.Timers.Timer m_checkTimer;
        protected List<string> m_errors = new List<string>();
        private FileInfo m_fileInfo;
        private string m_statFile;

        [field: CompilerGenerated]
        public event EventHandler StatFileChanged;

        public StatFileScanner(string statFile)
        {
            this.m_statFile = statFile;
            this.m_fileInfo = new FileInfo(this.m_statFile);
        }

        private bool FileChanged()
        {
            FileInfo info = new FileInfo(this.m_statFile);
            return ((info.Exists && !this.m_fileInfo.Exists) || (info.LastWriteTime != this.m_fileInfo.LastWriteTime));
        }

        private void m_checkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.m_checkTimer.Stop();
            if (this.FileChanged())
            {
                this.m_fileInfo = new FileInfo(this.m_statFile);
                if (this.StatFileChanged != null)
                {
                    this.StatFileChanged(this, new EventArgs());
                }
            }
            this.m_checkTimer.Start();
        }

        protected void SetStatProperty(StatsDataSource stats, ParseItem item, string value)
        {
            foreach (char ch in item.Replace)
            {
                value = value.Replace(ch.ToString(), string.Empty);
            }
            PropertyInfo property = stats.GetType().GetProperty(item.DataSourceProperty);
            if (item.DataSourceProperty == "LevelTime")
            {
                char[] separator = new char[] { ':' };
                string[] strArray = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                property.SetValue(stats, (Convert.ToSingle(strArray[0]) * 60f) + Convert.ToSingle(strArray[1]));
            }
            else
            {
                try
                {
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(stats, value);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(stats, Convert.ToInt32(value));
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        property.SetValue(stats, Convert.ToSingle(value));
                    }
                }
                catch
                {
                    this.m_errors.Add($"Failed for parse value[{value}] for [{item.DataSourceProperty}]");
                }
            }
        }

        public void StartScanning()
        {
            if (this.m_checkTimer == null)
            {
                this.m_fileInfo = new FileInfo(this.m_statFile);
                this.m_checkTimer = new System.Timers.Timer(1000.0);
                this.m_checkTimer.Elapsed += new ElapsedEventHandler(this.m_checkTimer_Elapsed);
                this.m_checkTimer.Start();
            }
        }

        public void StopScanning()
        {
            if (this.m_checkTimer != null)
            {
                this.m_checkTimer.Stop();
            }
        }

        public string StatFile =>
            this.m_statFile;

        protected class ParseItem
        {
            public ParseItem(string regexInput, string replace, string dataSourceProperty)
            {
                this.RegexInput = regexInput;
                this.Replace = replace;
                this.DataSourceProperty = dataSourceProperty;
            }

            public string DataSourceProperty { get; set; }

            public string RegexInput { get; set; }

            public string Replace { get; set; }
        }
    }
}

