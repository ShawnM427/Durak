using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Durak.Common.Cards
{
//  [Serializable]
    //public class ComputerPlayer : Player
    //{
    //    private Random _random = new Random();

    //    public ComputerSkillLevel Skill { get; set; }

    //    public override string Name
    //    {
    //        get { return string.Format("Computer {0}", PlayerId); }
    //    }

//    public void PerformDraw(Deck deck, PlayingCard availableCard)
//    {
//      switch (Skill)
//      {
//        case ComputerSkillLevel.Dumb:
//          DrawCard(deck);
//          break;
//        default:
//          DrawBestCard(deck, availableCard, (Skill == ComputerSkillLevel.Cheats));
//          break;
//      }
//    }

//    public void PerformDiscard(Deck deck)
//    {
//      switch (Skill)
//      {
//        case ComputerSkillLevel.Dumb:
//          int discardIndex = _random.Next(Hand.Count);
//          DiscardCard(Hand[discardIndex]);
//          break;
//        default:
//          DiscardWorstCard();
//          break;
//      }
//    }

//    private void DrawBestCard(Deck deck, PlayingCard availableCard, bool cheat = false)
//    {
//      var bestSuit = CalculateBestSuit();
//      if (availableCard.suit == bestSuit)
//        AddCard(availableCard);
//      else if (cheat == false)
//        DrawCard(deck);
//      else
//        AddCard(deck.SelectCardOfSpecificSuit(bestSuit));
//    }

//    private void DiscardWorstCard()
//    {
//      var worstSuit = CalculateWorstSuit();
//      foreach (PlayingCard card in Hand)
//      {
//        if (card.suit == worstSuit)
//        {
//          DiscardCard(card);
//          break;
//        }
//      }
//    }

//    private Suit CalculateBestSuit()
//    {
//      Dictionary<Suit, List<PlayingCard>> cardSuits = new Dictionary<Suit, List<PlayingCard>>();
//      cardSuits.Add(Suit.Club, new List<PlayingCard>());
//      cardSuits.Add(Suit.Diamond, new List<PlayingCard>());
//      cardSuits.Add(Suit.Heart, new List<PlayingCard>());
//      cardSuits.Add(Suit.Spade, new List<PlayingCard>());
//      int max = 0;
//      Suit currentSuit = Suit.Club;

//      foreach (PlayingCard card in Hand)
//      {
//        cardSuits[card.suit].Add(card);
//        if (cardSuits[card.suit].Count > max)
//        {
//          max = cardSuits[card.suit].Count;
//          currentSuit = card.suit;
//        }
//      }
//      return currentSuit;
//    }

//    private Suit CalculateWorstSuit()
//    {
//      Dictionary<Suit, List<PlayingCard>> cardSuits = new Dictionary<Suit, List<PlayingCard>>();
//      cardSuits.Add(Suit.Club, new List<PlayingCard>());
//      cardSuits.Add(Suit.Diamond, new List<PlayingCard>());
//      cardSuits.Add(Suit.Heart, new List<PlayingCard>());
//      cardSuits.Add(Suit.Spade, new List<PlayingCard>());
//      int min = Hand.Count;
//      Suit currentSuit = Suit.Club;

//      foreach (PlayingCard card in Hand)
//      {
//        cardSuits[card.suit].Add(card);
//      }
//      foreach (var item in cardSuits)
//      {
//        if (item.Value.Count > 0 && item.Value.Count < min)
//        {
//          min = item.Value.Count;
//          currentSuit = item.Key;
//        }
//      }
//      return currentSuit;
//    }
    //}
}
