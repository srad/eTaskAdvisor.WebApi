using eTaskAdvisor.WebApi.Data;
using eTaskAdvisor.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace eTaskAdvisor.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddTransient<MongoContext>();

            services.Configure<MongoSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            var context = services.BuildServiceProvider().GetService<MongoContext>();
            if (context.Influences.CountDocuments(_ => true) == 0)
            {
                context.Influences.InsertOne(new Influence {Name = "Positive", Description = "The effect is positive"});
                context.Influences.InsertOne(new Influence {Name = "Negative", Description = "The effect is negative"});
                context.Influences.InsertOne(new Influence {Name = "Unclear", Description = "The effect is unclear yet"});
                context.Influences.InsertOne(new Influence {Name = "Indifferent", Description = "The effect make no difference"});
            }

            if (context.AspectTypes.CountDocuments(_ => true) == 0)
            {
                context.AspectTypes.InsertOne(new AspectType {Name = "Activity", Value = "activity", Description = "Actions taken to complete a part of a learning task."});
                context.AspectTypes.InsertOne(new AspectType {Name = "General Learning", Value = "learning", Description = "General aspects affecting learning."});
            }

            if (context.FactorTypes.CountDocuments(_ => true) == 0)
            {
                context.FactorTypes.InsertOne(new FactorType {Name = "Air Quality", Description = "Indoor or outdoor air quality"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Color", Description = "Exposure to color"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Decoration", Description = "Decoration or other symbolic influences"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Environment", Description = "Any kind of specific environment, like natural ones"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Light", Description = "Natural or artificial light"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Music", Description = "Different Types of music"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Noise", Description = "Different noise types"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Temperature", Description = "Room temperature, outdoor temperature"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Technology", Description = "Any technology affecting learning in the environment"});
                context.FactorTypes.InsertOne(new FactorType {Name = "Other", Description = "Other types of influences"});
            }

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<Data.AppSettings>();
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<Services.IUserService, Services.UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Runs behind a SSL proxy.
            // app.UseHttpsRedirection();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}