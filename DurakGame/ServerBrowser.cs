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
    public partial class frmServerBrowser : Form
    {
        GameClient myClient;
        Timer myTimer;
        DiscoveredServerCollection myServers;

        Dictionary<ServerTag, ListViewItem> myListItems;

        public frmServerBrowser()
        {
            InitializeComponent();

            Initialize();
        }

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

        private void ServerUpdated(object sender, ServerTag e)
        {
            myListItems[e].SubItems[0].Text = e.Name;
            myListItems[e].SubItems[1].Text = e.PlayerCount + "/" + e.SupportedPlayerCount;
        }

        private void NewServerDiscovered(object sender, ServerTag tag)
        {
            ListViewItem item = new ListViewItem(new string[] { tag.Name, tag.PlayerCount + "/" + tag.SupportedPlayerCount });
            item.Tag = tag;
            myListItems.Add(tag, item);
            lstServers.Items.Add(item);
        }

        private void PollServersEvent(object sender, EventArgs e)
        {
            myClient.DiscoverServers();
        }

        private void ServerDiscovered(object sender, ServerTag tag)
        {
            myServers.AddItem(tag);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
