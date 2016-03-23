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
        /// <param name="move">The move that was made</param>
        /// <param name="players">The list of current players</param>
        /// <param name="state">The current game's state</param>
        void UpdateState(GameMove move, PlayerCollection players, GameState state);
    }
}
