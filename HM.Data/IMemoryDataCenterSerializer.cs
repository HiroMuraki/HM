namespace HM.Data;

public interface IMemoryDataCenterSerializer<T>
{
    void Serialize(MemoryDataCenter<T>? memoryDataCenter);

    Task<T>? Deserialize();
}
