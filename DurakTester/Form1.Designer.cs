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
            this.rtbServerOutput.Size = new System.Drawing.Size(200, 276);
            this.rtbServerOutput.TabIndex = 1;
            this.rtbServerOutput.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 531);
            this.Controls.Add(this.rtbServerOutput);
            this.Controls.Add(this.btnInitServer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInitServer;
        private System.Windows.Forms.RichTextBox rtbServerOutput;
    }
}

