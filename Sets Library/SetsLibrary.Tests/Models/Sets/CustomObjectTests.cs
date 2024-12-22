using SetsLibrary;
using Xunit;

namespace SetsLibrary.Tests.Models.Sets
{
    public class Person : ICustomObjectConverter<Person>, IComparable<Person>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public Person() { }

        public Person(string name, string surname, int age)
        {
            Name = name;
            Surname = surname;
            Age = age;
        }

        public int CompareTo(Person other)
        {
            return Age.CompareTo(other.Age); // Sorting by age
        }

        public override string ToString()
        {
            return $"{Name}, {Surname},{Age}";
        }

        // ICustomObjectConverter implementation
        public Person ToObject(string field, SetExtractionConfiguration<Person> settings)
        {
            var parts = field.Split(',');
            if (parts.Length != 3)
                throw new ArgumentException("Invalid format.");

            return new Person(parts[0].Trim(), parts[1].Trim(), int.Parse(parts[2].Trim()));
        }
    }

    public class CustomObjectSetTests
    {
        [Fact]
        public void TestBasicParsingAndSorting()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{Phiwo, Smith, 15\nHello, Kitty, 10\nBen, Clips, 20}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            var expected = "{Hello, Kitty,10\nPhiwo, Smith,15\nBen, Clips,20}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestDuplicateHandling()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{Phiwo, Smith,15\nHello, Kitty,10\nPhiwo, Smith,15}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            var expected = "{Hello, Kitty,10\nPhiwo, Smith,15}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestComplexNestedAndSortedSet()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{Phiwo,Smith,15\nHello,Kitty,10\n{Ben,Clips,20\nBen,Clips,20}\n{Ben,Clips,20}\n{Brum, Ficher, 35}}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            var expected = "{Hello, Kitty,10\nPhiwo, Smith,15\n{Ben, Clips,20}\n{Brum, Ficher,35}}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestInvalidFormat()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{Invalid Format\nHello, Kitty,10}";

            // Act & Assert
            var exception = Assert.Throws<SetsException>(() => new CustomObjectSet<Person>(expression, config));
            Assert.Contains("Conversion failed", exception.InnerException?.Message);
        }

        [Fact]
        public void TestEmptySet()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            var expected = "{∅}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestSingleElementSet()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{John, Doe, 30}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            var expected = "{John, Doe,30}";
            Assert.Equal(expected, result);
        }
        // Test: Handling of Invalid Format
        [Fact]
        public void TestInvalidFormatInStringExpression()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{Phiwo, Smith, 15\nHello, Kitty, 10\nInvalidFieldFormat}";

            // Act & Assert
            Assert.Throws<SetsException>(() => new CustomObjectSet<Person>(expression, config));
        }

        // Test: Empty String Expression
        [Fact]
        public void TestEmptyStringExpression()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);

            // Assert
            Assert.Empty(set.EnumerateSubsets()); // Set should be empty
            Assert.Empty(set.EnumerateRootElements()); // Set should be empty
        }

        // Test: Field Terminator Equals Row Terminator
        [Fact]
        public void TestFieldTerminatorEqualsRowTerminator()
        {
            // Arrange & Act
            var exception = Assert.Throws<SetsConfigurationException>(() =>
                new SetExtractionConfiguration<Person>(",", ",", new Person()));

            // Assert
            Assert.Contains("Terminators cannot be the same.", exception.Message);
        }

        // Test: Field Terminator Contains Reserved Characters
        [Fact]
        public void TestFieldTerminatorContainsReservedCharacters()
        {
            // Arrange & Act
            var exception = Assert.Throws<SetsConfigurationException>(() =>
                new SetExtractionConfiguration<Person>("{", "\n", new Person()));

            // Assert
            Assert.Contains("Cannot use reserved characters.", exception.Message);
        }

        // Test: Row Terminator Contains Reserved Characters
        [Fact]
        public void TestRowTerminatorContainsReservedCharacters()
        {
            // Arrange & Act
            var exception = Assert.Throws<SetsConfigurationException>(() =>
                new SetExtractionConfiguration<Person>(",", "{", new Person()));

            // Assert
            Assert.Contains("Cannot use reserved characters.", exception.Message);
        }

        // Test: Handling of Missing Row Terminator
        [Fact]
        public void TestMissingRowTerminator()
        {
            // Arrange & Act
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new SetExtractionConfiguration<Person>(",", null, new Person()));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter '_rowTerminator')", exception.Message);
        }

        // Test: Set with Single Element
        [Fact]
        public void TestSetWithSingleElement()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{Hello, Kitty,10}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            Assert.Equal("{Hello, Kitty,10}", result); // Set should have one element
        }

        // Test: Null Record Handling in String Expression
        [Fact]
        public void TestNullRecordHandling()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{Hello, Kitty, 10\nnull}";

            // Act & Assert
            var exception = Assert.Throws<SetsException>(() => new CustomObjectSet<Person>(expression, config));
            Assert.Contains("Conversion failed", exception.InnerException?.Message ?? "");
        }

        // Test: Handling Special Characters in Fields
        [Fact]
        public void TestSpecialCharactersInFields()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{John, Doe,20\nJane, O'Connor,30}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            Assert.Equal("{John, Doe,20\nJane, O'Connor,30}", result);
        }

        // Test: Case Sensitivity in Fields
        [Fact]
        public void TestCaseSensitivityInFields()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{Alice, Wonderland,25\nalice, wonderland,25}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            Assert.Equal("{Alice, Wonderland,25}", result); // Case-sensitive, duplicates removed
        }

        // Test: Large Input Set with Complex Nested Sets
        [Fact]
        public void TestLargeInputSetWithComplexNestedSets()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{John, Doe,20\n{Alice, Wonderland,25\nBob, Marley,30\n{Charlie, Brown,35}}\nJane, Austen,40}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            var expected = "{John, Doe,20\nJane, Austen,40\n{Alice, Wonderland,25\nBob, Marley,30\n{Charlie, Brown,35}}}";
            Assert.Equal(expected, result);
        }

        // Test: Empty Nested Set Handling
        [Fact]
        public void TestEmptyNestedSetHandling()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{John, Doe,20\n{}}";

            // Act
            var set = new CustomObjectSet<Person>(expression, config);
            var result = set.BuildStringRepresentation();

            // Assert
            Assert.Equal("{John, Doe,20\n{∅}}", result); // Empty nested set should be ignored
        }

        // Test: Multiple Custom Object Converters (Different Field Terminators)
        [Fact]
        public void TestMultipleCustomObjectConverters()
        {
            // Arrange
            var config = new SetExtractionConfiguration<Person>(",", "\n", new Person());
            string expression = "{John, Doe,20|Jane, Austen,30}"; // Custom delimiter `|`

            // Act & Assert
            Assert.Throws<SetsException>(() => new CustomObjectSet<Person>(expression, config));
        }
    }//class
}//namespace
