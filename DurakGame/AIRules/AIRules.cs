using Durak.Server;
using System.Linq;
using Durak.Common;
using Durak.Common.Cards;
using System.Collections.Generic;

namespace DurakGame.Rules
{
    /// <summary>
    /// Represents the Core AI rule that determines the best card to play
    /// </summary>
    public class AIRules : IAIRule
    {
        /// <summary>
        /// A method that finds what the player bot should be doing and acts accordingly
        /// </summary>
        /// <param name="proposals">The dictionary of all cards in the bots hand, and their confidence in playing that card</param>        
        /// <param name="server">The server to execute on</param>
        /// <param name="hand">The bot's hand</param>
        public void Propose(Dictionary<PlayingCard, float> proposals, GameServer server, CardCollection hand)
        {
            GameState state = server.GameState;

            // Get trump card suit
            CardSuit trumpSuit = state.GetValueCard(Names.TRUMP_CARD).Suit;

            // Determine the cards we are working with
            PlayingCard[] keys = proposals.Keys.ToArray();

            //Bot player is in attacking mode
            if (state.GetValueBool(Names.IS_ATTACKING))
            {
                foreach (PlayingCard key in keys)
                {
                    //Auto set all cards proposal to 1.0
                    proposals[key] += .50f;
                    if (key.Suit == trumpSuit)
                    {
                        proposals[key] -= .25f;
                    }
                    ////If its not the first round do logic to only use cards that can be played based on pervious cards
                    if (state.GetValueInt(Names.CURRENT_ROUND) != 0)
                    {
                        for (int i = 0; i >= state.GetValueInt(Names.CURRENT_ROUND); i++)
                        {
                            //If the card does not share the same rank as the attacking or defending card or does not share the same suit as the trump
                            if (state.GetValueCard(Names.ATTACKING_CARD, i).Rank != key.Rank || state.GetValueCard(Names.DEFENDING_CARD, i).Rank == key.Rank || key.Suit == trumpSuit)
                            {
                                proposals[key] = 0.0f;
                            }
                        }
                    }
                }

            }

            //Bot Player is defending
            else
            {
                PlayingCard attackingCard = state.GetValueCard(Names.ATTACKING_CARD, state.GetValueInt(Names.CURRENT_ROUND));

                foreach (PlayingCard key in keys)
                {
                    //Add weight to all cards of the attacking cards suit and trump cards suit
                    if (key.Suit == trumpSuit | key.Suit == attackingCard.Suit)
                    {
                        proposals[key] += .25f;
                        //Add a little more weight if the card is not a trump card. So the bot has less chance to waste its trumps
                        if (key.Suit != trumpSuit)
                        {
                            proposals[key] += .25f;
                        }
                        //Add weight if the rank is higher than the attacking cards rank
                        if (key.Rank > attackingCard.Rank)
                        {
                            proposals[key] += .25f - ((int)key.Rank / 100.0f);
                        }
                        //If the cards rank is less than the attacking card and the suits are the same remove weight (meaning trump cards with a lower rank will still have weight)
                        else if (key.Rank < attackingCard.Rank && key.Suit == attackingCard.Suit)
                        {
                            proposals[key] = 0.0f;
                        }
                    }
                }
            }         
        }
    }
}
