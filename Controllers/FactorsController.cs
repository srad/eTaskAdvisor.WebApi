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
    public class FactorsController : AppController
    {
        public FactorsController(IOptions<MongoSettings> settings) : base(settings)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public async Task<IEnumerable<Factor>> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            }

            return await DbContext
                .Factors
                .Find(_ => true)
                .Skip(page)
                .Limit(limit)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<Factor> Post([FromBody] Factor factor)
        {
            await DbContext.Factors.InsertOneAsync(factor);
            return factor;
        }

        [HttpPut]
        public async Task<Factor> Put([FromBody] Factor factor)
        {
            await DbContext.Factors.ReplaceOneAsync(f => f.FactorId == factor.FactorId, factor);
            return factor;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id) =>
            (await DbContext.Factors.DeleteOneAsync(f => f.FactorId == id)).DeletedCount > 0;
    }
}