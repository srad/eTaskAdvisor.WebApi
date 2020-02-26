using System;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace eTaskAdvisor.WebApi.Helpers
{
    public static class SecurityHelper
    {
        private static readonly Random Random = new Random();
        const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(Chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static string HashPassword(string password, string salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new System.Text.ASCIIEncoding().GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}