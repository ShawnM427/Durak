using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common
{
    /// <summary>
    /// Stores all the shared network settings, such as port and application name
    /// </summary>
    public static class NetSettings
    {
        /// <summary>
        /// Gets the default server port
        /// </summary>
        public const int DEFAULT_SERVER_PORT = 55440;
        /// <summary>
        /// Gets the default server timeout
        /// </summary>
        public const float DEFAULT_SERVER_TIMEOUT = 30.0f;
        /// <summary>
        /// Gets the identifier for the app
        /// </summary>
        public const string APP_IDENTIFIER = "DURAK_1.0";
        /// <summary>
        /// Gets the shutdown message for servers
        /// </summary>
        public const string DEFAULT_SERVER_SHUTDOWN_MESSAGE = "Server is shutting down...";
        /// <summary>
        /// Gets the shutdown message for clients
        /// </summary>
        public const string DEFAULT_CLIENT_SHUTDOWN_MESSAGE = "Client is shutting down...";
    }
}
