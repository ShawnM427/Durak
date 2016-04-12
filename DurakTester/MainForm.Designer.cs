namespace DurakTester
{
    partial class frmMain
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
            this.chkLogRules = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.gameStateVisualizer1 = new DurakCommon.GameStateVisualizer();
            this.lblClients = new System.Windows.Forms.Label();
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
            // gameStateVisualizer1
            // 
            this.gameStateVisualizer1.Location = new System.Drawing.Point(671, 12);
            this.gameStateVisualizer1.Name = "gameStateVisualizer1";
            this.gameStateVisualizer1.Size = new System.Drawing.Size(430, 422);
            this.gameStateVisualizer1.TabIndex = 6;
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClients.Location = new System.Drawing.Point(529, 13);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(85, 25);
            this.lblClients.TabIndex = 7;
            this.lblClients.Text = "Clients";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 442);
            this.Controls.Add(this.lblClients);
            this.Controls.Add(this.gameStateVisualizer1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkLogRules);
            this.Controls.Add(this.btnKillServer);
            this.Controls.Add(this.rtbServerOutput);
            this.Controls.Add(this.btnInitServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMain";
            this.Text = "Durak Tester";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInitServer;
        private System.Windows.Forms.RichTextBox rtbServerOutput;
        private System.Windows.Forms.Button btnKillServer;
        private System.Windows.Forms.CheckBox chkLogRules;
        private System.Windows.Forms.Button btnClear;
        private DurakCommon.GameStateVisualizer gameStateVisualizer1;
        private System.Windows.Forms.Label lblClients;
    }
}

