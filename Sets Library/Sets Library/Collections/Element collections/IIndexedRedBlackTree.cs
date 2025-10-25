
namespace SetsLibrary.Collections
{
    public interface IIndexedRedBlackTree<TElement> where TElement : IComparable<TElement>
    {
        TElement this[int index] { get; }

        int Count { get; }
        bool IsEmpty { get; }

        void Add(TElement item);
        void Clear();
        bool Contains(TElement item);
        IEnumerator<TElement> GetEnumerator();
        int IndexOf(TElement item);
        TElement Max();
        TElement Min();
        bool Remove(TElement item);
        TElement RemoveAt(int index);
        bool TryRemove(TElement item, out TElement? removedData);
    }
}