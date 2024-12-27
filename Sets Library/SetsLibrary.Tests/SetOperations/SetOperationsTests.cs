using Xunit;
using SetsLibrary.SetOperations;
using SetsLibrary;
using System.Collections;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
namespace SetsLibrary.Tests
{
    public class SetsOperationsTests
    {
        // Dummy implementation of IStructuredSet<T> for testing purposes
        public class StructuredSet<T> : BaseSet<T>, IStructuredSet<T> where T : IComparable<T>
        {
            public StructuredSet(SetExtractionConfiguration<T> extractionConfiguration) : base(extractionConfiguration)
            {
            }

            public StructuredSet(IIndexedSetTree<T> indexedSetTree) : base(indexedSetTree)
            {
            }

            public StructuredSet(string expression, SetExtractionConfiguration<T> config) : base(expression, config)
            {
            }

            public override IStructuredSet<T> BuildNewSet(string setString)
            {
                return new StructuredSet<T>(setString, base.ExtractionConfiguration);
            }

            public override IStructuredSet<T> BuildNewSet()
            {
                return new StructuredSet<T>(ExtractionConfiguration);
            }
        }

        // Test helper method to create a set with elements
        private IStructuredSet<int> CreateSet(IEnumerable<int> elements)
        {
            var config = new SetExtractionConfiguration<int>("-", ",");
            var set = new StructuredSet<int>(config);
            foreach (var elem in elements)
            {
                set.AddElement(elem);
            }
            return set;
        }
        private ISetTree<int> CreateTree(IEnumerable<int> elements)
        {
            var config = new SetExtractionConfiguration<int>("-", ",");
            
            var tree = new SetTree<int>(config, elements);
            
            return tree;
        }//CreateTree

