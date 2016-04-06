using Lidgren.Network;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Durak.Common
{
    /// <summary>
    /// Stores a list of all players for a game, this is used to look up
    /// players by their ID's
    /// </summary>
    public sealed class PlayerCollection : IEnumerable<Player>
    {
        /// <summary>
        /// Defines the default player count for new player collections
        /// </summary>
        public const int DEFAULT_PLAYER_COUNT = 4;

        /// <summary>
        /// Internal array for this collection
        /// </summary>
        private Player[] myPlayers;

        /// <summary>
        /// Gets or sets the player with the given ID
        /// </summary>
        /// <param name="playerId">The ID of the player to get/set</param>
        /// <returns>The player with the given player ID</returns>
        public Player this[byte playerId]
        {
            get { return myPlayers[playerId]; }
            set { myPlayers[playerId] = value; }
        }

        /// <summary>
        /// Gets the player with the given network connection
        /// </summary>
        /// <param name="connection">The connecion of the player to get</param>
        /// <returns>The player with the given connection</returns>
        public Player this[NetConnection connection]
        {
            get
            {
                // Iterate and compare connections
                for(int index = 0; index < myPlayers.Length; index ++)
                    if (myPlayers[index] != null && myPlayers[index].Connection == connection)
                        return myPlayers[index];

                return null;
            }
        }

        /// <summary>
        /// Gets the number of player slots in this collection
        /// </summary>
        public int Count
        {
            get { return myPlayers.Length; }
        }

        /// <summary>
        /// Gets the number of not-null players
        /// </summary>
        public byte PlayerCount
        {
            get { return (byte)myPlayers.Where(X => X != null).Count(); }
        }

        /// <summary>
        /// Creates a new player collection with the default number of players
        /// </summary>
        public PlayerCollection() : this(DEFAULT_PLAYER_COUNT)
        { }

        /// <summary>
        /// Creates a new player collection with the given number of players
        /// </summary>
        /// <param name="numPlayers">The number of players in this collection</param>
        public PlayerCollection(int numPlayers)
        {
            myPlayers = new Player[numPlayers];
        }

        /// <summary>
        /// Gets the first available ID for this player collection, or -1 if no slots are open
        /// </summary>
        /// <returns>The next available ID, or -1 if no slot is open</returns>
        public int GetNextAvailableId()
        {
            // Gets the next available ID
            for (int index = 0; index < myPlayers.Length; index++)
                if (myPlayers[index] == null)
                    return index;

            return -1;
        }

        /// <summary>
        /// Remove the given player from this player collection
        /// </summary>
        /// <param name="player">The player to remove</param>
        public void Remove(Player player)
        {
            // Make sure the player exists, then remove him
            if (player != null)
                for (int index = 0; index < myPlayers.Length; index++)
                    if (myPlayers[index] != null && myPlayers[index].Equals(player))
                    {
                        myPlayers[index] = null;
                        return;
                    }
        }

        /// <summary>
        /// Clears this player collection
        /// </summary>
        internal void Clear()
        {
            for (int index = 0; index < myPlayers.Length; index++)
                myPlayers[index] = null;
        }

        /// <summary>
        /// Gets the iterator instance to enumerate over all the players in this collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Player> GetEnumerator()
        {
            return myPlayers.Where(x => x != null).GetEnumerator();
        }

        /// <summary>
        /// Gets the iterator instance to enumerate over all the players in this collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return myPlayers.Where(x => x != null).GetEnumerator();
        }
    }
}
