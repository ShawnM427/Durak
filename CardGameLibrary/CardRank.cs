/**
 * CardRank.cs - The CardRank Enumeration
 *
 * This enumeration represents all the different ranks in a deck of cards
 *
 * @author      Ryan Schuette
 * @version     1.0
 * @since       23 Feb 2016
 */

namespace CardGameLibrary
{
    /// <summary>
    /// CardRank Enumeration
    /// Used to represent the 13 ranks in a standard deck of playing cards, including joker
    /// </summary>
    public enum CardRank : byte
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Joker
    }
}
