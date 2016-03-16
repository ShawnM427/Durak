using DurakFinal.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakFinal.Common
{
    /// <summary>
    /// Represents the state of a game. This is a re-usable class that can be used by any card game
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Stores the dictionary of parameters
        /// </summary>
        private Dictionary<string, object> myParameters;

        /// <summary>
        /// Invoked when a single state withing this game state is changed
        /// </summary>
        public event EventHandler<StateChangedEventArgs> OnStateChanged;

        /// <summary>
        /// Creates a new instance of a game state
        /// </summary>
        public GameState()
        {
            myParameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Private method used by all sets that handles setting a parameter
        /// </summary>
        /// <typeparam name="T">The type of the parameter to set</typeparam>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        private void InternalSet<T>(string name, T value)
        {
            // If the parameter does not exist, add it, otherwise update it
            if (!myParameters.ContainsKey(name))
                myParameters.Add(name, value);
            else
            {
                // Make sure it is the same type, or throw an exception
                if (myParameters[name].GetType() == typeof(T))
                    myParameters[name] = value;
                else
                    throw new InvalidCastException(string.Format("Cannot cast parameter of type {0} to {1}", myParameters[name].GetType().Name, typeof(T).Name));
            }

            // invoke the state change if an event is attached
            if (OnStateChanged != null)
                OnStateChanged.Invoke(this, new StateChangedEventArgs(name, value));
        }

        /// <summary>
        /// Sets the given parameter to a byte value
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, byte value)
        {
            InternalSet(name, value);
        }

        /// <summary>
        /// Sets the given parameter to a character value
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, char value)
        {
            InternalSet(name, value);
        }

        /// <summary>
        /// Sets the given parameter to a 16 bit integer value
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, short value)
        {
            InternalSet(name, value);
        }

        /// <summary>
        /// Sets the given parameter to a 32 bit integer value
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, int value)
        {
            InternalSet(name, value);
        }

        /// <summary>
        /// Sets the given parameter to a boolean value
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, bool value)
        {
            InternalSet(name, value);
        }

        /// <summary>
        /// Sets the given parameter to a card suit
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, CardSuit value)
        {
            InternalSet(name, value);
        }

        /// <summary>
        /// Sets the given parameter to a card rank
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, CardRank value)
        {
            InternalSet(name, value);
        }

        /// <summary>
        /// Sets the given parameter to a string value
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, string value)
        {
            InternalSet(name, value);
        }
        
        /// <summary>
        /// Sets the given parameter to a card 
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, PlayingCard value)
        {
            InternalSet(name, value);
        }

        /// <summary>
        /// Private method used by all gets that handles getting a parameter.
        /// Note that if a parameter is not defined, the default for that type is returned
        /// </summary>
        /// <typeparam name="T">The type of the parameter to get</typeparam>
        /// <param name="name">The name of the parameter to get</param>
        private T GetValueInternal<T>(string name)
        {
            // If we have that parameter, then get it
            if (myParameters.ContainsKey(name))
            {
                // Get the object value
                object value = myParameters[name];

                // Verify type before returning
                if (value is T)
                    return (T)value;
                else
                    throw new InvalidCastException(string.Format("Cannot cast {0} to {1}", value.GetType().Name, typeof(T).Name));
            }
            // Otherwise make a default one
            else
            {
                // Add and return a default parameter of type T
                myParameters.Add(name, default(T));
                return (T)myParameters[name];
            }
        }
        
        /// <summary>
        /// Gets the parameter with the given name as a byte
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public int GetValueByte(string name)
        {
            return GetValueInternal<byte>(name);
        }

        /// <summary>
        /// Gets the parameter with the given name as a character
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public int GetValueChar(string name)
        {
            return GetValueInternal<char>(name);
        }

        /// <summary>
        /// Gets the parameter with the given name as a 16 bit integer
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public int GetValueShort(string name)
        {
            return GetValueInternal<short>(name);
        }

        /// <summary>
        /// Gets the parameter with the given name as a 32 bit integer
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public int GetValueInt(string name)
        {
            return GetValueInternal<int>(name);
        }

        /// <summary>
        /// Gets the parameter with the given name as a boolean
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public bool GetValueBool(string name)
        {
            return GetValueInternal<bool>(name);
        }

        /// <summary>
        /// Gets the parameter with the given name as a card rank
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public CardRank GetValueCardRank(string name)
        {
            return GetValueInternal<CardRank>(name);
        }

        /// <summary>
        /// Gets the parameter with the given name as a card suit
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public CardSuit GetValueCardSuit(string name)
        {
            return GetValueInternal<CardSuit>(name);
        }

        /// <summary>
        /// Gets the parameter with the given name as a string
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public string GetValueString(string name)
        {
            return GetValueInternal<string>(name);
        }

        /// <summary>
        /// Gets the parameter with the given name as a playing card
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The parameter with the given name</returns>
        public PlayingCard GetCard(string name)
        {
            return GetValueInternal<PlayingCard>(name);
        }
    }
}
