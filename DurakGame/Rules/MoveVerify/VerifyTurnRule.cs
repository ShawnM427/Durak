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
        /// <param name="server">The server to excecute on</param>
        /// <param name="move">The move being played</param>
        /// <param name="reason">The reason that the move is invalid</param>
        /// <returns>True if the room is valid, false if otherwise</returns>
        public bool IsValidMove(GameServer server, GameMove move, ref string reason)
        {
            if (server.GameState.GetValueBool(Names.IS_ATTACKING))
            {
                if (move.Player.PlayerId == server.GameState.GetValueByte(Names.ATTACKING_PLAYER))
                    return true;

                if (server.GameState.GetValueBool(Names.REQUEST_HELP) && move.Player.PlayerId != server.GameState.GetValueByte(Names.DEFENDING_PLAYER))
                    return true;
            }
            else
            {
                if (move.Player.PlayerId == server.GameState.GetValueByte(Names.DEFENDING_PLAYER))
                    return true;

            }

            reason = "It is not your turn to " + (server.GameState.GetValueBool(Names.IS_ATTACKING) ? "attack." : "defend.");
            return false;
        }
    }
}
