namespace DoomLauncher
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            string launchFile = null;
            if (args.Length != 0)
            {
                string[] strArray = ValidateLaunchFiles(args);
                if (strArray.Length != 0)
                {
                    launchFile = strArray[0];
                }
            }
            Directory.SetCurrentDirectory(AssemblyDirectory);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(launchFile));
        }

        private static string[] ValidateLaunchFiles(string[] files) => 
            (from item in files
                where File.Exists(item)
                select item).ToArray<string>();

        private static string AssemblyDirectory =>
            Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Program.<>c <>9 = new Program.<>c();
            public static Func<string, bool> <>9__1_0;

            internal bool <ValidateLaunchFiles>b__1_0(string item) => 
                File.Exists(item);
        }
    }
}

