using SetsLibrary.Collections;
using Xunit;

namespace SetsLibrary.Tests.Collections.Element_Collections
{
    public class SortedElementsTests
    {
        [Fact]
        public void Add_OneElement_ElementIsAdded()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(5);

            Assert.Equal(1, sortedElements.Count);
            Assert.Equal(5, sortedElements[0]);
        }//Add_OneElement_ElementIsAdded

        [Fact]
        public void Add_MultipleElements_ElementsAreSorted()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(3);
            sortedElements.AddIfDuplicate(1);
            sortedElements.AddIfDuplicate(2);

            Assert.Equal(3, sortedElements.Count);
            Assert.Equal(1, sortedElements[0]);
            Assert.Equal(2, sortedElements[1]);
            Assert.Equal(3, sortedElements[2]);
        }//Add_MultipleElements_ElementsAreSorted

        [Fact]
        public void Add_NullElement_ThrowsArgumentNullException()
        {
            var sortedElements = new SortedElements<string>();
            Assert.Throws<ArgumentNullException>(() => sortedElements.AddIfDuplicate(null));
        }//Add_NullElement_ThrowsArgumentNullException

        [Fact]
        public void Remove_ExistingElement_ElementIsRemoved()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(1);
            sortedElements.AddIfDuplicate(2);
            sortedElements.AddIfDuplicate(3);

            sortedElements.Remove(2);
            Assert.Equal(2, sortedElements.Count);
            Assert.Equal(1, sortedElements[0]);
            Assert.Equal(3, sortedElements[1]);
        }//Remove_ExistingElement_ElementIsRemoved

        [Fact]
        public void Remove_NonExistingElement_ReturnsFalse()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(1);
            sortedElements.AddIfDuplicate(2);

            bool result = sortedElements.Remove(3);
            Assert.False(result);
            Assert.Equal(2, sortedElements.Count);
        }//Remove_NonExistingElement_ReturnsFalse

        [Fact]
        public void Contains_ExistingElement_ReturnsTrue()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(1);
            sortedElements.AddIfDuplicate(2);

            Assert.True(sortedElements.Contains(1));
        }//Contains_ExistingElement_ReturnsTrue

        [Fact]
        public void Contains_NonExistingElement_ReturnsFalse()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(1);
            sortedElements.AddIfDuplicate(2);

            Assert.False(sortedElements.Contains(3));
        }//Contains_NonExistingElement_ReturnsFalse

        [Fact]
        public void IndexOf_ExistingElement_ReturnsCorrectIndex()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(1);
            sortedElements.AddIfDuplicate(2);
            sortedElements.AddIfDuplicate(3);

            int index = sortedElements.IndexOf(2);
            Assert.Equal(1, index);
        }//IndexOf_ExistingElement_ReturnsCorrectIndex

        [Fact]
        public void IndexOf_NonExistingElement_ReturnsMinusOne()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(1);
            sortedElements.AddIfDuplicate(2);

            int index = sortedElements.IndexOf(3);
            Assert.Equal(-1, index);
        }//IndexOf_NonExistingElement_ReturnsMinusOne

        [Fact]
        public void Clear_ElementsAreRemoved()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddIfDuplicate(1);
            sortedElements.AddIfDuplicate(2);
            sortedElements.Clear();

            Assert.Equal(0, sortedElements.Count);
        }//Clear_ElementsAreRemoved

        [Fact]
        public void AddRange_AddsMultipleElements_SortedOrderMaintained()
        {
            var sortedElements = new SortedElements<int>();
            sortedElements.AddRange(new List<int> { 5, 3, 8 });

            Assert.Equal(3, sortedElements.Count);
            Assert.Equal(3, sortedElements[0]);
            Assert.Equal(5, sortedElements[1]);
            Assert.Equal(8, sortedElements[2]);
        }//AddRange_AddsMultipleElements_SortedOrderMaintained

        [Theory]
        [InlineData(new int[] { 5, 1, 3 }, new int[] { 1, 3, 5 })]
        [InlineData(new int[] { 10, 20, 15 }, new int[] { 10, 15, 20 })]
        [InlineData(new int[] { 2, 4, 3, 1 }, new int[] { 1, 2, 3, 4 })]
        [InlineData(new int[] { -1, -3, 0, 2 }, new int[] { -3, -1, 0, 2 })]
        public void AddRange_MultipleInputs_SortedOrderMaintained(int[] inputs, int[] expected)
        {
            // Arrange
            var sortedElements = new SortedElements<int>();

            // Act
            sortedElements.AddRange(inputs);

            // Assert
            Assert.Equal(expected.Length, sortedElements.Count);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], sortedElements[i]);
            }
        }//AddRange_MultipleInputs_SortedOrderMaintained
    }//class
}//namespace
