using Durak.Server;
using System.Linq;
using Durak.Common;
using Durak.Common.Cards;
using System.Collections.Generic;

namespace DurakTester.Rules
{
    public class SampleAIRule : IAIRule
    {
        public void Propose(Dictionary<PlayingCard, float> proposals, GameState state, CardCollection hand)
        {

            if (state.GetValueBool("IsAttacking"))
            {

            }
            else
            {
                for(int index = 0; index < proposals.Count; index ++) 
                {
                    proposals[proposals.Keys.ElementAt(index)] += ((int)proposals.Keys.ElementAt(index).Suit / 56.0f);
                }
            }
        }
    }
}
