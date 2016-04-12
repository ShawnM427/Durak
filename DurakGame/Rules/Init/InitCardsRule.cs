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
    class InitCardsRule : IGameInitRule
    {
        public bool IsEnabled
        {
            get;
            set;
        }

        public int Priority
        {
            get
            {
                return 50;
            }
        }

        public string ReadableName
        {
            get { return "Deal cards"; }
        }

        public void InitState(PlayerCollection players, GameState state)
        {
            // Get the deck
            Deck toDrawFrom = new Deck(state.GetValueCardCollection(Names.DECK));

            // Iterate over all players
            for (byte index = 0; index < players.Count; index++)
            {
                // If the player exists
                if (players[index] != null)
                {
                    // Add 6 cards to their hand
                    for (int cIndex = 0; cIndex < 6; cIndex++)
                        players[index].Hand.Add(toDrawFrom.Draw());
                }
            }

            // Update the deck and the count in the state
            state.Set<CardCollection>(Names.DECK, toDrawFrom.Cards);
            state.Set<int>(Names.DECK_COUNT, toDrawFrom.CardsInDeck);
        }
    }
}
