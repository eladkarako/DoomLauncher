namespace DoomLauncher
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AdditionalFilesEventArgs : EventArgs
    {
        public AdditionalFilesEventArgs(object item)
        {
            this.Item = item;
            this.Cancel = false;
        }

        public AdditionalFilesEventArgs(object item, string displayText)
        {
            this.Item = item;
            this.DisplayText = displayText;
            this.Cancel = false;
        }

        public bool Cancel { get; set; }

        public string DisplayText { get; set; }

        public object Item { get; set; }

        public List<object> NewItems { get; set; }
    }
}

