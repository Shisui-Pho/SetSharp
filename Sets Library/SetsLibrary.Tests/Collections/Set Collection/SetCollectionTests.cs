using SetLibrary.Collections;
using SetsLibrary.Models;
using SetsLibrary.Models.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SetsLibrary.Tests.Collections.Set_Collection
{
    public class SetCollectionTests
    {
        private static readonly SetExtractionConfiguration<int> settings = new SetExtractionConfiguration<int>(";",",");

        [Fact]
        public void Count_ReturnsZero_WhenCollectionIsEmpty()
        {
            // Arrange
            var setCollection = new SetCollection<int>();

            // Act
            int count = setCollection.Count;

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void Add_IncrementsCount_WhenAddingNewSet()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();

            // Act
            setCollection.Add(set);

            // Assert
            Assert.Equal(1, setCollection.Count);
        }

        [Fact]
        public void Remove_DecrementsCount_WhenRemovingSet()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            setCollection.Remove("A");

            // Assert
            Assert.Equal(0, setCollection.Count);
        }
        [Fact]
        public void Count_ReturnsZero_WhenCollectionIsEmpty2()
        {
            // Arrange
            var setCollection = new SetCollection<int>();

            // Act
            int count = setCollection.Count;

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void Add_IncrementsCount_WhenAddingNewSet2()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();

            // Act
            setCollection.Add(set);

            // Assert
            Assert.Equal(1, setCollection.Count);
        }

        [Fact]
        public void Add_SetsUniqueNames_WhenAddingSets()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set1 = new MockSetStructure();
            var set2 = new MockSetStructure();

            // Act
            setCollection.Add(set1);
            setCollection.Add(set2);
            var lst = setCollection.ToList();
            // Assert
            Assert.NotEqual(lst[0].Key, lst[1].Key);
        }

        [Fact]
        public void Add_IncrementsNameCorrectly_WhenAddingMultipleSets()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set1 = new MockSetStructure();
            var set2 = new MockSetStructure();

            // Act
            setCollection.Add(set1);
            setCollection.Add(set2);

            var lst = setCollection.ToList();

            // Assert
            Assert.Equal("A", lst[0].Key);
            Assert.Equal("B", lst[1].Key);
        }

        [Fact]
        public void Remove_UpdatesNames_WhenRemovingSet()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set1 = new MockSetStructure();
            var set2 = new MockSetStructure();
            setCollection.Add(set1);
            setCollection.Add(set2);

            // Act
            setCollection.Remove("A");
            var lst = setCollection.ToList();

            // Assert
            Assert.Equal("B", lst[0].Key);
        }
        [Fact]
        public void Count_ReturnsZero_WhenCollectionIsEmpty3()
        {
            // Arrange
            var setCollection = new SetCollection<int>();

            // Act
            int count = setCollection.Count;

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void Add_IncrementsCount_WhenAddingNewSet3()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();

            // Act
            setCollection.Add(set);

            // Assert
            Assert.Equal(1, setCollection.Count);
        }

        [Fact]
        public void Remove_DecrementsCount_WhenRemovingSet3()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            setCollection.Remove("A");

            // Assert
            Assert.Equal(0, setCollection.Count);
        }

        [Fact]
        public void FindSetByName_ReturnsSet_WhenNameExists()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            var foundSet = setCollection.FindSetByName("A");

            // Assert
            Assert.NotNull(foundSet);
            Assert.Equal(set.ToString(), foundSet.ToString());
            Assert.Equal(set.Cardinality, foundSet.Cardinality);
        }

        [Fact]
        public void FindSetByName_ReturnsNull_WhenNameDoesNotExist()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            var foundSet = setCollection.FindSetByName("B");

            // Assert
            Assert.Null(foundSet);
        }

        [Fact]
        public void GetSetByName_ReturnsSet_WhenNameExists()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            var foundSet = setCollection.FindSetByName("A");

            // Assert
            Assert.NotNull(foundSet);
            Assert.Equal(set.ToString(), foundSet.ToString());
            Assert.Equal(set.Cardinality, foundSet.Cardinality);
        }

        [Fact]
        public void Count_ReturnsZero_WhenCollectionIsEmpty_New()
        {
            // Arrange
            var setCollection = new SetCollection<int>();

            // Act
            int count = setCollection.Count;

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void Add_IncrementsCount_WhenAddingNewSet_New()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();

            // Act
            setCollection.Add(set);

            // Assert
            Assert.Equal(1, setCollection.Count);
        }

        [Fact]
        public void Remove_DecrementsCount_WhenRemovingSet_New()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            setCollection.Remove("A");

            // Assert
            Assert.Equal(0, setCollection.Count);
        }

        [Fact]
        public void FindSetByName_ReturnsSet_WhenNameExists_New()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            var foundSet = setCollection.FindSetByName("A");

            // Assert
            Assert.NotNull(foundSet);
            Assert.Equal(set.ToString(), foundSet.ToString());
            Assert.Equal(set.Cardinality, foundSet.Cardinality);
        }

        [Fact]
        public void FindSetByName_ReturnsNull_WhenNameDoesNotExist_New()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            var foundSet = setCollection.FindSetByName("B");

            // Assert
            Assert.Null(foundSet);
        }

        [Fact]
        public void GetSetByName_ReturnsSet_WhenNameExists_New()
        {
            // Arrange
            var setCollection = new SetCollection<int>();
            var set = new MockSetStructure();
            setCollection.Add(set);

            // Act
            var foundSet = setCollection.FindSetByName("A");

            // Assert
            Assert.NotNull(foundSet);
            Assert.Equal(set.ToString(), foundSet.ToString());
            Assert.Equal(set.Cardinality, foundSet.Cardinality);
        }



    }//CLASS
}//namespace
