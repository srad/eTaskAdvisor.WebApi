using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using eTaskAdvisor.WebApi.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Data;
using eTaskAdvisor.WebApi.Models;
using MongoDB.Driver;

namespace eTaskAdvisor.WebApi.Services
{
    public interface IUserService
    {
        Task<Client> Authenticate(string password);
    }

    public class UserService : IUserService
    {
        private readonly MongoContext _context = null;

        private readonly AppSettings _settings;

        public UserService(IOptions<AppSettings> appSettings, IOptions<MongoSettings> settings)
        {
            _settings = appSettings.Value;
            _context = new MongoContext(settings);
        }

        // Create random password, store hashed, but return the original password.
        private async Task<Client> Create()
        {
            // Save the password hashed but return clear to the client
            var pass = SecurityHelper.RandomString(50);
            var client = new Client {Password = SecurityHelper.HashPassword(pass, _settings.Secret)};
            await _context.Clients.InsertOneAsync(client);
            
            client.Token = CreateToken(client.ClientId);
            await _context.Clients.UpdateOneAsync(clientQuery => clientQuery.ClientId == client.ClientId,
                Builders<Client>.Update.Set(c => c.Token, client.Token));
            client.Password = pass;
            return client;
        }

        public async Task<Client> Authenticate(string password)
        {
            Client user = null;

            if (password != null)
            {
                var hashed = SecurityHelper.HashPassword(password, _settings.Secret);
                user = await _context.Clients.Find(client => client.Password == hashed).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new Exception("User not found by password");
                }
            }

            if (user == null)
            {
                return await Create();
                //return created.Wi .WithPassword();
            }

            user.Token = CreateToken(user.ClientId);
            return user.ForPublic();
        }

        private string CreateToken(string createFrom)
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