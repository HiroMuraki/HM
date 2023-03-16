namespace HM.Data;

public interface ISingleData<T>
{
    T? Get();

    int Update(T item);

    int Delete();
}
