using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Windows.Forms;

namespace UAutoPack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);
            //MessageBox.Show(script);
            CallPS1();

            //var val1 = DicValConvert<string>(dic, "1");
            //MessageBox.Show("val1:" + val1);
            //var val = DicValConvert<List<string>>(dic, "2", "[]");
            //MessageBox.Show("val:" + string.Join(",", val.ToArray()));
            //var val4 = DicValConvert<string>(dic, "4");
            //MessageBox.Show("val4:" + val4);
        }

        private static string script = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"/ps1/sum.ps1");

        public static string sln = @"D:/work/WpfAutoPack/WpfAutoPack.sln";

        private static void CallPS1()
        {
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                PowerShell ps = PowerShell.Create();
                ps.Runspace = runspace;
                ps.AddScript(script);
                ps.Invoke();

                ps.AddCommand("Sum").AddParameters(
                    new Dictionary<string, int>()
                    {
                        {"first", 15},
                        {"second", 14},
                        {"three", 16}
                    }
                );

                foreach (PSObject result in ps.Invoke())
                {
                    Console.WriteLine("CallPS1()");
                    Console.WriteLine(result);
                    MessageBox.Show(result.ToString());
                }
                ps.AddCommand("GetMsBuildPath").AddParameters(new List<bool>() { { true } });

                foreach (PSObject result in ps.Invoke())
                {
                    Console.WriteLine(result.ToString());
                    MessageBox.Show(result.ToString());
                }
                

                ps.AddCommand("BuildSln").AddParameters(new List<string>() { { sln } });

                foreach (PSObject result in ps.Invoke())
                {
                    MessageBox.Show(result.ToString());
                }

            }
        }

        private static Dictionary<string, string> dic = new Dictionary<string, string>
        {
            {"1","1"},
            {"2","1[]2"},
            {"S","S1,S2"},
            {"4",""}
         };
        private static T DicValConvert<T>(Dictionary<string, string> dic, string keyStr, string split = ",")
        {
            string keyVal = string.Empty;
            if (!string.IsNullOrEmpty(dic[keyStr])) keyVal = dic[keyStr];
            if (typeof(IList).IsAssignableFrom(typeof(T)))
            {
                var listCur = keyVal.Split(split.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                return (T)ChangeType(listCur, typeof(T));
            }
            else
            {
                return (T)ChangeType(keyVal, typeof(T));
            }
        }

        static public object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
    }
}
