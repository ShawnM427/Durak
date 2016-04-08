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

        public frmLobby()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Initializes the lobby in singleplayer mode
        /// </summary>
        public void InitSinglePlayer()
        {
            myServer = new GameServer();
            myServer.IsSinglePlayerMode = true;
            myServer.Run();
            
            myClient = new GameClient(new ClientTag(Settings.Default.UserName));
            myClient.OnServerStateUpdated += ServerStateUpdated;
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
            myClient.Run();

            string password = "";

            if (tag.PasswordProtected)
            {
                password = Prompt.ShowDialog("Enter password", "Enter Password");
            }
            
            myClient.ConnectTo(tag, password);
        }

        /// <summary>
        /// Requests that the game starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            myClient.RequestStart();
        }

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
    }
}
