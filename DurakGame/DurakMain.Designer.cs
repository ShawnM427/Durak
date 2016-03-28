namespace DurakGame
{
    partial class frmDurakMain
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCopyRight = new System.Windows.Forms.Label();
            this.btnPlaySingle = new System.Windows.Forms.Button();
            this.btnPlayMulti = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Stencil", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(371, 114);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Durak";
            // 
            // lblCopyRight
            // 
            this.lblCopyRight.AutoSize = true;
            this.lblCopyRight.Location = new System.Drawing.Point(107, 295);
            this.lblCopyRight.Name = "lblCopyRight";
            this.lblCopyRight.Size = new System.Drawing.Size(179, 13);
            this.lblCopyRight.TabIndex = 1;
            this.lblCopyRight.Text = "Copyright StormWeaverGames 2016";
            // 
            // btnPlaySingle
            // 
            this.btnPlaySingle.BackColor = System.Drawing.Color.DarkGreen;
            this.btnPlaySingle.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlaySingle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPlaySingle.Location = new System.Drawing.Point(95, 126);
            this.btnPlaySingle.Name = "btnPlaySingle";
            this.btnPlaySingle.Size = new System.Drawing.Size(202, 45);
            this.btnPlaySingle.TabIndex = 2;
            this.btnPlaySingle.Text = "Single Player";
            this.btnPlaySingle.UseVisualStyleBackColor = false;
            this.btnPlaySingle.MouseEnter += new System.EventHandler(this.btnPlaySingle_MouseEnter);
            this.btnPlaySingle.MouseLeave += new System.EventHandler(this.btnPlaySingle_MouseLeave);
            // 
            // btnPlayMulti
            // 
            this.btnPlayMulti.BackColor = System.Drawing.Color.DarkGreen;
            this.btnPlayMulti.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlayMulti.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPlayMulti.Location = new System.Drawing.Point(95, 177);
            this.btnPlayMulti.Name = "btnPlayMulti";
            this.btnPlayMulti.Size = new System.Drawing.Size(202, 45);
            this.btnPlayMulti.TabIndex = 3;
            this.btnPlayMulti.Text = "Multiplayer Player";
            this.btnPlayMulti.UseVisualStyleBackColor = false;
            this.btnPlayMulti.MouseEnter += new System.EventHandler(this.btnPlaySingle_MouseEnter);
            this.btnPlayMulti.MouseLeave += new System.EventHandler(this.btnPlaySingle_MouseLeave);
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.DarkGreen;
            this.btnAbout.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAbout.Location = new System.Drawing.Point(95, 228);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(202, 45);
            this.btnAbout.TabIndex = 4;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.MouseEnter += new System.EventHandler(this.btnPlaySingle_MouseEnter);
            this.btnAbout.MouseLeave += new System.EventHandler(this.btnPlaySingle_MouseLeave);
            // 
            // frmDurakMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(393, 317);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnPlayMulti);
            this.Controls.Add(this.btnPlaySingle);
            this.Controls.Add(this.lblCopyRight);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDurakMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Durak";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCopyRight;
        private System.Windows.Forms.Button btnPlaySingle;
        private System.Windows.Forms.Button btnPlayMulti;
        private System.Windows.Forms.Button btnAbout;
    }
}