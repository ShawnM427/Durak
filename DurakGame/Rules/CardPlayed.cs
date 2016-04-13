using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Server;
using Durak.Common;

namespace DurakGame.Rules
{
    public class CardPlayed : IMoveSucessRule
    {
        public void UpdateState(GameMove move, PlayerCollection players, GameState state)
        {
            // Remove the card from the players hand
            if (move.Move != null)
                move.Player.Hand.Discard(move.Move);

            // We need to switch on whether we are attacking
            if (state.GetValueBool(Names.IS_ATTACKING))
            {
                // If they played a null card, then they have forfeited
                if (move.Move == null)
                    state.Set(Names.ATTACKER_FORFEIT, true);
                // Otherwise, put the card into the right slot
                else
                    state.Set(Names.ATTACKING_CARD, state.GetValueInt(Names.CURRENT_ROUND), move.Move);

                // We are defending now
                state.Set<bool>(Names.IS_ATTACKING, false);
            }
            else
            {
                // If they played a null card, then they have forfeited
                if (move.Move == null)
                    state.Set(Names.DEFENDER_FORFEIT, true);
                // Otherwise, put the card into the right slot
                else
                    state.Set(Names.DEFENDING_CARD, state.GetValueInt(Names.CURRENT_ROUND), move.Move);

                // We are attacking now
                state.Set<bool>(Names.IS_ATTACKING, true);

                // Move to next round, we will check for defender win in a state check rule
                state.Set<int>(Names.CURRENT_ROUND, state.GetValueInt(Names.CURRENT_ROUND) + 1);
            }

            // After any card has been played we are no longer requesting help
            state.Set<bool>(Names.REQUEST_HELP, false);
        }
    }
}
