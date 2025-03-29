/*
 * File: CustomSetExtractionConfiguration.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 25 November 2024
 * 
 * Description:
 * Defines the SetExtractionConfiguration class, which specifies the configuration
 * for extracting sets, including terminators for fields and rows, and an optional 
 * custom converter for converting string literals into objects. It also ensures that
 * reserved characters cannot be used as terminators.
 * 
 * Key Features:
 * - Supports configuration with field and row terminators.
 * - Allows integration of a custom object converter implementing ICustomObjectConverter<T>.
 * - Includes validation to ensure terminators are not null, distinct, and do not contain reserved characters.
 * - Provides a method to convert string records into objects using the provided converter.
 */

namespace SetsLibrary;

/// <inheritdoc/>
internal class CustomSetExtractionConfiguration<TObject> : SetsConfigurationException
    where TObject : IComparable<TObject>
{
    //Add an additional feature for custom conversion
    public Func<string, SetsConfigurationException>? Funct_ToObject { get; private set; } = null;
    /// <inheritdoc/>
    public CustomSetExtractionConfiguration(string? details) : base(details)
    {
    }
    /// <inheritdoc/>
    public CustomSetExtractionConfiguration(string? message, string? details) : base(message, details)
    {
    }
    /// <inheritdoc/>
    public CustomSetExtractionConfiguration(string? message, string? details, Exception? innerException) : base(message, details, innerException)
    {
    }
}