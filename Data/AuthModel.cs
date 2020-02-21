using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace eTaskAdvisor.WebApi.Data
{
    public class AuthModel
    {
        public string Password { get; set; }
        public void HashPassword(string salt)
        {
            if (this.Password == null)
            {
                return;
            }
            this.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: Password,
            salt: new System.Text.ASCIIEncoding().GetBytes(salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        }
    }
}