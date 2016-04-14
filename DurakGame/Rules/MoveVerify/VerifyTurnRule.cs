using Durak.Common;
using Durak.Server;

namespace DurakGame.Rules
{
    /// <summary>
    /// Rule to verify that it is the players turn to play the card
    /// </summary>
    public class VerifyTurnRule : IGamePlayRule
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
                return "Verify the player's turn";
            }
        }

        /// <summary>
        /// Determines if a given move is valid
        /// </summary>
        /// <param name="players">The server's player collection</param>
        /// <param name="move">The move being played</param>
        /// <param name="currentState">The server's game state</param>
        /// <param name="reason">The reason that the move is invalid</param>
        /// <returns>True if the room is valid, false if otherwise</returns>
        public bool IsValidMove(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if (currentState.GetValueBool(Names.IS_ATTACKING))
            {
                if (move.Player.PlayerId == currentState.GetValueByte(Names.ATTACKING_PLAYER))
                    return true;

                if (currentState.GetValueBool(Names.REQUEST_HELP) && move.Player.PlayerId != currentState.GetValueByte(Names.DEFENDING_PLAYER))
                    return true;
            }
            else
            {
                if (move.Player.PlayerId == currentState.GetValueByte(Names.DEFENDING_PLAYER))
                    return true;

            }

            reason = "It is not your turn to " + (currentState.GetValueBool(Names.IS_ATTACKING) ? "attack." : "defend.");
            return false;
        }
    }
}
