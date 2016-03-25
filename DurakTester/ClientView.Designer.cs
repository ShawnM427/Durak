namespace DurakTester
{
    partial class ClientView
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
            this.btnDiscover = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.grpClientControls = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.grpMessageParams = new System.Windows.Forms.GroupBox();
            this.cmbMessageType = new System.Windows.Forms.ComboBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.gsvClientState = new DurakTester.GameStateVisualizer();
            this.cmbCards = new System.Windows.Forms.ComboBox();
            this.grpClientControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDiscover
            // 
            this.btnDiscover.Location = new System.Drawing.Point(13, 13);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.Size = new System.Drawing.Size(75, 23);
            this.btnDiscover.TabIndex = 0;
            this.btnDiscover.Text = "Discover";
            this.btnDiscover.UseVisualStyleBackColor = true;
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(94, 13);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(176, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // grpClientControls
            // 
            this.grpClientControls.Controls.Add(this.btnSend);
            this.grpClientControls.Controls.Add(this.grpMessageParams);
            this.grpClientControls.Controls.Add(this.cmbMessageType);
            this.grpClientControls.Controls.Add(this.lblMessage);
            this.grpClientControls.Location = new System.Drawing.Point(13, 42);
            this.grpClientControls.Name = "grpClientControls";
            this.grpClientControls.Size = new System.Drawing.Size(364, 283);
            this.grpClientControls.TabIndex = 4;
            this.grpClientControls.TabStop = false;
            this.grpClientControls.Text = "Client Controls";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(283, 17);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // grpMessageParams
            // 
            this.grpMessageParams.Location = new System.Drawing.Point(10, 46);
            this.grpMessageParams.Name = "grpMessageParams";
            this.grpMessageParams.Size = new System.Drawing.Size(348, 231);
            this.grpMessageParams.TabIndex = 4;
            this.grpMessageParams.TabStop = false;
            this.grpMessageParams.Text = "Message Params";
            // 
            // cmbMessageType
            // 
            this.cmbMessageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMessageType.FormattingEnabled = true;
            this.cmbMessageType.Location = new System.Drawing.Point(70, 19);
            this.cmbMessageType.Name = "cmbMessageType";
            this.cmbMessageType.Size = new System.Drawing.Size(121, 21);
            this.cmbMessageType.TabIndex = 3;
            this.cmbMessageType.SelectedIndexChanged += new System.EventHandler(this.cmbMessageType_SelectedIndexChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(11, 22);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(53, 13);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "Message:";
            // 
            // gsvClientState
            // 
            this.gsvClientState.Location = new System.Drawing.Point(399, 12);
            this.gsvClientState.Name = "gsvClientState";
            this.gsvClientState.Size = new System.Drawing.Size(460, 313);
            this.gsvClientState.TabIndex = 5;
            // 
            // cmbCards
            // 
            this.cmbCards.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCards.FormattingEnabled = true;
            this.cmbCards.Location = new System.Drawing.Point(272, 15);
            this.cmbCards.Name = "cmbCards";
            this.cmbCards.Size = new System.Drawing.Size(105, 21);
            this.cmbCards.TabIndex = 6;
            // 
            // ClientView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 336);
            this.Controls.Add(this.cmbCards);
            this.Controls.Add(this.gsvClientState);
            this.Controls.Add(this.grpClientControls);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnDiscover);
            this.Name = "ClientView";
            this.Text = "ClientView";
            this.grpClientControls.ResumeLayout(false);
            this.grpClientControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.GroupBox grpClientControls;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox grpMessageParams;
        private System.Windows.Forms.ComboBox cmbMessageType;
        private System.Windows.Forms.Label lblMessage;
        private GameStateVisualizer gsvClientState;
        private System.Windows.Forms.ComboBox cmbCards;
    }
}