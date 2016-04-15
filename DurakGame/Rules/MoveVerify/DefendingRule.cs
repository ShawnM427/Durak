using Durak.Common;
using Durak.Common.Cards;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    /// <summary>
    /// The rule that defines what cards are valid for a defender to play
    /// </summary>
    public class DefendingRule : IGamePlayRule
    {
        /// <summary>
        /// Gets or sets whether this rule is enabled
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the human readable name for this rule
        /// </summary>
        public string ReadableName
        {
            get
            {
                return "Defending Rule";
            }
        }

        /// <summary>
        /// Checks to see if the given move is valid
        /// </summary>
        /// <param name="server">The server to execute on</param>
        /// <param name="move">The move being played</param>
        /// <param name="reason">The reason the move is invalid</param>
        /// <returns>TRue if the move was sucessfull, false if otherwise</returns>
        public bool IsValidMove(GameServer server, GameMove move, ref string reason)
        {
            if (move.Move == null)
                return true;

            if (!server.GameState.GetValueBool(Names.IS_ATTACKING))
            {
                if (server.GameState.GetValueByte(Names.DEFENDING_PLAYER) == move.Player.PlayerId)
                {
                    PlayingCard attacking = server.GameState.GetValueCard(Names.ATTACKING_CARD, server.GameState.GetValueInt(Names.CURRENT_ROUND));
                    PlayingCard trump = server.GameState.GetValueCard(Names.TRUMP_CARD);

                    if (move.Move.Rank > attacking.Rank)
                    {
                        if (move.Move.Suit == attacking.Suit || move.Move.Suit == trump.Suit)
                        {
                            return true;
                        }
                        else
                        {
                            reason = "You must play a card of a higher rank of the same suit, or a trump card";
                            return false;
                        }
                    }
                    else if (move.Move.Suit == trump.Suit && attacking.Suit != trump.Suit)
                        return true;
                    else
                    {
                        reason = "You must play a card of a higher rank";
                        return false;
                    }
                }
                else
                {
                    reason = "It is not your turn to defend";
                    return false;
                }
            }
            else
                return true;
        }
    }
}
