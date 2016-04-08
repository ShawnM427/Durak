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
    public partial class frmMultiplayerMode : Form
    {
        public frmMultiplayerMode()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmLobby lobby = new frmLobby();
            lobby.InitMultiplayer();
            lobby.ShowDialog();

            this.Close();
        }

        private void btnPlaySingle_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmServerBrowser browser = new frmServerBrowser();
            browser.ShowDialog();

            this.Close();
        }
    }
}
