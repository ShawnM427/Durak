﻿using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class EmptyTalon
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
                return "Verify Talon is empty";
            }
        }

        public bool Talon(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if (currentState.GetValueInt("cards_in_deck") == 0)
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
