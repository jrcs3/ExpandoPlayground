using System;
using System.Collections.Generic;

namespace ExpandoPlayground
{
    public static class TestDataGenerator
    {
        private static readonly Random random = new Random();
        private static readonly List<string> firstNames = new List<string> { "John", "Jane", "Mike", "Mary", "Steve", "Nancy", "Brian", "Lisa", "James", "Patricia" };
        private static readonly List<string> lastNames = new List<string> { "Doe", "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez" };
        private static readonly List<string> jobTitles = new List<string> { "Developer", "Manager", "Analyst", "Designer", "Architect", "Engineer", "Consultant", "Technician", "Operator", "Supervisor" };
        private static readonly List<string> states = new List<string> { "CA", "NY", "TX", "FL", "PA", "IL", "OH", "GA", "NC", "MI" };
        private static readonly List<string> cities = new List<string> { "Los Angeles", "New York", "Houston", "Chicago", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose" };
        private static readonly List<string> streetNames = new List<string> { "Main St", "Oak St", "Pine St", "Maple St", "Cedar St", "Elm St", "View St", "Lake St", "Hill St", "Park St" };

        public static List<ContactDTO> GenerateTestData(int numberOfRecords)
        {
            var testData = new List<ContactDTO>();

            for (int i = 0; i < numberOfRecords; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Count)];
                var lastName = lastNames[random.Next(lastNames.Count)];
                var state = states[random.Next(states.Count)];
                var city = cities[random.Next(cities.Count)];
                var streetName = streetNames[random.Next(streetNames.Count)];

                testData.Add(new ContactDTO
                {
                    Id = i + 1,
                    FirstName = firstName,
                    MiddleName = random.Next(2) == 0 ? null : "A.",
                    LastName = lastName,
                    JobTitle = jobTitles[random.Next(jobTitles.Count)],
                    HiredDate = DateTime.Now.AddDays(-random.Next(365 * 2)),
                    FiredDate = random.Next(2) == 0 ? null : DateTime.Now.AddDays(random.Next(365)),
                    Position = $"Position {i + 1}",
                    HomePhone = $"555-{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    WorkPhone = random.Next(2) == 0 ? null : $"555-{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    TollFreePhone = $"800-{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    MobilePhone = $"555-{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    HomeAddressLine1 = $"{random.Next(100, 999)} {streetName}",
                    HomeAddressLine2 = random.Next(2) == 0 ? null : $"Apt {random.Next(1, 20)}",
                    HomeCity = city,
                    HomeState = state,
                    HomePostalCode = $"{random.Next(10000, 99999)}",
                    HomeCountry = "USA",
                    WorkAddressLine1 = $"{random.Next(100, 999)} {streetName}",
                    WorkAddressLine2 = random.Next(2) == 0 ? null : $"Suite {random.Next(100, 300)}",
                    WorkCity = city,
                    WorkState = state,
                    WorkPostalCode = $"{random.Next(10000, 99999)}",
                    WorkCountry = "USA",
                    Birthdate = GenerateRealisticBirthdate(),
                    Sex = random.Next(2) == 0 ? "F" : "M",
                });
            }

            return testData;
        }
        public static DateTime GenerateRealisticBirthdate()
        {
            var minAge = 18;
            var maxAge = 72;
            var randomAge = random.Next(minAge, maxAge + 1);
            var birthdate = DateTime.Today.AddYears(-randomAge).AddDays(-random.Next(365));
            return birthdate;
        }
    }
}
