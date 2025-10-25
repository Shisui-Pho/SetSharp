using System.Collections;
namespace SetsLibrary.Collections;
public class IndexedRedBlackTree<TElement> : IEnumerable<TElement> where TElement : IComparable<TElement>
{
    #region Internal Structures
    private enum NodeColor
    {
        Red,
        Black
    }
    private class Node(TElement item)
    {
        public TElement Data { get; set; } = item;
        public Node Left { get; set; } = null!;
        public Node Right { get; set; } = null!;
        public Node Parent { get; set; } = null!;
        public NodeColor Color { get; set; } = NodeColor.Red;

        public Node Grandparent => Parent.Parent;

        public Node Uncle
        {
            get
            {
                var grandparent = Grandparent;
                if (grandparent == null) return null!;

                return Parent == grandparent.Left ? grandparent.Right : grandparent.Left;
            }
        }

        public Node Sibling
        {
            get
            {
                if (Parent == null) return null!;
                return this == Parent.Left ? Parent.Right : Parent.Left;
            }
        }

    }
    #endregion Internal Structures

    #region Properties, indexers and constructors
    private Node? root = null!;
    public int Count { get; private set; }
    public bool IsEmpty => root == null;
    //Indexer
    public TElement this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException($"Index {index} is out of range. Tree has {Count} elements.");

            var node = GetNodeAtIndex(index);

            if (node == null)
                throw new NullReferenceException("Tree has not been initialized yet, possibility that it's empty");

