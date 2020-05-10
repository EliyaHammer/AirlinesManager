using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.DAOTests
{
    [TestClass]
    public class TicketDAOMSSQLTests
    {
        AirlineDAOMSSQL airlineDAO;
        TicketDAOMSSQL ticketDAO;
        FlightDAOMSSQL flightDAO;
        AdministratorDAOMSSQL administratorDAO;
        CustomerDAOMSSQL customerDAO;
        CountryDAOMSSQL countryDAO;

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
        }


        //ADD METHOD TESTS:
        [TestMethod]
        public void Add()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

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
            Assert.AreEqual(1, ticketDAO.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddNullException()
        {
            ticketDAO.Add(null);
        }


        //GET METHOD TESTS: > not used
        [TestMethod]
        public void Get()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            Assert.AreEqual(theTicket, ticketDAO.Get(theTicket.ID));
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetIDZeroException()
        {
            ticketDAO.Get(0);
        }

        [TestMethod]
        public void GetTicketNotFoundException()
        {
            Assert.AreEqual(null, ticketDAO.Get(1));
        }


        //GET ALL METHOD TESTS: > not used
        [TestMethod]
        public void GetAll()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);

            Assert.AreEqual(1, ticketDAO.GetAll().Count);
        }

        [TestMethod]
        public void GetAllNoTicketsException()
        {
            Assert.AreEqual(null, ticketDAO.GetAll());
        }


        //GET TICKETS BY AIRLINE METHOD TESTS:
        [TestMethod]
        public void GetTicketsByAirline()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSWORD", israel.ID));
            elal = airlineDAO.GetAll()[0];
            AirlineCompany arkia = airlineDAO.GetAll()[1];

            Flight theFlightOne = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlightOne);
            theFlightOne = flightDAO.GetAll()[0];
            Flight theFlightTwo = new Flight(arkia.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlightTwo);
            theFlightTwo = flightDAO.GetAll()[1];

            Ticket theTicket = new Ticket(theFlightOne.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            Ticket theTicketTwo = new Ticket(theFlightTwo.ID, theCustomer.ID);
            ticketDAO.Add(theTicketTwo);

            Assert.AreEqual(1, ticketDAO.GetTicketsByAirline(elal.ID).Count);
            Assert.AreEqual(1, ticketDAO.GetTicketsByAirline(arkia.ID).Count);
        }

        [TestMethod]
        public void GetTicketByAirlineNotFoundException()
        {
            Assert.AreEqual(null, ticketDAO.GetTicketsByAirline(1));
        }


        //GET TICKETS BY CUSTOMER METHOD TESTS: > not used
        [TestMethod]
        public void GetTicketsByCustomer()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            Assert.AreEqual(theTicket, ticketDAO.GetTicketsByCustomer(theCustomer)[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetTicketsByCustomerNullException()
        {
            ticketDAO.GetTicketsByCustomer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetTicketsByCustomerIDZeroException()
        {
            ticketDAO.GetTicketsByCustomer(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER"));
        }

        [TestMethod]
        public void GetTicketsByCustomerNotFoundException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            theCustomer.ID = 1;

            Assert.AreEqual(null, ticketDAO.GetTicketsByCustomer(theCustomer));
        }


        //GET TICKETS BY FLIGHT METHOD TESTS: > not used
        [TestMethod]
        public void GetTicketsByFlight()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            Assert.AreEqual(theTicket, ticketDAO.GetTicketsByFlight(theFlight)[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetTicketsByFlightNullException()
        {
            ticketDAO.GetTicketsByFlight(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetTicketsByFlightIDZeroException()
        {
            Flight theFlight = new Flight(1, 1, 1, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);

            ticketDAO.GetTicketsByFlight(theFlight);
        }

        [TestMethod]
        public void GetTicketsByFlightNotFoundException()
        {
            Flight theFlight = new Flight(1, 1, 1, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            theFlight.ID = 1;

            Assert.AreEqual(null, ticketDAO.GetTicketsByFlight(theFlight));
        }


        //REMOVE METHOD TESTS:
        [TestMethod]
        public void Remove()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            Assert.AreEqual(1, ticketDAO.GetAll().Count);
            ticketDAO.Remove(theTicket);
            Assert.AreEqual(null, ticketDAO.GetAll());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveNullException()
        {
            ticketDAO.Remove(null);
        }


        //UPDATE METHOD TESTS: > not used
        [TestMethod]
        public void Update()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            Customer theCustomerTwo = new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHNUMBER2", "CRDNUMBER2");
            customerDAO.Add(theCustomerTwo);
            theCustomerTwo = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            theTicket.CustomerID = theCustomerTwo.ID;
            ticketDAO.Update(theTicket);

            Assert.AreEqual(theCustomerTwo.ID, ticketDAO.Get(theTicket.ID).CustomerID);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNullException()
        {
            ticketDAO.Update(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateIDZeroException()
        {
            ticketDAO.Update(new Ticket(1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void UpdateAlreadyExistsException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            Customer theCustomerTwo = new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHNUMBER2", "CRDNUMBER2");
            customerDAO.Add(theCustomerTwo);
            theCustomerTwo = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];
            ticketDAO.Add(new Ticket(theFlight.ID, theCustomerTwo.ID));

            theTicket.CustomerID = theCustomerTwo.ID;
            ticketDAO.Update(theTicket);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateCustomerIDZeroException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            Customer theCustomerTwo = new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHNUMBER2", "CRDNUMBER2");
            customerDAO.Add(theCustomerTwo);
            theCustomerTwo = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            theTicket.CustomerID = 0;
            ticketDAO.Update(theTicket);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateFlightIDZeroException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            Customer theCustomerTwo = new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHNUMBER2", "CRDNUMBER2");
            customerDAO.Add(theCustomerTwo);
            theCustomerTwo = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            theTicket.FlightID = 0;
            ticketDAO.Update(theTicket);
        }

        [TestMethod]
        [ExpectedException(typeof(TicketNotFoundException))]
        public void UpdateTicketDoesntExistException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            Customer theCustomerTwo = new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHNUMBER2", "CRDNUMBER2");
            customerDAO.Add(theCustomerTwo);
            theCustomerTwo = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            ticketDAO.Remove(theTicket);
            ticketDAO.Update(theTicket);
        }

        [TestMethod]
        [ExpectedException(typeof(CustomerNotFoundException))]
        public void UpdateCustomerDoesntExistException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            Customer theCustomerTwo = new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHNUMBER2", "CRDNUMBER2");
            customerDAO.Add(theCustomerTwo);
            theCustomerTwo = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            theTicket.CustomerID = theCustomerTwo.ID + 3;
            ticketDAO.Update(theTicket);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public void UpdateFlightDoesntExistException()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            Customer theCustomerTwo = new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHNUMBER2", "CRDNUMBER2");
            customerDAO.Add(theCustomerTwo);
            theCustomerTwo = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            theTicket.FlightID = theFlight.ID + 2;
            ticketDAO.Update(theTicket);
        }


        //CHECK UNIQUE FIELDS EXISTANC METHOD TESTS:
        [TestMethod]
        public void CheckUniqueFieldsExistance()
        {
            Customer theCustomer = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHNUMBER", "CRDNUMBER");
            customerDAO.Add(theCustomer);
            theCustomer = customerDAO.GetAll()[0];
            Customer theCustomerTwo = new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHNUMBER2", "CRDNUMBER2");
            customerDAO.Add(theCustomerTwo);
            theCustomerTwo = customerDAO.GetAll()[0];

            Country israel = new Country("Israel");
            countryDAO.Add(israel);
            israel = countryDAO.GetAll()[0];

            AirlineCompany elal = new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSWORD", israel.ID);
            airlineDAO.Add(elal);
            elal = airlineDAO.GetAll()[0];

            Flight theFlight = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(theFlight);
            theFlight = flightDAO.GetAll()[0];

            Ticket theTicket = new Ticket(theFlight.ID, theCustomer.ID);
            ticketDAO.Add(theTicket);
            theTicket = ticketDAO.GetAll()[0];

            Ticket notAdded = new Ticket(theFlight.ID, theCustomer.ID);
            notAdded.ID = theTicket.ID + 1;

            Assert.AreEqual(true, ticketDAO.CheckTicketUniqueFieldsExistance(notAdded));
            Assert.AreEqual(false, ticketDAO.CheckTicketUniqueFieldsExistance(theTicket));

        }

        [TestMethod]
        public void CheckUniqueFieldsExistanceCustomerIDZero()
        {
            Assert.AreEqual(false, ticketDAO.CheckTicketUniqueFieldsExistance(new Ticket(1, 0)));
        }

        [TestMethod]
        public void CheckUniqueFieldsExistanceFlightIDZero()
        {
            Assert.AreEqual(false, ticketDAO.CheckTicketUniqueFieldsExistance(new Ticket(0, 1)));
        }

        [TestMethod]
        public void CheckUniqueFieldsExistanceTicketNull()
        {
            Assert.AreEqual(false, ticketDAO.CheckTicketUniqueFieldsExistance(null));
        }
    }
}
