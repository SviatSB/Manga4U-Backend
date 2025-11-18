using Services.Extensions;

using SharedConfiguration;

using WebApi.Extensions;
using WebApi.Extensions.Temp;
using WebApi.Hosted;

namespace WebApi
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

            #endregion

            #region Building

            services
                .AddAppCache()
                .AddAppDataInfrastructure()
                .AddMyServices()
                .AddAppIdentity()
                .AddAppJwt()
                .AddSeeders()
                .AddFrontendCors(FrontendCorsPolicy)
                .AddFluentValidationExtensions()
                .AddAppControllersAndSwagger();

            services.AddHostedService<SeederHostedService>();

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
