namespace HM.Data;

public interface IAsyncSingleData<T>
{
    Task<T?> GetAsync();

    Task<int> UpdateAsync(T item);

    Task<int> DeleteAsync();
}