using Durak.Common.Cards;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Durak.Common
{
    /// <summary>
    /// Represents the event arguments for a state change event
    /// </summary>
    public class StateParameter : EventArgs
    {
        private static readonly Dictionary<System.Type, Type> SUPPORTED_TYPES = new Dictionary<System.Type, Type>()
        {
            { typeof(byte), Type.Byte },
            { typeof(char), Type.Char },
            { typeof(short), Type.Short },
            { typeof(int), Type.Int },
            { typeof(bool), Type.Bool },
            { typeof(CardSuit), Type.CardSuit },
            { typeof(CardRank), Type.CardRank },
            { typeof(string), Type.String },
            { typeof(PlayingCard), Type.PlayingCard }
        };


        private string myName;
        private object myValue;
        private Type myType;
        
        /// <summary>
        /// Gets this parameter's name
        /// </summary>
        public string Name
        {
            get { return myName; }
        }
        /// <summary>
        /// Get's this parameter's raw value
        /// </summary>
        public object RawValue
        {
            get { return myValue; }
        }
        
        /// <summary>
        /// Creates a new instance of a state changed 
        /// </summary>
        /// <param name="name">The name of the parameter that has changed</param>
        /// <param name="value">The new value of the parameter</param>
        private StateParameter()
        {
        }

        private StateParameter(string name, object value)
        {
            myName = name;
            SetValueInternal(value);
        }

        public static StateParameter Construct<T>(string name, T value)
        {
            StateParameter result = new StateParameter();

            result.myName = name;
            result.SetValueInternal(value);

            return result;
        }

        internal T GetValueInternal<T>()
        {
            if (myValue is T)
                return (T)myValue;
            else
                throw new InvalidCastException(string.Format("Cannot cast {0} to {1}", myValue.GetType().Name, typeof(T).Name));
        }

        public int GetValueByte()
        {
            return GetValueInternal<byte>();
        }

        public int GetValueChar()
        {
            return GetValueInternal<char>();
        }

        public int GetValueShort()
        {
            return GetValueInternal<short>();
        }

        public int GetValueInt()
        {
            return GetValueInternal<int>();
        }

        public bool GetValueBool()
        {
            return GetValueInternal<bool>();
        }

        public CardSuit GetValueCardSuit()
        {
            return GetValueInternal<CardSuit>();
        }

        public CardRank GetValueCardRank()
        {
            return GetValueInternal<CardRank>();
        }

        public string GetValueString()
        {
            return GetValueInternal<string>();
        }
        
        internal void SetValueInternal<T>(T value)
        {
            if (SUPPORTED_TYPES.ContainsKey(typeof(T)))
            {
                myType = SUPPORTED_TYPES[typeof(T)];
                myValue = value;
            }
            else
                throw new InvalidCastException("Type " + value.GetType() + " is not supported");
        }

        public void SetValue(byte value)
        {
            SetValueInternal(value);
        }
        public void SetValue(char value)
        {
            SetValueInternal(value);
        }
        public void SetValue(short value)
        {
            SetValueInternal(value);
        }
        public void SetValue(int value)
        {
            SetValueInternal(value);
        }
        public void SetValue(bool value)
        {
            SetValueInternal(value);
        }
        public void SetValue(CardSuit value)
        {
            SetValueInternal(value);
        }
        public void SetValue(CardRank value)
        {
            SetValueInternal(value);
        }
        public void SetValue(string value)
        {
            SetValueInternal(value);
        }
        public void SetValue(PlayingCard value)
        {
            SetValueInternal(value);
        }

        /// <summary>
        /// Encodes this state parameter to a network message
        /// </summary>
        /// <param name="msg">The message to encode to</param>
        public void Encode(NetOutgoingMessage msg)
        {
            msg.Write(Name);
            msg.Write((byte)myType);

            switch (myType)
            {
                case Type.Byte:
                    msg.Write((byte)myValue);
                    break;
                case Type.Char:
                    msg.Write((char)myValue);
                    break;
                case Type.Short:
                    msg.Write((short)myValue);
                    break;
                case Type.Int:
                    msg.Write((int)myValue);
                    break;
                case Type.Bool:
                    msg.Write((bool)myValue);
                    break;
                case Type.CardSuit:
                    msg.Write((byte)((CardSuit)myValue));
                    break;
                case Type.CardRank:
                    msg.Write((byte)((CardSuit)myValue));
                    break;
                case Type.String:
                    msg.Write((string)myValue);
                    break;
                case Type.PlayingCard:
                    msg.Write((byte)(myValue as PlayingCard).Rank);
                    msg.Write((byte)(myValue as PlayingCard).Suit);
                    break;
            }

            msg.WritePadBits();
        }

        private static void DecodeInternal(String name, Type type, ref StateParameter result, NetIncomingMessage msg)
        {
            switch (type)
            {
                case Type.Byte:
                    result = new StateParameter(name, msg.ReadByte());
                    break;
                case Type.Char:
                    result = new StateParameter(name, msg.ReadByte());
                    break;
                case Type.Short:
                    result = new StateParameter(name, msg.ReadInt16());
                    break;
                case Type.Int:
                    result = new StateParameter(name, msg.ReadInt32());
                    break;
                case Type.Bool:
                    result = new StateParameter(name, msg.ReadBoolean());
                    break;
                case Type.CardSuit:
                    result = new StateParameter(name, (CardSuit)msg.ReadByte());
                    break;
                case Type.CardRank:
                    result = new StateParameter(name, (CardRank)msg.ReadByte());
                    break;
                case Type.String:
                    result = new StateParameter(name, msg.ReadString());
                    break;
                case Type.PlayingCard:
                    result = new StateParameter(name, new PlayingCard((CardRank)msg.ReadByte(), (CardSuit)msg.ReadByte()) { FaceUp = true });
                    break;
            }

            msg.ReadPadBits();
        }

        /// <summary>
        /// Decodes a State parameter from the given message
        /// </summary>
        /// <param name="msg">The message to decode from</param>
        /// <returns>The state loaded from the message</returns>
        public static StateParameter Decode(NetIncomingMessage msg)
        {
            string name = msg.ReadString();
            Type type = (Type)msg.ReadByte();

            StateParameter result = null;

            DecodeInternal(name, type, ref result, msg);


            return result;
        }

        /// <summary>
        /// Decodes a state parameter from the message, using a game state
        /// to update
        /// </summary>
        /// <param name="msg">The message to decode from</param>
        /// <param name="state">The game state to update</param>
        /// <returns>The state parameter loaded from the message</returns>
        public static StateParameter Decode(NetIncomingMessage msg, GameState state)
        {
            string name = msg.ReadString();
            Type type = (Type)msg.ReadByte();

            StateParameter result = state.GetParameter(name, SUPPORTED_TYPES.FirstOrDefault(x => x.Value == type).Key);

            DecodeInternal(name, type, ref result, msg);

            msg.ReadPadBits();

            return result;
        }

        /// <summary>
        /// Gets the type that this parameter encompasses
        /// </summary>
        public enum Type
        {
            Byte,
            Char,
            Short,
            Int,
            Bool,
            CardSuit,
            CardRank,
            String,
            PlayingCard,
            Unknown
        }
    }
}