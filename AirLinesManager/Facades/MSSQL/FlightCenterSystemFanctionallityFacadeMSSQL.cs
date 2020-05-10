using AirLinesManager.DAO.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager.Facades.MSSQL
{
    public class FlightCenterSystemFanctionallityFacadeMSSQL : FacadeBase, IFlightCenterSystemFunctionallityFacade
    {
        public FlightCenterSystemFanctionallityFacadeMSSQL() : base(new AirlineDAOMSSQL(), new CountryDAOMSSQL(), new CustomerDAOMSSQL(), new FlightDAOMSSQL(), new TicketDAOMSSQL(), new AdministratorDAOMSSQL())
        { }
        
        public void MoveFlightAndTicketsToHistory()
        {
            _flightDAO.MoveFlightsToHistory();
            _ticketDAO.MoveTicketsToHistory();
        }
    }
}
