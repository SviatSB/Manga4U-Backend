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

namespace WEBAPI
{
    public class Program
    {
        private const string FrontendCorsPolicy = "AllowFrontend";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // ===== Services =====
            builder.Services
                .AddAppControllersAndSwagger()
                .AddDatabaseAndAzureStorage(config)
                .AddIdentityAndJwt(config)
                .AddFrontendCors(FrontendCorsPolicy)
                .AddCache(config);

            builder.Services.AddMyServices();
            builder.Services.AddHostedService<DbSeederHostedService>();

            // ===== App pipeline =====
            var app = builder.Build();

            app.UseDeveloperFeatures();
            app.UseAppRequestPipeline(FrontendCorsPolicy);

            app.Run();
        }
    }
}
