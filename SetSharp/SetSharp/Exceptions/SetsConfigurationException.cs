/*
 * File: SetsConfigurationException.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 22 December 2024
 * 
 * Description:
 * Defines the SetsConfigurationException class, a custom exception used within the SetsLibrary to indicate 
 * there was a problem with the configuration of the sets objects. This class extends the base 
 * SetsException class, allowing for additional detail information to be included, which aids in debugging 
 * and providing context about the missing brace issue.
 * 
 * Key Features:
 * - Inherits from the SetsException class.
 * - Supports the inclusion of both a custom message and detailed information when throwing the exception.
 */
namespace SetSharp;

/// <summary>
/// Represents a custom exception that is thrown when there is something wrong with the configurations.
/// Inherits from <see cref="SetsException"/> to allow additional details about issue.
/// </summary>
[Serializable]
public class SetsConfigurationException : SetsException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetsConfigurationException"/> class with the specified details.
    /// </summary>
    /// <param name="details">Additional details about the exception.</param>
    public SetsConfigurationException(string? details) : base(details)
    {
    }//ctor main
    /// <summary>
    /// Initializes a new instance of the <see cref="SetsConfigurationException"/> class with the specified message and details.
    /// </summary>
    /// <param name="message">The message that describes the exception.</param>
    /// <param name="details">Additional details about the exception.</param>
    public SetsConfigurationException(string? message, string? details) : base(message, details)
    {
    }//ctor 02
    /// <summary>
    /// Initializes a new instance of the <see cref="SetsConfigurationException"/> class with the specified message, details, and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the exception.</param>
    /// <param name="details">Additional details about the exception.</param>
    /// <param name="innerException">The inner exception that caused this exception.</param>
    public SetsConfigurationException(string? message, string? details, Exception? innerException) : base(message, details, innerException)
    {
    }//ctor 03
     // Methods (inherited from SetsException)
     // - Message: Returns a formatted string containing both the exception message and additional details.
     // - SetDetails: Allows the exception details to be updated.
     // - GetFullExceptionMessage: Generates a full exception message that includes the inner exception, if any.
}//class