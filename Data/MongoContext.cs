using eTaskAdvisor.WebApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace eTaskAdvisor.WebApi.Data
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.Database);
            }
        }

        public IMongoCollection<Client> Clients => _database.GetCollection<Client>("Clients");
        public IMongoCollection<Aspect> Aspects => _database.GetCollection<Aspect>("Aspects");
        public IMongoCollection<Factor> Factors => _database.GetCollection<Factor>("Factors");
        public IMongoCollection<Influence> Influences => _database.GetCollection<Influence>("Influences");
        public IMongoCollection<AspectType> AspectTypes => _database.GetCollection<AspectType>("AspectTypes");
        public IMongoCollection<Affect> Affects => _database.GetCollection<Affect>("Affects");
        public IMongoCollection<ClientTask> ClientTasks => _database.GetCollection<ClientTask>("ClientTasks");
    }
}