using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// Gets the subnet mask fro a given IP address on this machine
        /// </summary>
        /// <param name="address">The address to search for</param>
        /// <returns>The network mask, or null if it could not be found</returns>
        public static IPAddress GetSubnetMask(this IPAddress address)
        {
            // Modified from
            // http://www.java2s.com/Code/CSharp/Network/GetSubnetMask.htm

            // Iterate over all the network interfaces on this computer (Wifi, Ethernet, etc...)
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Get each unicast address from the adapter
                // See http://www.vsicam.com/_faq/what-is-the-difference-between-unicast-and-multicast-streams/
                foreach (UnicastIPAddressInformation uniIpAddressInfo in adapter.GetIPProperties().UnicastAddresses)
                {
                    // If this is an inter-network IP address
                    // https://msdn.microsoft.com/en-us/library/system.net.sockets.addressfamily%28v=vs.110%29.aspx
                    if (uniIpAddressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        // If this is the droid we're looking for, return it's Net Mask
                        if (address.Equals(uniIpAddressInfo.Address))
                            return uniIpAddressInfo.IPv4Mask;
                    }
                }
            }

            // We could not find this IP address
            return null;
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

        internal static IPAddress GetGateway(IPAddress address)
        {
            // Modified from
            // http://www.java2s.com/Code/CSharp/Network/GetSubnetMask.htm

            // Modified from
            // http://www.java2s.com/Code/CSharp/Network/GetSubnetMask.htm

            // Iterate over all the network interfaces on this computer (Wifi, Ethernet, etc...)
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Get each unicast address from the adapter
                // See http://www.vsicam.com/_faq/what-is-the-difference-between-unicast-and-multicast-streams/
                foreach (UnicastIPAddressInformation uniIpAddressInfo in adapter.GetIPProperties().UnicastAddresses)
                {
                    // If this is an inter-network IP address
                    // https://msdn.microsoft.com/en-us/library/system.net.sockets.addressfamily%28v=vs.110%29.aspx
                    if (uniIpAddressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        // If this is the droid we're looking for, return it's Net Mask
                        if (address.Equals(uniIpAddressInfo.Address))
                        {
                            GatewayIPAddressInformation gateway =  adapter.GetIPProperties().GatewayAddresses.FirstOrDefault();

                            if (gateway != null)
                                return gateway.Address;
                        }
                    }
                }
            }

            // We could not find this IP address
            return null;

            // We could not find this IP address
            return null;
        }
    }

    //http://stackoverflow.com/questions/14327022/calculate-ip-range-by-subnet-mask

    class IPSegment
    {

        private UInt32 _ip;
        private UInt32 _mask;

        public IPSegment(string ip, string mask)
        {
            _ip = ip.ParseIp();
            _mask = mask.ParseIp();
        }

        public UInt32 NumberOfHosts
        {
            get { return ~_mask + 1; }
        }

        public UInt32 NetworkAddress
        {
            get { return _ip & _mask; }
        }

        public UInt32 BroadcastAddress
        {
            get { return NetworkAddress + ~_mask; }
        }

        public IEnumerable<UInt32> Hosts()
        {
            for (var host = NetworkAddress + 1; host < BroadcastAddress; host++)
            {
                yield return host;
            }
        }

    }

    public static class IpHelpers
    {
        public static string ToIpString(this UInt32 value)
        {
            var bitmask = 0xff000000;
            var parts = new string[4];
            for (var i = 0; i < 4; i++)
            {
                var masked = (value & bitmask) >> ((3 - i) * 8);
                bitmask >>= 8;
                parts[i] = masked.ToString(CultureInfo.InvariantCulture);
            }
            return String.Join(".", parts);
        }

        public static UInt32 ParseIp(this string ipAddress)
        {
            var splitted = ipAddress.Split('.');
            UInt32 ip = 0;
            for (var i = 0; i < 4; i++)
            {
                ip = (ip << 8) + UInt32.Parse(splitted[i]);
            }
            return ip;
        }
    }
}
