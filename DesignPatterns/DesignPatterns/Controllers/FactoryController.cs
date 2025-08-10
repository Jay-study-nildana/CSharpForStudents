using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Factory Method defines an interface for creating an object but lets subclasses alter the type of objects that will be created.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/factory"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class FactoryController : ControllerBase
    {
        // Handles HTTP GET requests to "api/factory/{type}"
        [HttpGet("{type}")]
        public IActionResult GetProduct(string type)
        {
            // Selects the appropriate factory based on the 'type' parameter
            ProductFactory factory = type.ToLower() switch
            {
                "a" => new ProductAFactory(), // Factory for Product A
                "b" => new ProductBFactory(), // Factory for Product B
                _ => null // Invalid type returns null
            };

            // If no valid factory is found, return a 400 Bad Request
            if (factory == null) return BadRequest("Invalid product type");

            // Use the factory to create the product
            var product = factory.CreateProduct();
            // Return the product details with HTTP 200 OK status
            return Ok(product.GetDetails());
        }
    }

    // Abstract base class for products
    public abstract class Product
    {
        // Returns details about the product
        public abstract string GetDetails();
    }

    // Concrete implementation of Product A
    public class ConcreteProductA : Product
    {
        // Returns a string identifying Product A
        public override string GetDetails() => "This is Product A";
    }

    // Concrete implementation of Product B
    public class ConcreteProductB : Product
    {
        // Returns a string identifying Product B
        public override string GetDetails() => "This is Product B";
    }

    // Abstract factory class for creating products
    public abstract class ProductFactory
    {
        // Method to create a product; implemented by subclasses
        public abstract Product CreateProduct();
    }

    // Factory for creating Product A instances
    public class ProductAFactory : ProductFactory
    {
        // Creates and returns a new instance of ConcreteProductA
        public override Product CreateProduct() => new ConcreteProductA();
    }

    // Factory for creating Product B instances
    public class ProductBFactory : ProductFactory
    {
        // Creates and returns a new instance of ConcreteProductB
        public override Product CreateProduct() => new ConcreteProductB();
    }
}