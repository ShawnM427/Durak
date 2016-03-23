using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common
{
    public static class Rules
    {
        public static readonly List<IGameStateRule> STATE_RULES;
        public static readonly List<IGameInitRule> INIT_RULES;
        public static readonly List<IGamePlayRule> PLAY_RULES;
        public static readonly List<IAIRule> AI_RULES;

        static Rules()
        {
            STATE_RULES = new List<IGameStateRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, STATE_RULES);

            INIT_RULES = new List<IGameInitRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, INIT_RULES);

            PLAY_RULES = new List<IGamePlayRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, PLAY_RULES);

            AI_RULES = new List<IAIRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, AI_RULES);
        }

    }
}
