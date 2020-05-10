using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class LoggedInCustomerFacadeMSSQL : AnnonymousUserFacadeMSSQL, ILoggedInCustomerFacade
    {
        public LoggedInCustomerFacadeMSSQL ()
        { }
        
        //Checks if the current user is the given ticket's customer.
        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException($"User: '{token.user.UserName}' is empty.");
            if (ticket is null)
                throw new NullReferenceException($"The given ticket is empty.");
            if (ticket.ID <= 0)
                throw new IllegalValueException($"The ticket ID of the providen ticket: '{ticket.ID}' is not valid.");
            if (_ticketDAO.Get(ticket.ID) == null)
                throw new TicketNotFoundException($"Ticket: '{ticket.ID}' does not exist.");
            if (ticket.CustomerID != token.user.ID)
                throw new UserAccessabillityException($"Customer ID: '{ticket.CustomerID}' does not match user id: '{token.user.ID}'.");

            _ticketDAO.Remove(ticket);
        }

        public void ChangeMyPassword(LoginToken<Customer> token, string oldPassword, string newPassword)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("User given is empty.");
            if (oldPassword is null || newPassword is null)
                throw new NullReferenceException($"Fields: '{oldPassword}' , '{newPassword}' must be filled");
            if (token.user.Password != oldPassword)
                throw new WrongPasswordException("Password is wrong.");
            if (_customerDAO.Get(token.user.ID) == null)
                throw new AirlineNotFoundException($"Customer of username: '{token.user.UserName}' could not be found, therefore cannot be updated.");

            token.user.Password = newPassword;
            _customerDAO.Update(token.user);
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("User is empty.");
            if (token.user.ID <= 0)
                throw new IllegalValueException($"User's ID: '{token.user.ID}' is no valid; equal or less than zero.");

                return _flightDAO.GetFlightsByCustomer(token.user);
        }

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            if (token is null || token.user is null)
                throw new NullReferenceException("The given user is empty.");
            if (flight is null)
                throw new NullReferenceException("Flight given is empty.");
            if (token.user.ID <= 0)
                throw new IllegalValueException($"Customer ID: '{token.user.ID}' in the providen ticket is not valid.");
            if (flight.ID <= 0)
                throw new IllegalValueException($"Flight ID: '{flight.ID}' in the providen ticket is not valid.");
            if (_customerDAO.Get(token.user.ID) is null)
                throw new CustomerNotFoundException($"The customer: '{token.user.UserName}, id: {token.user.ID}' could not be found.");
            if (_flightDAO.Get(flight.ID) is null)
                throw new FlightNotFoundException($"Flight id: '{flight.ID}' could not be found.");
            if (_flightDAO.Get(flight.ID).RemainingTickets <= 0)
                throw new TicketNotFoundException("Flight is full. Cannot add ticket.");

            Ticket newTicket = new Ticket(flight.ID, token.user.ID);

            if (_ticketDAO.CheckTicketUniqueFieldsExistance(newTicket))
                throw new AlreadyExistsException($"There is already a ticket for this flight: '{flight.ID}' and customer: '{token.user.ID}'.");

                return _ticketDAO.Add(newTicket);
        }
    }
}
