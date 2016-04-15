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
    /// <summary>
    /// The form to display for singleplayer and multiplayer lobbies
    /// </summary>
    public partial class frmLobby : Form
    {
        /// <summary>
        /// Stores the game server if this game is host
        /// </summary>
        private GameServer myServer;
        /// <summary>
        /// Stores the game client
        /// </summary>
        private GameClient myClient;

        private bool isParentControllingClient;

        /// <summary>
        /// Stores a list of all the player views
        /// </summary>
        private List<PlayerView> myViews;

        /// <summary>
        /// Creates a new lobby form
        /// </summary>
        public frmLobby()
        {
            InitializeComponent();

            myViews = new List<PlayerView>();

            DialogResult = DialogResult.OK;

            chkSimulateBotThinkTime.Checked = Settings.Default.DefaultBotsThink;
        }

        /// <summary>
        /// Initializes the lobby in singleplayer mode
        /// </summary>
        public void InitSinglePlayer()
        {
            myServer = new GameServer(Settings.Default.DefaultMaxPlayers);
            myServer.IsSinglePlayerMode = true;
            
            myServer.Name = Settings.Default.DefaultServerName;
            myServer.Description = Settings.Default.DefaultServerDescription;
            myServer.Password = Settings.Default.DefaultServerPassword;

            myServer.Run();

            InitClient();

            myClient.ConnectTo(myServer, "");
        }

        /// <summary>
        /// Initializes the lobby in multiplayer mode, with the player being the host
        /// </summary>
        public void InitMultiplayer()
        {
            myServer = new GameServer(Settings.Default.DefaultMaxPlayers);
            
            myServer.Name = Settings.Default.DefaultServerName;
            myServer.Description = Settings.Default.DefaultServerDescription;
            myServer.Password = Settings.Default.DefaultServerPassword;

            myServer.Run();

            InitClient();

            myClient.ConnectTo(myServer);
        }

        /// <summary>
        /// Initializes the lobby in multiplayer mode, with the player connecting to another server
        /// </summary>
        /// <param name="tag">The tag to connect to</param>
        public void InitMultiplayer(ServerTag tag)
        {
            myServer = null;

            InitClient();

            string password = "";

            if (tag.PasswordProtected)
            {
                password = Prompt.ShowDialog("Enter password", "Enter Password");
            }
            
            myClient.ConnectTo(tag, password);
            btnStart.Text = "Ready";
        }

        /// <summary>
        /// Initializes the lobby in multiplayer mode, with the player connecting to another server
        /// </summary>
        /// <param name="client">The client to connect with</param>
        /// <param name="tag">The tag to connect to</param>
        public void InitMultiplayer(GameClient client, ServerTag tag)
        {
            myServer = null;

            myClient = client;
            isParentControllingClient = true;

            InitClient();

            string password = "";

            if (tag.PasswordProtected)
            {
                password = Prompt.ShowDialog("Enter password", "Enter Password");
            }
            
            myClient.ConnectTo(tag, password);
            btnStart.Text = "Ready";
        }

        /// <summary>
        /// Initializes the lobby in multiplayer mode, with the client already connected to the server
        /// </summary>
        /// <param name="client">The client to connect with</param>
        public void InitMultiplayer(GameClient client)
        {
            myServer = null;

            myClient = client;

            isParentControllingClient = true;

            InitClient();

            ClientConnected(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles setting up the local client and wiring it's events
        /// </summary>
        private void InitClient()
        {
            if (myClient == null)
                myClient = new GameClient(new ClientTag(Settings.Default.UserName));

            myClient.OnServerStateUpdated += ServerStateUpdated;
            myClient.OnFinishedConnect += ClientConnected;
            myClient.OnPlayerConnected += PlayerConnected;
            myClient.OnKicked += ClientKicked;
            myClient.OnPlayerLeft += PlayerLeft;
            myClient.OnPlayerChat += PlayerChat;
            myClient.OnConnectionFailed += ClientConnectFailed;
            myClient.OnDisconnected += ClientDisconnected;
            myClient.OnCannotStartGame += (x, y) => { MessageBox.Show(y); };

            if (!myClient.IsReady)
                myClient.Run();
        }

        private void ClientKicked(object sender, string e)
        {
            MessageBox.Show(e, "You have been kicked", MessageBoxButtons.OK);
            Close();
        }
        
        /// <summary>
        /// Invoked when the client has disconnected from the server
        /// </summary>
        /// <param name="sender">The object that invoked the event (the GameClient)</param>
        /// <param name="e">The default event arguments</param>
        private void ClientDisconnected(object sender, EventArgs e)
        {
            MessageBox.Show("Disconnected from server", "Disconnected");
            DialogResult = DialogResult.Abort;
            Close();
        }

        /// <summary>
        /// Invoked when the client has failed to connect to a server
        /// </summary>
        /// <param name="sender">The object that invoked the event (the GameClient)</param>
        /// <param name="e">The reason that the connection failed</param>
        private void ClientConnectFailed(object sender, string e)
        {
            MessageBox.Show(e, "Server Connection failed");
            DialogResult = DialogResult.Abort;
            Close();
        }

        /// <summary>
        /// Invoked when a player chat message has been received
        /// </summary>
        /// <param name="sender">The object that raised the event (the client)</param>
        /// <param name="player">The player that sent the message</param>
        /// <param name="message">The message that the player sent</param>
        private void PlayerChat(object sender, Player player, string message)
        {
            txtChat.AppendText(string.Format("[{0}]: {1}\n", player == null ? "Server" :  player.Name, message));
        }

        /// <summary>
        /// Invoked when a player has left the game
        /// </summary>
        /// <param name="sender">The object to invoke the event (the client)</param>
        /// <param name="player">The player that has left</param>
        /// <param name="reason">The reason they left</param>
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

            lblServerName.Text = myClient.ConnectedServer.Value.Name;
            lblServerDescription.Text = myClient.ConnectedServer.Value.Description;

            if (!myClient.IsHost)
                grpGameSettings.Visible = false;
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
                pnlAddBot.Visible = myClient.IsHost;
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

                myClient.OnDisconnected -= ClientDisconnected;
                myClient.OnConnectionFailed -= ClientConnectFailed;
                myClient.OnConnected -= ClientConnected;

                frmDurakGame mainForm = new frmDurakGame();
                mainForm.SetClient(myClient);
                mainForm.SetServer(myServer);

                mainForm.ShowDialog();

                System.Threading.Thread.Sleep(15);

                if (myClient.ConnectedServer == null)
                {
                    this.Close();
                }
                else
                {
                    this.Show();

                    myClient.OnDisconnected += ClientDisconnected;
                    myClient.OnConnectionFailed += ClientConnectFailed;
                    myClient.OnConnected += ClientConnected;
                }
            }
        }

        /// <summary>
        /// Called when the form is about to close. This shuts down the client and server when applicable
        /// </summary>
        /// <param name="e">The event arguments used to cancel the shutdown if needed</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            myClient.OnDisconnected -= ClientDisconnected;
            myClient.OnConnectionFailed -= ClientConnectFailed;

            myClient?.Disconnect();

            if (!isParentControllingClient)
                myClient?.Stop();

            myServer?.Stop();
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
            myClient.RequestBot((byte)(Settings.Default.DefaultBotDifficulty * 255), txtBotName.Text);
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
            if (myClient.IsHost)
            {
                if (myClient.ConnectedServer == null)
                    MessageBox.Show("Error, client not connected to local server");
                else
                {
                    myClient.RequestBotSettings(chkSimulateBotThinkTime.Checked, 1000, 4000, Settings.Default.DefaultBotDifficulty);

                    int numCards = 36;

                    if (rbn20Cards.Checked)
                        numCards = 20;
                    else if (rbn52Cards.Checked)
                        numCards = 52;

                    myClient.RequestState(StateParameter.Construct<int>(Names.NUM_INIT_CARDS, numCards, true));
                    myClient.RequestStart();
                }
            }
            else
            {
                myViews.FirstOrDefault(X => X.Player.PlayerId == myClient.PlayerId).IsReady = true;
                myClient.SetReadiness(true);
            }
        }

    }
}
