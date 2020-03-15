using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTaskAdvisor.WebApi.Data;
using eTaskAdvisor.WebApi.Data.Error;
using eTaskAdvisor.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace eTaskAdvisor.WebApi.Controllers
{
    public abstract class AppController : ControllerBase
    {
        protected readonly MongoContext DbContext;

        protected AppController(IOptions<MongoSettings> settings)
        {
            DbContext = new MongoContext(settings);
        }

        protected async Task<Client> GetClient()
        {
            var token = Request.Headers["Authorization"].First().Split(" ")[1];
            return await DbContext.Clients.Find(client => client.Token == token).FirstAsync();
        }
    }
}