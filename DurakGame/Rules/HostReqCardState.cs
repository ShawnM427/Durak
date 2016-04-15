using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Common;

namespace DurakGame.Rules
{
    class HostReqCardState : IClientStateSetValidator
    {
        /// <summary>
        /// Tries setting the requested state parameter
        /// </summary>
        /// <param name="parameter">The state parameter that is being requested</param>
        /// <param name="server">The server to excecute on</param>
        /// <param name="sender">The player that is requesting the state change</param>
        public void TrySetState(StateParameter parameter, GameServer server, Player sender)
        {
            if (parameter.Name == Names.NUM_INIT_CARDS && parameter.ParameterType == StateParameter.Type.Int && sender.IsHost)
            {
                server.GameState.Set(Names.NUM_INIT_CARDS, parameter.GetValueInt(), true);
            }
        }
    }
}
