using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Server;
using Durak.Common;

namespace DurakGame.Rules
{
    /// <summary>
    /// Represents the rule that is invoked after a card is successfully played
    /// </summary>
    public class CardPlayed : IMoveSucessRule
    {
        /// <summary>
        /// Updates the game state after the card has been played
        /// </summary>
        /// <param name="server">The server to execute on</param>
        /// <param name="move">The move that was played</param>
        public void UpdateState(GameServer server, GameMove move)
        {
            // Remove the card from the players hand
            if (move.Move != null)
                move.Player.Hand.Discard(move.Move);

            // We need to switch on whether we are attacking
            if (server.GameState.GetValueBool(Names.IS_ATTACKING))
            {
                // If they played a null card, then they have forfeited
                if (move.Move == null)
                    server.GameState.Set(Names.ATTACKER_FORFEIT, true);
                // Otherwise, put the card into the right slot
                else
                    server.GameState.Set(Names.ATTACKING_CARD, server.GameState.GetValueInt(Names.CURRENT_ROUND), move.Move);

                // We are defending now
                server.GameState.Set<bool>(Names.IS_ATTACKING, false);
            }
            else
            {
                // If they played a null card, then they have forfeited
                if (move.Move == null)
                    server.GameState.Set(Names.DEFENDER_FORFEIT, true);
                // Otherwise, put the card into the right slot
                else
                    server.GameState.Set(Names.DEFENDING_CARD, server.GameState.GetValueInt(Names.CURRENT_ROUND), move.Move);

                // We are attacking now
                server.GameState.Set<bool>(Names.IS_ATTACKING, true);

                // Move to next round, we will check for defender win in a state check rule
                server.GameState.Set<int>(Names.CURRENT_ROUND, server.GameState.GetValueInt(Names.CURRENT_ROUND) + 1);
            }

            // After any card has been played we are no longer requesting help
            server.GameState.Set<bool>(Names.REQUEST_HELP, false);
        }
    }
}
