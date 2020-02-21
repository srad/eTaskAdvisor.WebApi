using System;
using System.Collections.Generic;
using System.Linq;
using eTaskAdvisor.WebApi.Data.SchemaPoco;
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
    public class ClientsController : ControllerBase
    {
        private readonly IDatabase db;
        private readonly IUserService userService;
        private readonly IOptions<Data.AppSettings> appSettings;

        public ClientsController(IDatabase db, IUserService userService, IOptions<Data.AppSettings> appSettings)
        {
            this.db = db;
            this.userService = userService;
            this.appSettings = appSettings;
        }

        [HttpGet("{page?}/{limit?}")]
        public IEnumerable<ClientTask> Get(int page = 0, int limit = 10)
        {
            var sql = @"SELECT * from tasks
                JOIN activities USING(activity_id)
                WHERE client_id = @0";
            var userId = int.Parse(User.Identity.Name);
            return db.Fetch<ClientTask>(sql, userId);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public Client Authenticate([FromBody]Data.AuthModel model)
        {
            var client = userService.Authenticate(model.Password);
            return client;
        }
    }
}
