#pragma warning disable IDE0049


namespace HM.Debug.FakeData
{
    public interface IFakeCreateable<T>
    {
        static abstract T FakeOne();
        static abstract T[] FakeMany(int count);
    }
}
