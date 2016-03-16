using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DurakFinal.Common
{
    /// <summary>
    /// Contains usefull extension methods for security
    /// </summary>
    public static class SecurityUtils
    {
        private static HashAlgorithm myHashAlgorithm;

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
    }
}
