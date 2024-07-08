using NUnit.Framework.Internal;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExpandoPlayground.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void MakeDynamicContact_BasicLoadRecord()
        {
            // Arrange
            var dto = new ContactDTO
            {
                FirstName = "John",
                LastName = "Doe",
                HiredDate = DateTime.Now
            };
            var fieldNames = new List<FieldDescription> {
                new FieldDescription("First", "FirstName", null),
                new FieldDescription("Last", "LastName", null),
                new FieldDescription("HiredDate", "HiredDate", null)
            };

            // Act
            dynamic dynamicContact = DynamicDataMaker.MakeDynamicContact(dto, fieldNames);

            // Assert
            Assert.AreEqual("John", dynamicContact.First);
            Assert.AreEqual("Doe", dynamicContact.Last);
            Assert.AreEqual(dto.HiredDate, dynamicContact.HiredDate);
        }

        [Test]
        public void MakeDynamicContact_CheckNullResults()
        {
            // Arrange
            var dto = new ContactDTO
            {
                FirstName = "John",
                LastName = "Doe",
                HiredDate = DateTime.Now
            };
            var fieldNames = new List<FieldDescription> {
                new FieldDescription("FiredDate", "FiredDate", null),
                new FieldDescription("First", "FirstName", null),
                new FieldDescription("Last", "LastName", null),
            };

            // Act
            dynamic dynamicContact = DynamicDataMaker.MakeDynamicContact(dto, fieldNames);

            // Assert
            Assert.AreEqual("John", dynamicContact.First);
            Assert.AreEqual("Doe", dynamicContact.Last);
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "FiredDate"), Is.True);
            Assert.IsNull(dto.FiredDate);
        }


        [Test]
        public void MakeDynamicContact_OnlyAddReqeustedFields()
        {
            // Arrange
            DateTime hireDate = DateTime.Today.AddDays(55);
            var dto = new ContactDTO
            {
                FirstName = "John",
                LastName = "Doe",
                HiredDate = hireDate
            };
            var fieldNames = new List<FieldDescription> {
                new FieldDescription("First", "FirstName", null),
                new FieldDescription("HiredDate", "HiredDate", null)
            };

            // Act
            var dynamicContact = DynamicDataMaker.MakeDynamicContact(dto, fieldNames);

            // Assert
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "First"), Is.True);
            Assert.AreEqual("John", dynamicContact.First);
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "Last"), Is.False);
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "HiredDate"), Is.True);
            Assert.AreEqual(hireDate, dynamicContact.HiredDate);
        }

        [Test]
        public void MakeDynamicContact_OnlyAddPersonalData()
        {
            // Arrange
            DateTime BirthDate = TestDataGenerator.GenerateRealisticBirthdate();
            var dto = new ContactDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Birthdate = BirthDate,
                Sex = "M"
            };
            var fieldNames = new List<FieldDescription> {
                new FieldDescription("First", "FirstName", null),
                new FieldDescription("PersonalData:Birthdate", "Birthdate", null),
                new FieldDescription("PersonalData:Sex", "Sex", null)
            };

            // Act
            var dynamicContact = DynamicDataMaker.MakeDynamicContact(dto, fieldNames);

            // Assert
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "First"), Is.True);
            Assert.AreEqual("John", dynamicContact.First);
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "PersonalData"), Is.True);
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "PersonalData:Birthdate"), Is.False);
            dynamic personalData = dynamicContact.PersonalData;//as List<dynamic>;
            //Assert.AreEqual(1, personalData.Count);

            Assert.That(DynamicDataMaker.HasProperty(personalData, "Birthdate"), Is.True);
            Assert.AreEqual(BirthDate, personalData.Birthdate);
            Assert.That(DynamicDataMaker.HasProperty(personalData, "Sex"), Is.True);
            Assert.AreEqual("M", personalData.Sex);
        }


        [Test]
        public void MakeDynamicContact_phonesList()
        {
            // Arrange
            DateTime hireDate = DateTime.Today.AddDays(55);
            var dto = new ContactDTO
            {
                FirstName = "John",
                LastName = "Doe",
                HiredDate = hireDate,
                HomePhone = "509-555-1234",
                WorkPhone = "206-555-5678",
                TollFreePhone = "800-555-9876",
                MobilePhone = "509-555-4321"
            };
            var fieldNames = new List<FieldDescription> { 
                new FieldDescription("FirstName", "FirstName", null), 
                new FieldDescription("Phones:Number", "HomePhone", "Home"),
                new FieldDescription("Phones:Number", "WorkPhone", "Work"),
                new FieldDescription("Phones:Number", "TollFreePhone", "TollFreePhone"),
                new FieldDescription("Phones:Number", "MobilePhone", "Cell"),
                new FieldDescription("HiredDate", "HiredDate", null) 
            };

            // Act
            var dynamicContact = DynamicDataMaker.MakeDynamicContact(dto, fieldNames);

            // Assert
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "FirstName"), Is.True);
            Assert.AreEqual("John", dynamicContact.FirstName);
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "HiredDate"), Is.True);
            Assert.AreEqual(hireDate, dynamicContact.HiredDate);
            Assert.That(DynamicDataMaker.HasProperty(dynamicContact, "Phones"), Is.True);
            dynamic phones = dynamicContact.Phones;//as List<dynamic>;
            Assert.AreEqual(4, phones.Count);
            CheckPhone(phones[0], "Home", "509-555-1234");
            CheckPhone(phones[1], "Work", "206-555-5678");
            CheckPhone(phones[2], "TollFreePhone", "800-555-9876");
            CheckPhone(phones[3], "Cell", "509-555-4321");
        }

        private static void CheckPhone(dynamic phone1, string type, string number)
        {
            Assert.AreEqual(type, phone1.Type);
            Assert.AreEqual(number, phone1.Number);
        }

        [Test]
        [TestCase("Hello:World", ":", "Hello", "World", "Simple case with one separator")]
        [TestCase("Hello:World:Dude", ":", "Hello", "World:Dude", "Three parts with two separators")]
        [TestCase("Hello", ":", "Hello", null, "Single part with no separator")]
        [TestCase("123:456:789:000", ":", "123", "456:789:000", "Multiple parts with multiple separators")]
        [TestCase("Lorem:Ipsum:Dolor:Sit:Amet", ":", "Lorem", "Ipsum:Dolor:Sit:Amet", "Long string with multiple separators")]
        [TestCase("Hello,World", ",", "Hello", "World", "Simple case with comma separator")]
        [TestCase("Hello,World,Dude", ",", "Hello", "World,Dude", "Three parts with two comma separators")]
        [TestCase("Hello", ",", "Hello", null, "Single part with no comma separator")]
        [TestCase("Hello::World", "::", "Hello", "World", "Simple case with longer ('::') separator")]
        [TestCase("Hello::World::Dude", "::", "Hello", "World::Dude", "Three parts with two longer ('::') separators")]
        [TestCase("Hello", "::", "Hello", null, "Single part with no longer ('::') separator")]
        [TestCase(null, "::", null, null, "null returns null in both places")]
        public void BreakInTwo_WithValidInput(string? input, string separator, string? expectedPart1, string? expectedPart2, string comment)
        {
            // Arrange

            // Act
            (string? part1, string? part2) = DynamicDataMaker.BreakInTwo(input, separator);

            // Assert
            Assert.That(part1, Is.EqualTo(expectedPart1), comment);
            Assert.That(part2, Is.EqualTo(expectedPart2), comment);
        }

        [Test]
        public void GetDynamicPropertyByName_ExistingPropertyString_ReturnsPropertyValue()
        {
            dynamic obj = new ExpandoObject();
            obj.FirstName = "John";
            obj.LastName = "Doe";

            string propertyName = "FirstName";
            dynamic result = DynamicDataMaker.GetDynamicPropertyByName(obj, propertyName);

            Assert.AreEqual("John", result);
        }

        [Test]
        public void GetDynamicPropertyByName_ExistingPropertyInt_ReturnsPropertyValue()
        {
            dynamic obj = new ExpandoObject();
            obj.Age = 13;
            obj.LastName = "Doe";

            string propertyName = "Age";
            dynamic result = DynamicDataMaker.GetDynamicPropertyByName(obj, propertyName);

            Assert.AreEqual(13, result);
        }

        [Test]
        public void GetDynamicPropertyByName_NonExistingProperty_ReturnsNull()
        {
            dynamic obj = new ExpandoObject();
            obj.FirstName = "John";
            obj.LastName = "Doe";

            string propertyName = "Age";
            dynamic result = DynamicDataMaker.GetDynamicPropertyByName(obj, propertyName);

            Assert.AreEqual(null, result);
            Assert.IsNull(result);
        }
    }
}