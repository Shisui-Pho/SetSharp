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
 * - Handles edge cases, such as unmatched braces, improper nesting, and extraneous elements.
 * - Strictly evaluates only braces and ignores other characters.
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
    /// <param name="positionOfIncorrectBrace">The position of the incorrect brace if the expression is invalid, otherwise -1 if valid.</param>
    /// <returns>True if the braces are correctly balanced and nested; otherwise, false.</returns>
    /// <remarks>
    /// The expression is considered valid if:
    /// - Every opening brace '{' has a matching closing brace '}'.
    /// - Braces are properly nested.
    /// - The expression starts with '{' and ends with '}'.
    /// 
    /// This method uses a stack to track the opening braces and ensure that each closing brace matches the most recent unmatched opening brace.
    /// </remarks>
    public static bool AreBracesCorrect(string expression, out int positionOfIncorrectBrace)
    {
        // Initialize the position of incorrect brace to -1 (indicating no issues)
        positionOfIncorrectBrace = -1;

        // Check if the expression starts with '{' and ends with '}'.
        if (!expression.StartsWith("{") || !expression.EndsWith("}"))
            return false;

        // Stack to track opening braces
        Stack<char> elements = new();

        int lengthOfString = expression.Length;

        // Iterate through each character in the expression
        for (int position = 0; position < lengthOfString; position++)
        {
            char character = expression[position];

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
                {
                    positionOfIncorrectBrace = position;
                    return false;
                }

                // Pop the most recent opening brace
                elements.Pop();

                // Handle edge case where there is an extra character between braces
                // E.g., in expressions like '-{1,2},{3,4}', we should ensure braces are strictly balanced
                if (elements.Count == 0 && (position + 1) != lengthOfString)
                {
                    positionOfIncorrectBrace = position;
                    return false;
                }

                continue;
            }

            // Ignore non-brace characters (other characters won't affect the brace validation)
            if (elements.Count > 0)
                elements.Push(character); // Push the character onto the stack, if we have an opening brace
        }

        // If there are unmatched opening braces left in the stack, it's invalid
        if (elements.Count > 0)
        {
            positionOfIncorrectBrace = expression.Length - 1;
            return false;
        }

        // If we pass all checks, the braces are balanced
        positionOfIncorrectBrace = -1;
        return true;
    }
}