            return node.Data;
        }
    }//end indexer
    public IndexedRedBlackTree()
    {
        root = null;
        Count = 0;
    }
    public IndexedRedBlackTree(IEnumerable<TElement> elements) : this()
    {

    }
    #endregion Properties, indexers and constructors
    #region Public methods
    public void Add(TElement item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        //Add using BST
        var newNode = InsertBST(item);
        Count++;
        FixViolation(newNode);
    }
    public bool Contains(TElement item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return FindNode(item) != null;
    }
    public int IndexOf(TElement item)
    {
        if (root == null || item == null) return -1;

        var stack = new Stack<Node>();
        var current = root;
        int index = -1;
        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }
            index++;
            current = stack.Pop();
            if (current.Data.CompareTo(item) == 0)
                return index;
            current = current.Right;
        }
        //Element not found
        return -1;
    }
    public IEnumerator<TElement> GetEnumerator()
    {
        return InOrderTraversal().GetEnumerator();
    }
    public TElement Min()
    {
        if (root == null)
            throw new InvalidOperationException("Tree is empty");
        return FindMinNode(root).Data;
    }
    public TElement Max()
    {
        if (root == null)
            throw new InvalidOperationException("Tree is empty");
        return FindMaxNode(root).Data;
    }
    public void Clear()
    {
        root = null;
        Count = 0;
    }
    public bool TryRemove(TElement item, out TElement? removedData)
    {
        var node = FindNode(item);
        if (node == null)
        {
            removedData = default;
            return false;
        }

        removedData = node.Data;
        RemoveNode(node);
        Count--;
        return true;
    }
    public bool Remove(TElement item)
    {
        var node = FindNode(item);
        if (node == null)
            return false;

        RemoveNode(node);
        Count--;
        return true;
    }
    public TElement RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new IndexOutOfRangeException($"Index {index} is out of range. Tree has {Count} elements.");

        var node = GetNodeAtIndex(index) ?? throw new NullReferenceException("Tree has not been initialized yet, possibility that it's empty");

        TElement item = node.Data;
        RemoveNode(node);
        Count--;
        return item;
    }
    #endregion Public methods
    #region Helper methods
    private Node InsertBST(TElement item)
    {
        var newNode = new Node(item);

        if (root == null)
        {
            root = newNode;
        }
        else
        {
            var current = root;
            Node parent = null!;

            while (current != null)
            {
                parent = current;
                current = item.CompareTo(current.Data) < 0 ? current.Left : current.Right;
            }

            newNode.Parent = parent;
            if (parent != null)
            {
                if (item.CompareTo(parent.Data) < 0)
                    parent.Left = newNode;
                else
                    parent.Right = newNode;
            }
        }
        return newNode;
    }
    // Fix Red-Black Tree violations after insertion
    private void FixViolation(Node node)
    {
        while (node != root && node.Parent.Color == NodeColor.Red)
        {
            if (node.Parent == node.Grandparent?.Left)
            {
                var uncle = node.Uncle;

                // Case 1: Uncle is red
                if (uncle != null && uncle.Color == NodeColor.Red)
                {
                    node.Parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    node.Grandparent.Color = NodeColor.Red;
                    node = node.Grandparent;
                }
                else
                {
                    //Case 2: Node is right child
                    if (node == node.Parent.Right)
                    {
                        node = node.Parent;
                        RotateLeft(node);
                    }

                    //Case 3: Node is left child
                    node.Parent.Color = NodeColor.Black;
                    node.Grandparent.Color = NodeColor.Red;
                    RotateRight(node.Grandparent);
                }
            }
            else
            {
                var uncle = node.Uncle;

                //Case 1: Uncle is red
                if (uncle != null && uncle.Color == NodeColor.Red && node.Grandparent != null)
                {
                    node.Parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    node.Grandparent.Color = NodeColor.Red;
                    node = node.Grandparent;
                }
                else
                {
                    //Case 2: Node is left child
                    if (node == node.Parent.Left)
                    {
                        node = node.Parent;
                        RotateRight(node);
                    }

                    //Case 3: Node is right child
                    if (node.Grandparent != null)
                    {
                        node.Parent.Color = NodeColor.Black;
                        node.Grandparent.Color = NodeColor.Red;
                        RotateLeft(node.Grandparent);
                    }
                }
            }
        }
        if (root != null)
            root.Color = NodeColor.Black;
    }//FixViolation
     // Rotation methods
    private void RotateLeft(Node node)
    {
        var rightChild = node.Right;
        node.Right = rightChild.Left;

        if (rightChild.Left != null)
            rightChild.Left.Parent = node;

        rightChild.Parent = node.Parent;

        if (node.Parent == null)
            root = rightChild;
        else if (node == node.Parent.Left)
            node.Parent.Left = rightChild;
        else
            node.Parent.Right = rightChild;

        rightChild.Left = node;
        node.Parent = rightChild;
    }//RotateLeft
    private void RotateRight(Node node)
    {
        var leftChild = node.Left;
        node.Left = leftChild.Right;

        if (leftChild.Right != null)
            leftChild.Right.Parent = node;

        leftChild.Parent = node.Parent;

        if (node.Parent == null)
            root = leftChild;
        else if (node == node.Parent.Right)
            node.Parent.Right = leftChild;
        else
            node.Parent.Left = leftChild;

        leftChild.Right = node;
        node.Parent = leftChild;
    }//RotateRight
    private Node? GetNodeAtIndex(int index)
    {
        if (root == null) return null;

        var stack = new Stack<Node>();
        var current = root;
        int currentIndex = 0;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            current = stack.Pop();

            if (currentIndex == index)
                return current;

            currentIndex++;
            current = current.Right;
        }

        return null;
    }//GetNodeAtIndex
    private Node? FindNode(TElement item)
    {
        var current = root;

        while (current != null)
        {
            int comparison = item.CompareTo(current.Data);

            if (comparison == 0)
                return current;
            else if (comparison < 0)
                current = current.Left;
            else
                current = current.Right;
        }

        return null;
    }//FindNode
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }//GetEnumerator
    private IEnumerable<TElement> InOrderTraversal()
    {
        if (root == null) yield break;

        var stack = new Stack<Node>();
        var current = root;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            current = stack.Pop();
            yield return current.Data;
            current = current.Right;
        }//end while
    }//InOrderTraversal
