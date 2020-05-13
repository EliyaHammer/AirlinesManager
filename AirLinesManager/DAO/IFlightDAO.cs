using AirLinesManager.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public interface IFlightDAO : IBasicDAO<Flight>, IDAOTest
    {
        Dictionary<Flight, int> GetAllFlightsVacancy();
        IList<Flight> GetFlightsByOriginCountry(long countryID);
        IList<Flight> GetFlightsByDestinationCountry(long countryID);
        IList<Flight> GetFlightsByDeprtureDateAndTime(DateTime date);
        IList<Flight> GetFlightsByLandingDateAndTime(DateTime date);
        IList<Flight> GetFlightsByDeprtureDate(DateTime date);
        IList<Flight> GetFlightsByLandingDate(DateTime date);
        IList<Flight> GetFlightsByCustomer(Customer customer);
        IList<Flight> GetFlightsByAirline(long airlineID);
        void MoveFlightsToHistory();
        List<Flight> GetFlightHistory();
    }
}
