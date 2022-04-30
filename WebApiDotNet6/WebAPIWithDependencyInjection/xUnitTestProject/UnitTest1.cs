using WebAPIWithDependencyInjection.Interfaces;
using WebAPIWithDependencyInjection.POCO;
using Xunit;

namespace xUnitTestProject
{
    public class UnitTest1
    {
        //[Fact]
        //public void Test1()
        //{

        //}

        //Test the first implementation
        [Fact]
        public void TestSayMyNameOne()
        {
            ISayMyName sayMyName = new SayMyNameOne();
            bool actual = false;

            var response = sayMyName.IAmName();

            //I want a simple test. I will simply check that the string is not empty
            //In a real project, you should setup more complex tests.
            if(response.Length > 0)
            {
                //my assumption is that the lenght is 0 or negative.
                //the moment i see lenght is more than 0, I assume a string has been returned
                //So, I change this to true, allowing the test to pass.
                actual = true;
            }
            var expected = true;

            Assert.Equal(expected, actual);
        }

        //Test the second implementation
        //When following TDD, you test every implementation.
        [Fact]
        public void TestSayMyNameTwo()
        {
            ISayMyName sayMyName = new SayMyNameTwo();
            bool actual = false;

            var response = sayMyName.IAmName();

            //I want a simple test. I will simply check that the string is not empty
            //In a real project, you should setup more complex tests.
            if (response.Length > 0)
            {
                //my assumption is that the lenght is 0 or negative.
                //the moment i see lenght is more than 0, I assume a string has been returned
                //So, I change this to true, allowing the test to pass.
                actual = true;
            }
            var expected = true;

            Assert.Equal(expected, actual);
        }
    }
}