using Durak.Common;
using System.Collections.Generic;
using System.Collections;

namespace Durak.Client
{
    /// <summary>
    /// Represents a collection of server tags discovered by a client
    /// </summary>
    public class DiscoveredServerCollection : IEnumerable<ServerTag>
    {
        /// <summary>
        /// The underlying list to use
        /// </summary>
        List<ServerTag> myBackingList;

        /// <summary>
        /// Creates a new dicovered Server collection
        /// </summary>
        public DiscoveredServerCollection()
        {
            myBackingList = new List<ServerTag>();
        }

        /// <summary>
        /// Adds a server tag item to this list, or updates it if a sever with that address already exists
        /// </summary>
        /// <param name="tag">The server tag to add</param>
        public void AddItem(ServerTag tag)
        {
            int slot = myBackingList.FindIndex(X => X.Address == tag.Address);

            if (slot != -1)
                myBackingList[slot] = tag;
            else
                myBackingList.Add(tag);
        }

        /// <summary>
        /// Removes all Server tags whose address matches the server tag's address
        /// </summary>
        /// <param name="tag">The tag of the server to remove</param>
        public void RemoveItem(ServerTag tag)
        {
            myBackingList.RemoveAll(X => X.Address == tag.Address);
        }

        /// <summary>
        /// Clears this collection, removing all items
        /// </summary>
        public void Clear()
        {
            myBackingList.Clear();
        }

        /// <summary>
        /// Returns an enumerator that enumerates through this collection
        /// </summary>
        public IEnumerator<ServerTag> GetEnumerator()
        {
            return myBackingList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that enumerates through this collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return myBackingList.GetEnumerator();
        }
    }
}
