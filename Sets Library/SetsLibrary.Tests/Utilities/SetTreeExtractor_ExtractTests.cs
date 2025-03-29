using SetsLibrary.Utility;
using Xunit;
namespace SetsLibrary.Tests.Utilities.Extract
{

    public class SetTreeExtractor_ExtractTests
    {
        // Mocking the ISetTree interface for the purposes of testing
        public class MockSetTree<T> : ISetTree<T> where T : IComparable<T>
        {
            public string RootElements { get; set; }
            public int Count { get; set; }
            public int CountRootElements => RootElements.Split(',').Length;
            public int CountSubsets { get; set; }
            public SetExtractionConfiguration ExtractionSettings { get; set; }

            private List<ISetTree<T>> _subsets = new List<ISetTree<T>>();
            private List<T> _elements = new List<T>();

            public MockSetTree(string rootElements, SetExtractionConfiguration extractionSettings)
            {
                RootElements = rootElements;
                ExtractionSettings = extractionSettings;
            }

            public void AddSubSetTree(ISetTree<T> tree)
            {
                _subsets.Add(tree);
                CountSubsets++;
            }

            public void AddElement(T element)
            {
                _elements.Add(element);
                Count++;
            }

            public void AddRange(IEnumerable<T> elements)
            {
                _elements.AddRange(elements);
                Count += elements is ICollection<T> collection ? collection.Count : 0;
            }

            public IEnumerable<ISetTree<T>> GetSubsetsEnumerator() => _subsets;
            public IEnumerable<T> GetRootElementsEnumerator() => _elements;
            public bool RemoveElement(T element) => _elements.Remove(element);
            public bool RemoveElement(ISetTree<T> element) => _subsets.Remove(element);
            public int IndexOf(T element) => _elements.IndexOf(element);
            public int IndexOf(ISetTree<T> subset) => _subsets.IndexOf(subset);

            public override string ToString() => $"{RootElements} (Cardinality: {Count})";

            public void AddRange(IEnumerable<ISetTree<T>> subsets)
            {
                foreach (var item in subsets)
                    AddSubSetTree(item);
            }

            public int CompareTo(ISetTree<T>? other)
                => 5;
        }

        // Test to validate the basic Extract functionality with simple set expressions
        [Fact]
        public void Extract_ValidSimpleSetExpression()
        {
            // Arrange
            string expression = "{1,2,3}";
            var config = new SetExtractionConfiguration(";", ",");
            SetTreeExtractor<int> extractor = new SetTreeExtractor<int>();

            // Act
            ISetTree<int> tree = SetTreeExtractor<int>.Extract(expression, config);

            // Assert
            Assert.Equal("1,2,3", tree.RootElements);
            Assert.Equal(3, tree.Count);
            Assert.Equal(3, tree.CountRootElements);
            Assert.Equal(0, tree.CountSubsets);
        }

        // Test to validate handling of empty expression (no elements)
        [Fact]
        public void Extract_EmptySetExpression()
        {
            // Arrange
            string expression = "{}";
            var config = new SetExtractionConfiguration(";", ",");
            SetTreeExtractor<int> extractor = new SetTreeExtractor<int>();

            // Act
            ISetTree<int> tree = SetTreeExtractor<int>.Extract(expression, config);

            // Assert
            Assert.Equal("", tree.RootElements);
            Assert.Equal(0, tree.Count);
            Assert.Equal(0, tree.CountRootElements);
            Assert.Equal(0, tree.CountSubsets);
        }

