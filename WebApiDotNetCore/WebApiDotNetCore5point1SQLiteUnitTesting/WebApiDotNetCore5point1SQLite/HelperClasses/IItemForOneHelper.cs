using WebApiDotNetCore5point1SQLite.Models;

namespace WebApiDotNetCore5point1SQLite
{
    public class IItemForOneHelper : IItemForOne
    {
        public bool CompareSameItems(TodoItem FirstItem, TodoItem SecondItem)
        {
            bool tempResponse = true;

            if (FirstItem.IsComplete != SecondItem.IsComplete)
            {
                tempResponse = false;
            }

            if (FirstItem.Name != SecondItem.Name)
            {
                tempResponse = false;
            }

            return tempResponse;
        }
    }
}
