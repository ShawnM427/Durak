using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;

namespace Durak.Server
{
    /// <summary>
    /// A utility class that creates instances of all the Rule types
    /// </summary>
    public static class Rules
    {
        /// <summary>
        /// Gets a list of all the Game State validator rules
        /// </summary>
        public static readonly List<IGameStateRule> STATE_RULES;
        /// <summary>
        /// Gets a list of all the Game initialization rules
        /// </summary>
        public static readonly List<IGameInitRule> INIT_RULES;
        /// <summary>
        /// Gets a list of all Game play move rules
        /// </summary>
        public static readonly List<IGamePlayRule> PLAY_RULES;
        /// <summary>
        /// Gets a list of all Rules to be excecuted after a successfull move was made
        /// </summary>
        public static readonly List<IMoveSucessRule> MOVE_SUCCESS_RULES;
        /// <summary>
        /// Gets a list of all Bot rules that make up the bot AI
        /// </summary>
        public static readonly List<IAIRule> AI_RULES;
        /// <summary>
        /// Gets a list of all Bot Invoke rules that determine when a bot should be invoked
        /// </summary>
        public static readonly List<IBotInvokeStateChecker> BOT_INVOKE_RULES;
        /// <summary>
        /// Gets a list of all Server Side validators that validate what states a client can set 
        /// </summary>
        public static readonly List<IClientStateSetValidator> CLIENT_STATE_REQ_VALIDATORS;

        /// <summary>
        /// Static initializer, loads all types
        /// </summary>
        static Rules()
        {
            STATE_RULES = new List<IGameStateRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, STATE_RULES);

            INIT_RULES = new List<IGameInitRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, INIT_RULES);
            INIT_RULES.Sort((X, Y) => { return Y.Priority.CompareTo(X.Priority); });

            PLAY_RULES = new List<IGamePlayRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, PLAY_RULES);

            AI_RULES = new List<IAIRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, AI_RULES);

            CLIENT_STATE_REQ_VALIDATORS = new List<IClientStateSetValidator>();
            Utils.FillTypeList(AppDomain.CurrentDomain, CLIENT_STATE_REQ_VALIDATORS);

            BOT_INVOKE_RULES = new List<IBotInvokeStateChecker>();
            Utils.FillTypeList(AppDomain.CurrentDomain, BOT_INVOKE_RULES);

            MOVE_SUCCESS_RULES = new List<IMoveSucessRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, MOVE_SUCCESS_RULES);
        }

    }
}
