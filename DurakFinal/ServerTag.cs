using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common
{
    /// <summary>
    /// Represents Server tag data, such as server name, number of players connected, IP address, etc...
    /// </summary>
    public struct ServerTag
    {
        private int myPlayerCount;
        private bool isInGame;
        private string myName;
        private IPEndPoint myAddress;
        private string myDescription;
        private bool isPasswordProtected;

        /// <summary>
        /// Gets the player count for this server
        /// </summary>
        public int PlayerCount
        {
            get { return myPlayerCount; }
            set { myPlayerCount = value; }
        }
        /// <summary>
        /// Gets whether or not this server is currently in a game
        /// </summary>
        public bool InGame
        {
            get { return isInGame; }
            set { isInGame = value; }
        }
        /// <summary>
        /// Gets this server's name
        /// </summary>
        public string Name
        {
            get { return myName; }
            set { myName = value; }
        }
        /// <summary>
        /// Gets a description as defined by the server owner
        /// </summary>
        public string Description
        {
            get { return myDescription; }
            set { myDescription = value; }
        }
        /// <summary>
        /// Gets whether or not this server is password protected
        /// </summary>
        public bool PasswordProtected
        {
            get { return isPasswordProtected; }
        }
        /// <summary>
        /// Gets this server's address (note that this is NOT transferred over network)
        /// </summary>
        public IPEndPoint Address
        {
            get { return myAddress; }
        }

        /// <summary>
        /// Writes this server tag to an outgoing message
        /// </summary>
        /// <param name="outMessage">The network message to write to</param>
        public void WriteToPacket(NetOutgoingMessage outMessage)
        {
            outMessage.Write(myPlayerCount);
            outMessage.Write(isInGame);
            outMessage.Write(myName);
            outMessage.Write(myDescription);
            outMessage.Write(isPasswordProtected);
        }

        /// <summary>
        /// Reads a server tag from an incoming message, and set's it's address to the message's sender address
        /// </summary>
        /// <param name="inMessage">The newtork message to read</param>
        /// <returns>The server tag as read from the packet</returns>
        public static ServerTag ReadFromPacket(NetIncomingMessage inMessage)
        {
            ServerTag result;

            result.myPlayerCount = inMessage.ReadInt32();
            result.isInGame = inMessage.ReadBoolean();
            result.myName = inMessage.ReadString();
            result.myDescription = inMessage.ReadString();
            result.isPasswordProtected = inMessage.ReadBoolean();
            result.myAddress = inMessage.SenderEndPoint;

            return result;
        }
    }
}
