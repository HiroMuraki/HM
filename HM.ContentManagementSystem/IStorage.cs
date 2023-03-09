namespace HM.ContentManagementSystem;

public interface IStorage
{
    void Initialize();

    void CommitChanges();
}
