using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public interface ILoggedInCustomerFacade
    {
        void CancelTicket(LoginToken<Customer> token, Ticket ticket);
        Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight);
        IList<Flight> GetAllMyFlights(LoginToken<Customer> token);
        void ChangeMyPassword(LoginToken<Customer> token, string oldPassword, string newPassword);
    }
}
