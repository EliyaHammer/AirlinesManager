using System;
using AirLinesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest
{
    [TestClass]
    public class CustomerTest
    {
        // OPERATOR == TESTS:
        [TestMethod]
        public void OneNullComparison()
        {
            Customer x = new Customer();
            Customer y = null;
            x.ID = 1;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void BothNullComparison()
        {
            Customer x = null;
            Customer y = null;
            Assert.AreEqual(true, (x == y));
        }

        [TestMethod]
        public void DifferentIDComparison()
        {
            Customer x = new Customer();
            Customer y = new Customer();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void SameIDComparison()
        {
            Customer x = new Customer();
            Customer y = new Customer();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, (x == y));
        }

        //OPERATOR != TESTS:
        [TestMethod]
        public void OneNullUnComparison()
        {
            Customer x = new Customer();
            Customer y = null;
            x.ID = 1;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void BothNullUnComparison()
        {
            Customer x = null;
            Customer y = null;
            Assert.AreEqual(false, (x != y));
        }

        [TestMethod]
        public void DifferentIDUnComparison()
        {
            Customer x = new Customer();
            Customer y = new Customer();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void SameIDUnComparison()
        {
            Customer x = new Customer();
            Customer y = new Customer();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(false, (x != y));
        }

        // EQUALS TESTS:
        [TestMethod]
        public void SameIDEquals()
        {
            Customer x = new Customer();
            Customer y = new Customer();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, x.Equals(y));
        }
        [TestMethod]
        public void OneNullEquals()
        {
            Customer x = new Customer();
            Customer y = null;
            x.ID = 1;
            Assert.AreEqual(false, x.Equals(y));
        }

        [TestMethod]
        public void DifferentIDEqulas()
        {
            Customer x = new Customer();
            Customer y = new Customer();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, x.Equals(y));
        }

        // GET HASH CODE TESTS:
        [TestMethod]
        public void HashCodeCheck()
        {
            Customer x = new Customer();
            x.ID = 12;
            Assert.AreEqual(12, x.GetHashCode());
        }
    }
}
