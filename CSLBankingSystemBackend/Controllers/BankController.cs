using CSLBankingSystem.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace CSLBankingSystemBackend.Controllers
{

    public class HttpCustomer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int age { get; set; }
        public string socialNum { get; set; }
        public string phoneNum { get; set; }
        public string address { get; set; }
        public int zipCode { get; set; }

    }

    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ILogger<BankController> _logger;

        public BankController(ILogger<BankController> Logger)
        {
            _logger = Logger;
        }

        [HttpPost]
        public void Post([FromBody] HttpCustomer customer)
        {
            Bank bank = Bank.GetInstance();

            HttpCustomer c = customer;

            bank.CreateNewCustomer(c.firstName, c.lastName, c.email, c.age, c.socialNum, c.phoneNum, c.address, c.zipCode);

            return;
        }
    }
}
