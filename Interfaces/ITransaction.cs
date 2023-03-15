namespace CSLBankingSystem.Interfaces
{
    public interface ITransaction
    {
        string transactor { get; set; }
        string fromAccountId { get; set; }
        string toAccountId { get; set; }
        Int32 amount { get; set; }

    }
}
