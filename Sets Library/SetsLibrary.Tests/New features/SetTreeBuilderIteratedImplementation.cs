using SetsLibrary.Utility;
using Xunit;

namespace SetsLibrary.Tests.New_features
{
    public class SetTreeBuilderIteratedImplementation
    {
        [Fact]
        public void BuildSetTree_WithDuplicateElements_RemovesDuplicates()
        {
            var config = new SetsConfigurations(",", addBraces: false);
            string input = "2,2,1,3,1";
            var result = SetTreeBuilder<int>.BuildSetTree(input, config);

            var rootElements = new List<int>(result.GetRootElementsEnumerator());
            Assert.Equal(3, rootElements.Count);
            Assert.Equal(new List<int> { 1, 2, 3 }, rootElements); // sorted
        }

        [Fact]
        public void BuildSetTree_NestedSet_BuildsSubTree()
        {
            var config = new SetsConfigurations(",", addBraces: false);
            string input = "{1,2,{3,4}}";

            var result = SetTreeBuilder<int>.BuildSetTree(input, config);

            Assert.Equal(2, result.CountRootElements); // 1 and 2
            Assert.Equal(1, result.CountSubsets);      // One subset {3,4}

            var subsets = result.GetSubsetsEnumerator();
            var subset = Assert.Single(subsets);
            Assert.Equal(2, subset.CountRootElements);
        }

        [Fact]
        public void BuildSetTree_InvalidExpression_ThrowsMissingBraceException()
        {
            var config = new SetsConfigurations(",", addBraces: false);
            string input = "{1,2,{3,4}";

            var ex = Assert.Throws<MissingBraceException>(() =>
                SetTreeBuilder<int>.BuildSetTree(input, config)
            );

            Assert.Contains("Missing closing braces", ex.Message);
        }

        [Fact]
        public void SortAndRemoveDuplicates_HandlesEmptyValues()
        {
            var config = new SetsConfigurations(",", addBraces: false);
            string input = "1,,3";

            var result = SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out var hasNulls, out var nullCount);

            Assert.Equal(new List<int> { 1, 3 }, new List<int>(result));
            Assert.True(hasNulls);
            Assert.Equal(1, nullCount);
        }
    }
}
