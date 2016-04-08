namespace DurakGame
{
    partial class frmLobby
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
            this.pnlPlayers = new System.Windows.Forms.Panel();
            this.pnlAddBot = new System.Windows.Forms.Panel();
            this.btnAddBot = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBotName = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlGameSettings = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.pnlServerInfo = new System.Windows.Forms.Panel();
            this.lblServerDescription = new System.Windows.Forms.Label();
            this.lblServerName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.pnlPlayers.SuspendLayout();
            this.pnlAddBot.SuspendLayout();
            this.pnlGameSettings.SuspendLayout();
            this.pnlServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPlayers
            // 
            this.pnlPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPlayers.Controls.Add(this.pnlAddBot);
            this.pnlPlayers.Location = new System.Drawing.Point(13, 14);
            this.pnlPlayers.Name = "pnlPlayers";
            this.pnlPlayers.Size = new System.Drawing.Size(270, 370);
            this.pnlPlayers.TabIndex = 0;
            // 
            // pnlAddBot
            // 
            this.pnlAddBot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAddBot.Controls.Add(this.btnAddBot);
            this.pnlAddBot.Controls.Add(this.label7);
            this.pnlAddBot.Controls.Add(this.txtBotName);
            this.pnlAddBot.Location = new System.Drawing.Point(5, 5);
            this.pnlAddBot.Name = "pnlAddBot";
            this.pnlAddBot.Size = new System.Drawing.Size(260, 60);
            this.pnlAddBot.TabIndex = 6;
            // 
            // btnAddBot
            // 
            this.btnAddBot.Location = new System.Drawing.Point(205, 2);
            this.btnAddBot.Name = "btnAddBot";
            this.btnAddBot.Size = new System.Drawing.Size(46, 46);
            this.btnAddBot.TabIndex = 8;
            this.btnAddBot.Text = "+";
            this.btnAddBot.UseVisualStyleBackColor = true;
            this.btnAddBot.Click += new System.EventHandler(this.btnAddBot_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Player Name:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBotName
            // 
            this.txtBotName.Location = new System.Drawing.Point(83, 17);
            this.txtBotName.Name = "txtBotName";
            this.txtBotName.Size = new System.Drawing.Size(102, 20);
            this.txtBotName.TabIndex = 2;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(12, 524);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Leave";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pnlGameSettings
            // 
            this.pnlGameSettings.AutoScroll = true;
            this.pnlGameSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGameSettings.Controls.Add(this.radioButton4);
            this.pnlGameSettings.Controls.Add(this.radioButton3);
            this.pnlGameSettings.Controls.Add(this.radioButton2);
            this.pnlGameSettings.Controls.Add(this.radioButton1);
            this.pnlGameSettings.Controls.Add(this.label3);
            this.pnlGameSettings.Controls.Add(this.label2);
            this.pnlGameSettings.Location = new System.Drawing.Point(290, 119);
            this.pnlGameSettings.Name = "pnlGameSettings";
            this.pnlGameSettings.Size = new System.Drawing.Size(190, 398);
            this.pnlGameSettings.TabIndex = 3;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(47, 93);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(80, 17);
            this.radioButton4.TabIndex = 5;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Two Decks";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(47, 70);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(94, 17);
            this.radioButton3.TabIndex = 4;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Deck and Half";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(47, 47);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(70, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Full Deck";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(47, 24);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(73, 17);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Half Deck";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Number of cards:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Game Settings";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(405, 524);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(13, 497);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(192, 20);
            this.txtMessage.TabIndex = 5;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(209, 495);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // pnlServerInfo
            // 
            this.pnlServerInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlServerInfo.Controls.Add(this.lblServerDescription);
            this.pnlServerInfo.Controls.Add(this.lblServerName);
            this.pnlServerInfo.Controls.Add(this.label5);
            this.pnlServerInfo.Controls.Add(this.label4);
            this.pnlServerInfo.Location = new System.Drawing.Point(290, 13);
            this.pnlServerInfo.Name = "pnlServerInfo";
            this.pnlServerInfo.Size = new System.Drawing.Size(190, 100);
            this.pnlServerInfo.TabIndex = 7;
            // 
            // lblServerDescription
            // 
            this.lblServerDescription.Location = new System.Drawing.Point(4, 39);
            this.lblServerDescription.Name = "lblServerDescription";
            this.lblServerDescription.Size = new System.Drawing.Size(183, 50);
            this.lblServerDescription.TabIndex = 3;
            this.lblServerDescription.Text = "DESCRIPTION";
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(82, 4);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(38, 13);
            this.lblServerName.TabIndex = 2;
            this.lblServerName.Text = "NAME";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Description:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Server Name:";
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(13, 390);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(271, 101);
            this.txtChat.TabIndex = 8;
            // 
            // frmLobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 559);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.pnlServerInfo);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pnlGameSettings);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.pnlPlayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLobby";
            this.Text = "Lobby";
            this.pnlPlayers.ResumeLayout(false);
            this.pnlAddBot.ResumeLayout(false);
            this.pnlAddBot.PerformLayout();
            this.pnlGameSettings.ResumeLayout(false);
            this.pnlGameSettings.PerformLayout();
            this.pnlServerInfo.ResumeLayout(false);
            this.pnlServerInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlPlayers;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel pnlGameSettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel pnlServerInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlAddBot;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBotName;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label lblServerDescription;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Button btnAddBot;
    }
}