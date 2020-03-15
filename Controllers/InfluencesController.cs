using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace eTaskAdvisor.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Consumes("application/json")]
    [Route("[controller]")]
    public class InfluencesController : AppController
    {
        public InfluencesController(IOptions<MongoSettings> settings) : base(settings)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public async Task<IEnumerable<Influence>> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            }

            return await DbContext
                .Influences
                .Find(_ => true)
                .Skip(page)
                .Limit(limit)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<Influence> Post([FromBody] Influence influence)
        {
            await DbContext.Influences.InsertOneAsync(influence);
            return influence;
        }

        [HttpPut]
        public async Task<Influence> Put([FromBody] Influence influence)
        {
            await DbContext.Influences.ReplaceOneAsync(i => i.InfluenceId == influence.InfluenceId, influence);
            return influence;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id) =>
            (await DbContext.Influences.DeleteOneAsync(influence => influence.InfluenceId == id)).DeletedCount > 0;
    }
}