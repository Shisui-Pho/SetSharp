/*
 * File: CustomSetsConfigurations.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 29 March 2025
 * 
 * Description:
 * Defines the CustomSetsConfigurations class which inherit from SetsConfigurationException,
 * 
 * Key Features:
 * Introduces a new feature for converting any object
 */

namespace SetsLibrary;

/// <inheritdoc/>
internal class CustomSetsConfigurations<TObject> : SetsConfigurations
    where TObject : IComparable<TObject>
{
    //Add an additional feature for custom conversion
    public Func<string?[], TObject>? Funct_ToObject { get; set; } = null;

    /// <inheritdoc/>
    public CustomSetsConfigurations(string rowTerminator) : base(rowTerminator)
    {
        IsICustomObject = true;
    }
    /// <inheritdoc/>
    public CustomSetsConfigurations(string fieldTerminator, string rowTerminator, bool ignoreEmptySets) : base(fieldTerminator, rowTerminator,ignoreEmptySets)
    {
        IsICustomObject = true;
    }
}