using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    /// <summary>
    /// The rule that verifies if an attacking player can play a card
    /// </summary>
    public class AttackingRule : IGamePlayRule
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
                return "Rule for Atacker";
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
            if (server.GameState.GetValueBool(Names.IS_ATTACKING) && (move.Player.PlayerId == server.GameState.GetValueByte(Names.ATTACKING_PLAYER) || server.GameState.GetValueBool(Names.REQUEST_HELP)))
            {
                if (move.Move == null)
                {
                    if (server.GameState.GetValueByte(Names.ATTACKING_PLAYER) == move.Player.PlayerId)
                        return true;
                    else
                    {
                        reason = "You cannot forfeit on behalf of another player";
                        return false;
                    }
                }

                int round = server.GameState.GetValueInt(Names.CURRENT_ROUND);

                if (round == 0)
                {
                    return true;
                }
                else
                {
                    bool canPlay = false;

                    for (int index = 0; index < round; index++)
                    {
                        if (
                            move.Move.Rank == server.GameState.GetValueCard(Names.ATTACKING_CARD, index).Rank ||
                            move.Move.Rank == server.GameState.GetValueCard(Names.DEFENDING_CARD, index).Rank)
                            canPlay = true;
                    }

                    if (!canPlay)
                        reason = "You must play a card with a rank that has already been played";

                    return canPlay;
                }
            }
            else if (!server.GameState.GetValueBool(Names.IS_ATTACKING))
                return true;
            else
                return false;
        }
    }
}
