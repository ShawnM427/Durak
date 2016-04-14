using Durak.Common;
using Durak.Common.Cards;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class DefendingRule : IGamePlayRule
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

        public bool IsValidMove(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if (move.Move == null)
                return true;

            if (!currentState.GetValueBool(Names.IS_ATTACKING))
            {
                if (currentState.GetValueByte(Names.DEFENDING_PLAYER) == move.Player.PlayerId)
                {
                    PlayingCard attacking = currentState.GetValueCard(Names.ATTACKING_CARD, currentState.GetValueInt(Names.CURRENT_ROUND));
                    PlayingCard trump = currentState.GetValueCard(Names.TRUMP_CARD);

                    if (move.Move.Rank > attacking.Rank)
                    {
                        if (move.Move.Suit == attacking.Suit || move.Move.Suit == trump.Suit)
                        {
                            return true;
                        }
                        else
                        {
                            reason = "You must play a card of a higher rank of the same suit, or a trump card";
                            return false;
                        }
                    }
                    else if (move.Move.Suit == trump.Suit && attacking.Suit != trump.Suit)
                        return true;
                    else
                    {
                        reason = "You must play a card of a higher rank";
                        return false;
                    }
                }
                else
                {
                    reason = "It is not your turn to defend";
                    return false;
                }
            }
            else
                return true;
        }
    }
}
