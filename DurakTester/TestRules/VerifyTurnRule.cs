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
                return "Verify attacking players";
            }
        }

        public bool IsValidMove(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if (currentState.GetValueBool("IsAttacking"))
            {
                if (move.Player.PlayerId == currentState.GetValueByte("attacking_player_id"))
                    return true;

                if (currentState.GetValueBool("player_req_help") && move.Player.PlayerId != currentState.GetValueByte("defending_player_id"))
                    return true;
            }
            else
            {
                if (move.Player.PlayerId == currentState.GetValueByte("defending_player_id"))
                    return true;

            }

            return false;
        }
    }
}
