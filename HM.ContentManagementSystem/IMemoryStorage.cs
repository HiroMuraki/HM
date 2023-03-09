namespace HM.ContentManagementSystem;

public interface IMemoryStorage<T> : IStorage
{
    IMemoryDataCenterSerializer<T> DataSerializer { get; }
}
