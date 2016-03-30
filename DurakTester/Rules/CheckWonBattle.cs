using Durak.Common;
using Durak.Server;

namespace DurakTester.Rules
{
    public class CheckWonBattle : IGamePlayRule
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
                return "Check won battle";
            }
        }

        public bool IsValidMove(PlayerCollection players, GameMove move, GameState currentState, ref string reason)
        {
            //if (move.Move.Suit == currentState.GetValueCardSuit("defending_card_suit") && move.Move.Rank > currentState.GetValueCardRank("defending_card_rank"))
            //{
              // currentState.Set("round_winner", move.Player.PlayerId);
               //currentState.Set("current_round", (byte)(currentState.GetValueByte("current_round") + 1));

                return true;
            //}
            //else
            //{
             //   reason = "Move is not valid";
              //  return false;
            //}
        }
    }
}
