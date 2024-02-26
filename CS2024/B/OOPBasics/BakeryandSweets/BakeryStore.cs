using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryandSweets
{
    //a class that holds Bakery Store Details
    public class BakeryStore
    {
        public int NumberOfStoreEmployees { get; set; }
        public string StoreCity { get; set; }

        public string StoreAddress = "Here is the Address";

        public static int TotalNumberOfEmployeesInTwoStores(BakeryStore firststore, BakeryStore secondstore)
        {
            var total = firststore.NumberOfStoreEmployees + secondstore.NumberOfStoreEmployees;
            return total;
        }

        //default values have been added here for the parameter
        //if no value is sent for the number of stores, the default value will be used.
        public void UpdateStoreDetails(string StoreCity,int NumberOfStoreEmployees=20)
        {
            this.StoreCity = StoreCity;
            this.NumberOfStoreEmployees = NumberOfStoreEmployees;
        }

        //Method Overloading

        public void DisplayTheStore()
        {
            Console.WriteLine(" Store Location: " + this.StoreCity);
            Console.WriteLine(" Store Employees: " + this.NumberOfStoreEmployees);
        }

        //same name as above function, but overloaded
        public void DisplayTheStore(int pattern)
        {
            if(pattern == 0 )
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(" Store Location: " + this.StoreCity);
                Console.WriteLine(" Store Employees: " + this.NumberOfStoreEmployees);
                Console.WriteLine("--------------------------");
            }
            else {
                Console.WriteLine("**************************");
                Console.WriteLine(" Store Location: " + this.StoreCity);
                Console.WriteLine(" Store Employees: " + this.NumberOfStoreEmployees);
                Console.WriteLine("**************************");
            }

        }

        public ref string GetStoreAddressReference()
        {
            return ref StoreAddress;
        }

        public void CollectAllTheParameters(params string[] collectionofparameters)
        {
            for (int i = 0; i < collectionofparameters.Length; i++)
            {
                Console.WriteLine($"{collectionofparameters[i]}");
            }
        }
    }
}
