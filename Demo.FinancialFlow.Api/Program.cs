using Dapr;
using Dapr.Client;
using Demo.FinancialFlow.Api.Controllers.Dto;
using Demo.FinancialFlow.Api.Controllers.Validations;
using Demo.FinancialFlow.Api.Services;
using Demo.FinancialFlow.Api.Services.File;
using Demo.FinancialFlow.Domain.FileAggregate;
using Demo.FinancialFlow.Domain.FinancialFlowAggregate;
using Demo.FinancialFlow.Infrastructure;
using Demo.FinancialFlow.Infrastructure.Repositories;
using Demo.FinancialFlow.Infrastructure.Repositories.Dapr;
using Demo.FinancialFlow.Infrastructure.Repositories.Sql;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Demo.FinancialFlow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddValidatorsFromAssemblyContaining<UploadFileRequestValidator>();

            builder.Services.AddMvc();
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

            builder.Services.AddHostedService<MigrationHostedService<FinancialFlowContext>>();

            builder.Services.AddScoped<IFileProcessor, FileProcessingService>();
            builder.Services.AddScoped<FileProcessorFactory>();
            builder.Services.AddScoped<ProcessCsvFinancialFlow>();
            builder.Services.AddScoped<IFinancialFlowRepository, SqlFinancialFlowRepository>();
            builder.Services.AddScoped<IFinancialFlowFileAuditRepository, SqlFinancialFlowFileAuditRepository>();

            builder.Services.AddSingleton(_ => new DaprClientBuilder()
                                                 .UseHttpEndpoint("http://localhost:3500")
                                                 .Build());
            builder.Services.AddScoped<IStorageService, DaprStorageService>();
            builder.Services.AddScoped<IValidator<UploadFileRequest>, UploadFileRequestValidator>();

            builder.Services.AddSingleton(sp =>
                Policy
                    .Handle<DaprException>()
                    .Or<Exception>()
                    .WaitAndRetryAsync(
                        3,
                        attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            var logger = sp.GetRequiredService<ILogger<DaprStorageService>>();
                            logger.LogWarning(exception, "Retry {RetryCount} for Dapr operation due to: {Message}", retryCount, exception.Message);
                        }
                    )
);

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
