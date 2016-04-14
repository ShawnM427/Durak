/*
 * CardSuit.cs - The CardSuit Enumeration
 *
 * This enumeration represents all the different suits in a deck of cards
 *
 * @author      Ryan Schuette
 * @version     1.0
 * @since       23 Feb 2016
 */

namespace Durak.Common.Cards
{
    /// <summary>
    /// CardSuit Enumeration
    /// Used to represent the 4 suits in a standard deck of cards
    /// </summary>
    public enum CardSuit : byte
    {
        /// <summary>
        /// The card rank is Spades
        /// </summary>
        Spades,
        /// <summary>
        /// The card rank is Hearts
        /// </summary>
        Hearts,
        /// <summary>
        /// The card rank is Diamonds
        /// </summary>
        Diamonds,
        /// <summary>
        /// The card rank is Clubs
        /// </summary>
        Clubs
    }
}
