namespace HM.Data;

public class MemoryDataCenter<T> : IDataCenter<T>, IAsyncDataCenter<T>
{
    public IEnumerable<T> ReadedData => _readedData;

    public virtual void LoadInitialData(IEnumerable<T> data)
    {
        _readedData.Clear();
        _readedData.AddRange(data);
    }

    public T? Get(Predicate<T> predicate) => GetCore(predicate);

    public T[] GetMany(Predicate<T> predicate) => GetManyCore(predicate);

    public int Add(T item) => AddCore(item);

    public int AddMany(T[] items) => AddManyCore(items);

    public int Update(Predicate<T> predicate, T newValue) => UpdateCore(predicate, newValue, true);

    public int UpdateMany(Predicate<T> predicate, T newValue) => UpdateCore(predicate, newValue, false);

    public int Delete(Predicate<T> predicate) => DeleteCore(predicate, true);

    public int DeleteMany(Predicate<T> predicate) => DeleteCore(predicate, false);

    public async Task<T?> GetAsync(Predicate<T> predicate) => await Task.FromResult(GetCore(predicate));

    public async Task<T[]> GetManyAsync(Predicate<T> predicate) => await Task.FromResult(GetManyCore(predicate));

    public async Task<int> AddAsync(T item) => await Task.FromResult(AddCore(item));

    public async Task<int> AddManyAsync(T[] items) => await Task.FromResult(AddManyCore(items));

    public async Task<int> UpdateAsync(Predicate<T> predicate, T newValue) => await Task.FromResult(UpdateCore(predicate, newValue, true));

    public async Task<int> UpdateManyAsync(Predicate<T> predicate, T newValue) => await Task.FromResult(UpdateCore(predicate, newValue, false));

    public async Task<int> DeleteAsync(Predicate<T> predicate) => await Task.FromResult(DeleteCore(predicate, true));

    public async Task<int> DeleteManyAsync(Predicate<T> predicate) => await Task.FromResult(DeleteCore(predicate, false));

    public MemoryDataCenter()
    {

    }

    public MemoryDataCenter(IEnumerable<T> initialData)
    {
        _readedData.Clear();
        _readedData.AddRange(initialData);
    }

    #region
    private readonly List<T> _readedData = new();
    protected virtual int AddCore(T item)
    {
        _readedData.Add(item);
        return 1;
    }
    protected virtual int AddManyCore(T[] items)
    {
        int addedCount = 0;

        foreach (var item in items)
        {
            _readedData.Add(item);
            addedCount++;
        }

        return addedCount;
    }
    protected virtual T? GetCore(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return _readedData.FirstOrDefault(t => predicate(t));
    }
    protected virtual T[] GetManyCore(Predicate<T> predicate)
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
    protected virtual int DeleteCore(Predicate<T> predicate, bool single)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        int removedCount = 0;

        for (int i = 0; i < _readedData.Count; i++)
        {
            if (predicate(_readedData[i]))
            {
                _readedData.RemoveAt(i);
                removedCount++;
                if (single)
                {
                    break;
                }
                --i;
            }
        }

        return removedCount;
    }
    protected virtual int UpdateCore(Predicate<T> predicate, T newValue, bool single)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        int updatedCount = 0;

        for (int i = 0; i < _readedData.Count; i++)
        {
            if (predicate(_readedData[i]))
            {
                _readedData[i] = newValue;
                updatedCount++;
                if (single)
                {
                    break;
                }
            }
        }

        return updatedCount;
    }
    #endregion
}