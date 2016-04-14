using Durak;
using Durak.Client;
using Durak.Common;
using Durak.Common.Cards;
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

        //A struct to hold the ui items for a player
        private struct PlayerUITag
        {
            public BorderPanel Panel;
            public Label NameLabel;
            public Label CardCountLabel;
            public CardBox CardBox;
        }

        GameClient myClient;
        GameServer myServer;

        Dictionary<Player, PlayerUITag> myPlayerUIs;

        BorderPanel myDefendingPlayerContainer;
        BorderPanel myAttackingPlayerContainer;

        /// <summary>
        /// We use this to track if we are hard closing
        /// </summary>
        bool isHardClose = true;

        public frmDurakGame()
        {
            InitializeComponent();

            myPlayerUIs = new Dictionary<Player, PlayerUITag>();

            //Initially set the players panels visibility to false until specified if they are in the game.
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

            myClient.OnInvalidMove += (X, Y, Z) => { MessageBox.Show(Z, "Cannot play card"); };

            myClient.LocalState.AddStateChangedEvent(Names.TRUMP_CARD, (X, Y) => { cbxTrump.Card = Y.GetValuePlayingCard(); });

            myClient.LocalState.AddStateChangedEvent(Names.DISCARD, (X, Y) => { dscDiscard.Clear(); foreach (PlayingCard card in Y.GetValueCardCollection()) dscDiscard.AddCard(card); });

            myClient.LocalState.AddStateChangedEvent(Names.DECK_COUNT, (X, Y) => { lblCardsLeft.Text = "" + Y.GetValueInt(); if (Y.GetValueInt() == 0) cbxDeck.Card = null; });

            myClient.LocalState.AddStateChangedEvent(Names.GAME_OVER, GameOver);

            myClient.OnServerStateUpdated += (X, Y) => { if (Y == ServerState.InLobby) { isHardClose = false; this.Close(); } };

            myClient.LocalState.AddStateChangedEvent(Names.ATTACKING_PLAYER, AttackingPlayersChanged);
            myClient.LocalState.AddStateChangedEvent(Names.DEFENDING_PLAYER, AttackingPlayersChanged);

            //DebugClientView view = new DebugClientView();
            //view.SetGameState(myClient.LocalState);
            //view.Show();

            myClient.OnDisconnected += ClientDisconnected;

            myClient.OnConnected += ClientConnected;

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

            myPlayerUIs.Add(myClient.KnownPlayers[myClient.PlayerId], new PlayerUITag() { Panel = pnlMyView });

            foreach(KeyValuePair<Player, PlayerUITag> pair in myPlayerUIs)
            {
                PlayerUITag tag = pair.Value;

                if (tag.NameLabel != null)
                    tag.NameLabel.Text = pair.Key.Name;

                if (tag.CardCountLabel != null)
                    tag.CardCountLabel.Text = pair.Key.NumCards.ToString();
            }

            myClient.OnPlayerCardCountChanged += PlayerCardCountChanged;

            cplPlayersHand.Cards = myClient.Hand;
        }

        private void GameOver(object sender, StateParameter p)
        {
            string message = "Game Over!\n";

            if (myClient.LocalState.GetValueBool(Names.IS_TIE))
                message += "It's a tie!";
            else
            {
                Player durak = myClient.KnownPlayers[myClient.LocalState.GetValueByte(Names.LOSER_ID)];

                message += durak.Name + " is the Durak";
            }

            message += myClient.IsHost ? "\nPress OK to exit to lobby" : "";

            DialogResult result = MessageBox.Show(message, "Game over", MessageBoxButtons.OK);

            if (myClient.IsHost && result == DialogResult.OK)
            {
                myClient.RequestServerState(ServerState.InLobby);
            }
        }
        
        private void AttackingPlayersChanged(object sender, StateParameter p)
        {
            foreach (KeyValuePair<Player, PlayerUITag> pair in myPlayerUIs)
            {
                PlayerUITag tag = pair.Value;

                if (tag.Panel != null)
                    tag.Panel.ShowBorder = false;
            }

            Player attackingPlayer = myClient.KnownPlayers[myClient.LocalState.GetValueByte(Names.ATTACKING_PLAYER)];
            Player defendingPlayer = myClient.KnownPlayers[myClient.LocalState.GetValueByte(Names.DEFENDING_PLAYER)];
            
            myAttackingPlayerContainer = myPlayerUIs[attackingPlayer].Panel;
            myDefendingPlayerContainer = myPlayerUIs[defendingPlayer].Panel;

            myAttackingPlayerContainer.ShowBorder = true;
            myAttackingPlayerContainer.BorderColor = Color.Red;

            myDefendingPlayerContainer.ShowBorder = true;
            myDefendingPlayerContainer.BorderColor = Color.Blue;
        }

        private void ClientConnected(object sender, EventArgs e)
        {
            cplPlayersHand.Cards = myClient.Hand;
        }

        //Throw when a client disconnects
        private void ClientDisconnected(object sender, EventArgs e)
        {
            MessageBox.Show("Server has disconnected, returning to main menu");

            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (isHardClose)
            {
                myClient?.Stop();
                myServer?.Stop();

                myClient.OnDisconnected -= ClientDisconnected;

                myClient = null;
                myServer = null;
            }
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

        private void cplPlayersHand_OnCardSelected(object sender, Durak.Common.Cards.CardEventArgs e)
        {
            if (myClient != null)
                myClient.RequestMove(e.Card);
        }

        private void btnForfeit_Click(object sender, EventArgs e)
        {
            myClient.RequestMove(null);
        }

        private void btnReqHelp_Click(object sender, EventArgs e)
        {
            StateParameter param = StateParameter.Construct<bool>(Names.REQUEST_HELP, true, true);
            myClient.RequestState(param);
        }

        private void dscDiscard_Load(object sender, EventArgs e)
        {

        }
    }
}
