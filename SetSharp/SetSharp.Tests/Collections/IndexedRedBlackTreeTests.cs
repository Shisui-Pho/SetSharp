/*
 * File: IndexedRedBlackTreeTests.cs
 *
 * Description:
 * Comprehensive xUnit test coverage for IndexedRedBlackTree<TElement>, intended as a
 * regression safety net ahead of internal optimization work. Tests are written purely
 * against the PUBLIC surface of the class (Add, AddRange, Remove, RemoveAt, TryRemove,
 * the indexer, IndexOf, Contains, Min, Max, Clear, enumeration) so they remain valid
 * regardless of how the internal tree structure/balancing is implemented.
 *
 * NOTE - suspected pre-existing bug:
 * While tracing AddRange -> SwapTreesAddRange -> InsertExchangeSortedBFT, the private
 * constructor `IndexedRedBlackTree(Node root)` calls Clear() (which zeroes Count) AFTER
 * the caller already has a populated node graph for `root`. That stray zero Count causes
 * the subsequent `tree.AddRange(this)` call inside InsertExchangeSortedBFT to take the
 * "target is empty" branch (InsertDefaultSortedBFT), which overwrites the freshly-copied
 * large tree with a copy of the small tree instead of merging the two. The outer code
 * still does `Count += sorted.Count`, so the reported Count ends up correct while the
 * actual element data from the larger incoming tree is silently discarded.
 *
 * This only manifests when AddRange-ing a much larger IndexedRedBlackTree into a smaller,
 * NON-EMPTY tree (ratio of target.Count / source.Count <= 0.2). See the
 * AddRange_Ratio*Threshold* tests below, which bracket the exact boundary.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using SetSharp.Collections;
using Xunit;

namespace SetSharp.Collections.Tests;

public class IndexedRedBlackTreeTests
{
    #region Test helpers

    private static List<int> ShuffledRange(int count, int seed)
    {
        var rng = new Random(seed);
        var list = Enumerable.Range(0, count).ToList();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }

    private static List<int> RandomValues(int count, int seed, int maxValue = int.MaxValue)
    {
        var rng = new Random(seed);
        var list = new List<int>(count);
        for (int i = 0; i < count; i++)
            list.Add(rng.Next(maxValue));
        return list;
    }

    /// <summary>
    /// Verifies that the tree's reported Count, IsEmpty, full enumeration order, and every
    /// indexer position are all consistent with the sorted version of <paramref name="sourceValues"/>.
    /// </summary>
    private static void AssertSortedConsistency(IndexedRedBlackTree<int> tree, List<int> sourceValues)
    {
        var expectedSorted = sourceValues.OrderBy(x => x).ToList();

        Assert.Equal(expectedSorted.Count, tree.Count);
        Assert.Equal(expectedSorted.Count == 0, tree.IsEmpty);

        var actual = tree.ToList();
        Assert.Equal(expectedSorted, actual);

        for (int i = 0; i < expectedSorted.Count; i++)
        {
            Assert.Equal(expectedSorted[i], tree[i]);
        }
    }

    #endregion

    #region Construction

    [Fact]
    public void Constructor_Default_CreatesEmptyTree()
    {
        var tree = new IndexedRedBlackTree<int>();

        Assert.Equal(0, tree.Count);
        Assert.True(tree.IsEmpty);
    }

    [Fact]
    public void Constructor_WithElements_PopulatesTreeInSortedOrder()
    {
        var values = new[] { 5, 3, 8, 1, 9, 2 };
        var tree = new IndexedRedBlackTree<int>(values);

        AssertSortedConsistency(tree, values.ToList());
    }

    [Fact]
    public void Constructor_WithEmptyCollection_CreatesEmptyTree()
    {
        var tree = new IndexedRedBlackTree<int>(Enumerable.Empty<int>());

        Assert.Equal(0, tree.Count);
        Assert.True(tree.IsEmpty);
    }

    [Fact]
    public void Constructor_WithNullCollection_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new IndexedRedBlackTree<int>(null!));
    }

    [Fact]
    public void Constructor_WithDuplicateElements_RetainsAllDuplicates()
    {
        var values = new[] { 4, 4, 4, 1, 2 };
        var tree = new IndexedRedBlackTree<int>(values);

        Assert.Equal(values.Length, tree.Count);
        Assert.Equal(values.OrderBy(x => x), tree.ToList());
    }

    #endregion

    #region Add / AddRange

    [Fact]
    public void Add_SingleItem_IncreasesCountAndIsRetrievable()
    {
        var tree = new IndexedRedBlackTree<int>();

        tree.Add(42);

        Assert.Equal(1, tree.Count);
        Assert.False(tree.IsEmpty);
        Assert.True(tree.Contains(42));
        Assert.Equal(42, tree[0]);
    }

    [Fact]
    public void Add_Null_ThrowsArgumentNullException()
    {
        var tree = new IndexedRedBlackTree<string>();

        Assert.Throws<ArgumentNullException>(() => tree.Add(null!));
    }

    [Fact]
    public void Add_MultipleItems_MaintainsSortedOrderAtEveryStep()
    {
        var tree = new IndexedRedBlackTree<int>();
        var values = new List<int>();

        foreach (var v in new[] { 50, 20, 80, 10, 30, 70, 90, 5, 15 })
        {
            tree.Add(v);
            values.Add(v);
            AssertSortedConsistency(tree, values);
        }
    }

    [Fact]
    public void Add_DuplicateItems_AreAllRetained()
    {
        var tree = new IndexedRedBlackTree<int>();
        tree.Add(7);
        tree.Add(7);
        tree.Add(7);

        Assert.Equal(3, tree.Count);
        Assert.Equal(7, tree[0]);
        Assert.Equal(7, tree[1]);
        Assert.Equal(7, tree[2]);
    }

    [Fact]
    public void AddRange_Null_ThrowsArgumentNullException()
    {
        var tree = new IndexedRedBlackTree<int>();

        Assert.Throws<ArgumentNullException>(() => tree.AddRange(null!));
    }

    [Fact]
    public void AddRange_WithPlainEnumerable_AddsAllItems()
    {
        var tree = new IndexedRedBlackTree<int>();
        var values = new List<int> { 5, 1, 9, 3 };

        tree.AddRange(values);

        AssertSortedConsistency(tree, values);
    }

    [Fact]
    public void AddRange_WithEmptyEnumerable_LeavesTreeUnchanged()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        tree.AddRange(Enumerable.Empty<int>());

        AssertSortedConsistency(tree, new List<int> { 1, 2, 3 });
    }

    [Fact]
    public void AddRange_IncomingEmptyIndexedTree_LeavesTreeUnchanged()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3, 4, 5 });
        var empty = new IndexedRedBlackTree<int>();

        // Exercises the (float)Count / sorted.Count division-by-zero path (Infinity);
        // should be a safe no-op rather than throwing.
        tree.AddRange(empty);

        AssertSortedConsistency(tree, new List<int> { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public void AddRange_BothTreesEmpty_NoExceptionAndStaysEmpty()
    {
        var tree = new IndexedRedBlackTree<int>();
        var empty = new IndexedRedBlackTree<int>();

        // ratio = 0 / 0 = NaN here; should not throw.
        tree.AddRange(empty);

        Assert.True(tree.IsEmpty);
        Assert.Equal(0, tree.Count);
    }

    [Fact]
    public void AddRange_TargetEmpty_SourceNonEmptyIndexedTree_CopiesAllElements()
    {
        // this.Count == 0 -> ratio is always <= 0.2, takes the InsertDefaultSortedBFT path.
        var target = new IndexedRedBlackTree<int>();
        var source = new IndexedRedBlackTree<int>(new[] { 10, 30, 20, 5, 15 });

        target.AddRange(source);

        AssertSortedConsistency(target, new List<int> { 10, 30, 20, 5, 15 });
    }

    [Fact]
    public void AddRange_RatioAboveThreshold_NaiveMergeProducesCorrectUnion()
    {
        // ratio = 3 / 10 = 0.3 > 0.2 -> falls back to the simple per-item Add loop.
        var target = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });
        var source = new IndexedRedBlackTree<int>(Enumerable.Range(100, 10));

        target.AddRange(source);

        var expected = new List<int> { 1, 2, 3 }.Concat(Enumerable.Range(100, 10)).ToList();
        AssertSortedConsistency(target, expected);
    }

    [Fact]
    public void AddRange_RatioExactlyAtThreshold_TargetNonEmpty_MergesCorrectly()
    {
        // ratio = 2 / 10 = 0.2 <= MIN_CHANGE_STRUCTURE_RATIO -> SwapTreesAddRange takes the
        // InsertExchangeSortedBFT path (target is non-empty).
        //
        // Per the file header: based on a manual trace, this currently looks like it will
        // FAIL - Count comes out correct (12) but the larger incoming tree's data appears to
        // be discarded. Left as a normal (non-skipped) assertion so it's visible when run.
        var target = new IndexedRedBlackTree<int>(new[] { 1, 2 });
        var source = new IndexedRedBlackTree<int>(Enumerable.Range(100, 10));

        target.AddRange(source);

        var expected = new List<int> { 1, 2 }.Concat(Enumerable.Range(100, 10)).ToList();
        AssertSortedConsistency(target, expected);
    }

    [Fact]
    public void AddRange_RatioJustAboveThreshold_TargetNonEmpty_MergesCorrectly()
    {
        // ratio = 3 / 10 = 0.3 > 0.2 -> naive Add loop, should be unaffected by the
        // InsertExchangeSortedBFT issue above. Included to bracket the bug precisely.
        var target = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });
        var source = new IndexedRedBlackTree<int>(Enumerable.Range(100, 10));

        target.AddRange(source);

        var expected = new List<int> { 1, 2, 3 }.Concat(Enumerable.Range(100, 10)).ToList();
        AssertSortedConsistency(target, expected);
    }

    [Theory]
    [InlineData(1, 50)]
    [InlineData(5, 100)]
    [InlineData(50, 60)]
    public void AddRange_VariousRatios_RandomValues_ProducesCorrectUnion(int targetSize, int sourceSize)
    {
        var targetValues = RandomValues(targetSize, seed: targetSize * 31 + sourceSize);
        var sourceValues = RandomValues(sourceSize, seed: sourceSize * 17 + targetSize);

        var target = new IndexedRedBlackTree<int>(targetValues);
        var source = new IndexedRedBlackTree<int>(sourceValues);

        target.AddRange(source);

        AssertSortedConsistency(target, targetValues.Concat(sourceValues).ToList());
    }

    #endregion

    #region Contains

    [Fact]
    public void Contains_ExistingItem_ReturnsTrue()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        Assert.True(tree.Contains(2));
    }

    [Fact]
    public void Contains_NonExistingItem_ReturnsFalse()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        Assert.False(tree.Contains(99));
    }

    [Fact]
    public void Contains_EmptyTree_ReturnsFalse()
    {
        var tree = new IndexedRedBlackTree<int>();

        Assert.False(tree.Contains(1));
    }

    [Fact]
    public void Contains_Null_ThrowsArgumentNullException()
    {
        var tree = new IndexedRedBlackTree<string>(new[] { "a", "b" });

        Assert.Throws<ArgumentNullException>(() => tree.Contains(null!));
    }

    #endregion

    #region IndexOf

    [Fact]
    public void IndexOf_ExistingItem_ReturnsIndexMatchingSortedPosition()
    {
        var values = new[] { 50, 20, 80, 10, 30, 70, 90 };
        var tree = new IndexedRedBlackTree<int>(values);
        var sorted = values.OrderBy(x => x).ToList();

        foreach (var v in values)
        {
            Assert.Equal(sorted.IndexOf(v), tree.IndexOf(v));
        }
    }

    [Fact]
    public void IndexOf_NonExistingItem_ReturnsNegativeOne()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        Assert.Equal(-1, tree.IndexOf(99));
    }

    [Fact]
    public void IndexOf_EmptyTree_ReturnsNegativeOne()
    {
        var tree = new IndexedRedBlackTree<int>();

        Assert.Equal(-1, tree.IndexOf(1));
    }

    [Fact]
    public void IndexOf_WithDuplicates_ReturnsAValidMatchingIndex()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 5, 3, 5, 1, 5 });

        int index = tree.IndexOf(5);

        Assert.InRange(index, 0, tree.Count - 1);
        Assert.Equal(5, tree[index]);
    }

    #endregion

    #region Indexer

    [Fact]
    public void Indexer_ValidIndices_ReturnSortedElements()
    {
        var values = new[] { 9, 1, 5, 3, 7 };
        var tree = new IndexedRedBlackTree<int>(values);
        var sorted = values.OrderBy(x => x).ToList();

        for (int i = 0; i < sorted.Count; i++)
            Assert.Equal(sorted[i], tree[i]);
    }

    [Fact]
    public void Indexer_NegativeIndex_ThrowsIndexOutOfRangeException()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        Assert.Throws<IndexOutOfRangeException>(() => tree[-1]);
    }

    [Fact]
    public void Indexer_IndexEqualToCount_ThrowsIndexOutOfRangeException()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        Assert.Throws<IndexOutOfRangeException>(() => tree[3]);
    }

    [Fact]
    public void Indexer_EmptyTree_ThrowsIndexOutOfRangeException()
    {
        var tree = new IndexedRedBlackTree<int>();

        Assert.Throws<IndexOutOfRangeException>(() => tree[0]);
    }

    #endregion

    #region Enumeration

    [Fact]
    public void GetEnumerator_ReturnsElementsInSortedOrder()
    {
        var values = new[] { 4, 1, 7, 2, 9, 3 };
        var tree = new IndexedRedBlackTree<int>(values);

        Assert.Equal(values.OrderBy(x => x), tree.ToList());
    }

    [Fact]
    public void GetEnumerator_EmptyTree_YieldsNoElements()
    {
        var tree = new IndexedRedBlackTree<int>();

        Assert.Empty(tree.ToList());
    }

    [Fact]
    public void NonGenericEnumerator_Works()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 3, 1, 2 });
        System.Collections.IEnumerable nonGeneric = tree;

        var collected = new List<int>();
        foreach (var item in nonGeneric)
            collected.Add((int)item);

        Assert.Equal(new[] { 1, 2, 3 }, collected);
    }

    #endregion

    #region Min / Max

    [Fact]
    public void Min_ReturnsSmallestElement()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 5, 1, 9, 3 });

        Assert.Equal(1, tree.Min());
    }

    [Fact]
    public void Max_ReturnsLargestElement()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 5, 1, 9, 3 });

        Assert.Equal(9, tree.Max());
    }

    [Fact]
    public void Min_EmptyTree_ThrowsInvalidOperationException()
    {
        var tree = new IndexedRedBlackTree<int>();

        Assert.Throws<InvalidOperationException>(() => tree.Min());
    }

    [Fact]
    public void Max_EmptyTree_ThrowsInvalidOperationException()
    {
        var tree = new IndexedRedBlackTree<int>();

        Assert.Throws<InvalidOperationException>(() => tree.Max());
    }

    [Fact]
    public void MinAndMax_SingleElementTree_AreEqual()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 42 });

        Assert.Equal(42, tree.Min());
        Assert.Equal(42, tree.Max());
    }

    #endregion

    #region Clear

    [Fact]
    public void Clear_ResetsCountAndIsEmpty()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        tree.Clear();

        Assert.Equal(0, tree.Count);
        Assert.True(tree.IsEmpty);
    }

    [Fact]
    public void Clear_AllowsReAddingElementsAfterwards()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });
        tree.Clear();

        tree.Add(10);
        tree.Add(5);

        AssertSortedConsistency(tree, new List<int> { 10, 5 });
    }

    #endregion

    #region Remove / TryRemove / RemoveAt

    [Fact]
    public void TryRemove_ExistingItem_ReturnsTrueWithRemovedData()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        bool result = tree.TryRemove(2, out int removedData);

        Assert.True(result);
        Assert.Equal(2, removedData);
        Assert.Equal(2, tree.Count);
        Assert.False(tree.Contains(2));
    }

    [Fact]
    public void TryRemove_NonExistingItem_ReturnsFalse()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        bool result = tree.TryRemove(99, out int removedData);

        Assert.False(result);
        Assert.Equal(default,removedData);
        Assert.Equal(3, tree.Count);
    }

    [Fact]
    public void Remove_ExistingItem_ReturnsTrueAndDecreasesCount()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        bool result = tree.Remove(2);

        Assert.True(result);
        Assert.Equal(2, tree.Count);
        Assert.False(tree.Contains(2));
    }

    [Fact]
    public void Remove_NonExistingItem_ReturnsFalseAndLeavesTreeUnchanged()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        bool result = tree.Remove(99);

        Assert.False(result);
        Assert.Equal(3, tree.Count);
    }

    [Fact]
    public void Remove_DuplicateValues_RemovesOneInstanceAtATime()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 7, 7, 7, 1 });

        Assert.True(tree.Remove(7));
        Assert.Equal(3, tree.Count);
        Assert.True(tree.Remove(7));
        Assert.Equal(2, tree.Count);
        Assert.True(tree.Remove(7));
        Assert.Equal(1, tree.Count);
        Assert.False(tree.Contains(7));
        Assert.True(tree.Contains(1));
    }

    [Fact]
    public void RemoveAt_ValidIndex_RemovesAndReturnsElement()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3, 4, 5 });

        int removed = tree.RemoveAt(2); // sorted: [1,2,3,4,5] -> index 2 == 3

        Assert.Equal(3, removed);
        Assert.Equal(4, tree.Count);
        Assert.Equal(new[] { 1, 2, 4, 5 }, tree.ToList());
    }

    [Fact]
    public void RemoveAt_NegativeIndex_ThrowsIndexOutOfRangeException()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        Assert.Throws<IndexOutOfRangeException>(() => tree.RemoveAt(-1));
    }

    [Fact]
    public void RemoveAt_IndexEqualToCount_ThrowsIndexOutOfRangeException()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        Assert.Throws<IndexOutOfRangeException>(() => tree.RemoveAt(3));
    }

    [Fact]
    public void RemoveAt_RepeatedlyAtZero_DrainsTreeInAscendingOrder()
    {
        var values = ShuffledRange(40, seed: 7);
        var tree = new IndexedRedBlackTree<int>(values);
        var expectedSorted = values.OrderBy(x => x).ToList();

        foreach (var expectedValue in expectedSorted)
        {
            var removed = tree.RemoveAt(0);
            Assert.Equal(expectedValue, removed);
        }

        Assert.True(tree.IsEmpty);
        Assert.Equal(0, tree.Count);
    }

    [Fact]
    public void RemoveAt_RepeatedlyAtLastIndex_DrainsTreeInDescendingOrder()
    {
        var values = ShuffledRange(40, seed: 11);
        var tree = new IndexedRedBlackTree<int>(values);
        var expectedDescending = values.OrderByDescending(x => x).ToList();

        foreach (var expectedValue in expectedDescending)
        {
            var removed = tree.RemoveAt(tree.Count - 1);
            Assert.Equal(expectedValue, removed);
        }

        Assert.True(tree.IsEmpty);
    }

    [Fact]
    public void Remove_RootValue_MaintainsRemainingTreeIntegrity()
    {
        var values = new[] { 50, 25, 75, 10, 30, 60, 90 };
        var tree = new IndexedRedBlackTree<int>(values);

        // 50 is inserted first into an empty tree, so it starts as the root; removing it
        // forces RemoveNode's two-children branch (successor swap).
        Assert.True(tree.Remove(50));

        var remaining = values.Where(v => v != 50).ToList();
        AssertSortedConsistency(tree, remaining);
    }

    [Fact]
    public void Remove_LeafValue_MaintainsRemainingTreeIntegrity()
    {
        var values = new[] { 50, 25, 75, 10, 30, 60, 90 };
        var tree = new IndexedRedBlackTree<int>(values);

        Assert.True(tree.Remove(90)); // inserted last, likely a leaf

        var remaining = values.Where(v => v != 90).ToList();
        AssertSortedConsistency(tree, remaining);
    }

    [Fact]
    public void Remove_AllElements_OneAtATimeInRandomOrder_EndsEmpty()
    {
        var values = ShuffledRange(60, seed: 99);
        var tree = new IndexedRedBlackTree<int>(values);
        var removalOrder = ShuffledRange(60, seed: 100);

        var remaining = values.ToList();
        foreach (var value in removalOrder)
        {
            Assert.True(tree.Remove(value));
            remaining.Remove(value);
            Assert.Equal(remaining.Count, tree.Count);
        }

        Assert.True(tree.IsEmpty);
    }

    #endregion

    #region Randomized invariant / stress tests

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(17)]
    [InlineData(256)]
    public void RandomInsertionOrder_IndexAndValueStayAligned(int size)
    {
        // Values are exactly 0..size-1 inserted in shuffled order, so the *sorted* position
        // of a value always equals the value itself - a strong, unambiguous invariant.
        var shuffled = ShuffledRange(size, seed: size * 13 + 1);
        var tree = new IndexedRedBlackTree<int>(shuffled);

        Assert.Equal(size, tree.Count);
        for (int i = 0; i < size; i++)
        {
            Assert.Equal(i, tree[i]);
            Assert.Equal(i, tree.IndexOf(i));
        }
    }

    [Fact]
    public void AscendingInsertionOrder_StaysCorrect()
    {
        // Worst case for an unbalanced BST (a straight chain of right children); a correctly
        // balancing implementation should still produce correct values/order/indices.
        var values = Enumerable.Range(0, 500).ToList();
        var tree = new IndexedRedBlackTree<int>();

        foreach (var v in values)
            tree.Add(v);

        for (int i = 0; i < values.Count; i++)
            Assert.Equal(i, tree[i]);
    }

    [Fact]
    public void DescendingInsertionOrder_StaysCorrect()
    {
        var values = Enumerable.Range(0, 500).Reverse().ToList();
        var tree = new IndexedRedBlackTree<int>();

        foreach (var v in values)
            tree.Add(v);

        for (int i = 0; i < 500; i++)
            Assert.Equal(i, tree[i]);
    }

    [Theory]
    [InlineData(123, 30, 300)]
    [InlineData(456, 80, 500)]
    public void RandomInsertAndRemoveSequence_MaintainsConsistency(int seed, int initialSize, int operationCount)
    {
        var rng = new Random(seed);
        var oracle = RandomValues(initialSize, seed).ToList();
        var tree = new IndexedRedBlackTree<int>(oracle);

        for (int op = 0; op < operationCount; op++)
        {
            bool shouldAdd = oracle.Count == 0 || rng.Next(2) == 0;

            if (shouldAdd)
            {
                int value = rng.Next(1000);
                tree.Add(value);
                oracle.Add(value);
            }
            else
            {
                int removeIndex = rng.Next(oracle.Count);
                var sortedOracle = oracle.OrderBy(x => x).ToList();
                var valueToRemove = sortedOracle[removeIndex];

                Assert.True(tree.Remove(valueToRemove));

                // Remove a single matching instance from the oracle list too (duplicates are
                // interchangeable by value, so it doesn't matter which physical instance the
                // tree happened to remove).
                oracle.Remove(valueToRemove);
            }
        }

        AssertSortedConsistency(tree, oracle);
    }

    [Fact]
    public void RandomRemoveAtSequence_DistinctValues_MatchesOracleList()
    {
        const int size = 200;
        var values = ShuffledRange(size, seed: 2024);
        var tree = new IndexedRedBlackTree<int>(values);
        var oracle = values.OrderBy(x => x).ToList();

        var rng = new Random(2025);
        while (oracle.Count > 0)
        {
            int index = rng.Next(oracle.Count);

            var expected = oracle[index];
            var actual = tree.RemoveAt(index);

            Assert.Equal(expected, actual);
            oracle.RemoveAt(index);

            Assert.Equal(oracle.Count, tree.Count);
        }

        Assert.True(tree.IsEmpty);
    }

    [Fact]
    public void LargeScale_InsertThenRemoveAll_EndsEmptyAndConsistent()
    {
        const int size = 2000;
        var values = ShuffledRange(size, seed: 8675309);
        var tree = new IndexedRedBlackTree<int>();

        foreach (var v in values)
            tree.Add(v);

        AssertSortedConsistency(tree, values);

        var removalOrder = ShuffledRange(size, seed: 909);
        foreach (var v in removalOrder)
            Assert.True(tree.Remove(v));

        Assert.True(tree.IsEmpty);
        Assert.Equal(0, tree.Count);
    }

    #endregion

    #region TryAdd

    [Fact]
    public void TryAdd_EmptyTree_ReturnsTrueAndInsertsElement()
    {
        var tree = new IndexedRedBlackTree<int>();

        bool result = tree.TryAdd(42);

        Assert.True(result);
        Assert.Equal(1, tree.Count);
        Assert.Equal(42, tree[0]);
    }

    [Fact]
    public void TryAdd_UniqueItem_ReturnsTrueAndInsertsElement()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        bool result = tree.TryAdd(99);

        Assert.True(result);
        Assert.Equal(4, tree.Count);
        Assert.True(tree.Contains(99));
    }

    [Fact]
    public void TryAdd_DuplicateItem_ReturnsFalseAndLeavesTreeUnchanged()
    {
        var tree = new IndexedRedBlackTree<int>(new[] { 1, 2, 3 });

        bool result = tree.TryAdd(2);

        Assert.False(result);
        Assert.Equal(3, tree.Count);
        // Sorted order must be unchanged
        Assert.Equal(new[] { 1, 2, 3 }, tree.ToList());
    }

    [Fact]
    public void TryAdd_Null_ThrowsArgumentNullException()
    {
        var tree = new IndexedRedBlackTree<string>();

        Assert.Throws<ArgumentNullException>(() => tree.TryAdd(null!));
    }

    [Fact]
    public void TryAdd_MaintainsSortedOrder()
    {
        var tree = new IndexedRedBlackTree<int>();
        var unique = new[] { 5, 1, 9, 3, 7 };

        foreach (var v in unique)
            tree.TryAdd(v);

        Assert.Equal(unique.OrderBy(x => x), tree.ToList());
    }

    [Fact]
    public void TryAdd_AllDuplicates_NeverInsertsAndCountStaysZero()
    {
        var tree = new IndexedRedBlackTree<int>();
        tree.TryAdd(10);            // first insertion succeeds

        for (int i = 0; i < 50; i++)
        {
            bool result = tree.TryAdd(10);
            Assert.False(result);
        }

        Assert.Equal(1, tree.Count);
    }

    [Fact]
    public void TryAdd_MixOfUniqueAndDuplicates_OnlyInsertsUnique()
    {
        var tree = new IndexedRedBlackTree<int>();
        var inserts = new[] { 5, 3, 5, 1, 3, 7, 7, 9 };

        int inserted = 0;
        foreach (var v in inserts)
            if (tree.TryAdd(v)) inserted++;

        var expectedUnique = inserts.Distinct().OrderBy(x => x).ToList();

        Assert.Equal(expectedUnique.Count, inserted);
        Assert.Equal(expectedUnique.Count, tree.Count);
        Assert.Equal(expectedUnique, tree.ToList());
    }

    [Fact]
    public void TryAdd_IsEquivalentToContainsPlusAdd_ForUniqueItems()
    {
        // Verify TryAdd produces exactly the same tree as the old Contains + Add pattern,
        // using a shuffled sequence with intentional duplicates.
        var values = ShuffledRange(100, seed: 42).Concat(ShuffledRange(50, seed: 43)).ToList();

        var treeOld = new IndexedRedBlackTree<int>();
        foreach (var v in values)
        {
            if (!treeOld.Contains(v))
                treeOld.Add(v);
        }

        var treeNew = new IndexedRedBlackTree<int>();
        foreach (var v in values)
            treeNew.TryAdd(v);

        Assert.Equal(treeOld.ToList(), treeNew.ToList());
        Assert.Equal(treeOld.Count, treeNew.Count);
    }

    [Fact]
    public void TryAdd_DuplicateDoesNotCorruptSizeFields()
    {
        // If TryAdd returned false but accidentally updated Size on some nodes before
        // detecting the duplicate, indexed access would break. This test catches that.
        var tree = new IndexedRedBlackTree<int>(new[] { 10, 20, 30, 40, 50 });

        tree.TryAdd(30); // duplicate — must be a no-op

        // Every indexed position must still resolve correctly
        var expected = new[] { 10, 20, 30, 40, 50 };
        for (int i = 0; i < expected.Length; i++)
            Assert.Equal(expected[i], tree[i]);
    }

    [Fact]
    public void TryAdd_LargeRandomSequenceWithDuplicates_MatchesDistinctOracle()
    {
        var rng = new Random(2025);
        var values = Enumerable.Range(0, 2000).Select(_ => rng.Next(500)).ToList();
        var tree = new IndexedRedBlackTree<int>();

        foreach (var v in values)
            tree.TryAdd(v);

        var expected = values.Distinct().OrderBy(x => x).ToList();

        Assert.Equal(expected.Count, tree.Count);
        Assert.Equal(expected, tree.ToList());
    }

    [Fact]
    public void TryAdd_AscendingInsertionWithDuplicates_StaysCorrect()
    {
        var tree = new IndexedRedBlackTree<int>();

        // Alternating unique and duplicate — worst case for an unbalanced BST
        for (int i = 0; i < 500; i++)
        {
            tree.TryAdd(i);
            tree.TryAdd(i); // immediate duplicate
        }

        Assert.Equal(500, tree.Count);
        for (int i = 0; i < 500; i++)
            Assert.Equal(i, tree[i]);
    }

    #endregion TryAdd


    #region Custom comparable / reference types

    [Fact]
    public void WorksWithStrings()
    {
        var values = new[] { "banana", "apple", "cherry", "date" };
        var tree = new IndexedRedBlackTree<string>(values);

        Assert.Equal(values.OrderBy(x => x), tree.ToList());
    }

    [Fact]
    public void WorksWithCustomComparableReferenceType_AndRetainsDuplicateKeys()
    {
        var tree = new IndexedRedBlackTree<TestItem>();
        var a = new TestItem(5, "a");
        var b = new TestItem(5, "b");
        var c = new TestItem(1, "c");

        tree.AddRange(new[] { a, b, c });

        Assert.Equal(3, tree.Count);
        Assert.Equal(1, tree[0].Key);
        Assert.Equal(5, tree[1].Key);
        Assert.Equal(5, tree[2].Key);
    }

    #endregion
}

internal sealed class TestItem : IComparable<TestItem>
{
    public int Key { get; }
    public string Tag { get; }

    public TestItem(int key, string tag)
    {
        Key = key;
        Tag = tag;
    }

    public int CompareTo(TestItem? other)
    {
        if (other is null) return 1;
        return Key.CompareTo(other.Key);
    }

    public override string ToString() => $"{Key}({Tag})";
}