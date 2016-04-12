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
    class DefenderLose : IGameStateRule
    {
        public bool IsEnabled
        {
            get;
            set;
        }

        public string ReadableName
        {
            get
            {
                return "Verify attacking players gets battling cards";
            }
        }

        public void ValidateState(PlayerCollection players, GameState state)
        {
            if (state.GetValueBool("defender_forfeit"))
            {
                int round = state.GetValueInt("current_round");
                Player defender = players[state.GetValueByte("defending_player_id")];

                for (int index = 0; index <= round; index++)
                {
                    defender.Hand.Add(state.GetValueCard("attacking_card", index));

                    PlayingCard card = state.GetValueCard("defending_card", index);
                    if (card != null)
                    defender.Hand.Add(card);

                    state.Set<PlayingCard>("attacking_card", index, null);
                    state.Set<PlayingCard>("defending_card", index, null);
                }

                // Move to next match
                byte attackingPlayerId = (byte)(state.GetValueByte("attacking_player_id") + 1);
                byte defendingPlayerId = (byte)(state.GetValueByte("defending_player_id") + 1);

                while (players[attackingPlayerId] == null)
                    attackingPlayerId = (byte)(attackingPlayerId + 1 >= players.Count ? 0 : attackingPlayerId + 1);

                while (players[defendingPlayerId] == null)
                    defendingPlayerId = (byte)(defendingPlayerId + 1 >= players.Count ? 0 : defendingPlayerId + 1);

                state.Set("attacking_player_id", attackingPlayerId);
                state.Set("defending_player_id", defendingPlayerId);
                state.Set("IsAttacking", true);

                state.Set("defender_forfeit", false);
            }
        }
    }
}
