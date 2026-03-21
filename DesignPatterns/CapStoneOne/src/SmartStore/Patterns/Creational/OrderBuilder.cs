namespace SmartStore.Patterns.Creational;

// ================================================================
// BUILDER PATTERN
// ================================================================
// Constructs a complex Order object step-by-step using a fluent API.
// Separates object construction from its representation.
//
// Intent   : Construct complex objects from simpler parts, step-by-step.
// Problem  : How do you build an Order that may have individual items,
//            bundle items, notes, and varied customers without huge constructors?
// Solution : A dedicated Builder class accumulates state and produces
//            the final object only when Build() is called.
// ================================================================
public class OrderBuilder
{
    private Customer _customer = null!;
    private readonly List<OrderItemBase> _items = new();
    private string? _notes;
    private static int _nextId = 1;

    public OrderBuilder ForCustomer(Customer customer)
    {
        _customer = customer;
        return this;
    }

    public OrderBuilder WithItem(Product product, int quantity)
    {
        _items.Add(new OrderItem
        {
            Product = product,
            Quantity = quantity,
            UnitPrice = product.Price
        });
        return this;
    }

    public OrderBuilder WithBundle(BundleOrderItem bundle)
    {
        _items.Add(bundle);
        return this;
    }

    public OrderBuilder WithNotes(string notes)
    {
        _notes = notes;
        return this;
    }

    /// <summary>Finalises and returns the built Order. Validates required fields.</summary>
    public Order Build()
    {
        if (_customer is null)
            throw new InvalidOperationException("Builder: a Customer is required.");
        if (_items.Count == 0)
            throw new InvalidOperationException("Builder: at least one item is required.");

        return new Order
        {
            Id = _nextId++,
            Customer = _customer,
            Items = new List<OrderItemBase>(_items),
            Notes = _notes,
            Status = OrderStatus.Draft,
            CreatedAt = DateTime.Now
        };
    }
}
