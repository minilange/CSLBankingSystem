using Microsoft.AspNetCore.Mvc;

namespace CSLBankingSystemBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ILogger<BankController> _logger;

        public BankController(ILogger<BankController> Logger)
        {
            _logger = Logger;
        }

        //[HttpGet(Name = "Get")]
    }
}