#nullable disable
    private void RemoveNode(Node node)
    {
        // Case 1: Node has two children - find inorder successor and replace
        if (node.Left != null && node.Right != null)
        {
            var successor = FindMinNode(node.Right);
            node.Data = successor.Data;
            node = successor; // Now remove the successor instead
        }

        // Get the child of the node to be deleted
        Node child = node.Left ?? node.Right;
        bool isNodeBlack = node.Color == NodeColor.Black;

        if (child != null)
            child.Parent = node.Parent;

        // Update parent references
        if (node.Parent == null)
        {
            root = child;
        }
        else if (node == node.Parent.Left)
        {
            node.Parent.Left = child;
        }
        else
        {
            node.Parent.Right = child;
        }

        // If the deleted node was black, we need to fix violations
        if (isNodeBlack)
        {
            if (child != null && child.Color == NodeColor.Red)
            {
                // Simple case: if child is red, just make it black
                child.Color = NodeColor.Black;
            }
            else
            {
                // Complex case: double black situation
                if (child != null && node.Parent != null)
                    FixDoubleBlack(child, node.Parent);
            }
        }
    }
#nullable enable
    private void FixDoubleBlack(Node node, Node parent)
    {
        // Continue fixing until we reach root or the node becomes red
        while (node != root && (node == null || node.Color == NodeColor.Black))
        {
            if (node == parent.Left)
            {
                FixDoubleBlackLeft(ref node, ref parent);
            }
            else
            {
                if (node != null)
                    FixDoubleBlackRight(ref node, ref parent);
            }
        }

        // If node is not null, color it black
        if (node != null)
            node.Color = NodeColor.Black;
    }
    private void FixDoubleBlackLeft(ref Node node, ref Node parent)
    {
        var sibling = parent.Right;

        // Case 1: Sibling is red
        if (sibling.Color == NodeColor.Red)
        {
            sibling.Color = NodeColor.Black;
            parent.Color = NodeColor.Red;
            RotateLeft(parent);
            sibling = parent.Right;
        }

        // Case 2: Sibling is black with two black children
        if ((sibling.Left == null || sibling.Left.Color == NodeColor.Black) &&
            (sibling.Right == null || sibling.Right.Color == NodeColor.Black))
        {
            sibling.Color = NodeColor.Red;
            node = parent;
            parent = node.Parent;
        }
        else
        {
            // Case 3: Sibling is black with left red and right black
            if (sibling.Right == null || sibling.Right.Color == NodeColor.Black)
            {
                if (sibling.Left != null)
                    sibling.Left.Color = NodeColor.Black;
                sibling.Color = NodeColor.Red;
                RotateRight(sibling);
                sibling = parent.Right;
            }

            // Case 4: Sibling is black with right red
            sibling.Color = parent.Color;
            parent.Color = NodeColor.Black;
            if (sibling.Right != null)
                sibling.Right.Color = NodeColor.Black;
            RotateLeft(parent);
            node = root ?? throw new NullReferenceException("Root node is null"); // Terminate the loop
        }
    }
    private void FixDoubleBlackRight(ref Node node, ref Node parent)
    {
        var sibling = parent.Left;

        // Case 1: Sibling is red
        if (sibling.Color == NodeColor.Red)
        {
            sibling.Color = NodeColor.Black;
            parent.Color = NodeColor.Red;
            RotateRight(parent);
            sibling = parent.Left;
        }

        // Case 2: Sibling is black with two black children
        if ((sibling.Left == null || sibling.Left.Color == NodeColor.Black) &&
            (sibling.Right == null || sibling.Right.Color == NodeColor.Black))
        {
            sibling.Color = NodeColor.Red;
            node = parent;
            parent = node.Parent;
        }
        else
        {
            // Case 3: Sibling is black with right red and left black
            if (sibling.Left == null || sibling.Left.Color == NodeColor.Black)
            {
                if (sibling.Right != null)
                    sibling.Right.Color = NodeColor.Black;
                sibling.Color = NodeColor.Red;
                RotateLeft(sibling);
                sibling = parent.Left;
            }

            // Case 4: Sibling is black with left red
            sibling.Color = parent.Color;
            parent.Color = NodeColor.Black;
            if (sibling.Left != null)
                sibling.Left.Color = NodeColor.Black;
            RotateRight(parent);
            node = root ?? throw new NullReferenceException("Root node is null"); // Terminate the loop
        }
    }
    private static Node FindMinNode(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }
    private static Node FindMaxNode(Node node)
    {
        while (node.Right != null)
        {
            node = node.Right;
        }
        return node;
    }
    #endregion Helper methods
}//class