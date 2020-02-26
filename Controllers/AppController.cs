using System.Linq;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Data;
using eTaskAdvisor.WebApi.Data.Error;
using eTaskAdvisor.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;

namespace eTaskAdvisor.WebApi.Controllers
{
    public abstract class AppController : ControllerBase
    {
        protected readonly IDatabase Database;

        protected AppController(IDatabase database)
        {
            Database = database;
        }

        protected async  Task<Client> GetClient()
        {
            var token = Request.Headers["Authorization"].First().Split(" ")[1];
            var client = await Database.SingleOrDefaultAsync<Client>(@"SELECT * from clients WHERE token = '" + token + "'");
            return client;
        }
    }
}