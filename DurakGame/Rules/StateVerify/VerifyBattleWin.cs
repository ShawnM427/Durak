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
    public class VerifyBattleWin : IGameStateRule
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
                return "Check won battle";
            }
        }

        public void ValidateState(GameServer server)
        {
            // Todo check won battle

            //if (server.GameState.GetValueInt(Names.CURRENT_ROUND) == 6)
            //{
            //    // Get the current round and the discard pile from the state
            //    int round = server.GameState.GetValueInt(Names.CURRENT_ROUND);
            //    CardCollection discard = server.GameState.GetValueCardCollection(Names.DISCARD);

            //    // Iterate over over all the previous rounds, as this round has no attacking or defending cards
            //    for (int index = 0; index < round; index++)
            //    {
            //        // Add the cards to the discard pile
            //        discard.Add(server.GameState.GetValueCard(Names.ATTACKING_CARD, index));
            //        discard.Add(server.GameState.GetValueCard(Names.DEFENDING_CARD, index));

            //        // Remove the cards from the state
            //        server.GameState.Set<PlayingCard>(Names.ATTACKING_CARD, index, null);
            //        server.GameState.Set<PlayingCard>(Names.DEFENDING_CARD, index, null);
            //    }

            //    // Update the discard pile
            //    server.GameState.Set(Names.DISCARD, discard);

            //    // Move to the next duel
            //    Utils.MoveNextDuel(server);
            //}
        }
    }
}
