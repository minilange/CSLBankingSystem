using CSLBankingSystem.Classes;
using CSLBankingSystem.Interfaces;
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;

namespace CSLBankingSystemBackend.Classes
{
    public class DbHandler
    {

        private static string connString = SQLConnectionString.connString;

        private static SqlConnection ConnectToDB()
        {
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                return conn;
            }
            catch (Exception ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");
                return null;
            }

        }

        public static List<Token> GetAllActiveTokens()
        {
            string query = $"SELECT [Value], [CustomerId], [Timestamp] FROM Tokens WHERE [Timestamp] WHERE BETWEEN DATEADD(DAY, -1, [Timestamp]) AND [Timestamp]";
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);

            List<Token> tokens = new List<Token>();

            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Token token = new Token(
                            Convert.ToString(reader["value"]),
                            Convert.ToInt32(reader["customerId"]),
                            Convert.ToString(reader["timestamp"])
                            );

                        tokens.Add(token);
                    }
                }

                return tokens;
            }
            catch (Exception ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

                return new List<Token>();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        #region Accounts
        public static void UpdateBalanceFromTransaction(Transaction transaction)
        {
            string query = $"UPDATE Accounts SET balance = balance - {transaction.amount} WHERE id = {transaction.fromAccountId}; UPDATE Accounts SET balance = balance + {transaction.amount} WHERE id = {transaction.toAccountId}";
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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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
            string query = $"INSERT INTO Accounts ([Name], [Description], [Balance]) VALUES ('{account.name}', '{account.description}', {account.balance}); SELECT @@IDENTITY as AccountId";
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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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

        public static void InsertAccountBinder(int accountId, int customerId)
        {
            string query = $"INSERT INTO AccountBinders ([AccountId], [CustomerId]) VALUES ({accountId}, {customerId})";
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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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


        #region Customers
        public static int CheckPasswordToEmail(string email, string password)
        {
            string query = $"SELECT [CustomerId] FROM Customers WHERE Email = '{email}' and Password = '{password}'";
            SqlConnection conn = ConnectToDB();
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                int tempCounter = 0;
                int customerId = -1;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerId = Convert.ToInt32(reader["CustomerId"]);
                        tempCounter++;
                    }
                }

                if (tempCounter == 1)
                {
                    return customerId;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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

        public static List<int> GetAllCustomerAccountIds(int customerId)
        {
            string query = $"SELECT [AccountId] FROM AccountBinder WHERE CustomerId = {customerId}";
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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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
            string query = $"INSERT INTO Customers ([FirstName], [Lastname], [Email], [Password], [Age], [SocialNum], [PhoneNum], [Address], [ZipCode]) VALUES ('{customer.firstName}', '{customer.lastName}', '{customer.email}', '{customer.password}', {customer.age}, '{customer.socialNum}', '{customer.phoneNum}', '{customer.address}', {customer.zipCode}); SELECT @@IDENTITY as CustomerId";
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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

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



        private static string GetSHA256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();

                foreach (Byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
