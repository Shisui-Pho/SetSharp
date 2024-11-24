using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetsLibrary.Collections
{
    public abstract class BaseSortedCollection<TElement> : ISortedSetCollection<TElement, TElement>
        where TElement : IComparable<TElement>
    {

        //Data field to store elements
        private List<TElement> _elements;

        //Properties
        public int Count => _elements.Count;

        //Indexer
        public TElement this[int index]
        {
            get
            {
                if (index < 0 || index >= _elements.Count)
                    throw new IndexOutOfRangeException(nameof(index));

                return _elements[index];
            }
        }

        //Default constructor
        public BaseSortedCollection()
        {
            Clear();
        }//ctor main

        //Constructor that initializes the collection with an existing set of elements
        public BaseSortedCollection(IEnumerable<TElement> elements)
        {
            Clear();
            AddRange(elements);
        }//ctor 02

        //Methods

        public void Add(TElement value)
        {
            //Don't add if null
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            //If first element
            if (Count == 0)
            {
                _elements.Add(value);
                return;
            }

            //Find the index for insertion
            int pointOfInsertion = FindIndexOfInsertion(value);
            _elements.Insert(pointOfInsertion, value);
        }//Add

        private int FindIndexOfInsertion(TElement valueToAdd)
        {
            int lowerbound = 0;
            int upperbound = Count; //Set upperbound to Count to allow insertion at the end

            while (lowerbound < upperbound)
            {
                int mid = (lowerbound + upperbound) / 2;
                int comparer = _elements[mid].CompareTo(valueToAdd);

                if (comparer < 0)
                {
                    lowerbound = mid + 1; //Search right
                }
                else
                {
                    upperbound = mid; //Search left
                }
            }
            return lowerbound; //This is the correct insertion index
        }//FindIndexOfInsertion

        public void AddRange(IEnumerable<TElement> coll)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNull(coll, nameof(coll));

            foreach (var item in coll)
            {
                Add(item);
            }
        }//AddRange

        public bool Contains(TElement val)
        {
            ArgumentNullException.ThrowIfNull(val, nameof(val));

            //Get the index of val
            int index = IndexOf(val);

            return index >= 0 && index < Count;
        }//Contains

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }//GetEnumerator

        public int IndexOf(TElement val)
        {
            ArgumentNullException.ThrowIfNull(val, nameof(val));

            int index = FindIndexOfInsertion(val);

            if (index < Count && val.CompareTo(_elements[index]) == 0)
            {
                return index;
            }

            return -1; //Element not found
        }//IndexOf

        public bool Remove(TElement val)
        {
            ArgumentNullException.ThrowIfNull(val, nameof(val));

            int index = IndexOf(val);

            if (index < 0 || index >= Count) 
                return false; //Element not found

            RemoveAt(index);

            return true; //Element removed successfully
        }//Remove

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _elements.RemoveAt(index);
        }//RemoveAt

        public void Clear()
        {
            _elements = new List<TElement>();
        }//Clear

        //Explicit interface implementation for non-generic IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }//GetEnumerator
    }//class
}//namespace
