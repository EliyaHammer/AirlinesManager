using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.FacadeTests
{
    [TestClass]
    public class LoggenInCustomerFacadeTests
    {
        AirlineDAOMSSQL airlineDAO;
        TicketDAOMSSQL ticketDAO;
        FlightDAOMSSQL flightDAO;
        AdministratorDAOMSSQL administratorDAO;
        CustomerDAOMSSQL customerDAO;
        CountryDAOMSSQL countryDAO;
        LoggedInCustomerFacadeMSSQL facade;

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

            facade = new LoggedInCustomerFacadeMSSQL();
        }

        //CANCELL TICKET METHOD TESTS:
        [TestMethod]
        public void CancelTicket()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            Flight flightOne = flightDAO.GetAll()[0];

            customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));
            customerDAO.Add(new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PNUMBER2", "CNUMBER2"));
            Customer customerOne = customerDAO.GetAll()[0];
            Customer customerTwo = customerDAO.GetAll()[1];

            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = customerOne;

            ticketDAO.Add(new Ticket(flightOne.ID, customerOne.ID));
            ticketDAO.Add(new Ticket(flightOne.ID, customerTwo.ID));
            Ticket ticketOne = ticketDAO.GetAll()[0];

            flightOne = flightDAO.GetAll()[0];

            Assert.AreEqual(28, flightOne.RemainingTickets);
            facade.CancelTicket(customerLoggedIn, ticketOne);
            flightOne = flightDAO.GetAll()[0];
            Assert.AreEqual(29, flightOne.RemainingTickets);
            Assert.AreEqual(1, ticketDAO.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CancelTicketNullTokenException()
        {
            LoginToken<Customer> customerLoggedIn = null;

            facade.CancelTicket(customerLoggedIn, new Ticket(1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CancelTicketNullTicketException()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();

            facade.CancelTicket(customerLoggedIn, null);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAccessabillityException))]
        public void CancelTicketWrongUserForTicketException()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            Flight flightOne = flightDAO.GetAll()[0];

            customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));
            customerDAO.Add(new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PNUMBER2", "CNUMBER2"));
            Customer customerOne = customerDAO.GetAll()[0];
            Customer customerTwo = customerDAO.GetAll()[1];

            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = customerOne;

            ticketDAO.Add(new Ticket(flightOne.ID, customerTwo.ID));
            Ticket ticket = ticketDAO.GetAll()[0];

            facade.CancelTicket(customerLoggedIn, ticket);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CancelTicketNullUserException()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();

            facade.CancelTicket(customerLoggedIn, new Ticket(1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CancelTicketIDZeroException()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));

            facade.CancelTicket(customerLoggedIn ,new Ticket(1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(TicketNotFoundException))]
        public void CancelTicketDoesnotExistException()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));

            Ticket theTicket = new Ticket(1, 1);
            theTicket.ID = 1;

            facade.CancelTicket(customerLoggedIn, theTicket);
        }

        //CHANGE MY PASSWORD METHOD TESTS:
        [TestMethod]
        public void ChangeMyPassword()
        {
            Customer user = customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));

            LoginToken<Customer> customerLoggenIn = new LoginToken<Customer>();
            customerLoggenIn.user = user;

            facade.ChangeMyPassword(customerLoggenIn, "PASSWORD", "NEWPASSWORD");
            Assert.AreEqual("NEWPASSWORD", customerLoggenIn.user.Password);
            Assert.AreEqual("NEWPASSWORD", customerDAO.Get(user.ID).Password);
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
            LoginToken<Customer> loggenCustomer = new LoginToken<Customer>();

            facade.ChangeMyPassword(loggenCustomer, "E", "t");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ChangeMyPasswordNullOldPasswordException()
        {
            Customer user = customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));

            LoginToken<Customer> customerLoggenIn = new LoginToken<Customer>();
            customerLoggenIn.user = user;

            facade.ChangeMyPassword(customerLoggenIn, null, "NEWPASSWORD");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ChangeMyPasswordNullNewPasswordException()
        {
            Customer user = customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));

            LoginToken<Customer> customerLoggenIn = new LoginToken<Customer>();
            customerLoggenIn.user = user;

            facade.ChangeMyPassword(customerLoggenIn, "PASSWORD", null);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void ChangeMyPasswordWrongPasswordException()
        {
            Customer user = customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));

            LoginToken<Customer> customerLoggenIn = new LoginToken<Customer>();
            customerLoggenIn.user = user;

            facade.ChangeMyPassword(customerLoggenIn, "WRONG", "NEWPASSWORD");
        }


        //GET ALL MY FLIGHTS METHOD TESTS:
        [TestMethod]
        public void GetAllMyFlights()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            Flight flightOne = flightDAO.GetAll()[0];

            customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));
            customerDAO.Add(new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PNUMBER2", "CNUMBER2"));
            Customer customerOne = customerDAO.GetAll()[0];
            Customer customerTwo = customerDAO.GetAll()[1];

            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = customerOne;

            ticketDAO.Add(new Ticket(flightOne.ID, customerOne.ID));
            ticketDAO.Add(new Ticket(flightOne.ID, customerTwo.ID));

            Assert.AreEqual(1, facade.GetAllMyFlights(customerLoggedIn).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllMyFlightsNullUserException()
        {
            LoginToken<Customer> customerLoggedIn = null;

            facade.GetAllMyFlights(customerLoggedIn);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllMyFlightsNullTokenException()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            
            facade.GetAllMyFlights(customerLoggedIn);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetAllMyFlightsIDZeroException()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER");

            facade.GetAllMyFlights(customerLoggedIn);
        }

        [TestMethod]
        public void GetAllMyFlightsNoFlights()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));

            Assert.AreEqual(null, facade.GetAllMyFlights(customerLoggedIn));
        }


        //PURCHASE TICKET METHOD TESTS:
        [TestMethod]
        public void PurchaseTicket()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            Flight flightOne = flightDAO.GetAll()[0];

            customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER"));
            Customer customerOne = customerDAO.GetAll()[0];

            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = customerOne;

            facade.PurchaseTicket(customerLoggedIn, flightOne);
            Assert.AreEqual(1, ticketDAO.GetAll().Count);
            Assert.AreEqual(flightOne.RemainingTickets - 1, flightDAO.GetAll()[0].RemainingTickets);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PurchaseTicketNullTokenException()
        {
            LoginToken<Customer> customerLoggedIn = null;

            facade.PurchaseTicket(customerLoggedIn, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PurchaseTicketNullUserException()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();

            facade.PurchaseTicket(customerLoggedIn, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PurchaseTicketNullFlightException()
        {
            LoginToken<Customer> customerLoggedIn = new LoginToken<Customer>();
            customerLoggedIn.user = new Customer();

            facade.PurchaseTicket(customerLoggedIn, null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void PurchaseTicketCustomerIDZeroException()
        {
            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            LoginToken<Customer> loggedInCustomer = new LoginToken<Customer>();
            loggedInCustomer.user = new Customer();

            facade.PurchaseTicket(loggedInCustomer, theFlight);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void PurchaseTicketFlightIDZeroException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            LoginToken<Customer> loggedInCustomer = new LoginToken<Customer>();
            loggedInCustomer.user = theCustomer;

            facade.PurchaseTicket(loggedInCustomer, new Flight());
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void PurchaseTicketAlreadyExistsException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            LoginToken<Customer> loggedInCustomer = new LoginToken<Customer>();
            loggedInCustomer.user = theCustomer;

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            ticketDAO.Add(new Ticket(theFlight.ID, theCustomer.ID));

            facade.PurchaseTicket(loggedInCustomer, theFlight);
        }

        [TestMethod]
        [ExpectedException(typeof(CustomerNotFoundException))]
        public void PurchaseTicketCustomerNotFoundException()
        {
            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            LoginToken<Customer> loggedInCustomer = new LoginToken<Customer>();
            loggedInCustomer.user = new Customer();
            loggedInCustomer.user.ID = 1;

            facade.PurchaseTicket(loggedInCustomer, theFlight);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public void PurchaseTicketFlightNotFoundException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            LoginToken<Customer> loggedInCustomer = new LoginToken<Customer>();
            loggedInCustomer.user = theCustomer;

            Flight theFlight = new Flight();
            theFlight.ID = 1;

            facade.PurchaseTicket(loggedInCustomer, theFlight);
        }

        [TestMethod]
        [ExpectedException(typeof(TicketNotFoundException))]
        public void PurchaseTicketNoTicketsLeftException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            LoginToken<Customer> loggedInCustomer = new LoginToken<Customer>();
            loggedInCustomer.user = theCustomer;

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 0, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            facade.PurchaseTicket(loggedInCustomer, theFlight);
        }
    }
}
