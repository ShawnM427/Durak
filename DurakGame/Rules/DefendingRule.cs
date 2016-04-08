using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class DefendingRule 
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
                return "Defending Rule";
            }
        }
        public bool DefendRule(PlayerCollection players, GameState state, GameState currentState)
        {
            if (currentState.GetValueCardSuit("defending_card_suit") == currentState.GetValueCardSuit("attacking_card_suit") && currentState.GetValueCardRank("defending_card_rank") == currentState.GetValueCardRank("attacking_card_rank"))
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
