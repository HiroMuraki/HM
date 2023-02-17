#pragma warning disable IDE0049

using System.Reflection;
using System.Text;

namespace HM.Debug.FakeData
{
    public class Student : FakeObject<Student>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string Major { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string IdCard { get; set; } = string.Empty;
    }
}
