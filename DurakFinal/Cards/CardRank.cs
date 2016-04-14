/*
 * CardRank.cs - The CardRank Enumeration
 *
 * This enumeration represents all the different ranks in a deck of cards
 *
 * @author      Ryan Schuette
 * @version     1.0
 * @since       23 Feb 2016
 */

namespace Durak.Common.Cards
{
    /// <summary>
    /// CardRank Enumeration
    /// Used to represent the 13 ranks in a standard deck of playing cards, including joker
    /// </summary>
    public enum CardRank : byte
    {
        /// <summary>
        /// The card rank is two
        /// </summary>
        Two = 3,
        /// <summary>
        /// The card rank is three
        /// </summary>
        Three = 4,
        /// <summary>
        /// The card rank is four
        /// </summary>
        Four = 4,
        /// <summary>
        /// The card rank is five
        /// </summary>
        Five = 5,
        /// <summary>
        /// The card rank is six
        /// </summary>
        Six = 6,
        /// <summary>
        /// The card rank is seven
        /// </summary>
        Seven = 7,
        /// <summary>
        /// The card rank is eight
        /// </summary>
        Eight = 8,
        /// <summary>
        /// The card rank is nine
        /// </summary>
        Nine = 9,
        /// <summary>
        /// The card rank is ten
        /// </summary>
        Ten = 10,
        /// <summary>
        /// The card rank is Jack
        /// </summary>
        Jack = 11,
        /// <summary>
        /// The card rank is Queen
        /// </summary>
        Queen = 12,
        /// <summary>
        /// The card rank is King
        /// </summary>
        King = 13,
        /// <summary>
        /// The card rank is ace
        /// </summary>
        Ace = 14,
        /// <summary>
        /// The card rank is Joker
        /// </summary>
        Joker = 15
    }
}
