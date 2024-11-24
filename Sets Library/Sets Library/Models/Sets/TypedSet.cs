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

using SetsLibrary.Interfaces;

namespace SetsLibrary.Models.Sets
{
    /// <summary>
    /// Represents a typed set that can contain elements of a specified type T.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set, which must implement <see cref="IComparable{T}"/>.</typeparam>
    public class TypedSet<T> : BaseSet<T>
        where T : IComparable<T>
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
} // namespace
