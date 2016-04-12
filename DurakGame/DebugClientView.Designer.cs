namespace DurakGame
{
    partial class DebugClientView
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
            this.gsvMain = new DurakCommon.GameStateVisualizer();
            this.SuspendLayout();
            // 
            // gsvMain
            // 
            this.gsvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gsvMain.Location = new System.Drawing.Point(0, 0);
            this.gsvMain.Name = "gsvMain";
            this.gsvMain.Size = new System.Drawing.Size(516, 417);
            this.gsvMain.TabIndex = 0;
            // 
            // DebugClientView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 417);
            this.Controls.Add(this.gsvMain);
            this.Name = "DebugClientView";
            this.Text = "DebugClientView";
            this.ResumeLayout(false);

        }

        #endregion

        private DurakCommon.GameStateVisualizer gsvMain;
    }
}