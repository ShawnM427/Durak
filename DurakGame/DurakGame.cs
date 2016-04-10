using Durak;
using Durak.Client;
using Durak.Common;
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
        private struct PlayerUITag
        {
            public Panel Panel;
            public Label NameLabel;
            public Label CardCountLabel;
            public CardBox CardBox;
        }

        GameClient myClient;
        GameServer myServer;

        Dictionary<Player, PlayerUITag> myPlayerUIs;

        public frmDurakGame()
        {
            InitializeComponent();

            myPlayerUIs = new Dictionary<Player, PlayerUITag>();

            pnlPlayer1.Visible = false;
            pnlPlayer2.Visible = false;
            pnlPlayer3.Visible = false;
            pnlPlayer4.Visible = false;
            pnlPlayer5.Visible = false;
        }

        public void SetServer(GameServer server)
        {
            myServer = server;
        }

        public void SetClient(GameClient client)
        {
            myClient = client;

            myClient.LocalState.AddStateChangedEvent("attacking_card", 0, (X, Y) => { cbxPlayerAttack1.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("attacking_card", 1, (X, Y) => { cbxPlayerAttack2.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("attacking_card", 2, (X, Y) => { cbxPlayerAttack3.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("attacking_card", 3, (X, Y) => { cbxPlayerAttack4.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("attacking_card", 4, (X, Y) => { cbxPlayerAttack5.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("attacking_card", 5, (X, Y) => { cbxPlayerAttack6.Card = Y.GetValuePlayingCard(); });

            myClient.LocalState.AddStateChangedEvent("defending_card", 0, (X, Y) => { cbxDefence1.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("defending_card", 1, (X, Y) => { cbxDefence2.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("defending_card", 2, (X, Y) => { cbxDefence3.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("defending_card", 3, (X, Y) => { cbxDefence4.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("defending_card", 4, (X, Y) => { cbxDefence5.Card = Y.GetValuePlayingCard(); });
            myClient.LocalState.AddStateChangedEvent("defending_card", 5, (X, Y) => { cbxDefence6.Card = Y.GetValuePlayingCard(); });

            myClient.OnDisconnected += ClientDisconnected;

            int localIndex = 0;
            for(byte index = 0; index < myClient.KnownPlayers.Count; index ++)
            {
                Player player = myClient.KnownPlayers[index];
                if (player != null && player.PlayerId != myClient.PlayerId)
                {
                    PlayerUITag tag = new PlayerUITag();

                    switch (localIndex)
                    {
                        case 0:
                            tag.Panel = pnlPlayer1;
                            tag.NameLabel = lblPlayer1;
                            tag.CardCountLabel = lblPlayer1CardsLeft;
                            tag.CardBox = cbxPlayer1;
                            break;
                        case 1:
                            tag.Panel = pnlPlayer2;
                            tag.NameLabel = lblPlayer2;
                            tag.CardCountLabel = lblPlayer2CardsLeft;
                            tag.CardBox = cbxPlayer2;
                            break;
                        case 2:
                            tag.Panel = pnlPlayer3;
                            tag.NameLabel = lblPlayer3;
                            tag.CardCountLabel = lblPlayer3CardsLeft;
                            tag.CardBox = cbxPlayer3;
                            break;
                        case 3:
                            tag.Panel = pnlPlayer4;
                            tag.NameLabel = lblPlayer4;
                            tag.CardCountLabel = lblPlayer4CardsLeft;
                            tag.CardBox = cbxPlayer4;
                            break;
                        case 4:
                            tag.Panel = pnlPlayer5;
                            tag.NameLabel = lblPlayer5;
                            tag.CardCountLabel = lblPlayer5CardsLeft;
                            tag.CardBox = cbxPlayer5;
                            break;
                    }

                    myPlayerUIs.Add(player, tag);

                    tag.Panel.Visible = true;

                    localIndex++;
                }
            }

            myClient.OnPlayerCardCountChanged += PlayerCardCountChanged;
        }

        private void ClientDisconnected(object sender, EventArgs e)
        {
            MessageBox.Show("Server has disconnected, returning to main menu");

            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            myClient?.Stop();
            myServer?.Stop();

            myClient.OnDisconnected -= ClientDisconnected;

            myClient = null;
            myServer = null;
        }

        private void PlayerCardCountChanged(Durak.Common.Player player, int newCardCount)
        {
            if (player.PlayerId != myClient.PlayerId)
            {
                PlayerUITag tag = myPlayerUIs[player];

                tag.CardCountLabel.Text = newCardCount.ToString();
            }
        }

        private void frmDurakGame_Load(object sender, EventArgs e)
        {

        }
    }
}
