using Durak.Server;
using System.Linq;
using Durak.Common;
using Durak.Common.Cards;
using System.Collections.Generic;

namespace DurakTester.Rules
{
    public class SampleAIRule2 : IAIRule
    {
        public void Propose(Dictionary<PlayingCard, float> proposals, GameState state, CardCollection hand)
        {
            if (state.GetValueBool("IsAttacking"))
            {
                
            }
            else
            {
                CardSuit attackingSuit = state.GetValueCard("attacking_card", state.GetValueInt("current_round")).Suit;
                CardSuit trumpSuit = state.GetValueCardSuit("trump_suit");

                PlayingCard[] keys = proposals.Keys.ToArray();

                foreach(PlayingCard key in keys)
                {
                    if(key.Suit == attackingSuit || key.Suit == trumpSuit)
                    {
                        proposals[key] += 0.25f;
                    }
                }
            }
        }
    }
}
