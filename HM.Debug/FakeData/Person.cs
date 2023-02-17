namespace HM.Debug.FakeData
{
    public class Person : FakeObject, IFakeCreateable<Person>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public DateTime Birthday { get; set; }

        public static Person FakeOne()
        {
            return new Person()
            {

            };
        }
        public static Person[] FakeMany(int count) => FakerObjectHelper.FakeMany(FakeOne, count);
    }
}
