/*
 * File: SetTreeExtensions.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 13 January 2025
 * 
 * Description:
 * This file contains extension methods for the ISetTree<TElement> interface, 
 * providing various utilities to convert set trees to different collection types, 
 * such as lists, structured sets, and typed sets. The methods also include 
 * conversion functionality for nested subsets within the set tree.
 * 
 * Key Features:
 * - Converts ISetTree<TElement> to lists of root elements, subtrees, and subsets.
 * - Supports conversion to structured sets, typed sets, and string literal sets.
 * - Provides helper methods for handling enumerators and internal collections.
 * - Handles conversion for custom object subsets and exception management.
 * 
 * Generic Constraints:
 * - TElement must implement IComparable<TElement> for proper element comparison within the set.
 * - Specific conversion methods also support additional constraints like IConvertible and ICustomObjectConverter<TElement>.
 */

using SetsLibrary.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SetsLibrary.Extensions
{
    /// <summary>
    /// Provides extension methods for converting <see cref="ISetTree{TElement}"/> objects into different collection types.
    /// </summary>
    public static class SetTreeExtensions
    {
        /// <summary>
        /// Converts an <see cref="ISetTree{TElement}"/> to a list of root elements.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements within the set tree.</typeparam>
        public class SetTreeListConverter<TElement>(ISetTree<TElement> tree)
            where TElement : IComparable<TElement>
        {
            private readonly ISetTree<TElement> _tree = tree;

            /// <summary>
            /// Converts the root elements of the set tree to a list.
            /// </summary>
            /// <returns>A list of root elements.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the tree is null.</exception>
            /// <exception cref="Exception">Thrown if conversion fails.</exception>
            public List<TElement> ToListRootElements()
            {
                // Check for null
                ArgumentNullException.ThrowIfNull(_tree, nameof(tree));

                if (_tree is SetTree<TElement> setTree)
                {
                    return ToListElementsUsingInternalCollection(setTree, () => setTree._elements);
                }
                else
                {
                    return ToListElementsUsingEnumerator(_tree.GetRootElementsEnumerator());
                }
            }

            /// <summary>
            /// Converts the subtrees of the set tree to a list.
            /// </summary>
            /// <returns>A list of subtrees.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the tree is null.</exception>
            /// <exception cref="Exception">Thrown if conversion fails.</exception>
            public List<ISetTree<TElement>> ToListSubTrees()
            {
                // Check for null
                ArgumentNullException.ThrowIfNull(_tree, nameof(tree));

                if (_tree is SetTree<TElement> setTree)
                {
                    return ToListElementsUsingInternalCollection<ISetTree<TElement>>(setTree, () => setTree._subSets);
                }
                else
                {
                    return ToListElementsUsingEnumerator(_tree.GetSubsetsEnumerator());
                }
            }

            /// <summary>
            /// Converts the subtrees to a list of structured sets.
            /// </summary>
            /// <param name="generateSet">A function that generates structured sets.</param>
            /// <returns>A list of structured sets.</returns>
            /// <exception cref="NotImplementedException">Thrown if the method is not implemented.</exception>
            public List<IStructuredSet<TElement>> ToListSubtreesAsStructuredSubsets(Func<IIndexedSetTree<TElement>, IStructuredSet<TElement>> generateSet)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Converts the subtrees to a list of subsets of a specific type.
            /// </summary>
            /// <typeparam name="TType">The type of the subsets.</typeparam>
            /// <param name="generateSet">A function that generates the subsets.</param>
            /// <returns>A list of subsets of type <typeparamref name="TType"/>.</returns>
            /// <exception cref="NotImplementedException">Thrown if the method is not implemented.</exception>
            public List<TType> ToListSubTreeAsSubsets<TType>(Func<IIndexedSetTree<TElement>, TType> generateSet)
                where TType : IStructuredSet<TElement>
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Helper method to convert a collection of elements into a list using an enumerator.
            /// </summary>
            /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
            /// <param name="items">The enumerable collection of items.</param>
            /// <returns>A list of items.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the items are null.</exception>
            private static List<TItem> ToListElementsUsingEnumerator<TItem>(IEnumerable<TItem> items)
            {
                // Check for null
                ArgumentNullException.ThrowIfNull(items, nameof(items));

                // Create a new list and add elements
                List<TItem> elements = new List<TItem>();
                elements.AddRange(items);

                return elements;
            }

            /// <summary>
            /// Helper method to convert the internal collection of a set tree into a list.
            /// </summary>
            /// <typeparam name="TItem">The type of the items in the internal collection.</typeparam>
            /// <param name="tree">The set tree to convert.</param>
            /// <param name="getInternalElements">A function to get the internal elements collection.</param>
            /// <returns>A list of items from the internal collection.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the tree is null.</exception>
            private static List<TItem> ToListElementsUsingInternalCollection<TItem>(SetTree<TElement> tree, Func<ISortedCollection<TItem>> getInternalElements)
                where TItem : IComparable<TItem>
            {
                // Check for null
                ArgumentNullException.ThrowIfNull(tree, nameof(tree));

                List<TItem> elements;

                // Get the internal elements
                var internalElements = getInternalElements();

                if (internalElements is SortedCollection<TItem> sortedElements)
                {
                    elements = sortedElements.ToList();
                }
                else
                {
                    elements = new List<TItem>(internalElements);
                }

                return elements;
            }
        }

        #region Converting "ISetTree<TElement>" to type "IStructuredSet<TElement>"

        /// <summary>
        /// Converts an <see cref="ISetTree{TElement}"/> to a structured set using a provided function.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the set.</typeparam>
        /// <param name="tree">The set tree to convert.</param>
        /// <param name="generateSetFunct">A function that generates a structured set from the indexed tree.</param>
        /// <returns>A structured set.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the tree or the function is null.</exception>
        /// <exception cref="SetsException">Thrown if an error occurs during conversion.</exception>
        public static IStructuredSet<T> ToStructuredSet<T>([NotNullIfNotNull("tree")] this ISetTree<T> tree, Func<IIndexedSetTree<T>, IStructuredSet<T>> generateSetFunct)
            where T : IComparable<T>
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNull(tree, nameof(tree));
            ArgumentNullException.ThrowIfNull(generateSetFunct, nameof(generateSetFunct));

            try
            {
                // Create an instance of indexed tree
                var indexedTree = new SetTreeWrapper<T>(tree);

                // Generate the structured set
                return generateSetFunct(indexedTree);
            }
            catch (Exception ex)
            {
                throw new SetsException("Could not complete conversion.", "Check inner exception.", ex);
            }
        }

        /// <summary>
        /// Converts an <see cref="ISetTree{TElement}"/> to a typed set.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the set.</typeparam>
        /// <param name="tree">The set tree to convert.</param>
        /// <returns>A typed set.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the tree is null.</exception>
        /// <exception cref="ArgumentException">Thrown if an error occurs during conversion.</exception>
        public static TypedSet<T> ToTypedSet<T>(this ISetTree<T> tree)
            where T : IComparable<T>, IConvertible
        {
            // Check for null
            ArgumentNullException.ThrowIfNull(tree, nameof(tree));

            try
            {
                // Create the indexed tree
                var indexedTree = new SetTreeWrapper<T>(tree);

                // Return a new typed set
                return new TypedSet<T>(indexedTree);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cannot complete operation.", "Check inner exception.", ex);
            }
        }

        /// <summary>
        /// Converts an <see cref="ISetTree{TElement}"/> to a string literal set.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the set tree.</typeparam>
        /// <param name="tree">The set tree to convert.</param>
        /// <returns>A string literal set.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the tree is null.</exception>
        /// <exception cref="ArgumentException">Thrown if an error occurs during conversion.</exception>
        public static StringLiteralSet ToStringLiteralSet<TElement>(this ISetTree<TElement> tree)
            where TElement : IComparable<TElement>
        {
            // Check for null
            ArgumentNullException.ThrowIfNull(tree, nameof(tree));

            try
            {
                // Clone the configurations
                var config = new SetExtractionConfiguration<string>(tree.ExtractionSettings.FieldTerminator, tree.ExtractionSettings.RowTerminator);

                // Get the string representation
                var expression = tree.ToString();

                // Create and return the string literal set
                return new StringLiteralSet(expression, config);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cannot complete operation.", "Check inner exception.", ex);
            }
        }

        #endregion

        #region Converting "ISetTree<TElement>" to type of List<ListItem>

        /// <summary>
        /// Converts the root elements of the set tree to a list of <typeparamref name="TElement"/>.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the set tree.</typeparam>
        /// <param name="tree">The set tree to convert.</param>
        /// <returns>A list of root elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the tree is null.</exception>
        /// <exception cref="SetsException">Thrown if conversion fails.</exception>
        public static List<TElement> ToListRootElements<TElement>(this ISetTree<TElement> tree)
            where TElement : IComparable<TElement>
        {
            // Check for null
            ArgumentNullException.ThrowIfNull(tree, nameof(tree));

            SetTreeListConverter<TElement> converter = new SetTreeListConverter<TElement>(tree);

            try
            {
                // Convert to list and return
                return converter.ToListRootElements();
            }
            catch (Exception ex)
            {
                throw new SetsException("Could not complete conversion.", "Check inner exception.", ex);
            }
        }

        /// <summary>
        /// Converts the subtrees of the set tree to a list of structured sets.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the set tree.</typeparam>
        /// <param name="tree">The set tree to convert.</param>
        /// <param name="generateSet">A function to generate the structured sets.</param>
        /// <returns>A list of structured sets.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the tree or the function is null.</exception>
        /// <exception cref="SetsException">Thrown if conversion fails.</exception>
        public static List<IStructuredSet<TElement>> ToListSubsets<TElement>(this ISetTree<TElement> tree, Func<IIndexedSetTree<TElement>, IStructuredSet<TElement>> generateSet)
            where TElement : IComparable<TElement>
        {
            // Check for null
            ArgumentNullException.ThrowIfNull(tree, nameof(tree));
            ArgumentNullException.ThrowIfNull(generateSet, nameof(generateSet));

            SetTreeListConverter<TElement> converter = new SetTreeListConverter<TElement>(tree);

            try
            {
                return converter.ToListSubtreesAsStructuredSubsets(generateSet);
            }
            catch (Exception ex)
            {
                throw new SetsException("Could not complete conversion.", "Check inner exception.", ex);
            }
        }

        /// <summary>
        /// Converts the subtrees of the set tree to a list of typed subsets.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the set tree.</typeparam>
        /// <param name="tree">The set tree to convert.</param>
        /// <returns>A list of typed subsets.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the tree is null.</exception>
        /// <exception cref="SetsException">Thrown if conversion fails.</exception>
        public static List<TypedSet<TElement>> ToListTypedSubsets<TElement>(this ISetTree<TElement> tree)
            where TElement : IComparable<TElement>, IConvertible
        {
            // Check for null
            ArgumentNullException.ThrowIfNull(tree, nameof(tree));

            SetTreeListConverter<TElement> converter = new SetTreeListConverter<TElement>(tree);

            try
            {
                return converter.ToListSubTreeAsSubsets(
                    indexedTree => new TypedSet<TElement>(indexedTree)
                );
            }
            catch (Exception ex)
            {
                throw new SetsException("Could not complete conversion.", "Check inner exception.", ex);
            }
        }

        #endregion
    }
}//namespace