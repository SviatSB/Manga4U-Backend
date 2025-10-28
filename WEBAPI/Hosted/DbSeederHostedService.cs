using DATAINFRASTRUCTURE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WEBAPI.Hosted
{
    public class DbSeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        public DbSeederHostedService(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var ownerLogin = _config["Seed:OwnerLogin"];
            var ownerPassword = _config["Seed:OwnerPassword"];
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            var strategy = db.Database.CreateExecutionStrategy();
            try
            {
                await strategy.ExecuteAsync(async () =>
                {
                    await SeedData.InitializeAsync(scope.ServiceProvider, ownerLogin!, ownerPassword!);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seed] Skipped after retries due to transient error: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
