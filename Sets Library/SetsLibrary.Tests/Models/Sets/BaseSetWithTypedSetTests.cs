using SetsLibrary;
using Xunit;

namespace SetsLibrary.Tests.Models.Sets
{
    public class BaseSetWithTypedSetTests
    {
        private readonly SetExtractionConfiguration<int> _extractionConfig;

        public BaseSetWithTypedSetTests()
        {
            _extractionConfig = new SetExtractionConfiguration<int>("nothing", ",");
        }

        [Fact]
        public void Test_SetInitializationAndParsing()
        {
            string expression = "{0,3,4,1,5,2,0,0,0,0,0,1,4,2,6,8}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Expected set after parsing: {0,1,2,3,4,5,6,8}
            Assert.Equal("{0,1,2,3,4,5,6,8}", baseSet.BuildStringRepresentation());
            Assert.Equal(8, baseSet.Cardinality);
        }

        [Fact]
        public void Test_AddElement()
        {
            string expression = "{0,1,2,3,4,5}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            baseSet.AddElement(6);
            Assert.Contains(6, baseSet.EnumerateRootElements());
            Assert.Equal(7, baseSet.Cardinality); // New element added
        }

        [Fact]
        public void Test_RemoveElement()
        {
            string expression = "{0,1,2,3,4,5}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            baseSet.RemoveElement(3);
            Assert.DoesNotContain(3, baseSet.EnumerateRootElements());
            Assert.Equal(5, baseSet.Cardinality); // Element removed
        }

        [Fact]
        public void Test_MergeWith()
        {
            string expressionA = "{0,1,2,3,4,5}";
            string expressionB = "{5,6,7,8}";

            var setA = new TypedSet<int>(expressionA, _extractionConfig);
            var setB = new TypedSet<int>(expressionB, _extractionConfig);

            var mergedSet = setA.MergeWith(setB);
            Assert.Equal("{0,1,2,3,4,5,6,7,8}", mergedSet.BuildStringRepresentation());
            Assert.Equal(9, mergedSet.Cardinality); // Merged set has all elements
        }

        [Fact]
        public void Test_Without()
        {
            string expressionA = "{0,1,2,3,4,5}";
            string expressionB = "{2,4}";

            var setA = new TypedSet<int>(expressionA, _extractionConfig);
            var setB = new TypedSet<int>(expressionB, _extractionConfig);

            var resultSet = setA.Without(setB);
            Assert.Equal("{0,1,3,5}", resultSet.BuildStringRepresentation());
            Assert.Equal(4, resultSet.Cardinality); // Elements of setB removed
        }

        [Fact]
        public void Test_Subsets()
        {
            string expression = "{0,1,2,{3,4},5,{6,7}}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            Assert.Contains("{3,4}", baseSet.BuildStringRepresentation());
            Assert.Contains("{6,7}", baseSet.BuildStringRepresentation());
            Assert.Equal(6, baseSet.Cardinality); // Includes subsets
        }

        [Fact]
        public void Test_NestedSetsHandling()
        {
            string expression = "{0,1,2,{3,4},5,{6,7}, {8, {9, 10}}}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Verifying nested sets are correctly represented
            Assert.Contains("{3,4}", baseSet.BuildStringRepresentation());
            Assert.Contains("{6,7}", baseSet.BuildStringRepresentation());
            Assert.Contains("{9,10}", baseSet.BuildStringRepresentation());

            Assert.Equal(7, baseSet.Cardinality); // All unique elements including nested sets
        }

        [Fact]
        public void Test_SetInitializationWithDuplicates()
        {
            string expression = "{0,0,1,2,2,3,4,4,5}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Ensure duplicates are ignored, set should be {0,1,2,3,4,5}
            Assert.Equal("{0,1,2,3,4,5}", baseSet.BuildStringRepresentation());
            Assert.Equal(6, baseSet.Cardinality); // Cardinality should be unique elements count
        }
        [Fact]
        public void Test_ClearMethod()
        {
            string expression = "{0,1,2,3,4,5}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            baseSet.Clear();
            Assert.Equal("{\u2205}", baseSet.BuildStringRepresentation()); // Should be empty set
            Assert.Equal(0, baseSet.Cardinality); // Cardinality should be 0 after clearing
        }

