using System;
using Durak.Common;
using Durak.Server;
using Durak.Common.Cards;

namespace DurakGame.Rules
{
    public class InitRoundStates : IGameInitRule
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
                return 100;
            }
        }

        public string ReadableName
        {
            get
            {
                return "Initialize start game states";
            }
        }

        public void InitState(PlayerCollection players, GameState state)
        {
            state.Set(Names.IS_ATTACKING, true);
            state.Set(Names.ATTACKER_FORFEIT, false);
            state.Set(Names.DEFENDER_FORFEIT, false);
            state.Set(Names.REQUEST_HELP, false);

            // Determine starting attacking and defending players
            byte attackingPlayerId = 0;
            byte defendingPlayerId = 1;

            while (players[attackingPlayerId] == null)
                attackingPlayerId = (byte)(attackingPlayerId + 1 >= players.Count ? 0 : attackingPlayerId + 1);

            while (players[defendingPlayerId] == null)
                defendingPlayerId = (byte)(defendingPlayerId + 1 >= players.Count ? 0 : defendingPlayerId + 1);

            state.Set(Names.ATTACKING_PLAYER, attackingPlayerId);
            state.Set(Names.DEFENDING_PLAYER, defendingPlayerId);

            // Build the deck
            Deck deck = new Deck(CardRank.Six, CardRank.King);
            deck.Shuffle();

            // Draw the trump card
            state.Set<PlayingCard>(Names.TRUMP_CARD, deck.Draw());

            // Set the deck on the state to be serverside
            state.Set<int>(Names.DECK_COUNT, deck.CardsInDeck);
            state.Set<CardCollection>(Names.DECK, deck.GetCards(), true);

            // Make the discard pile
            state.Set(Names.DISCARD, new CardCollection());

            // Initialize the battle slots
            for (int index = 0; index < 6; index ++)
            {
                state.Set<PlayingCard>(Names.DEFENDING_CARD, index, null);
                state.Set<PlayingCard>(Names.ATTACKING_CARD, index, null);
            }
        }
    }
}
