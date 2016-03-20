using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common
{
    public static class NetUtils
    {
        public static int GetOpenPort(int startPort = 2555)
        {
            int portStartIndex = startPort;
            int count = 99;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] udpEndPoints = properties.GetActiveUdpListeners();

            List<int> usedPorts = udpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            unusedPort = Enumerable.Range(portStartIndex, count).Where(port => !usedPorts.Contains(port)).FirstOrDefault();
            return unusedPort;
        }

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

            throw new NetworkInformationException();
        }
    }
}
