namespace HM.Data;

public interface IAsyncStorage
{
    Task CommitChangesAsync();

    Task InitializeAsync();
}
