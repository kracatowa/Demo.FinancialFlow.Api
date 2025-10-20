using Demo.FinancialFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Demo.FinancialFlow.Api.Services
{
    public class MigrationHostedService<T> (IServiceProvider serviceProvider, ILogger<MigrationHostedService<T>> logger) : IHostedService where T : DbContext  
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            try
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
                logger.LogInformation("Database migration completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database migration failed.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
