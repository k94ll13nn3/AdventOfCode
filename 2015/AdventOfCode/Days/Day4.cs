// stylecop.header

using System;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Days
{
    internal class Day4 : Day
    {
        public static string ComputeMD5(string password)
        {
            var encodedPassword = new UTF8Encoding().GetBytes(password);

            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            var encoded = BitConverter.ToString(hash)
               .Replace("-", string.Empty)
               .ToLower();

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
                md5 = ComputeMD5($"{this.Content}{i}");
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
                md5 = ComputeMD5($"{this.Content}{i}");
                i++;
            }

            return i - 1;
        }
    }
}