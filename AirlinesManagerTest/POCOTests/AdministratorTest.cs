using System;
using AirLinesManager;
using AirLinesManager.POCO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest
{
    [TestClass]
    public class AdministratorTest
    {
        // OPERATOR == TESTS:
        [TestMethod]
        public void OneNullComparison()
        {
            Administrator x = new Administrator();
            Administrator y = null;
            x.ID = 1;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void BothNullComparison()
        {
            Administrator x = null;
            Administrator y = null;
            Assert.AreEqual(true, (x == y));
        }

        [TestMethod]
        public void DifferentIDComparison()
        {
            Administrator x = new Administrator();
            Administrator y = new Administrator();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void SameIDComparison()
        {
            Administrator x = new Administrator();
            Administrator y = new Administrator();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, (x == y));
        }

        //OPERATOR != TESTS:
        [TestMethod]
        public void OneNullUnComparison()
        {
            Administrator x = new Administrator();
            Administrator y = null;
            x.ID = 1;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void BothNullUnComparison()
        {
            Administrator x = null;
            Administrator y = null;
            Assert.AreEqual(false, (x != y));
        }

        [TestMethod]
        public void DifferentIDUnComparison()
        {
            Administrator x = new Administrator();
            Administrator y = new Administrator();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void SameIDUnComparison()
        {
            Administrator x = new Administrator();
            Administrator y = new Administrator();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(false, (x != y));
        }

        // EQUALS TESTS:
        [TestMethod]
        public void SameIDEquals()
        {
            Administrator x = new Administrator();
            Administrator y = new Administrator();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, x.Equals(y));
        }
        [TestMethod]
        public void OneNullEquals()
        {
            Administrator x = new Administrator();
            Administrator y = null;
            x.ID = 1;
            Assert.AreEqual(false, x.Equals(y));
        }

        [TestMethod]
        public void DifferentIDEqulas()
        {
            Administrator x = new Administrator();
            Administrator y = new Administrator();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, x.Equals(y));
        }

        // GET HASH CODE TESTS:
        [TestMethod]
        public void HashCodeCheck()
        {
            Administrator x = new Administrator();
            x.ID = 12;
            Assert.AreEqual(12, x.GetHashCode());
        }
    }
}
