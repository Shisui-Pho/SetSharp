using SetsLibrary.Interfaces;
using System.Collections;
namespace SetsLibrary.Collections;

public class SortedSubSets<T> : BaseSortedCollection<ISetTree<T>>,  ISortedSubSets<T>
    where T : IComparable<T>
{
    //Contructors
    public SortedSubSets()
        : base()
    {
    }//ctor main
    public SortedSubSets(IEnumerable<ISetTree<T>> subSets)
        : base(subSets) 
    {
    }//ctor 01
}//class