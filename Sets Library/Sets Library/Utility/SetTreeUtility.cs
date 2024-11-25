/*
 * File: SetTreeUtility.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 25 November 2024
 * 
 * Description:
 * Defines the SetTreeUtility<T> class, which provides utility methods for working with tree-based sets. 
 * It includes functionality to convert a set tree into a string representation, accounting for nested subsets and 
 * empty sets.
 * 
 * Key Features:
 * - Converts a set tree to a string representation with proper formatting.
 * - Handles nested subsets and empty sets (represented by ∅).
 * - Implements a recursive method to build the tree string representation.
 * - Ensures null checks and throws appropriate exceptions.
 */

using SetsLibrary.Interfaces;

namespace SetsLibrary.Utility;

/// <summary>
/// Provides utility methods for working with set trees, including converting a set tree to its string representation.
/// </summary>
/// <typeparam name="T">The type of elements in the set tree. This type must implement <see cref="IComparable{T}"/>.</typeparam>
public static class SetTreeUtility<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Converts the specified set tree into a string representation.
    /// </summary>
    /// <param name="setTree">The set tree to convert to a string.</param>
    /// <returns>A string representing the set tree, enclosed in curly braces. Empty sets are represented by the symbol ∅.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="setTree"/> is null.</exception>
    public static string ToElementString(ISetTree<T> setTree)
    {
        // Check for nulls
        ArgumentNullException.ThrowIfNull(setTree, nameof(setTree));

        // Recursively build the tree string representation
        string tree = BuildTree(setTree);

        return "{" + tree + "}";
    }//ToElementString

    /// <summary>
    /// Recursively builds the string representation of a set tree.
    /// </summary>
    /// <param name="currentTree">The current set tree to convert.</param>
    /// <returns>A string representing the set tree, where subsets are enclosed in curly braces and empty sets are represented as ∅.</returns>
    private static string BuildTree(ISetTree<T> currentTree)
    {
        // Base case: if there are no subsets, return the root elements or the empty set symbol
        if (currentTree.CountSubsets == 0)
        {
            if (string.IsNullOrEmpty(currentTree.RootElements) || string.IsNullOrWhiteSpace(currentTree.RootElements))
                return "\u2205"; // This is the empty set/element (∅)

            // Return the root elements as a string
            return currentTree.RootElements;
        }

        // Initialize a string to build the element tree
        string elementTree = "";

        // Loop through all subsets of the current tree
        foreach (ISetTree<T> subtree in currentTree.GetSubsetsEnumarator())
        {
            // Recursively retrieve the string representation of the subtree
            string sub = BuildTree(subtree);

            // Add the subset to the element tree, enclosing it in curly braces
            elementTree += "{" + sub + "}";
        }

        return elementTree;
    }
}//BuildTree