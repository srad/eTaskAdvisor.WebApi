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
    public class FactorsController : AppController
    {
        public FactorsController(IDatabase database) : base(database)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public async Task<IEnumerable<Factor>> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            } 
            return await Database.FetchAsync<Factor>(@"SELECT * FROM factors ORDER BY  name LIMIT @0,@1", page, limit);
        }

        [HttpPost]
        public async Task<Factor> Post([FromBody] Factor factor)
        {
            await Database.InsertAsync(factor);
            return factor;
        }

        [HttpPut]
        public async Task<Factor> Put([FromBody] Factor factor)
        {
            await Database.UpdateAsync(factor);
            return factor;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await Database.DeleteAsync<Factor>(id);
        }
    }
}