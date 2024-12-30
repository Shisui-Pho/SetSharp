namespace SetsLibrary.Tests.Collections.Set_Collection;

internal class MockSetStructure : IStructuredSet<int>
{
    private static int _cardinality = 0;

    public int Cardinality => _cardinality;

    public MockSetStructure()
    {
        _cardinality++;
        if (_cardinality >= 1000)
            _cardinality = 0;
    }
    #region No-implementation needed
    public string OriginalExpression => throw new NotImplementedException();

    public SetExtractionConfiguration<int> ExtractionConfiguration => throw new NotImplementedException();


    public void AddElement(int Element)
    {
        throw new NotImplementedException();
    }

    public void AddElement(ISetTree<int> tree)
    {
        throw new NotImplementedException();
    }

    public void AddSubsetAsString(string subset)
    {
        throw new NotImplementedException();
    }

    public string BuildStringRepresentation()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(int Element)
    {
        throw new NotImplementedException();
    }

    public bool Contains(ISetTree<int> tree)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<int> EnumerateRootElements()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IStructuredSet<int>> EnumerateSubsets()
    {
        throw new NotImplementedException();
    }

    public bool IsElementOf(IStructuredSet<int> setB)
    {
        throw new NotImplementedException();
    }

    public bool IsSubSetOf(IStructuredSet<int> setB, out SetResultType type)
    {
        throw new NotImplementedException();
    }

    public IStructuredSet<int> MergeWith(IStructuredSet<int> set)
    {
        throw new NotImplementedException();
    }

    public bool RemoveElement(ISetTree<int> tree)
    {
        throw new NotImplementedException();
    }

    public bool RemoveElement(int Element)
    {
        throw new NotImplementedException();
    }

    public IStructuredSet<int> Without(IStructuredSet<int> setB)
    {
        throw new NotImplementedException();
    }

    public IStructuredSet<int> BuildNewSet()
    {
        throw new NotImplementedException();
    }

    public IStructuredSet<int> BuildNewSet(string setString)
    {
        throw new NotImplementedException();
    }

    #endregion No-Implementation needed
}//mock class
