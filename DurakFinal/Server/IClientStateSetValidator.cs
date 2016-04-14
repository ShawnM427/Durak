using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Server
{
    /// <summary>
    /// Represents the interface used by the server to determine whether a client can set a state parameter
    /// </summary>
    public interface IClientStateSetValidator
    {
        /// <summary>
        /// Tries setting the requested state parameter
        /// </summary>
        /// <param name="parameter">The parameter that the client is requesting</param>
        /// <param name="sender">The player that is requesting the state change</param>
        /// <param name="players">The server's player collection</param>
        /// <param name="state">The server's game state</param>
        void TrySetState(StateParameter parameter, Player sender, PlayerCollection players, GameState state);
    }
}
