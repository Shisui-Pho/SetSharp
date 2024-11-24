using SetsLibrary.Interfaces;
using System.Collections;
namespace SetsLibrary.Collections;

public class SortedSubSets<T> : ISortedSubSets<T>
    where T : IComparable<T>
{
    //data-fields
    private List<ISetTree<T>> _subSets;

    //Properties
    public int Count => _subSets.Count;

    //Indexer
    public ISetTree<T> this[int index]
    {
        get
        {
            if(index < 0 ||  index >= Count)
                throw new IndexOutOfRangeException(nameof(index));

            return _subSets[index];
        }//getter
    }//indexer

    //Contructors
    public SortedSubSets()
    {
        //Clear
        Clear();
    }//ctor main
    public SortedSubSets(IEnumerable<ISetTree<T>> subSets)
    {
        //Clear
        Clear();

        AddRange(subSets);
    }//ctor 01

    public void Add(ISetTree<T> value)
    {
        //First check for null
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        //If empty
        if(Count == 0)
        {
            //Just add the element
            this._subSets.Add(value);

            return;
        }//end if

        //Else not empty

        //Find index of insertion
        int indexOfInsertion = FindIndexOfInsertion(value);

        //Insert element
        this._subSets.Insert(indexOfInsertion,value);
    }//Add

    private int FindIndexOfInsertion(ISetTree<T> treeToAdd)
    {
        //Do binary search
        int lowerbound = 0;
        int upperbound = Count - 1;

        while(lowerbound < upperbound)
        {
            while (lowerbound < upperbound)
            {
                int mid = (lowerbound + upperbound) / 2;
                int comparer = _subSets[mid].CompareTo(treeToAdd);

                if (comparer < 0)
                {
                    lowerbound = mid + 1; //Search right
                }
                else
                {
                    upperbound = mid; //Search left
                }
            }//end while
            return lowerbound; //This is the correct insertion index
        }//end while

        return lowerbound;
    }//FindeIndexOfInsertion
    public void AddRange(IEnumerable<ISetTree<T>> coll)
    {
        //Checks if null
        ArgumentNullException.ThrowIfNull(coll, nameof(coll));

        foreach (var item in coll)
        {
            this.Add(item);
        }
    }//AddRange

    public bool Contains(ISetTree<T> val)
    {
        //Checks if null
        ArgumentNullException.ThrowIfNull(val, nameof(val));

        return this.IndexOf(val) >= 0;
    }//Contains


    public int IndexOf(ISetTree<T> val)
    {
        //Checks if null
        ArgumentNullException.ThrowIfNull(val, nameof(val));

        //Try get the index of insertion
        int index = FindIndexOfInsertion(val);
        
        //Check if the element in the index of insertion is the same as value

        if(index >= 0 && _subSets[index].CompareTo(val) == 0)
            return index;

        return -1;
    }//IndexOf

    public bool Remove(ISetTree<T> val)
    {
        //Checks if null
        ArgumentNullException.ThrowIfNull(val, nameof(val));

        //Find Index of value
        int index = FindIndexOfInsertion(val);

        //Element not found
        if(index < 0)
            return false;

        //Remove element
        RemoveAt(index);

        return true;
    }//Remove

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }//RemoveAt
    public void Clear()
    {
        //Reset collection
        this._subSets = new List<ISetTree<T>>();
    }//Clear
    public IEnumerator<ISetTree<T>> GetEnumerator()
    {
        return this._subSets.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this._subSets.GetEnumerator();
    }//GetEnumerator
}//class

//{
//    public class SortedSubSets<T> : ISortedSubSets<T>
//        where T : IComparable
//    {
//        //private datamembers
//        private List<ISetTree<T>> _collection;

//        public int Count => _collection.Count;

//        Element<T> ISortedSetCollection<T>.this[int index] => FindElementByIndex(index);

//        public ISetTree<T> this[int index]
//        {
//            get
//            {
//                if (index < 0 || index >= _collection.Count)
//                    throw new IndexOutOfRangeException();

//                return _collection[index];
//            }
//        }//indexer
//        public SortedSubSets()
//        {
//            this._collection = new List<ISetTree<T>>();
//        }//ctor 01
//        public SortedSubSets(IEnumerable<ISetTree<T>> subsets) : this()
//        {
//            this.AddRange(subsets);
//        }//ctor 02
//        public void Add(ISetTree<T> newTree)
//        {
//            //If it is empty
//            if(this.Count <= 0)
//            {
//                _collection.Add(newTree);
//                return;
//            }//end if empty

//            //Get the index of the last element
//            //-The point of insertion will always be (indexOfLast + 1)
//            int indexOfLast = _collection.Count - 1;

//            //Add an empty sport at the end of the list
//            _collection.Add(default);

//            //A boolean condition that will tell us if an insersion point was found or not
//            bool insertionPointFound = false;

