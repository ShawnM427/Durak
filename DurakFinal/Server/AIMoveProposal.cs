using Durak.Common.Cards;

namespace Durak.Server
{
    public struct AIMoveProposal
    {
        public float Confidence { get; set; }

        public PlayingCard Move { get; set; }
    }
}