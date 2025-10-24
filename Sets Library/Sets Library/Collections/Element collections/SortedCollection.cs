/*
 * File: BaseSortedCollection.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the BaseSortedCollection<TElement> abstract class, which provides 
 * a foundation for implementing a sorted collection of elements. The class ensures elements 
 * remain sorted after each addition and provides functionalities such as adding elements 
 * (individually or in bulk), removing elements, searching for elements, and iterating through the collection.
 * 
 * Key Features:
 * - Uses a generic List<TElement> as the internal data structure for storing elements.
 * - Supports binary search to find the appropriate insertion index, maintaining sorted order.
 * - Provides methods for adding, removing, and checking for elements.
 * - Implements both generic and non-generic enumerators for iterating through the collection.
 * - Ensures type safety by requiring elements to implement the IComparable<TElement> interface.
 * - Includes robust error handling for null arguments and out-of-range indices.
 * 
 * Dependencies:
 * - System.Collections for non-generic IEnumerable implementation.
 * - System.Collections.Generic for List<T> and IEnumerable<T>.
 */
using System.Collections;
namespace SetsLibrary.Collections;

///<summary>
///Represents a base class for a sorted collection of elements.
///Provides functionality for adding, removing, and searching elements.
///</summary>
///<typeparam name="TElement">The type of elements in the collection, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
public class SortedCollection<TElement> : ISortedCollection<TElement>
    where TElement : IComparable<TElement>
{
    //Data field to store elements
    private List<TElement> _elements;

    //Properties

    ///<summary>
    ///Gets the number of elements in the collection.
    ///</summary>
    public int Count => _elements.Count;

    //Indexer

    ///<summary>
    ///Gets or sets the element at the specified index.
    ///</summary>
    ///<param name="index">The index of the element to get or set.</param>
    ///<returns>The element at the specified index.</returns>
    ///<exception cref="IndexOutOfRangeException">Thrown if the index is out of range.</exception>
    public TElement this[int index]
    {
        get
        {
            if (index < 0 || index >= _elements.Count)
                throw new IndexOutOfRangeException(nameof(index));

            return _elements[index];
        }
    }

    //Constructors

    ///<summary>
    ///Initializes a new instance of the <see cref="SortedCollection{TElement}"/> class.
    ///</summary>
    public SortedCollection()
    {
        Clear();
    }//ctor main

    ///<summary>
    ///Initializes a new instance of the <see cref="SortedCollection{TElement}"/> class with an existing set of elements.
    ///</summary>
    ///<param name="elements">The collection of elements to initialize the sorted collection with.</param>
    public SortedCollection(IEnumerable<TElement> elements)
    {
        Clear();
        AddRange(elements);
    }//ctor 02

    //Methods

    ///<summary>
    ///Adds an element to the collection, ensuring the collection remains sorted. Duplicates will also be added.
    ///</summary>
    ///<param name="value">The element to add.</param>
    ///<exception cref="ArgumentNullException">Thrown if the element is null.</exception>
    public void AddIfDuplicate(TElement value)
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
    }//AddIfDuplicate
    ///<summary>
    ///Adds an element to the collection, ensuring the collection remains sorted. Duplicates will not be added.
    ///</summary>
    ///<param name="val">The element to add.</param>
    ///<exception cref="ArgumentNullException">Thrown if the element is null.</exception>
    public void Add(TElement val)
    {
        //Don't add if null
        ArgumentNullException.ThrowIfNull(val, nameof(val));

        //If first element
        if (Count == 0)
        {
            _elements.Add(val);
            return;
        }

        int indexOfInsertion = FindIndexOfInsertion(val);

        //Only add unique elements
        if (indexOfInsertion < Count && _elements[indexOfInsertion].CompareTo(val) == 0) 
            return;

        
        _elements.Insert(indexOfInsertion, val);
    }//Add
    ///<summary>
    ///Finds the index at which the specified value should be inserted to maintain sorted order.
    ///</summary>
    ///<param name="valueToAdd">The value to find the insertion index for.</param>
    ///<returns>The index where the value should be inserted.</returns>
    private int FindIndexOfInsertion(TElement valueToAdd)
    {
        int lowerBound = 0;
        int upperBound = Count; //Set upperBound to Count to allow insertion at the end

        while (lowerBound < upperBound)
        {
            int mid = (lowerBound + upperBound) / 2;
            int comparer = _elements[mid].CompareTo(valueToAdd);

            if (comparer < 0)
            {
                lowerBound = mid + 1; //Search right
            }
            else
            {
                upperBound = mid; //Search left
            }
        }
        return lowerBound; //This is the correct insertion index
    }//FindIndexOfInsertion

    ///<summary>
    ///Adds a range of elements to the collection, maintaining the sorted order.
    ///</summary>
    ///<param name="coll">The collection of elements to add.</param>
    ///<exception cref="ArgumentNullException">Thrown if the provided collection is null.</exception>
    public void AddRange(IEnumerable<TElement> coll)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(coll, nameof(coll));

        foreach (var item in coll)
        {
            AddIfDuplicate(item);
        }
    }//AddRange

    ///<summary>
    ///Checks whether the collection contains the specified element.
    ///</summary>
    ///<param name="val">The element to check for existence.</param>
    ///<returns>True if the element is found, otherwise false.</returns>
    ///<exception cref="ArgumentNullException">Thrown if the element is null.</exception>
    public bool Contains(TElement val)
    {
        ArgumentNullException.ThrowIfNull(val, nameof(val));

        //Get the index of val
        int index = IndexOf(val);

        return index >= 0 && index < Count;
    }//Contains

    ///<summary>
    ///Gets an enumerator that iterates through the collection.
    ///</summary>
    ///<returns>An enumerator for the collection.</returns>
    public IEnumerator<TElement> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }//GetEnumerator

    ///<summary>
    ///Finds the index of a specific element in the collection.
    ///</summary>
    ///<param name="val">The element to find.</param>
    ///<returns>The index of the element, or -1 if the element is not found.</returns>
    ///<exception cref="ArgumentNullException">Thrown if the element is null.</exception>
    public int IndexOf(TElement val)
    {
        ArgumentNullException.ThrowIfNull(val, nameof(val));

        int index = FindIndexOfInsertion(val);

        if (index < Count && val.CompareTo(_elements[index]) == 0)
        {
            return index;
        }

        return -1; //Element not found
    }//IndexOf

    ///<summary>
    ///Removes the specified element from the collection.
    ///</summary>
    ///<param name="val">The element to remove.</param>
    ///<returns>True if the element was successfully removed, otherwise false.</returns>
    ///<exception cref="ArgumentNullException">Thrown if the element is null.</exception>
    public bool Remove(TElement val)
    {
        ArgumentNullException.ThrowIfNull(val, nameof(val));

        int index = IndexOf(val);

        if (index < 0 || index >= Count)
            return false; //Element not found

        RemoveAt(index);

        return true; //Element removed successfully
    }//Remove

    ///<summary>
    ///Removes the element at the specified index from the collection.
    ///</summary>
    ///<param name="index">The index of the element to remove.</param>
    ///<exception cref="ArgumentOutOfRangeException">Thrown if the index is out of range.</exception>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        _elements.RemoveAt(index);
    }//RemoveAt

    ///<summary>
    ///Clears the collection, removing all elements.
    ///</summary>
    public void Clear()
    {
        _elements = new List<TElement>();
    }//Clear

    //Explicit interface implementation for non-generic IEnumerable

    ///<summary>
    ///Gets a non-generic enumerator that iterates through the collection.
    ///</summary>
    ///<returns>A non-generic enumerator for the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }//GetEnumerator
}//class
 //namespace