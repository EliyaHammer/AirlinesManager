using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.FacadeTests
{
    [TestClass]
    public class AnnonymousUserFacadeTests
    {
        AirlineDAOMSSQL airlineDAO;
        TicketDAOMSSQL ticketDAO;
        FlightDAOMSSQL flightDAO;
        AdministratorDAOMSSQL administratorDAO;
        CustomerDAOMSSQL customerDAO;
        CountryDAOMSSQL countryDAO;
        AnnonymousUserFacadeMSSQL facade;

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

            airlineDAO.RemoveAllReplica();
            ticketDAO.RemoveAllReplica();
            countryDAO.RemoveAllReplica();
            flightDAO.RemoveAllReplica();
            customerDAO.RemoveAllReplica();
            administratorDAO.RemoveAllReplica();

            facade = new AnnonymousUserFacadeMSSQL();
        }

        //GET ALL AIRLINE COMPANIES METHOD TESTS:
        [TestMethod]
        public void GetAllAirlineCompanies()
        {
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            Assert.AreEqual(2, facade.GetAllAirlineCompanies().Count);
        }

        [TestMethod]
        public void GetAllNoAirlines()
        {
            Assert.AreEqual(null, facade.GetAllAirlineCompanies());
        }


        //GET ALL FLIGHTS METHOD TESTS:
        [TestMethod]
        public void GetAllFlights()
        {
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            Assert.AreEqual(2, facade.GetAllFlights().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllFlightsNullDAOException()
        {
            flightDAO = null;

            Assert.AreEqual(2, facade.GetAllFlights().Count);
        }

        [TestMethod]
        public void GetAllNoFlightsFound()
        {
            Assert.AreEqual(null, facade.GetAllFlights());
        }


        //GET ALL FLIGHTS VACANCEY METHOD TESTS:
        [TestMethod]
        public void GetAllFlightsVacancy()
        {
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 12, FlightStatus.NotDeparted));
            Flight flightOne = flightDAO.GetAll()[0];
            Flight flightTwo = flightDAO.GetAll()[1];

            Assert.AreEqual(30, facade.GetAllFlightsVacancy()[flightOne]);
            Assert.AreEqual(12, facade.GetAllFlightsVacancy()[flightTwo]);
        }

        [TestMethod]
        public void GetAllFlightsVacancyNoFlight()
        {
            Assert.AreEqual(null, facade.GetAllFlightsVacancy());
        }


        //GET FLIGHT BY ID METHOD TESTS:
        [TestMethod]
        public void GetFlightByID()
        {
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            Flight filghtOne = flightDAO.GetAll()[0];
            Flight flightTwo = flightDAO.GetAll()[1];

            Assert.AreEqual(filghtOne, facade.GetFlightByID(filghtOne.ID));
            Assert.AreEqual(flightTwo, facade.GetFlightByID(flightTwo.ID));
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetFlightByIDZeroException()
        {
            facade.GetFlightByID(0);
        }

        [TestMethod]
        public void GetFlightNotFound()
        {
            Assert.AreEqual(null, facade.GetFlightByID(1));
        }


        //GET FLIGHTS BY AIRLINE METHOD TESTS:
        [TestMethod]
        public void GetFlightsByAirline ()
        {
            countryDAO.Add(new Country("Israel"));
            AirlineCompany elal = airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            Assert.AreEqual(2, facade.GetFlightsByAirline(elal.ID).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetFlightsByAirlineIDZeroException()
        {
            facade.GetFlightsByAirline(0);
        }

        [TestMethod]
        public void GetFlightsByAirlineNotFound()
        {
            Assert.AreEqual(null, facade.GetFlightsByAirline(1));
        }


        //GET FLIGHT BY DEPARTURE DATE AND TIME METHOD TESTS:
        [TestMethod]
        public void GetFlightByDepartureDateAndTime()
        {
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2, 12 , 0, 0), new DateTime(DateTime.Now.Year + 1, 12, 3, 12 , 0, 0), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2, 12, 0, 0), new DateTime(DateTime.Now.Year + 1, 12, 3, 12, 0, 0), 30, FlightStatus.NotDeparted));

            Assert.AreEqual(2, facade.GetFlightsByDepartureDateAndTime(new DateTime(DateTime.Now.Year + 1, 12, 2, 12, 0, 0)).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetFlightsByDepartureDateAndTimeInvalidDateException()
        {
            facade.GetFlightsByDepartureDateAndTime(new DateTime((DateTime.Now.Year - 1), 12, 5, 12, 0,0));
        }

        [TestMethod]
        public void GetFlightsByDepartureDateAndTimeNotFound()
        {
            Assert.AreEqual(null, facade.GetFlightsByDepartureDateAndTime(new DateTime((DateTime.Now.Year + 1), 12, 5, 12, 0, 0)));
        }


        //GET FLIGHT BY DEPARTURE DATE ONLY METHOD TESTS:
        [TestMethod]
        public void GetFlightByDepartureDate()
        {
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2, 12, 0 ,0), new DateTime(DateTime.Now.Year + 1, 12, 3, 12 ,0 ,0), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2, 12 , 0, 0), new DateTime(DateTime.Now.Year + 1, 12, 3, 12 , 0 ,0 ), 30, FlightStatus.NotDeparted));

            Assert.AreEqual(2, facade.GetFlightsByDepartureDate(new DateTime(DateTime.Now.Year + 1, 12, 2)).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetFlightsByDepartureDateInvalidDateException()
        {
            facade.GetFlightsByDepartureDate(new DateTime((DateTime.Now.Year - 1), 12, 5));
        }

        [TestMethod]
        public void GetFlightsByDepartureDateNotFound()
        {
            Assert.AreEqual(null, facade.GetFlightsByDepartureDate(new DateTime((DateTime.Now.Year + 1), 12, 5)));
        }


        //GET FLIGHTS BY LANDING DATE AND TIME METHOD TESTS:
        [TestMethod]
        public void GetFlightByLandingDateAndTime()
        { 
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2, 12, 0,0), new DateTime(DateTime.Now.Year + 1, 12, 3,12,0,0), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2, 12, 0, 0), new DateTime(DateTime.Now.Year + 1, 12, 3, 12, 0, 0), 30, FlightStatus.NotDeparted));

            Assert.AreEqual(2, facade.GetFlightsByLandingDateAndTime(new DateTime(DateTime.Now.Year + 1, 12, 3, 12, 0, 0)).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetFlightsByLandingDateAndTimeInvalidDateException()
        {
            facade.GetFlightsByLandingDateAndTime(new DateTime((DateTime.Now.Year - 1), 12, 5, 12, 0, 0));
        }

        [TestMethod]
        public void GetFlightsByLandingDateAndTimeNotFound()
        {
            Assert.AreEqual(null, facade.GetFlightsByLandingDateAndTime(new DateTime((DateTime.Now.Year + 1), 12, 5, 12, 0, 0)));
        }


        //GET FLIGHTS BY LANDING DATE ONLY METHOD TESTS:
        [TestMethod]
        public void GetFlightByLandingDate()
        {
            countryDAO.Add(new Country("Israel"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2, 12, 0, 0), new DateTime(DateTime.Now.Year + 1, 12, 3, 12, 0, 0), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2, 12, 0, 0), new DateTime(DateTime.Now.Year + 1, 12, 3, 12, 0, 0), 30, FlightStatus.NotDeparted));

            Assert.AreEqual(2, facade.GetFlightsByLandingDate(new DateTime(DateTime.Now.Year + 1, 12, 3)).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetFlightsByLandingDateInvalidDateException()
        {
            facade.GetFlightsByLandingDate(new DateTime((DateTime.Now.Year - 1), 12, 5));
        }

        [TestMethod]
        public void GetFlightsByLandingDateNotFound()
        {
            Assert.AreEqual(null, facade.GetFlightsByLandingDate(new DateTime((DateTime.Now.Year + 1), 12, 5)));
        }

        //GET FLIGHTS BY DESTINATION COUNTRY METHOD TESTS:
        [TestMethod]
        public void GetFlightByDestinationCountry()
        {
            countryDAO.Add(new Country("Israel"));
            countryDAO.Add(new Country("Spain"));
            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Spain").ID, countryDAO.GetCountryByName("Spain").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            Assert.AreEqual(1, facade.GetFlightsByDestinationCountry(countryDAO.GetCountryByName("Israel").ID).Count);
            Assert.AreEqual(1, facade.GetFlightsByDestinationCountry(countryDAO.GetCountryByName("Spain").ID).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetFlightsByDestinationCountryIDException()
        {
            facade.GetFlightsByDestinationCountry(0);
        }

        [TestMethod]
        public void GetFlightsByDestinationCountryNotFound()
        {
            Assert.AreEqual(null, facade.GetFlightsByDestinationCountry(1));
        }

        //GET FLIGHTS BY ORIGIN COUNTRY METHOD TESTS:
        [TestMethod]
        public void GetFlightByOriginCountry()
        {
            countryDAO.Add(new Country("Israel"));
            countryDAO.Add(new Country("Spain"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));
           
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Israel").ID, countryDAO.GetCountryByName("Israel").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));
            flightDAO.Add(new Flight(airlineDAO.GetAirlineByName("ELAL").ID, countryDAO.GetCountryByName("Spain").ID, countryDAO.GetCountryByName("Spain").ID, new DateTime(DateTime.Now.Year + 1, 12, 2), new DateTime(DateTime.Now.Year + 1, 12, 3), 30, FlightStatus.NotDeparted));

            Assert.AreEqual(1, facade.GetFlightsByOriginCountry(countryDAO.GetCountryByName("Israel").ID).Count);
            Assert.AreEqual(1, facade.GetFlightsByOriginCountry(countryDAO.GetCountryByName("Spain").ID).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetFlightsByOriginCountryIDException()
        {
            facade.GetFlightsByOriginCountry(0);
        }

        [TestMethod]
        public void GetFlightsByOriginCountryNotFound()
        {
            Assert.AreEqual(null, facade.GetFlightsByOriginCountry(1));
        }
    }
}