        // Test for IntersectWith method
        [Fact]
        public void IntersectWith_ValidSets_ReturnsIntersection()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3, 4 });
            var setB = CreateSet(new[] { 3, 4, 5, 6 });

            // Act
            var result = setA.IntersectWith(setB);

            // Assert
            Assert.Contains(3, result.EnumerateRootElements());
            Assert.Contains(4, result.EnumerateRootElements());
            Assert.DoesNotContain(1, result.EnumerateRootElements());
            Assert.DoesNotContain(2, result.EnumerateRootElements());
        }

        // Test for IntersectWith method when one set is empty
        [Fact]
        public void IntersectWith_EmptySet_ReturnsEmptySet()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new int[] { });

            // Act
            var result = setA.IntersectWith(setB);

            // Assert
            Assert.Empty(result.EnumerateRootElements());
        }

        // Test for UnionWith method
        [Fact]
        public void UnionWith_ValidSets_ReturnsUnion()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 3, 4, 5 });

            // Act
            var result = setA.UnionWith(setB);

            // Assert
            Assert.Contains(1, result.EnumerateRootElements());
            Assert.Contains(2, result.EnumerateRootElements());
            Assert.Contains(3, result.EnumerateRootElements());
            Assert.Contains(4, result.EnumerateRootElements());
            Assert.Contains(5, result.EnumerateRootElements());
        }

        // Test for Complement method (A's complement in Universal set)
        [Fact]
        public void Complement_ValidSets_ReturnsComplement()
        {
            // Arrange
            var universalSet = CreateSet(new[] { 1, 2, 3, 4, 5, 6 });
            var setA = CreateSet(new[] { 1, 2, 3 });

            // Act
            var result = setA.Complement(universalSet);

            // Assert
            Assert.Contains(4, result.EnumerateRootElements());
            Assert.Contains(5, result.EnumerateRootElements());
            Assert.Contains(6, result.EnumerateRootElements());
            Assert.DoesNotContain(1, result.EnumerateRootElements());
            Assert.DoesNotContain(2, result.EnumerateRootElements());
            Assert.DoesNotContain(3, result.EnumerateRootElements());
        }

        // Test for UnionWith method with null set (should throw exception)
        [Fact]
        public void UnionWith_NullSet_ThrowsArgumentNullException()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => setA.UnionWith(null));
        }

        // Test for IntersectWith method with null set (should throw exception)
        [Fact]
        public void IntersectWith_NullSet_ThrowsArgumentNullException()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => setA.IntersectWith(null));
        }

        // Test for Complement method with invalid cardinality (should throw exception)
        [Fact]
        public void Complement_InvalidCardinality_ThrowsSetsOperationException()
        {
            // Arrange
            var universalSet = CreateSet(new[] { 1, 2 });
            var setA = CreateSet(new[] { 1, 2, 3 });

            // Act & Assert
            var exception = Assert.Throws<SetsOperationException>(() => setA.Complement(universalSet));
            Assert.Contains("Cardinality of the set A must be less or equal to the cardinality of the universalSet", exception.Message);
        }

        [Fact]
        public void IntersectWith_NoCommonElements_ReturnsEmptySet()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 4, 5, 6 });

            // Act
            var result = setA.IntersectWith(setB);

            // Assert
            Assert.Empty(result.EnumerateRootElements());
        }

        [Fact]
        public void UnionWith_DuplicateElements_ReturnsUniqueUnion()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 3, 4, 5 });

            // Act
            var result = setA.UnionWith(setB);

            // Assert
            var elements = result.EnumerateRootElements().ToList();
            Assert.Equal(5, elements.Count);
            Assert.Contains(1, elements);
            Assert.Contains(2, elements);
            Assert.Contains(3, elements);
            Assert.Contains(4, elements);
            Assert.Contains(5, elements);
        }
        [Fact]
        public void Complement_LargerUniversalSet_ReturnsComplement()
        {
            // Arrange
            var universalSet = CreateSet([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
            var setA = CreateSet(new[] { 1, 2, 3, 4 });

            // Act
            var result = setA.Complement(universalSet);

            // Assert
            Assert.Contains(5, result.EnumerateRootElements());
            Assert.Contains(6, result.EnumerateRootElements());
            Assert.Contains(7, result.EnumerateRootElements());
            Assert.Contains(8, result.EnumerateRootElements());
            Assert.Contains(9, result.EnumerateRootElements());
            Assert.Contains(10, result.EnumerateRootElements());
            Assert.DoesNotContain(1, result.EnumerateRootElements());
            Assert.DoesNotContain(2, result.EnumerateRootElements());
            Assert.DoesNotContain(3, result.EnumerateRootElements());
            Assert.DoesNotContain(4, result.EnumerateRootElements());
        }

        [Fact]
        public void Complement_SameAsUniversalSet_ReturnsEmptySet()
        {
            // Arrange
            var universalSet = CreateSet(new[] { 1, 2, 3, 4, 5 });
            var setA = CreateSet(new[] { 1, 2, 3, 4, 5 });

            // Act
            var result = setA.Complement(universalSet);

            // Assert
            Assert.Empty(result.EnumerateRootElements());
        }

        [Fact]
        public void UnionWith_Subset_ReturnsUnionWithoutDuplicates()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 2, 3 });

            // Act
            var result = setA.UnionWith(setB);

            // Assert
            var elements = result.EnumerateRootElements().ToList();
            Assert.Equal(3, elements.Count);
            Assert.Contains(1, elements);
            Assert.Contains(2, elements);
            Assert.Contains(3, elements);
        }

        [Fact]
        public void UnionWith_LargeSets_ReturnsUnionCorrectly()
        {
            // Arrange
            var setA = CreateSet(Enumerable.Range(1, 10_000));
            var en = Enumerable.Range(5_000, 5_000);
            var setB = CreateSet(en);

            // Act
            var result = setA.UnionWith(setB);

            // Assert
            Assert.Equal(10_000, result.EnumerateRootElements().Count());
        }

        [Fact]
        public void IntersectWith_DisjointSets_ReturnsEmptySet()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 4, 5, 6 });

            // Act
            var result = setA.IntersectWith(setB);

            // Assert
            Assert.Empty(result.EnumerateRootElements());
        }


        [Fact]
        public void IntersectWith_SameSets_ReturnsSameSet()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });

            // Act
            var result = setA.IntersectWith(setA);

            // Assert
            var elements = result.EnumerateRootElements().ToList();
            Assert.Equal(3, elements.Count);
            Assert.Contains(1, elements);
            Assert.Contains(2, elements);
            Assert.Contains(3, elements);
        }

        [Fact]
        public void Difference_NullSets_ThrowsException()
        {
            // Arrange
            var setA = CreateSet([1, 2, 3, 5, 5, 4, 7, 85, 7, 6, 8, 5]);

            // Act 
            Assert.Throws<ArgumentNullException>(() => setA.Difference(default(IStructuredSet<int>)));
        }//Difference_NullSets_ThrowsException

        [Fact]
        public void Difference_SameSets_ReturnsEmptySet()
        {
            // Arrange
            var setA = CreateSet([1, 2, 3, 4, 5, 6, 7, 8, 9]);

            // Act
            var result = setA.Difference(setA);

            // Assert
            Assert.True(result.Cardinality == 0);
            Assert.Empty(result.EnumerateRootElements());
            Assert.Empty(result.EnumerateSubsets());
        }//Difference_SameSets_ReturnsEmptySet

        [Fact]
        public void Difference_DifferentSets_ReturnsDifference()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 1 });

            //Act
            var A_B = setA.Difference(setB);
            var B_A = setB.Difference(setA);

            // Assert
            //For A -B 
            Assert.Contains(2, A_B.EnumerateRootElements());
            Assert.Contains(3, A_B.EnumerateRootElements());
            Assert.DoesNotContain(1, A_B.EnumerateRootElements());
            Assert.Equal(2, A_B.Cardinality);

            //For B - A
            Assert.DoesNotContain(1, B_A.EnumerateRootElements());
            Assert.DoesNotContain(2, B_A.EnumerateRootElements());
            Assert.DoesNotContain(3, B_A.EnumerateRootElements());
            Assert.Equal(0, B_A.Cardinality);

        }//Difference_DifferentSets_ReturnsDifference

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7 })]
        [InlineData(new int[] { 5, 6, 7 }, new int[] { 1, 2, 3, 4 })]
        [InlineData(new int[] { 7, 6, 1, 4}, new int[] { 3, 2, 8, 5, 9 })]
        [InlineData(new int[] { 3, 2, 8, 5, 9 },new int[] { 7, 6, 1, 4})]
        [InlineData(new int[] { 9228, 4620, 192, 466, 7187 }, new int[] { 4121, 6973, 4341, 617, 1088, 6887, 1721, 9650 })]
        [InlineData(new int[] { 2138, 5591, 5825, 5677, 2870, 7458, 3740, 4980, 3757 }, new int[] { 8936, 4849, 7490, 7455, 97, 6569 })]
        [InlineData(new int[] { 2943, 3762, 8083, 7121, 5908, 7441, 4168, 5502, 8477, 9184, 6948, 1444 }, new int[] { 8957, 8119, 9072, 1515 })]
        [InlineData(new int[] { 1005, 7273 }, new int[] { 1810, 8330 })]
        [InlineData(new int[] { 7280, 3622, 2744, 3389, 8219 }, new int[] { 3510 })]
        [InlineData(new int[] { 601, 7547, 6124 }, new int[] { 3455, 4336, 5824, 9067 })]
        [InlineData(new int[] { 7595, 2093, 3255, 501, 7318, 5314, 9109, 1674 }, new int[] { 5304, 9156, 1061, 5487, 3219, 9652, 1819, 260 })]
        [InlineData(new int[] { 11, 8, 14, 5, 20 }, new int[] { 3, 27, 19, 1, 26, 28, 21, 18 })]
        [InlineData(new int[] { 23, 27, 28, 24, 14, 20, 26, 11, 4 }, new int[] { 12, 7, 5, 3, 10, 19 })]
        [InlineData(new int[] { 14, 10, 25, 19, 18, 27, 11, 24, 21, 12, 23, 9 }, new int[] { 22, 26, 5, 7 })]
        [InlineData(new int[] { 2, 3 }, new int[] { 6, 20 })]
        [InlineData(new int[] { 19, 23, 1, 9, 16 }, new int[] { 11 })]
        [InlineData(new int[] { 8, 25, 6 }, new int[] { 12, 19, 15, 24 })]
        [InlineData(new int[] { 17, 27, 18, 23, 9, 13, 29, 16 }, new int[] { 1, 15, 26, 21, 2, 8, 12, 5 })]
        public void Difference_DisjointSets_ReturnsSameSets(int[] enumA, int[] enumB)
        {
            // Arrange
            var setA = CreateSet(enumA);
            var setB = CreateSet(enumB);

            // Act
            var result = setA.Difference(setB);

            // Assert
            Assert.Equal(result.EnumerateRootElements(), setA.EnumerateRootElements());
        }//Difference_DisjointSets_ReturnsSameSets
        
        [Fact]
        public void UnionWith_OverlappingSets_ReturnsUniqueUnion()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3, 4 });
            var setB = CreateSet(new[] { 3, 4, 5, 6 });

            // Act
            var result = setA.UnionWith(setB);

            // Assert
            Assert.Contains(1, result.EnumerateRootElements());
            Assert.Contains(2, result.EnumerateRootElements());
            Assert.Contains(3, result.EnumerateRootElements());
            Assert.Contains(4, result.EnumerateRootElements());
            Assert.Contains(5, result.EnumerateRootElements());
            Assert.Contains(6, result.EnumerateRootElements());
        }
        [Fact]
        public void Complement_UniversalSet_ReturnsEmptySet()
        {
            // Arrange
            var universalSet = CreateSet(new[] { 1, 2, 3, 4, 5 });
            var setA = CreateSet(new[] { 1, 2, 3, 4, 5 });

            // Act
            var result = setA.Complement(universalSet);

            // Assert
            Assert.Empty(result.EnumerateRootElements());
        }

        [Fact]
        public void SymmetricDifference_ValidSubSets_ReturnsSymmetricDifference()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3, 4 });
            var setB = CreateSet(new[] { 3, 4, 5, 6 });

            //1 2 3 4 - A
            //3 4 5 6 - B

            //Symmetric difference
            // B - A = 5 6 
            // A - B = 1 2
            // Diff = 1 2 5 6

            // Act
            var result = setA.SymmetricDifference(setB);

            // Assert
            Assert.Contains(1, result.EnumerateRootElements());
            Assert.Contains(2, result.EnumerateRootElements());
            Assert.Contains(5, result.EnumerateRootElements());
            Assert.Contains(6, result.EnumerateRootElements());
            Assert.DoesNotContain(3, result.EnumerateRootElements());
            Assert.DoesNotContain(4, result.EnumerateRootElements());
        }

        // Test for StructuralEqual method when sets are structurally equal
        [Fact]
        public void StructuralEqual_StructurallyEqualSets_ReturnsTrue()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 1, 2, 3 });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.True(result);
        }

        // Test for StructuralEqual method when sets are not structurally equal
        [Fact]
        public void StructuralEqual_SetsWithDifferentElements_ReturnsFalse()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 4, 5, 6 });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.False(result);
        }

        // Test for StructuralEqual method when one set is empty
        [Fact]
        public void StructuralEqual_EmptySet_ReturnsFalse()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new int[] { });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.False(result);
        }

        // Test for StructuralEqual method when sets are structurally equal (different order)
        [Fact]
        public void StructuralEqual_SetsWithSameElementsDifferentOrder_ReturnsTrue()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 3, 2, 1 });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.True(result);
        }//StructuralEqual_SetsWithSameElementsDifferentOrder_ReturnsTrue
        // Test for SymmetricDifference method
        [Fact]
        public void SymmetricDifference_ValidSets_ReturnsSymmetricDifference()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3, 4 });
            var setB = CreateSet(new[] { 3, 4, 5, 6 });
            var t1 = CreateTree(new[] { 1 });
            var t2 = CreateTree(new[] { 1, 3, 2, 4, 5, 8, 4, 6 });
            var t3 = CreateTree(new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1 });
            setA.AddElement(t1);
            setA.AddElement(t2);
            setA.AddElement(t3);
            setB.AddElement(t1);
            var t4 = CreateTree(new[] { 1 });
            t4.AddSubSetTree(t2);
            setB.AddElement(t4);

            // Act
            var result = setA.SymmetricDifference(setB);


            // Assert
            Assert.Contains(1, result.EnumerateRootElements());
            Assert.Contains(2, result.EnumerateRootElements());
            Assert.Contains(5, result.EnumerateRootElements());
            Assert.Contains(6, result.EnumerateRootElements());
            Assert.DoesNotContain(3, result.EnumerateRootElements());
            Assert.DoesNotContain(4, result.EnumerateRootElements());
            Assert.Contains(t2, result.EnumerateSubsets());

