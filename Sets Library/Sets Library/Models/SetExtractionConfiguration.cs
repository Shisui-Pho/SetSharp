/*
 * File: SetExtractionConfiguration.cs
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

/// <summary>
/// Represents the configuration for extracting sets, including terminators, optional custom converter, 
/// and validation for reserved characters used in terminators.
/// </summary>
public class SetExtractionConfiguration
{
    // Reserved characters that cannot be used in field or row terminators
    private const string RESERVED_CHARACTERS = "{}";

    /// <summary>
    /// Gets the field terminator used to separate fields in a record.
    /// </summary>
    public string FieldTerminator { get; private set; }

    /// <summary>
    /// Gets the row terminator used to separate rows in the data.
    /// </summary>
    public string RowTerminator { get; private set; }

    /// <summary>
    /// Gets a value indicating whether a custom object converter is provided.
    /// </summary>
    public bool IsICustomObject { get; internal set; }

    // Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="SetExtractionConfiguration"/> class with a row terminators. The
    /// default field is tab character(\t). 
    /// </summary>
    /// <param name="elementSeperator">The string used to separate rows in the data.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="elementSeperator"/> is null.</exception>
    /// <exception cref="SetsConfigurationException">Thrown if the default field terminator is the same as <paramref name="elementSeperator"/> or if they contain reserved characters.</exception>
    public SetExtractionConfiguration(string elementSeperator)
    {
        string fieldTerminator = "\t";
        VerifyProperties(fieldTerminator, elementSeperator);
        IsICustomObject = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetExtractionConfiguration"/> class with field and row terminators.
    /// </summary>
    /// <param name="fieldTerminator">The string used to separate fields in a record.</param>
    /// <param name="rowTerminator">The string used to separate rows in the data.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fieldTerminator"/> or <paramref name="rowTerminator"/> is null.</exception>
    /// <exception cref="SetsConfigurationException">Thrown if <paramref name="fieldTerminator"/> is the same as <paramref name="rowTerminator"/> or if they contain reserved characters.</exception>
    public SetExtractionConfiguration(string fieldTerminator, string rowTerminator)
    {
        VerifyProperties(fieldTerminator, rowTerminator);
        IsICustomObject = false;
    }

    /// <summary>
    /// Verifies that the field and row terminators are not null, not the same, and do not contain reserved characters.
    /// </summary>
    /// <param name="_fieldTerminator">The field terminator to check.</param>
    /// <param name="_rowTerminator">The row terminator to check.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="_fieldTerminator"/> or <paramref name="_rowTerminator"/> is null.</exception>
    /// <exception cref="SetsConfigurationException">Thrown if <paramref name="_fieldTerminator"/> is the same as <paramref name="_rowTerminator"/> or if they contain reserved characters.</exception>
    private void VerifyProperties(string _fieldTerminator, string _rowTerminator)
    {
        // Check if they are not null
        ArgumentNullException.ThrowIfNull(_fieldTerminator, nameof(_fieldTerminator));
        ArgumentNullException.ThrowIfNull(_rowTerminator, nameof(_rowTerminator));

        // Check if they are not the same
        if (_fieldTerminator == _rowTerminator)
        {
            //throw new ArgumentException("Terminators cannot be the same.");
            throw new SetsConfigurationException("Terminators cannot be the same.");
        }

        // Check if terminators contain any reserved characters
        string det = $"The character {string.Join("", RESERVED_CHARACTERS)} cannot be used in any of the terminators.";
        for (int i = 0; i < RESERVED_CHARACTERS.Length; i++)
        {
            if (_fieldTerminator.Contains(RESERVED_CHARACTERS[i]))
            {
                //Get the index of the invalid character
                int indexOfInvalidCharacter = _fieldTerminator.IndexOf(RESERVED_CHARACTERS[i]);
                det += $"\nThe {nameof(_fieldTerminator)} contains a reserved character at index {indexOfInvalidCharacter}.";

                //Throw the exception here
                throw new SetsConfigurationException("Cannot use reserved characters.", det);
            }
            if (_rowTerminator.Contains(RESERVED_CHARACTERS[i]))
            {
                //Get the index of the invalid character
                int indexOfInvalidCharacter = _rowTerminator.IndexOf(RESERVED_CHARACTERS[i]);
                det += $"\nThe {nameof(_rowTerminator)} contains a reserved character at index {indexOfInvalidCharacter}.";

                //Throw the exception here
                throw new SetsConfigurationException("Cannot use reserved characters.", det);
            }
        }

        // Assign terminators
        FieldTerminator = _fieldTerminator;
        RowTerminator = _rowTerminator;
    }
}//class