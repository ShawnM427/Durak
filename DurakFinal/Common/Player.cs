using System;
using Durak.Common.Cards;
using Durak.Server;
using Lidgren.Network;

namespace Durak.Common
{
    /// <summary>
    /// Represents a single card game player. This basically conatins all of a single player's data
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets this player's player ID
        /// </summary>
        public byte PlayerId { get; private set; }
        /// <summary>
        /// Gets this player's connection to the server
        /// </summary>
        public NetConnection Connection { get; private set; }
        /// <summary>
        /// Gets the player's name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the player's current hand (not visible client side)
        /// </summary>
        public CardCollection Hand { get; private set; }
        /// <summary>
        /// Gets or sets the number of cards in the player's hand. This is the only way to see number of 
        /// cards clientside
        /// </summary>
        public int NumCards { get; set; }
        /// <summary>
        /// Gets or sets whether or not this player is a bot
        /// </summary>
        public bool IsBot { get; set; }
        /// <summary>
        /// Gets or sets whether this player is ready for the game
        /// </summary>
        public bool IsReady { get; set; }
        /// <summary>
        /// Gets or sets whether this player instance is a host player
        /// </summary>
        public bool IsHost { get; set; }

        /// <summary>
        /// Invoked when a card has been added to the player's hand
        /// </summary>
        public event EventHandler<PlayingCard> OnCardAddedToHand;
        /// <summary>
        /// Invoked when a card has been removed from the player's hand
        /// </summary>
        public event EventHandler<PlayingCard> OnCardRemovedFromHand;

        /// <summary>
        /// Creates a new player isntance
        /// </summary>
        /// <param name="playerId">The ID of this player</param>
        /// <param name="name">This player's name</param>
        /// <param name="isBot">Whether or not this player is a bot</param>
        public Player(byte playerId, string name, bool isBot)
        {
            PlayerId = playerId;
            Name = name;
            IsBot = isBot;
            Hand = new CardCollection();

            Hand.OnCardAdded += CardAdded;
            Hand.OnCardRemoved += CardRemoved;
        }

        /// <summary>
        /// Creates a new client-side player instance
        /// </summary>
        /// <param name="tag">The client tag</param>
        /// <param name="playerId">The player's ID</param>
        public Player(ClientTag tag, byte playerId) : this(playerId, tag.Name, false)
        {
        }

        /// <summary>
        /// Creates a new client-side player instance
        /// </summary>
        /// <param name="tag">The client tag</param>
        /// <param name="connection">The connection for this client</param>
        /// <param name="playerId">The player's ID</param>
        public Player(ClientTag tag, NetConnection connection, byte playerId) 
            : this(tag, playerId)
        {
            Connection = connection;
        }

        /// <summary>
        /// Invoked when the player's hand has lost a card
        /// </summary>
        /// <param name="sender">The object that invoked the event (the hand)</param>
        /// <param name="e">The card event arguments for the event</param>
        private void CardRemoved(object sender, CardEventArgs e)
        {
            if (OnCardRemovedFromHand != null)
                OnCardRemovedFromHand.Invoke(this, e.Card);
        }

        /// <summary>
        /// Invoked when the player's hand has gained a card
        /// </summary>
        /// <param name="sender">The object that invoked the event (the hand)</param>
        /// <param name="e">The card event arguments for the event</param>
        private void CardAdded(object sender, CardEventArgs e)
        {
            if (OnCardAddedToHand != null)
                OnCardAddedToHand.Invoke(this, e.Card);
        }

        /// <summary>
        /// Overrides the equality check for this object
        /// </summary>
        /// <param name="obj">The object to check equality against</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return (obj is Player && (obj as Player).PlayerId == PlayerId);
        }

        /// <summary>
        /// Overrides the hash code for this object, returning a semi-unique value
        /// </summary>
        /// <returns>A semi-unique value</returns>
        public override int GetHashCode()
        {
            return PlayerId;
        }

        /// <summary>
        /// Encodes this instance to a network message
        /// </summary>
        /// <param name="msg">The message to encode to</param>
        public void Encode(NetOutgoingMessage msg)
        {
            msg.Write(PlayerId);
            msg.Write(Name);
            msg.Write(Hand.Count);
            msg.Write(IsBot);
            msg.Write(IsReady);
            msg.Write(IsHost);
            msg.WritePadBits();
        }

        /// <summary>
        /// Encodes this instance to a network message
        /// </summary>
        /// <param name="msg">The message to encode to</param>
        public void Decode(NetIncomingMessage msg)
        {
            PlayerId = msg.ReadByte();
            Name = msg.ReadString();
            NumCards = msg.ReadInt32();
            IsBot = msg.ReadBoolean();
            IsReady = msg.ReadBoolean();
            IsHost = msg.ReadBoolean();
            msg.ReadPadBits();
        }

        /// <summary>
        /// Creates a new clone of this player instance
        /// </summary>
        /// <returns>A clone of this player</returns>
        public Player Clone()
        {
            Player result = new Player(PlayerId, Name, true);
            return result;
        }

        /// <summary>
        /// Creates a bot player for this player
        /// </summary>
        /// <param name="gameServer">The server that the bot exists on</param>
        /// <param name="difficulty">The bots difficulty level from 0-1</param>
        /// <returns>A bot for this player</returns>
        public BotPlayer CreateBot(GameServer gameServer, float difficulty)
        {
            Player player = this.Clone();
            return new BotPlayer(gameServer, player, difficulty);
        }
    }
}