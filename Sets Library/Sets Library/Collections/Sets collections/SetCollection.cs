using SetsLibrary.Collections;
using SetsLibrary.Interfaces;
using System.Collections;
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
        private class Key 
        {
            private Stack<char> _keyChars;
            public string FullKey => string.Join("", _keyChars);
            public Key()
            {
                _keyChars = new Stack<char>();
            }
            public Key(string name)
                : this()
            {
                //Check for nulls
                ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

                //Make the name a key
                ToKey(name);
            }
            private void ToKey(string name)
            {
                var characters = name.ToCharArray();
                for (var i = 0; i < characters.Length; i++) 
                    _keyChars.Push(characters[i]);
            }//ToKey

            //Generate next key
            public Key GenerateNextKey()
            {
                //This will generate the next key based on it current state
                int count_Z_occurences = 0;
                //The original characters of the current key class should be kept as they are
                Stack<char> original = new Stack<char>();
                while (_keyChars.Count > 0)
                {
                    //Pop the last character
                    char currentTop = _keyChars.Pop();
                    original.Push(currentTop);
                    //Check if it is the letter 'Z'?
                    if(currentTop != 'Z')
                    {
                        //Elarge it and push it to the stack
                        currentTop++;

                        _keyChars.Push(currentTop);

                        break;
                    }

                    //Here it means we have the 'Z'character
                    //-We need to add that to our queue
                    count_Z_occurences++;
                }//

                if (_keyChars.Count == 0)//If there's nothing in the stack we need to add one more elemet
                    count_Z_occurences++;

                //Add 'A's
                string name = this.FullKey;
                for (int i = 0; i < count_Z_occurences; i++)
                    name += "A";

                //Re-add the removed charcters
                while(original.Count > 0)
                    _keyChars.Push(original.Pop());

                return new Key(name);
            }//GenerateNextKey
            //Reset key
            public void ResetKey()
            {
                _keyChars.Clear();
            }
        }//emebed class
        #endregion Embedded class
        //Properties
        public int Count => _dicCollection.Count;

        //inderxer
        public IStructuredSet<T> this[string name]
        {
            get
            {
                Key key = new Key(name);
                
                return _dicCollection[key];
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
            //Check for nulls
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            //If we are adding the first element
            //If empty
            if(Count  == 0)
            {
                
                //Add the element with the current key
                _dicCollection.Add(_lastKey, item);
            }

            _lastKey = _lastKey.GenerateNextKey();

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
            _dicCollection = new Dictionary<Key, IStructuredSet<T>>();
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

        public IStructuredSet<T> FindSetByName(string name)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            return this[name];
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
            Reset();

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