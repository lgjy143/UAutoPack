using SharpCompress.Archive;
using SharpCompress.Archive.Zip;
using SharpCompress.Common;
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
using System.Windows.Forms;
using System.Xml.Linq;

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
            CallPS1();

            //var val1 = DicValConvert<string>(dic, "1");
            //MessageBox.Show("val1:" + val1);
            //var val = DicValConvert<List<string>>(dic, "2", "[]");
            //MessageBox.Show("val:" + string.Join(",", val.ToArray()));
            //var val4 = DicValConvert<string>(dic, "4");
            //MessageBox.Show("val4:" + val4);
        }

        private static string script = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"/ps1/sum.ps1");

        public static string sln = @"E:\workspace\vssWork\Source\EAS_ShoeERP\EAS_ShoeERP.sln";
        //public static string sln = @"F:\WorkDemo\WpfAutoPack\WpfAutoPack.sln";
        public static string _PackageTempDir = @"C:\Users\U724\Desktop\dev\artifact";

        private void CallPS1()
        {
            //Console.WriteLine(DateTime.Now.ToString("dddd"));//星期几

            #region GetVersion

            var version = GetVersion();
            Console.WriteLine("version:" + version.ToString());

            #endregion

            var richText = new Utils.RichTextBoxUtils(this.rtbInfo);
            richText.Write("version:" + version.ToString());

            var outputZip = @"C:\Users\U724\Desktop\dev\" + version + ".zip";
            CompressionPubilcZip(outputZip);
            Console.WriteLine("发布文件打包完成.");

            richText.Write("发布文件打包完成.");

            return;

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
                    Console.WriteLine("CallPS1:" + result);
                    //MessageBox.Show(result.ToString());
                }
                ps.AddCommand("GetMsBuildPath").AddParameters(new List<bool>() { { true } });

                foreach (PSObject result in ps.Invoke())
                {
                    Console.WriteLine(result.ToString());
                    //MessageBox.Show(result.ToString());
                }

                ps.AddCommand("BuildSln").AddParameters(new List<string>() { { sln } });
                Console.WriteLine("Start Build ...");
                foreach (PSObject result in ps.Invoke())
                {
                    Console.WriteLine(result.ToString());
                    //MessageBox.Show(result.ToString());
                }
                Console.WriteLine("Build to Completed");
                DeleteConfigFile();
                Console.WriteLine("DeleteConfigFile to Completed");

            }
        }

        private static void DeleteConfigFile()
        {
            var listFile = new List<string> { { "Web.Config" }, { "Log4Net.Config" } };
            if (Directory.Exists(_PackageTempDir))
            {
                listFile.ForEach(item =>
                {
                    var filePath = _PackageTempDir + @"\" + item;
                    if (File.Exists(filePath)) { File.Delete(filePath); }
                });
            }
        }

        private static string GetVersion()
        {
            var version = DateTime.Now.ToString("yyyymmddhhmmss");

            XElement doc = XElement.Load(_PackageTempDir + @"\Config\SystemConfig.config");
            var _version = string.Empty;
            try
            {
                var listVersion = from ele in doc.Descendants("SoftVersion")
                                  select new
                                  {
                                      Version = ele.Value
                                  };

                if (listVersion != null)
                {
                    _version = listVersion.FirstOrDefault().Version;
                }
            }
            catch (Exception) { }
            if (!string.IsNullOrEmpty(_version))
            {
                version = "V" + _version;
            }
            return version;
        }

        private static void CompressionPubilcZip(string outputZip)
        {
            if (File.Exists(outputZip)) { File.Delete(outputZip); }

            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(_PackageTempDir);
                archive.SaveTo(outputZip, CompressionType.Deflate);
            }
        }

        public delegate void ConsoleOutPutDelegate(string output);

        public void ConsoleOutPutLog(string output)
        {
            rtbInfo.AppendText(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] : ") + output + Environment.NewLine);
        }

        public void ConsoleOutPut(string output)
        {
            //rtbInfo.BeginInvoke(new TaskConsoleOutPut(rtbInfo, DateTime.Now.ToLongDateString() + " : " + output + Environment.NewLine));

            //this.BeginInvoke(new TaskConsoleOutPut(ConsoleOutPut), new object[] { DateTime.Now.ToLongDateString() + " : " + output + Environment.NewLine });

            //rtbInfo.AppendText(DateTime.Now.ToLongDateString() + " : " + output + Environment.NewLine);

            //rtbInfo.Text += DateTime.Now.ToLongDateString() + " : " + output + Environment.NewLine;

            //var dlg = new ConsoleOutPutDelegate(ConsoleOutPutLog);
            //rtbInfo.Invoke(dlg, output);

            rtbInfo.Invoke(new ConsoleOutPutDelegate(ConsoleOutPutLog), output);

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

        private void btnSln_Click(object sender, EventArgs e)
        {
            ThreadExecute(this.bgWork, new DoWorkEventHandler(this.DoWork), new RunWorkerCompletedEventHandler(this.WorkComp));
        }

        public void SetText(string str)
        {
            rtbInfo.Invoke(new Action(() =>
            {
                rtbInfo.AppendText(str);
                rtbInfo.ScrollToCaret();
            }));
        }

        protected void DoWork(object sender, DoWorkEventArgs e)
        {
            var richText = new Utils.RichTextBoxUtils(this.rtbInfo);
            richText.Write("1");
            richText.Write("2");
            //System.Threading.Thread.Sleep(1000);
            richText.Write("1");
            richText.Write("2");
            //System.Threading.Thread.Sleep(1000);
            richText.Write("11");
            //System.Threading.Thread.Sleep(1000);
            richText.Write("1");
            richText.Write("2");
            //System.Threading.Thread.Sleep(1000);

            //SetText("123 ");
            //SetText("123-1 ");
            //System.Threading.Thread.Sleep(1000);

            //SetText("123 ");
            //SetText("123-1 ");
            //System.Threading.Thread.Sleep(1000);

            //SetText("123 ");
            //SetText("123-1 ");
            //System.Threading.Thread.Sleep(1000);

            //SetText("123 ");
            //SetText("123-1 ");
            //System.Threading.Thread.Sleep(1000);
            //SetText("123");

            //LogMessage("绿色1");
            //LogError("红色2");
            //LogWarning("粉色3");
            ////System.Threading.Thread.Sleep(1000);
            //LogMessage("绿色4");
            //LogError("红色5");
            //LogWarning("粉色6");
            ////System.Threading.Thread.Sleep(1000);
            //LogMessage("绿色7");
            //LogError("红色8");
            //LogWarning("粉色9");
            ////System.Threading.Thread.Sleep(1000);

        }

        protected void WorkComp(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        public static void ThreadExecute(BackgroundWorker bgWorker, DoWorkEventHandler doWork, RunWorkerCompletedEventHandler doWorkComplete)
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += doWork;
            bgWorker.RunWorkerCompleted += doWorkComplete;
            if (!bgWorker.IsBusy)
            {
                bgWorker.RunWorkerAsync();
            }
        }


        #region 日志记录、支持其他线程访问  

        public delegate void LogAppendDelegate(Color color, string text);

        public void LogAppendMethod(Color color, string text)
        {
            rtbInfo.AppendText("\n");
            rtbInfo.SelectionColor = color;
            rtbInfo.AppendText(text);
            rtbInfo.ScrollToCaret();
        }

        public void LogError(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppendMethod);
            rtbInfo.Invoke(la, Color.Red, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
        }
        public void LogWarning(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppendMethod);
            rtbInfo.Invoke(la, Color.Violet, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
        }
        public void LogMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppendMethod);
            rtbInfo.Invoke(la, Color.Green, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
        }

        #endregion

    }
}
