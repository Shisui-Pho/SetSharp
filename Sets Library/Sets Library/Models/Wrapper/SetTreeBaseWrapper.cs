using SetsLibrary.Interfaces;
namespace SetsLibrary.Models;

public abstract class SetTreeBaseWrapper<T> : ISetTree<T>
    where T : IComparable<T>
{
    protected readonly ISetTree<T> setTree;

    public SetTreeBaseWrapper(ISetTree<T> setTree)
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(setTree, nameof(setTree));


        this.setTree = setTree;
    }//ctor default

    public string RootElements => setTree.RootElements;

    public int Count => setTree.Count;

    public int CountRootElements => setTree.CountRootElements;

    public int CountSubsets => setTree.CountSubsets;

    public SetExtractionConfiguration<T> ExtractionSettings => setTree.ExtractionSettings;

    public void AddElement(T element)
    {
        setTree.AddElement(element);
    }

    public void AddRange(IEnumerable<T> elements)
    {
        setTree.AddRange(elements);
    }

    public void AddRange(IEnumerable<ISetTree<T>> subsets)
    {
        setTree.AddRange(subsets);
    }

    public void AddSubSetTree(ISetTree<T> tree)
    {
        setTree.AddSubSetTree(tree);
    }

    public int CompareTo(ISetTree<T>? other)
    {
        return setTree.CompareTo(other);
    }

    public IEnumerable<T> GetRootElementsEnumarator()
    {
        return setTree.GetRootElementsEnumarator();
    }

    public IEnumerable<ISetTree<T>> GetSubsetsEnumarator()
    {
        return setTree.GetSubsetsEnumarator();
    }

    public int IndexOf(T element)
    {
        return setTree.IndexOf(element);
    }

    public int IndexOf(ISetTree<T> subset)
    {
        return setTree.IndexOf(subset);
    }

    public bool RemoveElement(T element)
    {
        return setTree.RemoveElement(element);
    }

    public bool RemoveElement(ISetTree<T> element)
    {
        return setTree.RemoveElement(element);
    }
}//class