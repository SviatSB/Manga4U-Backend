using DataInfrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Services.Interfaces;

using SharedConfiguration.Options;

namespace WebApi.Hosted
{
    public class SeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeederHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            var seeders = scope.ServiceProvider.GetServices<ISeeder>();

            //var strategy = db.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{
            foreach (var seeder in seeders)
            {
                try
                {
                    await seeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Seed] {seeder.GetType().Name} skipped due to error: {ex.Message}");
                }
            }
            //});
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
