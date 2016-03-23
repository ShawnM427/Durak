using Durak.Common;
using Durak.Common.Cards;
using Durak.Server;

namespace DurakTester.Rules
{
    class SampleInitRule : IGameInitRule
    {
        public bool Enabled
        {
            get;
            set;
        }

        public string ReadableName
        {
            get { return "DO NOT KEEP RULE THIS IS A SAMPLE"; }
        }

        public void InitState(PlayerCollection players, GameState state)
        {
            for (byte index = 0; index < 4; index++)
                if (players[index] != null)
                {
                    for (int cIndex = 0; cIndex < 10; cIndex++)
                        //players[index].Hand.Add(new PlayingCard((CardRank)cIndex, CardSuit.Clubs));
                        players[index].Hand.Add(new Deck(state.GetValueCardCollection("source_deck")).Draw());
                }
        }
    }
}
