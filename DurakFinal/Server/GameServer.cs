using Durak.Common;
using Durak.Common.Cards;
using Durak.Properties;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Durak.Server
{
    /// <summary>
    /// Represents a game server that handles networking clients and running the game logic
    /// </summary>
    public class GameServer
    {
        private static readonly string[] BOT_NAMES;

        static GameServer()
        {
            BOT_NAMES = Resources.names_corrected.ToLower().Split('\n');
        }

        /// <summary>
        /// Stores this server's IP address
        /// </summary>
        private IPAddress myAddress;
        /// <summary>
        /// The output text box for the server log
        /// </summary>
        private RichTextBox myOutput;
        /// <summary>
        /// Stores this server's server tag
        /// </summary>
        private ServerTag myTag;
        /// <summary>
        /// Stores the list of gameplay rules to use
        /// </summary>
        private List<IGamePlayRule> myPlayRules;
        /// <summary>
        /// Stores the list of game state rules to use
        /// </summary>
        private List<IGameStateRule> myStateRules;
        /// <summary>
        /// Stores the list of game initialization rules to use
        /// </summary>
        private List<IGameInitRule> myInitRules;
        /// <summary>
        /// Stores the packet handling methods
        /// </summary>
        private Dictionary<MessageType, PacketHandler> myMessageHandlers;
        /// <summary>
        /// Stores the list of players currently connected
        /// </summary>
        private PlayerCollection myPlayers;
        /// <summary>
        /// Stores the network peer
        /// </summary>
        private NetPeer myServer;
        /// <summary>
        /// Stores the SHA256 of the server's password
        /// </summary>
        private string myPassword;
        /// <summary>
        /// Stores the server's current state
        /// </summary>
        private ServerState myState;
        /// <summary>
        /// Stores the game's state
        /// </summary>
        private GameState myGameState;
        /// <summary>
        /// Stores the player that has control over this game
        /// </summary>
        private Player myGameHost;
        /// <summary>
        /// Stores all the bots operating on this server
        /// </summary>
        private List<BotPlayer> myBots;
        /// <summary>
        /// Stores whether this server is in singleplayer mode (only accepting local connections)
        /// </summary>
        private bool isSinglePlayer;

        /// <summary>
        /// Stores whether this server is set up for the current game
        /// </summary>
        private bool isInitialized = false;

        /// <summary>
        /// Gets the server's IP address
        /// </summary>
        public IPAddress IP
        {
            get { return myAddress; }
        }
        /// <summary>
        /// Gets or sets whether this server should long each rule
        /// </summary>
        public bool LogLongRules
        {
            get;
            set;
        }
        /// <summary>
        /// Get's this server's game state
        /// </summary>
        public GameState GameState
        {
            get
            {
                return myGameState;
            }
        }
        /// <summary>
        /// Gets or sets this server's name as discplayed in server browsers
        /// </summary>
        public string Name
        {
            get { return myTag.Name; }
            set { myTag.Name = value; }
        }
        /// <summary>
        /// Gets or sets a short description for this server
        /// </summary>
        public string Description
        {
            get { return myTag.Description; }
            set { myTag.Description = value; }
        }
        /// <summary>
        /// Sets the password for this server. As it is hashed, we cannot retreive it
        /// </summary>
        public string Password
        {
            set { myPassword = SecurityUtils.Hash(value); }
        }
        /// <summary>
        /// Gets this server instance's tag
        /// </summary>
        public ServerTag Tag
        {
            get { return myTag; }
        }
        /// <summary>
        /// Gets or sets whether this server is in singleplayer mode (only accepts local connections)
        /// </summary>
        public bool IsSinglePlayerMode
        {
            get { return isSinglePlayer; }
            set
            {
                if (myServer.Status == NetPeerStatus.NotRunning)
                    isSinglePlayer = value;
                else
                    throw new ArgumentException("Cannot set server to singleplayer after server has been started");
            }
        }

        /// <summary>
        /// Gets or sets the number of players this server supports
        /// </summary>
        public int NumPlayers
        {
            get { return myPlayers.Count; }
            set
            {
                if (NumPlayers < value)
                {
                    myTag.SupportedPlayerCount = value;
                    myPlayers.Resize(value);
                }
                else if (NumPlayers > value)
                {
                    throw new InvalidOperationException("Cannot shrink player collection");
                }
            }
        }
        public ServerState State
        {
            get
            {
                if (myServer.Status != NetPeerStatus.Running)
                    return ServerState.NotRunning;
                else
                    return myState;
            }
        }

        /// <summary>
        /// Creates a new instance of a game server
        /// </summary>
        public GameServer(int numPlayers = 4)
        {
            myTag = new ServerTag();
            myTag.SupportedPlayerCount = numPlayers;

            myPlayRules = new List<IGamePlayRule>();
            myStateRules = new List<IGameStateRule>();
            myInitRules = new List<IGameInitRule>();

            myPlayers = new PlayerCollection(numPlayers);
            myBots = new List<BotPlayer>();

            myState = ServerState.InLobby;
            myGameState = new GameState();
            myGameState.OnStateChanged += MyGameState_OnStateChanged;

            myMessageHandlers = new Dictionary<MessageType, PacketHandler>();

            InitServer();
        }

        #region Initialization

        /// <summary>
        /// Sets the password for this server
        /// </summary>
        /// <param name="plainTextPassword">The server's password in plain text</param>
        public void SetPassword(string plainTextPassword)
        {
            if (!string.IsNullOrEmpty(plainTextPassword))
            {
                // Hashes the password and stores it
                myPassword = plainTextPassword.Hash();

                myTag.PasswordProtected = true;
            }
            else
            {
                myPassword = null;
                myTag.PasswordProtected = false;
            }
        }

        /// <summary>
        /// Sets the control to send log output to
        /// </summary>
        /// <param name="control">The control to log to</param>
        public void SetOutput(RichTextBox control)
        {
            myOutput = control;
        }

        /// <summary>
        /// Initializes the server 
        /// </summary>
        private void InitServer()
        {
            // Create a new net config
            NetPeerConfiguration netConfig = new NetPeerConfiguration(NetSettings.APP_IDENTIFIER);

            myAddress = NetUtils.GetAddress();

            // Allow incoming connections
            netConfig.AcceptIncomingConnections = true;
            // Set the ping interval
            netConfig.PingInterval = NetSettings.DEFAULT_SERVER_TIMEOUT / 10.0f;
            // Set the address
            netConfig.LocalAddress = myAddress;
            // Set the timeout between heartbeats before a client is considered disconnected
            netConfig.ConnectionTimeout = NetSettings.DEFAULT_SERVER_TIMEOUT;
            // Set the maximum number of connections to the number of players
            netConfig.MaximumConnections = myPlayers.Count;
            // Set the port to use
            netConfig.Port = NetSettings.DEFAULT_SERVER_PORT;
            // We want to recycle old messages (improves performance)
            netConfig.UseMessageRecycling = true;

            // We want to accept Connection Approval messages (requests for connection)
            netConfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            // We want to accept data (duh)
            netConfig.EnableMessageType(NetIncomingMessageType.Data);
            // We want to accept discovery requests
            netConfig.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            // We want to accept status change messages (client connect / disconnect)
            netConfig.EnableMessageType(NetIncomingMessageType.StatusChanged);
            // We want the connection latency updates (heartbeats)
            netConfig.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);

            // Create the network peer
            myServer = new NetServer(netConfig);
            
            // Register the callback function. Lidgren will handle the threading for us
            myServer.RegisterReceivedCallback(new SendOrPostCallback(MessageReceived));

            // Collect the rule types from tis assembly
            LoadRules();

            // Add the message handlers... Would be nicer w/ reflection and custom attributes, but whatever
            myMessageHandlers.Add(MessageType.SendMove, HandleGameMove);
            myMessageHandlers.Add(MessageType.HostReqStart, HandleHostReqStart);
            myMessageHandlers.Add(MessageType.RequestServerState, HandleStateRequest);
            myMessageHandlers.Add(MessageType.PlayerReady, HandlePlayerReady);
            myMessageHandlers.Add(MessageType.HostReqAddBot, HandleHostReqBot);
            myMessageHandlers.Add(MessageType.HostReqKick, HandleHostReqKick);
            myMessageHandlers.Add(MessageType.PlayerChat, HandlePlayerChat);
        }

        /// <summary>
        /// Loads all the game rules
        /// </summary>
        private void LoadRules()
        {
            Utils.FillTypeList(AppDomain.CurrentDomain, myPlayRules);
            Utils.FillTypeList(AppDomain.CurrentDomain, myStateRules);
            myInitRules = Rules.INIT_RULES;
        }

        /// <summary>
        /// Starts up this server to start accepting messages
        /// </summary>
        public void Run()
        {
            Log("Starting server");

            // Simply start the server
            myServer.Start();

            Log("Server Started on {0}:{1}", myAddress, myServer.Configuration.Port);
        }

        /// <summary>
        /// Stops this server
        /// </summary>
        public void Stop()
        {
            // Shut down the underlying server
            myServer.Shutdown(NetSettings.DEFAULT_SERVER_SHUTDOWN_MESSAGE);

            Log("Stopping server");
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets whether a player can play a card, this simply returns true or false, and does not excecute or log it
        /// </summary>
        /// <param name="player">The player that wants to play the move</param>
        /// <param name="card">The card to play</param>
        /// <returns>True if the move can be played, false if otherwise</returns>
        public bool CanPlayMove(Player player, PlayingCard card)
        {
            // Make sure it's face up... we prolly don't even need faceup in card anymore serverside
            card.FaceUp = true;

            // Make the move
            GameMove move = new GameMove(player, card);

            // We need this :/
            string failReason = "";

            // Iterate over each game rule
            for (int index = 0; index < myPlayRules.Count; index++)
            {
                // If the move is valid, continue, otherwise a rule was violated
                if (!myPlayRules[index].IsValidMove(myPlayers, move, myGameState, ref failReason))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Invoked by game state when a parameter has changed
        /// </summary>
        /// <param name="sender">The object to invoke this method</param>
        /// <param name="e">The event arguments</param>
        private void MyGameState_OnStateChanged(object sender, StateParameter e)
        {
            if (myState == ServerState.InGame && e.IsSynced)
            {
                // Prepare the game state changed
                NetOutgoingMessage msg = myServer.CreateMessage();
                msg.Write((byte)MessageType.GameStateChanged);
                e.Encode(msg);

                // Send to all clients
                SendToAll(msg);
            }
        }

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="message">The message to log</param>
        private void Log(string message, params object[] format)
        {
            // Log it
            Logger.Write(message, format);

            // If we have an output control, log the text
            if (myOutput != null)
                myOutput.AppendText(string.Format(message, format) + "\n");
        }

        /// <summary>
        /// Sets the game state for the server and updates all the clients
        /// </summary>
        /// <param name="state">The new server state</param>
        /// <param name="reason">The reason for the state change</param>
        private void SetServerState(ServerState state, string reason = "Game Started")
        {
            // Only update if the state actually changed
            if (myState != state)
            {
                // update the state
                myState = state;

                // Notify clients
                NetOutgoingMessage updateMessage = myServer.CreateMessage();
                updateMessage.Write((byte)MessageType.NotifyServerStateChanged);
                updateMessage.Write((byte)state);
                updateMessage.Write(reason);
                SendToAll(updateMessage);

                // If we are now in game
                if (state == ServerState.InGame)
                {
                    Log("Starting game");

                    // Turn the rules to silent mode
                    myGameState.SilentSets = true;

                    // Call all the init rules
                    for (int index = 0; index < myInitRules.Count; index++)
                        myInitRules[index].InitState(myPlayers, myGameState);

                    // Disable silent mode
                    myGameState.SilentSets = false;

                    // Transfer the game state
                    TransferGameState();

                    // Flag the game as initialized
                    isInitialized = true;
                }
                else if (state == ServerState.InLobby)
                {
                    // We clear the game state
                    myGameState.Clear();
                    myBots.Clear();
                    myPlayers.Clear();
                    isInitialized = false;
                }
            }
        }

        /// <summary>
        /// Transfer the entire game state to all clients
        /// </summary>
        private void TransferGameState()
        {
            // Prepare the message
            NetOutgoingMessage msg = myServer.CreateMessage();
            msg.Write((byte)MessageType.FullGameStateTransfer);
            myGameState.Encode(msg);

            // Sends to all clients
            SendToAll(msg);
        }

        /// <summary>
        /// Notifies a connection that the server is currently in the wrong state for that message
        /// </summary>
        /// <param name="connection">The connection to respond to</param>
        /// <param name="reason">The reason for the bad state</param>
        private void NotifyBadState(NetConnection connection, string reason)
        {
            // Create the message
            NetOutgoingMessage outMsg = myServer.CreateMessage();

            // Write the header and the bad move to the packet
            outMsg.Write((byte)MessageType.InvalidServerState);
            outMsg.Write(reason);

            // Send the packet
            myServer.SendMessage(outMsg, connection, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Handles when a player has left
        /// </summary>
        /// <param name="player">The player that left</param>
        /// <param name="reason">The reason that the player has left</param>
        private void PlayerLeft(Player player, string reason)
        {
            // Remove that player!
            myPlayers.Remove(player);

            // Create the outgioing message
            NetOutgoingMessage outMsg = myServer.CreateMessage();

            // Write the header and player ID 
            outMsg.Write((byte)MessageType.PlayerLeft);
            outMsg.Write(player.PlayerId);
            outMsg.Write(reason);

            // Update the tag
            myTag.PlayerCount = myPlayers.PlayerCount;

            // Send to all clients
            SendToAll(outMsg);

            // If this guy was the game host
            if (player == myGameHost)
            {
                // Work up the chain, this will naturally pick the second person to join
                for(byte index = 0; index < myPlayers.Count; index ++)
                {
                    // We only want the host to be not null
                    if (myPlayers[index] != null)
                    {
                        // Update the local variable
                        myGameHost = myPlayers[index];

                        // Notify clients
                        NotifyHostChanged(index);
                        break;
                    }
                }
            }

            // Get the player count
            int playersLeft = myServer.Connections.Count;

            // If there's no-one left, we return to lobby
            if (playersLeft == 0)
            {
                Log("All players left, returning to lobby");
                SetServerState(ServerState.InLobby, "Game empty");
            }
        }

        /// <summary>
        /// Notifies all connected clients that a new player has joined the game
        /// </summary>
        /// <param name="player">The player that has joined</param>
        private void NotifyPlayerJoined(Player player)
        {
            // Create the outgioing message
            NetOutgoingMessage outMsg = myServer.CreateMessage();

            // Write the header and move to the message
            outMsg.Write((byte)MessageType.PlayerJoined);
            outMsg.Write(player.PlayerId);
            outMsg.Write(player.Name);
            outMsg.Write(player.IsBot);
            outMsg.Write(player.IsHost);
            outMsg.WritePadBits();

            // Send to all clients
            SendToAll(outMsg);
        }

        /// <summary>
        /// Handles when a player has joined this server
        /// </summary>
        /// <param name="clientTag">The client's tag</param>
        /// <param name="connection">The connection for the new client</param>
        private void PlayerJoined(ClientTag clientTag, NetConnection connection)
        {
            // Get the ID of the new player
            int id = myPlayers.GetNextAvailableId();

            // Check to see if the server is full
            if (id != -1)
            {
                // Create the serverside player isntance
                Player player = new Player(clientTag, connection, (byte)id);

                player.OnCardAddedToHand += PlayerGainedCard;
                player.OnCardRemovedFromHand += PlayerRemovedCard;

                // If this is the first player, they are immediately the host
                if (id == 0)
                {
                    myGameHost = player;
                    player.IsHost = true;
                    Log("Setting host to \"{0}\"", player.Name);
                }

                // Client can connect
                connection.Approve();

                // Notify other players
                NotifyPlayerJoined(player);
                
                // Add the player to the player list
                myPlayers[player.PlayerId] = player;

                // Update the tag
                myTag.PlayerCount = myPlayers.PlayerCount;


            }
            else
            {
                // Deny the connection
                connection.Deny("Server is full");
            }
        }

        /// <summary>
        /// Invoked when a player hand has lost a card
        /// </summary>
        /// <param name="sender">The player that the event is for</param>
        /// <param name="e">The card that was removed</param>
        private void PlayerRemovedCard(object sender, PlayingCard e)
        {
            Player player = sender as Player;

            NotifyNewCardState(player, e, false);
        }

        /// <summary>
        /// Invoked when a player hand has gained a card
        /// </summary>
        /// <param name="sender">The player that the event is for</param>
        /// <param name="e">The card that was added</param>
        private void PlayerGainedCard(object sender, PlayingCard e)
        {
            Player player = sender as Player;

            NotifyNewCardState(player, e, true);
        }

        /// <summary>
        /// Notify the clients when a client's hand has changed
        /// </summary>
        /// <param name="player">The player that this card change is for</param>
        /// <param name="e">The card that was added/removed</param>
        /// <param name="added">True if this was added, false if it was removed</param>
        private void NotifyNewCardState(Player player, PlayingCard e, bool added)
        {
            // Prepare the message for the player that this change applies to
            NetOutgoingMessage msg = myServer.CreateMessage();
            msg.Write((byte)MessageType.PlayerHandChanged);
            msg.Write(added);
            msg.Write(e != null);
            msg.WritePadBits();
            if (e != null)
            {
                msg.Write((byte)e.Rank);
                msg.Write((byte)e.Suit);
            }
            myServer.SendMessage(msg, player.Connection, NetDeliveryMethod.ReliableOrdered, 0);

            // Prepare the message for the player that this change applies to
            NetOutgoingMessage msg2 = myServer.CreateMessage();
            msg2.Write((byte)MessageType.CardCountChanged);
            msg2.Write(player.PlayerId);
            msg2.Write(player.Hand.Count);
            SendToAll(msg2);
        }

        /// <summary>
        /// Sends the welcome packet to the specified client
        /// </summary>
        /// <param name="playerId">The ID of the player to send the message to</param>
        private void SendWelcomePacket(byte playerId)
        {
            // Gets the player with the given player ID
            Player player = myPlayers[playerId];

            // Prepare the message
            NetOutgoingMessage msg = myServer.CreateMessage();
            msg.Write((byte)MessageType.PlayerConnectInfo);
            msg.Write((byte)playerId);
            msg.Write((bool)(player == myGameHost));
            msg.WritePadBits();

            msg.Write(myPlayers.Count);
            msg.Write(myPlayers.PlayerCount);

            for (byte index = 0; index < myPlayers.Count; index++)
            {
                if (myPlayers[index] != null)
                    myPlayers[index].Encode(msg);
            }

            myGameState.Encode(msg);

            // Send the message to the client
            myServer.SendMessage(msg, player.Connection, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Handles a player making a game move
        /// </summary>
        /// <param name="move"></param>
        private void HandleMove(GameMove move)
        {
            // define the reason
            string failReason = "Unknown";

            Log("Player {0} wants to play {1}", move.Player.PlayerId, move.Move);

            // Iterate over each game rule
            for (int index = 0; index < myPlayRules.Count; index++)
            {
                // If the move is valid, continue, otherwise a rule was violated
                if (!myPlayRules[index].IsValidMove(myPlayers, move, myGameState, ref failReason))
                {
                    // If the person who made the rule sucked at making rules, we catch their mistake
                    if (failReason == "Unknown")
                        failReason = "Failed on " + myPlayRules[index].ReadableName;

                    // Notify the source user
                    NotifyInvalidMove(move, failReason);

                    if (LogLongRules)
                        Log("\tFailed rule \"{0}\": {1}", myPlayRules[index].ReadableName, failReason);
                    return; // Do not send to other clients, so break out of method
                }
                else if (LogLongRules)
                {
                    Log("\tPassed rule \"{0}\"", myPlayRules[index].ReadableName);
                }
            }

            Log("Move played");

            // Create the outgioing message
            NetOutgoingMessage outMsg = myServer.CreateMessage();

            // Write the header and move to the message
            outMsg.Write((byte)MessageType.SucessfullMove);
            move.Encode(outMsg);

            // Send to all connected clients
            SendToAll(outMsg);

            // Update all the sucessfull move states
            for (int index = 0; index < Rules.MOVE_SUCCESS_RULES.Count; index++)
            {
                Rules.MOVE_SUCCESS_RULES[index].UpdateState(move, myPlayers, myGameState);
            }
        }

        /// <summary>
        /// Notifies all clients that the game hsot has changed
        /// </summary>
        /// <param name="newHostId">The new host's player ID</param>
        private void NotifyHostChanged(byte newHostId)
        {
            NetOutgoingMessage msg = myServer.CreateMessage();
            msg.Write((byte)MessageType.HostChanged);
            msg.Write(newHostId);
            SendToAll(msg);
        }

        /// <summary>
        /// Notifies a client that they made an invalid move
        /// </summary>
        /// <param name="move">The move that was determined to be invalid</param>
        private void NotifyInvalidMove(GameMove move, string reason)
        {
            // Create the message
            NetOutgoingMessage outMsg = myServer.CreateMessage();

            // Write the header and the bad move to the packet
            outMsg.Write((byte)MessageType.InvalidMove);
            move.Encode(outMsg);
            outMsg.Write(reason);

            // Send the packet
            if (move.Player.Connection != null)
                myServer.SendMessage(outMsg, move.Player.Connection, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Sends a given network message to all connected clients
        /// </summary>
        /// <param name="msg">The message to send</param>
        private void SendToAll(NetOutgoingMessage msg)
        {
            if (myServer.Connections.Count > 0)
                myServer.SendMessage(msg, myServer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        #endregion

        #region Message Handling

        /// <summary>
        /// Handles when the server has received a message
        /// </summary>
        private void MessageReceived(object peer)
        {
            // Get the incoming message
            NetIncomingMessage inMsg = ((NetPeer)peer).ReadMessage();

            // We don't want the server to crash on one bad packet
            try
            {
                // Determine the message type to correctly handle it
                switch (inMsg.MessageType)
                {
                    // Handle when a client's status has changed
                    case NetIncomingMessageType.StatusChanged:
                        // Gets the status and reason
                        NetConnectionStatus status = (NetConnectionStatus)inMsg.ReadByte();
                        string reason = inMsg.ReadString();

                        // Depending on the status, we handle players joining or leaving
                        switch (status)
                        {
                            // A player has disconnected
                            case NetConnectionStatus.Disconnected:
                                PlayerLeft(myPlayers[inMsg.SenderConnection], reason);
                                break;
                            // A player is connecting
                            case NetConnectionStatus.Connected:
                                // Send the welcome packet
                                SendWelcomePacket(myPlayers[inMsg.SenderConnection].PlayerId);
                                break;
                        }

                        // Log the message
                        Log("Connection status updated for connection from {0}: {1}", inMsg.SenderEndPoint, status);

                        break;

                    // Handle when a player is trying to join
                    case NetIncomingMessageType.ConnectionApproval:

                        if (IsSinglePlayerMode & inMsg.SenderEndPoint.Address.ToString() != myAddress.ToString())
                            inMsg.SenderConnection.Deny("Server is in singleplayer mode");

                        // Get the client's info an hashed password from the packet
                        ClientTag clientTag = ClientTag.ReadFromPacket(inMsg);
                        string hashedPass = inMsg.ReadString();

                        // Make sure we are in the lobby when joining new players
                        if (myState == ServerState.InLobby)
                        {
                            // Check the password if applicable
                            if ((myTag.PasswordProtected && myPassword.Equals(hashedPass)) | (!myTag.PasswordProtected))
                            {
                                // Check to see if the lobby is full
                                if (myPlayers.GetNextAvailableId() != -1)
                                {
                                    // Go ahead and try to join that playa
                                    PlayerJoined(clientTag, inMsg.SenderConnection);
                                    Log("Player \"{0}\" joined from {1}", clientTag.Name, clientTag.Address);
                                }
                                else
                                {
                                    // Deny connection if lobby is full
                                    inMsg.SenderConnection.Deny("Game is full");
                                    Log("Player \"{0}\" was denied access to full game from {1}", clientTag.Name, clientTag.Address);
                                }
                            }
                            else
                            {
                                // Fuck you brah!
                                inMsg.SenderConnection.Deny("Password authentication failed");

                                Log("Player \"{0}\" failed to connect (password failed) from {1}", clientTag.Name, clientTag.Address);
                            }
                        }
                        else
                        {
                            // We are mid-way through a game
                            inMsg.SenderConnection.Deny("Game has already started");

                            Log("Player \"{0}\" attempted to connect mid game from {1}", clientTag.Name, clientTag.Address);
                        }
                        break;

                    // Handle when the server has received a discovery request
                    case NetIncomingMessageType.DiscoveryRequest:

                        // Prepare the response
                        NetOutgoingMessage msg = myServer.CreateMessage();
                        // Write the tag to the response
                        myTag.WriteToPacket(msg);
                        // Send the response
                        myServer.SendDiscoveryResponse(msg, inMsg.SenderEndPoint);

                        Log("Pinged discovery response to {0}", inMsg.SenderEndPoint);

                        break;

                    // Handles when the server has received data
                    case NetIncomingMessageType.Data:
                        HandleMessage(inMsg);
                        break;
                }
            }
            // An exception has occured parsing the packet
            catch (Exception e)
            {
                // Log the exception
                Log("Encountered exception parsing packet from {0}:\n\t{1}", inMsg.SenderEndPoint, e);
                Logger.Write(e);
            }

            // If we are in game, we should update the state
            if (myState == ServerState.InGame && isInitialized)
            {
                // Iterate over the rules and validate them all
                foreach (IGameStateRule stateRule in Rules.STATE_RULES)
                {
                    stateRule.ValidateState(myPlayers, myGameState);
                }

                // Handle the bots
                foreach (BotPlayer botPlayer in myBots)
                {
                    // Make bots detect if they are in a valid place to move
                    botPlayer.StateUpdated(myGameState);

                    // If the bot's move is ready
                    if (botPlayer.ShouldInvoke)
                    {
                        // Get and play the move
                        PlayingCard move = botPlayer.DetermineMove();
                        HandleMove(new GameMove(botPlayer.Player, move));
                    }
                }
            }
        }
                
        /// <summary>
        /// Handles an incoming network message that has already been determined to be data
        /// </summary>
        /// <param name="inMessage">The message to handle</param>
        private void HandleMessage(NetIncomingMessage inMessage)
        {
            // Keep trying to read as long as we have bytes available
            while(inMessage.PositionInBytes < inMessage.LengthBytes)
            {
                // Get the next message type
                MessageType messageType = (MessageType)inMessage.ReadByte();

                if (myMessageHandlers.ContainsKey(messageType))
                    myMessageHandlers[messageType].Invoke(inMessage);
                else
                {
                    // Logs the message
                    Log("Invalid message received from \"{0}\" ({1})", myPlayers[inMessage.SenderConnection].Name, inMessage.SenderEndPoint);
                    inMessage.ReadBytes(inMessage.LengthBytes - inMessage.PositionInBytes);
                }
            }
        }

        /// <summary>
        /// Handles the message received when the player requests a game move to be made
        /// </summary>
        /// <param name="msg">The message to handle</param>
        private void HandleGameMove(NetIncomingMessage msg)
        {
            // Reads move from the packet
            GameMove move = GameMove.DecodeFromClient(msg, myPlayers);

            // We only handle moves in game
            if (myState == ServerState.InGame)
            {
                // Check that the move came from the right client before handling
                if (move.Player == myPlayers[msg.SenderConnection])
                    HandleMove(move); // Handle the move
                else
                    Log("Bad packet received from \"{0}\" ({1})", myPlayers[msg.SenderConnection].Name, msg.SenderEndPoint);
            }
            else
            {
                // We are not in the right state, notify client
                NotifyBadState(msg.SenderConnection, "Game is not currently running");
                Log("Player \"{0}\" attempted move during non-game state", myPlayers[msg.SenderConnection].Name, msg.SenderEndPoint);
            }
        }

        /// <summary>
        /// Handles the message received when the host requests the game to start
        /// </summary>
        /// <param name="msg">The message to handle</param>
        private void HandleHostReqStart(NetIncomingMessage msg)
        {
            // Read the boolean and the padding bits
            bool start = msg.ReadBoolean();
            msg.ReadPadBits();

            // Ensure the sending player is the host
            if (myPlayers[msg.SenderConnection] == myGameHost)
            {
                // Log the request
                Log("Host requesting game start");

                bool isLobbyReady = true;

                Player[] notReady = myPlayers.Where(x => !x.IsHost && (!x.IsReady && !x.IsBot)).ToArray();

                // Determine if we're good to go
                isLobbyReady = notReady.Length == 0;

                // If everyone is ready proceed to game
                if (isLobbyReady)
                    SetServerState(ServerState.InGame);
                else
                {
                    Log("Cannot start game, all players not ready");
                    
                    string message = "Cannot start, following players not ready:\n";
                    foreach (Player p in notReady)
                        message += "\t" + p.Name + "\n";

                    NetOutgoingMessage outMsg = myServer.CreateMessage();
                    outMsg.Write((byte)MessageType.CannotStart);
                    outMsg.Write(message);

                    myServer.SendMessage(outMsg, myGameHost.Connection, NetDeliveryMethod.ReliableOrdered);
                }
            }
            else
            {
                Log("Someone who's not host is requesting game start");
            }
        }

        /// <summary>
        /// Handles the message received when a client requests the server state
        /// </summary>
        /// <param name="msg">The message to handle</param>
        private void HandleStateRequest(NetIncomingMessage msg)
        {
            // Create the message
            NetOutgoingMessage outMsg = myServer.CreateMessage();

            // Write the header and the bad move to the packet
            outMsg.Write((byte)MessageType.NotifyServerStateChanged);
            outMsg.Write((byte)myState);

            // Send the packet
            myServer.SendMessage(outMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Handles the message received when a player wants to ready up
        /// </summary>
        /// <param name="msg">The message to handle</param>
        private void HandlePlayerReady(NetIncomingMessage msg)
        {
            // Read the packet
            byte playerId = myPlayers[msg.SenderConnection].PlayerId;
            bool isReady = msg.ReadBoolean();
            msg.ReadPadBits();

            // If the message came from the right client
            if (myPlayers[playerId] == myPlayers[msg.SenderConnection])
            {
                // Update the player's state
                myPlayers[playerId].IsReady = isReady;

                // Prepare the message
                NetOutgoingMessage outMsg = myServer.CreateMessage();
                outMsg.Write((byte)MessageType.PlayerReady);
                outMsg.Write(playerId);
                outMsg.Write(isReady);
                outMsg.WritePadBits();

                Log("Player \"{0}\" is {1}", myPlayers[playerId].Name, isReady ? "ready" : "not ready");

                // Send to all clients
                SendToAll(outMsg);
            }
            else
                Log("Bad ready packet: from: {0} for: {1} status: {2}", myPlayers[msg.SenderConnection].PlayerId, playerId, isReady);

        }

        /// <summary>
        /// Handles when the host requests for a bot to be added
        /// </summary>
        /// <param name="msg">The message to handle</param>
        private void HandleHostReqBot(NetIncomingMessage msg)
        {
            // Read the bot data
            byte difficulty = msg.ReadByte();
            string botName = msg.ReadString();

            if (string.IsNullOrWhiteSpace(botName))
            {                
                Random r = new Random();
                int randomLineNumber = r.Next(0, BOT_NAMES.Length - 1);
                botName = BOT_NAMES[randomLineNumber];
            }

            // We can only add bots in lobby
            if (myState == ServerState.InLobby)
            {
                // Try to get an open Player ID
                int playerID = myPlayers.GetNextAvailableId();

                Log("Attempting to add bot \"{0}\" with difficulty {1}", botName, difficulty / 255.0f);

                // If there is a slot open, add bot
                if (playerID != -1)
                {
                    // Make the player instance
                    Player botPlayer = new Player(new ClientTag(botName), (byte)playerID) { IsBot = true };
                    // Make the bot instance
                    BotPlayer bot = new BotPlayer(this, botPlayer, (difficulty / 256.0f));

                    // Add the bot player
                    myPlayers[(byte)playerID] = botPlayer;

                    // Ad the bot instance
                    myBots.Add(bot);

                    // Notify all players that a bot has joined
                    NotifyPlayerJoined(botPlayer);

                    Log("Bot added");
                }
                else
                {
                    Log("Failed to add bot, lobby full");
                }
            }
            else
            {
                NotifyBadState(msg.SenderConnection, "Cannot add bot outside of lobby");
                Log("Failed to add bot during game");
            }
        }

        /// <summary>
        /// Handles when the host wants to kick a player
        /// </summary>
        /// <param name="msg">The message to handle</param>
        private void HandleHostReqKick(NetIncomingMessage msg)
        {
            // Get the message data
            byte playerId = msg.ReadByte();
            string reason = msg.ReadString();

            // Confirm that this came from the host, they are refering to a player, and they aren't kick themselves
            if (myPlayers[msg.SenderConnection] == myGameHost && myPlayers[playerId] != null && playerId != myGameHost.PlayerId)
            {
                // Prepare the outgoing message
                NetOutgoingMessage send = myServer.CreateMessage();
                send.Write((byte)MessageType.PlayerKicked);
                send.Write(playerId);
                send.Write(reason);

                if (myPlayers[playerId].IsBot)
                {
                    myBots.Remove(myBots.FirstOrDefault(x => x.Player == myPlayers[playerId]));
                    PlayerLeft(myPlayers[playerId], reason);
                }

                // Kick the player
                myPlayers[playerId].Connection.Disconnect("You have been kicked: " + reason);

                // Send the message to all clients
                SendToAll(send);
            }

        }

        /// <summary>
        /// Handles when a player has sent a chat message
        /// </summary>
        /// <param name="msg">The message to handle</param>
        private void HandlePlayerChat(NetIncomingMessage msg)
        {
            // Read packet info
            byte playerId = myPlayers[msg.SenderConnection].PlayerId;
            string message = msg.ReadString();

            // Prepare message
            NetOutgoingMessage send = myServer.CreateMessage();
            send.Write((byte)MessageType.PlayerChat);
            send.Write(playerId);
            send.Write(message);

            Log("[{0}]: {1}", myPlayers[msg.SenderConnection], message);

            // Forward to all clients
            SendToAll(send);
        }

        #endregion
    }
}
