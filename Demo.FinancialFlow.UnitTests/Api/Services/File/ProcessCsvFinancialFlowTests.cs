using Demo.FinancialFlow.Api.Services.File;
using Demo.FinancialFlow.Domain.FinancialFlowAggregate;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace Demo.FinancialFlow.UnitTests.Api.Services.File
{
    public class ProcessCsvFinancialFlowTests
    {
        private ProcessCsvFinancialFlow CreateProcessor(Mock<ILogger<ProcessCsvFinancialFlow>>? loggerMock = null)
        {
            return new ProcessCsvFinancialFlow(loggerMock?.Object ?? new Mock<ILogger<ProcessCsvFinancialFlow>>().Object);
        }

        [Fact]
        public void ProcessFinancialFlow_CommaSeparator_ParsesValidRows()
        {
            var csv = "Amount,TransactionDate,Description,FlowType,Subsidiairy\n" +
                      "100.5,2025-10-20,Test,Past,SubA\n" +
                      "200.0,2025-10-21,Test2,Future,SubB\n";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            var processor = CreateProcessor();

            var result = processor.ProcessFinancialFlow(stream, ".csv");

            Assert.Equal(2, result.Count);
            Assert.Equal(100.5f, result[0].Amount);
            Assert.Equal(FlowType.Past, result[0].FlowType);
            Assert.Equal("SubA", result[0].Subsidiairy);
        }

        [Fact]
        public void ProcessFinancialFlow_SemicolonSeparator_ParsesValidRows()
        {
            var csv = "Amount;TransactionDate;Description;FlowType;Subsidiairy\n" +
                      "150.0;2025-10-22;Test3;Future;SubC\n";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            var processor = CreateProcessor();

            var result = processor.ProcessFinancialFlow(stream, ".csv");

            Assert.Single(result);
            Assert.Equal(150.0f, result[0].Amount);
            Assert.Equal(FlowType.Future, result[0].FlowType);
            Assert.Equal("SubC", result[0].Subsidiairy);
        }

        [Fact]
        public void ProcessFinancialFlow_InvalidSeparator_ThrowsFormatException()
        {
            var csv = "Amount|TransactionDate|Description|FlowType|Subsidiairy\n" +
                      "100.0|2025-10-23|Test4|Past|SubD\n";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            var processor = CreateProcessor();

            Assert.Throws<FormatException>(() => processor.ProcessFinancialFlow(stream, ".csv"));
        }

        [Fact]
        public void ProcessFinancialFlow_InvalidRow_IsSkippedAndLogged()
        {
            var csv = "Amount,TransactionDate,Description,FlowType,Subsidiairy\n" +
                      "-10,2025-10-20,Test,Past,SubA\n" + // Invalid: negative amount
                      "100.0,2025-10-21,,Future,SubB\n" + // Invalid: empty description
                      "200.0,2025-10-22,Test2,Future,SubC\n"; // Valid
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            var loggerMock = new Mock<ILogger<ProcessCsvFinancialFlow>>();
            var processor = CreateProcessor(loggerMock);

            var result = processor.ProcessFinancialFlow(stream, ".csv");

            Assert.Single(result);
            Assert.Equal(200.0f, result[0].Amount);
            Assert.Equal("Test2", result[0].Description);

            // Verify logger was called for invalid rows
            loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Invalid CSV line")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}
