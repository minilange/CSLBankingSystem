using CSLBankingSystemBackend.Classes;

namespace CSLBankingSystem.Classes
{
    public class Bank
    {

        private Dictionary<int, Account> accounts = new Dictionary<int, Account>();
        private Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        

        public Bank()
        {
            // Read from DB and create all customers
            accounts = DbHandler.GetAllAccounts();

            // Read from DB and instantiate all accounts
            customers = DbHandler.GetAllCustomers();
        }

        public static int CreateBankAccount()
        {
            Account acc = new Account();

            return 1;
        }

        public void MakeTransaction(int transactorId, int fromAccountId, int toAccountID, float amount)
        {

            Account fromAccount = this.accounts[fromAccountId];
            Account toAccount = this.accounts[toAccountID];

            // Checks if the deducted balance is allowed
            try
            {
                fromAccount.DeductFromBalance(amount);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }

            // Checks if the added balance is allowed
            // If added balance is not allowed, add balance back to 'fromAccount'
            try
            {
                toAccount.AddToBalance(amount);
            }
            catch (OverflowException ex)
            {
                fromAccount.AddToBalance(amount);
                Console.Error.WriteLine(ex.Message);
                return;
            }

            Transaction transaction = new Transaction(
                transactorId,
                fromAccountId,
                toAccountID,
                amount,
                DateTime.Now
                );

            fromAccount.AddTransaction(transaction);

        }
    }
}
