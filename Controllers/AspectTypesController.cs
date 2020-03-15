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
    [Route("[controller]")]
    public class AspectTypesController : AppController
    {
        public AspectTypesController(IOptions<MongoSettings> settings) : base(settings)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public async Task<IEnumerable<AspectType>> Get(int page = 0, int limit = 100)
        {
            return await DbContext.AspectTypes
                .Find(_ => true)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<AspectType> Post([FromBody] AspectType type)
        {
            await DbContext.AspectTypes.InsertOneAsync(type);
            return type;
        }

        [HttpPut]
        public async Task<AspectType> Put([FromBody] AspectType type)
        {
            await DbContext.AspectTypes.ReplaceOneAsync(a => a.AspectTypeId == type.AspectTypeId, type);
            return type;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => (await DbContext.AspectTypes.DeleteOneAsync(type => type.AspectTypeId == id)).DeletedCount > 0;
    }
}