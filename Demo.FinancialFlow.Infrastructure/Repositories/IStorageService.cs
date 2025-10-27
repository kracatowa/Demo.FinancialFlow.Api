namespace Demo.FinancialFlow.Infrastructure.Repositories
{
    public interface IStorageService
    {
        public void UploadFile(string fileName, Stream fileStream);
        public Task<MemoryStream> Downloadfile(string fileName);
    }
}
