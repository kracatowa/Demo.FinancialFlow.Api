using Demo.FinancialFlow.Domain.FileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Demo.FinancialFlow.Infrastructure.Configurations
{
    public class FinancialFlowFileAuditConfiguration : IEntityTypeConfiguration<FinancialFlowFileAudit>
    {
        private readonly static JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public void Configure(EntityTypeBuilder<FinancialFlowFileAudit> builder)
        {
            builder.ToTable("FinancialFlowFileAudits");

            builder.HasKey(x => x.Id);
            builder.Property(o => o.Id)
                .UseHiLo("financialfileseq");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Filename)
                .IsRequired()
                .HasMaxLength(260);

            builder.Property(x => x.StorageFileId)
                .IsRequired();

#pragma warning disable CS8603 // Possible null reference return.
            builder.Property(x => x.FinancialFlowFileAudits)
                .HasConversion(
                    v => JsonSerializer.Serialize(v ?? new List<FinancialFlowFileAuditStatus>(), JsonSerializerOptions),
                    v => string.IsNullOrEmpty(v)
                        ? new List<FinancialFlowFileAuditStatus>()
                        : JsonSerializer.Deserialize<List<FinancialFlowFileAuditStatus>>(v, JsonSerializerOptions) )
                .HasColumnType("nvarchar(max)")
                .IsRequired();
#pragma warning restore CS8603 // Possible null reference return.

            builder.Property(x => x.CreationDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
