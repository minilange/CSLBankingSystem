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
        [Route("api/Login/")]
        public string LoginGet([FromBody] string? token, string email, string password)
        {

            if  (token != null)
            {
                Bank bank = Bank.GetInstance();
                bank.IsTokenActive(token);

            }


            int id = DbHandler.CheckPasswordToEmail(email, password);

            if (id != -1)
            {
                Bank bank = Bank.GetInstance();

                Customer customer = bank.GetCustomerFromId(id);

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

        [HttpPost]
        [Route("api/signup")]
        public string SignUpPost()
        {
            return "";
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
