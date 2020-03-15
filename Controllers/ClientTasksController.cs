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
    [Consumes("application/json")]
    [Route("[controller]")]
    public class ClientTasksController : AppController
    {
        public ClientTasksController(IOptions<MongoSettings> settings) : base(settings)
        {
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("{page?}/{limit?}")]
        public async Task<IEnumerable<ClientTask>> Get(int page = 0, int limit = 100)
        {
            if (limit > 100)
            {
                limit = 100;
            }

            return await DbContext
                .ClientTasks
                .Aggregate()
                .Lookup<Aspect, ClientTaskTempResult>("Aspects", "AspectId", "AspectId", "Aspect")
                .Lookup<AspectType, ClientTaskTempResult>("AspectTypes", "Aspect.AspectTypeId", "AspectTypeId", "AspectType")
                .Unwind("Aspect")
                .Unwind("AspectType")
                .As<ClientTaskResult>()
                .ToListAsync();
        }

        /// <summary>
        /// Get done=false/true
        /// </summary>
        /// <param name="done"></param>
        /// <returns></returns>
        [HttpGet("{done}")]
        public async Task<IEnumerable<ClientTask>> GetDone([FromQuery(Name = "done")] bool done)
        {
            return await DbContext
                .ClientTasks
                .Aggregate()
                .Match(t => t.Done == done)
                .Lookup<Aspect, ClientTaskTempResult>("Aspects", "AspectId", "AspectId", "Aspect")
                .Lookup<AspectType, ClientTaskTempResult>("AspectTypes", "Aspect.AspectTypeId", "AspectTypeId", "AspectType")
                .Unwind("Aspect")
                .Unwind("AspectType")
                .As<ClientTaskResult>()
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ClientTask> Post([FromBody] ClientTask task)
        {
            var client = await GetClient();
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            task.ClientId = client.ClientId;
            await DbContext.ClientTasks.InsertOneAsync(task);

            return await DbContext
                .ClientTasks
                .Aggregate()
                .Match(t => t.TaskId == task.TaskId)
                .Lookup<Aspect, ClientTaskTempResult>("Aspects", "AspectId", "AspectId", "Aspect")
                .Lookup<AspectType, ClientTaskTempResult>("AspectTypes", "Aspect.AspectTypeId", "AspectTypeId", "AspectType")
                .Unwind("Aspect")
                .Unwind("AspectType")
                .As<ClientTaskResult>()
                .FirstAsync();
        }

        [HttpPut("done")]
        public async Task<ClientTask> UpdateDone([FromBody] ClientTask task)
        {
            var update = Builders<ClientTask>.Update.Set(u => u.Done, task.Done);
            
            var client = await GetClient();
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            
            await DbContext.ClientTasks.UpdateOneAsync(
                t => t.ClientId == client.ClientId && t.TaskId == task.TaskId,
                update);

            return await DbContext
                .ClientTasks
                .Aggregate()
                .Match(t => t.TaskId == task.TaskId)
                .Lookup<Aspect, ClientTaskTempResult>("Aspects", "AspectId", "AspectId", "Aspect")
                .Lookup<AspectType, ClientTaskTempResult>("AspectTypes", "Aspect.AspectTypeId", "AspectTypeId", "AspectType")
                .Unwind("Aspect")
                .Unwind("AspectType")
                .As<ClientTaskResult>()
                .FirstAsync();
        }

        [HttpDelete("{taskId}")]
        public async Task<bool> Delete(string taskId)
        {
            var client = await GetClient();
            if (client == null)
            {
                throw new Exception("Client not found");
            }

            var task = await DbContext
                .ClientTasks
                .DeleteOneAsync(t => t.ClientId == client.ClientId && t.TaskId == taskId);

            return task.DeletedCount > 0;
        }
    }
}