//            while(!insertionPointFound && indexOfLast>= 0)
//            {
//                //First compare the cardinalities
//                int cardinalityA = newTree.Cardinality;//This is the cardinality of the new element
//                int cardinalityB = _collection[indexOfLast].Cardinality;//This is the cardinality of the last element

//                if (cardinalityA > cardinalityB)//This means that the current tree comes before the new tree
//                    insertionPointFound = true;

//                if (cardinalityA == cardinalityB)//This means that further inspection needs to be considered
//                    insertionPointFound = NewElementBeforeExistingElement(newTree, _collection[indexOfLast]);

//                if (cardinalityA == 0)//If this is an empty set
//                    insertionPointFound = false;

//                if (!insertionPointFound) 
//                {
//                    //Move the last element to next sport
//                    _collection[indexOfLast + 1] = _collection[indexOfLast];

//                    indexOfLast--;
//                }
//            }//end while

//            //Here we add the element at indexOfLast + 1
//            _collection[++indexOfLast] = newTree;
//        }//AddNew
//        private bool NewElementBeforeExistingElement(ISetTree<T> newTree, ISetTree<T> existing)
//        {
//            //If the new tree has less subsets than the existsing one then the new one comes before the existsing one 
//            if (newTree.NumberOfSubsets < existing.NumberOfSubsets)
//                return true;

//            if (newTree.NumberOfSubsets > existing.NumberOfSubsets)
//                return false;

//            //Here it means they have the same number of elements and subset subsets

//            //-If both of them have no subsets
//            if(newTree.NumberOfSubsets == 0 && existing.NumberOfSubsets == 0)
//            {
//                int i = 0;
//                int cardinality = newTree.Cardinality;
//                while (i < cardinality)
//                {
//                    //Compare the corresponding elements
//                    int compareVal = existing.GetRootElementByIndex(i).CompareTo(newTree.GetRootElementByIndex(i));
//                    if (compareVal < 0)
//                        return true;
//                    if (compareVal > 0)
//                        return false;
//                    i++;
//                }//end while
//                return false;
//            }//end if no subsets


//            //-Compare the nesting levels of the first elements in the subtrees
//            int levelNew = newTree[0].NestedLevel, levelExisting = existing[0].NestedLevel;
//            if (levelNew > levelExisting)
//                return false;
//            if (levelExisting > levelNew)
//                return true;

//            //This means we have nested subset with alsmost the same structure
//            return NewElementBeforeExistingElement(newTree.GetSubSetElementByIndex(0), existing.GetSubSetElementByIndex(0));
//        }//NewElementBeforeExistingElement
//        public Element<T> FindElementByIndex(int index)
//        {
//            int nestingLevel = 0, currentIndex = 0;
//            Element<T> element = default(Element<T>);
//            for (int i = 0; i < _collection.Count; i++)
//            {
//                element = FindElement(_collection[i], ref currentIndex, index, nestingLevel);
//                if (element.ElementFound)
//                    break;
//            }
//            return element;
//            //return new Element<T>(val, nestingLevel, nestingLevel == 1);
//        }//FindByIndex
//        private Element<T> FindElement(ISetTree<T> currentTree, ref int currentIndex, int index, int nestinglevel)
//        {
//            Element<T> element = default(Element<T>);
//             var a = element.ElementFound == true;
//            nestinglevel++;
//            //First start with the root elements
//            foreach (var item in currentTree.GetRootElementsEnumarator())
//            {
//                if (currentIndex == index)
//                {
//                    element = new Element<T>(item, nestinglevel, false);
//                    return element;
//                }
//                //increase the current index
//                currentIndex++;
//            }//end root elements foreach

//            foreach (var item in currentTree.GetSubsetsEnumarator())
//            {
//                element = FindElement(item, ref currentIndex, index, nestinglevel);
                
//                if (currentIndex >= index && element.ElementFound)
//                    return element;
//            }//end for each
//            return element;
//        }//FindElement
//        public int IndexOf(ISetTree<T> val) 
//        {
//            string set = val.ToString();
//            int index = 0;
//            foreach (var item in this)
//            {
//                if (item.ToString() == set)
//                    return index;
//                index++;
//            }
//            return -1;
//        }//end index
//        public bool Remove(ISetTree<T> val)
//        {
//            return this._collection.Remove(val);
//        }//Remove
//        public void RemoveAt(int index) => _collection.RemoveAt(index);
//        public IEnumerator<ISetTree<T>> GetEnumerator()
//        {
//            return _collection.GetEnumerator();
//        }//end enumarator
//        public bool Contains(ISetTree<T> val)
//        {
//            return this._collection.Contains(val);
//        }//Contains
//        public void AddRange(IEnumerable<ISetTree<T>> coll)
//        {
//            foreach (var item in coll)
//            {
//                this.Add(item);
//            }
//        }//AddRange
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return this.GetEnumerator();
//        }//GetEnumerator
//    }//namespace
//}//namespace