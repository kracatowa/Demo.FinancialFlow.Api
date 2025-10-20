using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.FinancialFlow.Infrastructure.Configurations
{
    public class FinancialFlowConfiguration : IEntityTypeConfiguration<Domain.FinancialFlow>
    {
        public void Configure(EntityTypeBuilder<Domain.FinancialFlow> builder)
        {
            builder.Property(o => o.Id)
                .UseHiLo("financialflowseq");

            builder.Property(o=> o.CreationDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(i => i.FlowType)
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<Domain.FlowType>(v)
                );

            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.TransactionDate).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Subsidiairy).HasMaxLength(100);
        }
    }
}
