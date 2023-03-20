using CSLBankingSystem.Interfaces;
using CSLBankingSystemBackend.Classes;
using System.Runtime.CompilerServices;

namespace CSLBankingSystem.Classes
{
    public class Customer
    {
        public int customerId { get; private set; }
        public string firstName { get; private set; }
        public string lastName { get; private set; }
        public string email { get; private set; }
        public int age { get; private set; }
        public string phoneNum { get; private set; }
        public string socialNum { get; private set; }
        public string address { get; private set; }
        public int zipCode { get; private set; }
        public List<int> accountIds { get; private set; }
        //public List<ITransaction> transactions { get; private set; }



        public Customer(int CustomerId, string firstName, string lastName, string email, int age, string socialNum, string phoneNum, string address, int zipCode)
        {
            this.customerId = CustomerId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.age = age;
            this.socialNum = socialNum;
            this.phoneNum = phoneNum;
            this.address = address;
            this.zipCode = zipCode;





            // Extract data from DB

            // Set customer data
        }

        public Customer(string firstName, string lastName, string email, int age, string socialNum, string phoneNum, string address, int zipCode) 
        {
            this.firstName = firstName;
            this.lastName = lastName; 
            this.email = email;
            this.age = age;
            this.socialNum = socialNum;
            this.phoneNum = phoneNum;
            this.address = address;
            this.zipCode = zipCode;

            this.customerId = this.SaveNewCustomer();

            Bank bank = Bank.GetInstance();

            int accId = bank.CreateBankAccount();

            this.accountIds = new List<int>() { accId };

        }

        private int SaveNewCustomer()
        {
            // Save current customer data into database
            int id = DbHandler.InsertCustomer(this);

            // Return id set by database
            return id;
        }
    }
}
