using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class PlayerCanNotMove : IGamePlayRule
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

        public bool IsValidMove(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if (currentState.GetValueBool("IsAttacking") && move.Move == null)
            {
                currentState.Set("attacker_forfeit", true);
                return true;
            }

            else
            {
                currentState.Set("defender_forfeit", true);
                return true;

            }
        }
    }
}
