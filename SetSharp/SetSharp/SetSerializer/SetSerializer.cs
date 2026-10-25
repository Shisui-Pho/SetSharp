/*
 * File: SetSerializer.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 25 October 2026
 *
 * Description:
 * SetSerializer converts a set tree into a textual set expression. The serializer
 * recursively traverses the tree, writing root elements and nested subsets enclosed
 * in braces and separated by the configured row terminator. It uses the supplied
 * SetsConfigurations to determine how empty sets are handled during serialization.
 * The resulting string represents the structure and contents of the set tree.
 */
using System.Text;

namespace SetSharp;

/// <summary>
/// Serializes a set tree into a textual set expression.
/// </summary>
/// <typeparam name="T">
/// The type of elements stored in the set tree. Must implement
/// <see cref="IComparable{T}"/>.
/// </typeparam>
/// <remarks>
/// The serializer converts an <see cref="ISetTree{T}"/> into a string
/// representation using braces and the configured row terminator.
/// Root elements are written directly from the set tree, while child subsets
/// are serialized recursively.
///
/// Empty trees are represented by "{}". The
/// <see cref="SetsConfigurations.IgnoreEmptySets"/> configuration determines
/// whether empty subsets are omitted from the serialized expression.
/// When empty sets are included, duplicate empty subsets are represented only
/// once at the same level of the tree.
///
/// The serializer uses the row terminator specified by
/// <see cref="SetsConfigurations.RowTerminator"/> to separate content within
/// a set expression. The resulting expression represents the structure and
/// contents of the supplied set tree.
/// </remarks>
public class SetSerializer<T> where T : IComparable<T>
{
    private readonly SetsConfigurations _configurations;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetSerializer{T}"/> class.
    /// </summary>
    /// <param name="configurations">
    /// The configuration settings used when serializing the set tree,
    /// including the row terminator and whether empty sets should be ignored.
    /// </param>
    public SetSerializer(SetsConfigurations configurations)
    {
        _configurations = configurations;
    }

    /// <summary>
    /// Serializes the specified set tree into a textual set expression.
    /// </summary>
    /// <param name="setTree">
    /// The set tree whose elements and subsets will be serialized. Must not be null.
    /// </param>
    /// <returns>
    /// A string containing the serialized representation of the set tree,
    /// including braces, root elements, nested subsets, and configured row
    /// terminators as applicable.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="setTree"/> is null.
    /// </exception>
    /// <remarks>
    /// Serialization begins at the root of the supplied tree and recursively
    /// processes its subsets. Empty trees are represented by "{}".
    /// Empty subsets may be omitted depending on the configured
    /// <see cref="SetsConfigurations.IgnoreEmptySets"/> setting.
    /// </remarks>
    public string Serialize(ISetTree<T> setTree)
    {
        ArgumentNullException.ThrowIfNull(setTree, nameof(setTree));


        StringBuilder expression = new();

        BuildTree(setTree, expression);

        return expression.ToString();
    }

    /// <summary>
    /// Recursively appends the serialized representation of a set tree to the
    /// specified string builder.
    /// </summary>
    /// <param name="setTree">
    /// The set tree to serialize, including its root elements and child subsets.
    /// </param>
    /// <param name="expression">
    /// The <see cref="StringBuilder"/> to which the serialized expression is appended.
    /// </param>
    /// <remarks>
    /// The method encloses each non-empty tree representation in opening and
    /// closing braces and writes the root elements before processing child subsets.
    /// It uses the tree's metadata to determine whether the tree is empty or
    /// contains null elements.
    ///
    /// Child subsets are processed recursively in the order returned by
    /// <see cref="ISetTree{T}.GetSubsetsEnumerator"/>. The configured row
    /// terminator is inserted between written content where required.
    /// Empty subsets are skipped when
    /// <see cref="SetsConfigurations.IgnoreEmptySets"/> is enabled. Otherwise,
    /// duplicate empty subsets at the same level are suppressed so that an
    /// empty subset is serialized at most once at that level.
    ///
    /// This method modifies the supplied <paramref name="expression"/> rather
    /// than returning a new string.
    /// </remarks>
    private void BuildTree(ISetTree<T> setTree, StringBuilder expression)
    {
        var ti = setTree.TreeInfo;

        //if there is nothing base case
        if (ti.IsEmptyTree)
        {
            //If we are ignoring empty sets then we can just disregard this
            //if (!_configurations.IgnoreEmptySets)
            expression.Append("{}");

            return;
        }

        //Grab the root elements
        //TODO: Later on use the SetConfiguration to serialize instead of relying on what the tree is configured to use innitially
        //- in other words, instead of getting the elements property, it's best to just do the join here!!!
        expression.Append('{');
        expression.Append(setTree.RootElements);

        bool hasWrittenContent = setTree.CountRootElements > 0;

        //If there are no elements on the tree and we want to know that
        if (ti.HasNullElements && !_configurations.IgnoreEmptySets)
        {
            if (hasWrittenContent)
            {
                //it means expression had some elements, so we need the row terminator
                expression.Append(_configurations.RowTerminator);
            }

            expression.Append("{}");
            hasWrittenContent = true;
        }

        bool hasEmptySetAlreadyCaptured = false;

        //Now we need to generate the children/subsets
        foreach (var subset in setTree.GetSubsetsEnumerator())
        {
            //Skip empty sets
            //-This is because, for a set, we only keep track of 1 empty subsets as is unique
            if (subset.TreeInfo.IsEmptyTree && _configurations.IgnoreEmptySets) continue;

            //If we have already capture the empty set, then we don't need to re-add it\
            if (subset.TreeInfo.IsEmptyTree)
            {
                if (hasEmptySetAlreadyCaptured) continue;

                hasEmptySetAlreadyCaptured = true;
            }

            //Since this cannot be an empty set, we can just append the row terminator
            //-But this is only true if the tree had root elements
            if (hasWrittenContent)
                expression.Append(_configurations.RowTerminator);

            BuildTree(subset, expression);

            hasWrittenContent = true;
        }

        //Add closing brace
        expression.Append('}');
    }
    //public static string Serialize(BaseSet<T> set)
    //{
    //    var tree = set as BaseSet<T>
    //}
    //public static string Serialize(BaseSet<T> set, SetsConfigurations configs)
    //{

    //}
}