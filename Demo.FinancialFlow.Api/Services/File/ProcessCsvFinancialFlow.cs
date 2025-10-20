using Demo.FinancialFlow.Domain.FinancialFlowAggregate.FileDto;
using System.Globalization;
using System.Text;

namespace Demo.FinancialFlow.Api.Services.File
{
    public class ProcessCsvFinancialFlow(ILogger<ProcessCsvFinancialFlow> logger) : IFileProcessor
    {
        public List<Domain.FinancialFlowAggregate.FinancialFlow> ProcessFinancialFlow(MemoryStream stream, string extension)
        {
            var result = new List<Domain.FinancialFlowAggregate.FinancialFlow>();
            stream.Position = 0;

            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);

            var header = reader.ReadLine();
            if (header == null)
                return result;

            // Check for valid separator
            bool hasComma = header.Contains(',');
            bool hasSemicolon = header.Contains(';');
            if (!hasComma && !hasSemicolon)
                throw new FormatException("CSV separator must be either ',' or ';'.");

            char separator = hasSemicolon && !hasComma ? ';' : ',';

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var columns = line.Split(separator);

                if (columns.Length < 5)
                    continue;

                var dto = new CsvFinancialFlow
                {
                    Amount = float.Parse(columns[0], CultureInfo.InvariantCulture),
                    TransactionDate = DateTime.Parse(columns[1], CultureInfo.InvariantCulture),
                    Description = columns[2],
                    FlowType = columns[3],
                    Subsidiairy = columns[4]
                };

                var errors = dto.Validate().ToList();
                if (errors.Count != 0)
                {
                    logger.LogWarning("Invalid CSV line: {Line}. Errors: {Errors}", line, string.Join("; ", errors));
                }
                else
                {
                    result.Add(dto.ToDomainModel());
                }
            }

            return result;
        }
    }
}
