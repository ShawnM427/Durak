using System.Security.Cryptography;
using System.Text;

namespace Durak.Common
{
    /// <summary>
    /// Contains usefull extension methods for security
    /// </summary>
    public static class SecurityUtils
    {
        /// <summary>
        /// Stores the hashing algorithm to use
        /// </summary>
        private static HashAlgorithm myHashAlgorithm;

        /// <summary>
        /// Static constructor
        /// </summary>
        static SecurityUtils()
        {
            myHashAlgorithm = new SHA256CryptoServiceProvider();
        }

        /// <summary>
        /// Hashes this string using SHA-256 hash algorithm
        /// </summary>
        /// <param name="text">The text to hash</param>
        /// <returns>The hashed text</returns>
        public static string Hash(this string text)
        {
            return Encoding.ASCII.GetString(myHashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(text)));
        }

        /// <summary>
        /// Returns the bit at the given index in a byte
        /// </summary>
        /// <param name="b">The byte to get the bit at</param>
        /// <param name="index">The index in the byte to get</param>
        /// <returns>True if the bit at <i>index</i> is set</returns>
        public static bool BitAt(this byte b, int index)
        {
            return (b & (1 << (7 - index))) != 0;
        }
    }
}
