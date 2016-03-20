using Durak.Common;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Durak.Client
{
    /// <summary>
    /// Represents a single game client. This can be a bot, a player
    /// </summary>
    public class GameClient
    {
        private ClientTag myTag;
        private NetPeer myPeer;
        private IPAddress myAddress;
        private ServerTag? myConnectedServer;

        private byte myPlayerId;
        private bool isHost;

        public event EventHandler OnConnected;
        public event EventHandler<string> OnConnectionFailed;
        public event EventHandler OnDisconnected;
        public event EventHandler OnConnectionTimedOut;
        public event ServerDiscoveredEvent OnServerDiscovered;

        public GameClient(ClientTag tag)
        {
            myTag = tag;
        }

        public object Tag
        {
            get;
            set;
        }

        public byte PlayerId
        {
            get { return myPlayerId; }
        }

        /// <summary>
        /// Initializes the server 
        /// </summary>
        public void Initialize()
        {
            // Create a new net config
            NetPeerConfiguration netConfig = new NetPeerConfiguration(NetSettings.APP_IDENTIFIER);

            // Gets the IP's associated with this server
            IPAddress[] IpList = Dns.GetHostAddresses(Dns.GetHostName());

            // Iterate over them
            foreach (IPAddress IP in IpList)
            {
                // Gets the Internetwork IP
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    // Assign to myAddress
                    myAddress = IP;
                    break;
                }
            }

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

        public void Stop()
        {
            myPeer.Shutdown(NetSettings.DEFAULT_CLIENT_SHUTDOWN_MESSAGE);
        }

        public void Disconnect()
        {
            if (myPeer.ConnectionsCount > 0)
            {
                if (myConnectedServer != null)
                {
                    myPeer.GetConnection(myConnectedServer.Value.Address).Disconnect(NetSettings.DEFAULT_CLIENT_SHUTDOWN_MESSAGE);
                    myConnectedServer = null;
                }
            }
        }

        public void ConnectTo(ServerTag server)
        {
            NetOutgoingMessage hailMessage = myPeer.CreateMessage();
            myTag.WriteToPacket(hailMessage);
            hailMessage.Write("");

            myConnectedServer = server;

            System.Diagnostics.Debug.WriteLine(server.Address.ToString());

            myPeer.Connect(server.Address, hailMessage);
        }

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

                            case NetConnectionStatus.Connected:
                                if (OnConnected != null)
                                    OnConnected(this, EventArgs.Empty);
                                break;
                                
                        }
                        
                        break;
                                                
                    // Handle when the server has received a discovery request
                    case NetIncomingMessageType.DiscoveryResponse:

                        ServerTag serverTag = ServerTag.ReadFromPacket(inMsg);

                        if (OnServerDiscovered != null)
                            OnServerDiscovered(this, serverTag);

                        myConnectedServer = serverTag;
                            
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

            }
        }

        public void ConnectTo(object p)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(NetIncomingMessage inMsg)
        {
            MessageType type = (MessageType)inMsg.ReadByte();

            switch (type)
            {
                case MessageType.PlayerConnectInfo:
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

        public void DiscoverServers()
        {
            myPeer.DiscoverLocalPeers(NetSettings.DEFAULT_SERVER_PORT);
        }

        public delegate void ServerDiscoveredEvent(object sender, ServerTag tag);

        public NetOutgoingMessage PrepareDebugMessage()
        {
            return myPeer.CreateMessage();
        }

        public void SendMessageDebug(NetOutgoingMessage msg)
        {
            if (myPeer.Connections.Count > 0)
                myPeer.SendMessage(msg, myPeer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
