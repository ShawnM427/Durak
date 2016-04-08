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
    public partial class frmDurakMain : Form
    {
        public frmDurakMain()
        {
            InitializeComponent();
        }


        private void btnPlaySingle_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor =  Color.DarkGreen;
        }

        private void btnPlaySingle_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.White;
        }

        private void btnPlaySingle_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmLobby lobby = new frmLobby();
            lobby.InitSinglePlayer();
            lobby.ShowDialog();

            this.Show();
        }
    }
}
