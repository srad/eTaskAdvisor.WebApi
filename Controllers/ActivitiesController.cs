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
    public class ActivitiesController : AppController
    {
        public ActivitiesController(IDatabase database) : base(database)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public IEnumerable<Activity> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            } 
            return Database.Fetch<Activity>(@"SELECT * FROM activities ORDER BY name LIMIT @0,@1", page, limit).ToList();
        }

        [HttpGet("affect/{activityId:int}")]
        public IEnumerable<Affect> AffectedBy(int activityId)
        {
            const string sql = @"SELECT * FROM affects
                JOIN factors USING(factor_id)
                JOIN influences USING(influence_name)
                WHERE activity_id = @0";

            return Database.Fetch<Affect, Factor, Influence>(sql, activityId);
        }

        [HttpPost]
        public async Task<Activity> Post([FromBody] Activity activity)
        {
            await Database.InsertAsync(activity);
            return activity;
        }

        [HttpPut]
        public async Task<Activity> Put([FromBody] Activity activity)
        {
            await Database.UpdateAsync(activity);
            return activity;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await Database.DeleteAsync<Activity>(id);
        }
    }
}