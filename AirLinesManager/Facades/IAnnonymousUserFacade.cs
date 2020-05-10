using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    interface IAnnonymousUserFacade
    {
        IList<AirlineCompany> GetAllAirlineCompanies();
        IList<Flight> GetAllFlights();
        Dictionary<Flight, int> GetAllFlightsVacancy();
        Flight GetFlightByID(long id);
        IList<Flight> GetFlightsByOriginCountry(long countryCode);
        IList<Flight> GetFlightsByDestinationCountry(long countryCode);
        IList<Flight> GetFlightsByDepartureDateAndTime(DateTime departureDate);
        IList<Flight> GetFlightsByLandingDateAndTime(DateTime landingDate);
        IList<Flight> GetFlightsByDepartureDate(DateTime departureDate);
        IList<Flight> GetFlightsByLandingDate(DateTime landingDate);
        IList<Flight> GetFlightsByAirline(long airlineID);
    }
}
