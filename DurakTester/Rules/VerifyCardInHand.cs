﻿using Durak.Common;
using Durak.Server;

namespace DurakTester.Rules
{
    public class VerifyCardInHand : IGamePlayRule
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
                return "Verify card in Hand";
            }
        }

        public bool IsValidMove(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            if (!move.Player.Hand.Contains(move.Move))
            {
                reason = "Card is not in players hand";
                return false;
            }
            else
            {
                move.Player.Hand.Remove(move.Move);
                int round = 0; // currentState.GetValueInt("current_round");
                currentState.Set("round_" + round + "_attack", move.Move);
            }

            return true;
        }
    }
}