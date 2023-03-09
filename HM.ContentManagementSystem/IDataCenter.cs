namespace HM.ContentManagementSystem;

public interface IDataCenter<T>
{
    T[] Get(Predicate<T> predicate);

    T? First(Predicate<T> predicate);

    int Add(T item);

    int Update(Predicate<T> predicate, T newValue);

    int Delete(Predicate<T> predicate);
}