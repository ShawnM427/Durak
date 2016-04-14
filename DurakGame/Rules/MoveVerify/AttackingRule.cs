using Durak.Common;
using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    class AttackingRule : IGamePlayRule
    {
        public bool IsEnabled
        {
            get;
            set;
        }

        public string ReadableName
        {
            get
            {
                return "Rule for Atacker";
            }
        }

        public bool IsValidMove(GameServer server, GameMove move, ref string reason)
        {
            if (server.GameState.GetValueBool(Names.IS_ATTACKING) && (move.Player.PlayerId == server.GameState.GetValueByte(Names.ATTACKING_PLAYER) || server.GameState.GetValueBool(Names.REQUEST_HELP)))
            {
                if (move.Move == null)
                    return true;

                int round = server.GameState.GetValueInt(Names.CURRENT_ROUND);

                if (round == 0)
                {
                    return true;
                }
                else
                {
                    bool canPlay = false;

                    for (int index = 0; index < round; index++)
                    {
                        if (
                            move.Move.Rank == server.GameState.GetValueCard(Names.ATTACKING_CARD, index).Rank ||
                            move.Move.Rank == server.GameState.GetValueCard(Names.DEFENDING_CARD, index).Rank)
                            canPlay = true;
                    }

                    if (!canPlay)
                        reason = "You must play a card with a rank that has already been played";

                    return canPlay;
                }
            }
            else if (!server.GameState.GetValueBool(Names.IS_ATTACKING))
                return true;
            else
                return false;
        }
    }
}
