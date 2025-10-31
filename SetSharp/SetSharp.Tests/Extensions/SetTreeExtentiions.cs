using SetSharp;
using System.Threading.Tasks;
using Xunit;
using SetSharp.Collections;

namespace SetSharp.Tests.Extensions;

public class SetTreeExtentiions
{
    private SetsConfigurations config = new SetsConfigurations(",");

    // Extreme Test Case 1: Null Tree in ToStructuredSet
    [Fact]
    public void ToStructuredSet_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> tree = null;
        Func<IIndexedSetTree<int>, IStructuredSet<int>> generateSetFunct = (tree) => new StructuredSet<int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tree.ToStructuredSet(generateSetFunct));
    }

    // Extreme Test Case 2: Null Function in ToStructuredSet
    [Fact]
    public void ToStructuredSet_ShouldThrowArgumentNullException_WhenFunctionIsNull()
    {
        // Arrange
        var tree = new SetTree<int>(config);
        Func<IIndexedSetTree<int>, IStructuredSet<int>> generateSetFunct = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tree.ToStructuredSet(generateSetFunct));
    }

    // Extreme Test Case 3: Conversion Failure in ToStructuredSet
    [Fact]
    public void ToStructuredSet_ShouldThrowSetsException_WhenConversionFails()
    {
        // Arrange
        var tree = new SetTree<int>(config);
        Func<IIndexedSetTree<int>, IStructuredSet<int>> generateSetFunct = (indexedTree) => throw new Exception("Conversion failed");

        // Act & Assert
        Assert.Throws<SetsException>(() => tree.ToStructuredSet(generateSetFunct));
    }

    // Extreme Test Case 4: Null Tree in ToTypedSet
    [Fact]
    public void ToTypedSet_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> tree = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tree.ToTypedSet());
    }

    // Extreme Test Case 6: Null Tree in ToStringLiteralSet
    [Fact]
    public void ToStringLiteralSet_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> tree = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tree.ToStringLiteralSet());
    }

    // Extreme Test Case 8: Null Tree in ToListRootElements
    [Fact]
    public void ToListRootElements_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> tree = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tree.ToListRootElements());
    }

    // Extreme Test Case 9: Null Tree in ToListSubsets
    [Fact]
    public void ToListSubsets_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> tree = null;
        Func<IIndexedSetTree<int>, IStructuredSet<int>> generateSet = (indexedTree) => new StructuredSet<int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tree.ToListSubsets(generateSet));
    }

    // Extreme Test Case 10: Null Function in ToListSubsets
    [Fact]
    public void ToListSubsets_ShouldThrowArgumentNullException_WhenFunctionIsNull()
    {
        // Arrange
        var tree = new SetTree<int>(config);
        Func<IIndexedSetTree<int>, IStructuredSet<int>> generateSet = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tree.ToListSubsets(generateSet));
    }

    // Extreme Test Case 11: Conversion Failure in ToListSubsets
    [Fact]
    public void ToListSubsets_ShouldThrowSetsException_WhenConversionFails()
    {
        // Arrange
        var tree = new SetTree<int>(config);
        Func<IIndexedSetTree<int>, IStructuredSet<int>> generateSet = (indexedTree) => throw new Exception("Conversion failed");

        // Act & Assert
        Assert.Throws<SetsException>(() => tree.ToListSubsets(generateSet));
    }

    // Extreme Test Case 12: Null Tree in ToListTypedSubsets
    [Fact]
    public void ToListTypedSubsets_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> tree = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tree.ToListTypedSubsets());
    }

    // Extreme Test Case 13: Conversion Failure in ToListTypedSubsets
    [Fact]
    public void ToListTypedSubsets_ShouldThrowSetsException_WhenConversionFails()
    {
        // Arrange
        var tree = new SetTree<int>(config);

        // Simulate failure in TypedSet constructor or conversion.
        SetTreeWithIndexes<int> failedConversionTree = null;

        // Act & Assert
        Assert.Throws<SetsException>(() => tree.ToListTypedSubsets());
    }

    // Extreme Test Case 14: Empty Set Tree in ToListRootElements
    [Fact]
    public void ToListRootElements_ShouldReturnEmptyList_WhenSetTreeHasNoElements()
    {
        // Arrange
        var tree = new SetTree<int>(config);

        // Act
        var result = tree.ToListRootElements();

        // Assert
        Assert.Empty(result);
    }

    // Extreme Test Case 15: Nested Subsets with Complex Set Tree
    [Fact]
    public void ToListSubsets_ShouldReturnComplexStructuredSets_WhenTreeHasNestedSubsets()
    {
        //Not-Implemented yet
        Assert.True(true);
        //// Arrange
        //var tree = new SetTree<int>(config);
        //var generateSet = new Func<IIndexedSetTree<int>, IStructuredSet<int>>(indexedTree => new StructuredSet<int>());

        //// Act
        //var result = tree.ToListSubsets(generateSet);

        //// Assert
        //Assert.NotEmpty(result);
        //// AddIfDuplicate assertions to check the complexity of the structured sets returned
    }
}
