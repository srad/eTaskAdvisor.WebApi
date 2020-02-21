using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Data.SchemaPoco;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetaPoco;

namespace eTaskAdvisor.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IDatabase db;

        public ActivitiesController(IDatabase db)
        {
            this.db = db;
        }

        [HttpGet("{page?}/{limit?}")]
        public IEnumerable<Activity> Get(int page = 0, int limit = 10)
        {
            var sql = @"SELECT * FROM activities LIMIT @0,@1";
            return db.Fetch<Activity>(sql, page, limit).ToList();
        }

        [HttpPost]
        public async Task<Activity> Post([FromBody] Activity activity)
        {
            await db.InsertAsync(activity);
            return activity;
        }

        [HttpPut]
        public async Task<Activity> Put([FromBody] Activity activity)
        {
            await db.UpdateAsync(activity);
            return activity;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await db.DeleteAsync<Activity>(id);
        }
    }
}
