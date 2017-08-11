namespace DoomLauncher.TextFileParsers
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    internal class IdGamesTextFileParser : ITextFileParser
    {
        private static string s_fullRegex = @"\s*{0}\s*:[^\r\n]*";
        private static string s_fullRegexDescription = @"\s*{0}\s*:[^=]*";
        private static string s_regex = @"\s*{0}\s*:\s*";

        public IdGamesTextFileParser(string text, string[] dateParseFormats)
        {
            this.Title = this.FindValue(text, "Title", s_fullRegex, false);
            this.Author = this.FindValue(text, "Authors*", s_fullRegex, false);
            this.Description = this.FindValue(text, "Description", s_fullRegexDescription, false).Replace("\r\n", "\n");
            string[] textArray1 = new string[] { "Date Finished", "Release date" };
            string str = string.Empty;
            foreach (string str2 in textArray1)
            {
                str = this.FindValue(text, str2, s_fullRegex, true).Replace("th", string.Empty).Trim();
                if (!string.IsNullOrEmpty(str))
                {
                    break;
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                DateTime time;
                str = str.Replace(".", "/");
                if (this.ParseDate1(str, dateParseFormats, out time))
                {
                    this.ReleaseDate = new DateTime?(time);
                }
                else if (this.ParseDate2(str, dateParseFormats, out time))
                {
                    this.ReleaseDate = new DateTime?(time);
                }
                else if (this.ParseDate3(str, dateParseFormats, out time))
                {
                    this.ReleaseDate = new DateTime?(time);
                }
            }
        }

        private string FindValue(string text, string category, string regexFull, bool ignoreCase)
        {
            Match match = new Regex(string.Format(regexFull, category)).Match(text);
            if (match.Success)
            {
                return match.Value.Substring(new Regex(string.Format(s_regex, category)).Match(match.Value).Value.Length);
            }
            return string.Empty;
        }

        private bool ParseDate1(string date, string[] dateParseFormats, out DateTime dt) => 
            (DateTime.TryParse(date, out dt) || DateTime.TryParseExact(date, dateParseFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt));

        private bool ParseDate2(string date, string[] dateParseFormats, out DateTime dt)
        {
            if ((date.Length > 0) && !char.IsNumber(date.Last<char>()))
            {
                date = date.Substring(0, date.Length - 1);
            }
            if ((date.Length > 0) && !char.IsNumber(date.First<char>()))
            {
                date = date.Substring(1);
            }
            return this.ParseDate1(date, dateParseFormats, out dt);
        }

        private bool ParseDate3(string date, string[] dateParseFormats, out DateTime dt)
        {
            while (!char.IsNumber(date.Last<char>()))
            {
                date = date.Substring(0, date.Length - 1);
            }
            return this.ParseDate1(date, dateParseFormats, out dt);
        }

        public string Author { get; set; }

        public string Description { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string Title { get; set; }
    }
}

