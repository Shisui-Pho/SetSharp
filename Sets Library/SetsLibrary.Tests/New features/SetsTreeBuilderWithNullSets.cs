using SetsLibrary;
using SetsLibrary.Utility;
using Xunit;
namespace SetsLibrary.Tests.New_features;
public class SetsTreeBuilderWithNullSets
{
    [Fact]
    public void BuildSetTree_EmptySetExpression_ReturnsEmptyTree()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{}";

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{}", result.ToString());
        Assert.Equal(0, result.CountRootElements);
        Assert.Equal(0, result.CountSubsets);
    }

    [Fact]
    public void BuildSetTree_EmptyElements_NotIgnored_AppendsEmptySetSymbol()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: true);
        string input = "{,}"; // empty elements

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{}", result.ToString()); //
        Assert.True(result.TreeInfo.HasNullElements);
    }

    [Fact]
    public void BuildSetTree_EmptyElementAndValidElements_MixedToString()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{1,,3}";

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,3,{}}", result.ToString());
        Assert.True(result.TreeInfo.HasNullElements);
        Assert.Equal(2, result.CountRootElements); // 1 and 3 only
    }

    [Fact]
    public void BuildSetTree_EmptyNestedSubset_PrintsAsEmptySet()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{1,2,{},3}";

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        string output = result.ToString();
        Assert.Contains("{}", output); // empty nested subset
        Assert.Equal(3, result.CountRootElements);
        Assert.Equal(1, result.CountSubsets);

        var nested = Assert.Single(result.GetSubsetsEnumerator());
        Assert.Equal("{}", nested.ToString());
        Assert.True(nested.TreeInfo.IsEmptyTree);
    }

    [Fact]
    public void BuildSetTree_EmptySet_WithIgnoreFlag_DoesNotPrintBraces()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: true);
        string input = "{1,,3}";

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,3}", result.ToString());
        Assert.True(result.TreeInfo.HasNullElements);//Shout stull have null elements
    }
    [Fact]
    public void BuildSetTree_EmptyElementsInMixedSet_HandlesEmptyValues()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{1,,2,1,3,4,1,,8, ,6}"; // Some empty elements between valid values

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,2,3,4,6,8,{}}", result.ToString());
        Assert.True(result.TreeInfo.HasNullElements); // empty elements handled
    }

    [Fact]
    public void BuildSetTree_EmptySetWithinSetWithMultipleEmptyElements_ReturnsExpectedTree()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: true);
        string input = "{1,,2,3,{,}}"; // Nested empty set

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,2,3,{}}", result.ToString());
        Assert.Equal(3, result.CountRootElements); // 1, 2, and 3
        Assert.Equal(1, result.CountSubsets); // one nested empty set
    }

    [Fact]
    public void BuildSetTree_IgnoreEmptyFields_EmptyElementsAreIgnored()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: true);
        string input = "{1,,2,1,3,4,1,,8, ,6}"; // Empty elements should be ignored

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,2,3,4,6,8}", result.ToString()); // Empty values omitted
        Assert.True(result.TreeInfo.HasNullElements); //Should still have null elements
    }

    [Fact]
    public void BuildSetTree_EmptySetInTheMiddle_ReturnsCorrectStringRepresentation()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{5,6,,8,}"; // Empty element between 6 and 8

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{5,6,8,{}}", result.ToString());
        Assert.Equal(3, result.CountRootElements); // 5, 6, and 8
        Assert.True(result.TreeInfo.HasNullElements); // Empty elements included
    }

    [Fact]
    public void BuildSetTree_EmptySetsInComplexTree_NestedAndFlat()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{1,,2,{},3,{}}"; // Two empty sets

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,2,3,{}}", result.ToString());
        Assert.Equal(3, result.CountRootElements); // 1, 2, and 3
        Assert.Equal(1, result.CountSubsets); // Two empty subsets
    }

    [Fact]
    public void BuildSetTree_EmptySetsInSubsets_ShouldPrintCorrectly()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{1,{},{},3,{2}}"; // Empty sets in subsets

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,3,{},{2}}", result.ToString());
        Assert.Equal(2, result.CountRootElements); // Root element: 1
        Assert.Equal(2, result.CountSubsets); // Two subsets, with empty sets
    }

    [Fact]
    public void BuildSetTree_MultipleConsecutiveEmptyElements_ShouldBeHandledCorrectly()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{1,,,,,,2}"; // Multiple consecutive empty elements

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,2,{}}", result.ToString());
        Assert.True(result.TreeInfo.HasNullElements); // Empty elements handled as {}
    }

    [Fact]
    public void BuildSetTree_EmptySetWithElementsAtEnd_ShouldNotAffectStructure()
    {
        // Arrange
        var config = new SetsConfigurations(",", ignoreEmptyFields: false);
        string input = "{1,2,3,,}"; // Empty at the end

        // Act
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        // Assert
        Assert.Equal("{1,2,3,{}}", result.ToString());
        Assert.True(result.TreeInfo.HasNullElements); // Empty elements handled as {}
    }

}///class
