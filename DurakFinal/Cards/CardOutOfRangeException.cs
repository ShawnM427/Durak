using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common.Cards
{
    public class CardOutOfRangeException : Exception
    {
        private Hand deckContents;

        public Hand DeckContents
        {
            get
            {
                return deckContents;
            }
        }

        public CardOutOfRangeException(Hand sourceDeckContents) :
            base("There are only 52 cards in the deck.")
        {
            deckContents = sourceDeckContents;
        }
    }
}
