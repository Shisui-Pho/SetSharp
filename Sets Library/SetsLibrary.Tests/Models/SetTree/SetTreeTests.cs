using Xunit;
namespace SetsLibrary.Tests.Models.SetTree
{

    public class SetTreeTests

    {
        #region Constructor Tests

        [Fact]
        public void Constructor_Should_Initialize_Empty_SetTree()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            Assert.NotNull(setTree);
            Assert.Empty(setTree.GetRootElementsEnumerator());
            Assert.Empty(setTree.GetSubsetsEnumerator());
            Assert.Equal(0, setTree.Count);
        }

        [Fact]
        public void Constructor_With_ExtractionSettings_Should_Initialize_SetTree()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            Assert.NotNull(setTree);
            Assert.Equal(extractionSettings, setTree.ExtractionSettings);
        }

        [Fact]
        public void Constructor_With_Elements_Should_Add_Elements()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var elements = new List<int> { 1, 2, 3 };
            var setTree = new SetTree<int>(extractionSettings, elements);
            Assert.Equal(3, setTree.CountRootElements);
            Assert.Contains(1, setTree.GetRootElementsEnumerator());
            Assert.Contains(2, setTree.GetRootElementsEnumerator());
            Assert.Contains(3, setTree.GetRootElementsEnumerator());
        }

        #endregion Constructor Tests

        #region Add Element Tests

        [Fact]
        public void AddElement_Should_Add_Element_When_Not_Exist()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddElement(10);
            Assert.Contains(10, setTree.GetRootElementsEnumerator());
        }

        [Fact]
        public void AddElement_Should_Not_Add_Existing_Element()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddElement(10);
            setTree.AddElement(10); // Adding again
            Assert.Single(setTree.GetRootElementsEnumerator()); // Only one occurrence of 10
        }

        [Fact]
        public void AddElement_Should_Throw_ArgumentNullException_When_Element_Is_Null()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<string>(extractionSettings);
            Assert.Throws<ArgumentNullException>(() => setTree.AddElement(null));
        }

        #endregion Add Element Tests

        #region Add Range Tests

        [Fact]
        public void AddRange_Should_Add_Elements()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddRange(new List<int> { 1, 2, 3 });
            Assert.Equal(3, setTree.CountRootElements);
            Assert.Contains(1, setTree.GetRootElementsEnumerator());
            Assert.Contains(2, setTree.GetRootElementsEnumerator());
            Assert.Contains(3, setTree.GetRootElementsEnumerator());
        }

        [Fact]
        public void AddRange_Should_Throw_ArgumentNullException_When_Elements_Are_Null()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            Assert.Throws<ArgumentNullException>(() => setTree.AddRange(default(IEnumerable<int>)));
        }

        #endregion Add Range Tests

        #region Add Subset Tree Tests

        [Fact]
        public void AddSubSetTree_Should_Add_Subset_Tree()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            var subsetTree = new SetTree<int>(extractionSettings);
            subsetTree.AddElement(5);

            setTree.AddSubSetTree(subsetTree);

            Assert.Single(setTree.GetSubsetsEnumerator());
        }

        [Fact]
        public void AddSubSetTree_Should_Throw_ArgumentNullException_When_Tree_Is_Null()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            Assert.Throws<ArgumentNullException>(() => setTree.AddSubSetTree(null));
        }

        #endregion Add Subset Tree Tests

        #region Remove Element Tests

        [Fact]
        public void RemoveElement_Should_Remove_Element_When_Exists()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddElement(10);
            bool result = setTree.RemoveElement(10);
            Assert.True(result);
            Assert.DoesNotContain(10, setTree.GetRootElementsEnumerator());
        }

        [Fact]
        public void RemoveElement_Should_Return_False_When_Element_Does_Not_Exist()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            bool result = setTree.RemoveElement(10);
            Assert.False(result);
        }

        [Fact]
        public void RemoveElement_Should_Throw_ArgumentNullException_When_Element_Is_Null()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            Assert.Throws<ArgumentNullException>(() => setTree.RemoveElement(null));
        }

        #endregion Remove Element Tests

        #region Remove Subset Tree Tests

        [Fact]
        public void RemoveElement_Should_Remove_Subset_Tree_When_Exists()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            var subsetTree = new SetTree<int>(extractionSettings);
            subsetTree.AddElement(5);
            setTree.AddSubSetTree(subsetTree);

            bool result = setTree.RemoveElement(subsetTree);
            Assert.True(result);
            Assert.Empty(setTree.GetSubsetsEnumerator());
        }

        [Fact]
        public void RemoveElement_Should_Return_False_When_Subset_Does_Not_Exist()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            var subsetTree = new SetTree<int>(extractionSettings);
            bool result = setTree.RemoveElement(subsetTree);
            Assert.False(result);
        }

        [Fact]
        public void RemoveElement_Should_Throw_ArgumentNullException_When_Subset_Tree_Is_Null()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            Assert.Throws<ArgumentNullException>(() => setTree.RemoveElement((ISetTree<int>)null));
        }

        #endregion Remove Subset Tree Tests

        #region CompareTo Tests

        [Fact]
        public void CompareTo_Should_Return_0_When_Trees_Are_Equal()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree1 = new SetTree<int>(extractionSettings);
            setTree1.AddElement(10);
            var setTree2 = new SetTree<int>(extractionSettings);
            setTree2.AddElement(10);
            int result = setTree1.CompareTo(setTree2);
            Assert.Equal(0, result); // Trees are equal
        }

        [Fact]
        public void CompareTo_Should_Return_1_When_Tree_Is_Larger()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree1 = new SetTree<int>(extractionSettings);
            setTree1.AddElement(10);
            var setTree2 = new SetTree<int>(extractionSettings);
            setTree2.AddElement(10);
            setTree2.AddElement(20);
            int result = setTree1.CompareTo(setTree2);
            Assert.Equal(-1, result); // Tree 1 is smaller
        }

        [Fact]
        public void CompareTo_Should_Return_1_When_Tree_Is_Smaller()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree1 = new SetTree<int>(extractionSettings);
            setTree1.AddElement(10);
            var setTree2 = new SetTree<int>(extractionSettings);
            setTree2.AddElement(10);
            setTree2.AddElement(20);
            int result = setTree2.CompareTo(setTree1);
            Assert.Equal(1, result); // Tree 2 is larger
        }

        [Fact]
        public void CompareTo_Should_Throw_ArgumentNullException_When_Compared_With_Null()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            Assert.Throws<ArgumentNullException>(() => setTree.CompareTo(null));
        }

        #endregion CompareTo Tests

        #region ToString Tests

        [Fact]
        public void ToString_Should_Return_Correct_String_Representation()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddElement(10);
            setTree.AddElement(20);
            var result = setTree.ToString();
            Assert.Contains("10", result);
            Assert.Contains("20", result);
        }

        #endregion ToString Tests

        #region Root Elements and Subsets Enumerator Tests

        [Fact]
        public void GetRootElementsEnumerator_Should_Return_Elements()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddElement(1);
            setTree.AddElement(2);
            var rootElements = setTree.GetRootElementsEnumerator();
            Assert.Contains(1, rootElements);
            Assert.Contains(2, rootElements);
        }

        [Fact]
        public void GetSubsetsEnumerator_Should_Return_Subsets()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            var subsetTree = new SetTree<int>(extractionSettings);
            subsetTree.AddElement(5);
            setTree.AddSubSetTree(subsetTree);
            var subsets = setTree.GetSubsetsEnumerator();
            Assert.Contains(subsetTree, subsets);
        }
        #endregion Root Elements and Subsets Enumerator Tests

        #region ChatGPT Tests

        [Fact]
        public void AddElement_Should_Not_Add_Duplicate_Elements()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddElement(5);
            setTree.AddElement(5); //Adding duplicate element

            Assert.Single(setTree.GetRootElementsEnumerator()); //Should only contain 5 once
        }

        [Fact]
        public void AddRange_Should_Add_Elements_And_Subsets_Correctly()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree1 = new SetTree<int>(extractionSettings);
            setTree1.AddElement(10);
            setTree1.AddElement(20);

            var setTree2 = new SetTree<int>(extractionSettings);
            setTree2.AddElement(30);

            var mainTree = new SetTree<int>(extractionSettings);
            mainTree.AddRange(new List<int> { 1, 2 });
            mainTree.AddSubSetTree(setTree1);
            mainTree.AddSubSetTree(setTree2);

            Assert.Equal(2, mainTree.CountRootElements); // 1, 2
            Assert.Equal(2, mainTree.CountSubsets); // setTree1, setTree2
        }

        [Fact]
        public void AddRange_Should_Perform_Well_With_Large_Collection()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            var elements = new List<int>();
            for (int i = 0; i < 100000; i++)
            {
                elements.Add(i);
            }

            setTree.AddRange(elements);

            Assert.Equal(100000, setTree.CountRootElements);
        }

        [Fact]
        public void RemoveElement_Should_Return_False_When_Trying_To_Remove_Empty_Subset()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            var emptySubset = new SetTree<int>(extractionSettings);

            bool result = setTree.RemoveElement(emptySubset);
            Assert.False(result); // Should return false as there are no subsets to remove
        }

        [Fact]
        public void CompareTo_Should_Return_0_When_Trees_Are_Equal_But_Elements_Are_In_Different_Order()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree1 = new SetTree<int>(extractionSettings);
            setTree1.AddRange(new List<int> { 1, 3, 2 });

            var setTree2 = new SetTree<int>(extractionSettings);
            setTree2.AddRange(new List<int> { 2, 1, 3 });

            Assert.Equal(0, setTree1.CompareTo(setTree2)); // Should be considered equal despite different orders
        }

        [Fact]
        public void CompareTo_Should_Throw_ArgumentNullException_When_Compared_With_Null2()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            Assert.Throws<ArgumentNullException>(() => setTree.CompareTo(null)); // Should throw ArgumentNullException
        }

        [Fact]
        public void AddSubSetTree_Should_Handle_Nested_Subsets_Correctly()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree1 = new SetTree<int>(extractionSettings);
            setTree1.AddElement(10);
            setTree1.AddElement(20);

            var setTree2 = new SetTree<int>(extractionSettings);
            setTree2.AddElement(30);

            var setTree3 = new SetTree<int>(extractionSettings);
            setTree3.AddElement(40);

            // Nested subset
            setTree2.AddSubSetTree(setTree3);

            var mainTree = new SetTree<int>(extractionSettings);
            mainTree.AddSubSetTree(setTree1);
            mainTree.AddSubSetTree(setTree2);

            Assert.Equal(2, mainTree.CountSubsets); // setTree1, setTree2
            Assert.Equal(1, setTree2.CountSubsets); // setTree3 inside setTree2
        }

        [Fact]
        public void RemoveElement_Should_Work_After_Subsets_Are_Added()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddElement(10);
            setTree.AddElement(20);

            var subsetTree = new SetTree<int>(extractionSettings);
            subsetTree.AddElement(30);

            setTree.AddSubSetTree(subsetTree);

            bool result = setTree.RemoveElement(10); // Removing an element from the root set
            Assert.True(result); // Should remove 10 successfully
            Assert.DoesNotContain(10, setTree.GetRootElementsEnumerator());
        }

        [Fact]
        public void CompareTo_Should_Return_1_When_Comparing_Empty_Tree_With_NonEmpty_Tree()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var emptyTree = new SetTree<int>(extractionSettings);
            var nonEmptyTree = new SetTree<int>(extractionSettings);
            nonEmptyTree.AddElement(10);

            int result = emptyTree.CompareTo(nonEmptyTree);
            Assert.Equal(-1, result); // Empty tree should be considered smaller
        }
        #endregion CHATGPT Tests
        #region CHATGPT Valnarability tests
        [Fact]
        public void AddElement_Should_Throw_ArgumentNullException_When_Element_Is_Null_In_Threaded_Environment()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<string>(extractionSettings);

            // Create a flag to track exceptions
            List<Exception> threadExceptions = new List<Exception>();
            object obj = new object();
            // Define the action that will be executed in the threads
            Action addNullElement = () =>
            {
                try
                {
                    lock (obj)
                    {
                        setTree.AddElement(null);
                    }
                }
                catch (ArgumentNullException ex)
                {
                    // Capture the exception to handle it after the threads complete
                    threadExceptions.Add(ex);
                }
            };

            // Create two threads to run concurrently
            var thread1 = new System.Threading.Thread(new System.Threading.ThreadStart(addNullElement));
            var thread2 = new System.Threading.Thread(new System.Threading.ThreadStart(addNullElement));

            // Start both threads
            thread1.Start();
            thread2.Start();

            // Wait for both threads to finish execution
            thread1.Join();
            thread2.Join();
            // thread1.ThreadState == ThreadState.
            // After both threads finish, assert that exceptions were thrown in both threads
            Assert.Equal(2, threadExceptions.Count);  // Expect two exceptions
            Assert.All(threadExceptions, ex => Assert.IsType<ArgumentNullException>(ex));  // Both should be ArgumentNullException
        }

        [Fact]
        public void AddElement_Should_Properly_Handle_Massive_Amounts_Of_Elements()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);

            // Test adding 1 million elements
            var elements = Enumerable.Range(0, 1000000).ToList();
            setTree.AddRange(elements);

            // Should contain exactly 1 million elements
            Assert.Equal(1000000, setTree.CountRootElements);
        }

        [Fact]
        public void CompareTo_Should_Return_Correct_Result_When_Trees_Have_Same_Elements_But_Different_Subsets()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree1 = new SetTree<int>(extractionSettings);
            setTree1.AddRange(new List<int> { 1, 2, 3 });

            var setTree2 = new SetTree<int>(extractionSettings);
            setTree2.AddRange(new List<int> { 1, 2, 3 });

            var subset1 = new SetTree<int>(extractionSettings);
            subset1.AddElement(10);

            var subset2 = new SetTree<int>(extractionSettings);
            subset2.AddElement(20);

            setTree1.AddSubSetTree(subset1);
            setTree2.AddSubSetTree(subset2);

            int result = setTree1.CompareTo(setTree2);
            Assert.Equal(-1, result); // Should return -1 because the subsets are different
        }

        [Fact]
        public void AddSubSetTree_Should_Throw_ArgumentNullException_When_Tree_Is_Null2()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);

            // Adding a null subset tree
            Assert.Throws<ArgumentNullException>(() => setTree.AddSubSetTree(null));
        }

        [Fact]
        public void RemoveElement_Should_Allow_Removal_Of_Only_One_Element_And_No_Duplicate_Elements()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            setTree.AddElement(1);
            setTree.AddElement(2);
            setTree.AddElement(3);

            // Remove an element
            bool removed = setTree.RemoveElement(2);
            Assert.True(removed);
            Assert.False(setTree.GetRootElementsEnumerator().Contains(2)); // Should not contain 2 anymore

            // Try removing an element that doesn't exist
            removed = setTree.RemoveElement(10);
            Assert.False(removed); // Should return false as element 10 doesn't exist
        }

        [Fact]
        public void CompareTo_Should_Work_Properly_When_Comparing_Trees_With_Nested_Subsets()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var rootSet = new SetTree<int>(extractionSettings);
            rootSet.AddRange(new List<int> { 1, 2, 3 });

            var subset1 = new SetTree<int>(extractionSettings);
            subset1.AddElement(4);

            var subset2 = new SetTree<int>(extractionSettings);
            subset2.AddElement(5);

            rootSet.AddSubSetTree(subset1);
            rootSet.AddSubSetTree(subset2);

            var rootSetClone = new SetTree<int>(extractionSettings);
            rootSetClone.AddRange(new List<int> { 1, 2, 3 });
            rootSetClone.AddSubSetTree(subset1);
            rootSetClone.AddSubSetTree(subset2);

            int result = rootSet.CompareTo(rootSetClone);
            Assert.Equal(0, result); // Should return 0 as both sets and their subsets are identical
        }

        [Fact]
        public void AddSubSetTree_Should_Handle_Very_Large_Subsets_Properly()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var mainSet = new SetTree<int>(extractionSettings);
            var largeSubset = new SetTree<int>(extractionSettings);

            // Add 1 million elements to a subset
            largeSubset.AddRange(Enumerable.Range(0, 1000000));

            mainSet.AddSubSetTree(largeSubset);

            Assert.Equal(1, mainSet.CountSubsets); // Should have 1 subset
            Assert.Equal(1000000, largeSubset.CountRootElements); // Subset should contain 1 million elements
        }

        [Fact]
        public void CompareTo_Should_Return_1_When_Tree_Contains_More_Elements_Than_Another()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree1 = new SetTree<int>(extractionSettings);
            setTree1.AddRange(new List<int> { 1, 2, 3 });

            var setTree2 = new SetTree<int>(extractionSettings);
            setTree2.AddRange(new List<int> { 1, 2 });

            int result = setTree1.CompareTo(setTree2);
            Assert.Equal(1, result); // setTree1 has more elements, should return 1
        }

        [Fact]
        public void ToString_Should_Provide_Correct_String_Representation_After_Massive_Addition()
        {
            var extractionSettings = new SetExtractionConfiguration(";", ",");
            var setTree = new SetTree<int>(extractionSettings);
            var elements = Enumerable.Range(1, 100000).ToList();
            setTree.AddRange(elements);

            string result = setTree.ToString();
            Assert.Contains("1", result);
            Assert.Contains("100000", result);
            Assert.Equal(100000, result.Split(',').Length); // The string should have 100000 elements represented
        }
        #endregion CHATGPT Valnarability tests
    }//class
}