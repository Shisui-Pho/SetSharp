/*
 * File: SetCollection.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 8 December 2024
 * 
 * Description:
 * This file contains the definition of the SetCollection<T> class, 
 * a generic collection that holds unique sets, identified by string keys. 
 * It provides methods to add, remove, check, and retrieve sets by their names, 
 * as well as functionalities to reset and iterate over the collection.
 * 
 * Key Features:
 * - Stores sets of elements that implement IComparable.
 * - Each set is identified by a unique string key, generated sequentially.
 * - Supports adding individual sets, adding a range of sets, and clearing the collection.
 * - Provides methods for checking the existence of sets by name or by object reference.
 * - Allows for enumerating over the collection of sets.
 * 
 * Generic Constraints:
 * - T must implement IComparable<T> to ensure ordered comparison of elements within the sets.
 */

using SetsLibrary.Collections;
using SetsLibrary.Interfaces;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SetLibrary.Collections
{
    /// <summary>
    /// A generic collection class that holds a set of unique sets, identified by string keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sets, which must implement IComparable.</typeparam>
    public class SetCollection<T> : ISetCollection<IStructuredSet<T>, T>
        where T : IComparable<T>
    {
        // Data-fields
        // This dictionary will hold the set names (Unique) as well as the set itself.
        private Dictionary<string, IStructuredSet<T>> _dicCollection;

        // This will keep track of the current key/last key.
        private Key _lastKey;

        #region Embedded class
        /// <summary>
        /// Represents a key used to uniquely identify sets within the collection.
        /// </summary>
        private class Key : IEqualityComparer<Key>
        {
            private readonly List<char> _keyChars;

            /// <summary>
            /// Gets the full key value as a string.
            /// </summary>
            public string FullKeyValue { get; private set; }

            /// <summary>
            /// Default constructor initializing an empty key.
            /// </summary>
            public Key()
            {
                _keyChars = new List<char>();
                FullKeyValue = "";
            }

            /// <summary>
            /// Initializes a key with a list of character values.
            /// </summary>
            /// <param name="characterKeys">The collection of characters representing the key.</param>
            public Key(IEnumerable<char> characterKeys)
                : this()
            {
                ArgumentNullException.ThrowIfNull(characterKeys, nameof(characterKeys));

                _keyChars.AddRange(characterKeys);

                FullKeyValue = string.Join("", characterKeys);
            }

            /// <summary>
            /// Generates the next sequential key by incrementing the current key value.
            /// </summary>
            public Key GenerateNextKey()
            {
                // If the current list is empty
                if (_keyChars.Count == 0)
                    return new Key("A");

                // Create a copy
                // Check the last and first elements if they are 'Z'
                char[]? newValues = null;

                if (_keyChars[0] == 'Z' && _keyChars[_keyChars.Count - 1] == 'Z')
                    newValues = new char[_keyChars.Count + 1];
                else
                    newValues = new char[_keyChars.Count];

                // Here the length of the array is already been determined
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

                if (_keyChars.Count < newValues.Length) // It means that the values array has one extra slot
                    newValues[newValues.Length - 1] = 'A';

                return new Key(newValues);
            }

            /// <summary>
            /// Resets the key to an empty state.
            /// </summary>
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
        } // Embedded class
        #endregion Embedded class

        #region Properties

        /// <summary>
        /// Gets the count of sets in the collection.
        /// </summary>
        public int Count => _dicCollection.Count;

        /// <summary>
        /// Indexer to access sets by their name.
        /// </summary>
        public IStructuredSet<T>? this[string name]
        {
            get
            {
                return FindSetByName(name);
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default constructor that initializes an empty collection.
        /// </summary>
        public SetCollection()
        {
            Clear();
        }

        /// <summary>
        /// Initializes the collection with a range of sets.
        /// </summary>
        /// <param name="collection">The collection of sets to add.</param>
        public SetCollection(IEnumerable<IStructuredSet<T>> collection)
        {
            Clear();

            // Check for nulls
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));

            // Add the range
            this.AddRange(collection);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds a single set to the collection.
        /// </summary>
        /// <param name="item">The set to add.</param>
        public void Add(IStructuredSet<T> item)
        {
            _lastKey = _lastKey.GenerateNextKey();

            // Check for nulls
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            // Add the element
            _dicCollection.Add(_lastKey.FullKeyValue, item);
        }

        /// <summary>
        /// Adds multiple sets to the collection.
        /// </summary>
        /// <param name="collection">The collection of sets to add.</param>
        public void AddRange(IEnumerable<IStructuredSet<T>> collection)
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));

            foreach (var item in collection)
                Add(item);
        }

        /// <summary>
        /// Clears the collection, removing all sets.
        /// </summary>
        public void Clear()
        {
            _dicCollection = new Dictionary<string, IStructuredSet<T>>();
            _lastKey = new Key();
        }

        /// <summary>
        /// Checks if a set is present in the collection.
        /// </summary>
        /// <param name="item">The set to check.</param>
        /// <returns>True if the set is in the collection; otherwise, false.</returns>
        public bool Contains(IStructuredSet<T> item)
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            // Use LINQ to check for it
            return _dicCollection.ContainsValue(item);
        }

        /// <summary>
        /// Checks if a set with a given name exists in the collection.
        /// </summary>
        /// <param name="name">The name of the set to check for.</param>
        /// <returns>True if a set with the name exists; otherwise, false.</returns>
        public bool Contains(string name)
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            // Get the key value
            Key key = new Key(name);

            return _dicCollection.ContainsKey(key.FullKeyValue);
        }

        /// <summary>
        /// Finds a set by its name.
        /// </summary>
        /// <param name="name">The name of the set.</param>
        /// <returns>The set if found, otherwise null.</returns>
        public IStructuredSet<T>? FindSetByName(string name)
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            // Generate the key
            Key key = new Key(name);

            // Check if the key exists
            if (_dicCollection.ContainsKey(key.FullKeyValue))
                return _dicCollection[key.FullKeyValue];

            return null;
        }

        /// <summary>
        /// Removes a set by its name.
        /// </summary>
        /// <param name="name">The name of the set to remove.</param>
        /// <returns>True if the set was removed; otherwise, false.</returns>
        public bool Remove(string name)
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            Key key = new Key(name);

            return _dicCollection.Remove(key.FullKeyValue);
        }

        /// <summary>
        /// Resets the collection to its initial state, re-adding all sets that were previously added.
        /// </summary>
        public void Reset()
        {
            // Make a copy of the list
            var array = _dicCollection.Values;

            // Reset the current class
            Clear();

            // Re-add the sets
            AddRange(array);
        }

        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<KeyValuePair<string, IStructuredSet<T>>> GetEnumerator()
        {
            foreach (var item in _dicCollection)
            {
                yield return new KeyValuePair<string, IStructuredSet<T>>(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Returns a non-generic enumerator for the collection.
        /// </summary>
        /// <returns>A non-generic enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion Methods
    } // class
} // namespace