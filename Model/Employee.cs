using System.Security.Cryptography;

namespace InterviewTest.Model
{
    public class Employee
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public (string name, int value) ToKeyValuePair => (Name, Value);
    }
}
