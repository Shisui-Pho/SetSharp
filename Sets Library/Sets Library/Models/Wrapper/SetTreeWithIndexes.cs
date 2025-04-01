/*
 * File: SetTreeWithIndexes.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * Defines the SetTreeWithIndexes class, a wrapper around the SetTree class that extends its functionality
 * to allow access to elements and subsets by index, and provides a method to clear the internal elements 
 * and subsets directly. It also includes checks for index bounds and handles cases where the internal 
 * SetTree structure is not available.
 * 
 * Key Features:
 * - Allows retrieving root elements and subsets by their index.
 * - Provides a clear method for directly clearing internal elements and subsets.
 * - Implements validation for index range and null checks.
 */

namespace SetsLibrary;

/// <summary>
/// Represents a wrapper for the SetTree class that extends functionality for accessing elements 
/// and subsets by index and allows clearing internal elements and subsets directly.
/// </summary>
/// <typeparam name="T">The type of the elements in the set. This type must implement <see cref="IComparable{T}"/>.</typeparam>
public class SetTreeWithIndexes<T> : BaseSetTreeWithIndexes<T>
    where T : IComparable<T>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTreeWithIndexes{T}"/> class.
    /// </summary>
    /// <param name="setTree">The SetTree instance to wrap.</param>
    public SetTreeWithIndexes(ISetTree<T> setTree) : base(setTree)
    {
    }
    internal SetTreeWithIndexes(SetsConfigurations config) : base(config) { }
    /// <summary>
    /// Gets the root element at the specified index.
    /// </summary>
    /// <param name="index">The index of the root element to retrieve.</param>
    /// <returns>The root element at the specified index, or <c>null</c> if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
    public override T? GetRootElementByIndex(int index)
    {
        if (index >= Count || index < 0)
            throw new ArgumentOutOfRangeException("index");

        var _assSetTree = base.setTree as SetTree<T>;

        if (_assSetTree != null)
            return _assSetTree._elements[index]; // Access directly from the internal collection

        // Else, do a loop
        int currentIndex = 0;
        foreach (T item in base.GetRootElementsEnumerator())
        {
            if (currentIndex == index)
                return item;

            currentIndex++;
        }
        return default(T);
    }

    /// <summary>
    /// Gets the subset at the specified index.
    /// </summary>
    /// <param name="index">The index of the subset to retrieve.</param>
    /// <returns>The subset at the specified index, or <c>null</c> if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
    public override ISetTree<T>? GetSubsetByIndex(int index)
    {
        if (index >= Count || index < 0)
            throw new ArgumentOutOfRangeException("index");

        // Else, do a loop
        int currentIndex = 0;
        foreach (ISetTree<T> item in base.GetSubsetsEnumerator())
        {
            if (currentIndex == index)
                return item;

            currentIndex++;
        }
        return default(ISetTree<T>);
    }

    /// <summary>
    /// Clears the internal elements and subsets of the SetTree.
    /// </summary>
    /// <exception cref="SetsException">Thrown if the internal SetTree structure is not available.</exception>
    public override void Clear()
    {
        var _assSetTree = base.setTree as SetTree<T>;
        if (_assSetTree != null)
        {
            _assSetTree._elements.Clear();
            _assSetTree._subSets.Clear();
        }
        else
        {
            string det = $"The clear methods work only if a SetTree implementation is used.";
            throw new SetsException("Cannot access the clear properties of underlying data structure.", det);
        }
    }
}//class