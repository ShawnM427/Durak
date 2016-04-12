using Durak.Common;
using Durak.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Durak.Server
{
    /// <summary>
    /// Represents a AI bot that performs actions on behalf of a player
    /// </summary>
    public class BotPlayer
    {
        /// <summary>
        /// Gets or sets whether bots should take time to make their decision
        /// </summary>
        public static bool SimulateThinkTime = false;
        /// <summary>
        ///  The minimum time for a bot to make a decision
        /// </summary>
        public static int ThinkSleepMinTime = 0;
        /// <summary>
        /// The maximum time for a bot to make a decision
        /// </summary>
        public static int ThinkSleepMaxTime = 5000;

        /// <summary>
        /// Stores whether or not this bot should invoke on the end of the current message pump
        /// </summary>
        private bool shouldInvoke;
        /// <summary>
        /// Stores the player this bot represents
        /// </summary>
        private Player myPlayer;
        /// <summary>
        /// Stores the game server the bot is operating on
        /// </summary>
        private GameServer myServer;
        /// <summary>
        /// Stores the bot's difficulty in the range of 0-1
        /// </summary>
        private float myDifficulty;
        /// <summary>
        /// Stores the RNG for this bot
        /// </summary>
        private Random myRandom;
        /// <summary>
        /// Stores the timer instance for waiting on callback result. We wait for the timer before updating our shouldInvoke
        /// </summary>
        private Timer myTimer;
        /// <summary>
        /// Stores a dictionary of proposed moves, wiped when determining new move
        /// </summary>
        private Dictionary<PlayingCard, float> myProposedMoves;

        /// <summary>
        /// Gets or sets the difficulty for this bot in the range of 0-1
        /// 
        /// 0 is mentally challenged
        /// 1 is Stephen Hawking
        /// </summary>
        public float Difficulty
        {
            get { return myDifficulty; }
            set
            {
                // Clamp difficulty between 0 and 1
                myDifficulty = value < 0 ? 0 : value > 1 ? 1 : value;
            }
        }
        /// <summary>
        /// Gets whether this bot should be updating it's logic
        /// </summary>
        public bool ShouldInvoke
        {
            get { return shouldInvoke; }
        }
        /// <summary>
        /// Gets the player this bot is representing
        /// </summary>
        public Player Player
        {
            get { return myPlayer; }
        }

        /// <summary>
        /// Creates a new bot for the given game server and player
        /// </summary>
        /// <param name="server">The server this bot runs on</param>
        /// <param name="player">The player to wrap for this bot</param>
        public BotPlayer(GameServer server, Player player, float difficulty)
        {
            myServer = server;
            myPlayer = player;
            myDifficulty = difficulty;
            
            myRandom = new Random(player.PlayerId + DateTime.Now.Millisecond);
            myTimer = new Timer();
            myTimer.Elapsed += TimerElapsed;

            myProposedMoves = new Dictionary<PlayingCard, float>();
            player.Hand.OnCardRemoved += CardRemoved;
            player.Hand.OnCardAdded += CardAdded;
        }

        /// <summary>
        /// Invoked when a card has been removed from the player's hand
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="e">The card that has been removed</param>
        private void CardRemoved(object sender, CardEventArgs e)
        {
            if (myProposedMoves.ContainsKey(e.Card))
                myProposedMoves.Remove(e.Card);
        }

        /// <summary>
        /// Invoked when a card has been added to the player's hand
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="e">The card that has been added</param>
        private void CardAdded(object sender, CardEventArgs e)
        {
            e.Card.FaceUp = true;

            if (!myProposedMoves.ContainsKey(e.Card))
                myProposedMoves.Add(e.Card, 0.0f);
        }

        /// <summary>
        /// Invoked when the timer has elapsed, this updates our shouldInvoke after a given "think time" for bot
        /// </summary>
        /// <param name="sender">The timer</param>
        /// <param name="e">The elapsed arguments</param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            shouldInvoke = true;
            myTimer.Stop();
        }

        /// <summary>
        /// Invoked when a game state has been updated
        /// </summary>
        /// <param name="state">The game state</param>
        public void StateUpdated(GameState state)
        {
            // Override the shouldInvoke so we can't accidentally set it
            bool shouldInvoke = Rules.BOT_INVOKE_RULES.Count > 0;
            
            // Iterate over each state rule and set to false if one of them fails
            foreach(IBotInvokeStateChecker stateChecker in Rules.BOT_INVOKE_RULES)
            {
                // If one state check fails, they all fail
                if (!stateChecker.ShouldInvoke(state, this))
                {
                    shouldInvoke = false;
                    break;
                }
            }

            if (shouldInvoke & !myTimer.Enabled)
            {
                // If we simulate thinking time, then prep the timer and start it
                if (SimulateThinkTime)
                {
                    // Calculate a random interval based off min and max time and bot "skill"
                    myTimer.Interval = ThinkSleepMinTime + (myRandom.Next(0, ThinkSleepMaxTime - ThinkSleepMinTime) * (1 - myRandom.NextDouble() * (1 - myDifficulty)));
                    // Begin the timer
                    myTimer.Start();
                }
                else
                    // Copy method local to instance member
                    this.shouldInvoke = shouldInvoke;
            }
        }

        /// <summary>
        /// Determines the move this bot should make
        /// </summary>
        /// <returns>The playing card this bot wants to play</returns>
        public PlayingCard DetermineMove()
        {
            // Prepare a list of proposed moves
            for(int index = 0; index < myProposedMoves.Values.Count; index ++)
                myProposedMoves[myProposedMoves.Keys.ElementAt(index)] = 0;

            // The confidence of making no move
            float noMoveConfidence = 0;

            // Iterate over all rules
            foreach(IAIRule rule in Rules.AI_RULES)
            {
                // Get the proposed move
                rule.Propose(myProposedMoves, myServer.GameState, myPlayer.Hand);             
            }

            for(int index = 0; index < myProposedMoves.Count; index ++)
            {
                myProposedMoves[myProposedMoves.Keys.ElementAt(index)] *= (float)((myRandom.NextDouble() - 0.5d) * (1 - myDifficulty / 4));
            }

            // Sort the list
            List<KeyValuePair<PlayingCard, float>> sortedList = myProposedMoves.ToList();
            sortedList.Sort((X, Y) => -(X.Value.CompareTo(Y.Value)));

            // Reset this to avoid infinite loops, should be set too false when state updates though
            shouldInvoke = false;
            
            // Loop through list and attempt each move, if valid, then return that move
            for(int index = 0; index < sortedList.Count; index ++)
                if (sortedList[index].Value > noMoveConfidence && myServer.CanPlayMove(myPlayer, sortedList[index].Key))
                    return sortedList[index].Key;

            // If there was no good move, return null
            return null;
        }
    }
}
