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
    /// <summary>
    /// Represents the main game form for Durak
    /// </summary>
    public partial class frmDurakGame : Form
    {
        /// <summary>
        /// A struct to hold the ui items for a player
        /// </summary>        
        private struct PlayerUITag
        {
            /// <summary>
            /// Gets or sets the panel that this player is sitting on
            /// </summary>
            public BorderPanel Panel;
            /// <summary>
            /// Gets or sets the label that shows the player's name
            /// </summary>
            public Label NameLabel;
            /// <summary>
            /// Gets or sets the label that shows the player's card count
            /// </summary>
            public Label CardCountLabel;
            /// <summary>
            /// Gets or sets the card box that show's this player's card box
            /// </summary>
            public CardBox CardBox;
        }

        /// <summary>
        /// Stores the game client for this game
        /// </summary>
        private GameClient myClient;
        /// <summary>
        /// Stores the game server for this game, if this person is host
        /// </summary>
        private GameServer myServer;

        /// <summary>
        /// Stores the list of player UI tags
        /// </summary>
        private Dictionary<Player, PlayerUITag> myPlayerUIs;
        
        /// <summary>
        /// We use this to track if we are hard closing
        /// </summary>
        bool isHardClose = true;

        /// <summary>
        /// Creates a new Game client form
        /// </summary>
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

        /// <summary>
        /// Sets the server for this game
        /// </summary>
        /// <param name="server">The server for this host</param>
        public void SetServer(GameServer server)
        {
            myServer = server;
        }

        /// <summary>
        /// Sets the Game client for this game
        /// </summary>
        /// <param name="client">The client to represent</param>
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


            myClient.LocalState.AddStateChangedEvent(Names.TRUMP_CARD_USED, TrumpPickedUp);


            myClient.LocalState.AddStateChangedEvent(Names.DISCARD, (X, Y) => { dscDiscard.Clear(); foreach (PlayingCard card in Y.GetValueCardCollection()) dscDiscard.AddCard(card); });

            myClient.LocalState.AddStateChangedEvent(Names.DECK_COUNT, (X, Y) => { lblCardsLeft.Text = "" + Y.GetValueInt(); if (Y.GetValueInt() == 0) cbxDeck.Card = null; });

            myClient.LocalState.AddStateChangedEvent(Names.GAME_OVER, GameOver);

            myClient.OnServerStateUpdated += (X, Y) => { if (Y == ServerState.InLobby) { isHardClose = false; this.Close(); } };

            myClient.LocalState.AddStateChangedEvent(Names.REQUEST_HELP, (X, Y) => 
            {
                btnReqHelp.BackColor = Y.GetValueBool() ? Color.Yellow : Color.DarkGreen;
                lblAttackerReqHelp.Text = Y.GetValueBool() ? "Attacker Requesting Help" : "";
            } );

            myClient.OnPlayerChat += ReceivedChat;

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

        private void TrumpPickedUp(object sender, StateParameter p)
        {
            cbxTrump.Enabled = false;
        }

        /// <summary>
        /// Invoked when a chat message has been received
        /// </summary>
        /// <param name="sender">The object to invoked the event (the GameClient)</param>
        /// <param name="player">The player that said the message (null for server)</param>
        /// <param name="message">The message that was sent</param>
        private void ReceivedChat(object sender, Player player, string message)
        {
            int start = rtbChatLog.Text.Length;
            rtbChatLog.AppendText(player == null ? "[Server]" : "[" + player.Name + "]");
            rtbChatLog.Select(start, rtbChatLog.Text.Length - start);
            rtbChatLog.SelectionColor = player == null ? Color.Orange : player.IsHost ? Color.Yellow : Color.LightBlue;
            rtbChatLog.Select(rtbChatLog.Text.Length, 0);
            rtbChatLog.SelectionColor = Color.White;

            rtbChatLog.AppendText(" " + message + "\n");
            rtbChatLog.Select(rtbChatLog.Text.Length - 1, 1);
            rtbChatLog.ScrollToCaret();
        }

        /// <summary>
        /// Invoked when the send chat button is pressed
        /// </summary>
        /// <param name="sender">The button to invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtMessage.Text))
            {
                myClient.SendChatMessage(txtMessage.Text);
                txtMessage.Text = "";
            }
        }

        /// <summary>
        /// Invoked when the game is over
        /// </summary>
        /// <param name="sender">The that raised the event (The GameState)</param>
        /// <param name="p">The state parameter that was updated</param>
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

        /// <summary>
        /// Invoked when the attacking or defending player has changed
        /// </summary>
        /// <param name="sender">The that raised the event (The GameState)</param>
        /// <param name="p">The state parameter that was updated</param>
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

            BorderPanel myAttackingPlayerContainer = myPlayerUIs[attackingPlayer].Panel;
            BorderPanel myDefendingPlayerContainer = myPlayerUIs[defendingPlayer].Panel;

            myAttackingPlayerContainer.ShowBorder = true;
            myAttackingPlayerContainer.BorderColor = Color.Red;

            myDefendingPlayerContainer.ShowBorder = true;
            myDefendingPlayerContainer.BorderColor = Color.Blue;
        }

        /// <summary>
        /// Invoked when the client has successfully connected to the server
        /// </summary>
        /// <param name="sender">The object that invoked the event (the GameClient)</param>
        /// <param name="e">The blank event arguments</param>
        private void ClientConnected(object sender, EventArgs e)
        {
            cplPlayersHand.Cards = myClient.Hand;
        }

        /// <summary>
        /// Invoked when the client has disconnected from the server
        /// </summary>
        /// <param name="sender">The object that invoked the event (the GameClient)</param>
        /// <param name="e">The blank event arguments</param>
        private void ClientDisconnected(object sender, EventArgs e)
        {
            MessageBox.Show("Server has disconnected, returning to main menu");

            this.Close();
        }

        /// <summary>
        /// Overrides the OnClosing event for this form, will kill the client and server if this is a hard close
        /// </summary>
        /// <param name="e">The event arguments that lets us cancel the close</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (isHardClose)
            {
                myClient.OnDisconnected -= ClientDisconnected;
                myClient.OnConnected -= ClientConnected;

                myClient.Disconnect();
                myServer?.Stop();
            }
        }

        /// <summary>
        /// Invoked when the player's card count has changed
        /// </summary>
        /// <param name="player">The player whose card count has cahnged</param>
        /// <param name="newCardCount">The player's new card count</param>
        private void PlayerCardCountChanged(Durak.Common.Player player, int newCardCount)
        {
            if (player.PlayerId != myClient.PlayerId)
            {
                PlayerUITag tag = myPlayerUIs[player];

                tag.CardCountLabel.Text = newCardCount.ToString();
            }
        }

        /// <summary>
        /// Invoked when the Card Picker has had a card selected
        /// </summary>
        /// <param name="sender">The object that invoked the event (the Card Picker)</param>
        /// <param name="e">The card event arguments for the event, containing the card that was selected</param>
        private void cplPlayersHand_OnCardSelected(object sender, Durak.Common.Cards.CardEventArgs e)
        {
            if (myClient != null)
                myClient.RequestMove(e.Card);
        }

        /// <summary>
        /// Invoked when the forfeit button has been clicked
        /// </summary>
        /// <param name="sender">The object that raised the event (the button)</param>
        /// <param name="e">The blank event arguments</param>
        private void btnForfeit_Click(object sender, EventArgs e)
        {
            myClient.RequestMove(null);
        }

        /// <summary>
        /// Invoked when the request help button is clicked
        /// </summary>
        /// <param name="sender">The object that raised the event (the button)</param>
        /// <param name="e">The blank event arguments</param>
        private void btnReqHelp_Click(object sender, EventArgs e)
        {
            StateParameter param = StateParameter.Construct<bool>(Names.REQUEST_HELP, true, true);
            myClient.RequestState(param);
        }
    }
}
