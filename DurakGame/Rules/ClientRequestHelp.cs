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
        public void TrySetState(StateParameter parameter, Player sender, PlayerCollection players, GameState state)
        {
            if (parameter.Name == Names.REQUEST_HELP && parameter.ParameterType == StateParameter.Type.Bool)
            {
                if (state.GetValueInt(Names.ATTACKING_PLAYER) == sender.PlayerId)
                {
                    state.Set(Names.REQUEST_HELP, parameter.GetValueBool());
                }
            }
        }
    }
}
