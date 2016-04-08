using Durak.Client;
using Durak.Common;
using Durak.Server;
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
    public partial class frmLobby : Form
    {
        GameServer myServer;
        GameClient myClient;

        List<PlayerView> myViews;

        public frmLobby()
        {
            InitializeComponent();

            myViews = new List<PlayerView>();
        }

        /// <summary>
        /// Initializes the lobby in singleplayer mode
        /// </summary>
        public void InitSinglePlayer()
        {
            myServer = new GameServer(6);
            myServer.IsSinglePlayerMode = true;
            myServer.Run();
            
            myClient = new GameClient(new ClientTag(Settings.Default.UserName));
            myClient.OnServerStateUpdated += ServerStateUpdated;
            myClient.OnFinishedConnect += ClientConnected;
            myClient.OnPlayerConnected += PlayerConnected;
            myClient.OnPlayerLeft += PlayerLeft;
            myClient.Run();

            myClient.ConnectTo(myServer, "");
        }

        /// <summary>
        /// Initializes the lobby in multiplayer mode, with the player being the host
        /// </summary>
        public void InitMultiplayer()
        {
            myServer = new GameServer(6);
            myServer.Name = "Durak Game";
            myServer.Description = "";
            myServer.SetPassword(Settings.Default.DefaultServerPassword);
            myServer.Run();

            myClient.RequestStart();
            myClient = new GameClient(new ClientTag(Settings.Default.UserName));
            myClient.OnServerStateUpdated += ServerStateUpdated;
            myClient.OnFinishedConnect += ClientConnected;
            myClient.OnPlayerLeft += PlayerLeft;
            myClient.Run();

            myClient.ConnectTo(myServer);
        }

        /// <summary>
        /// Initializes the lobby in multiplayer mode, with the player connecting to another server
        /// </summary>
        /// <param name="tag"></param>
        public void InitMultiplayer(ServerTag tag)
        {
            myServer = null;

            myClient.RequestStart();
            myClient = new GameClient(new ClientTag(Settings.Default.UserName));
            myClient.OnServerStateUpdated += ServerStateUpdated;
            myClient.OnFinishedConnect += ClientConnected;
            myClient.OnPlayerLeft += PlayerLeft;
            myClient.Run();

            string password = "";

            if (tag.PasswordProtected)
            {
                password = Prompt.ShowDialog("Enter password", "Enter Password");
            }
            
            myClient.ConnectTo(tag, password);
        }

        private void PlayerLeft(object sender, Player player, string reason)
        {
            PlayerView toRemove = myViews.FirstOrDefault(x => x.Player == player);
            myViews.Remove(toRemove);

            if (toRemove != null)
                pnlPlayers.Controls.Remove(toRemove);

            UpdatePlayerView();
        }
        
        /// <summary>
        /// Invoked when a player has joined the game
        /// </summary>
        /// <param name="sender">The object ot invoke the event (the client)</param>
        /// <param name="e">The player that has joined</param>
        private void PlayerConnected(object sender, Player e)
        {
            PlayerView view = BuildView(e);
            pnlPlayers.Controls.Add(view);

            myViews.Add(view);

            UpdatePlayerView();
        }

        /// <summary>
        /// Invoked after the client has connected to the server and received the welcome package
        /// </summary>
        /// <param name="sender">The object ot invoke the event (the client)</param>
        /// <param name="e">An empty event argument</param>
        private void ClientConnected(object sender, EventArgs e)
        {
            foreach(Player p in myClient.KnownPlayers)
            {
                PlayerView view = BuildView(p);
                if (p.PlayerId == myClient.PlayerId)
                    view.HasControl = true;
                view.Left = 5;

                pnlPlayers.Controls.Add(view);
                myViews.Add(view);
            }
            UpdatePlayerView();
        }

        /// <summary>
        /// Updates the player view panel, moving all player views into the correct position
        /// </summary>
        private void UpdatePlayerView()
        {
            for (int index = 0; index < myViews.Count; index++)
            {
                myViews[index].Top = 5 + index * myViews[index].Height;
            }

            if (myClient.KnownPlayers.PlayerCount == myClient.KnownPlayers.Count)
            {
                pnlAddBot.Visible = false;
            }
            else
            {
                pnlAddBot.Visible = true;
                pnlAddBot.Top = 5 + myViews.Count * 60;
            }
        }

        /// <summary>
        /// Builds a player view for the specified player
        /// </summary>
        /// <param name="p">The player to build a view for</param>
        /// <returns>The view built for the player</returns>
        private PlayerView BuildView(Player p)
        {
            PlayerView view = new PlayerView();
            view.Client = myClient;
            view.Player = p;
            view.IsReady = p.IsReady;
            view.Left = 5;

            return view;
        }

        /// <summary>
        /// Invoked when the connected server's state has updated
        /// </summary>
        /// <param name="sender">The object to raise the event (the client)</param>
        /// <param name="e">The servers new state</param>
        private void ServerStateUpdated(object sender, ServerState e)
        {
            if (e == ServerState.InGame)
            {
                this.Hide();

                frmDurakGame mainForm = new frmDurakGame();
                mainForm.SetClient(myClient);
                mainForm.SetServer(myServer);

                mainForm.ShowDialog();

                this.Close();
            }
        }

        /// <summary>
        /// Called when the form is about to close. This shuts down the client and server when applicable
        /// </summary>
        /// <param name="e">The event arguments used to cancel the shutdown if needed</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            myServer?.Stop();
            myClient?.Stop();

            myServer = null;
            myClient = null;
        }

        /// <summary>
        /// Called when the back button is pressed. This will return to the main menu
        /// </summary>
        /// <param name="sender">The object to invoke the event (the form)</param>
        /// <param name="e">An empty event args</param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Called when the add bot button is clicked
        /// </summary>
        /// <param name="sender">The object to invoke the event (the form)</param>
        /// <param name="e">An empty event args</param>
        private void btnAddBot_Click(object sender, EventArgs e)
        {
            myClient.RequestBot(128, txtBotName.Text);
        }

        /// <summary>
        /// Called when the send chat message is clicked
        /// </summary>
        /// <param name="sender">The object to invoke the event (the form)</param>
        /// <param name="e">An empty event args</param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMessage.Text))
                myClient.SendChatMessage(txtMessage.Text);

            txtMessage.Text = "";
        }

        /// <summary>
        /// Requests that the game starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (myClient.ConnectedServer == null)
                MessageBox.Show("Error, client not connected to local server");
            else
                myClient.RequestStart();
        }

    }
}
