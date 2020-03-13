using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Data;
using eTaskAdvisor.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;

namespace eTaskAdvisor.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AffectsController : AppController
    {
        public AffectsController(IDatabase database) : base(database)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public IEnumerable<Affect> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            } 
            const string sql = @"SELECT * FROM affects
                JOIN aspects USING(aspect_id)
                JOIN influences USING(influence_name)
                JOIN factors USING(factor_id)
                ORDER BY aspects.name, factors.name
                LIMIT @0,@1";

            return Database.Fetch<Affect, Aspect, Influence, Factor>(sql, page, limit).ToList();
        }

        [HttpPost]
        public async Task<Affect> Post([FromBody] Affect affect)
        {
            await Database.InsertAsync(affect);

            const string sql = @"SELECT * FROM affects
                JOIN aspects USING(aspect_id)
                JOIN influences USING(influence_name)
                JOIN factors USING(factor_id)
                WHERE affect_id = @0";

            return Database.Fetch<Affect, Aspect, Influence, Factor>(sql, affect.AffectId).FirstOrDefault();
        }

        [HttpPut]
        public async Task<Affect> Put([FromBody] Affect aspectId)
        {
            await Database.UpdateAsync(aspectId);
            return aspectId;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await Database.DeleteAsync<Affect>(id);
        }
    }
}