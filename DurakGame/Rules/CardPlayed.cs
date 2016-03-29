using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Server;
using Durak.Common;

namespace DurakTester.Rules
{
    public class CardPlayed : IMoveSucessRule
    {
        public void UpdateState(GameMove move, PlayerCollection players, GameState state)
        {
            move.Player.Hand.Discard(move.Move);

            if (state.GetValueBool("IsAttacking"))
            {
                if (move.Move == null)
                {
                    state.Set("attacker_forfeit", true);
                }
                else
                {
                    state.Set("attacking_card", state.GetValueInt("current_round"), move.Move);
                }
            }
            else
            {
                if (move.Move == null)
                {
                    state.Set("defender_forfeit", true);
                }
                else
                {
                    state.Set("defending_card", state.GetValueInt("current_round"), move.Move);
                }
            }
        }
    }
}
