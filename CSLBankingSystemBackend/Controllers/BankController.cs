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
        public int accountId{ get; set; }
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
        public string LoginGet([FromBody] string? value, string email, string password)
        {
            // Check a token value is present
            if (value != null)
            {
                Bank bank = Bank.GetInstance();

                Token token = bank.IsTokenActive(value);

                // Checks if token value is in an active token
                if (token != null)
                {
                    Customer customer = bank.GetCustomerFromToken(token);

                    if (customer != null)
                    {
                        string json = JsonSerializer.Serialize(customer);

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
        public int SignUpPost([FromBody] HttpCustomer customer)
        {
            Bank bank = Bank.GetInstance();

            HttpCustomer c = customer;

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
        public string GetCustomerAccounts([FromBody] int customerId)
        {
            Bank bank = Bank.GetInstance();

            List<Account> accounts = bank.GetAccountsFromCustomerId(customerId);

            string json = JsonSerializer.Serialize(accounts);

            return json;

        }

        [HttpPatch]
        [Route("api/Deposit")]
        public bool Deposit([FromBody] HttpAtmAction action)
        {
            Bank bank = Bank.GetInstance();

            bool success = bank.MakeDeposit(action.accountId, action.customerId, action.amount);

            return success;
        }

        [HttpPatch]
        [Route("api/Withdrawal")]
        public bool Withdrawal([FromBody] HttpAtmAction action)
        {
            Bank bank = Bank.GetInstance();

            bool success = bank.MakeWithdrawal(action.accountId, action.customerId, action.amount);

            return success;
        }

        [HttpPatch]
        [Route("api/MakeTransaction")]
        public bool MakeTransaction([FromBody] HttpTransaction action)
        {
            Bank bank = Bank.GetInstance();

            bool success = bank.MakeTransaction(action.CustomerId, action.fromAccountId, action.toAccountId, action.amount);

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
