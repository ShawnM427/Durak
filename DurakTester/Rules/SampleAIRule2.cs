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

                foreach(KeyValuePair<PlayingCard, float> pair in proposals)
                {
                    if(pair.Key.Suit == attackingSuit || pair.Key.Suit == trumpSuit)
                    {
                        proposals[pair.Key] += 0.25f;
                    }
                }
            }
        }
    }
}
