namespace DoomLauncher
{
    using DoomLauncher.DataSources;
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using WadReader;

    internal static class Util
    {
        public static bool ChangeType(string obj, System.Type t, out object convertedObj)
        {
            convertedObj = null;
            if (obj != null)
            {
                if ((obj.GetType() == typeof(string)) && (t == typeof(string)))
                {
                    convertedObj = obj;
                    return true;
                }
                if (((obj.GetType() == typeof(string)) && (t == typeof(bool))) && ((obj == "0") || (obj == "1")))
                {
                    if (obj == "0")
                    {
                        convertedObj = false;
                    }
                    else
                    {
                        convertedObj = true;
                    }
                    return true;
                }
                System.Type[] types = new System.Type[] { typeof(string), System.Type.GetType($"{t.FullName}&") };
                MethodInfo method = t.GetMethod("TryParse", types);
                if (method != null)
                {
                    object[] parameters = new object[] { obj, convertedObj };
                    if ((bool) method.Invoke(null, parameters))
                    {
                        convertedObj = parameters[1];
                        return true;
                    }
                }
            }
            return false;
        }

        public static void DisplayUnexpectedException(Form form, Exception ex)
        {
            MessageBox.Show(form, $"Unexpected exception:

{ex.Message}

{ex.StackTrace}", "Unexpected Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        public static object FillObject(System.Type destType, object source)
        {
            object obj2 = Activator.CreateInstance(destType);
            foreach (PropertyInfo info in (from pSource in source.GetType().GetProperties()
                where ((pSource.GetSetMethod() != null) && (pSource.GetGetMethod() != null)) && (destType.GetProperty(pSource.Name) != null)
                select pSource).ToArray<PropertyInfo>())
            {
                PropertyInfo property = destType.GetProperty(info.Name);
                if (((property != null) && (property.GetSetMethod() != null)) && (property.GetGetMethod() != null))
                {
                    property.SetValue(obj2, info.GetValue(source, null), null);
                }
            }
            return obj2;
        }

        public static List<IGameFileDataSource> GetAdditionalFiles(IDataSourceAdapter adapter, IGameFileDataSource gameFile)
        {
            if (gameFile != null)
            {
                return GetAdditionalFiles(adapter, gameFile.SettingsFiles);
            }
            return new List<IGameFileDataSource>();
        }

        public static List<IGameFileDataSource> GetAdditionalFiles(IDataSourceAdapter adapter, ISourcePortDataSource sourcePort) => 
            GetAdditionalFiles(adapter, sourcePort.SettingsFiles);

        private static List<IGameFileDataSource> GetAdditionalFiles(IDataSourceAdapter adapter, string property)
        {
            char[] separator = new char[] { ';' };
            List<IGameFileDataSource> gameFiles = new List<IGameFileDataSource>();
            Array.ForEach<string>(property.Split(separator, StringSplitOptions.RemoveEmptyEntries), delegate (string x) {
                gameFiles.Add(adapter.GetGameFile(x));
            });
            return (from x in gameFiles
                where x > null
                select x).ToList<IGameFileDataSource>();
        }

        public static string GetMapStringFromWad(string file)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                FileStream fs = File.OpenRead(file);
                WadFileReader reader = new WadFileReader(fs);
                if (reader.WadType != WadType.Unknown)
                {
                    fs.Close();
                    foreach (FileLump lump in from lump in WadFileReader.GetMapMarkerLumps(reader.ReadLumps())
                        select lump into x
                        orderby x.Name
                        select x)
                    {
                        builder.Append(lump.Name);
                        builder.Append(", ");
                    }
                }
                else
                {
                    fs.Close();
                }
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return builder.ToString();
        }

        public static string[] GetSkills() => 
            new string[] { "1", "2", "3", "4", "5" };

        public static List<ISourcePortDataSource> GetSourcePortsData(IDataSourceAdapter adapter)
        {
            SourcePortDataSource item = new SourcePortDataSource {
                Name = "N/A",
                SourcePortID = -1
            };
            List<ISourcePortDataSource> list1 = adapter.GetSourcePorts().ToList<ISourcePortDataSource>();
            list1.Insert(0, item);
            return list1;
        }

        public static string GetTimePlayedString(int minutes)
        {
            string str = "Time Played: ";
            if (minutes < 60)
            {
                return (str + $"{minutes} minute{((minutes == 1) ? string.Empty : "s")}");
            }
            double num = Math.Round((double) (((double) minutes) / 60.0), 2);
            return (str + $"{num.ToString("N", CultureInfo.InvariantCulture)} hour{((num == 1.0) ? string.Empty : "s")}");
        }

        public static GameFileSearchField[] SearchFieldsFromSearchCtrl(SearchControl ctrlSearch)
        {
            List<GameFileSearchField> list = new List<GameFileSearchField>();
            string[] selectedSearchFilters = ctrlSearch.GetSelectedSearchFilters();
            for (int i = 0; i < selectedSearchFilters.Length; i++)
            {
                GameFileFieldType type;
                if (Enum.TryParse<GameFileFieldType>(selectedSearchFilters[i], out type))
                {
                    list.Add(new GameFileSearchField(type, GameFileSearchOp.Like, ctrlSearch.SearchText));
                }
            }
            return list.ToArray();
        }

        public static void SetDefaultSearchFields(SearchControl ctrlSearch)
        {
            string[] items = new string[] { "Title", "Author", "Filename", "Description" };
            ctrlSearch.SetSearchFilters(items);
            ctrlSearch.SetSearchFilter(items[0], true);
            ctrlSearch.SetSearchFilter(items[1], true);
            ctrlSearch.SetSearchFilter(items[2], true);
        }

        public static IEnumerable<object> TableToStructure(DataTable dt, System.Type type)
        {
            List<object> list = new List<object>();
            PropertyInfo[] infoArray = (from p in type.GetProperties()
                where (p.GetSetMethod() != null) && (p.GetGetMethod() != null)
                select p).ToArray<PropertyInfo>();
            foreach (DataRow row in dt.Rows)
            {
                object obj3 = Activator.CreateInstance(type);
                foreach (PropertyInfo info in infoArray)
                {
                    object obj2;
                    System.Type propertyType = info.PropertyType;
                    if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                    }
                    if (dt.Columns.Contains(info.Name) && ChangeType(row[info.Name].ToString(), propertyType, out obj2))
                    {
                        info.SetValue(obj3, obj2, null);
                    }
                }
                list.Add(obj3);
            }
            return list;
        }

        [Conditional("DEBUG")]
        public static void ThrowDebugException(string msg)
        {
            throw new Exception(msg);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly DoomLauncher.Util.<>c <>9 = new DoomLauncher.Util.<>c();
            public static Func<PropertyInfo, bool> <>9__0_0;
            public static Func<IGameFileDataSource, bool> <>9__12_1;
            public static Func<FileLump, FileLump> <>9__2_0;
            public static Func<FileLump, string> <>9__2_1;

            internal bool <GetAdditionalFiles>b__12_1(IGameFileDataSource x) => 
                (x > null);

            internal FileLump <GetMapStringFromWad>b__2_0(FileLump lump) => 
                lump;

            internal string <GetMapStringFromWad>b__2_1(FileLump x) => 
                x.Name;

            internal bool <TableToStructure>b__0_0(PropertyInfo p) => 
                ((p.GetSetMethod() != null) && (p.GetGetMethod() != null));
        }
    }
}

