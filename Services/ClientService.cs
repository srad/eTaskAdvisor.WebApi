using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using eTaskAdvisor.WebApi.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetaPoco;
using System.Collections;
using eTaskAdvisor.WebApi.Data;
using eTaskAdvisor.WebApi.Models;

namespace eTaskAdvisor.WebApi.Services
{
    public interface IUserService
    {
        Client Authenticate(string password);
    }

    public class UserService : IUserService
    {
        private readonly IDatabase _db;

        private readonly Data.AppSettings _settings;

        public UserService(IOptions<Data.AppSettings> appSettings, IDatabase db)
        {
            _settings = appSettings.Value;
            this._db = db;
        }

        // Create random password, store hashed, but return the original password.
        private Client Create()
        {
            var pass = SecurityHelper.RandomString(50);
            var c = new Client { Password = pass };

            c.Password = SecurityHelper.HashPassword(pass, _settings.Secret);
            c.Token = CreateToken(c.ClientId.ToString());
            _db.Insert(c);
            c.Password = pass;

            return c;
        }

        public Client Authenticate(string password)
        {
            Client user = null;

            if (password != null)
            {
                var hashed = SecurityHelper.HashPassword(password, _settings.Secret);
                var sql = @"SELECT * FROM clients WHERE password = '" + hashed + "' LIMIT 1";
                user = _db.Query<Client>(sql).SingleOrDefault();
                if (user == null)
                {
                    throw new Exception("User not found by password");
                }
            }

            if (user == null)
            {
                var created = Create();
                return created.WithPassword();
            }

            user.Token = CreateToken(user.ClientId.ToString());
            return user.ForPublic();
        }

        string CreateToken(string createFrom)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, createFrom)
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}