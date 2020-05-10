using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.DAOTests
{
    [TestClass]
    public class CountryDAOMSSQLTests
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

            airlineDAO.RemoveAllReplica();
            ticketDAO.RemoveAllReplica();
            countryDAO.RemoveAllReplica();
            flightDAO.RemoveAllReplica();
            customerDAO.RemoveAllReplica();
            administratorDAO.RemoveAllReplica();
        }

        //ADD METHOD TESTS:
        [TestMethod]
        public void Add()
        {
            countryDAO.Add(new Country("Israel"));
            Assert.AreEqual(1, countryDAO.GetAll().Count);
            Assert.AreEqual("Israel", countryDAO.GetAll()[0].CountryName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddNameNullException()
        {
            countryDAO.Add(null);
        }


        //GET METHOD TESTS: > not used.
        [TestMethod]
        public void Get()
        {
            Country addedCountry = new Country("Israel");
            countryDAO.Add(addedCountry);
            addedCountry = countryDAO.GetCountryByName("Israel");

            Assert.AreEqual("Israel", countryDAO.Get(addedCountry.ID).CountryName);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetIDZeroException()
        {
            countryDAO.Get(0);
        }

        [TestMethod]
        public void GetNotFoundException()
        {
            Assert.AreEqual(null, countryDAO.Get(2));
        }

        //GET ALL METHOS TESTS: not used.
        [TestMethod]
        public void GetAll()
        {
            countryDAO.Add(new Country("Israel"));
            countryDAO.Add(new Country("India"));

            Assert.AreEqual(2, countryDAO.GetAll().Count);
            Assert.AreEqual("Israel", countryDAO.GetAll()[0].CountryName);
            Assert.AreEqual("India", countryDAO.GetAll()[1].CountryName);
        }
        
        [TestMethod]
        public void GetAllNoCountries ()
        {
            Assert.AreEqual(null, countryDAO.GetAll());
        }

        //GET COUNTRY BY NAME METHOD TESTS: > not used
        [TestMethod]
        public void GetCountryByName()
        {
            Country addedCountry = new Country("Israel");
            countryDAO.Add(addedCountry);
            addedCountry = countryDAO.GetCountryByName("Israel");

            Assert.AreEqual(addedCountry.ID, countryDAO.GetCountryByName("Israel").ID);
            Assert.AreEqual(addedCountry.CountryName, countryDAO.GetCountryByName("Israel").CountryName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetCountryByNameNullException()
        {
            countryDAO.GetCountryByName(null);
        }

        [TestMethod]
        public void GetCountryByNameNotFound ()
        {
            Assert.AreEqual(null, countryDAO.GetCountryByName("Israel"));
        }

        //REMOVE METHOD TESTS:
        [TestMethod]
        public void Remove()
        {
            Country testCountry = new Country("Israel");
            countryDAO.Add(testCountry);
            countryDAO.Add(new Country("India"));
            testCountry = countryDAO.GetCountryByName("Israel");

            countryDAO.Remove(testCountry);
            Assert.AreEqual(false, countryDAO.GetAll().Contains(testCountry));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveNullException()
        {
            countryDAO.Remove(null);
        }


        //UPDATE METHOD TESTS:
        [TestMethod]
        public void Update()
        {
            Country countryTest = new Country("Israel");
            countryDAO.Add(countryTest);
            countryTest = countryDAO.GetCountryByName("Israel");

            countryTest.CountryName = "India";
            countryDAO.Update(countryTest);

            Assert.AreEqual(countryTest.ID, countryDAO.GetCountryByName("India").ID);
            Assert.AreEqual("India", countryDAO.Get(countryTest.ID).CountryName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNullException()
        {
            countryDAO.Update(null);
        }


        //CHECK NAME EXISTRANCE METHOD TESTS:
        [TestMethod]
        public void CheckNameExistance()
        {
            Country countryOne = new Country("Israel");
            Country countryTwo = new Country("Spain");
            countryDAO.Add(countryOne);
            countryDAO.Add(countryTwo);

            countryOne = countryDAO.GetCountryByName("Israel");
            countryTwo = countryDAO.GetCountryByName("Spain");

            Assert.AreEqual(true, countryDAO.CheckNameExistance("Israel", countryTwo.ID));
            Assert.AreEqual(false, countryDAO.CheckNameExistance("Israel", countryOne.ID));
        }

        [TestMethod]
        public void CheckNameExistanceNull()
        {
            Assert.AreEqual(false, countryDAO.CheckNameExistance(null, 1));
        }

    }

}

