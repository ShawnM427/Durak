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
    public class AttackerLose : IGameStateRule
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

        public void ValidateState(GameServer server)
        {
            if (server.Players[server.GameState.GetValueByte(Names.DEFENDING_PLAYER)].Hand.Count == 0)
                server.GameState.Set(Names.ATTACKER_FORFEIT, true);

            // Only run this state update if the attacker is forfeiting
            if (server.GameState.GetValueBool(Names.ATTACKER_FORFEIT))
            {
                // Get the current round and the discard pile from the state
                int round = server.GameState.GetValueInt(Names.CURRENT_ROUND);
                CardCollection discard = server.GameState.GetValueCardCollection(Names.DISCARD);

                // Iterate over over all the previous rounds, as this round has no attacking or defending cards
                for (int index = 0; index < round; index++)
                {
                    // Add the cards to the discard pile
                    discard.Add(server.GameState.GetValueCard(Names.ATTACKING_CARD, index));
                    discard.Add(server.GameState.GetValueCard(Names.DEFENDING_CARD, index));

                    // Remove the cards from the state
                    server.GameState.Set<PlayingCard>(Names.ATTACKING_CARD, index, null);
                    server.GameState.Set<PlayingCard>(Names.DEFENDING_CARD, index, null);
                }

                // Send the message
                server.SendServerMessage(server.Players[server.GameState.GetValueByte(Names.ATTACKING_PLAYER)].Name + " has forfeited");

                // Update the discard pile
                server.GameState.Set(Names.DISCARD, discard);

                // Move to next match
                Utils.MoveNextDuel(server);
            }
        }
    }
}
