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

using SetsLibrary.Collections;
using System.Text;

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
    public static ISetTree<T> Extract(string expression, SetExtractionConfiguration extractionConfig)
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
            IEnumerable<T> rootElements = SortAndRemoveDuplicates(expression, extractionConfig);
            return new SetTree<T>(extractionConfig, rootElements);
        }

        //Get the subsets
        var subsets = ExtractSubsets(expression, out string root);

        // Create the root tree with the remaining elements after removing subsets
        ISetTree<T> tree = Extract(root, extractionConfig);

        // Add all subsets as subtrees
        while (subsets.Count > 0)
        {
            string ex = subsets.Pop();
            tree.AddSubSetTree(Extract(ex, extractionConfig));
        }

        return tree;
    }//Extract
    private static Stack<string> ExtractSubsets(string expression, out string rootElements)
    {
        //Here the first braces have been removed 
        Stack<string> subsets = [];

        //String builders to hold the current subset and current root
        StringBuilder _roots = new();
        StringBuilder _subSet = new();

        //Stack to keep track of the braces
        Stack<int> stBraces = [];

        //Loop through the entire string
        for (int i = 0; i < expression.Length; i++)
        {
            //Get the current character
            char _current = expression[i];

            //Check for the opening brace
            if (_current == '{')
            {
                //Push something to the stack
                stBraces.Push(i);

                //Attach the brace to the current subset
                _subSet.Append(_current);

                //Move to the next character
                continue;
            }

            //Check for the closing brace
            if (_current == '}')
            {
                //If there's nothing in the stack then we have an error
                if (stBraces.Count == 0)
                {
                    string details = $"Encountered a closing without an opening brace\n {expression}\n{"".PadLeft(i)}";
                    throw new MissingBraceException("Missing an opening brace matching.", details);
                }
                _subSet.Append(_current);//Attach the closing braces

                //Remove one opening brace
                _ = stBraces.Pop();

                if (stBraces.Count == 0)
                {
                    //Here it means that the subset has been extracted
                    //-We need to add the subset
                    subsets.Push(_subSet.ToString());

                    //Reset the subset 
                    _subSet.Clear();
                }

                //Move to the next character
                continue;
            }

            //When the code reaches here, it means tha we either add to the subset
            // or the root elements

            if (stBraces.Count == 0)// THis goes to the root
                _roots.Append(_current);
            else
                _subSet.Append(_current);
        }//end for

        //Check if the stack is empty
        //- If it is then we have an error
        if (stBraces.Count > 0)
        {
            string details = $"Encountered {stBraces.Count} opening brace(s) without corresponding closing braces." +
                $"\nThe indexes are as follows : {string.Join(" , ", stBraces)}";
            throw new MissingBraceException("Missing closing braces.", details);
        }//end if invalid brace

        rootElements = _roots.ToString();
        return subsets;
    }//ExtractSubSets
    /// <summary>
    /// Sorts the root elements and removes duplicates, ensuring that the set only contains unique elements.
    /// </summary>
    /// <param name="rootElements">A string representing the root elements to be processed.</param>
    /// <param name="extractionConfig">The configuration that specifies terminators and custom object converters.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing the sorted and unique root elements.</returns>
    public static IEnumerable<T> SortAndRemoveDuplicates(string rootElements, SetExtractionConfiguration extractionConfig)
    {
        //Split elements based on the row terminator
        string[] elements = rootElements.Split(extractionConfig.RowTerminator, StringSplitOptions.RemoveEmptyEntries);

        //Use a sorted collection to handle sorting and duplicates
        ISortedElements<T> uniqueElements = new SortedElements<T>();

        //Loop through all elements and add them to the HashSet after converting them to the appropriate type
        foreach (string element in elements)
        {
            try
            {
                T? item = default(T);
                //If a custom converter is used, convert using that; otherwise, attempt to convert to T
                if (extractionConfig.IsICustomObject)
                {
                    ////Use the custom object converter
                    //if (SetExtractionConfiguration.ToObject is not null)
                    //    item = SetExtractionConfiguration.ToObject(element, extractionConfig);

                    //The above code works
                    //But I can't do this ????
                    //item = SetExtractionConfiguration.ToObject?.Invoke(element);// Why????

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
                    item = (T)Convert.ChangeType(_elem, typeof(T));
                    //uniqueElements.Add(item);
                }

                //This contains method uses a binary search algorithm 
                //-The add method adds the elements in a sorted order
                //if (!uniqueElements.Contains(item))
                //    uniqueElements.Add(item);
                if (item is not null)
                {
                    uniqueElements.AddIfUnique(item);
                }
                else
                {
                    throw new SetsException("Unable to complete conversion, check confiurations.");
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

        return uniqueElements;
    }//SortAndRemoveDuplicates
}//class
 //namespace