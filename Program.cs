using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetaPoco;
using PetaPoco.Providers;
using eTaskAdvisor.WebApi.Data.SchemaPoco;
using eTaskAdvisor.WebApi.Helpers;

namespace eTaskAdvisor.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var settings = new Data.AppSettings();
            config.GetSection("AppSettings").Bind(settings);

            // Or the fluent configuration (PostgreSQL as an example)
            var db = DatabaseConfiguration.Build()
                    .UsingConnectionString(config.GetConnectionString("PocoConnection"))
                    .UsingProvider<MySqlDatabaseProvider>()
                    .UsingDefaultMapper<ConventionMapper>(m =>
                    {
                        m.InflectTableName = (inflector, s) => inflector.Pluralise(inflector.Underscore(s));
                        m.InflectColumnName = (inflector, s) => inflector.Underscore(s);
                    }).Create();

            db.Delete<Affect>("");
            db.Delete<ClientTask>("");
            db.Delete<Activity>("");
            db.Delete<Factor>("");
            db.Delete<Influence>("");
            db.Delete<Client>("");

            if (db.Query<Influence>().Count() == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var pass = "password" + i;
                    var c = new Client { Name = "Client " + i, Password = pass };
                    c.Password = SecurityHelper.HashPassword(pass, settings.Secret);
                    db.Insert(c);
                }

                for (int i = 0; i < 30; i++)
                {
                    db.Insert(new Activity { Name = "Activity " + i, Description = "This is an activity description for Acvitity " + i });
                }

                for (int i = 0; i < 50; i++)
                {
                    db.Insert(new Factor { Name = "Factor " + i, Description = "This is an factor description for Factor " + i });
                }

                db.Insert(new Influence { InfluenceDisplay = "Positive", InfluenceName = "positive" });
                db.Insert(new Influence { InfluenceDisplay = "Negative", InfluenceName = "negative" });
                db.Insert(new Influence { InfluenceDisplay = "Indifferent", InfluenceName = "indifferent" });
                db.Insert(new Influence { InfluenceDisplay = "Unclear", InfluenceName = "unclear" });

                for (int i = 0; i < 100; i++)
                {
                    var a = db.Query<Activity>("SELECT * FROM activities ORDER BY RAND() LIMIT 1;").First();
                    var f = db.Query<Factor>("SELECT * FROM factors ORDER BY RAND() LIMIT 1;").First();
                    var inf = db.Query<Influence>("SELECT * FROM influences ORDER BY RAND() LIMIT 1;").First();

                    // Don't violate primary key constraint
                    var ex = db.Exists<Affect>("WHERE activity_id = @0 AND factor_id = @1 AND influence_name = @2", a.ActivityId, f.FactorId, inf.InfluenceName);
                    if (!ex)
                    {
                        db.Insert(new Affect
                        {
                            ActivityId = a.ActivityId,
                            FactorId = f.FactorId,
                            InfluenceName = inf.InfluenceName,
                            Source = "https://www.mysqltutorial.org/select-random-records-database-table.aspx",
                            Description = "This is some further description why " + a.Name + " is affected by" + f.Name + "."
                        });
                    }
                }

                for (int i = 0; i < 100; i++)
                {
                    var a = db.Query<Activity>("SELECT * FROM activities ORDER BY RAND() LIMIT 1;").First();
                    var c = db.Query<Client>("SELECT * FROM clients ORDER BY RAND() LIMIT 1;").First();
                    db.Insert(new ClientTask
                    {
                        ActivityId = a.ActivityId,
                        ClientId = c.ClientId,
                        Subject = "",
                        At = DateTime.Now,
                        Duration = new Random().Next(1, 10)
                    });
                }
            }

            /*
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    DbInit.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            */

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
