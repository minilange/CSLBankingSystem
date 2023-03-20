﻿using CSLBankingSystemBackend.Classes;

namespace CSLBankingSystem.Classes
{
    public class Bank
    {

        private static Bank bankInstance = null;

        private Dictionary<int, Account> accounts = new Dictionary<int, Account>();
        private Dictionary<int, Customer> customers = new Dictionary<int, Customer>();


        private Bank()
        {
            // Read from DB and create all customers
            accounts = DbHandler.GetAllAccounts();

            // Read from DB and instantiate all accounts
            customers = DbHandler.GetAllCustomers();
        }

        // Make the Bank class a singleton-class
        // To ensure everything is handled by the same instance
        public static Bank GetInstance()
        {
            if (bankInstance == null)
            {
                bankInstance = new Bank();
            }

            return bankInstance;
        }

        public int CreateBankAccount()
        {
            Account acc = new Account();

            accounts.Add(acc.id, acc);

            return acc.id;
        }

        public void CreateNewCustomer(string firstName, string lastName, string email, int age, string socialNum, string phoneNum, string address, int ZipCode)
        {
            Customer newCustomer = new Customer(firstName, lastName, email, age, socialNum, phoneNum, address, ZipCode);

            customers.Add(newCustomer.customerId, newCustomer);
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
