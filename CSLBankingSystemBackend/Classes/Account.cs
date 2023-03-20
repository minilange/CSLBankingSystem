using CSLBankingSystem.Interfaces;
using CSLBankingSystemBackend.Classes;
using CSLBankingSystemBackend.Interfaces;

namespace CSLBankingSystem.Classes
{
    public class Account : IAccount
    {

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float balance { get; set; }
        public List<Transaction> transactions { get; set; }


        public Account(int id, string name, string description, float balance)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.balance = balance;

            transactions = DbHandler.GetAllTransactions(this.id);
        }

        public Account()
        {
            this.name = "This is a new bank account";
            this.description = "";
            this.balance = 0f;

            this.SaveNewAccount();
        }

        private int SaveNewAccount()
        {

            int id = DbHandler.InsertAccount(this);

            this.id = id;

            return this.id;
        }

        public void DeductFromBalance(float amount)
        {
            
            if (this.balance - amount > 0)
            {
                this.balance -= amount;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

        }

        public void AddToBalance(float amount)
        {
            try
            {
                this.balance += amount;
            }
            catch (OverflowException ex)
            {
                throw ex;
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            DbHandler.AddTransaction(transaction);
            transactions.Add(transaction);
        }

        //public void MakeTransaction(int transactor, int toAccountId, float amount)
        //{

        //    Transaction trans = new Transaction(transactor, this.id, toAccountId, amount, DateTime.Now);

        //    transactions.Add(trans);



        //} 
    }
}
