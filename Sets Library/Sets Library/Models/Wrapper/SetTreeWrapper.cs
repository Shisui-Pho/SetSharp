/*
 * File: SetTreeWrapper.cs
 * Author: [Your Name]
 * Date: [Current Date]
 * 
 * Description:
 * Defines the SetTreeWrapper class, a wrapper around the SetTree class that extends its functionality
 * to allow access to elements and subsets by index, and provides a method to clear the internal elements 
 * and subsets directly. It also includes checks for index bounds and handles cases where the internal 
 * SetTree structure is not available.
 * 
 * Key Features:
 * - Allows retrieving root elements and subsets by their index.
 * - Provides a clear method for directly clearing internal elements and subsets.
 * - Implements validation for index range and null checks.
 */

using SetsLibrary.Interfaces;
using SetsLibrary.Models.SetTree;
namespace SetsLibrary.Models;

/// <summary>
/// Represents a wrapper for the SetTree class that extends functionality for accessing elements 
/// and subsets by index and allows clearing internal elements and subsets directly.
/// </summary>
/// <typeparam name="T">The type of the elements in the set. This type must implement <see cref="IComparable{T}"/>.</typeparam>
public class SetTreeWrapper<T> : SetTreeBaseWrapper<T>
    where T : IComparable<T>
{
    private readonly SetTree<T>? _assSetTree = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTreeWrapper{T}"/> class.
    /// </summary>
    /// <param name="setTree">The SetTree instance to wrap.</param>
    public SetTreeWrapper(ISetTree<T> setTree) : base(setTree)
    {
        if (setTree is SetTree<T>) // if they are the same, cast the setTree structure
            _assSetTree = setTree as SetTree<T>;
    }

    /// <summary>
    /// Gets the root element at the specified index.
    /// </summary>
    /// <param name="index">The index of the root element to retrieve.</param>
    /// <returns>The root element at the specified index, or <c>null</c> if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
    public T? GetRootElementByIndex(int index)
    {
        if (index >= Count || index < 0)
            throw new ArgumentOutOfRangeException("index");

        if (_assSetTree != null)
            return _assSetTree._elements[index]; // Access directly from the internal collection

        // Else, do a loop
        int currentIndex = 0;
        foreach (T item in base.GetRootElementsEnumarator())
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
    public ISetTree<T>? GetSubsetByIndex(int index)
    {
        if (index >= Count || index < 0)
            throw new ArgumentOutOfRangeException("index");

        // Else, do a loop
        int currentIndex = 0;
        foreach (ISetTree<T> item in base.GetSubsetsEnumarator())
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
    /// <exception cref="ArgumentException">Thrown if the internal SetTree structure is not available.</exception>
    public void Clear()
    {
        if (_assSetTree != null)
        {
            _assSetTree._elements.Clear();
            _assSetTree._subSets.Clear();
        }
        else
        {
            throw new ArgumentException("Use a separate wrapper for this operation or wrap this one.");
        }
    }
}//class