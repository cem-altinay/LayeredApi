using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sample.Api.Filters;
using Sample.Api.Helpers;
using Sample.Api.Security;
using Sample.Data.Access.DAL;
using Sample.Security;
using Sample.Security.Auth;
using Sample.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sample.Api.IoC
{
    public class ContainerSetup
    {
        public static void Setup(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureCors(services);
            AddUow(services, configuration);
            AddQueries(services);
            ConfigureAuth(services);
            JwtConfigure(services);
        }

        #region Auth And Jwt
        private static void ConfigureAuth(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITokenBuilder, TokenBuilder>();
            services.AddScoped<ISecurityContext, SecurityContext>();
        }

        private static void JwtConfigure(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (o) =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = TokenAuthOption.Key,
                    ValidAudience = TokenAuthOption.Audience,
                    ValidIssuer = TokenAuthOption.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });


            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }
        #endregion

        #region AutoMapper
        //private static void ConfigureAutoMapper(IServiceCollection services)
        //{
        //    var mapperConfig = AutoMapperConfigurator.Configure();
        //    var mapper = mapperConfig.CreateMapper();
        //    services.AddSingleton(x => mapper);
        //    services.AddTransient<IAutoMapper, AutoMapperAdapter>();
        //}
        #endregion

        #region Database
        /// <summary>
        /// Database Access 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddUow(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];

            services.AddEntityFrameworkSqlServer();

            #region Code First
            // Migration Context Save
            //services.AddDbContext<MainDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            // Migration Context
            //services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<MainDbContext>()));
            #endregion

            #region Db First
            // DbFirst Context Save
            services.AddDbContext<Sample.Data.Models.MainDbContext>(options =>
               options.UseSqlServer(connectionString));

            // DbFirstContext
            services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<Sample.Data.Models.MainDbContext>()));
            #endregion

            services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();
            services.AddScoped<UnitOfWorkFilterAttribute>();
        }
        #endregion

        #region Services
        private static void AddQueries(IServiceCollection services)
        {
            var exampleProcessorType = typeof(UserService);
            var types = (from t in exampleProcessorType.GetTypeInfo().Assembly.GetTypes()
                         where t.Namespace == exampleProcessorType.Namespace
                               && t.GetTypeInfo().IsClass
                               && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                         select t).ToArray();

            foreach (var type in types)
            {
                var interfaceQ = type.GetTypeInfo().GetInterfaces().First();
                services.AddScoped(interfaceQ, type);
            }
        }
        #endregion

        #region Cors
        private static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
        #endregion
    }
}
