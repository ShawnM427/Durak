using Durak.Client;
using Durak.Common;
using DurakGame.Properties;
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
    /// Represents the form used to browse for servers
    /// </summary>
    public partial class frmServerBrowser : Form
    {
        GameClient myClient;
        Timer myTimer;
        DiscoveredServerCollection myServers;

        Dictionary<ServerTag, ListViewItem> myListItems;

        /// <summary>
        /// Creates and initializes a new server browser form
        /// </summary>
        public frmServerBrowser()
        {
            InitializeComponent();

            Initialize();
        }

        /// <summary>
        /// Initializes this server browser form
        /// </summary>
        public void Initialize()
        {
            myClient = new GameClient(new ClientTag(Settings.Default.UserName));
            myClient.OnServerDiscovered += ServerDiscovered;

            myClient.Run();

            myClient.DiscoverServers();
            myTimer = new Timer();
            myTimer.Interval = 5000;
            myTimer.Enabled = true;
            myTimer.Tick += PollServersEvent;
            myTimer.Start();

            myServers = new DiscoveredServerCollection();
            myServers.OnNewServerDiscovered += NewServerDiscovered;
            myServers.OnServerUpdated += ServerUpdated;

            myListItems = new Dictionary<ServerTag, ListViewItem>();

            lstServers.MouseDoubleClick += ServerListDoubleClicked;
        }

        /// <summary>
        /// Invoked when a server list item has been double clicked
        /// </summary>
        /// <param name="sender">The object to be invoked</param>
        /// <param name="e">The mouse event argument containing the mouse information</param>
        private void ServerListDoubleClicked(object sender, MouseEventArgs e)
        {
            ListViewItem item = lstServers.HitTest(e.Location).Item;

            if (item != null)
            {
                if (!myClient.IsReady)
                    myClient.Run();

                ServerTag tag = (ServerTag)item.Tag;

                Hide();

                frmLobby lobby = new frmLobby();
                lobby.InitMultiplayer(myClient, tag);
                DialogResult result = lobby.ShowDialog();

                if (result == DialogResult.OK)
                    Close();
                else
                {
                    Show();
                    myClient.Run();
                }
            }
        }

        /// <summary>
        /// Invoked when a server tag has been updated
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The server tag that was updated</param>
        private void ServerUpdated(object sender, ServerTag e)
        {
            myListItems[e].SubItems[0].Text = e.Name;
            myListItems[e].SubItems[1].Text = e.PlayerCount + "/" + e.SupportedPlayerCount;
        }

        /// <summary>
        /// Invoked when a new server has been discovered
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="tag">The server tag that was discovered</param>
        private void NewServerDiscovered(object sender, ServerTag tag)
        {
            ListViewItem item = new ListViewItem(new string[] { tag.Name, tag.PlayerCount + "/" + tag.SupportedPlayerCount });
            item.Tag = tag;
            myListItems.Add(tag, item);
            lstServers.Items.Add(item);
        }

        /// <summary>
        /// Invoked when the server timer tick has been invoked
        /// </summary>
        /// <param name="sender">The sender that raised the event (the timer)</param>
        /// <param name="e">The default event arguments</param>
        private void PollServersEvent(object sender, EventArgs e)
        {
            myClient.DiscoverServers();
        }

        /// <summary>
        /// Invoked when a client has received a discovery response from a server
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="tag">The server tag that was discovered</param>
        private void ServerDiscovered(object sender, ServerTag tag)
        {
            myServers.AddItem(tag);
        }
    }
}
