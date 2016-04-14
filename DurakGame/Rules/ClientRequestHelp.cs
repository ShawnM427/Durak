using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Durak.Common;

namespace DurakGame.Rules
{
    class ClientRequestHelp : IClientStateSetValidator
    {
        /// <summary>
        /// Tries setting the requested state parameter
        /// </summary>
        /// <param name="parameter">The state parameter that is being requested</param>
        /// <param name="server">The server to excecute on</param>
        /// <param name="sender">The player that is requesting the state change</param>
        public void TrySetState(StateParameter parameter, GameServer server, Player sender)
        {
            if (parameter.Name == Names.REQUEST_HELP && parameter.ParameterType == StateParameter.Type.Bool)
            {
                if (server.GameState.GetValueInt(Names.ATTACKING_PLAYER) == sender.PlayerId)
                {
                    server.GameState.Set(Names.REQUEST_HELP, parameter.GetValueBool());
                }
            }
        }
    }
}
