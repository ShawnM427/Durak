using System;

namespace Durak.Common
{
    /// <summary>
    /// Represents the state of a game server
    /// </summary>
    public enum ServerState
    {
        /// <summary>
        /// The server is in the lobby and is accepting connections
        /// </summary>
        InLobby,
        /// <summary>
        /// The server is in game
        /// </summary>
        InGame
    }
}
