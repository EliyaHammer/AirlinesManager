using AirLinesManager.DAO.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public abstract class FacadeBase
    {
        protected IAirlineDAO _airlineDAO;
        protected ICountryDAO _countryDAO;
        protected ICustomerDAO _customerDAO;
        protected IFlightDAO _flightDAO;
        protected ITicketDAO _ticketDAO;
        protected IAdministratorDAO _administratorDAO;

        public FacadeBase (IAirlineDAO airlineDAO, ICountryDAO countryDAO, ICustomerDAO customerDAO, IFlightDAO flightDAO, ITicketDAO ticketDAO, IAdministratorDAO administratorDAO)
        {
            _airlineDAO = airlineDAO;
            _countryDAO = countryDAO;
            _customerDAO = customerDAO;
            _flightDAO = flightDAO;
            _ticketDAO = ticketDAO;
            _administratorDAO = administratorDAO;
        }
    }
}
