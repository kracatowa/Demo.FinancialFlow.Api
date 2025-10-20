using Demo.FinancialFlow.Domain.Seedwork;
using Demo.FinancialFlow.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Demo.FinancialFlow.Infrastructure
{
    /// <summary>
    /// For migrations, use this command :  dotnet ef migrations add {MigrationName} --project Demo.FinancialFlow.Infrastructure --startup-project Demo.FinancialFlow.Api --context FinancialFlowContext
    /// </summary>
    /// <param name="options"></param>
    public class FinancialFlowContext(DbContextOptions<FinancialFlowContext> dbContextOptions) : DbContext(dbContextOptions), IUnitOfWork
    {
        public DbSet<Domain.FinancialFlowAggregate.FinancialFlow> FinancialFlows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FinancialFlowConfiguration());
            modelBuilder.HasDefaultSchema("demo");
        }

    }
}
