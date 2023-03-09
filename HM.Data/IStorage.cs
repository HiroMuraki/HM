namespace HM.Data;

public interface IStorage
{
    void Initialize();

    void CommitChanges();
}
