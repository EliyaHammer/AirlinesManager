using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class LoggedInAirlineFacadeMSSQL : AnnonymousUserFacadeMSSQL, ILoggenInAirlineFacade
    {
        public LoggedInAirlineFacadeMSSQL()
        { }

        //Also checks if the current user is the given flight's airline.
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("User given is empty.");
            if (flight is null)
                throw new NullReferenceException("Flight provided is empty");
            if (flight.ID <= 0)
                throw new IllegalValueException($"Flight ID: '{flight.ID}' is not valid: less or equal to zero.");
            if (_flightDAO.Get(flight.ID) == null)
                throw new FlightNotFoundException($"The flight provided: 'id {flight.ID}' could not be found.");
            if (flight.AirlineCompanyID != token.user.ID)
                throw new UserAccessabillityException($"The flight provided: 'id {flight.ID}' is not associated with this user: ' id {token.user.ID}'. It assosicated with Airline company: 'id {flight.AirlineCompanyID}'");

                _flightDAO.Remove(flight);
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("User given is empty.");
            if (oldPassword is null || newPassword is null)
                throw new NullReferenceException($"Fields: '{oldPassword}' , '{newPassword}' must be filled");
            if (token.user.Password != oldPassword)
                throw new WrongPasswordException("Password is wrong.");
            if (_airlineDAO.Get(token.user.ID) == null)
                throw new AirlineNotFoundException($"Airline '{token.user.AirLineName}' could not be found, therefore cannot be updated.");

                token.user.Password = newPassword;
                _airlineDAO.Update(token.user);
        }

        //Also checks if the given flight is assigned to the current user.
        public void CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("Given user is empty.");
            if (flight is null)
                throw new NullReferenceException("Flight provided is empty.");
            if (flight.AirlineCompanyID <= 0)
                throw new IllegalValueException($"Airline Company ID: '{flight.AirlineCompanyID}' is not valid; less or equal to zero.");
            if (_airlineDAO.Get(flight.AirlineCompanyID) is null)
                throw new AirlineNotFoundException($"No airline found with the provided ID: '{flight.AirlineCompanyID}'.");
            if (flight.AirlineCompanyID != token.user.ID)
                throw new UserAccessabillityException($"Flight's airline ID: '{flight.AirlineCompanyID}' does not match user's ID: '{token.user.ID}'.");
            if (flight.DestinationCountryCode <= 0)
                throw new IllegalValueException($"Destination country code: '{flight.DestinationCountryCode}' is less or equal to zero.");
            if (flight.OriginCountryCode <= 0)
                throw new IllegalValueException($"Origin country code: '{flight.OriginCountryCode}' is less or equal to zero.");
            if ((DateTime.Compare(flight.DepartureTime, DateTime.Now)) < 0)
                throw new IllegalValueException($"Departure date and time: '{flight.DepartureTime}' must be later than the current date and time: '{DateTime.Now}'.");
            if ((DateTime.Compare(flight.LandingTime, DateTime.Now)) < 0)
                throw new IllegalValueException($"Landing date and time: '{flight.LandingTime}' must be later than the current date and time: '{DateTime.Now}'.");
            if (DateTime.Compare(flight.LandingTime, flight.DepartureTime) < 0)
                throw new IllegalValueException($"Landing time: '{flight.LandingTime}' cannot be sooner than departure time: '{flight.DepartureTime}'.");
            if (_countryDAO.Get(flight.DestinationCountryCode) is null || _countryDAO.Get(flight.OriginCountryCode) is null)
                throw new CountryNotFoundException("One of the countries could not be found.");

            _flightDAO.Add(flight);
        }

        public IList<Flight> GetAllMyFlights(LoginToken<AirlineCompany> token)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("User is empty.");
            if (token.user.ID <= 0)
                throw new IllegalValueException($"Airline ID: '{token.user.ID}' cannot be less or equal to zero.");

                return _flightDAO.GetFlightsByAirline(token.user.ID);
        }

        public IList<Ticket> GetAllMyTickets(LoginToken<AirlineCompany> token)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("User is empty.");
            if (token.user.ID <= 0)
                throw new IllegalValueException($"Airline ID: '{token.user.ID}' cannot be less or equal to zero");

               return _ticketDAO.GetTicketsByAirline(token.user.ID);
        }

        //Also checks that the current user is the same as the given airline to modify -> will not change a password.
        public void ModifyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("User is empty.");
            if (airline is null)
                throw new NullReferenceException("Airline provided is empty.");
            if (airline.ID <= 0)
                throw new IllegalValueException($"Airline's ID: '{airline.ID}' is not valid.");
            if (airline.ID != token.user.ID || airline.Password != token.user.Password)
                throw new UserAccessabillityException("This user cannot access the provided airline. ID or Password don't match.");
            if (_airlineDAO.Get(token.user.ID) == null)
                throw new AirlineNotFoundException($"Airline '{token.user.AirLineName}' could not be found, therefor cannot be updated.");
            if (airline.AirLineName is null || airline.Password is null || airline.UserName is null)
                throw new NullReferenceException($"One or more of the fields are not filled: '{airline.AirLineName}' '{airline.Password}' '{airline.UserName}'.");
            if (airline.CountryCode <= 0)
                throw new IllegalValueException($"Country code provided: '{airline.CountryCode}' is not valid.");
            if (_countryDAO.Get(airline.CountryCode) is null)
                throw new CountryNotFoundException($"Country: '{airline.CountryCode}' could not be found.");
            if (_airlineDAO.CheckNameExistance(airline.AirLineName, token.user.ID))
                throw new AlreadyExistsException($"The name providen: '{airline.AirLineName}' already exists.");
            if (_airlineDAO.CheckUsernameExistance(airline.UserName, token.user.ID))
                throw new AlreadyExistsException($"The username providen: '{airline.UserName}' already exists.");

                token.user = airline;
                _airlineDAO.Update(airline);
        }

        //Also checks if the given flight is assigned to the current user.
        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("User is empty.");
            if (flight is null)
                throw new NullReferenceException("Flight provided is empty.");
            if (flight.ID <= 0)
                throw new IllegalValueException($"Flight ID: '{flight.ID}' is not valid: less or equal to zero.");
            if (_flightDAO.Get(flight.ID) == null)
                throw new FlightNotFoundException($"Flight provided: '{flight.ID}' could not be found.");
            if (flight.AirlineCompanyID <= 0)
                throw new IllegalValueException($"Airline company ID: '{flight.AirlineCompanyID}' is not valid: less or equal to zero.");
            if (flight.AirlineCompanyID != token.user.ID)
                throw new UserAccessabillityException($"This user: id '{token.user.ID}' is not the flight's assosiated airline company: '{flight.AirlineCompanyID}'");
            if (_airlineDAO.Get(flight.AirlineCompanyID) is null)
                throw new AirlineNotFoundException($"No airline company with the provided ID: '{flight.AirlineCompanyID}' found.");
            if (flight.DestinationCountryCode <= 0)
                throw new IllegalValueException($"Destination country code: '{flight.DestinationCountryCode}' is not valid: equal or less than zero.");
            if (flight.OriginCountryCode <= 0)
                throw new IllegalValueException($"Origin country code: '{flight.OriginCountryCode}' is not valid: equal or less than zero.");
            if (_countryDAO.Get(flight.OriginCountryCode) is null)
                throw new CountryNotFoundException($"Origin country: id '{flight.OriginCountryCode}', could not be found.");
            if ( _countryDAO.Get(flight.DestinationCountryCode) is null)
                throw new CountryNotFoundException($"Destination country: id '{flight.DestinationCountryCode}', could not be found.");
            if ((DateTime.Compare(flight.DepartureTime, DateTime.Now)) < 0)
                throw new IllegalValueException($"Departure date and time: '{flight.DepartureTime}' must be later than the current date and time: '{DateTime.Now}'.");
            if ((DateTime.Compare(flight.LandingTime, DateTime.Now)) < 0)
                throw new IllegalValueException($"Landing date and time: '{flight.LandingTime}' must be later than the current date and time: '{DateTime.Now}'.");
            if (DateTime.Compare(flight.LandingTime, flight.DepartureTime) < 0)
                throw new IllegalValueException($"Landing time: '{flight.LandingTime}' cannot be sooner than departure time: '{flight.DepartureTime}'.");

            _flightDAO.Update(flight);
        }
    }
}
