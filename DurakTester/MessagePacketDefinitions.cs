using Durak.Common;
using System.Collections.Generic;

namespace DurakTester
{
    /// <summary>
    /// These message definitions are for client-side to server-side only
    /// </summary>
    public class MessagePacketDefinitions
    {
        private static Dictionary<MessageType, PayloadParameter[]> myPayloads;

        static MessagePacketDefinitions()
        {
            myPayloads = new Dictionary<MessageType, PayloadParameter[]>();

            myPayloads.Add(MessageType.HostReqStart, 
            new PayloadParameter[] {
                new PayloadParameter("Start Game", PayloadParamType.Boolean)
            });
            myPayloads.Add(MessageType.HostReqAddBot, 
            new PayloadParameter[] {
                new PayloadParameter("Difficulty", PayloadParamType.BotDifficulty),
                new PayloadParameter("Bot Name", PayloadParamType.String)
            });
            myPayloads.Add(MessageType.HostReqKick,
            new PayloadParameter[] {
                new PayloadParameter("Player ID", PayloadParamType.Byte),
                new PayloadParameter("Reason", PayloadParamType.String)
            });

            // Note that we cannot easily define these
            //myPayloads.Add(MessageType.GameStateChanged, null);
            //myPayloads.Add(MessageType.FullGameStateTransfer, null);

            myPayloads.Add(MessageType.RequestServerState,
            new PayloadParameter[] { });

            myPayloads.Add(MessageType.InvalidServerState,
            new PayloadParameter[] {
                new PayloadParameter("Reason", PayloadParamType.String)
            });

            myPayloads.Add(MessageType.NotifyServerStateChanged,
            new PayloadParameter[] {
                new PayloadParameter("New State", PayloadParamType.ServerState),
                new PayloadParameter("Reason", PayloadParamType.String)
            });

            myPayloads.Add(MessageType.PlayerJoined,
            new PayloadParameter[] {
                new PayloadParameter("Player Name", PayloadParamType.String)
            });

            myPayloads.Add(MessageType.PlayerLeft,
            new PayloadParameter[] {
                new PayloadParameter("Player Name", PayloadParamType.String)
            });

            myPayloads.Add(MessageType.PlayerReady,
            new PayloadParameter[] {
                new PayloadParameter("Player Ready", PayloadParamType.Boolean)
            });

            myPayloads.Add(MessageType.PlayerKicked,
            new PayloadParameter[] {
                new PayloadParameter("Reason", PayloadParamType.String)
            });

            myPayloads.Add(MessageType.SendMove,
            new PayloadParameter[] {
                new PayloadParameter("Has Value", PayloadParamType.Boolean),
                new PayloadParameter("Card Rank", PayloadParamType.CardRank),
                new PayloadParameter("Card Suit", PayloadParamType.CardSuit)
            });
            
            myPayloads.Add(MessageType.InvalidMove,
            new PayloadParameter[] {
                new PayloadParameter("Player ID", PayloadParamType.PlayerID),
                new PayloadParameter("Card Rank", PayloadParamType.CardRank),
                new PayloadParameter("Card Suit", PayloadParamType.CardSuit),
                new PayloadParameter("Reason", PayloadParamType.String)
            });

            myPayloads.Add(MessageType.SucessfullMove,
            new PayloadParameter[] {
                new PayloadParameter("Player ID", PayloadParamType.PlayerID),
                new PayloadParameter("Card Rank", PayloadParamType.CardRank),
                new PayloadParameter("Card Suit", PayloadParamType.CardSuit)
            });

            myPayloads.Add(MessageType.PlayerChat,
            new PayloadParameter[] {
                new PayloadParameter("Message", PayloadParamType.String)
            });
        }

        public static PayloadParameter[] GetParams(MessageType type)
        {
            return myPayloads[type];
        }

        public class PayloadParameter
        {
            public string Name;
            public PayloadParamType ParamType;

            public PayloadParameter(string name, PayloadParamType paramType)
            {
                Name = name;
                ParamType = paramType;
            }
        }

        public enum PayloadParamType
        {
            Boolean,
            Byte,
            Integer,
            String,
            ServerState,
            BotDifficulty,
            CardRank,
            CardSuit,
            PlayerID
        }
    }
}
