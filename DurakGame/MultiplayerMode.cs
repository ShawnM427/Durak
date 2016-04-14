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
    /// Represents a form that allows you to select a mode to play in multiplayer
    /// </summary>
    public partial class frmMultiplayerMode : Form
    {
        /// <summary>
        /// Creates a new multiplayer mode form
        /// </summary>
        public frmMultiplayerMode()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the back button has been clicked
        /// </summary>
        /// <param name="sender">The button that raised the event</param>
        /// <param name="e">The blank event arguments</param>
        private void BackClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Invoked when the host button has been clicked
        /// </summary>
        /// <param name="sender">The button that raised the event</param>
        /// <param name="e">The blank event arguments</param>
        private void HostClick(object sender, EventArgs e)
        {
            this.Hide();

            frmLobby lobby = new frmLobby();
            lobby.InitMultiplayer();
            lobby.ShowDialog();

            this.Close();
        }

        /// <summary>
        /// Invoked when the join button has been clicked
        /// </summary>
        /// <param name="sender">The button that raised the event</param>
        /// <param name="e">The blank event arguments</param>
        private void JoinClick(object sender, EventArgs e)
        {
            this.Hide();

            frmServerBrowser browser = new frmServerBrowser();
            browser.ShowDialog();

            this.Close();
        }
    }
}
