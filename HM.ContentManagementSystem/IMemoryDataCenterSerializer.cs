namespace HM.ContentManagementSystem;

public interface IMemoryDataCenterSerializer<T>
{
    void Serialize(MemoryDataCenter<T> memoryDataCenter);
}
