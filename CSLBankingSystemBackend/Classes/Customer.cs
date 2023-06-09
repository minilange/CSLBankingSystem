﻿using CSLBankingSystem.Interfaces;
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
        public string? password { get; private set; }
        public int age { get; private set; }
        public string phoneNum { get; private set; }
        public string socialNum { get; private set; }
        public string address { get; private set; }
        public int zipCode { get; private set; }
        public List<int> accountIds { get; private set; }
        //public List<ITransaction> transactions { get; private set; }



        public Customer(int CustomerId, string firstName, string lastName, string email, int age, string socialNum, string phoneNum, string address, int zipCode)
        {
            // Set customer data
            this.customerId = CustomerId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.age = age;
            this.socialNum = socialNum;
            this.phoneNum = phoneNum;
            this.address = address;
            this.zipCode = zipCode;

            // Extract account data from DB
            this.accountIds = DbHandler.GetAllCustomerAccountIds(this.customerId);
        }

        public Customer(string firstName, string lastName, string email, string password, int age, string socialNum, string phoneNum, string address, int zipCode, int accountId)
        {
            // Set all customer data
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
            this.age = age;
            this.socialNum = socialNum;
            this.phoneNum = phoneNum;
            this.address = address;
            this.zipCode = zipCode;

            // Create list of account ids
            this.accountIds = new List<int>() { accountId };

            // Save account to DB
            this.customerId = this.SaveNewCustomer();
        }

        private int SaveNewCustomer()
        {
            // Save current customer data into database
            int id = DbHandler.InsertCustomer(this);

            this.password = null;

            // Return id set by database
            return id;
        }
    }
}
