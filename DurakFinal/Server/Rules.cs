using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;

namespace Durak.Server
{
    public static class Rules
    {
        public static readonly List<IGameStateRule> STATE_RULES;
        public static readonly List<IGameInitRule> INIT_RULES;
        public static readonly List<IGamePlayRule> PLAY_RULES;
        public static readonly List<IMoveSucessRule> MOVE_SUCCESS_RULES;
        public static readonly List<IAIRule> AI_RULES;
        public static readonly List<IBotInvokeStateChecker> BOT_INVOKE_RULES;

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

            BOT_INVOKE_RULES = new List<IBotInvokeStateChecker>();
            Utils.FillTypeList(AppDomain.CurrentDomain, BOT_INVOKE_RULES);

            MOVE_SUCCESS_RULES = new List<IMoveSucessRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, MOVE_SUCCESS_RULES);
        }

    }
}
