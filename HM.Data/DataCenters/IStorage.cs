namespace HM.Data.DataCenters;

public interface IStorage
{
    void Initialize();

    void CommitChanges();
}
