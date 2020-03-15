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
    [Route("[controller]")]
    public class AffectsController : AppController
    {
        public AffectsController(IOptions<MongoSettings> settings) : base(settings)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public async Task<IEnumerable<Affect>> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            }

            return await DbContext
                .Affects
                .Aggregate()
                .Lookup<Aspect, AffectTempResult>("Aspects", "AspectId", "AspectId", "Aspect")
                .Lookup<AspectType, AffectTempResult>("AspectTypes", "Aspect.AspectTypeId", "AspectTypeId", "AspectType")
                .Lookup<Factor, AffectTempResult>("Factors", "FactorId", "FactorId", "Factor")
                .Lookup<Influence, AffectTempResult>("Influences", "InfluenceId", "InfluenceId", "Influence")
                .Unwind("Aspect")
                .Unwind("AspectType")
                .Unwind("Factor")
                .Unwind("Influence")
                .As<AffectResult>()
                .ToListAsync();
        }

        [HttpPost]
        public async Task<Affect> Post([FromBody] Affect affect)
        {
            await DbContext.Affects.InsertOneAsync(affect);
            
            return await DbContext
                .Affects
                .Aggregate()
                .Match(a => a.AffectId == affect.AffectId)
                .Lookup<Aspect, AffectTempResult>("Aspects", "AspectId", "AspectId", "Aspect")
                .Lookup<AspectType, AffectTempResult>("AspectTypes", "Aspect.AspectTypeId", "AspectTypeId", "AspectType")
                .Lookup<Factor, AffectTempResult>("Factors", "FactorId", "FactorId", "Factor")
                .Lookup<Influence, AffectTempResult>("Influences", "InfluenceId", "InfluenceId", "Influence")
                .Unwind("Aspect")
                .Unwind("AspectType")
                .Unwind("Factor")
                .Unwind("Influence")
                .As<AffectResult>()
                .FirstAsync();
        }

        [HttpPut]
        public async Task<Affect> Put([FromBody] Affect affect)
        {
            await DbContext.Affects.ReplaceOneAsync(a => a.AffectId == affect.AffectId, affect);
            return affect;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => (await DbContext.Affects.DeleteOneAsync(a => a.AffectId == id)).DeletedCount > 0;
    }
}