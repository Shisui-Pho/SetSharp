using System.Collections;

namespace SetsLibrary.Collections
{
    /// <summary>
    /// Represents a collection of sorted elements that allows for addition, removal, and searching within the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection, which must implement <see cref="IComparable{T}"/>.</typeparam>
    public class SortedElements<T> : ISortedElements<T>
        where T : IComparable<T>
    {
        //Data field to store elements
        private List<T> _elements;

        //Properties
        public int Count => _elements.Count;

        //Indexer
        /// <summary>
        /// Gets an element in the collection on the specified index.
        /// </summary>
        /// <param name="index">The zero based index</param>
        /// <returns>The element on the specified index</returns>
        public T this[int index]
        {
            get
            {
                if(index <  0 || index >= _elements.Count)
                    throw new IndexOutOfRangeException(nameof(index));

                return _elements[index];
            }//end getter
        }//indexer

        //Default constructor
        public SortedElements()
        {
            Clear();
        }//ctor main

        //Constructor that initializes the collection with an existing set of elements
        public SortedElements(IEnumerable<T> elements)
        {
            Clear();
            AddRange(elements);
        }//ctor 01

        //Methods

        /// <summary>
        /// Adds an element to the collection in sorted order.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(T value)
        {
            //Don't add if null
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            //If first element
            if (Count == 0)
            {
                _elements.Add(value);
                return;
            }

            //Find the index for insertion
            int pointOfInsertion = FindIndexOfInsertion(value);
            _elements.Insert(pointOfInsertion, value);
        }//Add

        /// <summary>
        /// Finds the index at which the specified value should be inserted.
        /// </summary>
        /// <param name="valueToAdd">The value to add.</param>
        /// <returns>The index where the value should be inserted.</returns>
        private int FindIndexOfInsertion(T valueToAdd)
        {
            int lowerbound = 0;
            int upperbound = Count; //Set upperbound to Count to allow insertion at the end

            while (lowerbound < upperbound)
            {
                int mid = (lowerbound + upperbound) / 2;
                int comparer = _elements[mid].CompareTo(valueToAdd);

                if (comparer < 0)
                {
                    lowerbound = mid + 1; //Search right
                }
                else
                {
                    upperbound = mid; //Search left
                }
            }//end while
            return lowerbound; //This is the correct insertion index

        }//FindIndexOfInsertion


        /// <summary>
        /// Adds a range of elements to the collection in sorted order.
        /// </summary>
        /// <param name="coll">The collection of elements to be added.</param>
        public void AddRange(IEnumerable<T> coll)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNull(coll, nameof(coll));

            foreach (var item in coll)
            {
                Add(item);
            }
        }//AddRange

        /// <summary>
        /// Checks if the specified element exists in the collection.
        /// </summary>
        /// <param name="val">The element to be checked.</param>
        /// <returns>True if the element is found; otherwise, false.</returns>
        public bool Contains(T val)
        {
            ArgumentNullException.ThrowIfNull(val, nameof(val));

            return IndexOf(val) >= 0;
        }//Contains

        /// <summary>
        /// Gets the enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }//GetEnumerator

        /// <summary>
        /// Finds the index of the specified element.
        /// </summary>
        /// <param name="val">The element whose index is to be found.</param>
        /// <returns>The index of the element if found; otherwise, -1.</returns>
        public int IndexOf(T val)
        {
            ArgumentNullException.ThrowIfNull(val, nameof(val));

            int index = FindIndexOfInsertion(val);

            if (index < Count && val.CompareTo(_elements[index]) == 0)
            {
                return index;
            }

            return -1; //Element not found
        }//IndexOf

        /// <summary>
        /// Removes the specified element from the collection.
        /// </summary>
        /// <param name="val">The element to be removed.</param>
        /// <returns>True if the element was successfully removed; otherwise, false.</returns>
        public bool Remove(T val)
        {
            ArgumentNullException.ThrowIfNull(val, nameof(val));

            int index = IndexOf(val);

            if (index < 0) return false; //Element not found

            RemoveAt(index);

            return true; //Element removed successfully
        }//Remove

        /// <summary>
        /// Removes an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to be removed.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _elements.RemoveAt(index);
        }//RemoveAt

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            _elements = new List<T>();
        }//Clear

        //Explicit interface implementation for non-generic IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }//GetEnumerator
    }//class
} //namespace SetLibrary.Collections