using System;
using AirLinesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest
{
    [TestClass]
    public class CountryTest
    {
        // OPERATOR == TESTS:
        [TestMethod]
        public void OneNullComparison()
        {
            Country x = new Country();
            Country y = null;
            x.ID = 1;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void BothNullComparison()
        {
            Country x = null;
            Country y = null;
            Assert.AreEqual(true, (x == y));
        }

        [TestMethod]
        public void DifferentIDComparison()
        {
            Country x = new Country();
            Country y = new Country();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void SameIDComparison()
        {
            Country x = new Country();
            Country y = new Country();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, (x == y));
        }

        //OPERATOR != TESTS:
        [TestMethod]
        public void OneNullUnComparison()
        {
            Country x = new Country();
            Country y = null;
            x.ID = 1;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void BothNullUnComparison()
        {
            Country x = null;
            Country y = null;
            Assert.AreEqual(false, (x != y));
        }

        [TestMethod]
        public void DifferentIDUnComparison()
        {
            Country x = new Country();
            Country y = new Country();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void SameIDUnComparison()
        {
            Country x = new Country();
            Country y = new Country();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(false, (x != y));
        }

        // EQUALS TESTS:
        [TestMethod]
        public void SameIDEquals()
        {
            Country x = new Country();
            Country y = new Country();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, x.Equals(y));
        }
        [TestMethod]
        public void OneNullEquals()
        {
            Country x = new Country();
            Country y = null;
            x.ID = 1;
            Assert.AreEqual(false, x.Equals(y));
        }

        [TestMethod]
        public void DifferentIDEqulas()
        {
            Country x = new Country();
            Country y = new Country();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, x.Equals(y));
        }

        // GET HASH CODE TESTS:
        [TestMethod]
        public void HashCodeCheck()
        {
            Country x = new Country();
            x.ID = 12;
            Assert.AreEqual(12, x.GetHashCode());
        }
    }
}
