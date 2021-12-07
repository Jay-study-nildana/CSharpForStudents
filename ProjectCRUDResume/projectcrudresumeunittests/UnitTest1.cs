using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using projectcrudresume.Models;

namespace projectcrudresumeunittests
{

    //TODO16 - make this ApplicationDbContext point to a test database.
    [TestClass]
    public class UnitTest1
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [TestMethod]
        public void TestMethod1()
        {
            var expected = true;
            var actual = true;

            var listOfStuff = db.UserProfiles.ToList();

            Assert.AreEqual(expected,actual);
        }
    }
}
