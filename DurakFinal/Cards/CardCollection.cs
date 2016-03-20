using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common.Cards
{
    /// <summary>
    /// Represents a collection of playing cards
    /// </summary>
    public class CardCollection : List<PlayingCard>, ICloneable
    {
        /// <summary>
        /// Clones this collection
        /// </summary>
        /// <returns>A clone of this collection</returns>
        public object Clone()
        {
            // Create the result
            CardCollection newCards = new CardCollection();

            // Clone each card
            foreach (PlayingCard sourceCard in this)
                newCards.Add(sourceCard.Clone() as PlayingCard);

            // Return the result
            return newCards;
        }

        /// <summary>
        /// Shuffles the cards in this collection
        /// </summary>
        public void Shuffle()
        {
            // The textbook was absolutely retarded, so I made my own that's truly random
            Random rand = new Random();

            // Randomly choose a number of times to loop
            int loopCount = rand.Next(0, 5);

            // Loop through each card
            for (int index = 0; index < Count * loopCount; index++)
            {
                // Find a card to swap with
                int newPos = rand.Next(0, Count);

                // Swap cards
                PlayingCard swap = this[newPos];
                this[newPos] = this[index];
                this[index] = swap;
            }
        }

        /// <summary>
        /// Copies all cards in this collection to another
        /// </summary>
        /// <param name="targetCards">The target collection</param>
        public void CopyTo(CardCollection targetCards)
        {
            // Iterate over all my cards and clone them to the other collection
            for (int index = 0; index < this.Count; index++)
                targetCards.Add(this[index].Clone() as PlayingCard);
        }

        /// <summary>
        /// Removes a card from this card collection
        /// </summary>
        /// <param name="card">The card to remove</param>
        public void Discard(PlayingCard card)
        {
            Remove(card);
        }
    }
}
