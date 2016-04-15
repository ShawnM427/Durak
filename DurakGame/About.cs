using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakGame
{
    /// <summary>
    /// The form for showing information about this app,
    /// </summary>
    public partial class frmAbout : Form
    {
        /// <summary>
        /// Creates a new About form
        /// </summary>
        public frmAbout()
        {
            InitializeComponent();

            wbrMain.Navigate(Environment.CurrentDirectory + "/Resources/about.html");
        }

        private void rtbMain_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
