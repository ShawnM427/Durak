using Durak.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Rules.StateVerify
{
    public class VerifyHasPlayers : IGameStateRule
    {
        /// <summary>
        /// Gets or sets whether this rule is enabled
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the human readable name for this rule
        /// </summary>
        public string ReadableName
        {
            get
            {
                return "Verify we have players";
            }
        }

        /// <summary>
        /// Handles validating the server state. If the attacking or defending player are invalid, attempt to repair them
        /// </summary>
        /// <param name="server">The server to execute on</param>
        public void ValidateState(GameServer server)
        {
            throw new NotImplementedException();
        }
    }
}
