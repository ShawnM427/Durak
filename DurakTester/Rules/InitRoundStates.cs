using System;
using Durak.Common;
using Durak.Server;
using Durak.Common.Cards;

namespace DurakTester.Rules
{
    public class InitRoundStates : IGameInitRule
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
                return "Initialize battle slots";
            }
        }

        public void InitState(PlayerCollection players, GameState state)
        {
            for(int index = 0; index < 6; index ++)
            {
                state.Set<PlayingCard>("defending_card", index, null);
                state.Set<PlayingCard>("attacking_card", index, null);
            }
        }
    }
}
