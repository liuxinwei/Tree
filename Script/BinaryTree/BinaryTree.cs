using UnityEngine;
using System.Collections;

public class BinaryTree<T> where T : System.IComparable
{
    private TreeNode<T> root;

    public void Insert(T _data)
    {
        if (root == null)
        {
            root = new TreeNode<T>();
            root.data = _data;
        }
        else
        {
            Insert(root, _data);
        }
    }

    private void Insert(TreeNode<T> _root, T _data)
    {
        if(root == null)
        {
            return;
        }
        if(_data.CompareTo(root.data) == 1)
        {
            if (_root.rightNode == null)
            {
                _root.rightNode = new TreeNode<T>();
                _root.rightNode.data = _data;
            }
            else
            {
                Insert(_root.rightNode, _data);
            }
        }
        else if (_data.CompareTo(root.data) == -1)
        {
            if (_root.leftNode == null)
            {
                _root.leftNode = new TreeNode<T>();
                _root.leftNode.data = _data;
            }
            else
            {
                Insert(_root.leftNode, _data);
            }
        }
    }

    public bool Search(T _data)
    {
        return Search(root, _data);
    }

    private bool Search(TreeNode<T> _root, T _data)
    {
        if(_root == null)
        {
            return false;
        }
        if (_data.CompareTo(_root.data) == 0)
        {
            return true;
        }
        if (_data.CompareTo(_root.data) == 1)
        {
            return Search(_root.rightNode, _data);
        }
        else
        {
            return Search(_root.leftNode, _data);
        }
    }

    public void Delete(T _data)
    {
        Delete(ref root, _data);
    }

    private void Delete(ref TreeNode<T> _root, T _data)
    {
        if (_root == null)
        {
            return;
        }
        if (_data.CompareTo(_root.data) == 0)
        {
            if (_root.rightNode == null && _root.leftNode == null)
            {
                _root = null;
            }
            else if (_root.rightNode == null && _root.leftNode != null)
            {
                _root = _root.leftNode;
            }
            else if (_root.rightNode != null && _root.leftNode == null)
            {
                _root = _root.rightNode;
            }
            else
            {
                TreeNode<T> parentNode = _root.leftNode;
                TreeNode<T> curNode = _root.leftNode;
                while (curNode != null)
                {
                    if (curNode.rightNode == null)
                    {
                        break;
                    }
                    parentNode = curNode;
                    curNode = curNode.rightNode;
                }
                if (parentNode == curNode)
                {
                    curNode.rightNode = _root.rightNode;
                    _root = curNode;
                }
                else
                {
                    parentNode.rightNode = curNode.leftNode;
                    curNode.leftNode = _root.leftNode;
                    curNode.rightNode = _root.rightNode;
                    _root = curNode;
                }
            }
            return;
        }
        if (_data.CompareTo(_root.data) == 1)
        {
            Delete(ref _root.rightNode, _data);
        }
        else if (_data.CompareTo(_root.data) == -1)
        {
            Delete(ref _root.leftNode, _data);
        }
    }
}
