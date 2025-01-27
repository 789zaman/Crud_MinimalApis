using System;

namespace Minimal.Api
{
    public record Person(string FullName);
    public class PeopleService
    {

        private readonly List<Person> _people = new()
        {
            new Person("Alester cook"),
            new Person("tim cook"),
            new Person("Abdulrub"),
            new Person("Abdullah")
        };

        public IEnumerable<Person> Search(string searchTerm)
        {
            return _people.Where(x => x.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }
}
