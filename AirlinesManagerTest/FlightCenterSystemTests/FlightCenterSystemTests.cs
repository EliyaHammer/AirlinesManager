using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using AirLinesManager.Facades.MSSQL;
using AirLinesManager.POCO;
using AirlinesManagerTest.FacadeTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.LoginService_CenterSystem
{
    [TestClass]
    public class FlightCenterSystemTests
    {
        AirlineDAOMSSQL airlineDAO;
        TicketDAOMSSQL ticketDAO;
        FlightDAOMSSQL flightDAO;
        AdministratorDAOMSSQL administratorDAO;
        CustomerDAOMSSQL customerDAO;
        CountryDAOMSSQL countryDAO;
        FlightCenterSystem centerSystem;

        //before every test: initialize the conn string and remove all replica.
        [TestInitialize]
        public void TestInitialize()
        {
            airlineDAO = new AirlineDAOMSSQL();
            AirlineDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            ticketDAO = new TicketDAOMSSQL();
            TicketDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            flightDAO = new FlightDAOMSSQL();
            FlightDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            administratorDAO = new AdministratorDAOMSSQL();
            AdministratorDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            customerDAO = new CustomerDAOMSSQL();
            CustomerDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            countryDAO = new CountryDAOMSSQL();
            CountryDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            centerSystem = FlightCenterSystem.GetInstance();

            airlineDAO.RemoveAllReplica();
            ticketDAO.RemoveAllReplica();
            countryDAO.RemoveAllReplica();
            flightDAO.RemoveAllReplica();
            customerDAO.RemoveAllReplica();
            administratorDAO.RemoveAllReplica();
        }

        //LOGIN METHOD TESTS:
        [TestMethod]
        public void LoginAirline()
        {
            countryDAO.Add(new Country("Israel"));
            AirlineCompany airlineUser = airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID));

            FacadeBase facade;
            ILoginToken loginToken;

            centerSystem.Login("ELALUSERNAME", "ELALPASSWORD", out facade, out loginToken);

            Assert.IsTrue(loginToken is LoginToken<AirlineCompany>);
            Assert.IsTrue(facade is LoggedInAirlineFacadeMSSQL);
        }

        [TestMethod]
        public void LoginCustomer()
        {
            Customer customerUser = customerDAO.Add(new Customer("FIRST", "LAST", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));

            FacadeBase facade;
            ILoginToken loginToken;

            centerSystem.Login("USERNAME", "PASSWORD", out facade, out loginToken);

            Assert.IsTrue(loginToken is LoginToken<Customer>);
            Assert.IsTrue(facade is LoggedInCustomerFacadeMSSQL);
        }

        [TestMethod]
        public void LoginAdministrator()
        {
            Administrator administratorUser = administratorDAO.Add(new Administrator("username", "password"));

            FacadeBase facade;
            ILoginToken loginToken;

            centerSystem.Login("username", "password", out facade, out loginToken);

            Assert.IsTrue(loginToken is LoginToken<Administrator>);
            Assert.IsTrue(facade is LoggedInAdministratorFacadeMSSQL);
        }

        [TestMethod]
        public void LoginNullFalse()
        {
            FacadeBase facade;
            ILoginToken loginToken;

            centerSystem.Login(null, "password", out facade, out loginToken);

            Assert.IsTrue(loginToken is null);
            Assert.IsTrue(facade is AnnonymousUserFacadeMSSQL);
        }

        [TestMethod]
        public void LoginNoOneSucceeded()
        {
            FacadeBase facade;
            ILoginToken loginToken;

            centerSystem.Login("username", "password", out facade, out loginToken);

            Assert.IsTrue(loginToken is null);
            Assert.IsTrue(facade is AnnonymousUserFacadeMSSQL);
        }

        [TestMethod]
        public void LoginWrongPassword()
        {
            Administrator administratorUser = administratorDAO.Add(new Administrator("username", "password"));

            FacadeBase facade;
            ILoginToken loginToken;

            centerSystem.Login("username", "wrong", out facade, out loginToken);

            Assert.IsTrue(loginToken is null);
            Assert.IsTrue(facade is AnnonymousUserFacadeMSSQL);
        }


    }
}
