using System;

namespace Durak.Common.Cards
{
    /// <summary>
    /// Thrown when a card index is out of bounds
    /// </summary>
    public class CardOutOfRangeException : Exception
    {
        /// <summary>
        /// Stores the deck for the exception
        /// </summary>
        private CardCollection deckContents;

        /// <summary>
        /// Gets the hand that the exception was thrown for
        /// </summary>
        public CardCollection DeckContents
        {
            get
            {
                return deckContents;
            }
        }

        /// <summary>
        /// Creates a new Card out of Range exception
        /// </summary>
        /// <param name="sourceDeckContents">The source deck that raised the exception</param>
        public CardOutOfRangeException(CardCollection sourceDeckContents) :
            base("There are only 52 cards in the deck.")
        {
            deckContents = sourceDeckContents;
        }
    }
}
