namespace HM.ContentManagementSystem;

public interface IAsyncMemoryDataCenterSerializer<T>
{
    void SerializeAsync(MemoryDataCenter<T>? memoryDataCenter);

    Task<T?> DeserializeAsync();
}
