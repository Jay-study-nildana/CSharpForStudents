namespace SmartStore.Patterns.Structural;

// ================================================================
// COMPOSITE PATTERN  —  Composite node
// ================================================================
// BundleOrderItem is a composite that holds multiple OrderItemBase
// children (leaf items or other bundles). Clients call GetTotalPrice()
// and Display() identically on both leaves and composites.
//
// Intent   : Compose objects into tree structures to represent part–whole
//            hierarchies. Let clients treat individual objects and
//            compositions uniformly.
// Problem  : A customer can buy a "Starter Bundle" (monitor + keyboard +
//            mouse) at a bundle discount. How do we keep pricing consistent
//            without special-casing bundles everywhere?
// Solution : BundleOrderItem extends OrderItemBase and aggregates children.
//            GetTotalPrice() sums children and applies the discount.
// ================================================================
public class BundleOrderItem : OrderItemBase
{
    private readonly List<OrderItemBase> _children = new();
    private readonly decimal _discountPercent;

    public BundleOrderItem(string bundleName, decimal discountPercent = 0)
    {
        BundleName = bundleName;
        _discountPercent = discountPercent;
    }

    public string BundleName { get; }
    public override string Name => BundleName;

    public void Add(OrderItemBase item)    => _children.Add(item);
    public void Remove(OrderItemBase item) => _children.Remove(item);

    public override decimal GetTotalPrice()
    {
        var subtotal = _children.Sum(c => c.GetTotalPrice());
        return subtotal * (1 - _discountPercent / 100m);
    }

    public override void Display(string indent = "")
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"{indent}+ [BUNDLE] {BundleName}  (−{_discountPercent}% discount)  = ${GetTotalPrice():F2}");
        Console.ResetColor();
        foreach (var child in _children)
            child.Display(indent + "  ");
    }
}
