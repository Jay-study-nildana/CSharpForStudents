using System;
using Xunit;
using WebApiDotNetCore5point1SQLite;
using WebApiDotNetCore5point1SQLite.Models;

namespace WebApiDotNetCore5point1SQLite.Tests
{
    public class IItemForOneUnitTest
    {
        [Fact]
        public void TestingDifferentItems()
        {
            TodoItem tempTodoItem1 = new TodoItem();
            TodoItem tempTodoItem2 = new TodoItem();

            tempTodoItem1.IsComplete = false;
            tempTodoItem1.Name = "Jay";

            tempTodoItem2.IsComplete = true;
            tempTodoItem2.Name = "Jay";

            IItemForOne tempIItemForOne = new IItemForOneHelper();
            bool response = tempIItemForOne.CompareSameItems(tempTodoItem1, tempTodoItem2);

            //Assert.Equal(true, response);  //this cannot be used for comparing bools.
            Assert.False(response);
        }

        [Fact]
        public void TestingSameItems()
        {
            TodoItem tempTodoItem1 = new TodoItem();
            TodoItem tempTodoItem2 = new TodoItem();

            tempTodoItem1.IsComplete = true;
            tempTodoItem1.Name = "Jay";

            tempTodoItem2.IsComplete = true;
            tempTodoItem2.Name = "Jay";

            IItemForOne tempIItemForOne = new IItemForOneHelper();
            bool response = tempIItemForOne.CompareSameItems(tempTodoItem1, tempTodoItem2);

            //Assert.Equal(true, response);  //this cannot be used for comparing bools.
            Assert.True(response);
        }

        //a test that will always pass
        //we are just comparing the same thing to itself
        [Fact]
        public void TestThatAlwaysPasses()
        {
            Assert.Equal(4, 4);
        }

        //a test that will always fail
        //we are just comparing the thing to something else
        [Fact]
        public void TestThatAlwaysFails()
        {
            Assert.Equal(4, 2);
        }
    }
}
