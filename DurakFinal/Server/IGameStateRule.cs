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
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a human readable name for this game rule. Used for debugging
        /// as well as logging and options
        /// </summary>
        string ReadableName { get; }

        /// <summary>
        /// Validates the game state
        /// </summary>
        /// <param name="server">The server to execute on</param>
        void ValidateState(GameServer server);
    }
}
