using SetsLibrary.Interfaces;
using SetsLibrary.Models.SetTree;
namespace SetsLibrary.Models;

public class SetTreeWrapper<T> : SetTreeBaseWrapper<T>
    where T: IComparable<T>
{
    private readonly SetTree<T>? _assSetTree = null;
    public SetTreeWrapper(ISetTree<T> setTree): base(setTree) 
    {
        if(setTree is SetTree<T>)//if they are the same cast the setTree structure
            _assSetTree = setTree as SetTree<T>;

    }//ctor main

    public T? GetRootElementByIndex(int index)
    {
        if (index >= Count || index < 0)
            throw new ArgumentOutOfRangeException("index");

        if(_assSetTree != null)
            return _assSetTree._elements[index];//Acces direcly from the internal colllection

        //Else do a loop
        int currentIndex = 0;
        foreach(T item in base.GetRootElementsEnumarator())
        {
            if(currentIndex ==  index)
                return item;

            currentIndex++;
        }
        return default(T);
    }//GetRootElementByIndex
    public ISetTree<T>? GetSubsetByIndex(int index)
    {
        if (index >= Count || index < 0)
            throw new ArgumentOutOfRangeException("index");

        //Else do a loop
        int currentIndex = 0;
        foreach (ISetTree<T> item in base.GetSubsetsEnumarator())
        {
            if (currentIndex == index)
                return item;

            currentIndex++;
        }
        return default(ISetTree<T>);
    }//GetSubsetByIndex
    public void Clear()
    {
        if(_assSetTree != null)
        {
            _assSetTree._elements.Clear();
            _assSetTree._subSets.Clear();
        }
        else
        {
            throw new ArgumentException("Use a seperate wrapper for this operation or wrapp this one.");
        }
    }//Clear

}//class
