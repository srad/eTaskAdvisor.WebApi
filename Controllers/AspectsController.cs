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
    public class AspectsController : AppController
    {
        public AspectsController(IOptions<MongoSettings> settings) : base(settings)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public async Task<IEnumerable<Aspect>> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            }

            return await DbContext.Aspects
                .Aggregate()
                .Lookup<Aspect, AspectType, AspectTempResult>(DbContext.AspectTypes,
                    localField => localField.AspectTypeId,
                    foreignField => foreignField.AspectTypeId,
                    result => result.AspectType)
                .Unwind<AspectTempResult, AspectResult>(a => a.AspectType)
                .Sort(Builders<AspectResult>.Sort.Ascending("Name"))
                .ToListAsync();
        }

        [HttpGet("affect/{aspectId}")]
        public async Task<IEnumerable<Affect>> AffectedBy(string aspectId)
        {
            return await DbContext
                .Affects
                .Aggregate()
                .Match(a => a.AspectId == aspectId)
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
        public async Task<AspectResult> Post([FromBody] Aspect aspect)
        {
            await DbContext.Aspects.InsertOneAsync(aspect);
            
            return await DbContext.Aspects
                .Aggregate()
                .Match(a => a.AspectId == aspect.AspectId)
                .Lookup<Aspect, AspectType, AspectTempResult>(DbContext.AspectTypes,
                    localField => localField.AspectTypeId,
                    foreignField => foreignField.AspectTypeId,
                    result => result.AspectType)
                .Unwind<AspectTempResult, AspectResult>(a => a.AspectType)
                .Sort(Builders<AspectResult>.Sort.Ascending("Name"))
                .FirstAsync();
        }

        [HttpPut]
        public async Task<Aspect> Put([FromBody] Aspect aspect)
        {
            await DbContext.Aspects.ReplaceOneAsync(a => a.AspectId == aspect.AspectId, aspect);
            return aspect;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id) =>
            (await DbContext.Aspects.DeleteOneAsync(a => a.AspectId == id)).DeletedCount > 0;
    }
}