using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Common;

namespace DurakGame.Rules
{
    class UpdateCards : IGameStateRule
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
                return "Cards update";
            }
        }

        public void ValidateState(PlayerCollection players, GameState currentState)
        {
            currentState.GetValueCardCollection("source_deck");
        }
    }
}
