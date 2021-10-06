using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moq4Demo1.Services
{
    public class Models
    {
    }

    public interface IAddressInfo
    {
        public string Street { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
    }

    public interface ICard
    {
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime ValidTo { get; set; }
    }

    public interface CartItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
