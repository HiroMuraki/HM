namespace HM.Data;

public interface IDataCenter<T>
{
    T? Get(Predicate<T> predicate);

    T[] GetMany(Predicate<T> predicate);

    int Add(T item);

    int AddMany(T[] items);

    int Update(Predicate<T> predicate, T newValue);

    int UpdateMany(Predicate<T> predicate, T newValue);

    int Delete(Predicate<T> predicate);

    int DeleteMany(Predicate<T> predicate);
}