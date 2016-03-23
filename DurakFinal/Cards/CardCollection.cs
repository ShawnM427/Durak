using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common.Cards
{
    /// <summary>
    /// Represents a collection of playing cards
    /// </summary>
    public class CardCollection : IEnumerable<PlayingCard>, ICloneable
    {
        /// <summary>
        /// Stores the underlying list
        /// </summary>
        private List<PlayingCard> myBackingList;

        public bool Contains(PlayingCard move)
        {
            return myBackingList.Contains(move);
        }

        /// <summary>
        /// Invoked when a card is added to this collection
        /// </summary>
        public event EventHandler<CardEventArgs> OnCardAdded;
        /// <summary>
        /// Invoked when a card is removed from this collection
        /// </summary>
        public event EventHandler<CardEventArgs> OnCardRemoved;

        /// <summary>
        /// Gets the number of cards in this collection
        /// </summary>
        public int Count
        {
            get { return myBackingList.Count; }
        }

        /// <summary>
        /// Gets or sets the card at the given index in this collection
        /// </summary>
        /// <param name="index">The index of the element to get/set</param>
        /// <returns>The element at the given index</returns>
        public PlayingCard this[int index]
        {
            get { return myBackingList[index]; }
            set { myBackingList[index] = value; }
        }

        /// <summary>
        /// Creates a new empty instance of a card collection
        /// </summary>
        public CardCollection()
        {
            myBackingList = new List<PlayingCard>();
        }

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
        /// Discards all cardsin this collection
        /// </summary>
        public void Clear()
        {
            while(Count > 0)
                Discard(myBackingList[0]);
        }

        /// <summary>
        /// Clears all cards in this collection without invoking events
        /// </summary>
        public void SilentClear()
        {
            myBackingList.Clear();
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
        /// Adds a card to this collection
        /// </summary>
        /// <param name="card">The card to add</param>
        public void Add(PlayingCard card)
        {
            myBackingList.Add(card);

            if (OnCardAdded != null)
                OnCardAdded.Invoke(this, new CardEventArgs(card));
        }

        /// <summary>
        /// Discards the card at the given index
        /// </summary>
        /// <param name="index">The index to discard at</param>
        public void DiscardAt(int index)
        {
            Discard(myBackingList[index]);
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
            myBackingList.Remove(card);

            if (OnCardRemoved != null)
                OnCardRemoved.Invoke(this, new CardEventArgs(card));
        }

        /// <summary>
        /// Gets the enumerator for this collection, this will simply call down to the backing list's enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<PlayingCard> GetEnumerator()
        {
            return myBackingList.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for this collection, this will simply call down to the backing list's enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return myBackingList.GetEnumerator();
        }
    }
}
