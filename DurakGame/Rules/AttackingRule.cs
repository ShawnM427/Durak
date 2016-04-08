using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class AttackingRule
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
                return "Rule for Atacker";
            }
        }
        public bool AttackRule(PlayerCollection players, GameState state, GameMove move, GameState currentState)
        {
            if (currentState.GetValueCardSuit("attacking_card_suit") == currentState.GetValueCardSuit("defending_card_suit") && currentState.GetValueCardRank("attacking_card_rank") > currentState.GetValueCardRank("defending_card_rank"))
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
