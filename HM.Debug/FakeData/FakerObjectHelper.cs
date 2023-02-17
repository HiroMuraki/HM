#pragma warning disable IDE0049


namespace HM.Debug.FakeData
{
    internal class FakerObjectHelper
    {
        public static T[] FakeMany<T>(Func<T> creater, int count)
        {
            ArgumentNullException.ThrowIfNull(creater);

            var result = new T[count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = creater();
            }
            return result;
        }
    }
}
