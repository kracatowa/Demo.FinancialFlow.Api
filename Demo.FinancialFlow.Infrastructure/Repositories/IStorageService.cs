namespace Demo.FinancialFlow.Infrastructure.Repositories
{
    public interface IStorageService
    {
        public void UploadFile(string fileName, Stream fileStream);
    }
}
