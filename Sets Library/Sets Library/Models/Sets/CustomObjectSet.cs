/*
 * File: CustomObjectSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file contains the implementation of the CustomObjectSet<T> class, 
 * which represents a custom set capable of holding elements of type T. 
 * The elements in the set must implement both the IComparable<T> and 
 * ICustomObjectConverter<T> interfaces. This set allows for operations 
 * such as checking for the presence of an element and merging with 
 * other sets, although the methods are currently not implemented.
 * 
 * Key Features:
 * - Defines a set for elements that are comparable and convertible from a string.
 * - Provides a placeholder method for checking element presence (Contains).
 * - Includes a method for merging sets (MergeWith) with future implementation.
 */
namespace SetsLibrary;

/// <summary>
/// Represents a custom set that can contain elements of type T, 
/// which must be comparable and convertible from a string representation.
/// </summary>
/// <typeparam name="T">The type of elements in the set, which must implement <see cref="IComparable{T}"/> and <see cref="ICustomObjectConverter{T}"/>.</typeparam>
public class CustomObjectSet<T> : BaseSet<T>
    where T : IComparable<T>, ICustomObjectConverter<T>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomObjectSet{T}"/> class with the specified extraction configuration.
    /// </summary>
    /// <param name="extractionConfiguration">The configuration to be used for extracting set elements and subsets.</param>
    public CustomObjectSet(SetExtractionConfiguration<T> extractionConfiguration)
        : base(extractionConfiguration)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomObjectSet{T}"/> class with the specified string expression and extraction configuration.
    /// </summary>
    /// <param name="expression">The string representation of the set expression.</param>
    /// <param name="config">The configuration to be used for extracting set elements and subsets.</param>
    public CustomObjectSet(string expression, SetExtractionConfiguration<T> config)
        : base(expression, config)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomObjectSet{T}"/> class by injecting an existing instance of IIndexedSetTree.
    /// This constructor allows for complete control over the set tree, including the expression and extraction settings.
    /// </summary>
    /// <param name="indexedSetTree">An existing instance of an IIndexedSetTree that provides both the set elements 
    /// and configuration for extraction.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexedSetTree"/> is null.</exception>
    public CustomObjectSet(IIndexedSetTree<T> indexedSetTree) : base(indexedSetTree) { }
    #endregion Constructors

    #region Overrides

    /// <summary>
    /// Builds and returns a new <see cref="CustomObjectSet{T}"/> based on the provided string representation of the set.
    /// </summary>
    /// <param name="setString">The string representation of the set to be created.</param>
    /// <returns>A new instance of <see cref="CustomObjectSet{T}"/>.</returns>
    public override IStructuredSet<T> BuildNewSet(string setString)
    {
        return new CustomObjectSet<T>(setString, this.ExtractionConfiguration);
    }//BuildNewSet

    /// <summary>
    /// Builds and returns a new, empty <see cref="CustomObjectSet{T}"/>.
    /// </summary>
    /// <returns>A new, empty instance of <see cref="CustomObjectSet{T}"/>.</returns>
    public override IStructuredSet<T> BuildNewSet()
    {
        return new CustomObjectSet<T>(this.ExtractionConfiguration);
    }//BuildNewSet
    /// <summary>
    /// Builds and returns a new set based on the provided indexed set tree wrapper.
    /// </summary>
    /// <param name="tree">The indexed tree of the set</param>
    /// <returns>A new instance of structured set</returns>
    protected internal override IStructuredSet<T> BuildNewSet(IIndexedSetTree<T> tree)
    {
        return new CustomObjectSet<T>(tree);
    }//BuildNewSet

    #endregion Overrides
} // class