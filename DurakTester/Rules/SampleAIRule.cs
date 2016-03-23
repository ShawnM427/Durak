using Durak.Server;
using System.Linq;
using Durak.Common;
using Durak.Common.Cards;

namespace DurakTester.Rules
{
    public class SampleAIRule : IAIRule
    {
        public AIMoveProposal Propose(GameState state, CardCollection hand)
        {
            return new AIMoveProposal(hand.FirstOrDefault(), 0.01f);
        }
    }
}
