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

        public void InitState(GameServer server)
        {
            // Get the deck
            Deck toDrawFrom = new Deck(server.GameState.GetValueCardCollection(Names.DECK));

            // Iterate over all players
            for (byte index = 0; index < server.Players.Count; index++)
            {
                // If the player exists
                if (server.Players[index] != null)
                {
                    // Add 6 cards to their hand
                    for (int cIndex = 0; cIndex < 6; cIndex++)
                        server.Players[index].Hand.Add(toDrawFrom.Draw());
                }
            }

            // Update the deck and the count in the state
            server.GameState.Set<CardCollection>(Names.DECK, toDrawFrom.Cards);
            server.GameState.Set<int>(Names.DECK_COUNT, toDrawFrom.CardsInDeck);
        }
    }
}
