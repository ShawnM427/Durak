using Durak.Common;
using Durak.Common.Cards;
using Lidgren.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace Durak.Client
{
    /// <summary>
    /// Represents a single game client. This can be a bot, a player
    /// </summary>
    public class GameClient
    {
        #region Fields

        /// <summary>
        /// Stores this client's tag
        /// </summary>
        private ClientTag myTag;
        /// <summary>
        /// Stores the Lidgren networking peer
        /// </summary>
        private NetPeer myPeer;
        /// <summary>
        /// Stores the client's IP address
        /// </summary>
        private IPAddress myAddress;
        /// <summary>
        /// The connected server's server tag
        /// </summary>
        private ServerTag? myConnectedServer;

        /// <summary>
        /// Stores the client's player ID
        /// </summary>
        private byte myPlayerId;
        /// <summary>
        /// Stores whether or not this client is the host
        /// </summary>
        private bool isHost;
        /// <summary>
        /// Stores the packet handlers
        /// </summary>
        private Dictionary<MessageType, PacketHandler> myMessageHandlers;
        /// <summary>
        /// Stores the client's local game state
        /// </summary>
        private GameState myLocalState;
        /// <summary>
        /// Stores the local know play collection
        /// </summary>
        private PlayerCollection myKnownPlayers;
        /// <summary>
        /// Stores whether this client is ready
        /// </summary>
        private bool isReady = false;
        /// <summary>
        /// Stores the client's hand
        /// </summary>
        private CardCollection myHand;

        #endregion

        #region Events

        /// <summary>
        /// Invoked when the client connects to a server
        /// </summary>
        public event EventHandler OnConnected;
        /// <summary>
        /// Invoked when the connection to a server has failed
        /// </summary>
        public event EventHandler<string> OnConnectionFailed;
        /// <summary>
        /// Invoked when the client has disconnected from the server
        /// </summary>
        public event EventHandler OnDisconnected;
        /// <summary>
        /// Invoked when the connection to a server has timed out
        /// </summary>
        public event EventHandler OnConnectionTimedOut;
        /// <summary>
        /// Invoked when a server has been discovered, note that 1 server can be discovered multiple times
        /// </summary>
        public event ServerDiscoveredEvent OnServerDiscovered;
        /// <summary>
        /// Invoked when the server state has been updated
        /// </summary>
        public event EventHandler<ServerState> OnServerStateUpdated;
        /// <summary>
        /// Invoked when the invalid server state packet has been received
        /// </summary>
        public event EventHandler<string> OnInvalidServerState;
        /// <summary>
        /// Invoked when a player or bot joins the game
        /// </summary>
        public event EventHandler<Player> OnPlayerConnected;
        /// <summary>
        /// Invoked when a player has left the game, but before they are removed from the local player collection
        /// </summary>
        public event PlayerLeftEvent OnPlayerLeft;
        /// <summary>
        /// Invoked when a player has left the game, but before they are removed from the local player collection
        /// </summary>
        public event PlayerLeftEvent OnPlayerKicked;
        /// <summary>
        /// Invoked when this client has been kicked from the game
        /// </summary>
        public event EventHandler<string> OnKicked;
        /// <summary>
        /// Invoked when a player has made a move. This may be a move that this client has played
        /// </summary>
        public event EventHandler<GameMove> OnMoveReceived;
        /// <summary>
        /// Invoked when this client has made an invalid move
        /// </summary>
        public event EventHandler<GameMove> OnInvalidMove;
        /// <summary>
        /// Invoked when a player has sent a chat message, this message may come from this client
        /// </summary>
        public event PlayerChatEvent OnPlayerChat;
        /// <summary>
        /// Invoked when a player is ready, this may be the client player
        /// </summary>
        public event EventHandler<Player> OnPlayerReady;
        /// <summary>
        /// Invoked when this client has become host
        /// </summary>
        public event EventHandler OnBecameHost;
        /// <summary>
        /// Invoked when the game host has changed, this will be invoked after OnBecameHost if applicable
        /// </summary>
        public event EventHandler<Player> OnHostChanged;
        /// <summary>
        /// Invoked when the client's hand has gained a card
        /// </summary>
        public event EventHandler<PlayingCard> OnHandCardAdded;
        /// <summary>
        /// Invoked when the client's hand has lost a card
        /// </summary>
        public event EventHandler<PlayingCard> OnHandCardRemoved;
        /// <summary>
        /// Invoked when a client's number of cards in hand has changed
        /// </summary>
        public event PlayerCardCountChangedEvent OnPlayerCountChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an object tag for this client
        /// </summary>
        public object Tag
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets whether this client is ready to play
        /// </summary>
        public bool IsReady
        {
            get { return isReady; }
            set
            {
                if (value != isReady)
                    SetReadiness(isReady);
            }
        }
        /// <summary>
        /// Gets whether this client is connected to a server
        /// </summary>
        public bool IsConnected
        {
            get { return (myConnectedServer != null && myPeer.ConnectionsCount > 0); }
        }
        /// <summary>
        /// Gets this game client's player ID
        /// </summary>
        public byte PlayerId
        {
            get { return myPlayerId; }
        }
        /// <summary>
        /// Gets this client's local game state
        /// </summary>
        public GameState LocalState
        {
            get { return myLocalState; }
        }
        /// <summary>
        /// Gets the collection of players that this client knows that exist on their server
        /// </summary>
        public PlayerCollection KnownPlayers
        {
            get { return myKnownPlayers; }
        }
        /// <summary>
        /// Gets the tag of the currently connected server
        /// </summary>
        public ServerTag? ConnectedServer
        {
            get { return myConnectedServer; }
        }
        /// <summary>
        /// Gets the client's hand
        /// </summary>
        public CardCollection Hand
        {
            get { return myHand; }
        }

        #endregion

        #region Constructors and Initialization

        /// <summary>
        /// Creates a new game client with the given client tag
        /// </summary>
        /// <param name="tag">The client tag for this game client</param>
        public GameClient(ClientTag tag)
        {
            myTag = tag;

            Initialize();
        }
        
        /// <summary>
        /// Initializes a client
        /// </summary>
        private void Initialize()
        {
            // Create a new net config
            NetPeerConfiguration netConfig = new NetPeerConfiguration(NetSettings.APP_IDENTIFIER);

            // Gets the IP for this client
            myAddress = NetUtils.GetAddress();

            // Create the dictionary
            myMessageHandlers = new Dictionary<MessageType, PacketHandler>();

            // Make the local game state
            myLocalState = new GameState();
            // Make the player collection
            myKnownPlayers = new PlayerCollection();
            // Define the hand
            myHand = new CardCollection();

            // Allow incoming connections
            netConfig.AcceptIncomingConnections = true;
            // Set the ping interval
            netConfig.PingInterval = NetSettings.DEFAULT_SERVER_TIMEOUT / 10.0f;
            // Set the address
            netConfig.LocalAddress = myAddress;
            // Set the timeout between heartbeats before a client is considered disconnected
            netConfig.ConnectionTimeout = NetSettings.DEFAULT_SERVER_TIMEOUT;
            // Set the port to use
            netConfig.Port = NetUtils.GetOpenPort(NetSettings.DEFAULT_SERVER_PORT + 1);
            // We want to recycle old messages (improves performance)
            netConfig.UseMessageRecycling = true;

            // We want to accept Connection Approval messages (requests for connection)
            netConfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            // We want to accept data (duh)
            netConfig.EnableMessageType(NetIncomingMessageType.Data);
            // We want to accept status change messages (client connect / disconnect)
            netConfig.EnableMessageType(NetIncomingMessageType.StatusChanged);
            // We want the connection latency updates (heartbeats)
            netConfig.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);

            // Create the network peer
            myPeer = new NetServer(netConfig);

            // Register the callback function. Lidgren will handle the threading for us
            myPeer.RegisterReceivedCallback(new SendOrPostCallback(MessageReceived));

            // Add all the handlers
            myMessageHandlers.Add(MessageType.PlayerConnectInfo, HandleConnectInfo);
            myMessageHandlers.Add(MessageType.GameStateChanged, HandleStateChanged);
            myMessageHandlers.Add(MessageType.FullGameStateTransfer, HandleStateReceived);
            myMessageHandlers.Add(MessageType.InvalidServerState, HandleInvalidServerState);
            myMessageHandlers.Add(MessageType.NotifyServerStateChanged, HandleServerStateReceived);

            myMessageHandlers.Add(MessageType.PlayerJoined, HandlePlayerJoined);
            myMessageHandlers.Add(MessageType.PlayerLeft, HandlePlayerLeft);
            myMessageHandlers.Add(MessageType.PlayerKicked, HandlePlayerKicked);

            myMessageHandlers.Add(MessageType.SucessfullMove, HandleMoveReceived);
            myMessageHandlers.Add(MessageType.InvalidMove, HandleInvalidMove);
            myMessageHandlers.Add(MessageType.PlayerChat, HandlePlayerChat);
            myMessageHandlers.Add(MessageType.PlayerReady, HandlePlayerReady);

            myMessageHandlers.Add(MessageType.CardCountChanged, HandleCardCountChanged);
            myMessageHandlers.Add(MessageType.PlayerHandChanged, HandleCardChanged);
        }

        /// <summary>
        /// Starts up this server to start accepting messages
        /// </summary>
        public void Run()
        {
            // Start listening to messages
            myPeer.Start();
        }

        /// <summary>
        /// Shuts down this game client
        /// </summary>
        public void Stop()
        {
            myPeer.Shutdown(NetSettings.DEFAULT_CLIENT_SHUTDOWN_MESSAGE);
        }

        #endregion

        #region Message Handlers

        /// <summary>
        /// Invoked when the network peer has received a message
        /// </summary>
        /// <param name="state">The object that invoked this (NetPeer)</param>
        private void MessageReceived(object state)
        {
            // Get the incoming message
            NetIncomingMessage inMsg = ((NetPeer)state).ReadMessage();

            // We don't want the server to crash on one bad packet
            //try
            //{
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
                        case NetConnectionStatus.Disconnected:

                            // If the reason the is shutdown message, we're good
                            if (reason.Equals(NetSettings.DEFAULT_SERVER_SHUTDOWN_MESSAGE))
                            {
                                if (OnDisconnected != null)
                                    OnDisconnected(this, EventArgs.Empty);
                            }
                            // Otherwise if the reason is that \/ , then we timed out
                            else if (reason.Equals("Failed to establish connection - no response from remote host", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (OnConnectionTimedOut != null)
                                    OnConnectionTimedOut(this, EventArgs.Empty);

                                if (OnConnectionFailed != null)
                                    OnConnectionFailed(this, reason);

                                if (OnDisconnected != null)
                                    OnDisconnected(this, EventArgs.Empty);
                            }
                            // Otherwise the connection failed for some other reason
                            else
                            {
                                if (OnConnectionFailed != null)
                                    OnConnectionFailed(this, reason);

                                if (OnDisconnected != null)
                                    OnDisconnected(this, EventArgs.Empty);
                            }

                            // Clear local state and forget connected server tag
                            myLocalState.Clear();
                            myConnectedServer = null;
                            isReady = false;
                            isHost = false;

                            break;

                        // We connected 
                        case NetConnectionStatus.Connected:
                            // invoked the onConnected event
                            if (OnConnected != null)
                                OnConnected(this, EventArgs.Empty);

                            break;

                    }

                    break;

                // Handle when the server has received a discovery request
                case NetIncomingMessageType.DiscoveryResponse:
                    // Read the server tag from the packet
                    ServerTag serverTag = ServerTag.ReadFromPacket(inMsg);

                    // Notify that we discovered a server
                    if (OnServerDiscovered != null)
                        OnServerDiscovered(this, serverTag);

                    break;

                // Handles when the server has received data
                case NetIncomingMessageType.Data:
                    HandleMessage(inMsg);
                    break;
            }
            //}
            // An exception has occured parsing the packet
            //catch (Exception e)
            //{
            // Log the exception
            //    Logger.Write(e.Message);
            //}
        }

        /// <summary>
        /// Handles an incoming data message
        /// </summary>
        /// <param name="inMsg">The message to handle</param>
        private void HandleMessage(NetIncomingMessage inMsg)
        {
            // Read the message type
            MessageType messageType = (MessageType)inMsg.ReadByte();

            if (myMessageHandlers.ContainsKey(messageType))
                myMessageHandlers[messageType].Invoke(inMsg);
            else
            {
                // Logs the message
                Logger.Write("Invalid message received from server ({0})", inMsg.SenderEndPoint);
            }
        }

        /// <summary>
        /// Handles the card changed packet
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandleCardChanged(NetIncomingMessage inMsg)
        {
            bool added = inMsg.ReadBoolean();
            bool hasValue = inMsg.ReadBoolean();
            inMsg.ReadPadBits();

            if (hasValue)
            {
                CardRank rank = (CardRank)inMsg.ReadByte();
                CardSuit suit = (CardSuit)inMsg.ReadByte();

                PlayingCard card = new PlayingCard(rank, suit) { FaceUp = true };

                if (added)
                {
                    myHand.Add(card);

                    if (OnHandCardAdded != null)
                        OnHandCardAdded(this, card);
                }
                else
                {
                    myHand.Discard(card);

                    if (OnHandCardRemoved != null)
                        OnHandCardRemoved(this, card);
                }
            }
        }

        /// <summary>
        /// Handles the card count changed message
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandleCardCountChanged(NetIncomingMessage inMsg)
        {
            byte playerId = inMsg.ReadByte();
            int numCards = inMsg.ReadInt32();

            myKnownPlayers[playerId].NumCards = numCards;

            if (OnPlayerCountChanged != null)
                OnPlayerCountChanged(myKnownPlayers[playerId], numCards);
        }

        /// <summary>
        /// Handles when the state parameter changed message has been received
        /// </summary>
        /// <param name="inMsg">The message to decode from</param>
        private void HandleStateChanged(NetIncomingMessage inMsg)
        {
            StateParameter.Decode(inMsg, myLocalState);
        }

        /// <summary>
        /// Handles when the connection info packet was recevied
        /// </summary>
        /// <param name="inMsg">The message to read</param>
        private void HandleConnectInfo(NetIncomingMessage inMsg)
        {
            myPlayerId = inMsg.ReadByte();
            isHost = inMsg.ReadBoolean();
            inMsg.ReadPadBits();

            int numPlayers = inMsg.ReadByte();

            for (int index = 0; index < numPlayers; index++)
            {
                Player player = new Player(0, "", false);
                player.Decode(inMsg);

                myKnownPlayers[player.PlayerId] = player;
            }

            myLocalState.Decode(inMsg);

            myKnownPlayers[myPlayerId] = new Player(myPlayerId, myTag.Name, false);
        }

        /// <summary>
        /// Handles the host changed message
        /// </summary>
        /// <param name="msg">The message to decode</param>
        private void HandleHostUpdated(NetIncomingMessage msg)
        {
            byte hostId = msg.ReadByte();

            // Update the player's info locally, we will assume the previous hose has left or will leave
            myKnownPlayers[hostId].IsHost = true;

            // If we are the new host
            if (myPlayerId == hostId)
            {
                // Update local variable
                isHost = true;

                // Invoke our event
                if (OnBecameHost != null)
                    OnBecameHost(this, EventArgs.Empty);
            }

            // Invoke the host changed event
            if (OnHostChanged != null)
                OnHostChanged(this, myKnownPlayers[hostId]);
        }

        /// <summary>
        /// Handles when the game state transfer packet has been received
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandleStateReceived(NetIncomingMessage inMsg)
        {
            myLocalState.Decode(inMsg);
        }

        /// <summary>
        /// Handles the serverStateUpdated packet type
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandleServerStateReceived(NetIncomingMessage inMsg)
        {
            if (myConnectedServer != null)
            {
                ServerTag tag = myConnectedServer.Value;
                tag.State = (ServerState)inMsg.ReadByte();
                myConnectedServer = tag;

                if (OnServerStateUpdated != null)
                    OnServerStateUpdated(this, tag.State);
            }
        }

        /// <summary>
        /// Handles when the invalid server state message has been received
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandleInvalidServerState(NetIncomingMessage inMsg)
        {
            if (myConnectedServer != null)
            {
                string reason = inMsg.ReadString();

                if (OnInvalidServerState != null)
                    OnInvalidServerState(this, reason);
            }
        }

        /// <summary>
        /// Handles the player joined message
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandlePlayerJoined(NetIncomingMessage inMsg)
        {
            byte playerId = inMsg.ReadByte();
            string name = inMsg.ReadString();
            bool isBot = inMsg.ReadBoolean();
            inMsg.ReadPadBits();

            myKnownPlayers[playerId] = new Player(playerId, name, isBot);

            if (OnPlayerConnected != null)
                OnPlayerConnected(this, myKnownPlayers[playerId]);
        }

        /// <summary>
        /// Handles the player left message
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandlePlayerLeft(NetIncomingMessage inMsg)
        {
            byte playerId = inMsg.ReadByte();
            string reason = inMsg.ReadString();

            if (OnPlayerLeft != null)
                OnPlayerLeft(this, myKnownPlayers[playerId], reason);

            myKnownPlayers[playerId] = null;
        }

        /// <summary>
        /// Handles the player ready packet
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandlePlayerReady(NetIncomingMessage inMsg)
        {
            byte playerId = inMsg.ReadByte();
            bool isReady = inMsg.ReadBoolean();
            inMsg.ReadPadBits();

            myKnownPlayers[playerId].IsReady = isReady;

            if (OnPlayerReady != null)
                OnPlayerReady(this, myKnownPlayers[playerId]);
        }

        /// <summary>
        /// Handles the player kicked message
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandlePlayerKicked(NetIncomingMessage inMsg)
        {
            byte playerId = inMsg.ReadByte();
            string reason = inMsg.ReadString();

            if (playerId != myPlayerId)
            {
                if (OnPlayerKicked != null)
                    OnPlayerKicked(this, myKnownPlayers[playerId], reason);

                myKnownPlayers[playerId] = null;
            }
            else
            {
                myConnectedServer = null;

                if (OnKicked != null)
                    OnKicked(this, reason);
            }
        }

        /// <summary>
        /// Handles the move played packet
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandleMoveReceived(NetIncomingMessage inMsg)
        {
            GameMove move = GameMove.Decode(inMsg, myKnownPlayers);

            if (OnMoveReceived != null)
                OnMoveReceived(this, move);
        }

        /// <summary>
        /// Handles the invalid move packet
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandleInvalidMove(NetIncomingMessage inMsg)
        {
            GameMove move = GameMove.Decode(inMsg, myKnownPlayers);

            if (OnInvalidMove != null)
                OnInvalidMove(this, move);
        }

        /// <summary>
        /// Handles the player chat message
        /// </summary>
        /// <param name="inMsg">The message to decode</param>
        private void HandlePlayerChat(NetIncomingMessage inMsg)
        {
            byte playerId = inMsg.ReadByte();
            string message = inMsg.ReadString();

            if (OnPlayerChat != null)
                OnPlayerChat(this, myKnownPlayers[playerId], message);
        }

        #endregion

        #region Utils
        
        /// <summary>
        /// Disconnects this client from the currently connected server
        /// </summary>
        public void Disconnect()
        {
            // If we are connected
            if (myPeer.ConnectionsCount > 0)
            {
                // If we are connected to a server
                if (myConnectedServer != null)
                {
                    // Disconnect and set the server tag to null
                    myPeer.GetConnection(myConnectedServer.Value.Address).Disconnect(NetSettings.DEFAULT_CLIENT_SHUTDOWN_MESSAGE);
                    myConnectedServer = null;
                }
            }
        }

        /// <summary>
        /// Connects to a specified server
        /// </summary>
        /// <param name="server">The server tag to connect to</param>
        /// <param name="serverPassword">The SHA256 encrypted password to connect to the server</param>
        public void ConnectTo(ServerTag server, string serverPassword = "")
        {
            if (myConnectedServer == null)
            {
                // Hash the password before sending
                serverPassword = SecurityUtils.Hash(serverPassword);

                // Write the hail message and send it
                NetOutgoingMessage hailMessage = myPeer.CreateMessage();
                myTag.WriteToPacket(hailMessage);
                hailMessage.Write(serverPassword);

                // Update our connected tag
                myConnectedServer = server;

                // Attempt the connection
                myPeer.Connect(server.Address, hailMessage);
            }
            else
                throw new InvalidOperationException("Cannot connect when this client is already connected");
        }

        /// <summary>
        /// Sends a message to the server
        /// </summary>
        /// <param name="msg">The message to send</param>
        private void Send(NetOutgoingMessage msg)
        {
            myPeer.SendMessage(msg, myPeer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        /// <summary>
        /// Sends discovery packets to all servers on the local network
        /// </summary>
        public void DiscoverServers()
        {
            // Discover local peers :D
            myPeer.DiscoverLocalPeers(NetSettings.DEFAULT_SERVER_PORT);

            // Get the IP range to ping
            IPSegment segment = new IPSegment(NetUtils.GetGateway(myAddress).ToString(), NetUtils.GetSubnetMask(myAddress).ToString());

            // Get an enumerable result
            IEnumerable<uint> hosts = segment.Hosts();

            // Iterate and ping each one
            foreach(uint host in hosts)
                myPeer.DiscoverKnownPeer(host.ToIpString(), NetSettings.DEFAULT_SERVER_PORT);
        }
        
        /// <summary>
        /// Requests for a move to be made by this client
        /// </summary>
        /// <param name="card">The card to play</param>
        public void RequestMove(PlayingCard card)
        {
            if (IsConnected)
            {
                GameMove move = new GameMove(myKnownPlayers[myPlayerId], card);

                NetOutgoingMessage msg = myPeer.CreateMessage();
                move.Encode(msg);
                Send(msg);
            }
        }

        /// <summary>
        /// Sends a chat message to the server
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendChatMessage(string message)
        {
            if (myConnectedServer != null && myPeer.ConnectionsCount > 0)
            {
                NetOutgoingMessage msg = myPeer.CreateMessage();
                msg.Write(message);
                Send(msg);
            }
        }

        /// <summary>
        /// Requests for a player to be kicked. Will only be sent if client is host
        /// </summary>
        /// <param name="player">The player to kick</param>
        /// <param name="reason">The reason they want the player kicked</param>
        public void RequestKick(Player player, string reason)
        {
            if (IsConnected && isHost)
            {
                NetOutgoingMessage msg = myPeer.CreateMessage();
                msg.Write(player.PlayerId);
                msg.Write(reason);
                Send(msg);
            }
        }

        /// <summary>
        /// Requests a bot to be added to the game, will only be sent if host
        /// </summary>
        /// <param name="difficulty">The difficulty of the bot, with 0 being dumb and 255 super smart</param>
        /// <param name="name">The name of the bot</param>
        public void RequestBot(byte difficulty, string name)
        {
            if (IsConnected && isHost)
            {
                NetOutgoingMessage msg = myPeer.CreateMessage();
                msg.Write(difficulty);
                msg.Write(name);
                Send(msg);
            }
        }

        /// <summary>
        /// Sets this client's readiness
        /// </summary>
        /// <param name="ready">True if this client is ready to play, false if otherwise</param>
        public void SetReadiness(bool ready)
        {
            if (myConnectedServer != null && myPeer.ConnectionsCount > 0)
            {
                isReady = ready;

                NetOutgoingMessage msg = myPeer.CreateMessage();
                msg.Write(ready);
                Send(msg);
            }
        }

        /// <summary>
        /// Queries the connected server's state, this will invoke the OnServerStateUpdated event when received
        /// </summary>
        public void QueryServerState()
        {
            if (myConnectedServer != null && myPeer.ConnectionsCount > 0)
            {
                NetOutgoingMessage msg = myPeer.CreateMessage();
                msg.Write((byte)MessageType.RequestServerState);
                Send(msg);
            }
        }

        #endregion

        #if DEBUG

        /// <summary>
        /// Prepare messages for debug purposes. This should not be used in final code
        /// </summary>
        /// <returns>A network message that you can write data to</returns>
        public NetOutgoingMessage PrepareDebugMessage()
        {
            return myPeer.CreateMessage();
        }

        /// <summary>
        /// Sends a message to this client's connections
        /// </summary>
        /// <param name="msg">The network message to send</param>
        public void SendMessageDebug(NetOutgoingMessage msg)
        {
            // If we're connected, we sent it
            if (myPeer.Connections.Count > 0)
                myPeer.SendMessage(msg, myPeer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        #endif
    }
}