#warning This need to be fixed
            Assert.Contains(t4, result.EnumerateSubsets());
        }

        // Test for CartesianProduct method (NotImplementedException expected)
        [Fact]
        public void CartesianProduct_ThrowsNotImplementedException()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2 });
            var setB = CreateSet(new[] { 3, 4 });

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() => setA.CartesianProduct(setB));
            Assert.Equal("A very complex procedure", exception.Message);
        }

        // Test for IsDisjoint method when sets are disjoint
        [Fact]
        public void IsDisjoint_DisjointSets_ReturnsTrue()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2 });
            var setB = CreateSet(new[] { 3, 4 });

            // Act
            var result = setA.IsDisjoint(setB);

            // Assert
            Assert.True(result);
        }

        // Test for IsDisjoint method when sets are not disjoint
        [Fact]
        public void IsDisjoint_SetsWithCommonElements_ReturnsFalse()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 3, 4, 5 });

            // Act
            var result = setA.IsDisjoint(setB);

            // Assert
            Assert.False(result);
        }

        // Test for SetStructuresEqual method when sets are equal
        [Fact]
        public void SetStructuresEqual_EqualSets_ReturnsTrue()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 1, 2, 3 });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.True(result);
        }

        // Test for SetStructuresEqual method when sets are not equal
        [Fact]
        public void SetStructuresEqual_SetsWithDifferentElements_ReturnsFalse()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 4, 5, 6 });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.False(result);
        }

        // Test for SetStructuresEqual method when one set is empty
        [Fact]
        public void SetStructuresEqual_EmptySet_ReturnsFalse()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new int[] { });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.False(result);
        }

        // Test for SetStructuresEqual method when sets are structurally equal (different order)
        [Fact]
        public void SetStructuresEqual_SetsWithSameElementsDifferentOrder_ReturnsTrue()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 3, 2, 1 });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.True(result);
        }
        // Test for SymmetricDifference with duplicate elements
        [Fact]
        public void SymmetricDifference_WithDuplicates_ReturnsCorrectSymmetricDifference()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 1, 2, 2, 3 });
            var setB = CreateSet(new[] { 2, 2, 4 });

            // Act
            var result = setA.SymmetricDifference(setB);

            // Assert
            Assert.Contains(1, result.EnumerateRootElements());
            Assert.Contains(3, result.EnumerateRootElements());
            Assert.Contains(4, result.EnumerateRootElements());
            Assert.DoesNotContain(2, result.EnumerateRootElements());
        }

        // Test for SymmetricDifference with empty set
        [Fact]
        public void SymmetricDifference_EmptySet_ReturnsOtherSet()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new int[] { });

            // Act
            var result = setA.SymmetricDifference(setB);

            // Assert
            Assert.True(setA.SetStructuresEqual(result)); // The result should be the same as setA
        }

        // Test for SymmetricDifference with large sets
        [Fact]
        public void SymmetricDifference_LargeSets_ReturnsCorrectSymmetricDifference()
        {
            // Arrange
            var setA = CreateSet(new int[1000]);
            var setB = CreateSet(new int[1000]);
            for (int i = 0; i < 1000; i++)
            {
                setA.AddElement(CreateTree(new[] { i }));
                setB.AddElement(CreateTree(new[] { i + 500 }));
            }

            // Act
            var result = setA.SymmetricDifference(setB);

            // Assert
            Assert.Equal(1000, result.Cardinality); // Symmetric difference should have 1000 elements
        }

        // Test for IsDisjoint when sets have identical elements
        [Fact]
        public void IsDisjoint_IdenticalSets_ReturnsFalse()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 1, 2, 3 });

            // Act
            var result = setA.IsDisjoint(setB);

            // Assert
            Assert.False(result); // The sets are not disjoint
        }

        // Test for IsDisjoint when one set is a subset of another
        [Fact]
        public void IsDisjoint_SubsetSet_ReturnsFalse()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2, 3 });
            var setB = CreateSet(new[] { 1, 2 });

            // Act
            var result = setA.IsDisjoint(setB);

            // Assert
            Assert.False(result); // The sets have common elements
        }

        // Test for SetStructuresEqual with sets having different cardinalities
        [Fact]
        public void SetStructuresEqual_DifferentCardinalities_ReturnsFalse()
        {
            // Arrange
            var setA = CreateSet(new[] { 1, 2 });
            var setB = CreateSet(new[] { 1, 2, 3 });

            // Act
            var result = setA.SetStructuresEqual(setB);

            // Assert
            Assert.False(result); // Different cardinalities means the sets are not equal
        }

        // Test for IsDisjoint with large sets
        [Fact]
        public void IsDisjoint_LargeDisjointSets_ReturnsTrue()
        {
            // Arrange
            var setA = CreateSet([10010]);
            var setB = CreateSet([1000]);
            for (int i = 0; i < 8; i++)
            {
                setA.AddElement(CreateTree(new[] { i + 1}));
                setB.AddElement(CreateTree(new[] { i + 1000 }));
            }

            // Act
            var result = setA.IsDisjoint(setB);

            // Assert
            Assert.True(result); // The sets are disjoint
        }
        // Advanced Theory for SymmetricDifference using multiple test cases
        [Theory]
        [InlineData(new[] { 1, 2, 3 }, new[] { 2, 3, 4 }, new[] { 1, 4 })]
        [InlineData(new[] { 1, 2 }, new[] { 3, 4 }, new[] { 1, 2, 3, 4 })]
        [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, new int[] { })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 3, 4, 5, 6 }, new[] { 1, 2, 5, 6 })]
        public void SymmetricDifference_TheoryTest(int[] setAElements, int[] setBElements, int[] expectedSymmetricDifference)
        {
            // Arrange
            var setA = CreateSet(setAElements);
            var setB = CreateSet(setBElements);

            // Act
            var result = setA.SymmetricDifference(setB);

            // Assert
            Assert.Equal(expectedSymmetricDifference.Length, result.Cardinality);
            foreach (var elem in expectedSymmetricDifference)
            {
                Assert.Contains(elem, result.EnumerateRootElements());
            }
        }
    }//class
}//namespace