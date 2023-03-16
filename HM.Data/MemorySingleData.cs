namespace HM.Data;

public class MemorySingleData<T> : IAsyncSingleData<T>, ISingleData<T>
{
    public int Delete()
    {
        _value = default;
        return 1;
    }

    public async Task<int> DeleteAsync()
    {
        _value = default;
        return await Task.FromResult(1);
    }

    public T? Get()
    {
        return _value;
    }

    public async Task<T?> GetAsync()
    {
        return await Task.FromResult(_value);
    }

    public int Update(T item)
    {
        _value = item;
        return 1;
    }

    public async Task<int> UpdateAsync(T item)
    {
        _value = item;
        return await Task.FromResult(1);
    }

    #region NonPublic
    private T? _value;
    #endregion
}
