namespace DoomLauncher.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static Bitmap bon2a =>
            ((Bitmap) ResourceManager.GetObject("bon2a", resourceCulture));

        internal static Bitmap bon2b =>
            ((Bitmap) ResourceManager.GetObject("bon2b", resourceCulture));

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get => 
                resourceCulture;
            set
            {
                resourceCulture = value;
            }
        }

        internal static Bitmap kill =>
            ((Bitmap) ResourceManager.GetObject("kill", resourceCulture));

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    resourceMan = new System.Resources.ResourceManager("DoomLauncher.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }

        internal static Bitmap secret =>
            ((Bitmap) ResourceManager.GetObject("secret", resourceCulture));

        internal static Bitmap th =>
            ((Bitmap) ResourceManager.GetObject("th", resourceCulture));
    }
}

