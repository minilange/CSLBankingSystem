using CSLBankingSystemBackend.Classes;

namespace CSLBankingSystemBackend.Interfaces
{
    public class IAccount
    {
        int id { get; set; }
        string name { get; set; }
        string description { get; set; }
        float balance { get; set; }
        List<Transaction> transactions { get; set; }
    }
}
