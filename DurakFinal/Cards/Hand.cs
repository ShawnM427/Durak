using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common.Cards
{
    public class Hand : List<PlayingCard>, ICloneable
    {
        public object Clone()
        {
            Hand newCards = new Hand();
            foreach (PlayingCard sourceCard in this)
            {
                newCards.Add(sourceCard.Clone() as PlayingCard);
            }
            return newCards;
        }
        
        // Utility method for copying card instances into another Cards
        // instance - used in Deck.Shuffle(). This implementation assumes that
        // source and target collections are the same size.
        public void CopyTo(Hand targetCards)
        {
            for (int index = 0; index < this.Count; index++)
            {
                targetCards[index] = this[index];
            }
        }

        public void Discard(PlayingCard card)
        {
            Remove(card);
        }

    }
}
