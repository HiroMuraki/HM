namespace HM.Serialization.Csv
{
    public interface ISheetReader<TCell>
    {
        IEnumerable<TCell[]> EnumerateRecords();
    }
}