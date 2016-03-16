using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurakFinal.Common;

namespace DurakFinal.Server.Rules
{
    public class VerifyCardInHand : IGameRule
    {
        public string ReadableName
        {
            get
            {
                return "Verify card in Hand";
            }
        }

        public bool IsValidMove(GameMove move, GameState currentState, ref string reason)
        {
            if (!move.Player.Hand.ContainsCards(move.Move))
            {
                reason = "Card is not in players hand";
                return false;
            }

            return true;
        }
    }
}
