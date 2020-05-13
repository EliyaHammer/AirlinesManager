using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class AirlineCompany : IPoco, IUser
    {
        public long ID { get; set; }
        public string AirLineName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long CountryCode { get; set; }

        public AirlineCompany() { }
        public AirlineCompany (string airlineName, string userName, string password, long countryCode)
        {
            if (airlineName != null)
                AirLineName = airlineName;
            if (userName != null)
                UserName = userName;
            if (password != null)
                Password = password;
            if (countryCode > 0)
                CountryCode = countryCode;
        }

        public static bool operator == (AirlineCompany x, AirlineCompany y)
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
        public static bool operator != (AirlineCompany x, AirlineCompany y)
        {
            return !(x == y);
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if ((obj as AirlineCompany) is null)
                return false;
            if (this.ID == (obj as AirlineCompany).ID)
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
            return $"ID: {ID}, Airline Name: {AirLineName}, Username: {UserName}, Country Code: {CountryCode}";
        }
    }
}
