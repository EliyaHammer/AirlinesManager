using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.FacadeTests
{
    [TestClass]
    public class LoggenInAirlineFacadeTests
    {
        AirlineDAOMSSQL airlineDAO;
        TicketDAOMSSQL ticketDAO;
        FlightDAOMSSQL flightDAO;
        AdministratorDAOMSSQL administratorDAO;
        CustomerDAOMSSQL customerDAO;
        CountryDAOMSSQL countryDAO;
        LoggedInAirlineFacadeMSSQL facade;

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

            ticketDAO.RemoveAllReplica();
            customerDAO.RemoveAllReplica();
            flightDAO.RemoveAllReplica();
            airlineDAO.RemoveAllReplica();
            countryDAO.RemoveAllReplica();
            administratorDAO.RemoveAllReplica();

            facade = new LoggedInAirlineFacadeMSSQL();
        }

        //CANCEL FLIGHT METHOS TESTS:
        [TestMethod]
        public void CancelFlight()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAYSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ARKIA").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));

            ticketDAO.Add(new Ticket(flightDAO.GetAll()[0].ID, customerDAO.GetCustomerByUsername("USERNAME").ID));
            ticketDAO.Add(new Ticket(flightDAO.GetAll()[1].ID, customerDAO.GetCustomerByUsername("USERNAME").ID));

            Assert.AreEqual(2, flightDAO.GetAll().Count);
            Assert.AreEqual(2, ticketDAO.GetAll().Count);

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");
            facade.CancelFlight(airlineLoggenIn, flightDAO.GetAll()[0]);
            Assert.AreEqual(1, flightDAO.GetAll().Count);
            Assert.AreEqual(1, ticketDAO.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CancelFlightNullTokenException()
        {
            facade.CancelFlight(null, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CancelFlightNullUserException()
        {
            LoginToken<AirlineCompany> loggedAirline = new LoginToken<AirlineCompany>();

            facade.CancelFlight(loggedAirline, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CancelFlightNullFlightException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAYSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            facade.CancelFlight(airlineLoggenIn, null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CancelFlightIDZeroException()
        {
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAYSERNAME", "ARKIAPASSWORD", countryDAO.Add(new Country("Israel")).ID));

            facade.CancelFlight(airlineLoggenIn, new Flight(1, 1, 1, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public void CancelFlightDoesNotExistException()
        {
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAYSERNAME", "ARKIAPASSWORD", countryDAO.Add(new Country("Israel")).ID));
            Flight flightTest = new Flight(1, 1, 1, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightTest.ID = 1;

            facade.CancelFlight(airlineLoggenIn, flightTest);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAccessabillityException))]
        public void CancelFlightWrongUserForFlightException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAYSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ARKIA");
            facade.CancelFlight(airlineLoggenIn, flightDAO.GetAll()[0]);
        }


        //CHANGE MY PASSWORD METHOD TESTS:
        [TestMethod]
        public void ChangeMyPassword()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            facade.ChangeMyPassword(airlineLoggenIn, "ELALPASSEORD", "ELALPASSWORD");
            Assert.AreEqual("ELALPASSWORD", airlineLoggenIn.user.Password);
            Assert.AreEqual("ELALPASSWORD", airlineDAO.GetAirlineByName("ELAL").Password);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ChangeMyPasswordNullTokenException()
        {
            facade.ChangeMyPassword(null, "e", "o");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ChangeMyPasswordNullUserException()
        {
            LoginToken<AirlineCompany> loggenAirline = new LoginToken<AirlineCompany>();

            facade.ChangeMyPassword(loggenAirline, "E", "t");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ChangeMyPasswordNullOldPasswordException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            facade.ChangeMyPassword(airlineLoggenIn, null, "e");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ChangeMyPasswordNullNewPasswordException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            facade.ChangeMyPassword(airlineLoggenIn, "ELALPASSEORD", null);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void ChangeMyPasswordWrongPasswordException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            facade.ChangeMyPassword(airlineLoggenIn, "OLDWRONG", "NEW");
        }

        //CREATE FLIGHT METHOD TESTS:
        [TestMethod]
        public void CreateFlight()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            Flight newFlight = new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted);

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            facade.CreateFlight(airlineLoggenIn, newFlight);
            Assert.AreEqual(1, flightDAO.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateFlightNullTokenException()
        {
            facade.CreateFlight(null, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateFlightNullUserException()
        {
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();

            facade.CreateFlight(airlineLoggenIn, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateFlightNullFlightException()
        {
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            facade.CreateFlight(airlineLoggenIn, null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateFlightAirlineIDZeroException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            AirlineCompany elal = airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", countryDAO.GetCountryByName("Israel").ID));
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            facade.CreateFlight(airlineLoggenIn, new Flight(0, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
        }

        [TestMethod]
        [ExpectedException(typeof(UserAccessabillityException))]
        public void CreateFlightWrongAirlineException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));

            Flight newFlight = new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted);

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ARKIA");

            facade.CreateFlight(airlineLoggenIn, newFlight);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateFlightCountryIDZeroException()
        {
            countryDAO.Add(new Country("Israel"));
            AirlineCompany elal = airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", countryDAO.GetCountryByName("Israel").ID));
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            facade.CreateFlight(airlineLoggenIn, new Flight(elal.ID, 0, 0, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateFlightInvalidDateException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            AirlineCompany elal = airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            facade.CreateFlight(airlineLoggenIn , new Flight(elal.ID, israel.ID, israel.ID, new DateTime(2010, 12, 5, 14, 00, 00), new DateTime(2010, 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateFlightLandingBeforeDepartException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            AirlineCompany elal = airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            facade.CreateFlight(airlineLoggenIn, new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 3), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 1), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
        }

        [TestMethod]
        [ExpectedException(typeof(CountryNotFoundException))]
        public void CreateFlightCountryNotFoundException()
        {
            AirlineCompany elal = airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", countryDAO.Add(new Country("Israel")).ID));
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            facade.CreateFlight(airlineLoggenIn, new Flight(elal.ID, 1, 1, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
        }

        [TestMethod]
        [ExpectedException(typeof(AirlineNotFoundException))]
        public void CreateFlightAirlineNotFoundException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            AirlineCompany elal = airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            facade.CreateFlight(airlineLoggenIn, (new Flight(1, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted)));
        }


        //GET ALL MY FLIGHTS METHOD TESTS:
        [TestMethod]
        public void GetAllMyFlights()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ARKIA").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ARKIA").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ARKIA");

            Assert.AreEqual(2, facade.GetAllMyFlights(airlineLoggenIn).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllMyFlightsNullTokenException()
        {
            facade.GetAllMyFlights(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllMyFlightsNullUserException()
        {
            LoginToken<AirlineCompany> airlineLoggen = new LoginToken<AirlineCompany>();

            facade.GetAllMyFlights(airlineLoggen);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetAllMyFlightsAirlineIDZeroException()
        {
            LoginToken<AirlineCompany> airlineLoggedIn = new LoginToken<AirlineCompany>();
            airlineLoggedIn.user = new AirlineCompany("NAME", "USERNAME", "PASSWORD", countryDAO.Add(new Country("Israel")).ID);

            facade.GetAllMyFlights(airlineLoggedIn);
        }

        [TestMethod]
        public void GetAllMyFlightsAirlineNotFoundException()
        {
            LoginToken<AirlineCompany> airlineLoggedIn = new LoginToken<AirlineCompany>();
            airlineLoggedIn.user = new AirlineCompany("NAME", "USERNAME", "PASSWORD", countryDAO.Add(new Country("Israel")).ID);
            airlineLoggedIn.user.ID = 1;

            Assert.AreEqual(null, facade.GetAllMyFlights(airlineLoggedIn));
        }

        //GET ALL MY TICKETS METHOD TESTS:
        [TestMethod]
        public void GetAllMyTickets()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ARKIA").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ARKIA").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));

            ticketDAO.Add(new Ticket(flightDAO.GetAll()[0].ID, customerDAO.GetAll()[0].ID));
            ticketDAO.Add(new Ticket(flightDAO.GetAll()[1].ID, customerDAO.GetAll()[0].ID));
            ticketDAO.Add(new Ticket(flightDAO.GetAll()[2].ID, customerDAO.GetAll()[0].ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ARKIA");

            Assert.AreEqual(2, facade.GetAllMyTickets(airlineLoggenIn).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllMyTicketsNullTokenException()
        {
            facade.GetAllMyTickets(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllMyTicketsNullUserException()
        {
            LoginToken<AirlineCompany> airlineLogged = new LoginToken<AirlineCompany>();

            facade.GetAllMyTickets(airlineLogged);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetAllMyTicketsUserIDZeroException()
        {
            LoginToken<AirlineCompany> loggedInAirline = new LoginToken<AirlineCompany>();
            loggedInAirline.user = new AirlineCompany("NAME", "USERNAME", "PASSWORD", countryDAO.Add(new Country("Israel")).ID);

            facade.GetAllMyTickets(loggedInAirline);
        }

        [TestMethod]
        public void GetAllMyTicketsAirlineUserNotFoundException()
        {
            LoginToken<AirlineCompany> loggedInAirline = new LoginToken<AirlineCompany>();
            loggedInAirline.user = new AirlineCompany("NAME", "USERNAME", "PASSWORD", countryDAO.Add(new Country("Israel")).ID);
            loggedInAirline.user.ID = 1;

            Assert.AreEqual(null, facade.GetAllMyTickets(loggedInAirline));
        }

        //MODIFY AIRLINE DETAILS METHOD TESTS:
        [TestMethod]
        public void ModifyAirlineDetails()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            AirlineCompany elalModified = airlineDAO.GetAirlineByName("ELAL");
            elalModified.AirLineName = "ELALNEW";
            facade.ModifyAirlineDetails(airlineLoggenIn, elalModified);

            Assert.AreEqual("ELALNEW", airlineLoggenIn.user.AirLineName);
            Assert.AreEqual(elalModified, airlineDAO.GetAirlineByName("ELALNEW"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ModifyAirlineDetailsNullTokenException()
        {
            facade.ModifyAirlineDetails(null, new AirlineCompany());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ModifyAirlineDetailsNullUserException()
        {
            LoginToken<AirlineCompany> loggedAirline = new LoginToken<AirlineCompany>();

            facade.ModifyAirlineDetails(loggedAirline, new AirlineCompany());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ModifyAirlineDetailsNullAirlineException()
        {
            LoginToken<AirlineCompany> loggedAirline = new LoginToken<AirlineCompany>();
            loggedAirline.user = new AirlineCompany();

            facade.ModifyAirlineDetails(loggedAirline, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAccessabillityException))]
        public void ModifyAirlineDetailsWrongUserException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSER", "PASSWORD", countryDAO.GetCountryByName("Israel").ID));

            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            facade.ModifyAirlineDetails(airlineLoggenIn, elal);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAccessabillityException))]
        public void ModifyAirlineDetailsWrongPasswordException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            AirlineCompany elalChanged = new AirlineCompany("ELAL", "ELALUSERNAME", "NEW", countryDAO.GetCountryByName("Israel").ID);
            elalChanged.ID = airlineLoggenIn.user.ID;

            facade.ModifyAirlineDetails(airlineLoggenIn, elalChanged);
        }

        [TestMethod]
        [ExpectedException(typeof(AirlineNotFoundException))]
        public void ModifyAirlineDoesntExistException()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            theAirline.ID = 1;

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            facade.ModifyAirlineDetails(airlineLoggenIn, theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void ModifyAirlineCountryIDZeroExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            theAirline = airlineDAO.Add(theAirline);

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            theAirline.CountryCode = 0;

            facade.ModifyAirlineDetails(airlineLoggenIn, theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(CountryNotFoundException))]
        public void ModifyCountryNotFoundExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            theAirline = airlineDAO.Add(theAirline);

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            theAirline.CountryCode = countryDAO.GetCountryByName("Israel").ID + 1;

            facade.ModifyAirlineDetails(airlineLoggenIn, theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ModifyNullNameExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            theAirline.AirLineName = null;
            facade.ModifyAirlineDetails(airlineLoggenIn, theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNullPasswordExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            theAirline.Password = null;
            facade.ModifyAirlineDetails(airlineLoggenIn, theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ModifyNullUsernameExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            theAirline.UserName = null;
            facade.ModifyAirlineDetails(airlineLoggenIn, theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void ModifyNameExistsExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            theAirline.AirLineName = "ARKIA";

            facade.ModifyAirlineDetails(airlineLoggenIn, theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void ModifyUsernameExistsExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            theAirline.UserName = "ARKIAUSERNAME";

            facade.ModifyAirlineDetails(airlineLoggenIn, theAirline);
        }


        //UPDATE FLIGHT METHOD TESTS:
        [TestMethod]
        public void UpdateFlight()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            Flight flightChanged = flightDAO.GetAll()[0];
            flightChanged.RemainingTickets = 10;

            facade.UpdateFlight(airlineLoggenIn, flightChanged);

            Assert.AreEqual(10, flightDAO.GetAll()[0].RemainingTickets);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateFlightNullTokenException()
        {
            facade.UpdateFlight(null, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateFlightNullUserException()
        {
            LoginToken<AirlineCompany> airlineLogged = new LoginToken<AirlineCompany>();

            facade.UpdateFlight(airlineLogged, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateFlightNullFlightException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = airlineDAO.GetAirlineByName("ELAL");

            facade.UpdateFlight(airlineLoggenIn, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAccessabillityException))]
        public void UpdateFlightWrongAirlineException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = new AirlineCompany();

            Flight flightChanged = flightDAO.GetAll()[0];
            flightChanged.RemainingTickets = 10;

            facade.UpdateFlight(airlineLoggenIn, flightChanged);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateFlightIDZeroException()
        {
            AirlineCompany theAirline = airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.Add(new Country("Israel")).ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            facade.UpdateFlight(airlineLoggenIn, new Flight(theAirline.ID, 1, 1, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted)); 
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public void UpdateFlightDoesntExistException()
        {
            AirlineCompany theAirline = airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.Add(new Country("Israel")).ID));

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = theAirline;

            Flight flightTest = new Flight(theAirline.ID, 1, 1, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightTest.ID = 2;
            facade.UpdateFlight(airlineLoggenIn, flightTest);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateFlightCountryIDZeroException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            flightTest.DestinationCountryCode = 0;
            facade.UpdateFlight(airlineLoggenIn, flightTest);
        }

        [TestMethod]
        [ExpectedException(typeof(CountryNotFoundException))]
        public void UpdateFlightCountryNotFoundException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            flightTest.DestinationCountryCode = 1;
            facade.UpdateFlight(airlineLoggenIn, flightTest);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateFlightAirlineIDZeroException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            flightTest.AirlineCompanyID = 0;
            facade.UpdateFlight(airlineLoggenIn, flightTest);
        }

        [TestMethod]
        [ExpectedException(typeof(AirlineNotFoundException))]
        public void UpdateFlightAirlineNotFoundException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            flightTest.AirlineCompanyID = elal.ID + 1;
            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;
            airlineLoggenIn.user.ID = flightTest.AirlineCompanyID;

            facade.UpdateFlight(airlineLoggenIn, flightTest);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateFlightDatePassedException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            flightTest.DepartureTime = new DateTime(DateTime.Now.Year - 1, 12, 5);
            facade.UpdateFlight(airlineLoggenIn, flightTest);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateFlightLandingBeforeDepartException()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            LoginToken<AirlineCompany> airlineLoggenIn = new LoginToken<AirlineCompany>();
            airlineLoggenIn.user = elal;

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            flightTest.LandingTime = new DateTime((DateTime.Now.Year + 1), 12, 7, 14, 00, 00);
            facade.UpdateFlight(airlineLoggenIn, flightTest);
        }
    }
}
