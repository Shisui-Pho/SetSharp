using SetsLibrary.Collections;
using SetsLibrary.Utility;

namespace SetsLibrary;

/// <summary>
/// Represents a tree structure for sets, allowing the organization and manipulation of set elements.
/// </summary>
/// <typeparam name="T">The type of elements in the set tree, which must be comparable.</typeparam>
public class SetTree<T> : ISetTree<T> where T : IComparable<T>
{
    #region Data-fields
    /// <summary>
    /// Collection of root elements.
    /// </summary>
    protected internal readonly ISortedElements<T> _elements;
    /// <summary>
    /// Collection of subsets in the root of the tree.
    /// </summary>
    protected internal readonly ISortedSubSets<T> _subSets;
    #endregion Data-fields

    #region Properties
    /// <summary>
    /// Gets the string representation of the root elements in the tree.
    /// </summary>
    public string RootElements
    {
        get
        {
            //Return something
            string elements = string.Join(ExtractionSettings.RowTerminator, _elements);
            return elements;
        }
    }

    /// <summary>
    /// Gets the total number of elements and subsets in the tree.
    /// </summary>
    public int Count => _elements.Count + _subSets.Count;

    /// <summary>
    /// Gets the configuration used for extracting elements from the set tree.
    /// </summary>
    public SetExtractionConfiguration<T> ExtractionSettings { get; private set; }

    /// <summary>
    /// Gets the number of root elements in the tree.
    /// </summary>
    public int CountRootElements => _elements.Count;

    /// <summary>
    /// Gets the number of subsets in the tree.
    /// </summary>
    public int CountSubsets => _subSets.Count;
    #endregion Properties

    #region Constructors

    internal SetTree()
    {
        //Create new instances of collection
        this._elements = new SortedElements<T>();
        this._subSets = new SortedSubSets<T>();
    }//ctor default
    /// <summary>
    /// Initializes a new instance of the <see cref="SetTree{T}"/> class with the specified extraction settings.
    /// </summary>
    /// <param name="extractionSettings">The settings used for extracting elements from the set tree.</param>
    public SetTree(SetExtractionConfiguration<T> extractionSettings)
        : this()
    {
        //Check if settings are null
        ArgumentNullException.ThrowIfNull(extractionSettings, nameof(extractionSettings));
        this.ExtractionSettings = extractionSettings;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTree{T}"/> class with the specified extraction settings and elements.
    /// </summary>
    /// <param name="extractionSettings">The settings used for extracting elements from the set tree.</param>
    /// <param name="elements">A collection of elements to add to the set tree.</param>
    public SetTree(SetExtractionConfiguration<T> extractionSettings, IEnumerable<T> elements)
        : this(extractionSettings)
    {
        //Add elements as ranges
        this.AddRange(elements);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTree{T}"/> class with the specified extraction settings and subsets.
    /// </summary>
    /// <param name="extractionSettings">The settings used for extracting elements from the set tree.</param>
    /// <param name="subsets">A collection of subsets to add to the set tree.</param>
    public SetTree(SetExtractionConfiguration<T> extractionSettings, IEnumerable<ISetTree<T>> subsets)
        : this(extractionSettings)
    {
        //Add elements as ranges
        this.AddRange(subsets);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTree{T}"/> class with the specified extraction settings, elements, and subsets.
    /// </summary>
    /// <param name="extractionSettings">The settings used for extracting elements from the set tree.</param>
    /// <param name="elements">A collection of elements to add to the set tree.</param>
    /// <param name="subsets">A collection of subsets to add to the set tree.</param>
    public SetTree(SetExtractionConfiguration<T> extractionSettings, IEnumerable<T> elements, IEnumerable<ISetTree<T>> subsets)
        : this(extractionSettings)
    {
        //Add ranges
        this.AddRange(elements);
        this.AddRange(subsets);
    }
    #endregion Constructors

    #region Public methods

    /// <summary>
    /// Adds a single element to the set tree.
    /// </summary>
    /// <param name="element">The element to add.</param>
    public void AddElement(T element)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        //Add element if it is unique
        _elements.AddIfUnique(element);
    }

    /// <summary>
    /// Adds a collection of elements to the set tree.
    /// </summary>
    /// <param name="elements">A collection of elements to add.</param>
    public void AddRange(IEnumerable<T> elements)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(elements, nameof(elements));

        //Add them one by one
        foreach (T element in elements)
            this.AddElement(element);
    }

    /// <summary>
    /// Adds a subset tree to the set tree.
    /// </summary>
    /// <param name="tree">The subset tree to add.</param>
    public void AddSubSetTree(ISetTree<T> tree)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(tree, nameof(tree));

        //Add if unique
        _subSets.AddIfUnique(tree);
    }

