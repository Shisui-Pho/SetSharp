/*
 * File: SetTreeExtractor.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 25 November 2024
 * 
 * Description:
 * This file contains the definition of the SetTreeExtractor class, which provides utility methods 
 * for extracting a set tree from a string expression based on a specified configuration. The class 
 * handles parsing set expressions, removing duplicates, and sorting elements, while also supporting 
 * the extraction of nested subsets to construct a complete set tree structure.
 * 
 * Key Features:
 * - Extracts and constructs a set tree from a string representation of a set expression.
 * - Supports nested subsets and handles braces to identify and parse them correctly.
 * - Allows for custom object conversion through a configurable extraction process.
 * - Ensures the uniqueness of elements in the set using a HashSet and sorts them.
 * - Handles edge cases such as missing braces or malformed expressions gracefully.
 */

namespace SetsLibrary.Utility;

/// <summary>
/// Provides utility methods to extract a set tree from a string expression and configuration.
/// </summary>
/// <typeparam name="T">The type of elements in the set tree. This type must implement <see cref="IComparable{T}"/>.</typeparam>
public class SetTreeExtractor<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Extracts a set tree from a string expression using the specified extraction configuration.
    /// </summary>
    /// <param name="expression">The string representation of the set expression to extract into a set tree.</param>
    /// <param name="extractionConfig">The configuration used for extracting the set, including terminators and optional custom object conversion.</param>
    /// <returns>An instance of <see cref="ISetTree{T}"/> representing the extracted set tree.</returns>
    public static ISetTree<T> Extract(string expression, SetExtractionConfiguration<T> extractionConfig)
    {
        // Remove the first and last brace if they exist
        if (expression.StartsWith("{") && expression.EndsWith("}"))
        {
            expression = expression.Remove(0, 1);
            expression = expression.Remove(expression.Length - 1);
        }

        // Base case: no subsets, return the root set elements as a tree
        if (!expression.Contains("}") && !expression.Contains("{"))
        {
            //Remove last trailing seperators
            if (expression.EndsWith(extractionConfig.RowTerminator))
                expression = expression.Remove(expression.Length - 1);

            if (expression.StartsWith(extractionConfig.RowTerminator))
                expression = expression.Remove(expression.Length - 1);


            IEnumerable<T> rootElements = SortAndRemoveDuplicates(expression, extractionConfig);
            return new SetTree<T>(extractionConfig, rootElements);
        }

        // Queues and stacks to manage braces and subsets
        Queue<int> oppeningBraces = new Queue<int>(); // Holds indices of opening braces
        Stack<int> clossingBraces = new Stack<int>();  // Holds indices of closing braces
        Stack<string> subsets = new Stack<string>();   // Holds the subsets at the first nesting level

        // Loop through all the characters in the expression to identify subsets
        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == '{')
                oppeningBraces.Enqueue(i);
            if (expression[i] == '}')
                clossingBraces.Push(i);

            if (oppeningBraces.Count > 0 && oppeningBraces.Count == clossingBraces.Count)
            {
                // Extract the outermost elements (subsets)
                int start = oppeningBraces.Dequeue();
                int end = clossingBraces.Pop();
                int length = end - start + 1;

                string subset = expression.Substring(start, length);

                // Remove the subset from the original expression to prevent duplicates
                expression = expression.Replace(subset, "");
                i = start; // Reset the index to continue from the correct position

                // Clear the braces management stacks and add the subset
                oppeningBraces = new Queue<int>();
                clossingBraces = new Stack<int>();
                subsets.Push(subset);
            }
        }

        // Create the root tree with the remaining elements after removing subsets
        ISetTree<T> tree = Extract(expression, extractionConfig);

        // Add all subsets as subtrees
        while (subsets.Count > 0)
        {
            string ex = subsets.Pop();
            tree.AddSubSetTree(Extract(ex, extractionConfig));
        }

        return tree;
    }//Extract

    /// <summary>
    /// Sorts the root elements and removes duplicates, ensuring that the set only contains unique elements.
    /// </summary>
    /// <param name="rootElements">A string representing the root elements to be processed.</param>
    /// <param name="extractionConfig">The configuration that specifies terminators and custom object converters.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing the sorted and unique root elements.</returns>
    public static IEnumerable<T> SortAndRemoveDuplicates(string rootElements, SetExtractionConfiguration<T> extractionConfig)
    {
        //Split elements based on the row terminator
        string[] elements = rootElements.Split(extractionConfig.RowTerminator, StringSplitOptions.RemoveEmptyEntries);

        //Use a HashSet to ensure uniqueness of elements
        HashSet<T> uniqueElements = new HashSet<T>();

        //Loop through all elements and add them to the HashSet after converting them to the appropriate type
        foreach (string element in elements)
        {
            try
            {
                //If a custom converter is used, convert using that; otherwise, attempt to convert to T
                if (extractionConfig.IsICustomObject)
                {
                    //Use the custom object converter
                    T item = extractionConfig.ToObject(element);
                    uniqueElements.Add(item);
                }
                else
                {
                    if ((element == "" || element == " ") && typeof(T) != typeof(string))
                        continue;//Ignore element

                    string _elem = element;

                    //Trim sting to remove white spaces
                    if (typeof(T) != typeof(string))
                        _elem = _elem.Trim();

                    //Convert to the specified type T
                    T item = (T)Convert.ChangeType(_elem, typeof(T));
                    uniqueElements.Add(item);
                }
            }
            catch
            (Exception ex)
            {
                //Handle any exceptions during conversion (throwing is optional)
                string det = $"Failed to convert the string \'{element}\' to type of \'{typeof(T)}\'";
                throw new SetsException("Conversion failed due to invalid format", det, ex);
            }
        }//end for each

        // Return the unique elements sorted
        return uniqueElements.OrderBy(x => x);
    }//SortAndRemoveDuplicates
}//class
 //namespace