namespace HM.ContentManagementSystem;

public interface IAsyncMemoryStorage<T> : IAsyncStorage
{
    IMemoryDataCenterSerializer<T> DataSerializer { get; }
}
