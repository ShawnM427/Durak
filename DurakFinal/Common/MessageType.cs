
namespace Durak.Common
{
    /// <summary>
    /// Represents the type for a given message
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Sent by the host client to request that the game start      
        ///                                                             
        /// Payload:                                                    
        ///     boolean - start                                         
        /// </summary>
        HostReqStart = 31,
        /// <summary>
        /// Sent by the host client to request that a bot is added
        /// 
        /// Payload:
        ///     byte   - difficulty
        ///     string - botName
        /// </summary>
        HostReqAddBot               = 32,
        /// <summary>
        /// Sent by the host client to request that a bot is deleted
        /// 
        /// Payload:
        ///     byte   - playerId
        ///     string - reason
        /// </summary>
        HostReqKick                 = 33,

        /// <summary>
        /// Sent by the server to notify that a game state parameter has changed
        /// 
        /// Payload:
        ///     string  - parameter name
        ///     byte    - parameter type
        ///     T       - parameter value
        /// </summary>
        GameStateChanged            = 50,
        /// <summary>
        /// Sent by the server to transfer the entire game state to a client
        /// 
        /// Payload:
        ///     int     - num params
        ///         string  - param 1 name
        ///         byte    - param 1 type
        ///         T       - param 1 value
        ///         string  - param 2 name
        ///         byte    - param 2 type
        ///         T       - param 2 value
        ///         ...
        /// </summary>
        FullGameStateTransfer       = 51,

        /// <summary>
        /// Sent by the client to request the state of the server (lobby, in game)
        /// </summary>
        RequestServerState          = 60,
        /// <summary>
        /// Sent by the server to notify the client that the server is in an invalid state for a request the client has made
        /// 
        /// Payload:
        ///     string  - reason
        /// </summary>
        InvalidServerState          = 61,
        /// <summary>
        /// Sent by the server to notify clients that the server's state has changed
        /// 
        /// Payload:
        ///     byte    - newState
        ///     string  - reason
        /// </summary>
        NotifyServerStateChanged    = 62,
        
        /// <summary>
        /// Sent by the server to notify clients that a player has joined
        /// 
        /// Payload:
        ///     byte    - playerId
        ///     string  - playerName
        /// </summary>
        PlayerJoined                = 130,
        /// <summary>
        /// Sent by the server to contify clients that a player has left
        /// 
        /// Payload:
        ///     byte    - playerId
        ///     string  - reason
        /// </summary>
        PlayerLeft                  = 131,
        /// <summary>
        /// Sent by the client to notify the server that a client is ready for the game.
        /// Sent by the server to notify clients of a player's ready status
        /// 
        /// Payload:
        ///     byte    - playerId
        ///     boolean - isReady
        /// </summary>
        PlayerReady                 = 132,
        /// <summary>
        /// Sent by the server to notify clients that a player has been kicked
        /// 
        /// Payload:
        ///     byte   - playerId
        ///     string - reason
        /// </summary>
        PlayerKicked                = 133,
        /// <summary>
        /// Send by the server to a client once it connects
        /// 
        /// Payload:
        ///     byte    - playerId
        ///     boolean - isHost
        /// </summary>
        PlayerConnectInfo = 134,

        /// <summary>
        /// Sent by the client to request a card to be played
        /// 
        /// Payload:
        ///     byte    - playerId
        ///     byte    - card rank
        ///     byte    - card suit
        /// </summary>
        SendMove                    = 150,
        /// <summary>
        /// Sent by the server to notify a client that their requested move is invalid
        /// 
        /// Payload:
        ///     byte    - playerId
        ///     byte    - card rank
        ///     byte    - card suit
        ///     string  - reason
        /// </summary>
        InvalidMove                 = 151,
        /// <summary>
        /// Sent by the server to ntify clients that a player has made a game move
        /// 
        /// Payload:
        ///     byte    - playerId
        ///     byte    - card rank
        ///     byte    - card suit
        /// </summary>
        SucessfullMove              = 152,

        /// <summary>
        /// Sent by the clients and the server for chat messages
        /// 
        /// Payload:
        ///     byte    - playerId
        ///     String  - message
        /// </summary>
        PlayerChat                  = 200
    }
}
