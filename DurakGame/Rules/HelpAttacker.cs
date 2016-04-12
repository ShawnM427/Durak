using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class HelpAttacker : IGamePlayRule
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
                return "Verify attacking player needs help";
            }
        }

        public bool IsValidMove(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if (currentState.GetValueByte("attacking_player_id") == move.Player.PlayerId || currentState.GetValueBool("player_req_help"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
