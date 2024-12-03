using SetsLibrary.Collections;
using SetsLibrary.Interfaces;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SetLibrary.Collections
{
    public class SetCollection<T> : ISetCollection<IStructuredSet<T>, T>
        where T : IComparable<T>
    {
        //Data-fields
        //This dictionary will hold the set names(Unique) as well as the set itselt 
        private Dictionary<Key, IStructuredSet<T>> _dicCollection;
        //This will keep track of the current key/last key
        private Key _lastKey;

        #region Embedded class
        private class Key : IEqualityComparer<Key>
        {
            private Stack<char> _keyChars;
            public string FullKey => string.Join("", _keyChars);
            public Key()
            {
                _keyChars = new Stack<char>();
            }
            private Key(IEnumerable<char> chars)
                : this()
            {
                //Check for nulls
                ArgumentNullException.ThrowIfNull(chars, nameof(chars));

                //Add to key
                ToKey(chars);
            }
            public Key(string name)
                : this()
            {
                //Check for nulls
                ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

                //Make the name a key
                ToKey(name);
            }
            private void ToKey(IEnumerable<char> characters)
            {
                foreach (var item in characters)
                    _keyChars.Push(item);                
            }

            //Generate next key
            public Key GenerateNextKey()
            {
                //Queue to keep track of the new characters
                Stack<char> newKey = new Stack<char>();
                Stack<char> currentCopy = new Stack<char>();

                //A flag to determine if a solution was found or not
                bool isIncremented = false;
                int lastAlphabetOccurences = 0;
                while(_keyChars.Count > 0)
                {
                    //Pop the top element
                    char currentTop = _keyChars.Pop();

                    //Add it to the copy
                    currentCopy.Push(currentTop);

                    //Check if the current Top element is the 'Z' element and the next key has 
                    //not been generated
                    if (currentTop != 'Z' && !isIncremented)
                    {
                        //Generate the key by incrementing the current top element
                        currentTop++;
                        isIncremented = true;
                    }
                    else if (currentTop == 'Z' && !isIncremented)
                    {
                        //Increment the top element to be an 'A'
                        currentTop = 'A';

                        //Count the 'Z' occurences 
                        lastAlphabetOccurences++;
                    }

                    //Count the Z occurences 
                    if(currentTop == 'Z')
                        lastAlphabetOccurences++;

                    //Push to the newKey stack
                    newKey.Push(currentTop);
                }//end while

                //Ovveride the new old stak with the new stack
                _keyChars = currentCopy;

                //Check if we need to increment the string, i.e, we have to add 'A'
                if (_keyChars.Count == lastAlphabetOccurences)
                    newKey.Push('A');

                return new Key(newKey.Reverse());
            }//GenerateNextKey
            //Reset key
            public void ResetKey()
            {
                _keyChars.Clear();
            }

            public bool Equals(Key? x, Key? y)
            {
                return x.FullKey == y.FullKey;
            }

            public int GetHashCode([DisallowNull] Key obj)
            {
                return obj.FullKey.GetHashCode();
            }
        }//emebed class
        #endregion Embedded class
        //Properties
        public int Count => _dicCollection.Count;

        //inderxer
        public IStructuredSet<T>? this[string name]
        {
            get
            {
                return FindSetByName(name);
            }
        }


        #region Constructors

        public SetCollection()
        {
            Clear();
        }//ctor default

        public SetCollection(IEnumerable<IStructuredSet<T>> collection)
        {
            Clear();
            
            //Check for nulls
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));

            //Add the range
            this.AddRange(collection);
        }

        #endregion Constructors



        public void Add(IStructuredSet<T> item)
        {
            _lastKey = _lastKey.GenerateNextKey();

            //Check for nulls
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            //Add the element
            _dicCollection.Add(_lastKey, item);
        }
        public void AddRange(IEnumerable<IStructuredSet<T>> collection)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));

            foreach (var item in collection)
                Add(item);
        }//AddRange

        public void Clear()
        {
            _dicCollection = new Dictionary<Key, IStructuredSet<T>>(new Key());
            _lastKey = new Key();
        }

        public bool Contains(IStructuredSet<T> item)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            //USe linq to check for it
            return _dicCollection.ContainsValue(item);
        }//Contains

        public bool Contains(string name)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            //Get the key value
            Key key = new Key(name);

            return _dicCollection.ContainsKey(key);
        }

        public IStructuredSet<T>? FindSetByName(string name)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            //Generate the key
            Key key = new Key(name);

            //Check if the key exists
            if( _dicCollection.ContainsKey(key))
                return _dicCollection[key];

            return null;
        }//FindSetByName

        public bool Remove(string name)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            Key key = new Key(name);

            return _dicCollection.Remove(key);
        }

        public void Reset()
        {
            //Make a copy of the list
            var array = _dicCollection.Values;

            //Reset the current clasas
            Clear();

            //Re-add the sets
            AddRange(array);
            //
        }//Reset

        public IEnumerator<KeyValuePair<string, IStructuredSet<T>>> GetEnumerator()
        {
            foreach (var item in _dicCollection)
            {
                yield return new KeyValuePair<string, IStructuredSet<T>>(item.Key.FullKey, item.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }//class
}//namespace