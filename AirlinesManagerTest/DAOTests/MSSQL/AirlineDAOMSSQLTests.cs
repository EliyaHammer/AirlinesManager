using System;
using System.Collections.Generic;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.DAOTests
{
    [TestClass]
    public class AirlineDAOMSSQLTests
    {
        AirlineDAOMSSQL airlineDAO;
        TicketDAOMSSQL ticketDAO;
        FlightDAOMSSQL flightDAO;
        AdministratorDAOMSSQL administratorDAO;
        CustomerDAOMSSQL customerDAO;
        CountryDAOMSSQL countryDAO;

        //before every test: initialize the conn string and remove all replica.
        [TestInitialize]
        public void TestInitialize ()
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
        }

        //ADD METHID TESTS:
        [TestMethod]
        public void Add()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ElAl", "ElAl2004", "123456", countryDAO.GetCountryByName("Israel").ID));
            Assert.AreEqual("ElAl", airlineDAO.GetAirlineByName("ElAl").AirLineName);
            Assert.AreEqual("ElAl2004", airlineDAO.GetAirlineByName("ElAl").UserName);
            Assert.AreEqual("123456", airlineDAO.GetAirlineByName("ElAl").Password);
            Assert.AreEqual(countryDAO.GetCountryByName("Israel").ID, airlineDAO.GetAirlineByName("ElAl").CountryCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddNullAirlineException()
        {
            airlineDAO.Add(null);
        }

        //GET METHOD TESTS:
        [TestMethod]
        public void Get()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany(TestData.AirlineDAOMSSQLTests_Get_AirlineName, "ELAL123", "PASSWORD", countryDAO.GetCountryByName("Israel").ID));
            Assert.AreEqual(TestData.AirlineDAOMSSQLTests_Get_AirlineName, airlineDAO.Get(airlineDAO.GetAirlineByName("ELAL").ID).AirLineName);
            Assert.AreEqual("ELAL123", airlineDAO.Get(airlineDAO.GetAirlineByName("ELAL").ID).UserName);
            Assert.AreEqual("PASSWORD", airlineDAO.Get(airlineDAO.GetAirlineByName("ELAL").ID).Password);
            Assert.AreEqual(countryDAO.GetCountryByName("Israel").ID, airlineDAO.Get(airlineDAO.GetAirlineByName("ELAL").ID).CountryCode);
        }

        //GET BY NAME TESTS: > not used.
        [TestMethod]
        public void GetAirlineByName()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELAL123", "PASSWORD", countryDAO.GetCountryByName("Israel").ID));
            Assert.AreEqual("ELAL", airlineDAO.GetAirlineByName("ELAL").AirLineName);
            Assert.AreEqual("ELAL123", airlineDAO.GetAirlineByName("ELAL").UserName);
            Assert.AreEqual("PASSWORD", airlineDAO.GetAirlineByName("ELAL").Password);
            Assert.AreEqual(countryDAO.GetCountryByName("Israel").ID, airlineDAO.GetAirlineByName("ELAL").CountryCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAirlineByNameNullNameException()
        {
            airlineDAO.GetAirlineByName(null);
        }

        [TestMethod]
        public void GetAirlineByNamenNotFound()
        {
            Assert.AreEqual(null, airlineDAO.GetAirlineByName("ELAL"));
        }

        //GET BY USERNAME TESTS: > not used.
        [TestMethod]
        public void GetAirlineByUserName()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELAL123", "PASSWORD", countryDAO.GetCountryByName("Israel").ID));
            Assert.AreEqual("ELAL", airlineDAO.GetAirlineByUsername("ELAL123").AirLineName);
            Assert.AreEqual("ELAL123", airlineDAO.GetAirlineByUsername("ELAL123").UserName);
            Assert.AreEqual("PASSWORD", airlineDAO.GetAirlineByUsername("ELAL123").Password);
            Assert.AreEqual(countryDAO.GetCountryByName("Israel").ID, airlineDAO.GetAirlineByUsername("ELAL123").CountryCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAirlineByUsernameNullNameException()
        {
            airlineDAO.GetAirlineByUsername(null);
        }

        [TestMethod]
        public void GetAirlineByUsernamenNotFound()
        {
            Assert.AreEqual(null, airlineDAO.GetAirlineByName("ELAL"));
        }

        //GET AIRLINES BY COUNTRY TESTS: > not used
        [TestMethod]
        public void GetAirlinesByCountry()
        {
            countryDAO.Add(new Country("Israel"));
            Country israel = countryDAO.GetCountryByName("Israel");

            airlineDAO.Add(new AirlineCompany("ELAL", "ELAL123", "PASSWORD", israel.ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIA123", "ARKIAPASSWORD", israel.ID));

            List<AirlineCompany> airlinesList = (List<AirlineCompany>)airlineDAO.GetAirlinesByCountry(israel.ID);

            Assert.AreEqual(2, airlinesList.Count);
            Assert.AreEqual("ELAL", airlinesList[0].AirLineName);
            Assert.AreEqual("ARKIA", airlinesList[1].AirLineName);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetAirlinesByCountryIDZeroException()
        {
            airlineDAO.GetAirlinesByCountry(0);
        }

        [TestMethod]
        public void GetAirlinesByCountryAirlineNotFound()
        {
            airlineDAO.GetAirlinesByCountry(2);
        }

        [TestMethod]
        public void GetAirlinesByCountryAirlineNotFoundExceptionTwo()
        {
            countryDAO.Add(new Country("Israel"));

            Assert.AreEqual(null, airlineDAO.GetAirlinesByCountry(countryDAO.GetCountryByName("Israel").ID));
        }

        //GET ALL TESTS:
        [TestMethod]
        public void GetAll()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID));
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIA123", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));

            List<AirlineCompany> airlinesList = airlineDAO.GetAll();

            Assert.AreEqual(2, airlinesList.Count);
            Assert.AreEqual("ELAL", airlinesList[0].AirLineName);
            Assert.AreEqual("ARKIA", airlinesList[1].AirLineName);
        }

        [TestMethod]
        public void GetAllNoAirlines()
        {
            Assert.AreEqual(null, airlineDAO.GetAll());
        }

        //REMOVE METHOD TESTS:
        [TestMethod]
        public void Remove()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");
            airlineDAO.Remove(theAirline);

            Assert.AreEqual(null, airlineDAO.GetAirlineByName("ELAL"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveNullException()
        {
            airlineDAO.Remove(null);
        }

        //UPDATE METHOD TESTS:
        [TestMethod]
        public void Update ()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");
            theAirline.AirLineName = "ARKIA";
            airlineDAO.Update(theAirline);

            Assert.AreEqual("ARKIA", airlineDAO.Get(theAirline.ID).AirLineName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateAirlineNullException()
        {
            airlineDAO.Update(null);
        }

        //CHECK USERNAME EXISTANCE METHOD TESTS:
        [TestMethod]
        public void CheckUsernameExistance ()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany airlineOne = new AirlineCompany("NAME", "USERNAME", "PASSWORD", countryDAO.GetCountryByName("Israel").ID);
            AirlineCompany airlineTwo = new AirlineCompany("NAME2", "USERNAME2", "PASSWORD2", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(airlineOne);
            airlineDAO.Add(airlineTwo);
            airlineOne = airlineDAO.GetAirlineByName("NAME");
            airlineTwo = airlineDAO.GetAirlineByName("NAME2");

            Assert.AreEqual(false, airlineDAO.CheckUsernameExistance("USERNAME", airlineOne.ID));
            Assert.AreEqual(true, airlineDAO.CheckUsernameExistance("USERNAME", airlineTwo.ID));
        }

        [TestMethod]
        public void CheckUsernameExistanceNull()
        {
            Assert.AreEqual(false, airlineDAO.CheckUsernameExistance(null, 1));
        }

        //CHECK NAME EXISTANCE METHOD TESTS:
        [TestMethod]
        public void CheckNameExistance()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany airlineOne = new AirlineCompany("NAME", "USERNAME", "PASSWORD", countryDAO.GetCountryByName("Israel").ID);
            AirlineCompany airlineTwo = new AirlineCompany("NAME2", "USERNAME2", "PASSWORD2", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(airlineOne);
            airlineDAO.Add(airlineTwo);
            airlineOne = airlineDAO.GetAirlineByName("NAME");
            airlineTwo = airlineDAO.GetAirlineByName("NAME2");

            Assert.AreEqual(false, airlineDAO.CheckNameExistance("NAME", airlineOne.ID));
            Assert.AreEqual(true, airlineDAO.CheckNameExistance("NAME", airlineTwo.ID));
        }

        [TestMethod]
        public void CheckNameExistanceNull()
        {
            Assert.AreEqual(false, airlineDAO.CheckNameExistance(null, 1));
        }

    }
}
