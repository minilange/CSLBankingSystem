using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;



namespace CSLBankingSystemUnitTest
{
    [TestClass]
    public class BankTest
    {
        [TestMethod] 
        public void TestBank()
        {
            // Instantiate Bank
            //Bank bank = Bank.GetInstance();
        }

        [TestMethod]
        public void CreateTestAccount_CorrectlyCreated_PrintAccount()
        {
            // Arrange
            

            // Act

            // Assert
        }

        [TestMethod]
        public void MakeTransaction_DeductCorrectAmount()
        {
            // Arrange
            double beginningBalance = 100.00;
            double deductAmount = 15.00;
            double expectedResult = 85.00;
            

            // Act


            // Assert
            
        }
    }
}
