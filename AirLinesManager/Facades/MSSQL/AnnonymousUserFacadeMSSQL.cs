using AirLinesManager.DAO.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class AnnonymousUserFacadeMSSQL : FacadeBase, IAnnonymousUserFacade
    {
        public AnnonymousUserFacadeMSSQL () : base (new AirlineDAOMSSQL(), new CountryDAOMSSQL(), new CustomerDAOMSSQL(), new FlightDAOMSSQL(), new TicketDAOMSSQL(), new AdministratorDAOMSSQL())
        { }

        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            if (_airlineDAO is null)
                throw new NullReferenceException("Airline DAO is empty.");

            return _airlineDAO.GetAll();
        }

        public IList<Flight> GetAllFlights()
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");

            return _flightDAO.GetAll();
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");

            return _flightDAO.GetAllFlightsVacancy();
        }

        public Flight GetFlightByID(long id)
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");
            if (id <= 0)
                throw new IllegalValueException($"ID provided: '{id}' is less or equal to zero.");

            return _flightDAO.Get(id);
        }

        public IList<Flight> GetFlightsByAirline(long airlineID)
        {
            if (airlineID <= 0)
                throw new IllegalValueException("Airline ID cannot be less or equal to zero.");

            return _flightDAO.GetFlightsByAirline(airlineID);
        }

        public IList<Flight> GetFlightsByDepartureDate(DateTime departureDate)
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");
            if ((DateTime.Compare(departureDate, DateTime.Now)) < 0)
                throw new IllegalValueException($"The date and time : '{departureDate}'must be later than the current date and time.");

            return _flightDAO.GetFlightsByDeprtureDate(departureDate);
        }

        public IList<Flight> GetFlightsByDepartureDateAndTime(DateTime departureDate)
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");
            if ((DateTime.Compare(departureDate, DateTime.Now)) < 0)
                throw new IllegalValueException($"The date and time : '{departureDate}'must be later than the current date and time.");

            return _flightDAO.GetFlightsByDeprtureDateAndTime(departureDate);
        }

        public IList<Flight> GetFlightsByDestinationCountry(long countryCode)
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");
            if (countryCode <= 0)
                throw new IllegalValueException($"Destination country ID : '{countryCode}' is not valid; less or equal to zero.");

            return _flightDAO.GetFlightsByDestinationCountry(countryCode);
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");
            if ((DateTime.Compare(landingDate, DateTime.Now)) < 0)
                throw new IllegalValueException($"The date: '{landingDate}' must be later than the current date and time.");

            return _flightDAO.GetFlightsByLandingDate(landingDate);
        }

        public IList<Flight> GetFlightsByLandingDateAndTime(DateTime landingDate)
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");
            if ((DateTime.Compare(landingDate, DateTime.Now)) < 0)
                throw new IllegalValueException($"The date: '{landingDate}' must be later than the current date and time.");

            return _flightDAO.GetFlightsByLandingDateAndTime(landingDate);
        }

        public IList<Flight> GetFlightsByOriginCountry(long countryCode)
        {
            if (_flightDAO is null)
                throw new NullReferenceException("Flight DAO is empty.");
            if (countryCode <= 0)
                throw new IllegalValueException($"Origin country ID : '{countryCode}' is not valid: less or equal to zero.");

            return _flightDAO.GetFlightsByOriginCountry(countryCode);
        }
    }
}
