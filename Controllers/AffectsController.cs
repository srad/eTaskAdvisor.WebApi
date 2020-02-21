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
    [Route("[controller]")]
    public class AffectsController : ControllerBase
    {
        private readonly IDatabase db;

        public AffectsController(IDatabase db)
        {
            this.db = db;
        }

        [HttpGet("{page?}/{limit?}")]
        public IEnumerable<Affect> Get(int page = 0, int limit = 10)
        {
            var sql = @"SELECT * FROM affects
                JOIN activities USING(activity_id)
                JOIN influences USING(influence_name)
                JOIN factors USING(factor_id)
                LIMIT @0,@1";

            return db.Fetch<Affect, Activity, Influence, Factor>(sql, page, limit).ToList();
        }

        [HttpPost]
        public async Task<Affect> Post([FromBody] Affect affect)
        {
            await db.InsertAsync(affect);
            return affect;
        }

        [HttpPut]
        public async Task<Affect> Put([FromBody] Affect activity)
        {
            await db.UpdateAsync(activity);
            return activity;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await db.DeleteAsync<Affect>(id);
        }
    }
}
