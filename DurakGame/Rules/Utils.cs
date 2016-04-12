using Durak.Common;
using Durak.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    static class Utils
    {
        public static void MoveNextDuel(GameState state, PlayerCollection players)
        {
            byte attackingPlayerId = (byte)(state.GetValueByte(Names.ATTACKING_PLAYER) + 1);
            byte defendingPlayerId = (byte)(state.GetValueByte(Names.DEFENDING_PLAYER) + 1);

            while (players[attackingPlayerId] == null)
                attackingPlayerId = (byte)(attackingPlayerId + 1 >= players.Count ? 0 : attackingPlayerId + 1);

            while (players[defendingPlayerId] == null)
                defendingPlayerId = (byte)(defendingPlayerId + 1 >= players.Count ? 0 : defendingPlayerId + 1);

            state.Set(Names.ATTACKING_PLAYER, attackingPlayerId);
            state.Set(Names.DEFENDING_PLAYER, defendingPlayerId);
            state.Set(Names.IS_ATTACKING, true);
            state.Set(Names.ATTACKER_FORFEIT, false);
            state.Set(Names.DEFENDER_FORFEIT, false);
            state.Set(Names.CURRENT_ROUND, 0);

            byte id = state.GetValueByte(Names.ATTACKING_PLAYER);
            byte startId = id;
            Deck deck = new Deck(state.GetValueCardCollection(Names.DECK));

            bool go = true;

            while (go)
            {
                Player player = players[id];

                if (player != null)
                {
                    while (player.Hand.Count < 6)
                    {
                        if (deck.Cards.Count > 0)
                        {
                            player.Hand.Add(deck.Draw());
                        }
                        else
                        {
                            player.Hand.Add(state.GetValueCard(Names.TRUMP_CARD));
                            go = false;
                            break;
                        }
                    }
                }

                id++;

                if (id >= players.Count)
                    id = 0;
                if (id == startId)
                    break;
            }

            state.Set(Names.DECK, deck.Cards);
            state.Set(Names.DECK_COUNT, deck.CardsInDeck);
        }
    }
}
