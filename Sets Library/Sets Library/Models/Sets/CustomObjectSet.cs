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

using SetLibrary.Models;
using SetsLibrary.Interfaces;

namespace SetsLibrary.Models.Sets;

/// <summary>
/// Represents a custom set that can contain elements of type T, 
/// which must be comparable and convertible from a string representation.
/// </summary>
/// <typeparam name="T">The type of elements in the set, which must implement <see cref="IComparable{T}"/> and <see cref="ICustomObjectConverter{T}"/>.</typeparam>
internal class CustomObjectSet<T> : BaseSet<T>
    where T : IComparable<T>, ICustomObjectConverter<T>
{
    /// <summary>
    /// Checks if the specified element exists in the set.
    /// </summary>
    /// <param name="Element">The element to check for presence in the set.</param>
    /// <returns>True if the element is found; otherwise, false.</returns>
    public override bool Contains(T Element)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Merges the current set with another set, returning a new set that contains elements from both.
    /// </summary>
    /// <param name="set">The set to merge with.</param>
    /// <returns>A new <see cref="IStructuredSet{T}"/> containing elements from both sets.</returns>
    public override IStructuredSet<T> MergeWith(IStructuredSet<T> set)
    {
        throw new NotImplementedException();
    }
} // class
