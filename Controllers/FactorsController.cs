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
    public class FactorsController : ControllerBase
    {
        private readonly IDatabase db;

        public FactorsController(IDatabase db)
        {
            this.db = db;
        }

        [HttpGet("{page?}/{limit?}")]
        public IEnumerable<Factor> Get(int page = 0, int limit = 10)
        {
            var sql = @"SELECT * FROM factors LIMIT @0,@1";
            return db.Fetch<Factor>(sql, page, limit).ToList();
        }

        [HttpPost]
        public async Task<Factor> Post([FromBody] Factor factor)
        {
            await db.InsertAsync(factor);
            return factor;
        }

        [HttpPut]
        public async Task<Factor> Put([FromBody] Factor factor)
        {
            await db.UpdateAsync(factor);
            return factor;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await db.DeleteAsync<Factor>(id);
        }
    }
}
