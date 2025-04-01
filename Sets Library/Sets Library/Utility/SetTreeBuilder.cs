/*
 * File: SetTreeBuilder.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 29 March 2025
 * 
 * Description:
 * This file contains the definition of the SetTreeBuilder class, which provides utility methods 
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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Linq;

namespace SetsLibrary.Utility;

/// <summary>
/// Provides utility methods to extract a set tree from a string expression and configuration.
/// </summary>
/// <typeparam name="T">The type of elements in the set tree. This type must implement <see cref="IComparable{T}"/>.</typeparam>
public class SetTreeBuilder<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Extracts a set tree from a string expression using the specified extraction configuration.
    /// </summary>
    /// <param name="expression">The string representation of the set expression to extract into a set tree.</param>
    /// <param name="extractionConfig">The configuration used for extracting the set, including terminators and optional custom object conversion.</param>
    /// <returns>An instance of <see cref="ISetTree{T}"/> representing the extracted set tree.</returns>
    public static ISetTree<T> BuildSetTree(string expression, SetExtractionConfiguration extractionConfig)
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
            IEnumerable<T> rootElements = SortAndRemoveDuplicates(expression, extractionConfig, out bool hasNulls, out int countNulls);
            var treeStructure = new SetTree<T>(extractionConfig, rootElements);
            treeStructure.WillHaveNullElements(hasNulls, countNulls);
            return treeStructure;
        }

        //Get the subsets
        var subsets = ExtractSubsets(expression,extractionConfig.RowTerminator.Length,out string root);

        // Create the root tree with the remaining elements after removing subsets
        ISetTree<T> tree = BuildSetTree(root, extractionConfig);

        // Add all subsets as subtrees
        while (subsets.Count > 0)
        {
            string ex = subsets.Pop();
            tree.AddSubSetTree(BuildSetTree(ex, extractionConfig));
        }

        return tree;
    }//BuildSetTree
    private static Stack<string> ExtractSubsets(string expression,int lenRowTerminator ,out string rootElements)
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

                    //Skip the next row terminator
                    i += lenRowTerminator;

                    //Remove the previouse terminator from the root if it exists
                    _roots = RemoveLastTerminator(_roots, lenRowTerminator, i < expression.Length);
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

    private static StringBuilder RemoveLastTerminator(StringBuilder roots, int lenRowTerminator, bool isEndOfExpression)
    {
        if (lenRowTerminator > roots.Length || isEndOfExpression)
            return roots;

        //Remove the last trailing row terminator
        roots = roots.Remove(roots.Length - lenRowTerminator, lenRowTerminator);

        return roots;
    }//RemoveLastTerminator

    /// <summary>
    /// Sorts the root elements and removes duplicates, ensuring that the set only contains unique elements.
    /// </summary>
    /// <param name="rootElements">A string representing the root elements to be processed.</param>
    /// <param name="extractionConfig">The configuration that specifies terminators and custom object converters.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing the sorted and unique root elements.</returns>
    public static IEnumerable<T> SortAndRemoveDuplicates(string rootElements, SetExtractionConfiguration extractionConfig, out bool hasAnEmptySet, out int countEmptySets)
    {
        hasAnEmptySet = false;
        countEmptySets = 0;
        //Split elements based on the row terminator
        string[] elements = rootElements.Split(extractionConfig.RowTerminator);

        //Use a sorted collection to handle sorting and duplicates
        ISortedElements<T> uniqueElements = new SortedElements<T>();

        //Loop through all elements and add them to the HashSet after converting them to the appropriate type
        foreach (string element in elements)
        {
            var fields = GetFields(element, extractionConfig, out bool isEmptySet);

            if (isEmptySet)
            {
                //Ignore the set/element
                hasAnEmptySet = true;
                countEmptySets += 1;
                continue;
            }
            T? item = default(T);

            try
            {
                //If a custom converter is used, convert using that; otherwise, attempt to convert to T
                if (extractionConfig.IsICustomObject)
                {
                    item = ToCustomObject(fields, extractionConfig);
                }
                else
                {
                    item = ToPrimitiveType(fields[0]);
                }

                //Check for nullls
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
    private static T? ToPrimitiveType([NotNullIfNotNull("field")]string? field)
    {
        //Check if the field is empty first
        if (field is null)
            return default(T);

        //Trim sting to remove white spaces
        if (typeof(T) != typeof(string))
            field = field.Trim();

        //Convert to the specified type T
        var item = (T)Convert.ChangeType(field, typeof(T));

        return item;
    }//ToPrimitiveType
    private static T? ToCustomObject(string?[] fields,SetExtractionConfiguration extractionConfig)
    {
        //Call api to convert to custom object
        var converter = ((CustomSetExtractionConfiguration<T>)extractionConfig).Funct_ToObject;

        if (converter is null)
        {
            string details = $"The object \'{typeof(T)}\' was marked as an element of a custom object set, but no parameter of converter wass passed.";
            throw new SetsConfigurationException("Failed to convert object to set.", details);
        }

        //Do the conversion
        var item = converter(fields);
        return item;
    }
    private static string?[] GetFields(string element, SetExtractionConfiguration config, out bool isempty)
    {
        //Split the string according to it's field terminator
        string?[] fields = element.Split(config.FieldTerminator);

        //Check if all the fields are empty
        int countEmptyFields = 0;
        for (int i = 0; i< fields.Length; i++)
        {
            fields[i] = fields[i]?.Trim();
            if (string.IsNullOrWhiteSpace(fields[i]))
            {
                //Replace with a null
                fields[i] = null;
                countEmptyFields++;
            }
        }
        isempty = countEmptyFields == fields.Length;

        return fields;
    }//IsEmptySet
}//class