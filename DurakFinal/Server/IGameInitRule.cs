using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Server
{
    /// <summary>
    /// Represents a rule to apply when intializing games
    /// </summary>
    public interface IGameInitRule
    {
        /// <summary>
        /// Gets or sets whether this rule is enabled
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a human readable name for this game rule. Used for debugging
        /// as well as logging and options
        /// </summary>
        string ReadableName { get; }

        /// <summary>
        /// Gets the priority to excecute in, higher priority gets excecuted first
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Validates the game state
        /// </summary>
        /// <param name="server">The server to execute on</param>
        void InitState(GameServer server);
    }
}
