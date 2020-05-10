using AirLinesManager.DAO.MSSQL;
using AirLinesManager.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager.LoginService
{
    public abstract class LoginServiceBase
    {
        protected IAirlineDAO _airlineDAO;
        protected ICustomerDAO _customerDAO;
        protected IAdministratorDAO _administratorDAO;

        public LoginServiceBase (IAirlineDAO airlineDAO, ICustomerDAO customerDAO, IAdministratorDAO administratorDAO)
        {
            _airlineDAO = airlineDAO;
            _customerDAO = customerDAO;
            _administratorDAO = administratorDAO;
        }

        internal abstract bool TryLogin(string username, string password, out FacadeBase facade, out ILoginToken loginToken);
        protected abstract bool TryAirLineLogin(string userName, string password, out LoginToken<AirlineCompany> token);
        protected abstract bool TryCustomerLogin(string userName, string password, out LoginToken<Customer> token);
        protected abstract bool TryAdministratorLogin(string userName, string password, out LoginToken<Administrator> token);
    }
}
