using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class Ticket : IPoco
    {
        public long ID { get; set; }
        public long FlightID { get; set; }
        public long CustomerID { get; set; }

        public Ticket() { }
        public Ticket (long flightID, long customerID)
        {
            if (flightID > 0 && customerID > 0)
            {
                FlightID = flightID;
                CustomerID = customerID;
            }
        }
       
        public static bool operator == (Ticket x, Ticket y)
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
        public static bool operator != (Ticket x, Ticket y)
        {
            return !(x == y);
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj as Ticket is null)
                return false;
            if (this.ID == (obj as Ticket).ID)
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
            return $"ID: {ID}, Flight ID: {FlightID}, Customer ID: {CustomerID}";
        }
    }
}
