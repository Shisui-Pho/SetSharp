/*
 * File: SetTreeBuilder.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 31 October 2025
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
using System.Collections;
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
    private struct ExtractElementResult
    {
        [MemberNotNullWhen(true, (nameof(Value)))]
        public bool HasValue { get; set; } = false;
        public T? Value { get; set; }
        public ExtractElementResult(bool hasValue, T? value = default(T))
        {
            HasValue = hasValue;
            Value = value;
        }
    }
    /// <summary>
    /// Extracts a set tree from a string expression using the specified extraction configuration.
    /// </summary>
    /// <param name="expression">The string representation of the set expression to extract into a set tree.</param>
    /// <param name="extractionConfig">The configuration used for extracting the set, including terminators and optional custom object conversion.</param>
    /// <returns>An instance of <see cref="ISetTree{T}"/> representing the extracted set tree.</returns>
    public static ISetTree<T> BuildSetTree(string expression, SetsConfigurations extractionConfig)
    {
        //Remove the first and last brace if they exist
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

        var roots = new SortedCollection<T>();
        var subsets = BreakDownExpression(expression, extractionConfig, roots);
        var tree = new SetTree<T>(extractionConfig ,roots, subsets);
        return tree;
    }//BuildSetTree
    private static SortedCollection<ISetTree<T>> BreakDownExpression(string expression, SetsConfigurations config, SortedCollection<T> rootElements)
    {
        SortedCollection<ISetTree<T>> subsets = [];

        //Loop through the entire string
        for (int i = 0; i < expression.Length; i++)
        {
            //Get the current character
            char _current = expression[i];

            //Check for the opening brace
            if (_current == '{')
            {
                //Extract a subset string
                var subset = ExtractSubSet(expression, ref i);
                //-Recursively build a subtree
                var subtree = BuildSetTree(subset, config);

                subsets.Add(subtree);
            }
            else
            {
                //A normal element
                var el = ExtractObject(expression, config, ref i);
                if (el.HasValue) rootElements.Add(el.Value);
            }
        }//end for
        return subsets;
    }//BreakDownExpression
    private static ExtractElementResult ExtractObject(string expression, SetsConfigurations config, ref int startIndex)
    {
        //TODO: Handle escape sequences
        if (expression[startIndex] == config.RowTerminator[0])
        {
            ThrowIfInvalidRowTerminatorInExpression(expression, config.RowTerminator, startIndex);
            startIndex += config.RowTerminator.Length;//Skip the terminator itself
        }
        
        //Extract element until we hit a row terminator
        StringBuilder element = new StringBuilder();
        bool startedLookAtElements = false;
        while(startIndex < expression.Length)
        {
            if (expression[startIndex] == config.RowTerminator[0])
            {
                ThrowIfInvalidRowTerminatorInExpression(expression, config.RowTerminator, startIndex);
                startIndex += config.RowTerminator.Length - 1;
                break;//Word was successfully extracted
            }
            
            //if we hit an opening brace without any row terminator, then it's invalid
            if (expression[startIndex] == '{' && startedLookAtElements)
            {
                throw new SetsException("Encountered an opening brace without any row terminator");
            }

            //If we hit a closing brace
            if (expression[startIndex] == '}' || expression[startIndex] == '{')
            {
                startIndex = startedLookAtElements ? startIndex :
                             startIndex - config.RowTerminator.Length;
                break;
            }
            //Ignore spaces
            if (!(expression[startIndex] == ' ' && !startedLookAtElements))
            {
                element.Append(expression[startIndex]);
                startedLookAtElements = true;
            }
            startIndex++;
        }//end while

        string?[] fields = GetFields(element.ToString(), config, out bool isEmpty);

        //Convert element
        T? obj = default;

        if (startedLookAtElements) obj = ConvertToObject(fields, config);

        return new ExtractElementResult { HasValue = startedLookAtElements, Value = obj};
    }//ExtractObject
    private static string ExtractSubSet(string expression, ref int startIndex)
    {
        //The expression should start with an opening brace
        if (expression[startIndex] != '{')
            throw new SetsException("Expression not in the right format");

        StringBuilder str = new();
        Stack<int> stBraces = [];
        while (startIndex < expression.Length)
        {
            if (expression[startIndex] == '{')
                stBraces.Push(0);//Does not matter what I push to the stack

            if(expression[startIndex] == '}')
            {
                if(stBraces.Count == 0)
                {
                    string details = $"Encountered a closing without an opening brace\n {expression}\n{"".PadLeft(startIndex)}";
                    throw new MissingBraceException("Missing an opening brace matching.", details);
                }
                //Remove one closing brace
                stBraces.Pop();
            }
            str.Append(expression[startIndex]);
            startIndex++;

            //If the current subset has been extracted
            if (stBraces.Count == 0)
                break;
        }

        //- If it is then we have an error
        if (stBraces.Count > 0)
        {
            string details = $"Encountered {stBraces.Count} opening brace(s) without corresponding closing braces." +
                $"\nThe indexes are as follows : {string.Join(" , ", stBraces)}";
            throw new MissingBraceException("Missing closing braces.", details);
        }//end if invalid brace

        return str.ToString();
    }//ExtractSubSet
    private static void ThrowIfInvalidRowTerminatorInExpression(string expression, string rowTerminator,int startIndex)
    {
        //TODO: Handle escape sequences
        int length = rowTerminator.Length;
        int index = 0;
        //Verify the row terminator
        while (index < rowTerminator.Length)
        {
            if (expression[startIndex + index] != rowTerminator[index])
                throw new SetsException("Expression contained an invalid row terminator, make sure to use the correct escape sequance where applicable.",
                                        $"At index {startIndex + index} is a row terminator character not matching the configuration.");
            index++;
        }
    }//ThrowIfInvalidRowTerminatorInExpression
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
    /// <param name="hasAnEmptySet">True if the set is empty.</param>
    /// <param name="countEmptySets">Number of empty sets per recors/row if using custom object sets, otherwise number of elements. </param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing the sorted and unique root elements.</returns>
    public static IEnumerable<T> SortAndRemoveDuplicates(string rootElements, SetsConfigurations extractionConfig, out bool hasAnEmptySet, out int countEmptySets)
    {
        //If there is nothing
        if(string.IsNullOrEmpty(rootElements) || string.IsNullOrWhiteSpace(rootElements))
        {
            hasAnEmptySet = false;
            countEmptySets = 0;
            return Enumerable.Empty<T>();
        }    

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
            //If a custom converter is used, convert using that; otherwise, attempt to convert to T
            T item = ConvertToObject(fields, extractionConfig);
            uniqueElements.Add(item);
        }//end for each
        return uniqueElements;
    }//SortAndRemoveDuplicates
    private static T ConvertToObject(string?[] fields, SetsConfigurations config)
    {
        T? item = default(T);
        try
        {
            ExtractElementResult? result = null;
            if (config.IsICustomObject)
            {
                item = ToCustomObject(fields, config);
            }
            else
            {
                result = ToPrimitiveType(fields[0]);
            }

            if (result.HasValue && result.Value.HasValue)
                item = result.Value.Value;

            if (item is null)
            {
                throw new SetsException("Unable to complete conversion, check configurations.");
            }
        }
        catch(Exception ex)
        {
            string element = string.Join(config.FieldTerminator, fields.Select(b => b is not null));
            //Handle any exceptions during conversion (throwing is optional)
            string det = $"Failed to convert the string \'{element}\' to type of \'{typeof(T)}\'";
            throw new SetsException("Conversion failed due to invalid format", det, ex);
        }//catch

        return item;
    }//ConvertToObject
    private static ExtractElementResult ToPrimitiveType([NotNullIfNotNull("field")] string? field)
    {
        //Check if the field is empty first
        if (field is null)
            return new ExtractElementResult { HasValue = false };

        //Trim sting to remove white spaces
        if (typeof(T) != typeof(string))
        {
            field = field.Trim();
        }
        
        if(typeof(T) != typeof(string) && string.IsNullOrEmpty(field))
            return new ExtractElementResult { HasValue = false };

        //ConvertToObject to the specified type T
        var item = (T)Convert.ChangeType(field, typeof(T));

        return new ExtractElementResult { HasValue = true, Value = item};
    }//ToPrimitiveType
    private static T? ToCustomObject(string?[] fields,SetsConfigurations extractionConfig)
    {
        var converter = ((CustomSetsConfigurations<T>)extractionConfig).Funct_ToObject;

        if (converter is null || fields is null)
        {
            string details = $"The object \'{typeof(T)}\' was marked as an element of a custom object set, but no parameter of converter wass passed.";
            throw new SetsConfigurationException("Failed to convert object to set.", details);
        }

        //Do the conversion
        var item = converter(fields);
        return item;
    }
    private static string?[] GetFields(string element, SetsConfigurations config, out bool isEmpty)
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
        isEmpty = countEmptyFields == fields.Length;

        return fields;
    }//IsEmptySet
}//class