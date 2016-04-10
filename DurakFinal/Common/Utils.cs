using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Common
{
    public static class Utils
    {
        /// <summary>
        /// Fills the list with a list of instances of all types that inherit from the list's type
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="result">The list to load the result into</param>
        public static void FillTypeList<T>(AppDomain domain, List<T> result)
        {
            // Modified from http://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
            Type[] types = (
                from domainAssembly in domain.GetAssemblies()                   // Get the referenced assemblies
                from assemblyType in domainAssembly.GetTypes()                  // Get all types in assembly
                where typeof(T).IsAssignableFrom(assemblyType)                  // Check to see if the type is a game rule
                where assemblyType.GetConstructor(Type.EmptyTypes) != null      // Make sure there is an empty constructor
                select assemblyType).ToArray();                                 // Convert IEnumerable to array

            // Iterate over them
            for (int index = 0; index < types.Length; index++)
            {
                // Create an instance
                result.Add((T)Activator.CreateInstance(types[index]));
            }
        }

        public static bool CanChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                return false;
            }

            if (value == null)
            {
                return false;
            }

            IConvertible convertible = value as IConvertible;

            if (convertible == null)
            {
                return false;
            }

            return true;
        }
    }
}
