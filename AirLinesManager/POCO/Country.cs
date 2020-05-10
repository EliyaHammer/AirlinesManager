using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class Country : IPoco
    {
        public long ID { get; set; }
        public string CountryName { get; set; }

        public Country() { }
        public Country (string countryName)
        {
            if (countryName != null)
                CountryName = countryName;
        }
        
        public static bool operator == (Country x, Country y)
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
        public static bool operator != (Country x, Country y)
        {
            return !(x == y);
        }
        public override bool Equals(object obj)
        {
            
                if (obj is null)
                    return false;
                if (obj as Country is null)
                    return false;
                if (this.ID == (obj as Country).ID)
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
            return $"ID: {ID}, Country Name: {CountryName}";
        }
    }
}
