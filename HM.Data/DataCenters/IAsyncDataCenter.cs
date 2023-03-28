namespace HM.Data.DataCenters;

public interface IAsyncDataCenter<T>
{
    Task<T?> GetAsync(Predicate<T> predicate);

    Task<T[]> GetManyAsync(Predicate<T> predicate);

    Task<int> AddAsync(T item);

    Task<int> AddManyAsync(T[] items);

    Task<int> UpdateAsync(Predicate<T> predicate, T newValue);

    Task<int> UpdateManyAsync(Predicate<T> predicate, T newValue);

    Task<int> DeleteAsync(Predicate<T> predicate);

    Task<int> DeleteManyAsync(Predicate<T> predicate);
}
