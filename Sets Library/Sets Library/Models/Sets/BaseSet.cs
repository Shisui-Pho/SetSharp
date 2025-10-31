/*
 * File: BaseSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 29 March 2025
 * 
 * Description:
 * This file contains the implementation of the abstract BaseSet class, which provides 
 * a foundation for structured set implementations. It defines common operations and properties 
 * for handling sets, including adding, removing, checking for elements, and merging sets. 
 * The class uses generics to support sets with any comparable type of elements.
 * 
 * Key Features:
 * - Provides an abstract class with methods to manage sets and nested subsets.
 * - Supports operations like adding, removing, and checking for elements and subsets.
 * - Includes functionality for merging sets, evaluating subset relationships, and cardinality.
 * - Allows for flexible set operations with a generic type parameter, enabling usage with different element types.
 * - Enables parsing of set expressions into structured sets using a tree-like model.
 * 
 * Version History:
 * 1.0 - Initial implementation
 * 
 * Notes:
 * This class is intended for use as a base class for specific set implementations that may add additional 
 * functionality specific to the type of set being created.
 */

using SetSharp.SetOperations;
using SetSharp.Utility;
using System.Diagnostics.CodeAnalysis;

namespace SetSharp;

/// <summary>
/// An abstract base class for structured sets, providing a foundation for specific set implementations.
/// </summary>
/// <typeparam name="T">The type of elements in the set, which must be comparable.</typeparam>
public abstract class BaseSet<T> : IStructuredSet<T>
    where T : IComparable<T>
{
    #region Embedded class

    /// <summary>
    /// Provides an equality comparison mechanism for <see cref="IStructuredSet{T}"/> instances.
    /// </summary>
    private class StructuredSetEqualityComparer : IEqualityComparer<IStructuredSet<T>>
    {
        /// <summary>
        /// Compares two <see cref="IStructuredSet{T}"/> instances for equality.
        /// </summary>
        /// <param name="x">The first <see cref="IStructuredSet{T}"/> instance to compare.</param>
        /// <param name="y">The second <see cref="IStructuredSet{T}"/> instance to compare.</param>
        /// <returns>
        ///   <see langword="true"/> if the two <see cref="IStructuredSet{T}"/> instances are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(IStructuredSet<T>? x, IStructuredSet<T>? y)
        {
            // Check for nulls first
            // If both x and y are null, they are considered equal
            if (x is null && y is null)
            {
                return true;
            }

            // If one of them is null, they are not equal
            if (x is null || y is null)
            {
                return false;
            }

            // Check if x and y refer to the same object in memory (same reference)
            if (ReferenceEquals(x, y))
            {
                return true; // They are the same object, so they are equal
            }

            // If none of the above, compare their structure to check equality
            return x.SetStructuresEqual(y); // Assumes that IStructuredSet<T> has a method SetStructuresEqual for deep comparison
        }

        /// <summary>
        /// Gets the hash code for the specified <see cref="IStructuredSet{T}"/> instance.
        /// </summary>
        /// <param name="obj">The <see cref="IStructuredSet{T}"/> instance for which the hash code is to be generated.</param>
        /// <returns>The hash code of the <see cref="IStructuredSet{T}"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="obj"/> is <see langword="null"/>.</exception>
        public int GetHashCode([DisallowNull] IStructuredSet<T> obj)
        {
            return obj.GetHashCode(); // Return the hash code of the set itself
        }
    }

    #endregion Embedded class
    // Data fields
    private readonly IIndexedSetTree<T> _treeWrapper;

    #region Properties

    /// <summary>
    /// Gets the original string representation of the set.
    /// </summary>
    public string OriginalExpression { get; private set; }

    /// <summary>
    /// Gets the cardinality (number of elements) of the evaluated set.
    /// </summary>
    public int Cardinality => _treeWrapper.Count;

    /// <summary>
    /// Gets the current settings of the set extractor.
    /// </summary>
    public SetsConfigurations ExtractionConfiguration { get; private set; }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSet{T}"/> class with the specified extraction configuration.
    /// This constructor sets the extraction configuration but does not evaluate or set the expression.
    /// The set tree will be created using the provided configuration.
    /// </summary>
    /// <param name="extractionConfiguration">The configuration to be used for extracting set elements and subsets.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="extractionConfiguration"/> is null.</exception>
    public BaseSet(SetsConfigurations extractionConfiguration)
    {
        // Ensure the extraction configuration is not null, as it is required for proper set extraction
        ArgumentNullException.ThrowIfNull(extractionConfiguration, nameof(extractionConfiguration));

        // Set the extraction configuration
        this.ExtractionConfiguration = ModifyConfigurations(extractionConfiguration);
        // Create a new instance of SetTreeWithIndexes using the provided configuration
        _treeWrapper = new SetTreeWithIndexes<T>(extractionConfiguration);
    } // Default constructor (uses SetsConfigurations)

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSet{T}"/> class with the specified string expression 
    /// and extraction configuration. This constructor evaluates the set tree based on the given expression
    /// and sets the original expression.
    /// </summary>
    /// <param name="expression">The string representation of the set expression.</param>
    /// <param name="config">The configuration to be used for extracting set elements and subsets.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="config"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="expression"/> is null or whitespace.</exception>
    public BaseSet(string expression, SetsConfigurations config)
    {
        // Ensure the configuration is not null, as it is needed to extract elements and subsets from the expression
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        //AddIfDuplicate braces if needed
        expression = AddBracesIfNeeded(expression, config);

        // Ensure the expression is valid (non-null and non-whitespace)
        ArgumentException.ThrowIfNullOrWhiteSpace(expression, nameof(expression));

        // Assign the configurations
        this.ExtractionConfiguration = ModifyConfigurations(config);
        // BuildSetTree the set tree from the provided expression and configuration

        _treeWrapper = new SetTreeWithIndexes<T>(Extractions(expression));

        // Assign the original expression after extraction
        OriginalExpression = expression;
    } // Constructor 1 (accepts expression and SetsConfigurations)

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSet{T}"/> class by injecting an existing instance of IIndexedSetTree.
    /// This constructor allows for complete control over the set tree, including the expression and extraction settings.
    /// </summary>
    /// <param name="indexedSetTree">An existing instance of an IIndexedSetTree that provides both the set elements 
    /// and configuration for extraction.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexedSetTree"/> is null.</exception>
    public BaseSet(IIndexedSetTree<T> indexedSetTree)
    {
        // Ensure the provided indexed set tree is not null, as we need it to initialize the set
        ArgumentNullException.ThrowIfNull(indexedSetTree, nameof(indexedSetTree));

        // Assign the injected indexed set tree to the _treeWrapper field
        this._treeWrapper = indexedSetTree;

        // Set the extraction configuration and original expression based on the injected tree
        this.ExtractionConfiguration = ModifyConfigurations(indexedSetTree.ExtractionSettings);
        this.OriginalExpression = indexedSetTree.ToString(); // Set the original expression from the tree's ToString method
    } // Constructor 2 (accepts injected IIndexedSetTree)

    /// <summary>
    /// Extracts the set tree from the provided expression, validates brace structure, 
    /// and returns the tree structure for the set.
    /// </summary>
    /// <param name="expression">The string representation of the set expression.</param>
    /// <returns>An instance of <see cref="ISetTree{T}"/> representing the extracted set tree.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="expression"/> is null, empty, or whitespace.</exception>
    /// <exception cref="MissingBraceException">Thrown if the braces in the set expression are mismatched or invalid.</exception>
    /// <exception cref="SetsException">Thrown if there was a problem when extracting the set tree.</exception>
    private ISetTree<T> Extractions(string expression)
    {
        //Check if the string is null, empty, or contains only whitespace
        ArgumentException.ThrowIfNullOrWhiteSpace(expression, nameof(expression));

        //BuildSetTree and return the set tree from the expression using the extraction configuration
        try
        {
            return SetTreeBuilder<T>.BuildSetTree(expression, ExtractionConfiguration);
        }
        catch (SetsException ex)
        {
            throw new SetsException("Failed to extract set string.", "", ex);
        }
    } // Extractions
    private string AddBracesIfNeeded(string expression, SetsConfigurations config)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof (expression));
        //Check if we need to add braces or not
        if(config.AutomaticallyAddBrace)
            return "{" + expression + "}";
        
        //Check if the the braces match
        if(!expression.StartsWith('{') || !expression.EndsWith('}'))
        {
            throw new MissingBraceException("Sets cannot be parsed, you're missing an oppenig or clossing or both braces.",
                                            "If you need to automatically add them, you can pass the parameter \"addBraces = true\" in the configurations.");
        }
        return expression;
    }
    /// <summary>
    /// Checks if the current instance is a custom object set.
    /// </summary>
    /// <returns>True if it is a custom object set.</returns>
    protected virtual SetsConfigurations ModifyConfigurations(SetsConfigurations config)
    {
        //If there are no overrides, return the configurations
        return config;
    }//ModifyConfigurations
    #endregion Constructors

    #region Implemented Methods
    /// <summary>
    /// Adds a new element to the set. If the element already exists, it will not be added.
    /// </summary>
    /// <param name="element">The element to be added.</param>
    public void AddElement(T element)
    {
        //Check if element is null or not
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        //AddIfDuplicate element to the tree
        _treeWrapper.AddElement(element);
    }//AddElement

    /// <summary>
    /// Adds a new tree as an element in the current set. If the tree already exists, it will not be added.
    /// </summary>
    /// <param name="subset">The tree to be added.</param>
    public void AddElement(IStructuredSet<T> subset)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(subset, nameof(subset));

        //Try get the underlying set
        var _set = TryGetUnderlyingBaseSet(subset);

        if (_set is not null)
            _treeWrapper.AddElement(_set._treeWrapper);
        else
        {
            if(subset.Cardinality == 0)
            {
                //add an empty set
                _treeWrapper.AddElement(Extractions("{}"));
            }
            else
            {
                //An in-efficient way
                var tree = Extractions(subset.BuildStringRepresentation());
                _treeWrapper.AddElement(tree);
            }
        }

    }//AddElement
    private BaseSet<T>? TryGetUnderlyingBaseSet(IStructuredSet<T> set)
    {
        return set as BaseSet<T>;
    }
    /// <summary>
    /// Adds a new subset to the current set from a string representation of the subset.
    /// </summary>
    /// <param name="subset">The string representation of the subset.</param>
    public void AddSubsetAsString(string subset)
    {
        //Check if null
        ArgumentNullException.ThrowIfNull(subset, nameof(subset));

        //BuildSetTree the tree
        var tree = Extractions(subset);

        var indexedTree = BuildNewSet(new SetTreeWithIndexes<T>(tree));
        //AddIfDuplicate the tree
        this.AddElement(indexedTree);
    }//AddSubsetAsString

    /// <summary>
    /// Clears all elements in the current set.
    /// </summary>
    public void Clear()
    {
        _treeWrapper.Clear();
    }//Clear

    /// <summary>
    /// Checks if the specified element exists in the set.
    /// </summary>
    /// <param name="element">The element to check for.</param>
    /// <returns>True if the element exists; otherwise, false.</returns>
    public bool Contains(T element)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        return _treeWrapper.IndexOf(element) >= 0;
    }//Contains

    /// <summary>
    /// Checks if the specified tree exists in the set.
    /// </summary>
    /// <param name="subSet">The tree to check for.</param>
    /// <returns>True if the tree exists; otherwise, false.</returns>
    public bool Contains(IStructuredSet<T> subSet)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(subSet, nameof(subSet));

        //Try get the underlying base structure

        var _set = TryGetUnderlyingBaseSet(subSet);

        if (_set is not null)
            return _treeWrapper.IndexOf(_set._treeWrapper) >= 0;


        //An in-efficient way
        var tree = Extractions(subSet.BuildStringRepresentation());

        return _treeWrapper.IndexOf(tree) >= 0;
    }//Contains

    /// <summary>
    /// Checks if the current set is an element of the specified set.
    /// </summary>
    /// <param name="setB">The set to check against.</param>
    /// <returns>True if the current set is an element of setB; otherwise, false.</returns>
    public bool IsElementOf(IStructuredSet<T> setB)
    {
        //Check for null
        ArgumentNullException.ThrowIfNull(setB, nameof(setB));

        return setB.Contains(this);
    }//IsElementOf

    /// <summary>
    /// Determines if the current set is a subset of the specified set.
    /// </summary>
    /// <param name="setB">The set to check against.</param>
    /// <param name="type">The type of set relationship.</param>
    /// <returns>True if the current set is a subset; otherwise, false.</returns>
    public bool IsSubSetOf(IStructuredSet<T> setB, out SetResultType type)
    {
        type = SetResultType.NotASubSet;

        //First check cardinalities
        if (this.Cardinality > setB.Cardinality)
            return false;

        if (IsSameSet(setB))
        {
            //Subset and a proper set
            type = SetResultType.Same_Set & SetResultType.SubSet;
            return true;
        }//end if

        //Here it's either they are the same or 
        if (!IsSubSetOf(setB))
            return false;

        type = SetResultType.ProperSet;
        return true;
    }//IsSubSetOf
    private bool IsSubSetOf(IStructuredSet<T> setB)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(setB, nameof(setB));

        //Check if setB is of type BaseSet<T>
        BaseSet<T>? setBBase = setB as BaseSet<T>;

        bool isSubset = false;

        if (setBBase is not null)
        {
            //Here do a sime contains expression
            //-IIndexedTree is ISetTree
            isSubset = this._treeWrapper.IndexOf((ISetTree<T>)(setBBase._treeWrapper)) >= 0;
        }
        else
        {
            isSubset = IsSubsetOfNoneBaseSet(setB);
        }

        return isSubset;
    }//IsProperSet
    private bool IsSubsetOfNoneBaseSet(IStructuredSet<T> setB)
    {
        //Perform loops
        foreach (var rootElement in setB.EnumerateRootElements())
        {
            if (!this.Contains(rootElement))
                return false;
        }

        //Here loop through the subsets
        foreach (var element in setB.EnumerateSubsets())
            if (!this.Contains(element))
                return false;

        return true;
    }//IsSubsetOfWithBaseSet
    private bool IsSameSet(IStructuredSet<T> setB)
    {
        ArgumentNullException.ThrowIfNull(setB, nameof(setB));

        //Check cardinalities
        if (this.Cardinality != setB.Cardinality)
            return false;

        BaseSet<T>? baseSetB = setB as BaseSet<T>;

        if (baseSetB != null)
            return baseSetB._treeWrapper.CompareTo(this._treeWrapper) == 0; //check if same set

        return CompareSetsNotBaseSet(setB);
    }//IsSameSet
    private bool CompareSetsNotBaseSet(IStructuredSet<T> setB)
    {
        //Compare the root elements
        bool areRootElementsEqual = setB.EnumerateRootElements().SequenceEqual(EnumerateRootElements());

        if (!areRootElementsEqual)
            return false;

        //Compare the subsets
        bool areSubSetsEqual = setB.EnumerateSubsets().SequenceEqual(EnumerateSubsets(), new StructuredSetEqualityComparer());

        if (!areSubSetsEqual) return false;

        //They are the same
        return true;
    }//CompareSetsNotBaseSet


    /// <summary>
    /// Removes the specified tree from the current set.
    /// </summary>
    /// <param name="subSet">The tree to remove.</param>
    /// <returns>True if the tree was found and removed; otherwise, false.</returns>
    public bool RemoveElement(IStructuredSet<T> subSet)
    {
        //Check for null
        ArgumentNullException.ThrowIfNull(subSet, nameof(subSet));

        //Try get the underlying base set
        var _set = TryGetUnderlyingBaseSet(subSet);

        if (_set is not null)
            return _treeWrapper.RemoveElement(_set._treeWrapper);

        //An in-efficient way
        var tree = Extractions(subSet.BuildStringRepresentation());

        return _treeWrapper.RemoveElement(tree);
    }//RemoveElement

    /// <summary>
    /// Removes the specified element from the current set.
    /// </summary>
    /// <param name="element">The element to remove.</param>
    /// <returns>True if the element was found and removed; otherwise, false.</returns>
    public bool RemoveElement(T element)
    {
        //Check for null
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        //Remove element
        return _treeWrapper.RemoveElement(element);
    }//RemoveElement

    /// <summary>
    /// Builds and returns a string representation of the structured set.
    /// This method has not been implemented yet, and calling it will throw a <see cref="NotImplementedException"/>.
    /// </summary>
    /// <returns>A string representation of the set.</returns>
    public string BuildStringRepresentation()
    {
        return _treeWrapper.ToString();
    }//BuildStringRepresentation

    /// <summary>
    /// Enumerates and returns all root elements in the current set.
    /// This method has not been implemented yet, and calling it will throw a <see cref="NotImplementedException"/>.
    /// </summary>
    /// <returns>An enumerable collection of root elements in the set.</returns>
    public IEnumerable<T> EnumerateRootElements()
    {
        return _treeWrapper.GetRootElementsEnumerator();
    }//EnumerateRootElements

    /// <summary>
    /// Enumerates and returns all subsets in the current set.
    /// </summary>
    /// <returns>An enumerable collection of subsets in the set.</returns>
    public IEnumerable<IStructuredSet<T>> EnumerateSubsets()
    {
        foreach (var item in _treeWrapper.GetSubsetsEnumerator())
        {
            yield return BuildNewSet(new SetTreeWithIndexes<T>(item));
        }
    }//EnumerateSubsets

    /// <summary>
    /// Merges the current set with another set and returns the resulting set.
    /// </summary>
    /// <param name="set">The set to merge with.</param>
    /// <returns>The merged set.</returns>
    public IStructuredSet<T> MergeWith(IStructuredSet<T> set)
    {
        //Check for argument null exceptions
        ArgumentNullException.ThrowIfNull(set, nameof(set));

        //Check if they have elements
        if (this.Cardinality <= 0)
            return set;

        if (set.Cardinality <= 0)
            return this;

        //Get the string representation of the sets
        string setA = BuildStringRepresentation();
        string? setB = set.BuildStringRepresentation();

        //Merge strings
        string _set = setA.Remove(setA.Length - 1) + "," + setB?.Remove(0, 1);
        return BuildNewSet(_set);
    }//MergeWith
    /// <summary>
    /// Removes all elements of the specified set from the current set and returns the resulting set.
    /// </summary>
    /// <param name="setB">The set to remove elements from.</param>
    /// <returns>The resulting set after removal.</returns>
    public IStructuredSet<T> Without(IStructuredSet<T> setB)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(setB, nameof(setB));

        //Check if the first one has elements
        if (this.Cardinality <= 0)
            return setB;

        if (setB.Cardinality <= 0)
            return this;

        //Start with the root elements

        var newSet = BuildNewSet();
        //Loop through the root elements and subsets of the current instance
        //-If the new set has the same elements as "setB" then all remaining elements
        //-the current instance need to be added in the newSet
        for (int i = 0; i < _treeWrapper.CountRootElements; i++)
        {
            //Check if setB contains the current element in it set
            var elem = _treeWrapper.GetRootElementByIndex(i);

            ArgumentNullException.ThrowIfNull(elem, nameof(elem));

            if (!setB.Contains(elem))
                newSet.AddElement(elem);
        }//end for

        //Loop through the subsets of this instance
        for (int i = 0; i < _treeWrapper.CountSubsets; i++)
        {
            //Check if setB contains the current subset in it set
            var sub = _treeWrapper.GetSubsetByIndex(i);

            ArgumentNullException.ThrowIfNull(sub, nameof(sub));

            //Make sub element a set
            var _subSet = BuildNewSet(new SetTreeWithIndexes<T>(sub));
            if (!setB.Contains(_subSet))
                newSet.AddElement(_subSet);
        }//end for 

        //return the new set
        return newSet;
    }//Without
    #endregion Implemented methods

    #region ovverides 
    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Compares the current <see cref="BaseSet{T}"/> instance with another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    ///   <see langword="true"/> if the specified object is equal to the current <see cref="BaseSet{T}"/> instance; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        // Create a new instance of the comparer used to compare IStructuredSet<T> objects
        StructuredSetEqualityComparer comP = new StructuredSetEqualityComparer();

        // Attempt to cast the object into an IStructuredSet<T> (the type we're comparing against)
        var y = obj as IStructuredSet<T>;

        // We accept nulls, so handle the case where obj is null
        // If obj is null, comP.Equals(this, y) will return false
        bool equal = comP.Equals(this, y);

        return equal; // Return the result of comparison
    }


    #endregion ovverides
    #region Abstract methods
    /// <summary>
    /// Builds and returns a new set based on the provided string representation.
    /// This method is abstract and must be implemented by derived classes to handle the specific logic 
    /// for creating a set from a string.
    /// </summary>
    /// <param name="setString">The string representation of the set to be created.</param>
    /// <returns>A new instance of a structured set.</returns>
    public abstract IStructuredSet<T> BuildNewSet(string setString);

    /// <summary>
    /// Builds and returns a new, empty set.
    /// This method is abstract and must be implemented by derived classes to handle the specific logic 
    /// for creating an empty set.
    /// </summary>
    /// <returns>A new, empty instance of a structured set.</returns>
    public abstract IStructuredSet<T> BuildNewSet();
    /// <summary>
    /// Builds and returns a new set based on the provided indexed set tree wrapper.
    /// </summary>
    /// <param name="tree">The indexed tree of the set</param>
    /// <returns>A new instance of structured set</returns>
    protected internal abstract IStructuredSet<T> BuildNewSet(IIndexedSetTree<T> tree);
    #endregion ABSTRACT METHODS
}//class
 //namespace