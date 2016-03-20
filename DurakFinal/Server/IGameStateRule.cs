using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Server
{
    /// <summary>
    /// Represents a rule to apply to game states
    /// </summary>
    public interface IGameStateRule
    {
        /// <summary>
        /// Gets or sets whether this ruls is enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets a human readable name for this game rule. Used for debugging
        /// as well as logging and options
        /// </summary>
        string ReadableName { get; }

        /// <summary>
        /// Validates the game state
        /// </summary>
        /// <param name="players">The players in the game, for easier access</param>
        /// <param name="state">The game state to validate</param>
        void ValidateState(PlayerCollection players, GameState state);
    }
}
