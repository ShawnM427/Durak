using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Durak.Common
{
    /// <summary>
    /// A utility class for networking
    /// </summary>
    public static class NetUtils
    {
        /// <summary>
        /// Gets the first open port
        /// </summary>
        /// <param name="startPort">The port to start searching from</param>
        /// <returns>The first open port</returns>
        public static int GetOpenPort(int startPort = NetSettings.DEFAULT_SERVER_PORT)
        {
            // Get the range
            int portStartIndex = startPort;
            int count = 99;

            // Get the list of active UDP listeners
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] udpEndPoints = properties.GetActiveUdpListeners();

            // Get a list of all ports being used
            List<int> usedPorts = udpEndPoints.Select(p => p.Port).ToList<int>();

            // Define the result (return 0 by default)
            int unusedPort = Enumerable.Range(portStartIndex, count).Where(port => !usedPorts.Contains(port)).FirstOrDefault();

            // Return the result
            return unusedPort;
        }

        /// <summary>
        /// Gets the local machine's IP address
        /// </summary>
        /// <returns>This machine's IP address</returns>
        public static IPAddress GetAddress()
        {
            // Gets the IP's associated with this server
            IPAddress[] IpList = Dns.GetHostAddresses(Dns.GetHostName());

            // Iterate over them
            foreach (IPAddress IP in IpList)
            {
                // Gets the Internetwork IP
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    // Assign to myAddress
                    return IP;
                }
            }

            // We didn't have any InterNetwork IP
            throw new NetworkInformationException();
        }
    }
}
