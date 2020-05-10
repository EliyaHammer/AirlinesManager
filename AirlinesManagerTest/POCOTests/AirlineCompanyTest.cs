using System;
using AirLinesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlinesManagerTest
{
    [TestClass]
    public class AirlineCompanyTest
    {

        // OPERATOR == TESTS:
        [TestMethod]
        public void OneNullComparison()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = null;
            x.ID = 1;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void BothNullComparison()
        {
            AirlineCompany x = null;
            AirlineCompany y = null;
            Assert.AreEqual(true, (x == y));
        }

        [TestMethod]
        public void DifferentIDComparison()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = new AirlineCompany();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, (x == y));
        }

        [TestMethod]
        public void SameIDComparison()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = new AirlineCompany();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, (x == y));
        }

        //OPERATOR != TESTS:
        [TestMethod]
        public void OneNullUnComparison()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = null;
            x.ID = 1;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void BothNullUnComparison()
        {
            AirlineCompany x = null;
            AirlineCompany y = null;
            Assert.AreEqual(false, (x != y));
        }

        [TestMethod]
        public void DifferentIDUnComparison()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = new AirlineCompany();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(true, (x != y));
        }

        [TestMethod]
        public void SameIDUnComparison()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = new AirlineCompany();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(false, (x != y));
        }

        // EQUALS TESTS:
        [TestMethod]
        public void SameIDEquals()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = new AirlineCompany();
            x.ID = 1;
            y.ID = 1;
            Assert.AreEqual(true, x.Equals(y));
        }
        [TestMethod]
        public void OneNullEquals()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = null;
            x.ID = 1;
            Assert.AreEqual(false, x.Equals(y));
        }

        [TestMethod]
        public void DifferentIDEqulas()
        {
            AirlineCompany x = new AirlineCompany();
            AirlineCompany y = new AirlineCompany();
            x.ID = 1;
            y.ID = 2;
            Assert.AreEqual(false, x.Equals(y));
        }

        // GET HASH CODE TESTS:
        [TestMethod]
        public void HashCodeCheck ()
        {
            AirlineCompany x = new AirlineCompany();
            x.ID = 12;
            Assert.AreEqual(12, x.GetHashCode());
        }

    }
}
