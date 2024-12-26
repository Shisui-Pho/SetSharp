/*
 * File: SetsException.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 16 December 2024
 * 
 * Description:
 * Defines the SetsException class, a custom exception used within the SetsLibrary. 
 * It provides additional detail information that can be included when throwing 
 * an exception related to sets, allowing for better context and debugging.
 * 
 * Key Features:
 * - Inherits from the base Exception class.
 * - Allows for the inclusion of a custom details message along with the exception message.
 * - Supports inner exceptions, enabling better exception chaining.
 * - Provides properties for retrieving exception details.
 */

namespace SetsLibrary
{
    /// <summary>
    /// Represents a custom exception for the SetsLibrary with additional detail information.
    /// This class can be used as a base for other custom exceptions in the library.
    /// </summary>
    [Serializable]
    public class SetsException : Exception
    {
        // Properties

        /// <summary>
        /// Gets the additional details associated with the exception.
        /// </summary>
        public string? Details { get; private set; }

        /// <summary>
        /// Gets the link to the website of the project.
        /// </summary>
        public override string? HelpLink { get; set; } = "https://github.com/Shisui-Pho/Sets-Library";
        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SetsException"/> class with the specified details.
        /// </summary>
        /// <param name="details">Additional details about the exception.</param>
        public SetsException(string? details)
            : base()
        {
            Details = details;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetsException"/> class with the specified message and details.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="details">Additional details about the exception.</param>
        public SetsException(string? message, string? details)
            : base(message)
        {
            Details = details;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetsException"/> class with the specified message, details, and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="details">Additional details about the exception.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public SetsException(string? message, string? details, Exception? innerException)
            : base(message, innerException)
        {
            Details = details;
        }

        // Methods

        /// <summary>
        /// Returns a formatted error message combining the exception message and the details.
        /// </summary>
        /// <returns>A formatted string containing both the exception message and additional details.</returns>
        public override string Message
        {
            get
            {
                return base.Message + (Details != null ? $"\n - Details: {Details}" : string.Empty);
            }
        }

        /// <summary>
        /// Allows the exception details to be updated after instantiation.
        /// This can be useful in derived classes or when additional context is available.
        /// </summary>
        /// <param name="newDetails">The new details string to be set.</param>
        public void SetDetails(string? newDetails)
        {
            Details = newDetails;
        }

        /// <summary>
        /// Generates a full exception message including the message, inner exception (if any), and additional details.
        /// This method is helpful for logging or debugging purposes.
        /// </summary>
        /// <returns>A full exception message as a string.</returns>
        public string GetFullExceptionMessage()
        {
            var fullMessage = Message;
            if (InnerException != null)
            {
                fullMessage += $" Inner Exception: {InnerException.Message}";
            }
            return fullMessage;
        }
    }//class
}//namespace