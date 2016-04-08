using Durak.Client;
using Durak.Common;
using Durak.Common.Cards;
using DurakGame.Rules;
using Durak.Server;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace DurakTester
{
    public partial class frmMain : Form
    {
        frmClientView[] myClients;
        GameServer myServer;
        IPAddress myServerAddress;

        public frmMain()
        {
            InitializeComponent();

            VerifyDuelWin w = new VerifyDuelWin();

            myClients = new frmClientView[4];
            
            for (int index = 0; index < myClients.Length; index ++)
            {
                Button clientStartButton = new Button();
                clientStartButton.Text = "Start";
                clientStartButton.Click += btnClientStart_Click;
                clientStartButton.Tag = index;
                clientStartButton.Left = rtbServerOutput.Right + 5;
                clientStartButton.Top = rtbServerOutput.Top + (24 * index);

                Button clientEndButton = new Button();
                clientEndButton.Text = "Stop";
                clientEndButton.Enabled = false;
                clientEndButton.Click += btnClientStop_Click;
                clientEndButton.Left = clientStartButton.Right + 5;
                clientEndButton.Top = clientStartButton.Top;

                ClientTag tag = new ClientTag()
                {
                    Start = clientStartButton,
                    End = clientEndButton,
                    Index = index
                };
                
                clientStartButton.Tag = tag;
                clientEndButton.Tag = tag;

                this.Controls.Add(clientStartButton);
                this.Controls.Add(clientEndButton);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BotPlayer.SimulateThinkTime = false;
        }

        private void btnInitServer_Click(object sender, EventArgs e)
        {
            myServer = new GameServer();
            myServer.SetOutput(rtbServerOutput);
            myServer.Run();

            if (myServerAddress == null)
                myServerAddress = myServer.IP;

            myServer.LogLongRules = chkLogRules.Checked;

            gameStateVisualizer1.SetState(myServer.GameState);

            btnInitServer.Enabled = false;
            btnKillServer.Enabled = true;
        }

        private void btnKillServer_Click(object sender, EventArgs e)
        {
            myServer.Stop();
            myServer = null;

            btnInitServer.Enabled = true;
            btnKillServer.Enabled = false;
        }
        
        private void btnClientStop_Click(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;

            if (buttonSender != null)
            {
                ClientTag tag = buttonSender.Tag as ClientTag;

                if (tag != null)
                {
                    tag.Viewer.Client.Stop();
                    tag.Viewer.Close();

                    tag.Start.Enabled = true;
                    tag.End.Enabled = false;
                }
            }
        }

        private void btnClientStart_Click(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;

            if (buttonSender != null)
            {
                ClientTag tag = buttonSender.Tag as ClientTag;

                if (tag != null)
                {
                    tag.Viewer = new frmClientView();

                    tag.Viewer.Show();
                    tag.Viewer.SetClient(new GameClient(new Durak.Common.ClientTag("Player " + tag.Index)));

                    tag.End.Enabled = true;
                    tag.Start.Enabled = false;
                }
            }
        }

        private class ClientTag
        {
            public Button Start;
            public Button End;
            public frmClientView Viewer;
            public int Index;
        }

        private void chkLogRules_CheckedChanged(object sender, EventArgs e)
        {
            if (myServer != null)
                myServer.LogLongRules = chkLogRules.Checked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbServerOutput.Text = "";
        }
    }
}
