using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    interface ILoggenInAirlineFacade
    {
        IList<Ticket> GetAllMyTickets(LoginToken<AirlineCompany> token);
        IList<Flight> GetAllMyFlights(LoginToken<AirlineCompany> token);
        void CancelFlight(LoginToken<AirlineCompany> token, Flight flight);
        void CreateFlight(LoginToken<AirlineCompany> token, Flight flight);
        void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight);
        void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword);
        void ModifyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline);
    }
}
