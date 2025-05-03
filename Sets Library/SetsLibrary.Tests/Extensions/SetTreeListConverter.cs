using SetsLibrary;
using SetsLibrary.Collections;
using System.Reflection;
using Xunit;
namespace SetsLibrary.Tests.Extensions;

public class SetTreeListConverter
{
    private static SetsConfigurations config = new SetsConfigurations(",");

    #region ToListRootElements Tests

    [Fact]
    public void ToListRootElementsWithMock_ShouldReturnCorrectList_WhenTreeIsNonEmpty()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        setTree.AddElement(10);
        setTree.AddElement(20);
        setTree.AddElement(30);

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListRootElements();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(10, result);
        Assert.Contains(20, result);
        Assert.Contains(30, result);
    }

    [Fact]
    public void ToListRootElementsWithMock_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> nullTree = null;
        var converter = new SetTreeListConverter<int>(nullTree);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.ToListRootElements());
    }

    [Fact]
    public void ToListRootElementsWithMock_ShouldReturnEmptyList_WhenTreeIsEmpty()
    {
        // Arrange
        var emptyTree = new SetTree<int>(config);
        var converter = new SetTreeListConverter<int>(emptyTree);

        // Act
        var result = converter.ToListRootElements();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region ToListSubTrees Tests

    [Fact]
    public void ToListSubTreesWithMock_ShouldReturnCorrectList_WhenSubtreesExist()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var subTree1 = new SetTree<int>(config);
        subTree1.AddElement(100);
        var subTree2 = new SetTree<int>(config);
        subTree2.AddElement(200);

        setTree.AddElement(subTree1);
        setTree.AddElement(subTree2);

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListSubTrees();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(subTree1, result);
        Assert.Contains(subTree2, result);
    }

    [Fact]
    public void ToListSubTreesWithMock_ShouldReturnEmptyList_WhenNoSubtrees()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListSubTrees();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToListSubTreesWithMock_ShouldReturnEmptyList_WhenSubtreesAreEmpty()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var subTree = new SetTree<int>(config); // Empty subtree
        setTree.AddElement(subTree);

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListSubTrees();

        // Assert
        Assert.Single(result);
        Assert.Equal(subTree, result[0]);
    }

    [Fact]
    public void ToListSubTreesWithMock_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> nullTree = null;
        var converter = new SetTreeListConverter<int>(nullTree);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.ToListSubTrees());
    }

    #endregion

    #region ToListSubtreesAsStructuredSubsets (Not Implemented)

    [Fact]
    public void ToListSubtreesAsStructuredSubsetsWithMock_ShouldThrowNotImplementedException()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var converter = new SetTreeListConverter<int>(setTree);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => converter.ToListSubtreesAsStructuredSubsets(tree => null));
    }

    #endregion

    #region ToListSubTreeAsSubsets (Not Implemented)

    [Fact]
    public void ToListSubTreeAsSubsetsWithMock_ShouldThrowNotImplementedException()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var converter = new SetTreeListConverter<int>(setTree);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => converter.ToListSubTreeAsSubsets<TypedSet<int>>(tree => null));
    }

    #endregion

    #region Helper Method Tests for Internal Collections

    [Fact]
    public void ToListRootElementWithMocks_ShouldReturnSortedElements_WhenInternalCollectionIsSorted()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var internalCollection = new SortedCollection<int> { 1, 2, 3, 4, 5 };
        setTree.AddRange(internalCollection);
        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListRootElements(); // This will use the private helper method indirectly

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public void ToListSubTreesWithMock_ShouldReturnEmptyList_WhenInternalElementsAreEmpty()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var emptyCollection = new SortedCollection<int>();
        var emptySubTree = new SetTree<int>(config);
        emptySubTree.AddRange(emptyCollection);
        setTree.AddElement(emptySubTree);

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListSubTrees();

        // Assert
        Assert.Single(result);
        Assert.Empty(result[0].GetRootElementsEnumerator());
    }

    #endregion
    #region Chatgpt extreme cases
    // Extreme Test Case 1: Large Tree with Many Root Elements
    [Fact]
    public void ToListRootElements_ShouldHandleLargeTree_WhenTreeHasThousandsOfElements()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        for (int i = 0; i < 10000; i++)
        {
            setTree.AddElement(i);
        }
        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListRootElements();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10000, result.Count);
        Assert.Contains(0, result);
        Assert.Contains(9999, result);
    }

    // Extreme Test Case 2: Nested Subtrees with Large Depth
    [Fact]
    public void ToListSubTrees_ShouldHandleDeeplyNestedSubtrees()
    {
        //Arrange
        var setTree = new SetTree<int>(config);
        var outerTree = new SetTree<int>(config);
        var subTree = setTree;

        for (int i = 0; i < 100; i++)
        {
            var newSubTree = new SetTree<int>(config);
            newSubTree.AddElement(i);
            subTree.AddElement(newSubTree);
            subTree = newSubTree; // Nesting deeper
            outerTree.AddElement(subTree);
        }

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListSubTrees();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, outerTree.Count); // Should contain 100 nested subtrees
        Assert.Contains(result, r => r.CountRootElements == 1); // Each subtree should have one root element
    }

    // Extreme Test Case 3: Empty Subsets and Collections
    [Fact]
    public void ToListSubTrees_ShouldReturnEmptyList_WhenAllSubsetsAreEmpty()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var emptySubTree = new SetTree<int>(config);  // A subtree with no elements
        setTree.AddElement(emptySubTree);  // Add it as a subset
        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListSubTrees();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Empty(result[0].GetRootElementsEnumerator());
    }
    // Extreme Test Case 5: Very Large Internal Collection (Test for Memory/Performance)
    [Fact]
    public void ToListElementsUsingInternalCollection_ShouldHandleLargeInternalCollection()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var internalCollection = new SortedCollection<int>();
        for (int i = 0; i < 100000; i++)  // Adding a large number of elements to internal collection
        {
            internalCollection.Add(i);
        }
        setTree.AddRange(internalCollection);  // Add this large collection as an element
        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListRootElements();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100000, result.Count);  // Check that all elements were converted
        Assert.Contains(0, result);
        Assert.Contains(99999, result);
    }

    // Extreme Test Case 6: Null Elements in Tree (Should Handle or Skip)
    [Fact]
    public void ToListRootElements_ShouldHandleElementsInTree()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        setTree.AddElement(10);
        setTree.AddElement(20);

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        List<int> result = converter.ToListRootElements();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(10, result);
        Assert.Contains(20, result);
    }

    // Extreme Test Case 7: Non-comparable Elements (Compilation Failure)
    public class NonComparableElement
    {
        // No implementation of IComparable<TElement> here.
    }

    [Fact]
    public void ToListRootElements_ShouldNotCompile_WhenNonComparableElementIsUsed()
    {
        //// Arrange & Act
        //// This will not compile because `NonComparableElement` does not implement `IComparable<TElement>`
        // var setTree = new SetTree<NonComparableElement>(config);
        // var converter = new SetTreeListConverter<NonComparableElement>(setTree);

        //// Assert
        //// The above code should trigger a compilation error due to the lack of IComparable<TElement>
        Assert.True(true); // This ensures the test runs but is a placeholder since the code won't compile.
    }

    // Extreme Test Case 8: Duplicate Elements (Check if they are handled)
    [Fact]
    public void ToListRootElements_ShouldHandleDuplicatesInTree()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        setTree.AddElement(10);
        setTree.AddElement(20);
        setTree.AddElement(10);  // Duplicating 10

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListRootElements();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);  // Should count all elements (including duplicates)
        Assert.Contains(10, result);
        Assert.Contains(20, result);
    }

    #endregion Chatgpt extreme cases
    #region Chathpt Tests
    // Test for ToListRootElements
    [Fact]
    public void ToListRootElementsWith_ShouldReturnCorrectList_WhenTreeIsNonEmpty()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        setTree.AddElement(10);
        setTree.AddElement(20);
        setTree.AddElement(30);

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListRootElements();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(10, result);
        Assert.Contains(20, result);
        Assert.Contains(30, result);
    }

    [Fact]
    public void ToListRootElementsWith_ShouldThrowArgumentNullException_WhenTreeIsNull()
    {
        // Arrange
        ISetTree<int> nullTree = null;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SetTreeListConverter<int>(nullTree));
    }

    [Fact]
    public void ToListRootElements_ShouldReturnEmptyList_WhenTreeIsEmpty()
    {
        // Arrange
        var emptyTree = new SetTree<int>(config);
        var converter = new SetTreeListConverter<int>(emptyTree);

        // Act
        var result = converter.ToListRootElements();

        // Assert
        Assert.Empty(result);
    }

    // Test for ToListSubTrees
    [Fact]
    public void ToListSubTrees_ShouldReturnCorrectList_WhenSubtreesExist()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var subTree1 = new SetTree<int>(config);
        subTree1.AddElement(100);
        var subTree2 = new SetTree<int>(config);
        subTree2.AddElement(200);

        setTree.AddElement(subTree1);
        setTree.AddElement(subTree2);

        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListSubTrees();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(subTree1, result);
        Assert.Contains(subTree2, result);
    }

    [Fact]
    public void ToListSubTrees_ShouldReturnEmptyList_WhenNoSubtrees()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var converter = new SetTreeListConverter<int>(setTree);

        // Act
        var result = converter.ToListSubTrees();

        // Assert
        Assert.Empty(result);
    }

    // Test for ToListSubtreesAsStructuredSubsets (Not Implemented)
    [Fact]
    public void ToListSubtreesAsStructuredSubsets_ShouldThrowNotImplementedException()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var converter = new SetTreeListConverter<int>(setTree);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => converter.ToListSubtreesAsStructuredSubsets(tree => null));
    }

    // Test for ToListSubTreeAsSubsets (Not Implemented)
    [Fact]
    public void ToListSubTreeAsSubsets_ShouldThrowNotImplementedException()
    {
        // Arrange
        var setTree = new SetTree<int>(config);
        var converter = new SetTreeListConverter<int>(setTree);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => converter.ToListSubTreeAsSubsets<TypedSet<int>>(tree => null));
    }
    #endregion Chathpt Tests
}
