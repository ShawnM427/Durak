using Durak.Common;

namespace Durak.Server
{
    /// <summary>
    /// Represents a rule to be exceuted on a sucessfull move
    /// </summary>
    public interface IMoveSucessRule
    {
        /// <summary>
        /// Updates the state with regards to the move
        /// </summary>
        /// <param name="server">The server to execute on</param>
        /// <param name="move">The move that was made</param>
        void UpdateState(GameServer server, GameMove move);
    }
}
