﻿namespace DurakGame
{
    partial class PlayerView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.imgPlayerType = new System.Windows.Forms.PictureBox();
            this.imgReady = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgPlayerType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgReady)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.Location = new System.Drawing.Point(59, 21);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(73, 13);
            this.lblPlayerName.TabIndex = 0;
            this.lblPlayerName.Text = "[Player Name]";
            // 
            // imgPlayerType
            // 
            this.imgPlayerType.Location = new System.Drawing.Point(5, 5);
            this.imgPlayerType.Margin = new System.Windows.Forms.Padding(0);
            this.imgPlayerType.Name = "imgPlayerType";
            this.imgPlayerType.Size = new System.Drawing.Size(50, 50);
            this.imgPlayerType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgPlayerType.TabIndex = 2;
            this.imgPlayerType.TabStop = false;
            // 
            // imgReady
            // 
            this.imgReady.Location = new System.Drawing.Point(205, 5);
            this.imgReady.Margin = new System.Windows.Forms.Padding(0);
            this.imgReady.Name = "imgReady";
            this.imgReady.Size = new System.Drawing.Size(50, 50);
            this.imgReady.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgReady.TabIndex = 1;
            this.imgReady.TabStop = false;
            // 
            // PlayerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.imgPlayerType);
            this.Controls.Add(this.imgReady);
            this.Controls.Add(this.lblPlayerName);
            this.Name = "PlayerView";
            this.Size = new System.Drawing.Size(260, 60);
            ((System.ComponentModel.ISupportInitialize)(this.imgPlayerType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgReady)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.PictureBox imgReady;
        private System.Windows.Forms.PictureBox imgPlayerType;
    }
}