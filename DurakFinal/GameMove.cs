using Durak.Common.Cards;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common
{
    /// <summary>
    /// Represents a single move made by a player
    /// </summary>
    public struct GameMove
    {
        private Player myPlayer;
        private PlayingCard myMove;

        /// <summary>
        /// Gets the player that performed this move
        /// </summary>
        public Player Player
        {
            get { return myPlayer; }
        }
        /// <summary>
        /// Gets the card that the player would like to play
        /// </summary>
        public PlayingCard Move
        {
            get { return myMove; }
        }

        /// <summary>
        /// Writes this game move to the given network packet
        /// </summary>
        /// <param name="outMessage">The message to write to</param>
        public void WriteToPacket(NetOutgoingMessage outMessage)
        {
            outMessage.Write(myPlayer.PlayerId);

            outMessage.Write((byte)myMove.Rank);
            outMessage.Write((byte)myMove.Suit);
        }

        /// <summary>
        /// Reads a game move from the given network packet
        /// </summary>
        /// <param name="inMessage">The message to read from</param>
        /// <param name="players">The player collection to get the player from</param>
        /// <returns>The Game Move read from the packet</returns>
        public static GameMove ReadFromPacket(NetIncomingMessage inMessage, PlayerCollection players)
        {
            GameMove result;

            byte playerId = inMessage.ReadByte();

            int moveRank = inMessage.ReadByte();
            int moveSuit = inMessage.ReadByte();

            result.myPlayer = players[playerId];
            result.myMove = new PlayingCard((CardRank)moveRank, (CardSuit)moveSuit);

            return result;
        }
    }
}
