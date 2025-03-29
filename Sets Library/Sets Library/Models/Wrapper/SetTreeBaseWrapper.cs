/*
 * File: SetTreeBaseWrapper.cs
 * Author: [Your Name]
 * Date: [Current Date]
 * 
 * Description:
 * Defines the SetTreeBaseWrapper class, which serves as a base wrapper for SetTree instances, providing 
 * basic functionality such as adding elements, getting enumerators for root elements and subsets, 
 * and removing elements. This class ensures that all required methods for interacting with SetTree 
 * are properly delegated to the underlying SetTree instance.
 * 
 * Key Features:
 * - Delegates the core functionality to the wrapped ISetTree instance.
 * - Provides methods to add elements, add subsets, remove elements, and compare sets.
 * - Exposes root element and subset enumerators, along with counting properties.
 */

namespace SetsLibrary;

/// <summary>
/// Represents a base wrapper for a SetTree, delegating functionality to the underlying ISetTree instance.
/// </summary>
/// <typeparam name="T">The type of the elements in the set. This type must implement <see cref="IComparable{T}"/>.</typeparam>
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
public abstract class SetTreeBaseWrapper<T> : IIndexedSetTree<T>
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    where T : IComparable<T>
{
    /// <summary>
    /// 
    /// </summary>
    protected readonly ISetTree<T> setTree;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTreeBaseWrapper{T}"/> class.
    /// </summary>
    /// <param name="setTree">The SetTree instance to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="setTree"/> is <c>null</c>.</exception>
    public SetTreeBaseWrapper(ISetTree<T> setTree)
    {
        // Check for nulls
        ArgumentNullException.ThrowIfNull(setTree, nameof(setTree));
        this.setTree = setTree;
    }
    internal SetTreeBaseWrapper(SetExtractionConfiguration config)
    {
        //Creates a new instance of the set tree
        setTree = new SetTree<T>(config);
    }//ctor default

    /// <summary>
    /// Gets the root elements of the set as a string.
    /// </summary>
    public string RootElements => setTree.RootElements;

    /// <summary>
    /// Gets the total number of elements in the set, including both root elements and subsets.
    /// </summary>
    public int Count => setTree.Count;

    /// <summary>
    /// Gets the number of root elements in the set.
    /// </summary>
    public int CountRootElements => setTree.CountRootElements;

    /// <summary>
    /// Gets the number of subsets in the set.
    /// </summary>
    public int CountSubsets => setTree.CountSubsets;

    /// <summary>
    /// Gets the extraction settings for the set.
    /// </summary>
    public SetExtractionConfiguration ExtractionSettings => setTree.ExtractionSettings;

    /// <summary>
    /// Adds a single element to the set.
    /// </summary>
    /// <param name="element">The element to add.</param>
    public void AddElement(T element)
    {
        setTree.AddElement(element);
    }

    /// <summary>
    /// Adds a range of elements to the set.
    /// </summary>
    /// <param name="elements">The elements to add.</param>
    public void AddRange(IEnumerable<T> elements)
    {
        setTree.AddRange(elements);
    }

    /// <summary>
    /// Adds a range of subsets to the set.
    /// </summary>
    /// <param name="subsets">The subsets to add.</param>
    public void AddRange(IEnumerable<ISetTree<T>> subsets)
    {
        setTree.AddRange(subsets);
    }

    /// <summary>
    /// Adds a subset tree to the set.
    /// </summary>
    /// <param name="tree">The subset tree to add.</param>
    public void AddSubSetTree(ISetTree<T> tree)
    {
        setTree.AddSubSetTree(tree);
    }

    /// <summary>
    /// Compares this set tree with another set tree.
    /// </summary>
    /// <param name="other">The other set tree to compare with.</param>
    /// <returns>A value indicating the relative ordering of the sets.</returns>
    public int CompareTo(ISetTree<T>? other)
    {
        return setTree.CompareTo(other);
    }

    /// <summary>
    /// Gets an enumerator for the root elements in the set.
    /// </summary>
    /// <returns>An enumerator for the root elements.</returns>
    public IEnumerable<T> GetRootElementsEnumerator()
    {
        return setTree.GetRootElementsEnumerator();
    }

    /// <summary>
    /// Gets an enumerator for the subsets in the set.
    /// </summary>
    /// <returns>An enumerator for the subsets.</returns>
    public IEnumerable<ISetTree<T>> GetSubsetsEnumerator()
    {
        return setTree.GetSubsetsEnumerator();
    }

    /// <summary>
    /// Finds the index of a given element in the set.
    /// </summary>
    /// <param name="element">The element to find.</param>
    /// <returns>The index of the element, or -1 if not found.</returns>
    public int IndexOf(T element)
    {
        return setTree.IndexOf(element);
    }

    /// <summary>
    /// Finds the index of a given subset in the set.
    /// </summary>
    /// <param name="subset">The subset to find.</param>
    /// <returns>The index of the subset, or -1 if not found.</returns>
    public int IndexOf(ISetTree<T> subset)
    {
        return setTree.IndexOf(subset);
    }

    /// <summary>
    /// Removes an element from the set.
    /// </summary>
    /// <param name="element">The element to remove.</param>
    /// <returns><c>true</c> if the element was removed; otherwise, <c>false</c>.</returns>
    public bool RemoveElement(T element)
    {
        return setTree.RemoveElement(element);
    }

    /// <summary>
    /// Removes a subset from the set.
    /// </summary>
    /// <param name="element">The subset to remove.</param>
    /// <returns><c>true</c> if the subset was removed; otherwise, <c>false</c>.</returns>
    public bool RemoveElement(ISetTree<T> element)
    {
        return setTree.RemoveElement(element);
    }
    public override string ToString()
    {
        return setTree.ToString();
    }
    #region Methods to be overriden

    /// <summary>
    /// Gets the root element at the specified index.
    /// </summary>
    /// <param name="index">The index of the root element to retrieve.</param>
    /// <returns>The root element at the specified index, or <c>null</c> if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
    public abstract T? GetRootElementByIndex(int index);

    /// <summary>
    /// Gets the subset at the specified index.
    /// </summary>
    /// <param name="index">The index of the subset to retrieve.</param>
    /// <returns>The subset at the specified index, or <c>null</c> if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
    public abstract ISetTree<T>? GetSubsetByIndex(int index);

    /// <summary>
    /// Clears the internal elements and subsets of the SetTree.
    /// </summary>
    public abstract void Clear();
    #endregion Methods to be overriden
}//class
