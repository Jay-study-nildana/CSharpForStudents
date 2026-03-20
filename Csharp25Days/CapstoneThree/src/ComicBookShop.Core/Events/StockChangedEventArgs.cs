namespace ComicBookShop.Core.Events;

/// <summary>
/// Event data raised when a comic book's stock level changes.
/// Demonstrates custom EventArgs and the observer pattern (Day 19).
/// </summary>
public class StockChangedEventArgs : EventArgs
{
    public Guid ComicBookId { get; }
    public string Title { get; }
    public int PreviousQuantity { get; }
    public int NewQuantity { get; }
    public bool IsLowStock { get; }

    public StockChangedEventArgs(
        Guid comicBookId, string title,
        int previousQuantity, int newQuantity,
        bool isLowStock)
    {
        ComicBookId = comicBookId;
        Title = title;
        PreviousQuantity = previousQuantity;
        NewQuantity = newQuantity;
        IsLowStock = isLowStock;
    }
}
