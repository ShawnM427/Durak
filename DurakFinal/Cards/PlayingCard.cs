﻿/*
 * Playing Card.cs - The PlayingCard Class
 *
 * This class represents everything there is to see and know about a simple playing card
 *
 * @author      Ryan Schuette
 * @version     1.0
 * @since       23 Feb 2016
 */

using System;
using System.Drawing;

namespace Durak.Common.Cards
{
    /// <summary>
    /// Represents a simple multi-use playing card
    /// </summary>
    public class PlayingCard : ICloneable, IComparable
    {

        #region FIELDS AND PROPERTIES

        /// <summary>
        /// Backing field for Suit
        /// </summary>
        protected CardSuit mySuit;
        /// <summary>
        /// Gets or sets the suit for this playing card
        /// </summary>
        public CardSuit Suit
        {
            get { return mySuit; } // return the suit
            set { mySuit = value; } // set the suit
        }

        /// <summary>
        /// Backing field for Rank
        /// </summary>
        protected CardRank myRank;
        /// <summary>
        /// Gets or sets the rank for this playing card
        /// </summary>
        public CardRank Rank
        {
            get { return myRank; } // return the rank
            set { myRank = value; } // set the rank
        }

        /// <summary>
        /// The backing field for the CardValue
        /// </summary>
        protected int myValue;
        /// <summary>
        /// Gets or sets the Value of this card
        /// </summary>
        public int CardValue
        {
            get { return myValue; } // return my value
            set { myValue = value; } // set my value
        }

        /// <summary>
        /// The backing field for AlternateValue
        /// </summary>
        protected int? altValue = null; // nullable type
        /// <summary>
        /// Gets or sets the alternative value of this card
        /// </summary>
        public int? AlternateValue
        {
            get { return altValue; } // return alt value
            set { altValue = value; } // set alt value
        }

        /// <summary>
        /// The backing field for FaceUp
        /// </summary>
        protected bool faceUp = false;
        /// <summary>
        /// Gets or sets whether this card is face up
        /// </summary>
        public bool FaceUp
        {
            get { return faceUp; } // return faceup value
            set { faceUp = value; } // set faceup value
        }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Create a new default playing card, this will be the Ace of Spades
        /// </summary>
        public PlayingCard() : this(CardRank.Ace, CardSuit.Spades)
        {

        }

        /// <summary>
        /// Card Constructor
        /// Initialized te playing card object. By default, card is face down, with no alternate value
        /// </summary>
        /// <param name="rank">The playing card rank. Defaults to 'Ace'</param>
        /// <param name="suit">The playing card suit. Defaults to 'Hearts'</param>
        public PlayingCard(CardRank rank = CardRank.Ace, CardSuit suit = CardSuit.Hearts)
        {
            // set the rank, suit
            this.myRank = rank;
            this.mySuit = suit;
            // set he default card value
            this.myValue = (int)rank;
        }
        
        #endregion

        #region PUBLIC METHODS
        
        /// <summary>
        /// CompareTo Method
        /// Card-specific comparison method used to sort Card instances. Compares this instance with a card object.
        /// </summary>
        /// <param name="obj">The object this card is being compared to.</param>
        /// <returns>an integer that indicates whether this card precedes, follows, or occurs in the same hand</returns>
        public virtual int CompareTo(object obj)
        {
            // is the argument null?
            if (obj == null)
            {
                // throw an argument null exception
                throw new ArgumentNullException("Unable to compare a Card to a null object");
            }
            // convert the argument to a Card
            PlayingCard compareCard = obj as PlayingCard;
            // if the conversion worked
            if (compareCard != null)
            {
                // compare based on valye first, then suit.
                int thisSort = this.myValue * 10 + (int)this.mySuit;
                int compareCardSort = compareCard.myValue * 10 + (int)compareCard.mySuit;
                return (thisSort.CompareTo(compareCardSort));
            }
            else // otherwise, the conversion failed
            {
                // throw an argument exception
                throw new ArgumentException("Object being compared cannot be converted to a Card.");
            }
        } // end of CompareTo

        /// <summary>
        /// Clone Method
        /// To support the IClonable interface. Used for deep copying in card collestion classes.
        /// </summary>
        /// <returns>A copy of the cards as a system.object</returns>
        public object Clone()
        {
            return this.MemberwiseClone(); // return a memberwise clone.
        }

