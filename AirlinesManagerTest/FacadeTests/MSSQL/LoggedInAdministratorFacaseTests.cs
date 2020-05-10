using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using AirLinesManager.Facades.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.FacadeTests
{
    [TestClass]
    public class LoggedInAdministratorFacaseTests
    {

        AirlineDAOMSSQL airlineDAO;
        TicketDAOMSSQL ticketDAO;
        FlightDAOMSSQL flightDAO;
        AdministratorDAOMSSQL administratorDAO;
        CustomerDAOMSSQL customerDAO;
        CountryDAOMSSQL countryDAO;
        LoggedInAdministratorFacadeMSSQL facade;

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

            facade = new LoggedInAdministratorFacadeMSSQL();
        }

        //ADD AIRLINE METHOD TESTS:
        [TestMethod]
        public void CreateNewAirline()
        {
            countryDAO.Add(new Country("Israel"));

            facade.CreateNewAirline(new AirlineCompany("ElAl", "ElAl2004", "123456", countryDAO.GetCountryByName("Israel").ID));
            Assert.AreEqual("ElAl", airlineDAO.GetAirlineByName("ElAl").AirLineName);
            Assert.AreEqual("ElAl2004", airlineDAO.GetAirlineByName("ElAl").UserName);
            Assert.AreEqual("123456", airlineDAO.GetAirlineByName("ElAl").Password);
            Assert.AreEqual(countryDAO.GetCountryByName("Israel").ID, airlineDAO.GetAirlineByName("ElAl").CountryCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateNewAirlineNullAirlineException()
        {
            facade.CreateNewAirline(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateNewAirlineNullFieldExceptionOne() 
        {
            AirlineCompany newAirline = new AirlineCompany(null, "USER", "PASSWORD", 1);
            facade.CreateNewAirline(newAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateNewAirlineNullFieldExceptionTwo()
        {
            AirlineCompany newAirline = new AirlineCompany("NAME", null, "PASSWORD", 1);
            facade.CreateNewAirline(newAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateNewAirlineNullFieldExceptionThree()
        {
            AirlineCompany newAirline = new AirlineCompany("NAME", "USER", null, 1);
            facade.CreateNewAirline(newAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateNewAirlineNullCountryCodeZero()
        {
            AirlineCompany newAirline = new AirlineCompany("NAME", "USER", "PASSWORD", 0);
            facade.CreateNewAirline(newAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void CreateNewAirlineUsernameExistsException()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany newAirline = new AirlineCompany("EL AL", "ELAL1234", "ELAL2005", countryDAO.GetCountryByName("Israel").ID);
            facade.CreateNewAirline(newAirline);

            AirlineCompany secondAirline = new AirlineCompany("ARKIA", "ELAL1234", "PASSWORD", countryDAO.GetCountryByName("Israel").ID);
            facade.CreateNewAirline(secondAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void CreateNewAirlineNameExistsException()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany newAirline = new AirlineCompany("EL AL", "ELAL1234", "ELAL2005", countryDAO.GetCountryByName("Israel").ID);
            facade.CreateNewAirline(newAirline);

            AirlineCompany secondAirline = new AirlineCompany("EL AL", "ELAL4", "PASSWORD", countryDAO.GetCountryByName("Israel").ID);
            facade.CreateNewAirline(secondAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(CountryNotFoundException))]
        public void CreateNewAirlineCountryNotFoundException()
        {
            AirlineCompany newAirline = new AirlineCompany("EL AL", "ELAL1234", "ELAL2005", 2);
            facade.CreateNewAirline(newAirline);
        }

        //REMOVE AIRLINE METHOD TESTS:
        [TestMethod]
        public void RemoveAirline()
        {
            facade.CreateNewCountry(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            facade.CreateNewAirline(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");
            facade.RemoveAirline(theAirline);

            Assert.AreEqual(null, airlineDAO.GetAirlineByName("ELAL"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveAirlineNullException()
        {
            facade.RemoveAirline(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void RemoveAirlineIDZeroException()
        {
            facade.RemoveAirline(new AirlineCompany("NAME", "USERNAME", "PASSWORD", 1));
        }

        [TestMethod]
        [ExpectedException(typeof(AirlineNotFoundException))]
        public void RemoveAirlineNotFoundException()
        {
            AirlineCompany theAirline = new AirlineCompany("NAME", "USERNAME", "PASSWORD", 1);
            theAirline.ID = 1;
            facade.RemoveAirline(theAirline);
        }


        //ADD COUNTRY METHOD TESTS: 
        [TestMethod]
        public void CreateNewCountry()
        {
            facade.CreateNewCountry(new Country("Israel"));
            Assert.AreEqual(1, countryDAO.GetAll().Count);
            Assert.AreEqual("Israel", countryDAO.GetAll()[0].CountryName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateNewCountryNullException()
        {
            facade.CreateNewCountry(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateNewCountryNameNullException()
        {
            facade.CreateNewCountry(new Country(null));
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void CreateNewCountryNameExistsException()
        {
            facade.CreateNewCountry(new Country("Israel"));
            facade.CreateNewCountry(new Country("Israel"));
        }


        //REMOVE COUNTRY METHOD TESTS:
        [TestMethod]
        public void RemoveCountry()
        {
            Country testCountry = new Country("Israel");
            countryDAO.Add(testCountry);
            countryDAO.Add(new Country("India"));
            testCountry = countryDAO.GetCountryByName("Israel");

            facade.RemoveCountry(testCountry);
            Assert.AreEqual(false, countryDAO.GetAll().Contains(testCountry));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveCountryNullException()
        {
            facade.RemoveCountry(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void RemoveCountryIDZeroException()
        {
            Country testCountry = new Country("Israel");
            facade.RemoveCountry(testCountry);
        }

        [TestMethod]
        [ExpectedException(typeof(CountryNotFoundException))]
        public void RemoveCountryNotFoundException()
        {
            Country testCountry = new Country("Israel");
            testCountry.ID = 1;
            facade.RemoveCountry(testCountry);
        }


        //UPDATE COUNTRY METHOD TESTS:
        [TestMethod]
        public void UpdateCountry()
        {
            Country countryTest = new Country("Israel");
            countryDAO.Add(countryTest);
            countryTest = countryDAO.GetCountryByName("Israel");

            countryTest.CountryName = "India";
            facade.UpdateCountry(countryTest);

            Assert.AreEqual(countryTest.ID, countryDAO.GetCountryByName("India").ID);
            Assert.AreEqual("India", countryDAO.Get(countryTest.ID).CountryName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateCountryNullException()
        {
            facade.UpdateCountry(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateCountryIDZeroException()
        {

            Country countryTest = new Country("Israel");
            facade.UpdateCountry(countryTest);
        }

        [TestMethod]
        [ExpectedException(typeof(CountryNotFoundException))]
        public void UpdateCountryNotFoundException()
        {
            Country countryTest = new Country("Israel");
            countryTest.ID = 1;
            facade.UpdateCountry(countryTest);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateCountryNameNullException()
        {
            Country countryTest = new Country("Israel");
            countryDAO.Add(countryTest);
            countryTest = countryDAO.GetCountryByName("Israel");

            countryTest.CountryName = null;
            facade.UpdateCountry(countryTest);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void UpdateCountryNameExistsException()
        {
            Country countryTest = new Country("Israel");
            countryDAO.Add(countryTest);
            countryDAO.Add(new Country("India"));
            countryTest = countryDAO.GetCountryByName("Israel");
            countryTest.CountryName = "India";

            facade.UpdateCountry(countryTest);
        }

        //UPDATE AIRLINE METHOD TESTS:
        [TestMethod]
        public void UpdateAirline()
        {
            countryDAO.Add(new Country("Israel"));

            airlineDAO.Add(new AirlineCompany("ELAL", "ELALUSERNAME", "ELALPASSEORD", countryDAO.GetCountryByName("Israel").ID));

            AirlineCompany elalModified = airlineDAO.GetAirlineByName("ELAL");
            elalModified.AirLineName = "ELALNEW";
            facade.UpdateAirline(elalModified);

            Assert.AreEqual(elalModified, airlineDAO.GetAirlineByName("ELALNEW"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateAirlineNullAirlineException()
        {
            facade.UpdateAirline(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateAirlineIDZeroExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            theAirline = airlineDAO.Add(theAirline);

            theAirline.ID = 0;

            facade.UpdateAirline(theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateAirlineCountryIDZeroExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            theAirline = airlineDAO.Add(theAirline);

            theAirline.CountryCode = 0;

            facade.UpdateAirline(theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(CountryNotFoundException))]
        public void UpdateAirlineCountryNotFoundExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            theAirline = airlineDAO.Add(theAirline);

            theAirline.CountryCode = countryDAO.GetCountryByName("Israel").ID + 1;

            facade.UpdateAirline(theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateAirlineNullNameExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            theAirline.AirLineName = null;
            facade.UpdateAirline(theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateAirlineNullPasswordExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            theAirline.Password = null;
            facade.UpdateAirline(theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateAirlineNullUsernameExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            theAirline.UserName = null;
            facade.UpdateAirline(theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void UpdateAirlineNameExistsExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            theAirline.AirLineName = "ARKIA";

            facade.UpdateAirline(theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void UpdateAirlineUsernameExistsExeption()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            airlineDAO.Add(theAirline);
            airlineDAO.Add(new AirlineCompany("ARKIA", "ARKIAUSERNAME", "ARKIAPASSWORD", countryDAO.GetCountryByName("Israel").ID));
            theAirline = airlineDAO.GetAirlineByName("ELAL");

            theAirline.UserName = "ARKIAUSERNAME";

            facade.UpdateAirline(theAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(AirlineNotFoundException))]
        public void UpdateAirlineDoesntExistException()
        {
            countryDAO.Add(new Country("Israel"));

            AirlineCompany theAirline = new AirlineCompany("ELAL", "ELAL123", "ELALPASSWORD", countryDAO.GetCountryByName("Israel").ID);
            theAirline.ID = 1;

            facade.UpdateAirline(theAirline);
        }

        //ADD CUSTOMER METHOD TESTS:
        [TestMethod]
        public void CreateNewCustomer()
        {
            facade.CreateNewCustomer(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));
            Assert.AreEqual(1, customerDAO.GetAll().Count);
            Assert.AreEqual("FIRSTNAME", customerDAO.GetAll()[0].FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateNewCustomerNullException()
        {
            facade.CreateNewCustomer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void CreateNewCustomerNullFieldException()
        {
            facade.CreateNewCustomer(new Customer());
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void CreateNewCustomerUsernameExistsException()
        {
            facade.CreateNewCustomer(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));
            facade.CreateNewCustomer(new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME", "PASSWORD2", "ADDRESS2", "PHONENUMBER2", "CARDNUMBER2"));
        }


        //REMOVE CUSTOMER METHOD TESTS:
        [TestMethod]
        public void RemoveCustomer()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");

            facade.RemoveCustomer(customerTest);
            Assert.AreEqual(null, customerDAO.GetAll());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveCustomerNullException()
        {
           facade.RemoveCustomer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void RemoveCustomerIDZeroException()
        {
            facade.RemoveCustomer(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));
        }

        [TestMethod]
        [ExpectedException(typeof(CustomerNotFoundException))]
        public void RemoveCustomerNotFoundException()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerTest.ID = 1;

            facade.RemoveCustomer(customerTest);
        }


        //UPDATE CUSTOMER METHOD TESTS:
        [TestMethod]
        public void UpdateCustomer()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");
            customerTest.FirstName = "CHANGED";
            facade.UpdateCustomer(customerTest);

            Assert.AreEqual("CHANGED", customerDAO.Get(customerTest.ID).FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateCustomerNullException()
        {
            facade.UpdateCustomer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateCustomerIDZeroException()
        {
            facade.UpdateCustomer(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));
        }

        [TestMethod]
        [ExpectedException(typeof(CustomerNotFoundException))]
        public void UpdateCustomerNotFoundException()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerTest.ID = 1;

            facade.UpdateCustomer(customerTest);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateCustomerEmptyFieldsException()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");
            customerTest.FirstName = null;

            facade.UpdateCustomer(customerTest);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void UpdateCustomerUsernameExistsException()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerDAO.Add(new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHONENUMBER2", "CARDNUMBER2"));
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");
            customerTest.UserName = "USERNAME2";

            facade.UpdateCustomer(customerTest);
        }
    }
}
