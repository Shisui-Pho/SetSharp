using SetsLibrary;
using SetsLibrary.Utility;
using Xunit;
namespace SetsLibrary.Tests.New_features;
public class SetsTreeBuilderWithNullSets
{
    //[Fact]
    //public void BuildSetTree_EmptySetExpression_ReturnsEmptyTree()
    //{
    //    // Arrange
    //    var config = new SetsConfigurations(",", ignoreEmptyFields: false);
    //    string input = "{}";

    //    // Act
    //    var result = SetTreeBuilder<int>.BuildSetTree(input, config);

    //    // Assert
    //    Assert.Equal("{}", result.ToString());
    //    Assert.Equal(0, result.CountRootElements);
    //    Assert.Equal(0, result.CountSubsets);
    //}

    //[Fact]
    //public void BuildSetTree_EmptyElements_NotIgnored_AppendsEmptySetSymbol()
    //{
    //    // Arrange
    //    var config = new SetsConfigurations(",", ignoreEmptyFields: false);
    //    string input = "{,}"; // empty elements

    //    // Act
    //    var result = SetTreeBuilder<int>.BuildSetTree(input, config);

    //    // Assert
    //    Assert.Equal("{{}{}}", result.ToString()); // two empty elements shown as {}
    //    Assert.True(result.TreeInfo.HasNullElements);
    //}

    //[Fact]
    //public void BuildSetTree_EmptyElementAndValidElements_MixedToString()
    //{
    //    // Arrange
    //    var config = new SetsConfigurations(",", ignoreEmptyFields: false);
    //    string input = "{1,,3}";

    //    // Act
    //    var result = SetTreeBuilder<int>.BuildSetTree(input, config);

    //    // Assert
    //    Assert.Equal("{{}1,3}", result.ToString());
    //    Assert.True(result.TreeInfo.HasNullElements);
    //    Assert.Equal(2, result.CountRootElements); // 1 and 3 only
    //}

    //[Fact]
    //public void BuildSetTree_EmptyNestedSubset_PrintsAsEmptySet()
    //{
    //    // Arrange
    //    var config = new SetsConfigurations(",", ignoreEmptyFields: false);
    //    string input = "{1,2,{},3}";

    //    // Act
    //    var result = SetTreeBuilder<int>.BuildSetTree(input, config);

    //    // Assert
    //    string output = result.ToString();
    //    Assert.Contains("{}", output); // empty nested subset
    //    Assert.Equal(3, result.CountRootElements);
    //    Assert.Equal(1, result.CountSubsets);

    //    var nested = Assert.Single(result.GetSubsetsEnumerator());
    //    Assert.Equal("{}", nested.ToString());
    //    Assert.True(nested.TreeInfo.IsEmptyTree);
    //}

    //[Fact]
    //public void BuildSetTree_EmptySet_WithIgnoreFlag_DoesNotPrintBraces()
    //{
    //    // Arrange
    //    var config = new SetsConfigurations(",", ignoreEmptyFields: true);
    //    string input = "{1,,3}";

    //    // Act
    //    var result = SetTreeBuilder<int>.BuildSetTree(input, config);

    //    // Assert
    //    Assert.Equal("{1,3}", result.ToString());
    //    Assert.False(result.TreeInfo.HasNullElements); // ignored
    //}
}///class
