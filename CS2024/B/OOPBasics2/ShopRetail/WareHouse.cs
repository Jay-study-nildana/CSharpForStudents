using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRetail
{
    public class WareHouse
    {
        //here is the delegate
        public delegate int TotalStockDelegate(int stocka, int stockb);
        public delegate void TotalStockDelegateSecondOne(int stocka, int stockb);

        public int TotalStock(int StockA, int StockB)
        {
            int CompleteStock = StockA + StockB;
            return CompleteStock;
        }

        public void TotalCustomers(int customersmonday, int customerstuesday)
        {
            int TotalCustomersShopped = customersmonday + customerstuesday;
            Console.WriteLine("Total Customers : " + TotalCustomersShopped);
        }

        public void TotalCustomersLaughing(int customersmonday, int customerstuesday)
        {
            int TotalCustomersLaughingHappily = customersmonday * 2 + customerstuesday * 2;
            Console.WriteLine("Total Customers Who Laughed : " + TotalCustomersLaughingHappily);
        }
    }
}
