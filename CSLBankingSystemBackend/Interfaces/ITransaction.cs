namespace CSLBankingSystem.Interfaces
{
    public interface ITransaction
    {
        int transactorId { get; set; }
        int fromAccountId { get; set; }
        int toAccountId { get; set; }
        float amount { get; set; }
        DateTime timestamp { get; set; }

    }
}
