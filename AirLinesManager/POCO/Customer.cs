using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class Customer : IPoco, IUser
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CreditCardNumber { get; set; }

        public Customer() { }
        public Customer (string firstName, string lastName, string userName, string password, string address, string phoneNumber, string cardNumber)
        {
            if (firstName != null)
                FirstName = firstName;
            if (lastName != null)
                LastName = lastName;
            if (userName != null)
                UserName = userName;
            if (password != null)
                Password = password;
            if (address != null)
                Address = address;
            if (phoneNumber != null)
                PhoneNumber = phoneNumber;
            if (cardNumber != null)
                CreditCardNumber = cardNumber;
        }

        public static bool operator == (Customer x, Customer y)
        {
            if (x is null && y is null)
                return true;
            if (x is null || y is null)
                return false;
            if (x.ID == y.ID)
                return true;
            else
                return false;
        }
        public static bool operator != (Customer x, Customer y)
        {
            return !(x == y);
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false; 
            if (obj as Customer is null)
                return false;
            if (this.ID == (obj as Customer).ID)
                return true;
            else
                return false;
        }
        public override int GetHashCode()
        {
            return (int)this.ID;
        }

        public override string ToString()
        {
            return $"ID: {ID}, First Name: {FirstName}, Last Name: {LastName}, Username: {UserName}, " +
                $"Address: {Address}, Phone Number: {PhoneNumber}, Card Number: {CreditCardNumber}";
        }
    }
}
