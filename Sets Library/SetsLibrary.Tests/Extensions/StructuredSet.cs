using SetSharp;

namespace SetSharp.Tests.Extensions;
public class StructuredSet<T> : IStructuredSet<T> where T : IComparable<T>
{
    private List<T> elements;
    private List<IStructuredSet<T>> subsets;

    public StructuredSet()
    {
        elements = new List<T>();
        subsets = new List<IStructuredSet<T>>();
    }

    public string OriginalExpression => string.Join(",", elements);

    public int Cardinality => elements.Count;

    public SetsConfigurations ExtractionConfiguration => new SetsConfigurations(",");

    public void AddElement(T Element)
    {
        if (!elements.Contains(Element))
        {
            elements.Add(Element);
        }
    }

    public void AddElement(IStructuredSet<T> subset)
    {
        if (!subsets.Contains(subset))
        {
            subsets.Add(subset);
        }
    }

    public void AddSubsetAsString(string subset)
    {
        // For simplicity, we won't convert the string to a subset here in the mock
        elements.Add((T)Convert.ChangeType(subset, typeof(T)));
    }

    public bool RemoveElement(IStructuredSet<T> subset)
    {
        return subsets.Remove(subset);
    }

    public IStructuredSet<T> MergeWith(IStructuredSet<T> set)
    {
        var mergedSet = new StructuredSet<T>();
        foreach (var element in elements)
        {
            mergedSet.AddElement(element);
        }
        foreach (var subset in subsets)
        {
            mergedSet.AddElement(subset);
        }
        return mergedSet;
    }

    public IStructuredSet<T> Without(IStructuredSet<T> setB)
    {
        var resultSet = new StructuredSet<T>();
        foreach (var element in elements)
        {
            if (!setB.Contains(element))
            {
                resultSet.AddElement(element);
            }
        }
        foreach (var subset in subsets)
        {
            if (!setB.Contains(subset))
            {
                resultSet.AddElement(subset);
            }
        }
        return resultSet;
    }

    public bool RemoveElement(T Element)
    {
        return elements.Remove(Element);
    }

    public bool Contains(T Element)
    {
        return elements.Contains(Element);
    }

    public bool Contains(IStructuredSet<T> subset)
    {
        return subsets.Contains(subset);
    }

    public bool IsSubSetOf(IStructuredSet<T> setB, out SetResultType type)
    {
        type = SetResultType.SubSet;
        return false;  // Default response for the mock
    }

    public bool IsElementOf(IStructuredSet<T> setB)
    {
        return false;  // Default response for the mock
    }

    public void Clear()
    {
        elements.Clear();
        subsets.Clear();
    }

    public string BuildStringRepresentation()
    {
        return string.Join(",", elements);
    }

    public IEnumerable<T> EnumerateRootElements()
    {
        return elements;
    }

    public IEnumerable<IStructuredSet<T>> EnumerateSubsets()
    {
        return subsets;
    }

    public IStructuredSet<T> BuildNewSet()
    {
        return new StructuredSet<T>();
    }

    public IStructuredSet<T> BuildNewSet(string setString)
    {
        var newSet = new StructuredSet<T>();
        newSet.AddSubsetAsString(setString);
        return newSet;
    }
}