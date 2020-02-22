using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Data.SchemaPoco;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;

namespace eTaskAdvisor.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Consumes("application/json")]
    [Route("[controller]")]
    public class InfluencesController : AppController
    {
        public InfluencesController(IDatabase database) : base(database)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public IEnumerable<Influence> Get(int page = 0, int limit = 10)
        {
            return Database.Fetch<Influence>(@"SELECT * FROM influences LIMIT @0,@1", page, limit).ToList();
        }

        [HttpPost]
        public async Task<Influence> Post([FromBody] Influence influence)
        {
            await Database.InsertAsync(influence);
            return influence;
        }

        [HttpPut]
        public async Task<Influence> Put([FromBody] Influence influence)
        {
            await Database.UpdateAsync(influence);
            return influence;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await Database.DeleteAsync<Influence>(id);
        }
    }
}