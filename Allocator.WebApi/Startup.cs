using Allocator.DAL;
using Allocator.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Allocator.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment{ get; } 
        public Startup(IHostingEnvironment env,IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);

            #region CORS

            //Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    // builder.WithOrigins("http://localhost:4200") //or builder.AllowAnyOrigin()
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    //.WithMethods("HEAD", "GET", "POST", "PUT", "DELETE") //Or AllowAnyMethod()
                    );
            });

            //Enable CORS for every MVC actions
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            });

            #endregion


            #region Singleton HiLo-GetValue Provider
            var dbFactory = new DbContextFactory(CurrentEnvironment.EnvironmentName);
            services.AddSingleton<IAllocatorGetValProvider>(provider => new AllocatorGetValProvider(dbFactory));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                (new DbContextSeedData()).Seed();
            }

            #region NLog
            //add NLog to ASP.NET Core
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug(LogLevel.Error);

            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog("NLog.config");
            #endregion

            app.UseMvc();
            app.UseCors("AllowSpecificOrigin");

        }
    }
}
