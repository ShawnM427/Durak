using Lidgren.Network;
using System.Net;
using System;

namespace Durak.Common
{
    /// <summary>
    /// Represents client tag data, such as player name, IP, type, etc...
    /// </summary>
    public struct ClientTag
    {
        /// <summary>
        /// Stores the client's player name
        /// </summary>
        private string myName;
        /// <summary>
        /// Stores the client's IP address
        /// </summary>
        private IPEndPoint myAddress;

        /// <summary>
        /// Gets this client's name
        /// </summary>
        public string Name
        {
            get { return myName; }
        }
        /// <summary>
        /// Get's this client's IP endpoint. This is NOT transferred over the network
        /// </summary>
        public IPEndPoint Address
        {
            get { return myAddress; }
        }

        /// <summary>
        /// Creates a new client tag with the given player name
        /// </summary>
        /// <param name="name">The player's name</param>
        public ClientTag(string name)
        {
            myName = name;
            myAddress = null;
        }

        /// <summary>
        /// Writes this client tag to an outgoing message
        /// </summary>
        /// <param name="outMessage">The network message to write to</param>
        public void WriteToPacket(NetOutgoingMessage outMessage)
        {
            outMessage.Write(myName);
        }

        /// <summary>
        /// Reads a client tag from an incoming message, and set's it's address to the message's sender address
        /// </summary>
        /// <param name="inMessage">The newtork message to read</param>
        /// <returns>The client tag as read from the packet</returns>
        public static ClientTag ReadFromPacket(NetIncomingMessage inMessage)
        {
            ClientTag result;
            
            result.myName = inMessage.ReadString();
            result.myAddress = inMessage.SenderEndPoint;

            return result;
        }

        /// <summary>
        /// Encodes this instance to a network message
        /// </summary>
        /// <param name="msg">The message to encode to</param>
        public void Encode(NetOutgoingMessage msg)
        {
            msg.Write(myName);
        }

        /// <summary>
        /// Decodes tis instance from a network message
        /// </summary>
        /// <param name="msg">The message to decode from</param>
        public void Decode(NetIncomingMessage msg)
        {
            myAddress = msg.SenderEndPoint;
            myName = msg.ReadString();
        }
    }
}
