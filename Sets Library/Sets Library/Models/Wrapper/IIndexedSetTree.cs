namespace SetsLibrary;
/// <summary>
/// Represents a wrapper for the SetTree class that extends functionality for accessing elements 
/// and subsets by index and allows clearing internal elements and subsets directly.
/// </summary>
/// <typeparam name="T">The type of the elements in the set. This type must implement <see cref="IComparable{T}"/>.</typeparam>
public interface IIndexedSetTree<T> : ISetTree<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Gets the root element at the specified index.
    /// </summary>
    /// <param name="index">The index of the root element to retrieve.</param>
    /// <returns>The root element at the specified index, or <c>null</c> if not found.</returns>
    T? GetRootElementByIndex(int index);
    /// <summary>
    /// Gets the subset at the specified index.
    /// </summary>
    /// <param name="index">The index of the subset to retrieve.</param>
    /// <returns>The subset at the specified index, or <c>null</c> if not found.</returns>
    ISetTree<T>? GetSubsetByIndex(int index);

    /// <summary>
    /// Clears the internal elements and subsets of the SetTree.
    /// </summary>
    void Clear();
}//class