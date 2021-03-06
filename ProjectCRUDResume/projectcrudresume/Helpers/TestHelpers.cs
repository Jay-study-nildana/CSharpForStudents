using projectcrudresume.DatabaseClasses;
using projectcrudresume.Models;
using projectcrudresume.PostmanClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//testing new branch Feb26th2019

namespace projectcrudresume.Helpers
{
    public class TestHelpers
    {
        public TestModel GetSingleTestItem()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var tempTestModelItem = db.TestModels.First();
            if(tempTestModelItem==null)
            {
                //item is null, so fill it up yourself
                tempTestModelItem = new TestModel();
                tempTestModelItem.Message1 = "Item Empty";
            }
            return tempTestModelItem;
        }

        //this one adds a single item to the database.
        public TestModelViewModel AddTestItemToSystem(TestModelViewModel testModelViewModel)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            try
            {
                var tempItemForDB = new TestModel();

                tempItemForDB.Message1 = testModelViewModel.Message1;
                tempItemForDB.Message2 = testModelViewModel.Message2;
                tempItemForDB.Number1 = testModelViewModel.Number1;
                tempItemForDB.Number2 = testModelViewModel.Number2;

                db.TestModels.Add(tempItemForDB);
                db.SaveChanges();

                testModelViewModel.StatusMessage = "Entry added successfully :) aka smiley face";

            }
            catch (Exception e)
            {
                testModelViewModel.StatusMessage = "something went bad - (facepalm) " + e.ToString();
            }

            return testModelViewModel;
        }
    }
}
