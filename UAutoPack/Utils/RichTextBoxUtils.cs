using System;
using System.Windows.Forms;

namespace UAutoPack.Utils
{
    /// <summary>
    /// RichTextBox实时输出
    /// </summary>
    public class RichTextBoxUtils
    {
        /// <summary>
        ///  var richText = new UAutoPack.Utils.RichTextBoxUtils(this.rtbInfo);
        ///  richText.Write("1");
        /// </summary>
        /// <param name="richText"></param>
        public RichTextBoxUtils(RichTextBox richText)
        {
            RichText = richText;
        }
        /// <summary>
        /// RichTextBox控件
        /// </summary>
        private RichTextBox RichText { get; set; }
        /// <summary>
        /// RichTextBox输出
        /// </summary>
        /// <param name="text"></param>

        public void Write(string text)
        {
            RichText.Invoke(new Action(() =>
            {
                RichText.AppendText(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + " : " + text + Environment.NewLine);
                RichText.ScrollToCaret();
            }));
        }
    }
}
