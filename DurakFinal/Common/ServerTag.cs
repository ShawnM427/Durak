using Lidgren.Network;
using System.Net;

namespace Durak.Common
{
    /// <summary>
    /// Represents Server tag data, such as server name, number of players connected, IP address, etc...
    /// </summary>
    public struct ServerTag
    {
        /// <summary>
        /// Gets the number of connected players
        /// </summary>
        private int myPlayerCount;
        /// <summary>
        /// Stores th number of players this server supports
        /// </summary>
        private int mySupportedPlayerCount;
        /// <summary>
        /// S the server is in game
        /// </summary>
        private ServerState myState;
        /// <summary>
        /// The server's name
        /// </summary>
        private string myName;
        /// <summary>
        /// The server's IP address
        /// </summary>
        private IPEndPoint myAddress;
        /// <summary>
        /// The server's description
        /// </summary>
        private string myDescription;
        /// <summary>
        /// Whether this server is password protected
        /// </summary>
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
        /// Gets or sets the number of players that this server supports
        /// </summary>
        public int SupportedPlayerCount
        {
            get { return mySupportedPlayerCount; }
            set { mySupportedPlayerCount = value; }
        }
        /// <summary>
        /// Getsor sets this server's state
        /// </summary>
        public ServerState State
        {
            get { return myState; }
            set { myState = value; }
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
            set { isPasswordProtected = value; }
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
            outMessage.Write((byte)myState);
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
            ServerTag result = new ServerTag();

            result.myPlayerCount = inMessage.ReadInt32();
            result.myState = (ServerState)inMessage.ReadByte();
            result.myName = inMessage.ReadString();
            result.myDescription = inMessage.ReadString();
            result.isPasswordProtected = inMessage.ReadBoolean();
            result.myAddress = inMessage.SenderEndPoint;

            return result;
        }

        /// <summary>
        /// Checks to see if this object is equal to another
        /// </summary>
        /// <param name="obj">The object to check against</param>
        /// <returns>True if the objects are equal, false if otherwise</returns>
        public override bool Equals(object obj)
        {
            return obj is ServerTag && (obj as ServerTag?).Value == this;
        }

        /// <summary>
        /// Gets a semi-unique hash code for this instance
        /// </summary>
        /// <returns>A semi-unique hashcode</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Check to see if one server tag is equal to another
        /// </summary>
        /// <param name="left">The left hand operand</param>
        /// <param name="right">The right hand operand</param>
        /// <returns>Whether the left is equal to the right</returns>
        public static bool operator ==(ServerTag left, ServerTag right)
        {
            return left.Address == right.Address && left.Name == right.Name;
        }

        /// <summary>
        /// Check to see if one server tag is not equal to another
        /// </summary>
        /// <param name="left">The left hand operand</param>
        /// <param name="right">The right hand operand</param>
        /// <returns>Whether the left is not equal to the right</returns>
        public static bool operator !=(ServerTag left, ServerTag right)
        {
            return !(left.Address == right.Address);
        }
    }
}
