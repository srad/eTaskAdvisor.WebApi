using System;
using System.Linq;
using Bogus;
using eTaskAdvisor.WebApi.Helpers;
using eTaskAdvisor.WebApi.Models;
using Microsoft.Extensions.Configuration;
using PetaPoco;
using PetaPoco.Providers;

namespace eTaskAdvisor.WebApi.Seeds
{
    public static class PocoSeed
    {
        public static void Generate()
        {
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

            var factorData = new Faker<Factor>();
            factorData.RuleFor(factor => factor.Name, faker => "Faker: " + faker.Name.JobArea());
            factorData.RuleFor(factor => factor.Description, faker => faker.Lorem.Paragraph());

            var activityData = new Faker<Activity>();
            activityData.RuleFor(a => a.Name, faker => "Activity: " + faker.Name.JobType());
            activityData.RuleFor(a => a.Description, faker => faker.Name.JobTitle());

            var clientData = new Faker<Client>();
            clientData.RuleFor(c => c.Password, faker => SecurityHelper.HashPassword(faker.Internet.Password(), settings.Secret));
            clientData.RuleFor(c => c.Token, (faker, client) => "");

            var affectData = new Faker<Affect>();
            affectData.RuleFor(a => a.FactorId, faker =>
            {
                var f = db.Query<Factor>("SELECT * FROM factors ORDER BY RAND() LIMIT 1;").First();
                return f.FactorId;
            });
            affectData.RuleFor(a => a.ActivityId, faker =>
            {
                var a = db.Query<Activity>("SELECT * FROM activities ORDER BY RAND() LIMIT 1;").First();
                return a.ActivityId;
            });
            affectData.RuleFor(a => a.InfluenceName, faker =>
            {
                var inf = db.Query<Influence>("SELECT * FROM influences ORDER BY RAND() LIMIT 1;").First();
                return inf.InfluenceName;
            });
            affectData.RuleFor(a => a.Source, faker => faker.Internet.Url());
            affectData.RuleFor(a => a.Description, faker => faker.Commerce.Product());

            var clienTaskData = new Faker<ClientTask>();
            clienTaskData.RuleFor(clientTask => clientTask.ActivityId, faker =>
            {
                var a = db.Query<Activity>("SELECT * FROM activities ORDER BY RAND() LIMIT 1;").First();
                return a.ActivityId;
            });
            clienTaskData.RuleFor(clientTask => clientTask.ClientId, faker =>
            {
                var c = db.Query<Client>("SELECT * FROM clients ORDER BY RAND() LIMIT 1;").First();
                return c.ClientId;
            });
            clienTaskData.RuleFor(clientTask => clientTask.Subject, faker => new Faker().Name.JobTitle());
            clienTaskData.RuleFor(clientTask => clientTask.At, faker => DateTime.Now);
            clienTaskData.RuleFor(clientTask => clientTask.Duration, faker => new Random().Next(1, 10));

            // Delete old data
            db.Delete<Affect>("");
            db.Delete<ClientTask>("");
            db.Delete<Activity>("");
            db.Delete<Factor>("");
            db.Delete<Influence>("");
            db.Delete<Client>("");

            // Start Generation
            if (db.Query<Influence>().Count() == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    db.Insert(clientData.Generate());
                }

                for (int i = 0; i < 30; i++)
                {
                    db.Insert(activityData.Generate());
                }

                for (int i = 0; i < 50; i++)
                {
                    db.Insert(factorData.Generate());
                }

                db.Insert(new Influence {InfluenceDisplay = "Positive", InfluenceName = "positive"});
                db.Insert(new Influence {InfluenceDisplay = "Negative", InfluenceName = "negative"});
                db.Insert(new Influence {InfluenceDisplay = "Indifferent", InfluenceName = "indifferent"});
                db.Insert(new Influence {InfluenceDisplay = "Unclear", InfluenceName = "unclear"});

                for (int i = 0; i < 100; i++)
                {
                    var a = affectData.Generate();
                    // Don't violate primary key constraint
                    var ex = db.Exists<Affect>("WHERE activity_id = @0 AND factor_id = @1 AND influence_name = @2", a.ActivityId, a.FactorId, a.InfluenceName);
                    if (!ex)
                    {
                        db.Insert(a);
                    }
                }

                for (int i = 0; i < 100; i++)
                {
                    db.Insert(clienTaskData.Generate());
                }
            }
        }
    }
}