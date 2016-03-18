/**
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
        Ace = 1,
        Two = 3,
        Three = 4,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Joker = 14
    }
}
