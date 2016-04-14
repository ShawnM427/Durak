using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame
{
    /// <summary>
    /// This a utility class containing state names as constants. This lets us ensure that we always use the same name
    /// to refer to important states
    /// </summary>
    public static class Names
    {
        /// <summary>
        /// The name of the defending cards collection
        /// This is of type PlayingCard
        /// </summary>
        public const string DEFENDING_CARD = "defending_card";
        /// <summary>
        /// The name of the attacking cards collection
        /// This is of type PlayingCard
        /// </summary>
        public const string ATTACKING_CARD = "attacking_card";
        /// <summary>
        /// The name of the current round counter
        /// This is of type byte
        /// </summary>
        public const string CURRENT_ROUND = "current_round";
        /// <summary>
        /// The name of the attacking player's ID
        /// This is of type byte
        /// </summary>
        public const string ATTACKING_PLAYER = "attacking_player_id";
        /// <summary>
        /// The name of the defending player's ID
        /// This is of type byte
        /// </summary>
        public const string DEFENDING_PLAYER = "defending_player_id";
        /// <summary>
        /// The name of the Is Attacking parameter
        /// This is of type boolean
        /// </summary>
        public const string IS_ATTACKING = "is_attacking";
        /// <summary>
        /// The name of the attacker forfeiting field
        /// This is of type boolean
        /// </summary>
        public const string ATTACKER_FORFEIT = "attacker_forfeit";
        /// <summary>
        /// The name of the defender forfeiting field
        /// This is of type boolean
        /// </summary>
        public const string DEFENDER_FORFEIT = "defender_forfeit";
        /// <summary>
        /// The name of the discard pile
        /// This is of type CardCollection
        /// </summary>
        public const string DISCARD = "discard_pile";
        /// <summary>
        /// The name of the trump card field
        /// This is of type PlayingCard
        /// </summary>
        public const string TRUMP_CARD = "trump_card";
        /// <summary>
        /// The name of the request help field
        /// This is of type boolean
        /// </summary>
        public const string REQUEST_HELP = "request_help";
        /// <summary>
        /// The name of the number of cards in deck field
        /// This is of type integer
        /// </summary>
        public const string DECK_COUNT = "cards_in_deck";
        /// <summary>
        /// The name of the source deck field
        /// This is of type CardCollection
        /// </summary>
        public const string DECK = "source_deck";
        /// <summary>
        /// The name of the game over field
        /// This is of type Boolean
        /// </summary>
        public const string GAME_OVER = "game_over";
        /// <summary>
        /// The name of the losing player's ID
        /// This is of type Byte
        /// </summary>
        public const string LOSER_ID = "loser_id";
        /// <summary>
        /// The name of the game tie field
        /// This is of type Boolean
        /// </summary>
        public const string IS_TIE = "is_tie";
        /// <summary>
        /// The name of the trump card used field
        /// This is of type Boolean
        /// </summary>
        public const string TRUMP_CARD_USED = "trump_card_used";
    }
}
