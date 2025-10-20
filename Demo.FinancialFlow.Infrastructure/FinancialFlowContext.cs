using Microsoft.EntityFrameworkCore;

namespace Demo.FinancialFlow.Infrastructure
{
    /// <summary>
    /// For migrations, use this command :  dotnet ef migrations add {MigrationName} --project Demo.FinancialFlow.Infrastructure --startup-project Demo.FinancialFlow.Api --context FinancialFlowContext
    /// </summary>
    /// <param name="options"></param>
    public class FinancialFlowContext(DbContextOptions<FinancialFlowContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("demo");
        }

    }
}
