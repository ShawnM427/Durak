﻿using Durak.Common.Cards;
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
        /// <summary>
        /// Gets the supported types. I don't like this ATM
        /// </summary>
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

        /// <summary>
        /// Stores the parameter's name
        /// </summary>
        private string myName;
        /// <summary>
        /// Stores the parameter's value
        /// </summary>
        private object myValue;
        /// <summary>
        /// Stores the parameter's type
        /// </summary>
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
        /// Creates a new instance of a state parameter 
        /// </summary>
        private StateParameter()
        {
        }

        /// <summary>
        /// Creates a new instance of a state parameter 
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        private StateParameter(string name, object value)
        {
            myName = name;
            SetValueInternal(value);
        }

        /// <summary>
        /// Constructs a new state parameter of the given type
        /// </summary>
        /// <typeparam name="T">The type of parameter to create</typeparam>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        public static StateParameter Construct<T>(string name, T value)
        {
            StateParameter result = new StateParameter();

            result.myName = name;
            result.SetValueInternal(value);

            return result;
        }

        /// <summary>
        /// Internal method for getting the value as the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>myValue as T</returns>
        internal T GetValueInternal<T>()
        {
            if (myValue is T)
                return (T)myValue;
            else
                throw new InvalidCastException(string.Format("Cannot cast {0} to {1}", myValue.GetType().Name, typeof(T).Name));
        }

        /// <summary>
        /// Get's this parameter's value as a byte
        /// </summary>
        /// <returns>myValue as a byte</returns>
        public int GetValueByte()
        {
            return GetValueInternal<byte>();
        }
        /// <summary>
        /// Get's this parameter's value as a character
        /// </summary>
        /// <returns>myValue as a char</returns>
        public int GetValueChar()
        {
            return GetValueInternal<char>();
        }
        /// <summary>
        /// Get's this parameter's value as a 16-bit signed integer
        /// </summary>
        /// <returns>myValue as a short</returns>
        public int GetValueShort()
        {
            return GetValueInternal<short>();
        }
        /// <summary>
        /// Get's this parameter's value as a 32-bit signed integer
        /// </summary>
        /// <returns>myValue as an int</returns>
        public int GetValueInt()
        {
            return GetValueInternal<int>();
        }
        /// <summary>
        /// Get's this parameter's value as a boolean
        /// </summary>
        /// <returns>myValue as a bool</returns>
        public bool GetValueBool()
        {
            return GetValueInternal<bool>();
        }
        /// <summary>
        /// Get's this parameter's value as a card suit
        /// </summary>
        /// <returns>myValue as a CardSuit</returns>
        public CardSuit GetValueCardSuit()
        {
            return GetValueInternal<CardSuit>();
        }
        /// <summary>
        /// Get's this parameter's value as a card rank
        /// </summary>
        /// <returns>myValue as a CardRank</returns>
        public CardRank GetValueCardRank()
        {
            return GetValueInternal<CardRank>();
        }
        /// <summary>
        /// Get's this parameter's value as a string
        /// </summary>
        /// <returns>myValue as a string</returns>
        public string GetValueString()
        {
            return GetValueInternal<string>();
        }
        /// <summary>
        /// Get's this parameter's value as a playing card
        /// </summary>
        /// <returns>myValue as a PlayingCard</returns>
        public PlayingCard GetValuePlayingCard()
        {
            return GetValueInternal<PlayingCard>();
        }

        /// <summary>
        /// Internal method used to set the value
        /// </summary>
        /// <typeparam name="T">The type to store in this parameter</typeparam>
        /// <param name="value">The value to set</param>
        internal void SetValueInternal<T>(T value)
        {
            // Make sure type is supported
            if (SUPPORTED_TYPES.ContainsKey(typeof(T)))
            {
                // update myType and myValue
                myType = SUPPORTED_TYPES[typeof(T)];
                myValue = value;
            }
            else
                throw new InvalidCastException("Type " + value.GetType() + " is not supported");
        }

        /// <summary>
        /// Set's this parameter to a byte
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetValue(byte value)
        {
            SetValueInternal(value);
        }
        /// <summary>
        /// Set's this parameter to a character
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetValue(char value)
        {
            SetValueInternal(value);
        }
        /// <summary>
        /// Set's this parameter to a 16-bit signed integer
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetValue(short value)
        {
            SetValueInternal(value);
        }
        /// <summary>
        /// Set's this parameter to a 32-bit signed integer
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetValue(int value)
        {
            SetValueInternal(value);
        }
        /// <summary>
        /// Set's this parameter to a boolean
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetValue(bool value)
        {
            SetValueInternal(value);
        }
        /// <summary>
        /// Set's this parameter to a CardSuit
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetValue(CardSuit value)
        {
            SetValueInternal(value);
        }
        /// <summary>
        /// Set's this parameter to a CardRank
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetValue(CardRank value)
        {
            SetValueInternal(value);
        }
        /// <summary>
        /// Set's this parameter to a string
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetValue(string value)
        {
            SetValueInternal(value);
        }
        /// <summary>
        /// Set's this parameter to a Playing Card
        /// </summary>
        /// <param name="value">The value to set</param>
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
            // Write the name and type
            msg.Write(Name);
            msg.Write((byte)myType);

            // Write the value
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

            // Write padding bits
            msg.WritePadBits();
        }

        /// <summary>
        /// Decodes the value from a message
        /// </summary>
        /// <param name="name">The parameter name</param>
        /// <param name="type">The parameter type</param>
        /// <param name="result">The value to store the result in</param>
        /// <param name="msg">The message to read from</param>
        private static void DecodeInternal(String name, Type type, ref StateParameter result, NetIncomingMessage msg)
        {
            // Read the value
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

            // Read the padding bits
            msg.ReadPadBits();
        }

        /// <summary>
        /// Decodes a State parameter from the given message
        /// </summary>
        /// <param name="msg">The message to decode from</param>
        /// <returns>The state loaded from the message</returns>
        public static StateParameter Decode(NetIncomingMessage msg)
        {
            // Read the name and type
            string name = msg.ReadString();
            Type type = (Type)msg.ReadByte();

            // Define the result
            StateParameter result = null;

            // Decode the value
            DecodeInternal(name, type, ref result, msg);

            // Return the result
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
            // Read the name and type
            string name = msg.ReadString();
            Type type = (Type)msg.ReadByte();

            // Get result from state
            StateParameter result = state.GetParameter(name, SUPPORTED_TYPES.FirstOrDefault(x => x.Value == type).Key);

            // Decode the value
            DecodeInternal(name, type, ref result, msg);
            
            // Return the result
            return result;
        }

        /// <summary>
        /// Represents the type a parameter can encompass
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
            PlayingCard
        }
    }
}