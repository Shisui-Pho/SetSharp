/*
 * File: SetResultType.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the SetResultType enumeration, which represents different types 
 * of set relationships, including subsets, proper sets, and universal sets. It is used 
 * to categorize and evaluate set comparisons in structured set operations.
 * 
 * Key Features:
 * - Provides an enumeration for different set relationship types.
 * - Helps categorize results of set comparisons, such as subset checks and equality tests.
 * - Used for evaluating proper sets, universal sets, and other set relationships.
 */

namespace SetsLibrary.Models
{
    /// <summary>
    /// Represents the types of relationships between sets.
    /// </summary>
    public enum SetResultType
    {
        /// <summary>
        /// Default relationship type when no specific relation is identified.
        /// </summary>
        Default,

        /// <summary>
        /// Indicates that the set is universal, containing all possible elements.
        /// </summary>
        Universal,

        /// <summary>
        /// Indicates that the set is a subset of another set.
        /// </summary>
        SubSet,

        /// <summary>
        /// Indicates that the set is not a subset of another set.
        /// </summary>
        NotASubSet,

        /// <summary>
        /// Indicates that the set is a proper subset of another set.
        /// </summary>
        ProperSet,

        /// <summary>
        /// Indicates that the set is non-universal, meaning it does not contain all possible elements.
        /// </summary>
        Non_Universal,

        /// <summary>
        /// Indicates that the two sets are equal.
        /// </summary>
        Same_Set
    }//enum
}//namespace
