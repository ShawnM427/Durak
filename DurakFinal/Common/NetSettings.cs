using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakFinal.Common
{
    /// <summary>
    /// Stores all the shared network settings, such as port and application name
    /// </summary>
    public static class NetSettings
    {
        public const int DEFAULT_SERVER_PORT = 55440;
        public const float DEFAULT_SERVER_TIMEOUT = 5.0f;
        public const string APP_IDENTIFIER = "DURAK_0.0";
        public const string DEFAULT_SERVER_SHUTDOWN_MESSAGE = "Server is shutting down...";
    }
}
