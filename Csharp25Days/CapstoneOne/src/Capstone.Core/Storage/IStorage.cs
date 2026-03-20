using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone.Core.Storage
{
    // Generic storage abstraction so we can swap persistence implementations for testing.
    public interface IStorage<T>
    {
        Task<IEnumerable<T>> LoadAsync();
        Task SaveAsync(IEnumerable<T> items);
    }
}
