namespace HM.ContentManagementSystem;

public class MemoryDataCenter<T> : IDataCenter<T>, IAsyncDataCenter<T>
{
    public IEnumerable<T> ReadedData => _readedData;

    public int Add(T item)
    {
        _readedData.Add(item);
        return 1;
    }

    public int Delete(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        int removedCount = 0;

        for (int i = 0; i < _readedData.Count; i++)
        {
            if (predicate(_readedData[i]))
            {
                _readedData.RemoveAt(i);
                --i;
                removedCount++;
            }
        }

        return removedCount;
    }

    public T? First(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return _readedData.FirstOrDefault(t => predicate(t));
    }

    public T[] Get(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var fetched = new List<T>();

        for (int i = 0; i < _readedData.Count; i++)
        {
            if (predicate(_readedData[i]))
            {
                fetched.Add(_readedData[i]);
            }
        }

        return fetched.ToArray();
    }

    public int Update(Predicate<T> predicate, T newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var updatedCount = 0;

        for (int i = 0; i < _readedData.Count; i++)
        {
            if (predicate(_readedData[i]))
            {
                _readedData[i] = newValue;
                updatedCount++;
            }
        }

        return updatedCount;
    }

    public async Task<T[]> GetAsync(Predicate<T> predicate)
        => await Task.FromResult(Get(predicate));

    public async Task<T?> FirstAsync(Predicate<T> predicate)
        => await Task.FromResult(First(predicate));

    public async Task<int> AddAsync(T item)
        => await Task.FromResult(Add(item));

    public async Task<int> UpdateAsync(Predicate<T> predicate, T newValue)
        => await Task.FromResult(Update(predicate, newValue));

    public async Task<int> DeleteAsync(Predicate<T> predicate)
        => await Task.FromResult(Delete(predicate));

    public MemoryDataCenter()
    {

    }

    public MemoryDataCenter(IEnumerable<T> readedData)
    {
        foreach (var item in readedData)
        {
            _readedData.Add(item);
        }
    }

    #region
    private readonly IList<T> _readedData = new List<T>();
    #endregion
}