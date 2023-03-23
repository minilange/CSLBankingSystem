using CSLBankingSystem.Classes;
using CSLBankingSystemBackend.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CSLBankingSystemBackend.Controllers
{

    public class HttpCustomer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int age { get; set; }
        public string socialNum { get; set; }
        public string phoneNum { get; set; }
        public string address { get; set; }
        public int zipCode { get; set; }
    }

    public class HttpAtmAction
    {
        public int customerId { get; set; }
        public int accountId { get; set; }
        public float amount { get; set; }
    }

    public class HttpTransaction
    {
        public int CustomerId { get; set; }
        public int fromAccountId { get; set; }
        public int toAccountId { get; set; }
        public float amount { get; set; }

    }

    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly ILogger<BankController> _logger;

        public BankController(ILogger<BankController> Logger)
        {
            _logger = Logger;
        }

        /// <summary>
        /// 
        ///     --- Int status codes ---
        ///     
        ///         0 = Success
        ///         1 = Exception thrown
        ///         2 = Invalid token
        /// 
        /// 
        /// 
        /// 
        ///     Login
        ///     Sign up
        /// 
        ///     GetCustomerAccounts
        /// 
        ///     Deposit
        ///     Withdrawl
        ///     
        ///     Transaction
        /// 
        /// 
        /// </summary>


        [HttpGet]
        [Route("api/Login")]
        public string LoginGet([FromBody] string? tokenHash, string email, string password)
        {
            // Check a token value is present
            if (tokenHash != null)
            {
                Bank bank = Bank.GetInstance();

                Token token = bank.IsTokenActive(tokenHash);

                // Checks if token value is in an active token
                if (token != null)
                {
                    Customer customer = bank.GetCustomerFromToken(token);

                    if (customer != null)
                    {

                        List<Object> loginList = new List<Object>() { customer, token };

                        string json = JsonSerializer.Serialize(loginList);

                        return json;
                    }
                }
            }


            int id = DbHandler.CheckPasswordToEmail(email, password);

            if (id != -1)
            {
                Bank bank = Bank.GetInstance();

                Customer customer = bank.GetCustomerFromId(id);

                if (customer != null)
                {
                    string shaToken = GetSHA256Hash($"{customer.customerId} - {DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss")}");

                    List<Object> loginList = new List<Object>() { customer, shaToken };


                    string json = JsonSerializer.Serialize(loginList);

                    return json;
                }
                else
                {
                    return "";
                }

            }
            else
            {
                return "";
            }
        }

        [HttpPost]
        [Route("api/Signup")]
        public int SignUpPost([FromBody] string tokenHash, HttpCustomer customer)
        {
            Bank bank = Bank.GetInstance();

            HttpCustomer c = customer;

            if (!IsTokenValid(tokenHash))
            {
                return 2;
            }

            try
            {
                bank.CreateNewCustomer(c.firstName, c.lastName, c.email, c.password, c.age, c.socialNum, c.phoneNum, c.address, c.zipCode);

                return 0;
            }
            catch (Exception ex)
            {
                string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.Error.WriteLine($"{funcName} - {ex}");

                return 1;
            }
        }

        [HttpGet]
        [Route("api/CustomerAccounts")]
        public string GetCustomerAccounts([FromBody] string tokenHash, int customerId)
        {
            Bank bank = Bank.GetInstance();

            if (!IsTokenValid(tokenHash))
            {
                return "Invalid token";
            }


            List<Account> accounts = bank.GetAccountsFromCustomerId(customerId);

            string json = JsonSerializer.Serialize(accounts);

            return json;

        }

        [HttpPatch]
        [Route("api/Deposit")]
        public int Deposit([FromBody] string tokenHash, HttpAtmAction action)
        {
            Bank bank = Bank.GetInstance();

            if (!IsTokenValid(tokenHash))
            {
                return 2;
            }

            int success = bank.MakeDeposit(action.accountId, action.customerId, action.amount);

            return success;
        }

        [HttpPatch]
        [Route("api/Withdrawal")]
        public int Withdrawal([FromBody] string tokenHash, HttpAtmAction action)
        {
            Bank bank = Bank.GetInstance();

            if (!IsTokenValid(tokenHash))
            {
                return 2;
            }

            int success = bank.MakeWithdrawal(action.accountId, action.customerId, action.amount);

            return success;
        }

        [HttpPatch]
        [Route("api/MakeTransaction")]
        public int MakeTransaction([FromBody] string tokenHash, HttpTransaction action)
        {
            Bank bank = Bank.GetInstance();

            if (!IsTokenValid(tokenHash))
            {
                return 2;
            }

            int success = bank.MakeTransaction(action.CustomerId, action.fromAccountId, action.toAccountId, action.amount);

            return success;
        }




        //[HttpPost]
        //[Route("api/[controller]")]
        //public string Post([FromBody] HttpCustomer customer)
        //{
        //    Bank bank = Bank.GetInstance();

        //    HttpCustomer c = customer;

        //    try
        //    {
        //        bank.CreateNewCustomer(c.firstName, c.lastName, c.email, c.password, c.age, c.socialNum, c.phoneNum, c.address, c.zipCode);

        //        return "New customer has been created";

        //    }
        //    catch (Exception ex)
        //    {
        //        string funcName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //        Console.Error.WriteLine($"{funcName} - {ex}");

        //        return "Something went wrong";
        //    }
        //}

        //[HttpGet]
        //[Route("api/[controller]/customer")]
        //public string Get()
        //{
        //    Bank bank = Bank.GetInstance();

        //    string serializedCustomers = bank.SerializeAllCustomers();

        //    return serializedCustomers;
        //}

        private static bool IsTokenValid(string tokenHash)
        {
            Bank bank = Bank.GetInstance();

            Token token = bank.IsTokenActive(tokenHash);

            if (token == null)
            {
                return false;
            }

            return true;
        }

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
