using System;
using System.Collections.Generic;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.DAOTests
{
    [TestClass]
    public class FlightDAOMSSQLTests
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
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50 , FlightStatus.NotDeparted));
            Assert.AreEqual(1, flightDAO.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddNullException()
        {
            flightDAO.Add(null);
        }


        //GET METHOD TESTS:
        [TestMethod]
        public void Get()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");



            Flight flightTest = new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted);
            flightDAO.Add(flightTest);
            flightTest = flightDAO.GetAll()[0];

            Assert.AreEqual(flightTest, flightDAO.Get(flightTest.ID));
        }

        [TestMethod]
        public void GetFlightNotFound()
        {
            Assert.AreEqual(null, flightDAO.Get(1));
        }

        //GET ALL METHOD TESTS:
        [TestMethod]
        public void GetAll()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");



            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Assert.AreEqual(1, flightDAO.GetAll().Count);
        }

        [TestMethod]
        public void GetAllNoFlightsFound()
        {
            Assert.AreEqual(null, flightDAO.GetAll());
        }


        //GET ALL FLIGHTS VACANCEY TESTS:
        [TestMethod]
        public void GetAllFlightsVacancy()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");



            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 20, FlightStatus.NotDeparted));

            Dictionary<Flight, int> vacancy = flightDAO.GetAllFlightsVacancy();
            Assert.AreEqual(50, vacancy[flightDAO.GetAll()[0]]);
            Assert.AreEqual(20, vacancy[flightDAO.GetAll()[1]]);
        }

        [TestMethod]
        public void GetAllFlightsVacancyNoFlight()
        {
            Assert.AreEqual(null, flightDAO.GetAllFlightsVacancy());
        }


        //GET FLIGHTS BY CUSTOMER METHOD TESTS: 
        [TestMethod]
        public void GetFlightsByCustomer()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 20, FlightStatus.NotDeparted));

            customerDAO.Add(new Customer("FIRSTNAME", "LAST NAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBE", "CARDNUMBER"));
            Customer customerTest = customerDAO.GetAll()[0];

            ticketDAO.Add(new Ticket(flightDAO.GetAll()[0].ID, customerTest.ID));
            ticketDAO.Add(new Ticket(flightDAO.GetAll()[1].ID, customerTest.ID));

            List<Flight> customerFlights = (List<Flight>)flightDAO.GetFlightsByCustomer(customerTest);
            Assert.AreEqual(2, customerFlights.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetFlightsByCustomerNullException()
        {
            flightDAO.GetFlightsByCustomer(null);
        }

        [TestMethod]
        public void GetFlightsByCustomerNoFlights()
        {
            Customer theCustomer = new Customer("FIRST", "LAST", "USERNAME", "PASSWORD", "ADDRESS", "PNUMBER", "CNUMBER");
            theCustomer.ID = 1;
            Assert.AreEqual(null, flightDAO.GetFlightsByCustomer(theCustomer));
        }


        //GET FLIGHTS BY DEPARTURE DATE AND TIME TESTS:
        [TestMethod]
        public void GetFlightsByDepartureDateAndTime()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            Assert.AreEqual(flightTest, flightDAO.GetFlightsByDeprtureDateAndTime(new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00))[0]);
        }

        [TestMethod]
        public void GetFlightsByDepartureDateAndTimeNotFound()
        {
            Assert.AreEqual(null, flightDAO.GetFlightsByDeprtureDateAndTime(new DateTime((DateTime.Now.Year + 1), 12, 5, 12, 0, 0)));
        }


        //GET FLIGHTS BY DESTINATION COUNTRY METHOD TESTS:
        [TestMethod]
        public void GetFlightsByDestinationCountry()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            Assert.AreEqual(flightTest, flightDAO.GetFlightsByDestinationCountry(israel.ID)[0]);
        }

        [TestMethod]
        public void GetFlightsByDestinationCountryNotFound()
        {
            Assert.AreEqual(null, flightDAO.GetFlightsByDestinationCountry(1));
        }


        //GET FLIGHTS BY LANDING DATE AND TIME:
        [TestMethod]
        public void GetFlightsByLandingDateAndTime()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            Assert.AreEqual(flightTest, flightDAO.GetFlightsByLandingDateAndTime(new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00))[0]);
        }

        [TestMethod]
        public void GetFlightsByLandingDateAndTimeNotFound()
        {
            Assert.AreEqual(null, flightDAO.GetFlightsByLandingDateAndTime(new DateTime((DateTime.Now.Year + 1), 12, 5, 12, 0, 0)));
        }


        //GET FLIGHTS BY ORIGIN COUNTRY METHOD TESTS:
        [TestMethod]
        public void GetFlightsByOriginCountry()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            Assert.AreEqual(flightTest, flightDAO.GetFlightsByDestinationCountry(israel.ID)[0]);
        }

        [TestMethod]
        public void GetFlightsByOriginCountryNotFoundException()
        {
            Assert.AreEqual(null, flightDAO.GetFlightsByOriginCountry(1));
        }


        //REMOVE TESTS METHOD:
        [TestMethod]
        public void Remove()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");



            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];
            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));

            flightDAO.Remove(flightTest);

            Assert.AreEqual(1, flightDAO.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveNullException()
        {
            flightDAO.Remove(null);
        }


        //UPDATE METHOD TESTS:
        [TestMethod]
        public void Update()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            flightTest.RemainingTickets = 5;
            flightDAO.Update(flightTest);
            Assert.AreEqual(5, flightDAO.Get(flightTest.ID).RemainingTickets);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNullException()
        {
            flightDAO.Update(null);
        }

        //GET FLIGHTS BY AIRLINE METHOD TESTS:
        [TestMethod]
        public void GetFlightsByAirline()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");
            airlineDAO.Add(new AirlineCompany("ARKIA", "USERNAME1", "PASSWORD2", israel.ID));



            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));

            Assert.AreEqual(2, flightDAO.GetFlightsByAirline(elal.ID).Count);
        }

        [TestMethod]
        public void GetFlightsByAirlineNotFoundException()
        {
            Assert.AreEqual(null, flightDAO.GetFlightsByAirline(1));
        }


        //DEPARTURE DATE ONLY :
        [TestMethod]
        public void GetFlightsByDepartureDate()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            Assert.AreEqual(flightTest, flightDAO.GetFlightsByDeprtureDate(new DateTime((DateTime.Now.Year + 2), 12, 5))[0]);
        }

        [TestMethod]
        public void GetFlightsByDepartureDateNotFound()
        {
            Assert.AreEqual(null, flightDAO.GetFlightsByDeprtureDate(new DateTime((DateTime.Now.Year + 1), 12, 09)));
        }

        //LANDING DATE ONLY :
        [TestMethod]
        public void GetFlightsByLandingDate()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "USERNAME", "PASSWORD", israel.ID));
            AirlineCompany elal = airlineDAO.GetAirlineByName("ELAL");

            flightDAO.Add(new Flight(elal.ID, israel.ID, israel.ID, new DateTime((DateTime.Now.Year + 2), 12, 5, 14, 00, 00), new DateTime((DateTime.Now.Year + 2), 12, 7, 14, 00, 00), 50, FlightStatus.NotDeparted));
            Flight flightTest = flightDAO.GetAll()[0];

            Assert.AreEqual(flightTest, flightDAO.GetFlightsByLandingDate(new DateTime((DateTime.Now.Year + 2), 12, 7))[0]);
        }

        [TestMethod]
        public void GetFlightsByLandingDateNotFound()
        {
            Assert.AreEqual(null, flightDAO.GetFlightsByLandingDate(new DateTime((DateTime.Now.Year + 1), 12, 09)));
        }
    }
}