        // Test to validate how the method handles nested set expressions
        [Fact]
        public void Extract_NestedSetExpression()
        {
            // Arrange
            string expression = "{1,2,{3,4}}";
            var config = new SetExtractionConfiguration(";", ",");
            SetTreeExtractor<int> extractor = new SetTreeExtractor<int>();

            // Act
            ISetTree<int> tree = SetTreeExtractor<int>.Extract(expression, config);

            // Assert for the root elements
            Assert.Equal("1,2", tree.RootElements);
            Assert.Equal(3, tree.Count);
            Assert.Equal(2, tree.CountRootElements);
            Assert.Equal(1, tree.CountSubsets);

            // Assert for the first subset
            ISetTree<int> subsetTree = tree.GetSubsetsEnumerator().First();
            Assert.Equal("3,4", subsetTree.RootElements);
            Assert.Equal(2, subsetTree.Count);
            Assert.Equal(2, subsetTree.CountRootElements);
            Assert.Equal(0, subsetTree.CountSubsets);
        }
        // Test for a more complex expression with multiple subsets
        [Fact]
        public void Extract_ComplexSetExpression()
        {
            // Arrange
            string expression = "{1,2,{3,4},{5,6}}";
            var config = new SetExtractionConfiguration(";", ",");
            SetTreeExtractor<int> extractor = new SetTreeExtractor<int>();

            // Act
            ISetTree<int> tree = SetTreeExtractor<int>.Extract(expression, config);

            // Assert for the root elements
            Assert.Equal("1,2", tree.RootElements);
            Assert.Equal(4, tree.Count);
            Assert.Equal(2, tree.CountRootElements);
            Assert.Equal(2, tree.CountSubsets);

            // Assert for the first subset
            ISetTree<int> subsetTree1 = tree.GetSubsetsEnumerator().ElementAt(0);
            Assert.Equal("3,4", subsetTree1.RootElements);
            Assert.Equal(2, subsetTree1.Count);

            // Assert for the second subset
            ISetTree<int> subsetTree2 = tree.GetSubsetsEnumerator().ElementAt(1);
            Assert.Equal("5,6", subsetTree2.RootElements);
            Assert.Equal(2, subsetTree2.Count);
        }

        // Test for a set with a custom conversion (e.g., converting strings to integers)
        [Fact]
        public void Extract_SetWithCustomConversion()
        {
            // Arrange
            string expression = "{1,2,3}";
            var customConverter = new CustomStringToIntConverter();
            var config = new SetExtractionConfiguration(";", ",");

            SetTreeExtractor<int> extractor = new SetTreeExtractor<int>();

            // Act
            ISetTree<int> tree = SetTreeExtractor<int>.Extract(expression, config);

            // Assert
            Assert.Equal("1,2,3", tree.RootElements);
            Assert.Equal(3, tree.Count);
        }

        // Test for an expression with spaces around the elements
        [Fact]
        public void Extract_SetWithSpaces()
        {
            // Arrange
            string expression = "{ 1 , 2 , 3 }";
            var config = new SetExtractionConfiguration(";", ",");
            SetTreeExtractor<int> extractor = new SetTreeExtractor<int>();

            // Act
            ISetTree<int> tree = SetTreeExtractor<int>.Extract(expression, config);

            // Assert
            Assert.Equal("1,2,3", tree.RootElements);
            Assert.Equal(3, tree.Count);
        }

        // Theory Test: Extract various sets with different configurations
        [Theory]
        [InlineData("{1,2,3}", "1,2,3", 3, 3, 0)]
        [InlineData("{1,2,{3,4}}", "1,2", 3, 2, 1)]
        [InlineData("{10,20,30,40}", "10,20,30,40", 4, 4, 0)]
        [InlineData("{a,b,{c,d},{e,f}}", "a,b", 4, 2, 2)]
        public void Extract_TheoryTests(string input, string expectedRootElements, int expectedCardinality, int expectedRootCount, int expectedSubsetCount)
        {
            // Arrange
            var config = new SetExtractionConfiguration(";", ",");
            SetTreeExtractor<string> extractor = new SetTreeExtractor<string>();

            // Act
            ISetTree<string> tree = SetTreeExtractor<string>.Extract(input, config);

            // Assert
            Assert.Equal(expectedRootElements, tree.RootElements);
            Assert.Equal(expectedCardinality, tree.Count);
            Assert.Equal(expectedRootCount, tree.CountRootElements);
            Assert.Equal(expectedSubsetCount, tree.CountSubsets);
        }
    }

    // Custom converter for testing
    public class CustomStringToIntConverter : ICustomObjectConverter<int>
    {
        public static int ToObject(string?[] field)
        {
            // For simplicity, parse the string into an integer
            return int.Parse(field[0] ?? "0");
        }
    }
}//namespae