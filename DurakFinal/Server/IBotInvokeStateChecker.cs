using Durak.Common;

namespace Durak.Server
{
    /// <summary>
    /// Represents the interface used to verify whether a bot should be invoked at the end of the current message pump
    /// </summary>
    public interface IBotInvokeStateChecker
    {
        /// <summary>
        /// Checks whether 
        /// </summary>
        /// <param name="server">The server to execute on</param>
        /// <param name="player">The bot  player to check for</param>
        /// <returns>True if the bot should be invoked, false if otherwise</returns>
        bool ShouldInvoke(GameServer server, BotPlayer player);
    }
}
