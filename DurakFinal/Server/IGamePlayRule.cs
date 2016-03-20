using Durak.Common;

namespace Durak.Server
{
    /// <summary>
    /// Represents the interface for a game rule that is used by the game logic
    /// server
    /// </summary>
    public interface IGamePlayRule
    {
        /// <summary>
        /// Gets or sets whether this rule is enabled
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a human readable name for this game rule. Used for debugging
        /// as well as logging and options
        /// </summary>
        string ReadableName { get; }

        /// <summary>
        /// Checks a proposed game move agains the current game state and returns whether it is valid
        /// </summary>
        /// <param name="move">The move to check</param>
        /// <param name="currentState">The current game state</param>
        /// <param name="reason">The reason why this move is invalid, should be set before returning</param>
        /// <returns>Tru if the move is valid, false if otherwise</returns>
        bool IsValidMove(GameMove move, GameState currentState, ref string reason);
    }
}
