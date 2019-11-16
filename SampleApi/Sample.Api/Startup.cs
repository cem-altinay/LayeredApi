using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Sample.Api.Filters;
using Sample.Api.Helpers;
using Sample.Api.IoC;
using Sample.Data.Access.DAL;
using Sample.Security.Auth;
using System;
namespace Sample.Api
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
            services.AddControllers();

            //IoC Setup
            ContainerSetup.Setup(services, Configuration);

            services.AddMvc(options => { options.Filters.Add(new ApiExceptionFilter()); options.RespectBrowserAcceptHeader = true; })
                                      .AddJsonOptions(o =>
                                      {
                                          // Newtonsoft JSON
                                          // o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                                          o.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                                      });


            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
                //etc
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            //InitDatabase(app);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Code First Init Data
        /// </summary>
        /// <param name="app"></param>
        private void InitDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MainDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
