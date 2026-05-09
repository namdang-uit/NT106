namespace DrawingServer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.lblOnlineCount = new System.Windows.Forms.Label();
            this.lbClients = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rtbLogs
            // 
            this.rtbLogs.Location = new System.Drawing.Point(62, 38);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.Size = new System.Drawing.Size(532, 584);
            this.rtbLogs.TabIndex = 0;
            this.rtbLogs.Text = "";
            // 
            // lblOnlineCount
            // 
            this.lblOnlineCount.AutoSize = true;
            this.lblOnlineCount.Location = new System.Drawing.Point(632, 41);
            this.lblOnlineCount.Name = "lblOnlineCount";
            this.lblOnlineCount.Size = new System.Drawing.Size(51, 20);
            this.lblOnlineCount.TabIndex = 1;
            this.lblOnlineCount.Text = "label1";
            // 
            // lbClients
            // 
            this.lbClients.FormattingEnabled = true;
            this.lbClients.ItemHeight = 20;
            this.lbClients.Location = new System.Drawing.Point(636, 208);
            this.lbClients.Name = "lbClients";
            this.lbClients.Size = new System.Drawing.Size(325, 304);
            this.lbClients.TabIndex = 2;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(779, 63);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(114, 44);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "openserver";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(965, 77);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(150, 26);
            this.txtPort.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 651);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lbClients);
            this.Controls.Add(this.lblOnlineCount);
            this.Controls.Add(this.rtbLogs);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.Label lblOnlineCount;
        private System.Windows.Forms.ListBox lbClients;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtPort;
    }
}

