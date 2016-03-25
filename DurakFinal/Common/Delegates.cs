using Lidgren.Network;

namespace Durak.Common
{
    /// <summary>
    /// Delegate for handling when a new server is discovered
    /// </summary>
    /// <param name="sender">The object that raised the event</param>
    /// <param name="tag">The server tag of the server that was discovered</param>
    public delegate void ServerDiscoveredEvent(object sender, ServerTag tag);

    /// <summary>
    /// Delegate for handling when a player has left a game
    /// </summary>
    /// <param name="sender">The object that raised the event</param>
    /// <param name="player">The player to leave</param>
    /// <param name="reason">The reason the player was left</param>
    public delegate void PlayerLeftEvent(object sender, Player player, string reason);

    /// <summary>
    /// Delegate for handling when a player has sent a chat message
    /// </summary>
    /// <param name="sender">The object that raised the event</param>
    /// <param name="player">The player to send the message</param>
    /// <param name="message">The message the player sent</param>
    public delegate void PlayerChatEvent(object sender, Player player, string message);

    /// <summary>
    /// Represents a method or event that handles incoming network packets
    /// </summary>
    /// <param name="msg">The message to handle</param>
    public delegate void PacketHandler(NetIncomingMessage msg);
}
