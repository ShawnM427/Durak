using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Common;

namespace DurakGame.Rules
{
    public class StateRule : IGameStateRule
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
                return "Update atacking and defending";
            }
        }

        public void ValidateState(PlayerCollection players, GameState state)
        {
            if (state.GetValueBool("IsAttacking"))
            {
                if (state.GetValueCard("attacking_card", state.GetValueByte("current_round")) != null)
                {
                    state.Set("IsAttacking", false);
                }
            }
            else
            {
                if (state.GetValueCard("defending_card", state.GetValueByte("current_round")) != null)
                {
                    state.Set("IsAttacking", true);
                    state.Set("current_round", state.GetValueByte("current_round") + 1);
                }
            }
        }
    }
}
