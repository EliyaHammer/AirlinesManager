using System;
using AirLinesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest
{
    [TestClass]
    public class TicketTest
    {
        // OPERATOR == TESTS:
        [TestMethod]
        public void OneNullComparison()
        {
            Ticket x = new Ticket();
            Ticket y = null;
            x.ID = 1;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void BothNullComparison()
        {
            Ticket x = null;
            Ticket y = null;
            Assert.AreEqual(true, (x == y));
        }

        [TestMethod]
        public void DifferentIDComparison()
        {
            Ticket x = new Ticket();
            Ticket y = new Ticket();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void SameIDComparison()
        {
            Ticket x = new Ticket();
            Ticket y = new Ticket();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, (x == y));
        }

        //OPERATOR != TESTS:
        [TestMethod]
        public void OneNullUnComparison()
        {
            Ticket x = new Ticket();
            Ticket y = null;
            x.ID = 1;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void BothNullUnComparison()
        {
            Ticket x = null;
            Ticket y = null;
            Assert.AreEqual(false, (x != y));
        }

        [TestMethod]
        public void DifferentIDUnComparison()
        {
            Ticket x = new Ticket();
            Ticket y = new Ticket();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void SameIDUnComparison()
        {
            Ticket x = new Ticket();
            Ticket y = new Ticket();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(false, (x != y));
        }

        // EQUALS TESTS:
        [TestMethod]
        public void SameIDEquals()
        {
            Ticket x = new Ticket();
            Ticket y = new Ticket();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, x.Equals(y));
        }
        [TestMethod]
        public void OneNullEquals()
        {
            Ticket x = new Ticket();
            Ticket y = null;
            x.ID = 1;
            Assert.AreEqual(false, x.Equals(y));
        }

        [TestMethod]
        public void DifferentIDEqulas()
        {
            Ticket x = new Ticket();
            Ticket y = new Ticket();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, x.Equals(y));
        }

        // GET HASH CODE TESTS:
        [TestMethod]
        public void HashCodeCheck()
        {
            Ticket x = new Ticket();
            x.ID = 12;
            Assert.AreEqual(12, x.GetHashCode());
        }
    }
}
