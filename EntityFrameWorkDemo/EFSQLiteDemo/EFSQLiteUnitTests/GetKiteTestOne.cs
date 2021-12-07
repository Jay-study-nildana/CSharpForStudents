
using EFSQLiteDemo.Enums;
using EFSQLiteDemo.Interfaces;
using EFSQLiteDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EFSQLiteDemoUnitTests
{
    public class GetKiteTestOne
    {
        [Fact]
        public void KiteUpdateOctober4thTestOne()
        {
            var actual = false;
            var expected = true;

            IGetKite getKite = new GetKiteVersionOne();
            var response = getKite.GiveMeAKite();

            if (response.KiteHeight < 30 && response.KiteWeight < 79.99)
            {
                actual = true;
            }

            Assert.Equal(actual, expected);
        }
    }

    internal class GetKiteVersionOne : IGetKite
    {
        public KiteUpdateOctober4th GiveMeAKite()
        {
            var response = new KiteUpdateOctober4th();
            response.KiteColor = ColorEnum.Blue;
            response.KiteDesigner = "Some Guy";
            response.KiteHeight = 20;
            response.KiteWeight = 59.99;
            response.KiteWidth = 30;

            return response;
        }
    }
}
