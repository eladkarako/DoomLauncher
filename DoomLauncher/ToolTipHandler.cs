namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    internal class ToolTipHandler
    {
        private string FormatDescritionToWidth(Font font, string description, int maxWidth, int maxLines)
        {
            char[] separator = new char[] { '\n' };
            StringBuilder builder = new StringBuilder();
            int num = 0;
            foreach (string str in description.Split(separator))
            {
                if (string.IsNullOrEmpty(str))
                {
                    builder.Append(Environment.NewLine);
                    continue;
                }
                string str2 = str;
                int width = 0;
                if (!string.IsNullOrEmpty(str2))
                {
                    width = TextRenderer.MeasureText(str2, font).Width;
                }
                while (width > maxWidth)
                {
                    string oldValue = this.TruncateLine(font, str2, maxWidth);
                    builder.Append(oldValue.TrimStart(new char[0]));
                    builder.Append(Environment.NewLine);
                    if (++num > maxLines)
                    {
                        break;
                    }
                    str2 = str2.Replace(oldValue, string.Empty);
                    width = TextRenderer.MeasureText(str2, font).Width;
                }
                if (num < maxLines)
                {
                    builder.Append(str2.TrimStart(new char[0]));
                    builder.Append(Environment.NewLine);
                }
                if (++num > maxLines)
                {
                    break;
                }
            }
            return builder.ToString();
        }

        public string GetToolTipText(Font font, IGameFileDataSource item)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Title: ");
            if (item.Title != null)
            {
                builder.Append(item.Title);
            }
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("Author: ");
            if (item.Author != null)
            {
                builder.Append(item.Author);
            }
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("Release Date: ");
            if (item.ReleaseDate.HasValue)
            {
                builder.Append(item.ReleaseDate.Value.ToShortDateString());
            }
            else
            {
                builder.Append("N/A");
            }
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("Description: ");
            try
            {
                builder.Append(this.FormatDescritionToWidth(font, item.Description.Replace("\r\n", "\n"), 640, 20));
            }
            catch
            {
                builder.Append(item.Description.Replace("\r\n", "\n"));
            }
            return builder.ToString();
        }

        private string TruncateLine(Font font, string line, int maxWidth)
        {
            for (int i = 1; i < line.Length; i++)
            {
                if (TextRenderer.MeasureText(line.Substring(0, i), font).Width > maxWidth)
                {
                    while ((i > 0) && (line[i] != ' '))
                    {
                        i--;
                    }
                    return line.Substring(0, i);
                }
            }
            return line;
        }
    }
}

