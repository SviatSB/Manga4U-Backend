using DataInfrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using SharedConfiguration.Options;

namespace WebApi.Hosted
{
    public class DbSeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppSeedOptions _seedOptions;
        public DbSeederHostedService(IServiceProvider serviceProvider, IOptions<AppSeedOptions> seedOptions)
        {
            _serviceProvider = serviceProvider;
            _seedOptions = seedOptions.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var ownerLogin = _seedOptions.OwnerLogin;
            var ownerPassword = _seedOptions.OwnerPassword;
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
