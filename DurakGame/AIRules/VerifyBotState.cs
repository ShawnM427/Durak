using Durak.Server;
using System;
using Durak.Common;

namespace DurakGame.Rules
{
    /// <summary>
    /// This class is used to check the state of each bot after each message pump to determine the state, wheather they should be attacking, defending, asking for help 
    /// </summary>
    public class VerifyBotState : IBotInvokeStateChecker
    {
        public bool ShouldInvoke(GameState state, BotPlayer player)
        {
            return 
                //Check if the attacking players id is equal to the bots id and it is currently attacking
                (state.GetValueByte(Names.ATTACKING_PLAYER) == player.Player.PlayerId & state.GetValueBool(Names.IS_ATTACKING)) | 
                
                //Check if the defending players id is equal to the bots id and if it is currently defending 
                (state.GetValueByte(Names.DEFENDING_PLAYER) == player.Player.PlayerId & !state.GetValueBool(Names.IS_ATTACKING)) | 

                //Checks the games state, if a player requires help and the bot players has the same value of the attacking player, this bot is asking for help
                (state.GetValueBool(Names.REQUEST_HELP) == true && state.GetValueByte(Names.DEFENDING_PLAYER) != player.Player.PlayerId);
        }
    }
}
