﻿using System.Text.Json;
using CSLBankingSystemBackend.Classes;

namespace CSLBankingSystem.Classes
{
    public class Bank
    {

        private static Bank bankInstance = null;
        private static int atmId = 1;

        private Dictionary<int, Account> accounts = new Dictionary<int, Account>();
        private Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        private List<Token> tokens = new List<Token>();


        // Private constructor due to class being a singletong-class
        private Bank()
        {
            // Read from DB and create all customers
            accounts = DbHandler.GetAllAccounts();

            // Read from DB and instantiate all accounts
            customers = DbHandler.GetAllCustomers();

            // Read from DB and retreve all active tokens
            tokens = DbHandler.GetAllActiveTokens();
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

        public Token? IsTokenActive(string value)
        {
            Token token = tokens.Find(x => x.value == value);

            return token;
        }

        public Customer? GetCustomerFromToken(Token token)
        {
            try
            {
                Customer customer = customers[token.customerId];
                return customer;
            }
            catch (Exception ex)
            {

                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

                return null;
            }
        }

        public Customer? GetCustomerFromId(int id)
        {
            try
            {
                return customers[id];

            }
            catch (Exception ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

                return null;
            }
        }

        public List<Account> GetAccountsFromCustomerId(int customerId)
        {
            try
            {
                List<Account> customerAccounts = new List<Account>();

                foreach (int id in customers[customerId].accountIds)
                {
                    customerAccounts.Add(accounts[id]);
                }

                return customerAccounts;
            }
            catch (Exception ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

                return new List<Account>();
            }
        }

        public int CreateBankAccount()
        {
            Account acc = new Account();

            accounts.Add(acc.id, acc);

            return acc.id;
        }

        public string SerializeAllCustomers()
        {
            string json = JsonSerializer.Serialize(customers);

            return json;
        }

        public void CreateNewCustomer(string firstName, string lastName, string email, string password, int age, string socialNum, string phoneNum, string address, int ZipCode)
        {
            int accId = CreateBankAccount();

            Customer newCustomer = new Customer(firstName, lastName, email, password, age, socialNum, phoneNum, address, ZipCode, accId);

            customers.Add(newCustomer.customerId, newCustomer);

            DbHandler.InsertAccountBinder(accId, newCustomer.customerId);
        }

        public int MakeDeposit(int customerId, int accountId, float amount)
        {
            try
            {
                this.MakeTransaction(customerId, atmId, accountId, amount);
                
                return 0;
            }
            catch (Exception ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");
                
                return 1;
            }
        }

        public int MakeWithdrawal(int customerId, int accountId, float amount)
        {
            try
            {
                this.MakeTransaction(customerId, accountId, atmId, amount);
                
                return 0;
            }
            catch (Exception ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");
                
                return 1;
            }
        }

        public int MakeTransaction(int transactorId, int fromAccountId, int toAccountId, float amount)
        {

            Account fromAccount = this.accounts[fromAccountId];
            Account toAccount = this.accounts[toAccountId];

            // Checks if the deducted balance is allowed
            try
            {
                if (fromAccount.name != "ATM")
                {
                    fromAccount.DeductFromBalance(amount);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

                return 1;
            }

            // Checks if the added balance is allowed
            // If added balance is not allowed, add balance back to 'fromAccount'
            try
            {
                if (toAccount.name != "ATM")
                {
                    toAccount.AddToBalance(amount);
                }
            }
            catch (OverflowException ex)
            {
                fromAccount.AddToBalance(amount);
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");
                return 1;
            }

            Transaction transaction = new Transaction(
                transactorId,
                fromAccountId,
                toAccountId,
                amount,
                DateTime.Now
                );

            fromAccount.AddTransaction(transaction);

            return 0;
        }
    }
}
