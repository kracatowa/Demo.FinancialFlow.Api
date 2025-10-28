using Demo.FinancialFlow.Domain.FinancialFlowAggregate.FileDto;
using System.Globalization;
using System.Text;

namespace Demo.FinancialFlow.Api.Services.File.Processors
{
    public class ProcessCsvFinancialFlow(ILogger<ProcessCsvFinancialFlow> logger) : IFileProcessor
    {
        public List<Domain.FinancialFlowAggregate.FinancialFlow> ProcessFinancialFlow(MemoryStream stream, Guid fileStorageId)
        {
            var result = new List<Domain.FinancialFlowAggregate.FinancialFlow>();
            stream.Position = 0;

            // Read the entire stream as a Base64 string
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            string base64Content = reader.ReadToEnd();

            // Decode the Base64 string to get the CSV content
            byte[] csvBytes;
            try
            {
                csvBytes = Convert.FromBase64String(base64Content);
            }
            catch (FormatException)
            {
                logger.LogError("Input stream is not valid Base64.");
                return result;
            }

            // Create a new stream for the decoded CSV content
            using var csvStream = new MemoryStream(csvBytes);
            using var csvReader = new StreamReader(csvStream, Encoding.UTF8);

            var header = csvReader.ReadLine();
            if (header == null)
                return result;

            // Check for valid separator
            bool hasComma = header.Contains(',');
            bool hasSemicolon = header.Contains(';');
            if (!hasComma && !hasSemicolon)
                throw new FormatException("CSV separator must be either ',' or ';'.");

            char separator = hasSemicolon && !hasComma ? ';' : ',';

            while (!csvReader.EndOfStream)
            {
                var line = csvReader.ReadLine();
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
                    result.Add(dto.ToDomainModel(fileStorageId));
                }
            }

            return result;
        }
    }
}
