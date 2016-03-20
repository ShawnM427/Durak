namespace DurakTester
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
            this.btnInitServer = new System.Windows.Forms.Button();
            this.rtbServerOutput = new System.Windows.Forms.RichTextBox();
            this.btnKillServer = new System.Windows.Forms.Button();
            this.grpClientControls = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.grpMessageParams = new System.Windows.Forms.GroupBox();
            this.cmbMessageType = new System.Windows.Forms.ComboBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.cmbFromClient = new System.Windows.Forms.ComboBox();
            this.lblFromClient = new System.Windows.Forms.Label();
            this.chkLogRules = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.grpClientControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInitServer
            // 
            this.btnInitServer.Location = new System.Drawing.Point(13, 13);
            this.btnInitServer.Name = "btnInitServer";
            this.btnInitServer.Size = new System.Drawing.Size(75, 23);
            this.btnInitServer.TabIndex = 0;
            this.btnInitServer.Text = "Init Server";
            this.btnInitServer.UseVisualStyleBackColor = true;
            this.btnInitServer.Click += new System.EventHandler(this.btnInitServer_Click);
            // 
            // rtbServerOutput
            // 
            this.rtbServerOutput.Location = new System.Drawing.Point(13, 43);
            this.rtbServerOutput.Name = "rtbServerOutput";
            this.rtbServerOutput.Size = new System.Drawing.Size(479, 391);
            this.rtbServerOutput.TabIndex = 1;
            this.rtbServerOutput.Text = "";
            // 
            // btnKillServer
            // 
            this.btnKillServer.Enabled = false;
            this.btnKillServer.Location = new System.Drawing.Point(94, 13);
            this.btnKillServer.Name = "btnKillServer";
            this.btnKillServer.Size = new System.Drawing.Size(75, 23);
            this.btnKillServer.TabIndex = 2;
            this.btnKillServer.Text = "Kill Server";
            this.btnKillServer.UseVisualStyleBackColor = true;
            this.btnKillServer.Click += new System.EventHandler(this.btnKillServer_Click);
            // 
            // grpClientControls
            // 
            this.grpClientControls.Controls.Add(this.btnSend);
            this.grpClientControls.Controls.Add(this.grpMessageParams);
            this.grpClientControls.Controls.Add(this.cmbMessageType);
            this.grpClientControls.Controls.Add(this.lblMessage);
            this.grpClientControls.Controls.Add(this.cmbFromClient);
            this.grpClientControls.Controls.Add(this.lblFromClient);
            this.grpClientControls.Location = new System.Drawing.Point(498, 151);
            this.grpClientControls.Name = "grpClientControls";
            this.grpClientControls.Size = new System.Drawing.Size(364, 283);
            this.grpClientControls.TabIndex = 3;
            this.grpClientControls.TabStop = false;
            this.grpClientControls.Text = "Client Controls";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(283, 15);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // grpMessageParams
            // 
            this.grpMessageParams.Location = new System.Drawing.Point(10, 71);
            this.grpMessageParams.Name = "grpMessageParams";
            this.grpMessageParams.Size = new System.Drawing.Size(348, 206);
            this.grpMessageParams.TabIndex = 4;
            this.grpMessageParams.TabStop = false;
            this.grpMessageParams.Text = "Message Params";
            // 
            // cmbMessageType
            // 
            this.cmbMessageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMessageType.FormattingEnabled = true;
            this.cmbMessageType.Location = new System.Drawing.Point(75, 44);
            this.cmbMessageType.Name = "cmbMessageType";
            this.cmbMessageType.Size = new System.Drawing.Size(121, 21);
            this.cmbMessageType.TabIndex = 3;
            this.cmbMessageType.SelectedIndexChanged += new System.EventHandler(this.cmbMessageType_SelectedIndexChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(16, 47);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(53, 13);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "Message:";
            // 
            // cmbFromClient
            // 
            this.cmbFromClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromClient.FormattingEnabled = true;
            this.cmbFromClient.Location = new System.Drawing.Point(75, 17);
            this.cmbFromClient.Name = "cmbFromClient";
            this.cmbFromClient.Size = new System.Drawing.Size(121, 21);
            this.cmbFromClient.TabIndex = 1;
            // 
            // lblFromClient
            // 
            this.lblFromClient.AutoSize = true;
            this.lblFromClient.Location = new System.Drawing.Point(7, 20);
            this.lblFromClient.Name = "lblFromClient";
            this.lblFromClient.Size = new System.Drawing.Size(62, 13);
            this.lblFromClient.TabIndex = 0;
            this.lblFromClient.Text = "From Client:";
            // 
            // chkLogRules
            // 
            this.chkLogRules.AutoSize = true;
            this.chkLogRules.Checked = true;
            this.chkLogRules.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogRules.Location = new System.Drawing.Point(175, 17);
            this.chkLogRules.Name = "chkLogRules";
            this.chkLogRules.Size = new System.Drawing.Size(101, 17);
            this.chkLogRules.TabIndex = 4;
            this.chkLogRules.Text = "Log Long Rules";
            this.chkLogRules.UseVisualStyleBackColor = true;
            this.chkLogRules.CheckedChanged += new System.EventHandler(this.chkLogRules_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(282, 13);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 442);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkLogRules);
            this.Controls.Add(this.grpClientControls);
            this.Controls.Add(this.btnKillServer);
            this.Controls.Add(this.rtbServerOutput);
            this.Controls.Add(this.btnInitServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpClientControls.ResumeLayout(false);
            this.grpClientControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInitServer;
        private System.Windows.Forms.RichTextBox rtbServerOutput;
        private System.Windows.Forms.Button btnKillServer;
        private System.Windows.Forms.GroupBox grpClientControls;
        private System.Windows.Forms.ComboBox cmbMessageType;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ComboBox cmbFromClient;
        private System.Windows.Forms.Label lblFromClient;
        private System.Windows.Forms.GroupBox grpMessageParams;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.CheckBox chkLogRules;
        private System.Windows.Forms.Button btnClear;
    }
}

