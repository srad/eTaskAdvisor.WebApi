using System;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Models;
using eTaskAdvisor.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace eTaskAdvisor.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : AppController
    {
        private readonly IUserService _userService;

        public ClientsController(IOptions<MongoSettings> settings, IUserService userService) : base(settings)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<Client> Authenticate([FromBody] Data.AuthModel model)
        {
            var client = await _userService.Authenticate(model.Password);
            return client;
        }


        [HttpGet]
        public async Task<Client> Get()
        {
            var client = await GetClient();
            if (client == null)
            {
                throw new Exception("Client not found");
            }

            return client;
        }
    }
}