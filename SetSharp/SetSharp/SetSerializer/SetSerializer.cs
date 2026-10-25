using SetSharp.SetOperations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SetSharp;

public class SetSerializer<T> where T : IComparable<T>
{
    private readonly SetsConfigurations _configurations;

    public SetSerializer(SetsConfigurations configurations)
    {
        _configurations = configurations;
    }

    public string Serialize(ISetTree<T> setTree)
    {
        ArgumentNullException.ThrowIfNull(setTree, nameof(setTree));


        StringBuilder expression = new();

        BuildTree(setTree, expression);

        return expression.ToString();
    }

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
            if(hasWrittenContent)
            {
                //it means expression had some elements, so we need the row terminator
                expression.Append(_configurations.RowTerminator);
            }

            expression.Append("{}");
            hasWrittenContent = true;
        }

        bool hasEmptySetAlreadyCaptured = false;

        //Now we need to generate the children/subsets
        foreach(var subset in setTree.GetSubsetsEnumerator())
        {
            //Skip empty sets
            //-This is because for a set, we only keep track of 1 empty subsets as is unique
            if (subset.TreeInfo.IsEmptyTree && _configurations.IgnoreEmptySets) continue;

            //If we have already capture the empty set, then we don't need to re-add it\
            if (subset.TreeInfo.IsEmptyTree)
            {
                if (hasEmptySetAlreadyCaptured) continue;

                hasEmptySetAlreadyCaptured = true;
            }

            //Since this cannot be an empty set, we can just append the row terminator
            //-But this is only true if the tree had root elements
            if(hasWrittenContent)
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
