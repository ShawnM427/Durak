using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class AttackingRule : IGamePlayRule
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

        public bool IsValidMove(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if (currentState.GetValueBool(Names.IS_ATTACKING) && move.Player.PlayerId == currentState.GetValueByte(Names.ATTACKING_PLAYER))
            {
                if (move.Move == null)
                    return true;

                int round = currentState.GetValueInt(Names.CURRENT_ROUND);

                if (round == 0)
                {
                    return true;
                }
                else
                {
                    bool canPlay = false;

                    for(int index = 0; index < round; index ++)
                    {
                        if (
                            move.Move.Rank == currentState.GetValueCard(Names.ATTACKING_CARD, index).Rank ||
                            move.Move.Rank == currentState.GetValueCard(Names.DEFENDING_CARD, index).Rank)
                            canPlay = true;
                    }

                    if (!canPlay)
                        reason = "You must play a card with a rank that has already been played";

                    return canPlay;
                }
            }
            // Make sure that the attacker is playing a valid card
            return true;
        }
    }
}
