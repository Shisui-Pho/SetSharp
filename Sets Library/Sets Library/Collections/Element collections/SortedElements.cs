using System.Collections;

namespace SetsLibrary.Collections
{
    /// <summary>
    /// Represents a collection of sorted elements that allows for addition, removal, and searching within the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection, which must implement <see cref="IComparable{T}"/>.</typeparam>
    public class SortedElements<T> : BaseSortedCollection<T> , ISortedElements<T>
        where T : IComparable<T>
    {
        //Default constructor
        public SortedElements() : base(){}//ctor main

        //Constructor that initializes the collection with an existing set of elements
        public SortedElements(IEnumerable<T> elements)
            : base(elements){}//ctor 01
    }//class
} //namespace SetLibrary.Collections