using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recert.IO
{
    public class Household
    {
        public Guid Id { get; private set; }
        public string HouseholdName { get; set; }
        public Address Address { get; set; }
        public DateTime EffeciveDate { get; set; }
        public List<Person> People { get; set; }

        public Household()
        {
            this.Address = new Address();
            this.People = new List<Person>();
            this.Id = Guid.NewGuid();
        }
        public int NumPeople {
            get
            {
                return People.Count;
            }
        }

        public int GetNumPeopleOve17()
        {
            int numPeople = 0;
            foreach (Person p in this.People)
            {
                if (p.GetAge() >= 17)
                {
                    numPeople += 1;
                }
            }
            return numPeople;
        }

    }

    public class Address
    {
        public int Number { get; set; }
        public string Street { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string Country { get; set; }
        public int ZipCode { get; set; }
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public Education Education { get; set; }
        public Employment Employment { get; set; }
        public Income Income { get; set; }
        public AccountCollection Accounts { get; set; }
        public LifeInsuranceType LifeInsurance { get; set; }

        public Person()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Education = new Education();
            this.Income = new Income();
            this.Accounts = new AccountCollection();
            this.Employment = new Employment();
            this.Id = Guid.NewGuid();
            this.BirthDate = DateTime.Today;
        }
        #region Methods
        public int GetAge()
        {
            // Save today's date.
            DateTime today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - this.BirthDate.Year;
            // Go back to the year the person was born in case of a leap year
            if (this.BirthDate > today.AddYears(-age)) age--;

            return age;
        }
        #endregion

        public override string ToString()
        {
            string returnString = $"{this.FirstName} {this.LastName}";
            if (returnString.Replace(" ", "") == "")
            {
                returnString = "Unnamed";
            }
            return returnString;

        }
    }

    public class Income
    {
        public bool EmploymentIncome { get; set; }
        public bool SS { get; set; }
        public bool SSI { get; set; }
        public bool SSP { get; set; }
        public bool ReceivesUnemployment { get; set; }
        public bool ReceivesInformalAlimony { get; set; }
        public bool ReceivesCourtOrderedAlimony { get; set; }
        public bool Contributions { get; set; }
        public bool DirectDebitCard { get; set; }
    }

    public class Employment
    {
        public bool Employed { get; set; }
        public bool SelfEmployed { get; set; }
    }

    public class Education
    {
        public bool GraduatedHighSchool { get; set; }
        public bool FullTimeStudent { get; set; }
    }

    public class Account
    {
        public Guid Id { get; set; }
        public AccountType Type { get; set; }

        public Account()
        {
            this.Id = Guid.NewGuid();
            this.Type = AccountType.RegularBankAccount;
        }

        public override string ToString()
        {
            return $"{this.Type.ToString()} - {this.Id}";
        }
    }

    public class AccountCollection
    {
        public List<Account> Accounts { get; set; }

        public bool HasBankAccount
        {
            get
            {
                bool result = false;
                foreach(Account a in this.Accounts)
                {
                    if (a.Type == AccountType.RegularBankAccount)
                    {
                        result = true;
                    }
                }
                return result;

            }
        }

        public bool HasOtherAccounts
        {
            get
            {
                bool result = false;
                foreach (Account a in this.Accounts)
                {
                    if (a.Type == AccountType.Other)
                    {
                        result = true;
                    }
                }
                return result;
            }
        }

        public AccountCollection()
        {
            this.Accounts = new List<Account>();
        }
    }

    public enum AccountType
    {
        RegularBankAccount = 0,
        Other = 1
    }

    public enum LifeInsuranceType
    {
        Whole = 0,
        Term = 1
    }

}
