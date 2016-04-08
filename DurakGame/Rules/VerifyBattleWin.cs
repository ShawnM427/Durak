using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class VerifyBattleWin : IGameStateRule
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
                return "Check won battle";
            }
        }

        public void ValidateState(PlayerCollection players, GameState currentState)
        {
            if (currentState.GetValueCardSuit("defending_card_suit") == currentState.GetValueCardSuit("attacking_card_suit") && currentState.GetValueCardRank("attacking_card_rank") >= currentState.GetValueCardRank("defending_card_rank"))
            {
                currentState.Set("current_round", (byte)(currentState.GetValueByte("current_round") + 1));
            }

        }
    }
}
