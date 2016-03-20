using Durak.Common.Cards;
using Lidgren.Network;
using System;

namespace Durak.Common
{
    /// <summary>
    /// Represents a single durak player. This basically conatins all of a single player's candle
    /// </summary>
    public class Player
    {
        public byte PlayerId { get; private set; }
        public NetConnection Connection { get; private set; }
        public virtual string Name { get; private set; }
        public Hand Hand { get; private set; }
        public int NumCards { get; set; }
        public bool IsBot { get; set; }
        public bool IsReady { get; set; }

        /// <summary>
        /// Creates a new client-side player instance
        /// </summary>
        /// <param name="tag">The client tag</param>
        /// <param name="playerId">The player's ID</param>
        public Player(ClientTag tag, byte playerId)
        {
            PlayerId = playerId;
            Name = tag.Name;
            IsBot = false;
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
            Hand = new Hand();
        }
    }
}