using Durak.Common;
using Durak.Common.Cards;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class DrawCardsRule : IGameInitRule
    {
        public bool IsEnabled
        {
            get;
            set;
        }

        public string ReadableName
        {
            get { return "Deal cards"; }
        }

        public void InitState(PlayerCollection players, GameState state)
        {
            Deck toDrawFrom = new Deck(state.GetValueCardCollection("source_deck"));
        }
    }
}
