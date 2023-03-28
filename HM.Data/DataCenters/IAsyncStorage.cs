namespace HM.Data.DataCenters;

public interface IAsyncStorage
{
    Task CommitChangesAsync();

    Task InitializeAsync();
}
