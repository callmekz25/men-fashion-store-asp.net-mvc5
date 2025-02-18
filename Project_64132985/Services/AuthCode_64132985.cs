using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Project_64132985.Services
{
    public class AuthCode_64132985
    {
        public static string GenerateCode(int length = 6)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new char[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);

                for (int i = 0; i < length; i++)
                {
                    code[i] = characters[bytes[i] % characters.Length];
                }
            }
            return new string(code);
        }
    }
}