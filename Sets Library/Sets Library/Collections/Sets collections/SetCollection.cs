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
        private Dictionary<string, IStructuredSet<T>> _dicCollection;
        //This will keep track of the current key/last key
        private Key _lastKey;

        #region Embedded class
        private class Key : IEqualityComparer<Key>
        {
            private readonly List<char> _keyChars;

            public string FullKeyValue { get; private set; }
            public Key()
            {
                _keyChars = new List<char>();
                FullKeyValue = "";
            }
            public Key(IEnumerable<char> characterKeys)
                : this()
            {
                ArgumentNullException.ThrowIfNull(characterKeys, nameof(characterKeys));

                _keyChars.AddRange(characterKeys);

                FullKeyValue = string.Join("", characterKeys);
            }//ctor main
            public Key GenerateNextKey()
            {
                //If the current list is empty
                if (_keyChars.Count == 0)
                    return new Key("A");

                //Create a copy
                //Check the last and first elements if they are 'Z'
                char[]? newValues = null;

                if (_keyChars[0] == 'Z' && _keyChars[_keyChars.Count - 1] == 'Z')
                    newValues = new char[_keyChars.Count + 1];
                else
                    newValues = new char[_keyChars.Count];

                //Here the length of the array is already been determined
                bool isIncremented = false;
                for (int i = _keyChars.Count - 1; i >= 0; i--)
                {
                    char current = _keyChars[i];
                    if (current == 'Z' && !isIncremented)
                        current = 'A';
                    else if (!isIncremented)
                    {
                        current++;
                        isIncremented = true;
                    }

                    newValues[i] = current;
                }

                if (_keyChars.Count < newValues.Length)//It means that the values array has one extra slot
                    newValues[newValues.Length - 1] = 'A';

                return new Key(newValues);
            }//GenerateNextKey
            //Reset key
            public void ResetKey()
            {
                _keyChars.Clear();
            }

            public bool Equals(Key? x, Key? y)
            {
                return x.FullKeyValue == y.FullKeyValue;
            }

            public int GetHashCode([DisallowNull] Key obj)
            {
                return obj.FullKeyValue.GetHashCode();
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
            _dicCollection.Add(_lastKey.FullKeyValue, item);
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
            _dicCollection = new Dictionary<string, IStructuredSet<T>>();
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

            return _dicCollection.ContainsKey(key.FullKeyValue);
        }

        public IStructuredSet<T>? FindSetByName(string name)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            //Generate the key
            Key key = new Key(name);

            //Check if the key exists
            if( _dicCollection.ContainsKey(key.FullKeyValue))
                return _dicCollection[key.FullKeyValue];

            return null;
        }//FindSetByName

        public bool Remove(string name)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            Key key = new Key(name);

            return _dicCollection.Remove(key.FullKeyValue);
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
                yield return new KeyValuePair<string, IStructuredSet<T>>(item.Key, item.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }//class
}//namespace