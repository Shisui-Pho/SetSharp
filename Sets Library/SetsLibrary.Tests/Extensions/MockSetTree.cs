using SetsLibrary;
namespace SetsLibrary.Tests.Extensions
{
    public class MockSetTree : ISetTree<int>
    {
        private List<int> rootElements;
        private List<ISetTree<int>> subsets;
        public MockSetTree(SetsConfigurations config)
        {
            rootElements = new List<int>();
            subsets = new List<ISetTree<int>>();
            ExtractionSettings = config;
        }

        public string RootElements => string.Join(",", rootElements);

        public int Count => rootElements.Count + subsets.Count;

        public SetsConfigurations ExtractionSettings{ get; private set; }

        public SetTreeInfo TreeInfo => throw new NotImplementedException();

        public int CountRootElements => rootElements.Count;

        public int CountSubsets => subsets.Count;

        public void AddElement(int element)
        {
            rootElements.Add(element);
        }

        public void AddElement(ISetTree<int> element)
        {
            subsets.Add(element);
        }

        public void AddRange(IEnumerable<int> elements)
        {
            foreach (var item in elements)
                AddElement(item);
        }

        public void AddRange(IEnumerable<ISetTree<int>> elements)
        {
            foreach (var item in elements)
                AddElement(item);
        }

        public int CompareTo(ISetTree<int>? other)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetRootElementsEnumerator()
        {
            return rootElements;
        }

        public IEnumerable<ISetTree<int>> GetSubsetsEnumerator()
        {
            return subsets;
        }

        public int IndexOf(int element)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(ISetTree<int> element)
        {
            throw new NotImplementedException();
        }

        public bool RemoveElement(int element)
        {
            throw new NotImplementedException();
        }

        public bool RemoveElement(ISetTree<int> element)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "";
        }
    }
}
