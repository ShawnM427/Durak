using Durak.Common.Cards;
using Lidgren.Network;
using System.Linq;

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
        /// Creates a new game move with the given playerID
        /// </summary>
        /// <param name="player">The player to make this move</param>
        /// <param name="move">The card to move</param>
        public GameMove(Player player, PlayingCard move) : this()
        {
            myPlayer = player;
            myMove = move;
        }

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

            outMessage.Write(myMove != null);
            outMessage.WritePadBits();

            // Write the card's info
            if (myMove != null)
            {
                outMessage.Write((byte)myMove.Rank);
                outMessage.Write((byte)myMove.Suit);
            }            
        }

        /// <summary>
        /// Reads a game move from the given network packet
        /// </summary>
        /// <param name="inMessage">The message to read from</param>
        /// <param name="players">The player collection to get the player from</param>
        /// <returns>The Game Move read from the packet</returns>
        public static GameMove ReadFromPacket(NetIncomingMessage inMessage, PlayerCollection players)
        {
            GameMove result = new GameMove();

            // Get the player ID
            byte playerId = inMessage.ReadByte();
            
            // Build the result
            result.myPlayer = players[playerId];
            
            bool hasValue = inMessage.ReadBoolean();
            inMessage.ReadPadBits();

            // Read if not null
            if (hasValue)
            {
                // Get the playing card
                int moveRank = inMessage.ReadByte();
                int moveSuit = inMessage.ReadByte();

                result.myMove = new PlayingCard((CardRank)moveRank, (CardSuit)moveSuit);
                result.myMove.FaceUp = true;
            }

            return result;
        }
    }
}
