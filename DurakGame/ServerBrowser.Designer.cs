namespace DurakGame
{
    partial class frmServerBrowser
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("");
            this.label1 = new System.Windows.Forms.Label();
            this.txtDirect = new System.Windows.Forms.TextBox();
            this.btnDirect = new System.Windows.Forms.Button();
            this.lstServers = new System.Windows.Forms.ListView();
            this.Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IPAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NoOfPlayers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnListConnect = new System.Windows.Forms.Button();
            this.txtSelected = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter IP Address:";
            // 
            // txtDirect
            // 
            this.txtDirect.Location = new System.Drawing.Point(109, 12);
            this.txtDirect.Name = "txtDirect";
            this.txtDirect.Size = new System.Drawing.Size(108, 20);
            this.txtDirect.TabIndex = 1;
            // 
            // btnDirect
            // 
            this.btnDirect.Location = new System.Drawing.Point(223, 10);
            this.btnDirect.Name = "btnDirect";
            this.btnDirect.Size = new System.Drawing.Size(102, 23);
            this.btnDirect.TabIndex = 2;
            this.btnDirect.Text = "Direct Connect";
            this.btnDirect.UseVisualStyleBackColor = true;
            // 
            // lstServers
            // 
            this.lstServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Name,
            this.IPAddress,
            this.NoOfPlayers});
            this.lstServers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstServers.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.lstServers.Location = new System.Drawing.Point(0, 93);
            this.lstServers.Name = "lstServers";
            this.lstServers.Size = new System.Drawing.Size(335, 209);
            this.lstServers.TabIndex = 3;
            this.lstServers.UseCompatibleStateImageBehavior = false;
            this.lstServers.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // btnListConnect
            // 
            this.btnListConnect.Location = new System.Drawing.Point(223, 45);
            this.btnListConnect.Name = "btnListConnect";
            this.btnListConnect.Size = new System.Drawing.Size(102, 23);
            this.btnListConnect.TabIndex = 4;
            this.btnListConnect.Text = "List Connect";
            this.btnListConnect.UseVisualStyleBackColor = true;
            this.btnListConnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSelected
            // 
            this.txtSelected.Location = new System.Drawing.Point(109, 47);
            this.txtSelected.Name = "txtSelected";
            this.txtSelected.Size = new System.Drawing.Size(108, 20);
            this.txtSelected.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Selected Server IP:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Server Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(137, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "IP Address";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(282, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Players";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // frmServerBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 302);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSelected);
            this.Controls.Add(this.btnListConnect);
            this.Controls.Add(this.lstServers);
            this.Controls.Add(this.btnDirect);
            this.Controls.Add(this.txtDirect);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmServerBrowser";
            this.Text = "Select Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDirect;
        private System.Windows.Forms.Button btnDirect;
        private System.Windows.Forms.ListView lstServers;
        private System.Windows.Forms.ColumnHeader Name;
        private System.Windows.Forms.ColumnHeader IPAddress;
        private System.Windows.Forms.ColumnHeader NoOfPlayers;
        private System.Windows.Forms.Button btnListConnect;
        private System.Windows.Forms.TextBox txtSelected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}