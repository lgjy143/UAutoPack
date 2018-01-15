namespace UAutoPack
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtSln = new System.Windows.Forms.TextBox();
            this.btnSln = new System.Windows.Forms.Button();
            this.btnPackDic = new System.Windows.Forms.Button();
            this.txtPackDic = new System.Windows.Forms.TextBox();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(410, 70);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtSln
            // 
            this.txtSln.Location = new System.Drawing.Point(12, 13);
            this.txtSln.Name = "txtSln";
            this.txtSln.Size = new System.Drawing.Size(374, 21);
            this.txtSln.TabIndex = 1;
            // 
            // btnSln
            // 
            this.btnSln.Location = new System.Drawing.Point(410, 13);
            this.btnSln.Name = "btnSln";
            this.btnSln.Size = new System.Drawing.Size(75, 23);
            this.btnSln.TabIndex = 2;
            this.btnSln.Text = "解决方案";
            this.btnSln.UseVisualStyleBackColor = true;
            this.btnSln.Click += new System.EventHandler(this.btnSln_Click);
            // 
            // btnPackDic
            // 
            this.btnPackDic.Location = new System.Drawing.Point(409, 43);
            this.btnPackDic.Name = "btnPackDic";
            this.btnPackDic.Size = new System.Drawing.Size(75, 23);
            this.btnPackDic.TabIndex = 4;
            this.btnPackDic.Text = "输出目录";
            this.btnPackDic.UseVisualStyleBackColor = true;
            // 
            // txtPackDic
            // 
            this.txtPackDic.Location = new System.Drawing.Point(12, 43);
            this.txtPackDic.Name = "txtPackDic";
            this.txtPackDic.Size = new System.Drawing.Size(373, 21);
            this.txtPackDic.TabIndex = 3;
            // 
            // rtbInfo
            // 
            this.rtbInfo.Location = new System.Drawing.Point(12, 70);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.Size = new System.Drawing.Size(373, 92);
            this.rtbInfo.TabIndex = 6;
            this.rtbInfo.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 174);
            this.Controls.Add(this.rtbInfo);
            this.Controls.Add(this.btnPackDic);
            this.Controls.Add(this.txtPackDic);
            this.Controls.Add(this.btnSln);
            this.Controls.Add(this.txtSln);
            this.Controls.Add(this.btnCreate);
            this.Name = "Form1";
            this.Text = "发布打包";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtSln;
        private System.Windows.Forms.Button btnSln;
        private System.Windows.Forms.Button btnPackDic;
        private System.Windows.Forms.TextBox txtPackDic;
        private System.Windows.Forms.RichTextBox rtbInfo;
    }
}

