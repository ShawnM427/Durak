using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class EmptyTalon
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
                return "Verify Talon is empty";
            }
        }

        public bool Talon(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if ()
            return false;
        }

    }
}
