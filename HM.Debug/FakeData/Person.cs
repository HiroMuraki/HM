#pragma warning disable IDE0049


namespace HM.Debug.FakeData
{
    public class Person : FakeObject<Person>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public DateTime Birthday { get; set; }
    }
}
