﻿using Durak.Common;
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
    /// Checks to see if the defender has lost, and updates the game state accordingly
    /// </summary>
    public class DefenderLose : IGameStateRule
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
                return "Verify attacking players gets battling cards";
            }
        }

        /// <summary>
        /// Handles validating the server state. If the defender has lost, this handles moving to the next round
        /// </summary>
        /// <param name="server">The server to execute on</param>
        public void ValidateState(GameServer server)
        {
            // Only run this state update if the defender is forfeiting
            if (server.GameState.GetValueBool(Names.DEFENDER_FORFEIT))
            {
                // Get the current round and the defending player
                int round = server.GameState.GetValueInt(Names.CURRENT_ROUND);
                Player defender = server.Players[server.GameState.GetValueByte(Names.DEFENDING_PLAYER)];

                // Iterate over all previous rounds
                for (int index = 0; index < round; index++)
                {
                    // Add to defenders hand
                    PlayingCard card1 = server.GameState.GetValueCard(Names.ATTACKING_CARD, index);
                    if (card1 != null)
                        defender.Hand.Add(card1);

                    PlayingCard card2 = server.GameState.GetValueCard(Names.DEFENDING_CARD, index);
                    if (card2 != null)
                        defender.Hand.Add(card2);

                    // Remove both from the state
                    server.GameState.Set<PlayingCard>(Names.ATTACKING_CARD, index, null);
                    server.GameState.Set<PlayingCard>(Names.DEFENDING_CARD, index, null);
                }

                // Send the message
                server.SendServerMessage(defender.Name + " has forfeited");

                // Move to next match
                Utils.MoveNextDuel(server);
            }
        }
    }
}
