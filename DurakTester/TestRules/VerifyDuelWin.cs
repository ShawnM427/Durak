using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class VerifyDuelWin : IGameStateRule
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

        public void ValidateState(PlayerCollection players, GameState state)
        {
            if (state.GetValueByte("current_round") > 6)
            {
                state.Set("attacker_forfeit", true);
            }
        }
    }
}
