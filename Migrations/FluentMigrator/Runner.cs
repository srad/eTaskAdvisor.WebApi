using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    internal class Runner
    {
        private readonly IServiceProvider _serviceProvider;

        public Runner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void UpdateDatabase()
        {
            using var scope = _serviceProvider.CreateScope();
            UpdateDatabase(scope.ServiceProvider);
        }

        public void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}