        [Fact]
        public void Test_AddAndRemoveInSubsets()
        {
            string expression = "{0,1,2,{3,4},5}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            var subset = baseSet.EnumerateSubsets().ToList()[0]; // Get the subset {3,4}
            
            var str = subset.ToString();

            subset?.AddElement(6);

            str = subset.ToString();

            string strr = baseSet.BuildStringRepresentation();
            Assert.Contains(6, subset?.GetRootElementsEnumarator() ?? Enumerable.Empty<int>());

            // Ensure main set is updated after adding an element to the subset
            Assert.DoesNotContain("{6}", baseSet.BuildStringRepresentation());
            Assert.Contains("{3,4,6}", baseSet.BuildStringRepresentation());
        }

        [Fact]
        public void Test_CardinalityForComplexNestedSets()
        {
            string expression = "{0,1,2,{3,4,5,{6,7}},8}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Should count unique elements
            Assert.Equal(5, baseSet.Cardinality); //
        }

        [Fact]
        public void Test_MergeWithMultipleSets()
        {
            string expressionA = "{0,1,2,3}";
            string expressionB = "{3,4,5}";
            string expressionC = "{5,6,7}";

            var setA = new TypedSet<int>(expressionA, _extractionConfig);
            var setB = new TypedSet<int>(expressionB, _extractionConfig);
            var setC = new TypedSet<int>(expressionC, _extractionConfig);

            var mergedSet = setA.MergeWith(setB).MergeWith(setC);
            Assert.Equal("{0,1,2,3,4,5,6,7}", mergedSet.BuildStringRepresentation());
            Assert.Equal(8, mergedSet.Cardinality); // All elements from A, B, C combined
        }
        [Fact]
        public void Test_AddElement_2()
        {
            string expression = "{1,2,3}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Add an element
            baseSet.AddElement(4);
            Assert.Contains(4, baseSet.EnumerateRootElements());
            Assert.Equal(4, baseSet.Cardinality); // Should now have 4 elements
        }

        [Fact]
        public void Test_AddElement_AlreadyExists()
        {
            string expression = "{1,2,3}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Try adding an element that already exists
            baseSet.AddElement(3);
            Assert.Equal("{1,2,3}", baseSet.BuildStringRepresentation()); // No change in representation
            Assert.Equal(3, baseSet.Cardinality); // Cardinality should not increase
        }

        [Fact]
        public void Test_ContainsElement()
        {
            string expression = "{1,2,3,4}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Check if elements exist
            Assert.True(baseSet.Contains(2));
            Assert.False(baseSet.Contains(5)); // 5 is not in the set
        }

        [Fact]
        public void Test_RemoveElement_2()
        {
            string expression = "{1,2,3,4,5}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Remove an element
            baseSet.RemoveElement(3);
            Assert.DoesNotContain(3, baseSet.EnumerateRootElements());
            Assert.Equal(4, baseSet.Cardinality); // Should have 4 elements now
        }

        [Fact]
        public void Test_RemoveElement_NotPresent()
        {
            string expression = "{1,2,3}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Try removing an element not in the set
            bool result = baseSet.RemoveElement(4);
            Assert.False(result); // Should return false since element was not found
            Assert.Equal("{1,2,3}", baseSet.BuildStringRepresentation()); // Set should remain unchanged
        }

        [Fact]
        public void Test_MergeWith_EmptySet()
        {
            string expressionA = "{1,2,3}";
            string expressionB = "{}"; // Empty set

            var setA = new TypedSet<int>(expressionA, _extractionConfig);
            var setB = new TypedSet<int>(expressionB, _extractionConfig);

            var mergedSet = setA.MergeWith(setB);
            Assert.Equal("{1,2,3}", mergedSet.BuildStringRepresentation()); // Should be same as setA
            Assert.Equal(3, mergedSet.Cardinality); // Cardinality should be 3
        }

