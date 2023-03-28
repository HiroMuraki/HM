namespace HM.Data.DataCenters;

public interface IAsyncSingleDataCenter<T>
{
    Task<T?> GetAsync();

    Task<int> UpdateAsync(T item);

    Task<int> DeleteAsync();
}