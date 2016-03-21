using Durak.Common;
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
        /// Gets or sets an object tag for this client
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets this game client's player ID
        /// </summary>
        public byte PlayerId
        {
            get { return myPlayerId; }
        }

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
        }

        /// <summary>
        /// Shuts down this game client
        /// </summary>
        public void Stop()
        {
            myPeer.Shutdown(NetSettings.DEFAULT_CLIENT_SHUTDOWN_MESSAGE);
        }

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
        /// Invoked when the network peer has received a message
        /// </summary>
        /// <param name="state">The object that invoked this (NetPeer)</param>
        private void MessageReceived(object state)
        {
            // Get the incoming message
            NetIncomingMessage inMsg = ((NetPeer)state).ReadMessage();

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
                            case NetConnectionStatus.Disconnected:

                                // If the reason the is shutdown message, we're good
                                if (reason.Equals(NetSettings.DEFAULT_SERVER_SHUTDOWN_MESSAGE))
                                {
                                    if (OnDisconnected != null)
                                        OnDisconnected(this, EventArgs.Empty);
                                }
                                // Otherwise if the reason is that \/ , then we timed out
                                else if (reason.Equals("Failed to establish connection - no response from remote host",StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (OnConnectionTimedOut != null)
                                        OnConnectionTimedOut(this, EventArgs.Empty);

                                    if (OnConnectionFailed != null)
                                        OnConnectionFailed(this, reason);
                                }
                                // Otherwise the connection failed for some other reason
                                else
                                {
                                    if (OnConnectionFailed != null)
                                        OnConnectionFailed(this, reason);
                                }
                                
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
            }
            // An exception has occured parsing the packet
            catch (Exception e)
            {
                // Log the exception
                Logger.Write(e.Message);
            }
        }

        /// <summary>
        /// Handles an incoming data message
        /// </summary>
        /// <param name="inMsg">The message to handle</param>
        private void HandleMessage(NetIncomingMessage inMsg)
        {
            // Read the message type
            MessageType type = (MessageType)inMsg.ReadByte();

            switch (type)
            {
                case MessageType.PlayerConnectInfo:
                    // Reads the welcome message in
                    myPlayerId = inMsg.ReadByte();
                    isHost = inMsg.ReadBoolean();
                    inMsg.ReadPadBits();

                    break;
            }
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
        /// Sends discovery packets to all servers on the local network
        /// </summary>
        public void DiscoverServers()
        {
            // Discover local peers :D
            myPeer.DiscoverLocalPeers(NetSettings.DEFAULT_SERVER_PORT);


            IPSegment segment = new IPSegment(NetUtils.GetGateway(myAddress).ToString(), NetUtils.GetSubnetMask(myAddress).ToString());

            IEnumerable<uint> hosts = segment.Hosts();

            foreach(uint host in hosts)
            {
                myPeer.DiscoverKnownPeer(host.ToIpString(), NetSettings.DEFAULT_SERVER_PORT);
            }

            // Get subnet
            // Use subnet to determine prefix
            // Calculate number of IP's in network

            // Iterate over all IP's
            //  Send a discovery

        }

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
    }

    /// <summary>
    /// Delegate for handling when a new server is discovered
    /// </summary>
    /// <param name="sender">The object that raised the event</param>
    /// <param name="tag">The server tag of the server that was discovered</param>
    public delegate void ServerDiscoveredEvent(object sender, ServerTag tag);
}
