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
        public const string DEFENDING_CARD = "defending_card";
        public const string ATTACKING_CARD = "attacking_card";
        public const string CURRENT_ROUND = "current_round";
        public const string ATTACKING_PLAYER = "attacking_player_id";
        public const string DEFENDING_PLAYER = "defending_player_id";
        public const string IS_ATTACKING = "is_attacking";
        public const string ATTACKER_FORFEIT = "attacker_forfeit";
        public const string DEFENDER_FORFEIT = "defender_forfeit";
        public const string DISCARD = "discard_pile";
        public const string TRUMP_CARD = "trump_card";
        public const string REQUEST_HELP = "request_help";
        public const string DECK_COUNT = "cards_in_deck";
        public const string DECK = "source_deck";
        public const string GAME_OVER = "game_over";
        public const string LOSER_ID = "loser_id";
        public const string IS_TIE = "is_tie";
        public const string TRUMP_CARD_USED = "trump_card_used";
    }
}
