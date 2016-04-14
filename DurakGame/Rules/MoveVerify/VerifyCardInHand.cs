using Durak.Common;
using Durak.Server;

namespace DurakTester.Rules
{
    /// <summary>
    /// Represents the rule that verifies that the card is in the player's hand
    /// </summary>
    public class VerifyCardInHand : IGamePlayRule
    {
        /// <summary>
        /// Gets or sets whether this rule is enabled
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the human readable name for this rule
        /// </summary>
        public string ReadableName
        {
            get
            {
                return "Verify card in Hand";
            }
        }

        /// <summary>
        /// Determines if a given move is valid
        /// </summary>
        /// <param name="server">The server to excecute on</param>
        /// <param name="move">The move being played</param>
        /// <param name="reason">The reason that the move is invalid</param>
        /// <returns>True if the room is valid, false if otherwise</returns>
        public bool IsValidMove(GameServer server, GameMove move, ref string reason)
        {
            if (move.Move == null)
                return true;

            if (!move.Player.Hand.Contains(move.Move))
            {
                reason = "Card is not in players hand";
                return false;
            }

            return true;
        }
    }
}
