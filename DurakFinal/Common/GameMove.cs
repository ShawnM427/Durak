using Durak.Common.Cards;
using Lidgren.Network;

namespace Durak.Common
{
    /// <summary>
    /// Represents a single move made by a player
    /// </summary>
    public struct GameMove
    {
        /// <summary>
        /// Stores the player that made the move
        /// </summary>
        private Player myPlayer;
        /// <summary>
        /// Stores the card to be played
        /// </summary>
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
            // Just transfer that name
            outMessage.Write(myPlayer.PlayerId);

            // Write the card's info
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

            // Get the player ID
            byte playerId = inMessage.ReadByte();

            // Get the playing card
            int moveRank = inMessage.ReadByte();
            int moveSuit = inMessage.ReadByte();

            // Build the result
            result.myPlayer = players[playerId];
            result.myMove = new PlayingCard((CardRank)moveRank, (CardSuit)moveSuit);
            result.myMove.FaceUp = true;

            return result;
        }
    }
}
