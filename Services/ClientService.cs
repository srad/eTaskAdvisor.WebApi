using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using eTaskAdvisor.WebApi.Data.SchemaPoco;
using eTaskAdvisor.WebApi.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetaPoco;
using System.Collections;

namespace eTaskAdvisor.WebApi.Services
{
    public interface IUserService
    {
        Client Authenticate(string password);
    }

    public class UserService : IUserService
    {
        private readonly IDatabase db;

        private readonly Data.AppSettings settings;

        public UserService(IOptions<Data.AppSettings> appSettings, IDatabase db)
        {
            settings = appSettings.Value;
            this.db = db;
        }

        // Create random password, store hashed, but return the original password.
        private Client Create()
        {
            var pass = SecurityHelper.RandomString(50);
            var c = new Client { Name = "Test User " + db.Query<Client>().Count() + 1, Password = pass };

            c.Password = SecurityHelper.HashPassword(pass, settings.Secret);
            c.Token = CreateToken(c.ClientId.ToString());
            db.Insert(c);
            c.Password = pass;

            return c;
        }

        public Client Authenticate(string password)
        {
            Client user = null;

            if (password != null)
            {
                var hashed = SecurityHelper.HashPassword(password, settings.Secret);
                var sql = @"SELECT * FROM clients WHERE password = '" + hashed + "' LIMIT 1";
                user = db.Query<Client>(sql).SingleOrDefault();
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
            var key = System.Text.Encoding.ASCII.GetBytes(settings.Secret);
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