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

        public void InitSinglePlayer()
        {
            myServer = new GameServer();
            myServer.IsSinglePlayerMode = true;
            myServer.Run();
            myClient = new GameClient(new ClientTag(Settings.Default.UserName));
            myClient.Run();

            myClient.ConnectTo(myServer, "");
        }
    }
}
