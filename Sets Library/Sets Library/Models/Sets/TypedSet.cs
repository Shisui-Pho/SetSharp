/*
 * File: TypedSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the TypedSet<T> class, which represents a typed set 
 * that can contain elements of a specific type T. The elements in the set 
 * must implement the IComparable<T> interface. The class provides methods 
 * for checking if an element exists in the set and merging with another set, 
 * although both methods are currently unimplemented.
 * 
 * Key Features:
 * - Typed set that enforces a type constraint on the elements.
 * - Placeholder methods for checking element presence (Contains).
 * - Merge functionality with other sets (MergeWith) for future implementation.
 */

namespace SetsLibrary;

/// <summary>
/// Represents a typed set that can contain elements of a specified type T.
/// </summary>
/// <typeparam name="T">The type of elements in the set, which must implement <see cref="IComparable{T}"/> and <see cref="IConvertible"/>.</typeparam>
public class TypedSet<T> : BaseSet<T>
    where T : IComparable<T>, IConvertible
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TypedSet{T}"/> class with the specified extraction configuration.
    /// </summary>
    /// <param name="extractionConfiguration">The configuration used for extracting set elements and subsets.</param>
    public TypedSet(SetsConfigurations extractionConfiguration) : base(extractionConfiguration)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypedSet{T}"/> class with the specified string expression and extraction configuration.
    /// </summary>
    /// <param name="expression">The string representation of the set expression.</param>
    /// <param name="config">The configuration used for extracting set elements and subsets.</param>
    public TypedSet(string expression, SetsConfigurations config) : base(expression, config)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TypedSet{T}"/> class by injecting an existing instance of IIndexedSetTree.
    /// This constructor allows for complete control over the set tree, including the expression and extraction settings.
    /// </summary>
    /// <param name="indexedSetTree">An existing instance of an IIndexedSetTree that provides both the set elements 
    /// and configuration for extraction.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexedSetTree"/> is null.</exception>
    public TypedSet(IIndexedSetTree<T> indexedSetTree) : base(indexedSetTree) { }
    #endregion Constructors

    #region Overrides

    /// <summary>
    /// Builds and returns a new <see cref="TypedSet{T}"/> based on the provided string representation of the set.
    /// </summary>
    /// <param name="setString">The string representation of the set to be created.</param>
    /// <returns>A new instance of <see cref="TypedSet{T}"/>.</returns>
    public override IStructuredSet<T> BuildNewSet(string setString)
    {
        return new TypedSet<T>(setString, this.ExtractionConfiguration);
    } // BuildNewSet

    /// <summary>
    /// Builds and returns a new, empty <see cref="TypedSet{T}"/>.
    /// </summary>
    /// <returns>A new, empty instance of <see cref="TypedSet{T}"/>.</returns>
    public override IStructuredSet<T> BuildNewSet()
    {
        return new TypedSet<T>(this.ExtractionConfiguration);
    } // BuildNewSet
    /// <summary>
    /// Builds and returns a new set based on the provided indexed set tree wrapper.
    /// </summary>
    /// <param name="tree">The indexed tree of the set</param>
    /// <returns>A new instance of structered set</returns>
    protected internal override IStructuredSet<T> BuildNewSet(IIndexedSetTree<T> tree)
    {
        return new TypedSet<T>(tree);
    }//BuildNewSet
    #endregion Overrides
} // class
// namespace
