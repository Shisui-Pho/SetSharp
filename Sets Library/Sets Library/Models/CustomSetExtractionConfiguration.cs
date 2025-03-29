/*
 * File: CustomSetExtractionConfiguration.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 29 March 2025
 * 
 * Description:
 * Defines the CustomSetExtractionConfiguration class which inherit from SetsConfigurationException,
 * 
 * Key Features:
 * Introduces a new feature for converting any object
 */

namespace SetsLibrary;

/// <inheritdoc/>
internal class CustomSetExtractionConfiguration<TObject> : SetExtractionConfiguration
    where TObject : IComparable<TObject>
{
    //Add an additional feature for custom conversion
    public Func<string?[], SetExtractionConfiguration, TObject>? Funct_ToObject { get; set; } = null;

    /// <inheritdoc/>
    public CustomSetExtractionConfiguration(string rowTerminator) : base(rowTerminator)
    {
        IsICustomObject = true;
    }
    /// <inheritdoc/>
    public CustomSetExtractionConfiguration(string fieldTerminator, string rowTerminator) : base(fieldTerminator, rowTerminator)
    {
        IsICustomObject = true;
    }
}