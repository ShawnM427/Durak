using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Durak.Common.Cards
{
    /// <summary>
    /// Represents an event argument for playing cards
    /// </summary>
    public class CardEventArgs : EventArgs
    {
        /// <summary>
        /// Get's or sets the card for this event
        /// </summary>
        public PlayingCard Card { get; set; }

        /// <summary>
        /// Creates a new card event args instance
        /// </summary>
        /// <param name="card">The card for this event</param>
        public CardEventArgs(PlayingCard card)
        {
            Card = card;
        }
    }
}
