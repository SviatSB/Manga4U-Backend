using Azure.Storage.Blobs;
using DATAINFRASTRUCTURE;
using ENTITIES;
using ENTITIES.Interfaces;
using ENTITIES.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SERVICES;
using System.Security.Claims;
using System.Text;
using ENTITIES.Options;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Extensions;
using WEBAPI.Hosted;
using SharedConfiguration;
using Microsoft.Identity.Client;
using SharedConfiguration.Options;

namespace WEBAPI
{
    public class Program
    {
        private const string FrontendCorsPolicy = "AllowFrontend";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            #region Configuration

            var config = ConfigurationFactory.BuildConfiguration();

            // Register option bindings into DI so IOptions<T> can be used across the app
            services.AddAppConfiguration(config);

            var azureStorageOptions = config.GetSection("AzureStorageOptions").Get<AppAzureStorageOptions>()!;
            //var cacheOptions = config.GetSection("CacheOptions").Get<AppCacheOptions>()!;
            var dataBaseOptions = config.GetSection("DataBaseOptions").Get<AppDataBaseOptions>()!;
            var identityOptions = config.GetSection("IdentityOptions").Get<AppIdentityOptions>()!;
            var jwtOptions = config.GetSection("JwtOptions").Get<AppJwtOptions>()!;
            var proxyOptions = config.GetSection("ProxyOptions").Get<AppProxyOptions>()!;
            //var seedOptions = config.GetSection("SeedOptions").Get<AppSeedOptions>()!;

            #endregion

            #region Building

            services
                .AddCache()
                .AddAppAzureStorage(azureStorageOptions)
                .AddAppDatabase(dataBaseOptions)
                .AddRepository()
                .AddMyServices(proxyOptions)
                .AddAppIdentity(identityOptions)
                .AddAppJwt(jwtOptions)
                .AddFrontendCors(FrontendCorsPolicy)
                .AddAppControllersAndSwagger();

            services.AddHostedService<DbSeederHostedService>();

            #endregion

            #region MiddleWare

            var app = builder.Build();

            app.UseDeveloperFeatures();
            app.UseAppRequestPipeline(FrontendCorsPolicy);

            app.Run();

            #endregion
        }
    }
}
