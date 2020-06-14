using System.Collections.Generic;
using ExamTestApp.Models;

namespace ExamTestApp
{
    public static class Service
    {
        public static IEnumerable<Person> GetPersonList()
        {
            return new List<Person>()
            {
                new Person(){Name = "James", City = "London"},
                new Person(){Name = "Bim", City = "San Diego"},
                new Person(){Name = "Arnold", City = "Stockholm"},
                new Person(){Name = "Isaac", City = "Seattle"},
                new Person(){Name = "Alan", City = "Hull"},
                new Person(){Name = "Teddy", City = ""},
                new Person(){Name = "Fred", City = "Berlin"},
                new Person(){Name = "David", City = "London"},
                new Person(){Name = "Leon", City = "Madrid"},
                new Person(){Name = "Karl", City = "London"},
                new Person(){Name = "Henry", City = "London"},
                new Person(){Name = "Peter", City = "New York"},
                new Person(){Name = "Mike", City = "London"},
                new Person(){Name = "Leo", City = ""},
            };
        }
    }
}