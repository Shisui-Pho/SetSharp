/*
 * File: Set.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the Set structure, which represents a mathematical set 
 * with a name, a string representation of its elements, and its cardinality (number of elements).
 * The structure is lightweight and immutable, making it suitable for serialization, 
 * display purposes, or basic metadata tracking.
 * 
 * Key Features:
 * - Encapsulation of set metadata: name, element string, and cardinality.
 * - Immutable design for safe usage across various scenarios.
 * - Designed for extensibility within the SetLibrary framework.
 */

namespace SetLibrary;

/// <summary>
/// Represents a set with a name, a string representation of its elements, and its cardinality.
/// </summary>
public struct Set
{
    /// <summary>
    /// Gets the name of the set.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the string representation of the elements in the set.
    /// </summary>
    public string ElementString { get; private set; }

    /// <summary>
    /// Gets the number of elements in the set.
    /// </summary>
    public int Cardinality { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Set"/> struct.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    /// <param name="set">The string representation of the set's elements.</param>
    /// <param name="cardinality">The number of elements in the set.</param>
    internal Set(string name, string set, int cardinality)
    {
        this.Name = name;
        this.ElementString = set;
        this.Cardinality = cardinality;
    } // ctor
} // structure
// namespace
