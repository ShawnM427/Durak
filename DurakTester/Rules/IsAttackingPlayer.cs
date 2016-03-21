using Durak.Common;
using Durak.Server;

namespace DurakTester.Rules
{
    public class IsAttackingPlayer : IGamePlayRule
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
            byte attackingPlayerId = currentState.GetValueByte("attacking_player_id");

            if (attackingPlayerId == move.Player.PlayerId || currentState.GetValueBool("player_req_help"))
            {
                attackingPlayerId++;

                if (attackingPlayerId >= players.PlayerCount)
                    attackingPlayerId = (byte)0;

                currentState.Set("attacking_player_id", attackingPlayerId);

                return true;
            }
            else
            {
                reason = "It is not the player's turn to attack";
                return false;
            }
        }
    }
}
