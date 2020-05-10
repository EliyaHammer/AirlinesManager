using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class Flight : IPoco
    {
        public long ID { get; set; }
        public long AirlineCompanyID { get; set; }
        public long OriginCountryCode { get; set; }
        public long DestinationCountryCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime LandingTime { get; set; }
        public int RemainingTickets { get; set; }
        public FlightStatus FlightStatus { get; set; }

        public Flight () { }
        public Flight (long airlineID, long originID, long destinationID, DateTime departTime, DateTime landingTime, int remainingTickets, FlightStatus status)
        {
                AirlineCompanyID = airlineID;
                OriginCountryCode = originID;
                DestinationCountryCode = destinationID;
                DepartureTime = departTime;
                LandingTime = landingTime;
                RemainingTickets = remainingTickets;
                FlightStatus = status;
        }
       
        public static bool operator == (Flight x, Flight y)
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
        public static bool operator != (Flight x, Flight y)
        {
            return !(x == y);
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj as Flight is null)
                return false;
            if (this.ID == (obj as Flight).ID)
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
            return $"ID: {ID}, Airline Company ID: {AirlineCompanyID}, Origin Country Code: {OriginCountryCode}, Destination Country Code: {DestinationCountryCode}," +
                $"Departure Time: {DepartureTime}, Landing Time: {LandingTime}, Remaining Tickets: {RemainingTickets}, Flight Status: {FlightStatus}";
        }
    }
}
