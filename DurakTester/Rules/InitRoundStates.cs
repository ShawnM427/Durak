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
            state.Set("IsAttacking", true);

            state.Set<byte>("attacking_player_id", 0);
            state.Set<byte>("defending_player_id", 1);

            state.Set<CardCollection>("source_deck", new Deck(CardRank.Six, CardRank.King).GetCards(), true);

            for (int index = 0; index < 6; index ++)
            {
                state.Set<PlayingCard>("defending_card", index, null);
                state.Set<PlayingCard>("attacking_card", index, null);
            }
        }
    }
}
