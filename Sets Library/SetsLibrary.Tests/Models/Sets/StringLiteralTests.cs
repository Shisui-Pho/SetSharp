using Xunit;
namespace SetsLibrary.Tests.Models.Sets
{
    public class StringLiteralSetTests
    {
        private SetExtractionConfiguration<string> _extractionConfig;

        public StringLiteralSetTests()
        {
            // Setup an example extraction configuration to use in the tests
            _extractionConfig = new SetExtractionConfiguration<string>("none", ",");
        }

        // Test constructor that uses the SetExtractionConfiguration
        [Fact]
        public void Constructor_WithSetExtractionConfiguration_ShouldInitializeCorrectly()
        {
            var stringSet = new StringLiteralSet(_extractionConfig);

            Assert.NotNull(stringSet);
            Assert.Equal(_extractionConfig, stringSet.ExtractionConfiguration);
        }

        // Test constructor with string expression and SetExtractionConfiguration
        [Fact]
        public void Constructor_WithExpressionAndSetExtractionConfiguration_ShouldInitializeCorrectly()
        {
            string expression = "{0,1,2,3,4,5}";
            var stringSet = new StringLiteralSet(expression, _extractionConfig);

            Assert.NotNull(stringSet);
            Assert.Equal(expression, stringSet.BuildStringRepresentation());
            Assert.Equal(_extractionConfig, stringSet.ExtractionConfiguration);
        }

        // Test the BuildNewSet method when no string input is provided
        [Fact]
        public void BuildNewSet_WithNoInput_ShouldReturnEmptySet()
        {
            var stringSet = new StringLiteralSet(_extractionConfig);

            Assert.IsType<StringLiteralSet>(stringSet);
            Assert.Equal("{\u2205}", stringSet.BuildStringRepresentation());  // Assumes the default set string is empty
        }

        // Test for the GetString method
        [Fact]
        public void GetString_ShouldReturnCorrectStringRepresentation()
        {
            string expression = "{0,1,2,3,4}";
            var stringSet = new StringLiteralSet(expression, _extractionConfig);

            var result = stringSet.BuildStringRepresentation();

            Assert.Equal("{0,1,2,3,4}", result);
        }

        // Test for adding a new element to the set (assuming Add method exists)
        [Fact]
        public void Add_ShouldAddElementToSet()
        {
            string expression = "{0,1,2,3}";
            var stringSet = new StringLiteralSet(expression, _extractionConfig);

            stringSet.AddElement("4");

            Assert.Contains("4", stringSet.BuildStringRepresentation());
        }

        // Test for removing an element from the set (assuming Remove method exists)
        [Fact]
        public void Remove_ShouldRemoveElementFromSet()
        {
            string expression = "{0,1,2,3}";
            var stringSet = new StringLiteralSet(expression, _extractionConfig);

            stringSet.RemoveElement("2");

            Assert.DoesNotContain("2", stringSet.BuildStringRepresentation());
        }

        // Test if the cardinality (size) of the set is correct
        [Fact]
        public void Cardinality_ShouldReturnCorrectSize()
        {
            string expression = "{0,1,2,3,4,5}";
            var stringSet = new StringLiteralSet(expression, _extractionConfig);

            var cardinality = stringSet.Cardinality;

            Assert.Equal(6, cardinality);  // Set should have 6 elements
        }

        // Test if the set correctly handles nested sets in string form (e.g., nested sets like {0,1,2,3} and {4,5,6})
        [Fact]
        public void NestedSets_ShouldBeHandledCorrectly()
        {
            string expression = "{0,1,2,3,{4,5,6}}";
            var stringSet = new StringLiteralSet(expression, _extractionConfig);

            var result = stringSet.BuildStringRepresentation();

            Assert.Equal("{0,1,2,3,{4,5,6}}", result);
        }

        // Test for an empty set
        [Fact]
        public void EmptySet_ShouldReturnEmptyString()
        {
            var stringSet = new StringLiteralSet(_extractionConfig);

            var result = stringSet.BuildStringRepresentation();
            stringSet.AddElement("2");
            Assert.Equal("{\u2205}", result);  // Assuming empty set results in an empty string representation
            Assert.Equal("{2}", stringSet.BuildStringRepresentation());
        }
    }
}
