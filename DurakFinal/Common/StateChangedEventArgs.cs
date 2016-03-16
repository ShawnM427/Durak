using System;

namespace DurakFinal.Common
{
    /// <summary>
    /// Represents the event arguments for a state change event
    /// </summary>
    public class StateChangedEventArgs : EventArgs
    {
        private string myName;
        private object myValue;
        
        /// <summary>
        /// Creates a new instance of a state changed 
        /// </summary>
        /// <param name="name">The name of the parameter that has changed</param>
        /// <param name="value">The new value of the parameter</param>
        public StateChangedEventArgs(string name, object value)
        {
            myName = name;
            myValue = value;
        }

        private T GetValueInternal<T>()
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
    }
}