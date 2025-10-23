using Demo.FinancialFlow.Domain.Seedwork;

namespace Demo.FinancialFlow.Domain.FileAggregate
{
    public class FinancialFlowFileAudit : Entity
    {
        public string UserId { get; private set; }
        public string Filename { get; private set; } 
        public Guid StorageFileId { get; private set; }
        public List<FinancialFlowFileAuditStatus> FinancialFlowFileAudits { get; private set; }

        protected FinancialFlowFileAudit() { }

        public FinancialFlowFileAudit(string userId, string filename, Guid storageFileId)
        {
            UserId = userId;
            Filename = filename;
            StorageFileId = storageFileId;
            FinancialFlowFileAudits = [FinancialFlowFileAuditStatus.Start];
        }

        public void MarkAsFailed()
        {
            FinancialFlowFileAudits.Add(FinancialFlowFileAuditStatus.Failed);
        }
    }
}
