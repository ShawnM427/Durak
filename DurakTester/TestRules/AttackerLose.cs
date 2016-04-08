using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class AttackerLose : IGameStateRule
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
                return "Verify attacking players gets battling cards";
            }
        }

        public void ValidateState(PlayerCollection players, GameState state)
        {
            if (state.GetValueBool("attacker_forfeit"))
            {
                //add cards in battle to attackers hand
            }
        }
    }
}
