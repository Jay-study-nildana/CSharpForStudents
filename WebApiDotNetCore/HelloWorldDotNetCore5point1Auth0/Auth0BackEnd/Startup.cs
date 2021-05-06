using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApiDotNetCore5point1SQLite.DatabaseContext;
using Microsoft.EntityFrameworkCore;
//for the sake of Auth0
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApiDotNetCore5point1SQLite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //let's finally add CORS
            //lets add some CORS stuff 
            //this is the standard way
            //check appsettings.json and update the values.
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(Configuration["CorsOriginLocalHost"],
                                        Configuration["CorsOriginStaging"],
                                        Configuration["CorsOriginProduction"]);
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                });
            });

            //inject the context
            //the connection string is stored in appsettings.json
            services.AddDbContext<TodoContext>(options => options.UseSqlite(Configuration["SqliteConnectionString"]));

            //lets add our authentication.
            //the positioning of all the services, including authentication is sort of important
            //so, I am putting auth services before AddControllers on purpose.

            //get the domain of authentication from appsettings file.
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = Configuration["Auth0:Audience"];
                });

            //here we add our authorization also called as scopes also called as policy.
            //this here, is the policy which is called "Permissions" or Scope in Auth0 dashboard
            //ensure that the name of the scope is exactly as it appears.
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy("read:messages", policy => policy.Requirements
                                                             .Add(new HasScopeRequirement("read:messages", domain)));
                }
            );
            //now include the scope handler. this is the class which checks for available scopes
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiDotNetCore5point1SQLite", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiDotNetCore5point1SQLite v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //you must ensure the CORS code comes here
            app.UseCors();

            //similar to what we did in ConfigureServices
            //the exact location where these things are included is very important
            //it is essential, for instance, UseAuthorization comes after UseAuthentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
