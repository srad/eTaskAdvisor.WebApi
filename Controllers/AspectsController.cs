﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;

namespace eTaskAdvisor.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AspectsController : AppController
    {
        public AspectsController(IDatabase database) : base(database)
        {
        }

        [HttpGet("{page?}/{limit?}")]
        public IEnumerable<Aspect> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            } 
            return Database.Fetch<Aspect, AspectType>(
                @"SELECT * FROM aspects
                JOIN types on (aspects.type_name = types.type_name)
                ORDER BY name LIMIT @0,@1", page, limit).ToList();
        }

        [HttpGet("types")]
        public IEnumerable<AspectType> GetAspectTypes()
        {
            return Database.Fetch<AspectType>(@"SELECT * FROM types").ToList();
        }

        [HttpGet("affect/{aspectId:int}")]
        public IEnumerable<Affect> AffectedBy(int aspectId)
        {
            const string sql = @"SELECT * FROM affects
                JOIN factors USING(factor_id)
                JOIN influences USING(influence_name)
                WHERE aspect_id = @0";

            return Database.Fetch<Affect, Factor, Influence>(sql, aspectId);
        }

        [HttpPost]
        public async Task<Aspect> Post([FromBody] Aspect aspect)
        {
            await Database.InsertAsync(aspect);
            return aspect;
        }

        [HttpPut]
        public async Task<Aspect> Put([FromBody] Aspect aspect)
        {
            await Database.UpdateAsync(aspect);
            return aspect;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await Database.DeleteAsync<Aspect>(id);
        }
    }
}