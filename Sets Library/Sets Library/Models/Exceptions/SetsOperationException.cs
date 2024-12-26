/*
 * File: SetsOperationException.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 December 2024
 * 
 * Description:
 * Defines the SetsOperationException class, a custom exception used specifically 
 * for handling errors related to operations on sets within the SetsLibrary. 
 * This class extends the SetsException class, allowing for more detailed error 
 * reporting related to set operations.
 * 
 * Key Features:
 * - Inherits from the SetsException class.
 * - Supports specifying a message and additional details when the exception is thrown.
 * - Allows for inner exceptions, supporting better exception chaining.
 * - Provides context-specific information related to set operations.
 */

namespace SetsLibrary
{
    /// <summary>
    /// Represents a custom exception for errors encountered during operations on sets.
    /// This class extends <see cref="SetsException"/> and is used specifically 
    /// for reporting errors related to set operations in the SetsLibrary.
    /// </summary>
    public class SetsOperationException : SetsException
    {
        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SetsOperationException"/> class with 
        /// a specified message and additional details about the error.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="details">Additional details about the exception.</param>
        public SetsOperationException(string? message, string? details)
            : base(message, details) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetsOperationException"/> class with 
        /// a specified message, additional details, and an inner exception that caused this exception.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="details">Additional details about the exception.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public SetsOperationException(string? message, string? details, Exception? innerException)
            : base(message, details, innerException) { }
    }
}