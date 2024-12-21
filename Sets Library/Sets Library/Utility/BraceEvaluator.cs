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
 * - Strictly matches braces and ignores other characters in the expression.
 * - Provides feedback on the position of the incorrect brace if the expression is invalid.
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
    /// <param name="expression">The string expression to evaluate. This should primarily contain '{' and '}' characters.</param>
    /// <param name="positionOfIncorrectBrace">The position of the incorrect brace if the expression is invalid; otherwise, -1.</param>
    /// <returns>True if the braces are correctly balanced and nested; otherwise, false.</returns>
    /// <remarks>
    /// The expression is considered valid if:
    /// - Every opening brace '{' has a matching closing brace '}'.
    /// - Braces are properly nested.
    /// - The expression starts with '{' and ends with '}'.
    /// 
    /// This method uses a stack to track the opening braces and ensures that each closing brace matches the most recent unmatched opening brace.
    /// </remarks>
    public static bool AreBracesCorrect(string expression, out int positionOfIncorrectBrace)
    {
        // Initialize the position of incorrect brace to -1 (indicating no issues)
        positionOfIncorrectBrace = -1;

        // Check if the expression starts with '{' and ends with '}'
        if (!expression.StartsWith("{") || !expression.EndsWith("}"))
            return false;

        // Stack to track opening braces
        Stack<char> elements = new();

        int lengthOfString = expression.Length;

        // Iterate through each character in the expression
        foreach (char character in expression)
        {
            positionOfIncorrectBrace++;

            // Check for opening brace
            if (character == '{')
            {
                elements.Push(character); // Add the opening brace to the stack
                continue;
            }

            // Check for closing brace
            if (character == '}')
            {
                // If there is no matching opening brace, the expression is invalid
                if (elements.Count <= 0)
                    return false;

                // Keep popping until we find the matching opening brace '{'
                while (elements.Count > 0 && elements.Peek() != '{')
                {
                    // Pop the elements
                    elements.Pop();
                }

                // If no matching opening brace found, return false
                if (elements.Count <= 0)
                    return false;

                // Remove the matching opening brace
                elements.Pop();

                // Handle edge case for a string like this: '-{1,2},{3,4}'
                // Without this condition, the brace evaluation would incorrectly pass
                if (elements.Count == 0 && (positionOfIncorrectBrace + 1) != lengthOfString)
                    return false;

                continue;
            }

            // If there's something in the stack (i.e., an opening brace),
            // we push non-brace characters onto the stack
            if (elements.Count > 0)
                elements.Push(character);
        }

        // If there are unmatched opening braces left in the stack, it's invalid
        if (elements.Count > 0)
        {
            positionOfIncorrectBrace = expression.Length - 1;
            return false;
        }

        // If all braces are matched and there are no unmatched opening braces, return true
        positionOfIncorrectBrace = -1;
        return true;
    }
}