using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//The Factory Method defines an interface for creating an object but lets subclasses alter the type of objects that will be created.

namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactoryController : ControllerBase
    {
        [HttpGet("{type}")]
        public IActionResult GetProduct(string type)
        {
            ProductFactory factory = type.ToLower() switch
            {
                "a" => new ProductAFactory(),
                "b" => new ProductBFactory(),
                _ => null
            };

            if (factory == null) return BadRequest("Invalid product type");

            var product = factory.CreateProduct();
            return Ok(product.GetDetails());
        }
    }

    public abstract class Product
    {
        public abstract string GetDetails();
    }

    public class ConcreteProductA : Product
    {
        public override string GetDetails() => "This is Product A";
    }

    public class ConcreteProductB : Product
    {
        public override string GetDetails() => "This is Product B";
    }

    // ProductFactory.cs
    public abstract class ProductFactory
    {
        public abstract Product CreateProduct();
    }

    public class ProductAFactory : ProductFactory
    {
        public override Product CreateProduct() => new ConcreteProductA();
    }

    public class ProductBFactory : ProductFactory
    {
        public override Product CreateProduct() => new ConcreteProductB();
    }
}
