using Durak.Client;
using Durak.Common;
using DurakGame.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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

        Dictionary<ServerTag, DataGridViewRow> myListItems;

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
            InitClient();

            myServers = new DiscoveredServerCollection();
            myServers.OnNewServerDiscovered += NewServerDiscovered;
            myServers.OnServerUpdated += ServerUpdated;

            myListItems = new Dictionary<ServerTag, DataGridViewRow>();


            dgvServers.MouseDoubleClick += ServerListDoubleClicked;
        }
        
        /// <summary>
        /// Handles setting up the game client
        /// </summary>
        private void InitClient()
        {
            myClient = new GameClient(new ClientTag(Settings.Default.UserName));
            myClient.OnServerDiscovered += ServerDiscovered;

            myClient.Run();

            myClient.OnConnected += ClientConnected;

            myClient.DiscoverServers();
            myTimer = new Timer();
            myTimer.Interval = 5000;
            myTimer.Enabled = true;
            myTimer.Tick += PollServersEvent;
            myTimer.Start();
        }

        /// <summary>
        /// Invoked when a server list item has been double clicked
        /// </summary>
        /// <param name="sender">The object to be invoked</param>
        /// <param name="e">The mouse event argument containing the mouse information</param>
        private void ServerListDoubleClicked(object sender, MouseEventArgs e)
        {
            int row = dgvServers.HitTest(e.X, e.Y).RowIndex;

            if (row >= 0 && row < dgvServers.RowCount)
            {
                DataGridViewRow dataRow = dgvServers.Rows[row];

                if (dataRow != null)
                {
                    if (!myClient.IsReady)
                        myClient.Run();


                    ServerTag tag = (ServerTag)dataRow.Tag;

                    string password = "";

                    if (tag.PasswordProtected)
                    {
                        password = Prompt.ShowDialog("Enter password", "Enter Password");
                    }

                    myClient.ConnectTo(tag);
                }
            }
        }

        /// <summary>
        /// Invoked when the client has successfully connected to a server
        /// </summary>
        /// <param name="sender">The object that invoked the event (the Game Client)</param>
        /// <param name="e">The empty event arguments</param>
        private void ClientConnected(object sender, EventArgs e)
        {
            myTimer.Stop();

            Hide();

            frmLobby lobby = new frmLobby();
            lobby.InitMultiplayer(myClient);
            DialogResult result = lobby.ShowDialog();

            if (!myClient.IsReady)
                myClient.Run();

            System.Threading.Thread.Sleep(25);

            myServers.Clear();
            dgvServers.Rows.Clear();
            myClient.OnServerDiscovered += ServerDiscovered;
            myClient.DiscoverServers();
            myTimer.Start();

            Show();
        }

        /// <summary>
        /// Invoked when a server tag has been updated
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The server tag that was updated</param>
        private void ServerUpdated(object sender, ServerTag e)
        {
            myListItems[e].Cells[0].Value = e.Name;
            myListItems[e].Cells[1].Value = e.PlayerCount + "/" + e.SupportedPlayerCount;
        }

        /// <summary>
        /// Invoked when a new server has been discovered
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="tag">The server tag that was discovered</param>
        private void NewServerDiscovered(object sender, ServerTag tag)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.Tag = tag;
            row.CreateCells(dgvServers);
            row.Cells[0].Value = tag.Name;
            row.Cells[1].Value = tag.PlayerCount + "/" + tag.SupportedPlayerCount;

            dgvServers.Rows.Add(row);
        }

        /// <summary>
        /// Overrides the on closing event for the server browser,
        /// this lets us gracefully kill the game client
        /// </summary>
        /// <param name="e">The event arguments that lets us cancel the closing if desired</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            myTimer.Tick -= PollServersEvent;
            myTimer.Stop();

            myClient.Stop();
            myClient = null;
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

        /// <summary>
        /// Invoked when the direct connect button is pressed, this will attempt to connect the client to the provided IP
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void btnDirect_Click(object sender, EventArgs e)
        {
            IPAddress address = null;

            if (IPAddress.TryParse(txtDirect.Text, out address))
            {
                myClient.TryDirectConnect(address);
            }
            else
            {
                MessageBox.Show("Could not parse IP, please ensure it was entered correctly");
                txtDirect.Focus();
                txtDirect.SelectAll();
            }
        }

        /// <summary>
        /// Invoked when the back button is pressed, this will close the server browser
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
