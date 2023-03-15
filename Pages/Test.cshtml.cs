using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CSLBankingSystem.Pages
{
    public class TestModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        public string Message { get; private set; } = "This is a C# variable";

        public TestModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Message += ". That comes from the backend!";
        }
    }
}