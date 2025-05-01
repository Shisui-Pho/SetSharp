/*
 * File: SetTreeUtility.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 30 March 2025
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

        return tree;
    }//ToElementString

    /// <summary>
    /// Recursively builds the string representation of a set tree.
    /// </summary>
    /// <param name="currentTree">The current set tree to convert.</param>
    /// <returns>A string representing the set tree, where subsets are enclosed in curly braces and empty sets are represented as ∅.</returns>
    private static string BuildTree(ISetTree<T> currentTree)
    {
        if(currentTree.TreeInfo.IsEmptyTree)
        {
            return "{}";//Return nothing (Ø)
        }

        //Here the tree is none empty(it has something in it)
        string representation = "{" + currentTree.RootElements;

        //Check for empty sets
        if (currentTree.TreeInfo.HasNullElements && !currentTree.ExtractionSettings.IgnoreEmptySets)
        {
            if (representation.Length == 1)//Here it means there's only the opening brace without clossing braces
                representation += "{}";
            else
                representation += currentTree.ExtractionSettings.RowTerminator + "{}";
        }


        //Loop through the subsets
        foreach (var subset in currentTree.GetSubsetsEnumerator())
        {
            //Attach each subset in the representation
            string subsetTree = BuildTree(subset);
            representation += subset.ExtractionSettings.RowTerminator + subsetTree;
        }

        //Attach the closing brace
        return representation + "}";
    }
}//BuildTree