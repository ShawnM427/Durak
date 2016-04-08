using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class AttackerLose
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

        public void AttackerGetsCards(PlayerCollection players, GameState currentState)
        {
            if (currentState.GetValueBool("attacker_forfeit"))
            {
                //add cards in battle to attackers hand
            }
        }
    }
}
