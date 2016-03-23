using System;
using Durak.Common;
using Durak.Common.Cards;

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
        /// <param name="state">The current game state</param>
        /// <param name="hand">The bot's current hand</param>
        /// <returns>The proposed move</returns>
        AIMoveProposal Propose(GameState state, CardCollection hand);
    }
}