    /// <summary>
    /// Adds a collection of subset trees to the set tree.
    /// </summary>
    /// <param name="subsets">A collection of subset trees to add.</param>
    public void AddRange(IEnumerable<ISetTree<T>> subsets)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(subsets, nameof(subsets));

        //Add subsets one by one
        foreach (ISetTree<T> subset in subsets)
            this.AddSubSetTree(subset);
    }

    /// <summary>
    /// Gets the index of the specified element in the set tree.
    /// </summary>
    /// <param name="element">The element to find.</param>
    /// <returns>The index of the element, or -1 if not found.</returns>
    public int IndexOf(T element)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        //Return index of element
        return _elements.IndexOf(element);
    }

    /// <summary>
    /// Gets the index of the specified subset tree in the set tree.
    /// </summary>
    /// <param name="subset">The subset tree to find.</param>
    /// <returns>The index of the subset tree, or -1 if not found.</returns>
    public int IndexOf(ISetTree<T> subset)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(subset, nameof(subset));

        //Get the index
        int index = _subSets.IndexOf(subset);

        //IF not found
        if (index == -1)
            return index;

        //Need to scale index to match the index in the set
        return _elements.Count + index;
    }

    /// <summary>
    /// Removes an element from the set tree.
    /// </summary>
    /// <param name="element">The element to remove.</param>
    /// <returns>True if the element was removed, false if it was not found.</returns>
    public bool RemoveElement(T element)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        //Find index of element
        int index = this._elements.IndexOf(element);

        if (index == -1)
            return false;

        //Remove element 
        this._elements.RemoveAt(index);
        return true;
    }

    /// <summary>
    /// Removes a subset tree from the set tree.
    /// </summary>
    /// <param name="element">The subset tree to remove.</param>
    /// <returns>True if the subset tree was removed, false if it was not found.</returns>
    public bool RemoveElement(ISetTree<T> element)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        //Find index of element
        int index = this._subSets.IndexOf(element);

        if (index == -1)
            return false;

        //Remove element
        this._subSets.RemoveAt(index);
        return true;
    }

    /// <summary>
    /// Compares this set tree with another set tree to determine their relative ordering.
    /// </summary>
    /// <param name="other">The other set tree to compare with.</param>
    /// <returns>-1 if this tree comes before the other, 1 if it comes after, or 0 if they are equal.</returns>
    public int CompareTo(ISetTree<T>? other)
    {
        //Check for nulls first
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        //First Check for cardinality
        //How compare works 
        //If this comes before other : -1
        //If this comes after  other : 1
        //If they are the same       : 0

        //First check cardinality
        if (this.Count > other.Count)
            return 1;//other comes before this

        if (this.Count < other.Count)
            return -1;//this comes before other

        //If cardinalities are the same it can mean two things
        //-1. They are the same
        //-2. They don't have sane elements

        //First preference is given to one with less root elements
        if (this.CountRootElements < other.CountRootElements)
            return -1;

        if (this.CountRootElements > other.CountRootElements)
            return 1;

        int elementIndex = 0;
        //Here they have the same number of root elements and subsets
        //-Enumerate through all the root elements of other and compare them with the elements of this
        foreach (T element in other.GetRootElementsEnumerator())
        {
            //Compare correspond elements
            int comparer = this._elements[elementIndex].CompareTo(element);

            //if not the same return the result
            if (comparer != 0)
                return comparer;

            //increment the index
            elementIndex++;
        }

        elementIndex = 0;
        //Here it means that the root elements are the same
        //-We need to check each subset
        foreach (ISetTree<T> tree in other.GetSubsetsEnumerator())
        {
            //Do it recursively
            int comparer = this._subSets[elementIndex].CompareTo(tree);

            //if not the same return the result
            if (comparer != 0)
                return comparer;

            //increment the index
            elementIndex++;
        }

        //It means that the trees are the same
        return 0;
    }

    /// <summary>
    /// Returns a string representation of the set tree.
    /// </summary>
    /// <returns>A string representation of the set tree.</returns>
    public override string ToString()
    {
        return SetTreeUtility<T>.ToElementString(this);
    }//ToString

    /// <summary>
    /// Gets an enumerator for the root elements of the set tree.
    /// </summary>
    /// <returns>An enumerator for the root elements.</returns>
    public IEnumerable<T> GetRootElementsEnumerator()
    {
        return this._elements;
    }

    /// <summary>
    /// Gets an enumerator for the subset trees of the set tree.
    /// </summary>
    /// <returns>An enumerator for the subset trees.</returns>
    public IEnumerable<ISetTree<T>> GetSubsetsEnumerator()
    {
        return this._subSets;
    }
    #endregion Public methods
}//class