        /// <summary>
        /// ToString: Overrides System.Object.ToString()
        /// </summary>
        /// <returns>the name of the card as a string</returns>
        public override string ToString()
        {
            string cardString; // holds the playing card name.
            // if the card is face up
            if (faceUp)
            {
                // If the card is a joker
                if (myRank == CardRank.Joker)
                {
                    // set the card name string to {Red|BLack} joker
                    // if the suit is black
                    if (mySuit == CardSuit.Clubs || mySuit == CardSuit.Spades)
                    {
                        // set the name string to black joker
                        cardString = "Joker_Black";
                    }
                    else // the suit must be red
                    {
                        // set the name string to red joker
                        cardString = "Joker_Red";
                    }
                }
                // otherwise, the card is face up but not a joker
                else
                {
                    // set the card name string to {Rank} of {Suit}
                    cardString = myRank.ToString() + " of " + mySuit.ToString();
                }
            }
            // otherwise the card is face down.
            else
            {
                // set the card name to face down
                cardString = "Face Down";
            }
            // return the appropriate card name string
            return cardString;
        }

        /// <summary>
        /// Equals: Overrides System.Object.Equals()
        /// </summary>
        /// <returns>true of the card values are equal</returns>
        public override bool Equals(object obj)
        {
            return obj is PlayingCard && (this.Rank == (obj as PlayingCard).Rank && this.Suit == (obj as PlayingCard).Suit);
        }

        /// <summary>
        /// GetHashCode: Overrides System.Object.GetHAshCode()
        /// </summary>
        /// <returns>Card valye *10 + suit number</returns>
        public override int GetHashCode()
        {
            return this.myValue * 100 + (int)this.mySuit * 10 + ((this.faceUp)?1:0);
        }

        /// <summary>
        /// GetCardImage
        /// Gets the image associated with the card from the resource file.
        /// </summary>
        /// <returns>an image corresponding to the playing card</returns>
        public Image GetCardImage()
        {
            string imageName; // the name of the image in the resources file
            Image cardImage; // holds the image
            // if the card is not face up
            if (!faceUp)
            {
                // set the image name to "Back"
                imageName = "Back"; // sets it to te image name for the back of a card
            }
            else if (myRank == CardRank.Joker) // if the card is a joker
            {
                // if the suit is black
                if (mySuit == CardSuit.Clubs || mySuit == CardSuit.Spades)
                {
                    // set the image to black joker
                    imageName = "Joker_Black";
                }
                else // the suit is red
                {
                    // set the image to red joker
                    imageName = "Joker_Red";
                }
            }
            else // otherwise, the card is face up and not a joker
            {
                // set the image name to {Suit}_{Rank}
                imageName = mySuit.ToString() + "_" + myRank.ToString(); // enumerations are handy!
            }
            // set the image to the appropriate object we get from the resources file
            cardImage = Durak.Properties.Resources.ResourceManager.GetObject(imageName) as Image;
            //return the image
            return cardImage;
        }

        /// <summary>
        /// DebugString
        /// Generates a string showing the state of the card object; useful for debug purposes
        /// </summary>
        /// <returns>a string showing the state of this card object</returns>
        public string DebugString()
        {
            string cardState = (string)(myRank.ToString() + " of " + mySuit.ToString()).PadLeft(20);
            cardState += (string)((FaceUp) ? "(Face Up)" : "(Face Down)").PadLeft(12);
            cardState += "Value: " + myValue.ToString().PadLeft(2);
            cardState += ((altValue != null) ? "/" + altValue.ToString() : "");
            return cardState;
        }

        #endregion

        #region RELATONAL OPERATORS

        /// <summary>
        /// Determines if the two cards are the same by value
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns></returns>
        public static bool operator ==(PlayingCard left, PlayingCard right)
        {
            if (!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null))
                // return the result of CardA == CardB
                return (left.Suit == right.Suit && left.Rank == right.Rank);
            else
                return object.ReferenceEquals(left, null) && object.ReferenceEquals(left, null);
        }

        /// <summary>
        /// Determines of the two cards are not the same by value
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns></returns>
        public static bool operator !=(PlayingCard left, PlayingCard right)
        {
            return !(left == null);
        }

        /// <summary>
        /// Determines if a card is less than another card by value
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns></returns>
        public static bool operator <(PlayingCard left, PlayingCard right)
        {
            // return the result of CardA < CardB
            return (left.CardValue < right.CardValue);
        }

        /// <summary>
        /// Determines if a card is less than or equal to another card by value
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(PlayingCard left, PlayingCard right)
        {
            // return the result of CardA <= CardB
            return (left.CardValue <= right.CardValue);
        }

        /// <summary>
        /// Determines if a card is greater than another card by value
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns></returns>
        public static bool operator >(PlayingCard left, PlayingCard right)
        {
            // return the result of CardA > CardB
            return (left.CardValue > right.CardValue);
        }

        /// <summary>
        /// Determines if a card is greater than or equal to another card by value
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns></returns>
        public static bool operator >=(PlayingCard left, PlayingCard right)
        {
            // return the result of CardA >= CardB
            return (left.CardValue >= right.CardValue);
        }

        #endregion

    } // end of class

} // end of namespace block
