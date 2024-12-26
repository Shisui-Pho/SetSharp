/*
 * File: StringLiteralSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file contains the implementation of the StringLiteralSet class, 
 * which represents a set specifically designed to hold string elements. 
 * The class extends the abstract BaseSet class, which provides the common 
 * functionality for structured sets. This class allows for operations 
 * such as adding, removing, checking for elements, and merging with other sets.
 * 
 * Key Features:
 * - A specialized set implementation for handling string elements.
 * - Provides operations such as adding, removing, and checking for elements.
 * - Supports set merging and subset checking.
 */

namespace SetsLibrary;
/// <summary>
/// Represents a string literal set that can contain any string elements.
/// </summary>
public class StringLiteralSet : BaseSet<string>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="StringLiteralSet"/> class with the specified extraction configuration.
    /// </summary>
    /// <param name="extractionConfiguration">The configuration to be used for extracting set elements and subsets.</param>
    public StringLiteralSet(SetExtractionConfiguration<string> extractionConfiguration) : base(extractionConfiguration)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringLiteralSet"/> class with the specified string expression and extraction configuration.
    /// </summary>
    /// <param name="expression">The string representation of the set expression.</param>
    /// <param name="config">The configuration to be used for extracting set elements and subsets.</param>
    public StringLiteralSet(string expression, SetExtractionConfiguration<string> config) : base(expression, config)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="StringLiteralSet"/> class by injecting an existing instance of IIndexedSetTree.
    /// This constructor allows for complete control over the set tree, including the expression and extraction settings.
    /// </summary>
    /// <param name="indexedSetTree">An existing instance of an IIndexedSetTree that provides both the set elements 
    /// and configuration for extraction.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexedSetTree"/> is null.</exception>
    public StringLiteralSet(IIndexedSetTree<string> indexedSetTree) : base(indexedSetTree) { }
    #endregion Constructors

    #region Overrides

    /// <summary>
    /// Builds and returns a new <see cref="StringLiteralSet"/> based on the provided string representation of the set.
    /// </summary>
    /// <param name="setString">The string representation of the set to be created.</param>
    /// <returns>A new instance of <see cref="StringLiteralSet"/>.</returns>
    public override IStructuredSet<string> BuildNewSet(string setString)
    {
        return new StringLiteralSet(setString, this.ExtractionConfiguration);
    } // BuildNewSet

    /// <summary>
    /// Builds and returns a new, empty <see cref="StringLiteralSet"/>.
    /// </summary>
    /// <returns>A new, empty instance of <see cref="StringLiteralSet"/>.</returns>
    public override IStructuredSet<string> BuildNewSet()
    {
        return new StringLiteralSet(this.ExtractionConfiguration);
    } // BuildNewSet

    #endregion Overrides
} // class
// namespace
