using WebApiDotNetCore5point1SQLite.Models;

namespace WebApiDotNetCore5point1SQLite
{
    public interface IItemForOne
    {
        public bool CompareSameItems(TodoItem FirstItem, TodoItem SecondItem);
    }
}
