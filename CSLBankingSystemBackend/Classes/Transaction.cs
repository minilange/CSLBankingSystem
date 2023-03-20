using CSLBankingSystem.Interfaces;

namespace CSLBankingSystemBackend.Classes
{
    public class Transaction : ITransaction
    {
        public int transactorId { get; set; }
        public int fromAccountId { get; set; }
        public int toAccountId { get; set; }
        public float amount { get; set; }
        public DateTime timestamp { get; set; }


        public Transaction(int transactorId, int fromAccountId, int toAccountId, float amount, DateTime tiemstamp)
        {
            this.transactorId = transactorId;
            this.fromAccountId = fromAccountId;
            this.toAccountId = toAccountId;
            this.amount = amount;
            this.timestamp = tiemstamp;
        }
    }
}
