using System;
using AirLinesManager;
using AirLinesManager.DAO.MSSQL;
using AirLinesManager.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest.DAOTests.MSSQL
{
    [TestClass]
    public class AdministratorDAOMSSQLTest
    {
        // TODO:
        // use interface instead of concrete class 
        IAirlineDAO airlineDAO = new AirlineDAOMSSQL();
        TicketDAOMSSQL ticketDAO = new TicketDAOMSSQL();
        FlightDAOMSSQL flightDAO = new FlightDAOMSSQL();
        AdministratorDAOMSSQL administratorDAO = new AdministratorDAOMSSQL();
        CustomerDAOMSSQL customerDAO = new CustomerDAOMSSQL();
        CountryDAOMSSQL countryDAO = new CountryDAOMSSQL();

        [AssemblyInitialize]
        public void AssemblyInitialize ()
        {
            BasicDAO<IPoco>._connectionString = MyConfig._replicaConnectionString;
        }

        [TestInitialize]
        public void TestInitialize()
        {
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
            administratorDAO.Add(new Administrator("USERNAME", "PASSWORD"));

            Assert.AreEqual(1, administratorDAO.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddNullException()
        {
            administratorDAO.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void AddNullFieldException()
        {
            Administrator toAdd = new Administrator();
            toAdd.Password = "PASSWORD";

            administratorDAO.Add(toAdd);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddUsernameAlreadyExistsException()
        {
            administratorDAO.Add(new Administrator("USERNAME", "PASSWORD"));
            administratorDAO.Add(new Administrator("USERNAME", "PASSWORD2"));
        }

        //REMOVE
        [TestMethod]
        public void Remove()
        {
            Administrator user = administratorDAO.Add(new Administrator("USERNAME", "PASSWORD"));
            administratorDAO.Remove(user);

            Assert.AreEqual(null, administratorDAO.GetAll());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveNullException()
        {
            administratorDAO.Remove(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void RemoveIDZeroException()
        {
            administratorDAO.Remove(new Administrator());
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void RemoveNotFoundException()
        {
            Administrator user = new Administrator();
            user.ID = 1;
            administratorDAO.Remove(user);
        }

        //UPDATE
        [TestMethod]
        public void Update()
        {
            Administrator user = administratorDAO.Add(new Administrator("USERNAME", "PASSWORD"));
            user.Username = "USERNAME2";
            administratorDAO.Update(user);

            Assert.AreEqual("USERNAME2", administratorDAO.GetAll()[0].Username);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNullException()
        {
            administratorDAO.Update(null);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateIDZeroException()
        {
            administratorDAO.Update(new Administrator());
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void UpdateUserNotFoundException()
        {
            Administrator user = new Administrator();
            user.ID = 1;
            administratorDAO.Update(user);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void UpdateNullFieldException()
        {
            Administrator user = administratorDAO.Add(new Administrator("USERNAME", "PASSWORD"));
            user.Username = null;

            administratorDAO.Update(user);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void UpdateUsernameExistsException()
        {
            administratorDAO.Add(new Administrator("USERNAME2", "PASSWORD2"));
            Administrator user = administratorDAO.Add(new Administrator("USERNAME", "PASSWORD"));
            user.Username = "USERNAME2";

            administratorDAO.Update(user);
        }


        //CheckUsernameExistance
        [TestMethod]
        public void CheckUsernameExistance()
        {
            Administrator user = administratorDAO.Add(new Administrator("USERNAME", "PASSWORD"));

            Assert.AreEqual(true, administratorDAO.CheckUsernameExistance("USERNAME", user.ID + 1));
            Assert.AreEqual(false, administratorDAO.CheckUsernameExistance("USERNAME", user.ID));
        }

        //GET
        [TestMethod]
        public void Get()
        {
            Administrator user = administratorDAO.Add(new Administrator("username", "password"));

            Assert.AreEqual(user, administratorDAO.Get(user.ID));
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalValueException))]
        public void GetIDZeroException()
        {
            administratorDAO.Get(0);
        }


        //GetAdminByUsername
        [TestMethod]
        public void GetByUsername()
        {
            Administrator user = administratorDAO.Add(new Administrator("username", "password"));

            Assert.AreEqual(user, administratorDAO.GetAdminByUsername(user.Username));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetByUsernameNullException()
        {
            administratorDAO.GetAdminByUsername(null);
        }


        //GetAll
        [TestMethod]
        public void GetAll()
        {
            administratorDAO.Add(new Administrator("username", "password"));
            administratorDAO.Add(new Administrator("username2", "password2"));

            Assert.AreEqual(2, administratorDAO.GetAll().Count);
        }

    }
}
