using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.DAOTests
{
    [TestClass]
    public class CustomerDAOMSSQLTests
    {
        IAirlineDAO airlineDAO = new AirlineDAOMSSQL();
        ITicketDAO ticketDAO = new TicketDAOMSSQL();
        IFlightDAO flightDAO = new FlightDAOMSSQL();
        IAdministratorDAO administratorDAO = new AdministratorDAOMSSQL();
        ICustomerDAO customerDAO = new CustomerDAOMSSQL();
        ICountryDAO countryDAO = new CountryDAOMSSQL();

        [TestInitialize]
        public void TestInitialize()
        {
            TicketDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            ticketDAO.RemoveAllReplica();
            CustomerDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            customerDAO.RemoveAllReplica();
            FlightDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            flightDAO.RemoveAllReplica();
            AirlineDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            airlineDAO.RemoveAllReplica();
            CountryDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            countryDAO.RemoveAllReplica();
            AdministratorDAOMSSQL._connectionString = MyConfig._replicaConnectionString;
            administratorDAO.RemoveAllReplica();
        }

        //ADD METHOD TESTS:
        [TestMethod]
        public void Add()
        {
            customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));
            Assert.AreEqual(1, customerDAO.GetAll().Count);
            Assert.AreEqual("FIRSTNAME", customerDAO.GetAll()[0].FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddNullException()
        {
            customerDAO.Add(null);
        }

        //GET METHOD TESTS: > not used
        [TestMethod]
        public void Get()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");

            Assert.AreEqual(customerTest, customerDAO.Get(customerTest.ID));
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetIDZeroException()
        {
            customerDAO.Get(0);
        }

        [TestMethod]
        public void GetNotFound()
        {
            Assert.AreEqual(null, customerDAO.Get(1));
        }


        //GET ALL METHOD TESTS: > not used
        [TestMethod]
        public void GetAll()
        {
            customerDAO.Add(new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER"));
            customerDAO.Add(new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHONENUMBER2", "CARDNUMBER2"));

            Assert.AreEqual(2, customerDAO.GetAll().Count);
            Assert.AreEqual("FIRSTNAME", customerDAO.GetAll()[0].FirstName);
            Assert.AreEqual("FIRSTNAME2", customerDAO.GetAll()[1].FirstName);
        }

        [TestMethod]
        public void GetAllNotFound ()
        {
            Assert.AreEqual(null, customerDAO.GetAll());
        }

        //GET CUSTOMER BY FULL NAME METHOD TESTS: > not used
        [TestMethod]
        public void GetByFullName()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");

            Assert.AreEqual(customerTest.ID, customerDAO.GetCustomerByFullName("FIRSTNAME", "LASTNAME")[0].ID); 
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetByFullNameNullException()
        {
            customerDAO.GetCustomerByFullName(null, null);
        }

        [TestMethod]
        public void GetByFullNameNotFoundException()
        {
            Assert.AreEqual(null, customerDAO.GetCustomerByFullName("FIRSTNAME", "LASTNAME"));
        }


        //GET CUSTOMER BY USERNAME METHOD TESTS: > not used
        [TestMethod]
        public void GetByUsername()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");

            Assert.AreEqual(customerTest.ID, customerDAO.GetCustomerByUsername("USERNAME").ID);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetByUsernameNullException()
        {
            customerDAO.GetCustomerByUsername(null);
        }

        [TestMethod]
        public void GetByUsernameNotFoundException()
        {
            Assert.AreEqual(null, customerDAO.GetCustomerByUsername("USERNAME"));
        }


        //REMOVE METHOD TESTS:
        [TestMethod]
        public void Remove()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");

            customerDAO.Remove(customerTest);
            Assert.AreEqual(null, customerDAO.GetAll());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveNullException()
        {
            customerDAO.Remove(null);
        }


        //UPDATE METHOD TESTS:
        [TestMethod]
        public void Update()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");
            customerTest.FirstName = "CHANGED";
            customerDAO.Update(customerTest);

            Assert.AreEqual("CHANGED", customerDAO.Get(customerTest.ID).FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNullException()
        {
            customerDAO.Update(null);
        }


        //CHECK USERNAME EXISTANCE METHOD TESTS:
        [TestMethod]
        public void CheckUsernameExistance()
        {
            Customer customerTest = new Customer("FIRSTNAME", "LASTNAME", "USERNAME", "PASSWORD", "ADDRESS", "PHONENUMBER", "CARDNUMBER");
            customerDAO.Add(customerTest);
            customerDAO.Add(new Customer("FIRSTNAME2", "LASTNAME2", "USERNAME2", "PASSWORD2", "ADDRESS2", "PHONENUMBER2", "CARDNUMBER2"));
            customerTest = customerDAO.GetCustomerByUsername("USERNAME");

            Assert.AreEqual(true, customerDAO.CheckUsernameExistance("USERNAME2", customerTest.ID));
            Assert.AreEqual(false, customerDAO.CheckUsernameExistance("USERNAME", customerTest.ID));
        }

        [TestMethod]
        public void CheckUsernameExistanceNull()
        {
            Assert.AreEqual(false, customerDAO.CheckUsernameExistance(null, 1));
        }

    }
}
