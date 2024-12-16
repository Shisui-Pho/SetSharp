/*
 * File: BaseSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
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
using SetsLibrary.Utility;
namespace SetsLibrary;

/// <summary>
/// An abstract base class for structured sets, providing a foundation for specific set implementations.
/// </summary>
/// <typeparam name="T">The type of elements in the set, which must be comparable.</typeparam>
public abstract class BaseSet<T> : IStructuredSet<T>
    where T : IComparable<T>
{
    //Data fields
    private readonly IIndexedSetTree<T> _treeWrapper;

    #region Properties
    /// <summary>
    /// Gets the original string representation of the set.
    /// </summary>
    public string OriginalExpression {get; private set; }

    /// <summary>
    /// Gets the cardinality (number of elements) of the evaluated set.
    /// </summary>
    public int Cardinality => _treeWrapper.Count;

    /// <summary>
    /// Gets the current settings of the set extractor.
    /// </summary>
    public SetExtractionConfiguration<T> ExtractionConfiguration {get; private set; }
    #endregion Properties

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSet{T}"/> class with the specified extraction configuration.
    /// This constructor sets the extraction configuration but does not evaluate or set the expression.
    /// The set tree will be created using the provided configuration.
    /// </summary>
    /// <param name="extractionConfiguration">The configuration to be used for extracting set elements and subsets.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="extractionConfiguration"/> is null.</exception>
    public BaseSet(SetExtractionConfiguration<T> extractionConfiguration)
    {
        // Ensure the extraction configuration is not null, as it is required for proper set extraction
        ArgumentNullException.ThrowIfNull(extractionConfiguration, nameof(extractionConfiguration));

        // Set the extraction configuration
        this.ExtractionConfiguration = extractionConfiguration;

        // Create a new instance of SetTreeWrapper using the provided configuration
        _treeWrapper = new SetTreeWrapper<T>(extractionConfiguration);
    }// Default constructor (uses SetExtractionConfiguration)


    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSet{T}"/> class with the specified string expression 
    /// and extraction configuration. This constructor evaluates the set tree based on the given expression
    /// and sets the original expression.
    /// </summary>
    /// <param name="expression">The string representation of the set expression.</param>
    /// <param name="config">The configuration to be used for extracting set elements and subsets.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="config"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="expression"/> is null or whitespace.</exception>
    public BaseSet(string expression, SetExtractionConfiguration<T> config)
    {
        // Ensure the configuration is not null, as it is needed to extract elements and subsets from the expression
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        // Ensure the expression is valid (non-null and non-whitespace)
        ArgumentException.ThrowIfNullOrWhiteSpace(expression, nameof(expression));

        //Assign the configurations
        this.ExtractionConfiguration = config;

        // Extract the set tree from the provided expression and configuration
        _treeWrapper = new SetTreeWrapper<T>(Extractions(expression));

        // Assign the original expression after extraction
        OriginalExpression = expression;
    }// Constructor 1 (accepts expression and SetExtractionConfiguration)


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
        this.ExtractionConfiguration = indexedSetTree.ExtractionSettings;
        this.OriginalExpression = indexedSetTree.ToString(); // Set the original expression from the tree's ToString method
    }// Constructor 2 (accepts injected IIndexedSetTree)

    private ISetTree<T> Extractions(string expression)
    {
        //Check if string is null or empty or a whitespace
        ArgumentException.ThrowIfNullOrWhiteSpace(expression, nameof(expression));
        ArgumentException.ThrowIfNullOrEmpty(expression, nameof(expression));

        //Evaluate braces
        if (!BraceEvaluator.AreBracesCorrect(expression))
        {
            //If braaces missmatch
            throw new ArgumentException("Braces of the expression are not matching");
        }

        //Extract the set tree
        return SetTreeExtractor<T>.Extract(expression, ExtractionConfiguration);
    }//Extractions
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

        //Add element to the tree
        _treeWrapper.AddElement(element);
    }//AddElement

    /// <summary>
    /// Adds a new tree as an element in the current set. If the tree already exists, it will not be added.
    /// </summary>
    /// <param name="tree">The tree to be added.</param>
    public void AddElement(ISetTree<T> tree)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(tree,nameof(tree));

        //Add to the tree
        _treeWrapper.AddSubSetTree(tree);
    }//AddElement

    /// <summary>
    /// Adds a new subset to the current set from a string representation of the subset.
    /// </summary>
    /// <param name="subset">The string representation of the subset.</param>
    public void AddSubsetAsString(string subset)
    {
        //Check if null
        ArgumentNullException.ThrowIfNull(subset, nameof(subset));

        //Extract the tree
        var tree = Extractions(subset);

        //Add the tree
        this.AddElement(tree);
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
        //Check for nullss
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        return _treeWrapper.IndexOf(element) >= 0;
    }//Contains

    /// <summary>
    /// Checks if the specified tree exists in the set.
    /// </summary>
    /// <param name="tree">The tree to check for.</param>
    /// <returns>True if the tree exists; otherwise, false.</returns>
    public bool Contains(ISetTree<T> tree)
    {
        //Check for nullss
        ArgumentNullException.ThrowIfNull(tree, nameof(tree));

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

        return setB.Contains(this._treeWrapper);
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

        if(IsSameSet(setB))
        {
            //Subset and a properset
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

        //First loop through the root elements
        var setBRootElements = setB.EnumerateRootElements().ToHashSet();

        bool areRootElementsContained = Contains(setBRootElements, 
                                        _treeWrapper.CountRootElements, 
                                        _treeWrapper.GetRootElementByIndex);

        //Check if all current root elements are contained in setB
        if (!areRootElementsContained)
            return false;

        //Now check for the Subsets
        var setBSubsets = setB.EnumerateSubsets().ToHashSet();

        return Contains(setBSubsets,
                        _treeWrapper.CountSubsets,
                        _treeWrapper.GetSubsetByIndex); ;
    }//IsProperSet
    private bool Contains<TElement>(HashSet<TElement> elements, int count, Func<int, TElement> funcElement)
    {
        for (int i = 0; i < count; i++)
        {
            if (!elements.Contains(funcElement(i)))
                return false;
        }

        return true;
    }//Contains
    private bool IsSameSet(IStructuredSet<T> setB)
    {
        ArgumentNullException.ThrowIfNull(setB, nameof (setB));

        //Check cardinalities
        if (this.Cardinality != setB.Cardinality)
            return false;
        
        BaseSet<T>? baseSetB = setB as BaseSet<T>;

        if(baseSetB != null)
            return baseSetB._treeWrapper.CompareTo(this._treeWrapper) == 0; //check if same set

        return CompareSetsNotBaseSet(setB);
    }//IsSameSet
    private bool CompareSetsNotBaseSet(IStructuredSet<T> setB)
    {
        //Compare the root elements
        bool areRootElementsEqual = setB.EnumerateRootElements().SequenceEqual(EnumerateRootElements());

        if(!areRootElementsEqual)
            return false;

        //Compare the subsets
        bool areSubSetsEqual = setB.EnumerateSubsets().SequenceEqual(EnumerateSubsets());

        if(!areSubSetsEqual) return false;

        //They are the same
        return true;
    }//CompareSetsNotBaseSet


    /// <summary>
    /// Removes the specified tree from the current set.
    /// </summary>
    /// <param name="tree">The tree to remove.</param>
    /// <returns>True if the tree was found and removed; otherwise, false.</returns>
    public bool RemoveElement(ISetTree<T> tree)
    {
        //Check for null
        ArgumentNullException.ThrowIfNull (tree, nameof (tree));

        //Remove element
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
        return _treeWrapper.GetRootElementsEnumarator();
    }//EnumerateRootElements

    /// <summary>
    /// Enumerates and returns all subsets in the current set.
    /// This method has not been implemented yet, and calling it will throw a <see cref="NotImplementedException"/>.
    /// </summary>
    /// <returns>An enumerable collection of subsets in the set.</returns>
    public IEnumerable<ISetTree<T>> EnumerateSubsets()
    {
        return _treeWrapper.GetSubsetsEnumarator();
    }//EnumerateSubsets

    /// <summary>
    /// Merges the current set with another set and returns the resulting set.
    /// </summary>
    /// <param name="set">The set to merge with.</param>
    /// <returns>The merged set.</returns>
    public IStructuredSet<T> MergeWith(IStructuredSet<T> set)
    {
        //Check for argument null exceptions
        ArgumentNullException.ThrowIfNull(set,  nameof (set));

        //Check if they have elements
        if (this.Cardinality <= 0)
            return set;

        if (set.Cardinality <= 0)
            return this;

        //Get the string representation of the sets
        string setA = _treeWrapper.ToString();
        string? setB = set.BuildStringRepresentation();

        //Merg strings
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

        //Start with the root elemets

        var newSet = BuildNewSet();
        //Loop through the root elements and subsets of the current instance
        //-If the new set has the same elements as "setB" then all remaining elements
        //-the current instace need to be added in the newSet
        for (int i = 0;i < _treeWrapper.CountRootElements; i++)
        {
            //Check if setB contains the current element in it set
            var elem = _treeWrapper.GetRootElementByIndex(i);
            if (!setB.Contains(elem))
                newSet.AddElement(elem);
        }//end for

        //Loop througth the subsets of this instance
        for (int i = 0; i < _treeWrapper.CountSubsets; i++)
        {
            //Check if setB contains the current subste in it set
            var sub = _treeWrapper.GetSubsetByIndex(i);
            if (!setB.Contains(sub))
                newSet.AddElement(sub);
        }//end for 

        //return the new set
        return newSet;
    }//Without
    #endregion Implemented methods

    #region Abstract methods
    /// <summary>
    /// Builds and returns a new set based on the provided string representation.
    /// This method is abstract and must be implemented by derived classes to handle the specific logic 
    /// for creating a set from a string.
    /// </summary>
    /// <param name="setString">The string representation of the set to be created.</param>
    /// <returns>A new instance of a structured set.</returns>
    protected abstract IStructuredSet<T> BuildNewSet(string setString);

    /// <summary>
    /// Builds and returns a new, empty set.
    /// This method is abstract and must be implemented by derived classes to handle the specific logic 
    /// for creating an empty set.
    /// </summary>
    /// <returns>A new, empty instance of a structured set.</returns>
    protected abstract IStructuredSet<T> BuildNewSet();
    #endregion ABSTRACT METHODS
}//class
//namespace