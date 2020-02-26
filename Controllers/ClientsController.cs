using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Data;
using eTaskAdvisor.WebApi.Data.Error;
using eTaskAdvisor.WebApi.Helpers;
using eTaskAdvisor.WebApi.Models;
using eTaskAdvisor.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetaPoco;

namespace eTaskAdvisor.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : AppController
    {
        private readonly IUserService _userService;

        public ClientsController(IDatabase database, IUserService userService) : base(database)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public Client Authenticate([FromBody] Data.AuthModel model)
        {
            var client = _userService.Authenticate(model.Password);
            return client;
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> ClientTasks([FromQuery(Name = "done")] bool done)
        {
            var client = await GetClient();

            if (client == null)
            {
                return NotFound(new ApiNotFoundError("The client has not been found"));
            }

            const string sql =
                @"SELECT
                    tasks.*,
                    activities.activity_id,
                    activities.name activity_name,
                    activities.description activity_description
                FROM tasks
                JOIN activities USING(activity_id)
                WHERE tasks.client_id = @0 AND tasks.done = @1
                ORDER BY at DESC
                LIMIT 100
                ";

            var tasks = Database.Query<ClientTask>(sql, client.ClientId, done)
                .Select(result => new ClientTask
                {
                    ActivityId = result.ActivityId,
                    TaskId = result.TaskId,
                    Subject = result.Subject,
                    At = result.At,
                    Duration = result.Duration,
                    Done =  result.Done,
                    Activity = new Activity {Name = result.ActivityName, Description = result.ActivityDescription},
                });

            return Ok(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = await GetClient();
            if (client == null)
            {
                return NotFound(new ApiNotFoundError("The client has not been found"));
            }

            return Ok(client.ForPublic());
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> PostClientTasks([FromBody] ClientTask task)
        {
            var client = await GetClient();
            if (client == null)
            {
                return NotFound(new ApiNotFoundError("The client has not been found"));
            }

            task.ClientId = client.ClientId;
            await Database.InsertAsync(task);
            task.Activity = await Database.FirstAsync<Activity>("WHERE activity_id = @0", task.ActivityId);
            return Ok(task);
        }

        [HttpPost("done")]
        public async Task<IActionResult> DoneTask([FromBody] ClientTask task)
        {
            var client = await GetClient();
            if (client == null)
            {
                return NotFound(new ApiNotFoundError("The client has not been found"));
            }

            var result = await Database.FirstAsync<ClientTask>("WHERE client_id = @0 AND task_id = @1", client.ClientId, task.TaskId);
            result.Done = task.Done;
            return Ok(await Database.UpdateAsync(result));
        }

        [HttpDelete("tasks/{taskId}")]
        public async Task<IActionResult> DeleteClientTasks(int taskId)
        {
            var client = await GetClient();
            if (client == null)
            {
                return NotFound(new ApiNotFoundError("The client has not been found"));
            }

            var task = await Database.FirstOrDefaultAsync<Task>(@"SELECT * FROM tasks WHERE task_id = @0 AND client_id = @1", taskId, client.ClientId);
            if (task == null)
            {
                return NotFound(new ApiNotFoundError("The task could not be found for deletion"));
            }
            return Ok(await Database.DeleteAsync<ClientTask>(taskId));
        }

        [HttpPut("tasks")]
        public async Task<IActionResult> PutClientTasks([FromBody] ClientTask factor)
        {
            await Database.UpdateAsync(factor);
            return Ok(factor);
        }
    }
}