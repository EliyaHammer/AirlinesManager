using System;
using AirLinesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest
{
    [TestClass]
    public class FlightTest
    {
        // OPERATOR == TESTS:
        [TestMethod]
        public void OneNullComparison()
        {
            Flight x = new Flight();
            Flight y = null;
            x.ID = 1;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void BothNullComparison()
        {
            Flight x = null;
            Flight y = null;
            Assert.AreEqual(true, (x == y));
        }

        [TestMethod]
        public void DifferentIDComparison()
        {
            Flight x = new Flight();
            Flight y = new Flight();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void SameIDComparison()
        {
            Flight x = new Flight();
            Flight y = new Flight();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, (x == y));
        }

        //OPERATOR != TESTS:
        [TestMethod]
        public void OneNullUnComparison()
        {
            Flight x = new Flight();
            Flight y = null;
            x.ID = 1;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void BothNullUnComparison()
        {
            Flight x = null;
            Flight y = null;
            Assert.AreEqual(false, (x != y));
        }

        [TestMethod]
        public void DifferentIDUnComparison()
        {
            Flight x = new Flight();
            Flight y = new Flight();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void SameIDUnComparison()
        {
            Flight x = new Flight();
            Flight y = new Flight();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(false, (x != y));
        }

        // EQUALS TESTS:
        [TestMethod]
        public void SameIDEquals()
        {
            Flight x = new Flight();
            Flight y = new Flight();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, x.Equals(y));
        }
        [TestMethod]
        public void OneNullEquals()
        {
            Flight x = new Flight();
            Flight y = null;
            x.ID = 1;
            Assert.AreEqual(false, x.Equals(y));
        }

        [TestMethod]
        public void DifferentIDEqulas()
        {
            Flight x = new Flight();
            Flight y = new Flight();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, x.Equals(y));
        }

        // GET HASH CODE TESTS:
        [TestMethod]
        public void HashCodeCheck()
        {
            Flight x = new Flight();
            x.ID = 12;
            Assert.AreEqual(12, x.GetHashCode());
        }
    }
}
