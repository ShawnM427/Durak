using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Common;

namespace Durak.Server.Rules
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

        public bool IsValidMove(GameMove move, GameState currentState, ref string reason)
        {
           // if (currentState.GetValueByte("attacking_player_id") == move.Player.PlayerId || currentState.GetValueBool("player_req_help"))
                return true;
            //else
            {
                reason = "It is not the player's turn to attack";
                return false;
            }
        }
    }
}
