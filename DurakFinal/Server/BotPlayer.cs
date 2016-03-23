using Durak.Common;
using Durak.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Durak.Server
{
    public class BotPlayer
    {
        private static readonly List<IAIRule> AI_RULES;

        /// <summary>
        /// Static constructor flor loading all AI rules
        /// </summary>
        static BotPlayer()
        {
            AI_RULES = new List<IAIRule>();
            Utils.FillTypeList<IAIRule>(AppDomain.CurrentDomain, AI_RULES);
        }
        
        private List<Tuple<string, object>> myStateInvokeOns;
        private bool shouldInvoke;


        public BotPlayer(EventHandler<StateParameter> stateChangedEvent)
        {
            stateChangedEvent += StateUpdated;
            myStateInvokeOns = new List<Tuple<string, object>>();
        }

        /// <summary>
        /// Invoked when a game state has been updated
        /// </summary>
        /// <param name="sender">The game state that invoked the event</param>
        /// <param name="changedState">The parameter that was changed</param>
        private void StateUpdated(object sender, StateParameter changedState)
        {
            // Reset should invoke
            shouldInvoke = myStateInvokeOns.Count > 0;

            // Get the state from the sender
            GameState state = sender as GameState;

            // Iterate over each state rule and set to false if one of them fails
            foreach(Tuple<string, object> stateRule in myStateInvokeOns)
            {
                if (!state.Equals(stateRule.Item1, stateRule.Item2))
                {
                    shouldInvoke = false;
                    break;
                }
            }
        }

        public PlayingCard DetermineMove(GameState state, CardCollection hand)
        {
            PlayingCard result = null;

            Dictionary<PlayingCard, float> proposedMoves = new Dictionary<PlayingCard, float>();

            foreach(IAIRule rule in AI_RULES)
            {
                AIMoveProposal proposal = rule.Propose(state, hand);

                if (proposedMoves.ContainsKey(proposal.Move))
                    proposedMoves[proposal.Move] += proposal.Confidence;
            }

            List<KeyValuePair<PlayingCard, float>> sortedList = proposedMoves.ToList();

            sortedList.Sort((X, Y) => X.Value.CompareTo(Y.Value));

            for(int index = 0; index < sortedList.Count; index ++)
            {
                // TODO validate move
            }

            return result;
        }
    }
}
