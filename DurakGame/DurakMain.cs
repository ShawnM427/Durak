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
    /// Represents the main entry form for the application
    /// </summary>
    public partial class frmDurakMain : Form
    {
        /// <summary>
        /// Creates a new main form
        /// </summary>
        public frmDurakMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the mouse has left the bounds of a button
        /// </summary>
        /// <param name="sender">The button that the event is invoked for</param>
        /// <param name="e">The blank event arguments</param>
        private void ButtonMouseLeft(object sender, EventArgs e)
        {
            (sender as Button).BackColor =  Color.DarkGreen;
        }

        /// <summary>
        /// Invoked when the mouse has left the bounds of a button
        /// </summary>
        /// <param name="sender">The button that the event is invoked for</param>
        /// <param name="e">The blank event arguments</param>
        private void ButtonMouseEntered(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.White;
        }

        /// <summary>
        /// Invoked when the play singleplayer button has been pressed
        /// </summary>
        /// <param name="sender">The button that the event is invoked for</param>
        /// <param name="e">The blank event arguments</param>
        private void btnPlaySingle_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmLobby lobby = new frmLobby();
            lobby.InitSinglePlayer();
            lobby.ShowDialog();

            this.Show();
        }

        /// <summary>
        /// Invoked when the play multiplayer button has been pressed
        /// </summary>
        /// <param name="sender">The button that the event is invoked for</param>
        /// <param name="e">The blank event arguments</param>
        private void btnPlayMulti_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmMultiplayerMode modeSelect = new frmMultiplayerMode();
            modeSelect.ShowDialog();

            this.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Hide();

            new frmSettings().ShowDialog();

            Show();
        }
    }
}
