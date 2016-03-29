using Durak.Client;
using Durak.Server;
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
    public partial class frmDurakGame : Form
    {
        GameClient myClient;
        GameServer myServer;

        public frmDurakGame()
        {
            InitializeComponent();
        }

        public void SetClient(GameClient client)
        {
            myClient = client;
        }

    }
}
