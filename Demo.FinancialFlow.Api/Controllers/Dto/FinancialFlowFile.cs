namespace Demo.FinancialFlow.Api.Controllers.Dto
{
    /// <summary>
    /// This class is a simple DTO for the file upload in the FinancialFlowController
    /// 
    /// It circumvent a bug that occurs with swagger UI where IFormFile route argument cannot be rendered
    /// </summary>
    public class FinancialFlowFile
    {
        public required IFormFile File { get; set; }
    }
}
