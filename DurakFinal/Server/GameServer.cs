using Durak.Common;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Durak.Server
{
    /// <summary>
    /// Represents a game server that handles networking clients and running the game logic
    /// </summary>
    public class GameServer
    {
        private IPAddress myAddress;
        // The output text box for the server log
        private RichTextBox myOutput;
        // Stores this server's server tag
        private ServerTag myTag;
        // Stores the list of rules to use
        private List<IGameRule> myRules;
        // Stores the list of players currently connected
        private PlayerCollection myPlayers;
        // Stores the network peer
        private NetPeer myServer;
        // Stores the SHA256 of the server's password
        private string myPassword;
        // Stores the server's current state
        private ServerState myState;
        // Stores the game's state
        private GameState myGameState;
        // Stores the player that has control over this game
        private Player myGameHost;

        /// <summary>
        /// Creates a new instance of a game server
        /// </summary>
        public GameServer()
        {
            myTag = new ServerTag();

            myRules = new List<IGameRule>();
            myPlayers = new PlayerCollection();

            myState = ServerState.InLobby;
            myGameState = new GameState();

            InitServer();
        }

        /// <summary>
        /// Sets the password for this server
        /// </summary>
        /// <param name="plainTextPassword">The server's password in plain text</param>
        public void SetPassword(string plainTextPassword)
        {
            // Hashes the password and stores it
            myPassword = plainTextPassword.Hash();
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

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="message">The message to log</param>
        private void Log(string message, params object[] format)
        {
            Logger.Write(message, format);

            if (myOutput != null)
                myOutput.AppendText(string.Format(message, format) + "\n");
        }

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
                            case NetConnectionStatus.Disconnected:
                                PlayerLeft(myPlayers[inMsg.SenderConnection], reason);
                                break;
                        }

                        Log("Connection status updated for connection from {0}: {1}", inMsg.SenderEndPoint, status);

                        break;

                    // Handle when a player is trying to join
                    case NetIncomingMessageType.ConnectionApproval:

                        // Get the client's info an hashed password from the packet
                        ClientTag clientTag = ClientTag.ReadFromPacket(inMsg);
                        string hashedPass = inMsg.ReadString();

                        // Make sure we are in the lobby when joining new players
                        if (myState == ServerState.InLobby)
                        {
                            // Check the password if applicable
                            if ((myTag.PasswordProtected && myPassword.Equals(hashedPass)) | (!myTag.PasswordProtected))
                            {
                                // Go ahead and try to join that playa
                                PlayerJoined(clientTag, inMsg.SenderConnection);

                                Log("Player \"{0}\" joined from {1}", clientTag.Name, clientTag.Address);
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
            catch(Exception e)
            {
                // Log the exception
                Log("Encountered exception parsing packet from {0}:\n\t{1}", inMsg.SenderEndPoint, e);
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

                // Switch message type to select proper message handling
                switch(messageType)
                {
                    case MessageType.SendMove: // A player has requested a move to be made

                        GameMove move = GameMove.ReadFromPacket(inMessage, myPlayers); // Get the move from the packet

                        if (myState == ServerState.InGame)
                        {
                            if (move.Player == myPlayers[inMessage.SenderConnection]) // Check that the move came from the right client before handling
                                HandleMove(move); // Handle the move
                            else
                                Log("Bad packet received from \"{0}\" ({1})", myPlayers[inMessage.SenderConnection].Name, inMessage.SenderEndPoint);
                        }
                        else
                        {
                            NotifyBadState(inMessage.SenderConnection, "Game is not currently running");
                            Log("Player \"{0}\" attempted move during non-game state", myPlayers[inMessage.SenderConnection].Name, inMessage.SenderEndPoint);
                        }
                        break;

                    // The client has requested the server's state
                    case MessageType.RequestServerState:
                        // Create the message
                        NetOutgoingMessage outMsg = myServer.CreateMessage();

                        // Write the header and the bad move to the packet
                        outMsg.Write((byte)MessageType.NotifyServerStateChanged);
                        outMsg.Write((byte)myState);

                        // Send the packet
                        myServer.SendMessage(outMsg, inMessage.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                        break;

                    default:
                        Logger.Write("Invalid message received from \"{0}\" ({1})", myPlayers[inMessage.SenderConnection].Name, inMessage.SenderEndPoint);
                        break;
                }
            }
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

            // Iterate over all clients
            for (int index = 0; index < myPlayers.Count; index++)
            {
                // Only send to not-null players
                if (myPlayers[(byte)index] != null)
                {
                    // Create the outgioing message
                    NetOutgoingMessage outMsg = myServer.CreateMessage();

                    // Write the header and player ID 
                    outMsg.Write((byte)MessageType.PlayerLeft);
                    outMsg.Write(player.PlayerId);
                    outMsg.Write(reason);

                    // Send the message to the player
                    myServer.SendMessage(outMsg, myPlayers[(byte)index].Connection, NetDeliveryMethod.ReliableOrdered);
                }
            }
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

                // Client can connect
                connection.Approve();

                // Iterate over all clients
                for (int index = 0; index < myPlayers.Count; index++)
                {
                    // Only send to not-null players
                    if (myPlayers[(byte)index] != null)
                    {
                        // Create the outgioing message
                        NetOutgoingMessage outMsg = myServer.CreateMessage();

                        // Write the header and move to the message
                        outMsg.Write((byte)MessageType.PlayerJoined);
                        outMsg.Write(player.PlayerId);
                        outMsg.Write(player.Name);
                        outMsg.Write(player.IsBot);

                        // Send the message to the player
                        myServer.SendMessage(outMsg, myPlayers[(byte)index].Connection, NetDeliveryMethod.ReliableOrdered);
                    }
                }

                // Add the player to the player list
                myPlayers[player.PlayerId] = player;
            }
            else
            {
                // Deny the connection
                connection.Deny("Server is full");
            }
        }

        /// <summary>
        /// Handles a player making a game move
        /// </summary>
        /// <param name="move"></param>
        private void HandleMove(GameMove move)
        {
            // define the reason
            string failReason = "Unkown";

            // Iterate over each game rule
            for (int index = 0; index < myRules.Count; index++)
            {
                // If the move is valid, continue, otherwise a rule was violated
                if (!myRules[index].IsValidMove(move, myGameState, ref failReason))
                {
                    NotifyInvalidMove(move, failReason); // Notify the source user
                    return; // Do not send to other clients, so break out of method
                }
            }

            // Iterate over all clients
            for(int index = 0; index < myPlayers.Count; index ++)
            {
                // Only send to not-null players
                if (myPlayers[(byte)index] != null)
                {
                    // Create the outgioing message
                    NetOutgoingMessage outMsg = myServer.CreateMessage();

                    // Write the header and move to the message
                    outMsg.Write((byte)MessageType.SucessfullMove);
                    move.WriteToPacket(outMsg);

                    // Send the message to the player
                    myServer.SendMessage(outMsg, myPlayers[(byte)index].Connection, NetDeliveryMethod.ReliableOrdered);
                }
            }
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
            move.WriteToPacket(outMsg);
            outMsg.Write(reason);

            // Send the packet
            myServer.SendMessage(outMsg, move.Player.Connection, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
