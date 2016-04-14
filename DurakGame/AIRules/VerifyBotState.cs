using Durak.Server;
using System;
using Durak.Common;
using Durak.Common.Cards;

namespace DurakGame.Rules
{
    /// <summary>
    /// This class is used to check the state of each bot after each message pump to determine the state, wheather they should be attacking, defending, asking for help 
    /// </summary>
    public class VerifyBotState : IBotInvokeStateChecker
    {
        /// <summary>
        /// Implements the bot state check interface
        /// </summary>       
        /// <param name="server">The server to execute on</param>
        /// <param name="player">The bot to verify for</param>
        /// <returns>True if the bot should pick a move, false if otherwise</returns>
        public bool ShouldInvoke(GameServer server, BotPlayer player)
        {
            GameState state = server.GameState;

            if (state.GetValueBool(Names.GAME_OVER))
                return false;

            byte attackingPlayerID = state.GetValueByte(Names.ATTACKING_PLAYER);
            byte defendingPlayerId = state.GetValueByte(Names.DEFENDING_PLAYER);
            bool isAttacking = state.GetValueBool(Names.IS_ATTACKING);
            bool isRequestingHelp = state.GetValueBool(Names.REQUEST_HELP);
            int currentRound = state.GetValueInt(Names.CURRENT_ROUND);
            PlayingCard attackingCard = state.GetValueCard(Names.ATTACKING_CARD, currentRound);
            PlayingCard defendingCard = state.GetValueCard(Names.DEFENDING_CARD, currentRound);

            if (isAttacking)
            {
                if (attackingPlayerID == player.Player.PlayerId)
                    return true;
                else if (isRequestingHelp & player.Player.PlayerId != defendingPlayerId)
                    return true;
            }
            else
            {
                if (defendingPlayerId == player.Player.PlayerId && attackingCard != null)
                    return true;
            }

            return false;
        }
    }
}
