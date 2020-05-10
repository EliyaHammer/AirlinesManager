using AirLinesManager.DAO.MSSQL;
using AirLinesManager.Facades.MSSQL;
using AirLinesManager.LoginService;
using AirLinesManager.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
   public class LoginServiceMSSQL : LoginServiceBase , ILoginService
    {

        public LoginServiceMSSQL() : base (new AirlineDAOMSSQL(), new CustomerDAOMSSQL(), new AdministratorDAOMSSQL())
        {
        }

        /// <summary>
        /// This method will NOT BE called from outside the project to interact will the flight center facades
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="facade"></param>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        internal override bool TryLogin(string username, string password, out FacadeBase facade, out ILoginToken loginToken)
        {
            loginToken = null; 
            facade = new AnnonymousUserFacadeMSSQL();

            LoginToken<AirlineCompany> airlineToken;
            if (TryAirLineLogin(username, password, out airlineToken))
            {
                loginToken = airlineToken;
                facade = new LoggedInAirlineFacadeMSSQL();
                return true;
            }

            LoginToken<Customer> customerToken;
            if (TryCustomerLogin(username, password, out customerToken))
            {
                loginToken = customerToken;
                facade = new LoggedInCustomerFacadeMSSQL();
                return true;
            }

            LoginToken<Administrator> administratorToken;
            if (TryAdministratorLogin(username, password, out administratorToken))
            {
                loginToken = administratorToken;
                facade = new LoggedInAdministratorFacadeMSSQL();
                return true;
            }

            return false;

        }

        protected override bool TryAirLineLogin(string userName, string password, out LoginToken<AirlineCompany> token)
        {
            token = null;
            if (userName is null || password is null)
                return false;

            AirlineCompany airlineResult;
            airlineResult = _airlineDAO.GetAirlineByUsername(userName);

            if (airlineResult == null)
                return false;

            if (airlineResult.Password != password)
                throw new WrongPasswordException ($"Wrong password for user {userName}.");
            else
            {
                token = new LoginToken<AirlineCompany>();
                token.user = airlineResult;
                return true;
            }
        }

        protected override bool TryCustomerLogin(string userName, string password, out LoginToken<Customer> token)
        {
            token = null;
            if (userName is null || password is null)
                return false;

            Customer customerResult;
            customerResult = _customerDAO.GetCustomerByUsername(userName);

            if (customerResult == null)
            return false;

            if (customerResult.Password != password)
                throw new WrongPasswordException("Wrong password.");
            else
            {
                token = new LoginToken<Customer>();
                token.user = customerResult;
                return true;
            }
        }

        protected override bool TryAdministratorLogin(string userName, string password, out LoginToken<Administrator> token)
        {
            token = null;
            if (userName is null || password is null)
                return false;

            Administrator administratorResult;
            administratorResult = _administratorDAO.GetAdminByUsername(userName);

            if (administratorResult == null)
                return false;

            if (administratorResult.Password != password)
                throw new WrongPasswordException("Wrong password.");
            else
            {
                token = new LoginToken<Administrator>();
                token.user = administratorResult;
                return true;
            }
        }
    }
}
