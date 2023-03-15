using CSLBankingSystem.Interfaces;

namespace CSLBankingSystem.Classes
{
    public class Account
    {

        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Int32 balance { get; set; }
        public List<ITransaction> transaction { get; set; }


        public Account(string id)
        {

        }

        public Account()
        {

        }
    }
}
