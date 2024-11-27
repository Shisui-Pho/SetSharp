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
    /// <typeparam name="T">The type of elements in the set, which must implement <see cref="IComparable{T}"/> and <see cref="IConvertible"/>.</typeparam>
    public class TypedSet<T> : BaseSet<T>
        where T : IComparable<T>, IConvertible
    {
        #region Constructors
        public TypedSet(SetExtractionConfiguration<T> extractionConfiguration) : base(extractionConfiguration)
        {
        }

        public TypedSet(string expression, SetExtractionConfiguration<T> config) : base(expression, config)
        {
        }
        #endregion Constructors
        protected override IStructuredSet<T> BuildNewSet(string setString)
        {
            return new TypedSet<T>(setString, this.ExtractionConfiguration);
        }//BuildNewSet

        protected override IStructuredSet<T> BuildNewSet()
        {
            return new TypedSet<T>(this.ExtractionConfiguration);
        }//BuildNewSet
    } // class
} // namespace
