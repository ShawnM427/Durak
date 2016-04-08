using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class VerifyDuelWin 
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
                return "Verify winner of duel";
            }
        }
        public void WinDuel(PlayerCollection players, GameState state, GameMove move, GameState currentState)
        {
            if (move.Move == null)
            {
                state.Set("defender_forfeit", true);
            }

            else if (move.Move == null)
            {
                state.Set("attacker_forfeit", true);
            }

            else if (state.GetValueInt("current_round") > 6)
            {
                state.Set("attacker_forfeit", true);
            }
        }
    }
}
