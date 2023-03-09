namespace HM.ContentManagementSystem;

public interface IAsyncDataCenter<T>
{
    Task<T[]> GetAsync(Predicate<T> predicate);

    Task<T?> FirstAsync(Predicate<T> predicate);

    Task<int> AddAsync(T item);

    Task<int> UpdateAsync(Predicate<T> predicate, T newValue);

    Task<int> DeleteAsync(Predicate<T> predicate);
}
