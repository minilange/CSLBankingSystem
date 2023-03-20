using CSLBankingSystem.Classes;
using CSLBankingSystem.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CSLBankingSystemBackend.Classes
{
    public class DbHandler
    {

        private static string connString = @"Server=tcp:banking-system-ser.database.windows.net,1433;Initial Catalog=BankingDB;Persist Security Info=False;User ID=bsdbAdmin;Password=daddyCarl!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private static SqlConnection ConnectToDB()
        {
            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }

        #region Accounts

        public static void AddTransaction(Transaction transaction)
        {
            string query = $"INSERT INTO Transactions ([TransactorId], [FromId], [ToId], [Amount], [Timestamp]) VALUES ({transaction.transactorId}, {transaction.fromAccountId}, {transaction.toAccountId}, {transaction.amount}, '{transaction.timestamp.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss")}')";
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                return;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);

                return;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static List<Transaction> GetAllTransactions(int accountId)
        {
            string query = $"SELECT [TransactorId], [FromId], [ToId], [Amount], [Timestamp] FROM Transactions WHERE FromId = {accountId}";
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);

            List<Transaction> transactions = new List<Transaction>();

            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Transaction transaction = new Transaction(
                            Convert.ToInt32(reader["TransactorId"]),
                            Convert.ToInt32(reader["FromId"]),
                            Convert.ToInt32(reader["ToId"]),
                            float.Parse(reader["Balance"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                            DateTime.Parse(reader["Timestamp"].ToString())
                            );
                        transactions.Add(transaction);

                    }
                }

                return transactions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new List<Transaction>();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }


        }

        public static int InsertAccount(Account account)
        {
            string query = $"INSERT INTO Customer ([Name], [Description], [Balance]) VALUES ('{account.name}', '{account.description}', {account.balance}) GO SELECT @@IDENTITY as AccountId";
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);

            int id = 0;

            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader["AccountId"]);
                    }
                }

                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return -1;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static Dictionary<int, Account> GetAllAccounts()
        {
            string query = "SELECT [AccountId], [Name], [Description], [Balance] FROM Accounts";
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);
            Dictionary<int, Account> accounts = new Dictionary<int, Account>();


            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Account account = new Account(
                            Convert.ToInt32(reader["AccountId"]),
                            Convert.ToString(reader["Name"]),
                            Convert.ToString(reader["Description"]),
                            float.Parse(reader["Balance"].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                        );

                        accounts.Add(Convert.ToInt32(reader["AccountId"]), account);

                    }
                }

                return accounts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new Dictionary<int, Account>();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region Customers
        public static List<int> GetAllCustomerAccountIds(int customerId)
        {
            string query = $"SELECET [AccountId] FROM AccountBinder WHERE CustomerId = {customerId}";
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);
            List<int> accountIds = new List<int>();

            try
            {

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accountIds.Add(Convert.ToInt32(reader["AccountId"]));
                    }
                }

                return accountIds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new List<int>();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static int InsertCustomer(Customer customer)
        {
            string query = $"INSERT INTO Customers ([FirstName], [Lastname], [Email], [Age], [SocialNum], [PhoneNum], [Address], [ZipCode]) VALUES ('{customer.firstName}', '{customer.lastName}', '{customer.email}', {customer.age}, '{customer.socialNum}', '{customer.phoneNum}', '{customer.address}', {customer.zipCode}); SELECT @@IDENTITY as CustomerId";
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);

            int id = 0;

            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader["CustomerId"]);
                    }
                }

                return id;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);

                return 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static Dictionary<int, Customer> GetAllCustomers()
        {
            string query = "SELECT [CustomerId], [FirstName], [LastName], [Email], [Age], [SocialNum], [PhoneNum], [Address], [ZipCode] FROM Customers";
            Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer(
                            Convert.ToInt32(reader["CustomerId"]),
                            Convert.ToString(reader["FirstName"]),
                            Convert.ToString(reader["LastName"]),
                            Convert.ToString(reader["Email"]),
                            Convert.ToInt32(reader["Age"]),
                            Convert.ToString(reader["SocialNum"]),
                            Convert.ToString(reader["PhoneNum"]),
                            Convert.ToString(reader["Address"]),
                            Convert.ToInt32(reader["ZipCode"])
                        );

                        customers.Add(Convert.ToInt32(reader["CustomerId"]), customer);

                    }
                }

                return customers;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new Dictionary<int, Customer>();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        #endregion

    }
}
