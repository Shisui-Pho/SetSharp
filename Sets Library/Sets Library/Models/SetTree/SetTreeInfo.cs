/*
 * File: NullElemensInfo.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 30 March 2025
 * 
 * Description:
 * Defines the `NullElementsInfo` class, which is used to determine whether a set tree contains any null elements
 * and to track the number of such null elements.
 * 
 * Key Features:
 * - Provides a flag indicating whether null elements exist in the set tree.
 * - Tracks the number of null elements in the set.
 */

namespace SetSharp;

/// <summary>
/// Represents information about null elements in a set tree, including a flag to indicate the presence of null elements
/// and a count of the number of null elements.
/// </summary>
public class SetTreeInfo
{
    /// <summary>
    /// Get a value indicating if a tree is null or not.
    /// </summary>
    public bool IsEmptyTree { get; internal set; } = true;

    /// <summary>
    /// Gets a value indicating whether the set tree contains any null elements.
    /// </summary>
    public bool HasNullElements { get; internal set; }

    /// <summary>
    /// Gets the number of null elements present in the set tree.
    /// </summary>
    public int NumberOfNullElements { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTreeInfo"/> class.
    /// </summary>
    /// <param name="hasNullElements">A boolean value indicating whether the set contains null elements.</param>
    /// <param name="numberOfNullElements">The number of null elements in the set.</param>
    public SetTreeInfo(bool hasNullElements, int numberOfNullElements)
    {
        HasNullElements = hasNullElements;
        NumberOfNullElements = numberOfNullElements;
    }
}