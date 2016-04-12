using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Server
{
    /// <summary>
    /// Represents the interface used by the server to determine
    /// </summary>
    public interface IClientSettableState
    {
        void TrySetState(StateParameter parameter, Player sender, PlayerCollection players, GameState state);
    }
}
