using Demo.FinancialFlow.Api.Services;
using Demo.FinancialFlow.Api.Services.File;
using Demo.FinancialFlow.Domain.FileAggregate;
using Demo.FinancialFlow.Domain.FinancialFlowAggregate;
using Demo.FinancialFlow.Infrastructure;
using Demo.FinancialFlow.Infrastructure.Repositories;
using Demo.FinancialFlow.Infrastructure.Repositories.Azure;
using Demo.FinancialFlow.Infrastructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;

namespace Demo.FinancialFlow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddDbContext<FinancialFlowContext>(options =>
            {
                var currentLogLevel = builder.Configuration.GetValue<LogLevel>("Logging:LogLevel:Microsoft.EntityFrameworkCore");

                options.UseSqlServer(builder.Configuration.GetConnectionString("Sql"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                })
                .LogTo(Console.WriteLine, currentLogLevel);

                if (currentLogLevel == LogLevel.Debug)
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            builder.Services.Configure<AzureStorageSettings>(
                builder.Configuration.GetSection("AzureStorage"));

            builder.Services.AddHostedService<MigrationHostedService<FinancialFlowContext>>();

            builder.Services.AddScoped<IFileProcessor, FileProcessingService>();
            builder.Services.AddScoped<FileProcessorFactory>();
            builder.Services.AddScoped<ProcessCsvFinancialFlow>();
            builder.Services.AddScoped<IFinancialFlowRepository, SqlFinancialFlowRepository>();
            builder.Services.AddScoped<IFinancialFlowFileAuditRepository, SqlFinancialFlowFileAuditRepository>();

            builder.Services.AddScoped<IStorageService, AzureStorageService>();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<Program>();
            });

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseCors("AllowAll");
            }

            app.MapControllers();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.Run();
        }
    }
}