        [Fact]
        public void Test_WithoutMethod()
        {
            string expressionA = "{1,2,3,4,5}";
            string expressionB = "{3,4}";

            var setA = new TypedSet<int>(expressionA, _extractionConfig);
            var setB = new TypedSet<int>(expressionB, _extractionConfig);

            var resultSet = setA.Without(setB);
            Assert.Equal("{1,2,5}", resultSet.BuildStringRepresentation()); // Remove 3 and 4 from setA
            Assert.Equal(3, resultSet.Cardinality); // Cardinality should be 3
        }

        [Fact]
        public void Test_IsSubSetOf_ProperSubSet()
        {
            string expressionA = "{1,2,3}";
            string expressionB = "{1,2,3,4,5}";

            var setA = new TypedSet<int>(expressionA, _extractionConfig);
            var setB = new TypedSet<int>(expressionB, _extractionConfig);

            // A is a subset of B
            bool result = setA.IsSubSetOf(setB, out var type);
            Assert.True(result);
            Assert.Equal(SetResultType.ProperSet, type); // Should return SubSet type
        }

        [Fact]
        public void Test_IsSubSetOf_SubSet()
        {
            string expressionA = "{1,2,3,4}";
            string expressionB = "{1,2,3,4}";

            var setA = new TypedSet<int>(expressionA, _extractionConfig);
            var setB = new TypedSet<int>(expressionB, _extractionConfig);

            // A is a proper subset of B
            bool result = setA.IsSubSetOf(setB, out var type);
            Assert.True(result);
            Assert.Equal(SetResultType.SubSet & SetResultType.Same_Set, type); // Should return ProperSet type
        }

        [Fact]
        public void Test_IsNotSubSetOf()
        {
            string expressionA = "{1,2,3}";
            string expressionB = "{4,5,6}";

            var setA = new TypedSet<int>(expressionA, _extractionConfig);
            var setB = new TypedSet<int>(expressionB, _extractionConfig);

            // A is not a subset of B
            bool result = setA.IsSubSetOf(setB, out var type);
            Assert.False(result);
            Assert.Equal(SetResultType.NotASubSet, type); // Should return NotASubSet type
        }

        [Fact]
        public void Test_EnumerateRootElements()
        {
            string expression = "{1,2,3,4}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            var rootElements = baseSet.EnumerateRootElements().ToList();
            Assert.Equal(4, rootElements.Count); // Should contain 4 root elements
            Assert.Contains(1, rootElements);
            Assert.Contains(4, rootElements);
        }

        [Fact]
        public void Test_EnumerateSubsets()
        {
            string expression = "{1,2,3,{4,5},{6,7}}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            var subsets = baseSet.EnumerateSubsets().ToList();
            Assert.Equal(2, subsets.Count); // Should have two subsets: {4,5} and {6,7}
            Assert.Contains("{4,5}", subsets[0].ToString());
            Assert.Contains("{6,7}", subsets[1].ToString());
        }

        [Fact]
        public void Test_NestedSetCardinality()
        {
            string expression = "{1,2,3,{4,5},6,{7,8}}";
            var baseSet = new TypedSet<int>(expression, _extractionConfig);

            // Cardinality should include all unique elements
            Assert.Equal(6, baseSet.Cardinality); // Should count: {1,2,3,4,5,6,7,8}
        }
        // Theory to test AddElement behavior with multiple sets of integers
        [Theory]
        [InlineData("{1,2,3}", 4, "{1,2,3,4}", 4)]
        [InlineData("{5,6,7}", 7, "{5,6,7}", 3)] // Trying to add an existing element
        [InlineData("{10,20,30}", 40, "{10,20,30,40}", 4)]
        [InlineData("{}", 100, "{100}", 1)] // Adding the first element to an empty set
        public void Test_AddElement_Theory(string initialExpression, int elementToAdd, string expectedExpression, int expectedCardinality)
        {
            var baseSet = new TypedSet<int>(initialExpression, _extractionConfig);

            // Add the element
            baseSet.AddElement(elementToAdd);

            // Assert the resulting set matches the expected string representation
            Assert.Equal(expectedExpression, baseSet.BuildStringRepresentation());

            // Assert the cardinality is as expected
            Assert.Equal(expectedCardinality, baseSet.Cardinality);
        }

    }//class
}//namespace
