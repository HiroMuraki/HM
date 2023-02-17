#pragma warning disable IDE0049

using System.Reflection;
using System.Text;

namespace HM.Debug.FakeData
{
    public class Student : Person, IFakeCreateable<Student>
    {
        public string Major { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string IdCard { get; set; } = string.Empty;

        public new static Student FakeOne()
        {
            var person = Person.FakeOne();
            return new Student()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Sex = person.Sex,
                Age = person.Age,
                Birthday = person.Birthday,
            };
        }
        public new static Student[] FakeMany(int count) => FakerObjectHelper.FakeMany(FakeOne, count);
    }
}
