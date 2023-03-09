namespace HM.ContentManagementSystem;

public interface IAsyncStorage
{
    Task CommitChangesAsync();

    Task InitializeAsync();
}
