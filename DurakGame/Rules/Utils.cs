using Durak.Common;
using Durak.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    /// <summary>
    /// A utility class used by Durak rules to perform actions common to multiple rules
    /// </summary>
    static class Utils
    {
        /// <summary>
        /// Handles moving on to the next duel, this will also check for the end of the game as set the GAME_OVER param to true if needed
        /// </summary>
        /// <param name="state">The state to update</param>
        /// <param name="players">The current player collection</param>
        public static void MoveNextDuel(GameState state, PlayerCollection players)
        {
            if (!state.GetValueBool(Names.GAME_OVER))
            {
                // Start by determining the number of active players
                int activePlayers = players.Where(X => X.Hand.Count > 0).Count();

                // If there are more than 1 players left, keep playing
                if (activePlayers > 1)
                {
                    // Start by getting a round idea of the new attackers and defenders
                    byte attackingPlayerId = (byte)(state.GetValueByte(Names.ATTACKING_PLAYER) + 1);
                    byte defendingPlayerId = (byte)(state.GetValueByte(Names.DEFENDING_PLAYER) + 1);

                    // Iterate over each player until we come across a valid attacker
                    while (players[attackingPlayerId] == null || players[attackingPlayerId]?.Hand?.Count <= 0)
                        attackingPlayerId = (byte)(attackingPlayerId + 1 >= players.Count ? 0 : attackingPlayerId + 1);

                    // Iterate over each player until we come across a valid defender
                    while (players[defendingPlayerId] == null || players[defendingPlayerId]?.Hand?.Count <= 0)
                        defendingPlayerId = (byte)(defendingPlayerId + 1 >= players.Count ? 0 : defendingPlayerId + 1);

                    // Update the attacker and defenders
                    state.Set(Names.ATTACKING_PLAYER, attackingPlayerId);
                    state.Set(Names.DEFENDING_PLAYER, defendingPlayerId);

                    // Clear the battle-specific states
                    state.Set(Names.IS_ATTACKING, true);
                    state.Set(Names.ATTACKER_FORFEIT, false);
                    state.Set(Names.DEFENDER_FORFEIT, false);

                    // Clear the match-specific states
                    state.Set(Names.CURRENT_ROUND, 0);

                    // Get the ID of the attacking player, this is the first person to get new cards
                    byte id = state.GetValueByte(Names.ATTACKING_PLAYER);
                    byte startId = id;

                    // Get the deck off the state 
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
                                else if (!state.GetValueBool(Names.TRUMP_CARD_USED))
                                {
                                    player.Hand.Add(state.GetValueCard(Names.TRUMP_CARD));
                                    state.Set<bool>(Names.TRUMP_CARD_USED, true);
                                    go = false;
                                    break;
                                }
                                else
                                    break;
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
                else
                {
                    if (activePlayers == 1)
                    {
                        state.Set<byte>(Names.LOSER_ID, players.Where(X => X.Hand.Count > 0).ElementAt(0).PlayerId);
                        state.Set<bool>(Names.IS_TIE, false);
                        state.Set<bool>(Names.GAME_OVER, true);
                    }
                    else if (activePlayers == 0)
                    {
                        state.Set<bool>(Names.IS_TIE, true);
                        state.Set<bool>(Names.GAME_OVER, true);
                    }
                }
            }
        }
    }
}
