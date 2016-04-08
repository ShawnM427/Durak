using System;
using System.Collections.Generic;

namespace Durak.Common.Cards
{
    /// <summary>
    /// A delegate used to handle when the last card is drawn from a given deck
    /// </summary>
    /// <param name="currentDeck">The deck that raised the event</param>
    public delegate void LastCardDrawnHandler(Deck currentDeck);

    /// <summary>
    /// Represents a card deck that contains all possible cards on creation
    /// </summary>
    public class Deck : ICloneable
    {
        /// <summary>
        /// Invoked when the last card is drawn from this deck
        /// </summary>
        public event LastCardDrawnHandler OnLastCardDrawn;

        /// <summary>
        /// Gets the cards in this deck
        /// </summary>
        /// <returns></returns>
        public CardCollection GetCards()
        {
            return cards.Clone() as CardCollection;
        }

        /// <summary>
        /// The internal card collection
        /// </summary>
        private CardCollection cards = new CardCollection();

        /// <summary>
        /// Default constructor - Creates a new card deck
        /// </summary>
        public Deck()
        {
            InsertCards(1, 14);
        }


        /// <summary>
        /// Paramaterized Constructor - Creates a deck from the given collection
        /// </summary>
        /// <param name="newCards">The collection to use</param>
        public Deck(CardCollection newCards)
        {
            cards = newCards;
        }

        /// <summary>
        /// Paramaterized Constructor - Creates a new card deck
        /// </summary>
        /// <param name="minRankIndex">The minimum rank</param>
        /// <param name="maxRankIndex">The maximum rank</param>
        public Deck(CardRank minRankIndex, CardRank maxRank)
        {
            InsertCards((int)minRankIndex, (int)maxRank + 1);
        }

        /// <summary>
        /// Insert every card from a normal deck into this deck
        /// </summary>
        /// <param name="minRankIndex">The minimum rank index</param>
        /// <param name="maxRankIndex">The maximum rank index</param>
        private void InsertCards(int minRankIndex, int maxRankIndex)
        {
            for (int suitVal = 0; suitVal < 4; suitVal++)
            {
                for (int rankVal = minRankIndex; rankVal < maxRankIndex; rankVal++)
                {
                    cards.Add(new PlayingCard((CardRank)rankVal, (CardSuit)suitVal));
                }
            }
        }

        /// <summary>
        /// Inserts all cards except those from an exempt list
        /// </summary>
        /// <param name="except">The cards to NOT add to this deck</param>
        private void InsertAllCards(List<PlayingCard> except)
        {
            for (int suitVal = 0; suitVal < 4; suitVal++)
            {
                for (int rankVal = 1; rankVal < 14; rankVal++)
                {
                    var card = new PlayingCard((CardRank)rankVal, (CardSuit)suitVal);

                    if (except.Contains(card))
                        continue;

                    cards.Add(card);
                }
            }
        }

        /// <summary>
        /// Gets the number of cards remaining in this deck
        /// </summary>
        public int CardsInDeck
        {
            get { return cards.Count; }
        }

        /// <summary>
        /// Gets the card collection this deck wraps around
        /// </summary>
        public CardCollection Cards
        {
            get { return cards; }
        }

        /// <summary>
        /// Shuffles the cards in this deck
        /// </summary>
        public void Shuffle()
        {
            cards.Shuffle();
        }

        /// <summary>
        /// Draws the next card in this deck
        /// </summary>
        /// <returns>The next card in the deck</returns>
        public PlayingCard Draw()
        {
            // If there are no cards, don't return anything
            if (cards.Count == 0)
                return null;
            else
            {
                // Gets the first card, then removes it
                var card = cards[0];
                cards.DiscardAt(0);

                // If this wwas the last card, inoke our event
                if (cards.Count == 0 && OnLastCardDrawn != null)
                    OnLastCardDrawn(this);
                
                // return the result
                return card;
            }
        }

        /// <summary>
        /// Attempts to select a card of a specific suit from the deck
        /// </summary>
        /// <param name="suit">The target suit</param>
        /// <returns>The best card that fits the suit, note that this will return a random card if no cards of that suit are available</returns>
        public PlayingCard SelectCardOfSpecificSuit(CardSuit suit)
        {
            // Get the result
            PlayingCard selectedCard = null;

            // Try to get a card of the suit
            foreach (PlayingCard card in cards)
            {
                if (card.Suit == suit)
                {
                    selectedCard = card;
                    break;
                }
            }

            // If the card is null, draw a random one
            if (selectedCard == null)
                return Draw();
            // If we found a card, make sure to remove it from the deck
            else
                cards.Discard(selectedCard);

            // Return the result
            return selectedCard;
        }

        /// <summary>
        /// Clones this card deck
        /// </summary>
        /// <returns>A clone of this deck</returns>
        public object Clone()
        {
            Deck newDeck = new Deck(cards.Clone() as CardCollection);
            return newDeck;
        }
    }
}