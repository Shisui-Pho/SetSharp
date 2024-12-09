/*
 * File: BraceEvaluator.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file contains the implementation of the BraceEvaluator utility class, 
 * which provides functionality to evaluate the correctness of braces in a given 
 * string expression. The class ensures that opening and closing braces are properly 
 * matched and nested according to expected rules.
 * 
 * Key Features:
 * - Validates that the expression starts with '{' and ends with '}'.
 * - Uses a stack to track and match braces in the expression.
 * - Handles edge cases, such as unmatched braces or improper nesting.
 * - Removes extraneous elements from evaluation, ensuring strict brace matching.
 */

namespace SetsLibrary.Utility;

/// <summary>
/// Provides methods to evaluate the correctness of braces in a given expression.
/// </summary>
public static class BraceEvaluator
{
    /// <summary>
    /// Checks if the braces in the specified expression are correctly balanced.
    /// </summary>
    /// <param name="expression">The string expression to evaluate.</param>
    /// <returns>True if the braces are correctly balanced; otherwise, false.</returns>
    public static bool AreBracesCorrect(string expression)
    {
        //First check the opening and clossing braces if they exist
        if (!expression.StartsWith("{") || !expression.EndsWith("}"))
            return false;
        //Stack that will contain all the 
        Stack<char> elements = new();
        //Remove white space
        //expression = expression.Replace(" ", "");

        int lengthOfString = expression.Length;

        //This will keep track of the number of elements that have been evaluated
        int Count = 0;
        foreach (char character in expression)
        {
            Count++;
            if (character == '{')
            {
                elements.Push(character);
                continue;
            }//if we have an oppening brace

            if (character == '}')
            {
                //Cannot have a clossing brace without an oppening brace
                if (elements.Count <= 0)
                    return false;
                //Keep on popping until we either have 
                while (elements.Count > 0 && elements.Peek() != '{')
                {
                    //Pop the elements
                    elements.Pop();
                }
                //If we have popped everything and have not encounterd an oppening brace
                if (elements.Count <= 0)
                    return false;

                //Remove the oppening brace
                elements.Pop();

                //Handle edge case for a string like this : 
                //-{1,2},{3,4}
                //-Without this condition the Brace evaluation will pass
                if (elements.Count == 0 && Count != lengthOfString)
                    return false;

                continue;
            }//end if oppening

            //If there's something in the stack
            //-i.e. An oppening brace
            if (elements.Count > 0)
                elements.Push(character);
        }//for each loop
        return elements.Count == 0;
    }//CheckBraces
}//class
//namespace
