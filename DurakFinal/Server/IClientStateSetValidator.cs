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
        /// <param name="parameter">The parameter to try and set</param>
        /// <param name="server">The server to excecute on</param>
        /// <param name="sender">The player that is requesting the state change</param>
        void TrySetState(StateParameter parameter, GameServer server, Player sender);
    }
}
