using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Common;

namespace DurakTester.Rules
{
    public class StateRule : IGameStateRule
    {
        public bool Enabled
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
                if (state.GetValueCard("attacking_card", state.GetValueInt("current_round")) != null)
                {
                    state.Set("IsAttacking", false);
                }
            }
            else
            {
                if (state.GetValueCard("defending_card", state.GetValueInt("current_round")) != null)
                {
                    state.Set("IsAttacking", true);
                    state.Set("current_round", state.GetValueInt("current_round") + 1);
                }
            }
        }
    }
}
