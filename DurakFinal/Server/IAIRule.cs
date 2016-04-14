using System;
using Durak.Common;
using Durak.Common.Cards;
using System.Collections.Generic;

namespace Durak.Server
{
    /// <summary>
    /// Represents a rule an AI bot uses to determine the card to play
    /// </summary>
    public interface IAIRule
    {
        /// <summary>
        /// Determines a proposed move from the given state and current hand
        /// </summary>
        /// <param name="proposalTable">The currently proposed moves, will contain records for all cards in hand</param>        
        /// <param name="server">The server to execute on</param>
        /// <param name="hand">The bot's current hand</param>
        /// <returns>The proposed move</returns>
        void Propose(Dictionary<PlayingCard, float> proposalTable, GameServer server, CardCollection hand);
    }
}