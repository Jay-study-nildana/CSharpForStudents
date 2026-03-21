// 07-IRepository.cs
// Generic repository interface to show DIP between domain and persistence layers.

using System.Collections.Generic;

public interface IRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Remove(T entity);
}