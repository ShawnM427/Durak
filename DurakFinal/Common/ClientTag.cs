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
    /// Represents client tag data, such as player name, IP, type, etc...
    /// </summary>
    public struct ClientTag
    {
        private string myName;
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
    }
}
