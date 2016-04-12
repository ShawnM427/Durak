using Durak.Common;
using Durak.Server;

namespace DurakGame.Rules
{
    public class VerifyTurnRule : IGamePlayRule
    {
        public bool IsEnabled
        {
            get;
            set;
        }

        public string ReadableName
        {
            get
            {
                return "Verify the player's turn";
            }
        }

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
