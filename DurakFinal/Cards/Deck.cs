using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common.Cards
{

    public delegate void LastCardDrawnHandler(Deck currentDeck);

    public class Deck : ICloneable
    {
        #region Event added in section "Expanding and Using CardLib"
        public event LastCardDrawnHandler LastCardDrawn;
        #endregion

        private Hand cards = new Hand();

        public Deck()
        {
            InsertAllCards();
        }

        private void InsertAllCards()
        {
            for (int suitVal = 0; suitVal < 4; suitVal++)
            {
                for (int rankVal = 1; rankVal < 14; rankVal++)
                {
                    cards.Add(new PlayingCard((CardRank)rankVal, (CardSuit)suitVal));
                }
            }
        }

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

        public int CardsInDeck
        {
            get { return cards.Count; }
        }

        private Deck(Hand newCards)
        {
            cards = newCards;
        }


        public PlayingCard GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= 51)
            {
                // Code modified in section "Expanding and Using CardLib"
                if ((cardNum == 51) && (LastCardDrawn != null))
                    LastCardDrawn(this);
                return cards[cardNum];
            }
            else
                throw new CardOutOfRangeException(cards.Clone() as Hand);
        }

        public void Shuffle()
        {
            Hand newDeck = new Hand();
            bool[] assigned = new bool[cards.Count];
            Random sourceGen = new Random();
            for (int i = 0; i < cards.Count; i++)
            {
                int sourceCard = 0;
                bool foundCard = false;
                while (foundCard == false)
                {
                    sourceCard = sourceGen.Next(cards.Count);
                    if (assigned[sourceCard] == false)
                        foundCard = true;
                }
                assigned[sourceCard] = true;
                newDeck.Add(cards[sourceCard]);
            }
            newDeck.CopyTo(cards);
        }

        public void ReshuffleDiscarded(List<PlayingCard> cardsInPlay)
        {
            InsertAllCards(cardsInPlay);
            Shuffle();
        }
        public PlayingCard Draw()
        {
            if (cards.Count == 0)
                return null;
            else
            {
                var card = cards[0];
                cards.RemoveAt(0);
                return card;
            }
        }

        public PlayingCard SelectCardOfSpecificSuit(CardSuit suit)
        {
            PlayingCard selectedCard = null;

            foreach (PlayingCard card in cards)
            {
                if (card.Suit == suit)
                {
                    selectedCard = card;
                    break;
                }
            }
            if (selectedCard == null)
                return Draw(); // Can't cheat, no cards of the correct Suit
            else
            {
                cards.Remove(selectedCard);
            }
            return selectedCard;
        }

        public object Clone()
        {
            Deck newDeck = new Deck(cards.Clone() as Hand);
            return newDeck;
        }
    }
}