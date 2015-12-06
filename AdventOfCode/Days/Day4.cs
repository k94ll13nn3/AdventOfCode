using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Days
{
    internal class Day4 : Day
    {
        public Day4() : base(4)
        {
        }

        public static string ComputeMD5(string password)
        {
            // byte array representation of that string
            var encodedPassword = new UTF8Encoding().GetBytes(password);

            // need MD5 to calculate the hash
            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            var encoded = BitConverter.ToString(hash)
               // without dashes
               .Replace("-", string.Empty)
               // make lowercase
               .ToLower();

            // encoded contains the hash you are wanting
            return encoded;
        }

        public static bool IsMatch(string md5, string pattern)
        {
            return md5.StartsWith(pattern, StringComparison.Ordinal);
        }

        public override object ProcessFirst()
        {
            var md5 = string.Empty;
            var i = 0;

            while (!IsMatch(md5, "00000"))
            {
                md5 = ComputeMD5($"{Content}{i}");
                i++;
            }

            return i - 1;
        }

        public override object ProcessSecond()
        {
            var md5 = string.Empty;
            var i = 0;

            while (!IsMatch(md5, "000000"))
            {
                md5 = ComputeMD5($"{Content}{i}");
                i++;
            }

            return i - 1;
        }
    }
}