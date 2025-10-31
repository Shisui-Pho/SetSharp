/*
 * File: MissingBraceException.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 21 December 2024
 * 
 * Description:
 * Defines the MissingBraceException class, a custom exception used within the SetsLibrary to indicate 
 * that a brace was expected but not found during parsing or evaluation. This class extends the base 
 * SetsException class, allowing for additional detail information to be included, which aids in debugging 
 * and providing context about the missing brace issue.
 * 
 * Key Features:
 * - Inherits from the SetsException class.
 * - Provides a specialized exception for handling missing braces in expressions.
 * - Supports the inclusion of both a custom message and detailed information when throwing the exception.
 * - Can be used for more specific exception handling related to brace evaluation or parsing errors.
 */

namespace SetSharp
{
    /// <summary>
    /// Represents a custom exception that is thrown when a missing brace is detected in an expression.
    /// Inherits from <see cref="SetsException"/> to allow additional details about the missing brace issue.
    /// </summary>
    [Serializable]
    public class MissingBraceException : SetsException
    {
        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingBraceException"/> class with the specified message and details.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="details">Additional details about the missing brace issue.</param>
        public MissingBraceException(string? message, string? details)
            : base(message, details)
        {
        }

        // Methods (inherited from SetsException)
        // - Message: Returns a formatted string containing both the exception message and additional details.
        // - SetDetails: Allows the exception details to be updated.
        // - GetFullExceptionMessage: Generates a full exception message that includes the inner exception, if any.
    }
}
