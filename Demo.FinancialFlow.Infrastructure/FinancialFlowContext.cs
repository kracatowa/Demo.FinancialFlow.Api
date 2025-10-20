using Microsoft.EntityFrameworkCore;

namespace Demo.FinancialFlow.Infrastructure
{
    public class FinancialFlowContext(DbContextOptions<FinancialFlowContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("demo");
        }

    }
}
