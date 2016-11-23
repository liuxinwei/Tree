using UnityEngine;
using System.Collections;

public class BinaryTree : MonoBehaviour 
{
    public static void Insert(TreeNode _root, int key)
    {
        if(_root == null)
        {
            return;
        }
        if (key > _root.data)
        {
            if (_root.rightNode == null)
            {
                _root.rightNode = new TreeNode();
                _root.rightNode.data = key;
            }
            else
            {
                Insert(_root.rightNode, key);
            }
        }
        else if(key < _root.data)
        {
            if (_root.leftNode == null)
            {
                _root.leftNode = new TreeNode();
                _root.leftNode.data = key;
            }
            else
            {
                Insert(_root.leftNode, key);
            }
        }
    }

    public static TreeNode Search(TreeNode _root, int _key)
    {
        if (_root == null)
        {
            return null;
        }
        if (_root.data == _key)
        {
            return _root;
        }
        if (_root.data < _key)
        {
            return Search(_root.rightNode, _key);
        }
        else
        {
            return Search(_root.rightNode, _key);
        }
    }

    public static void Delete(ref TreeNode _root, int _key)
    {
        if (_root == null)
        {
            return;
        }
        if (_root.data == _key)
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
                TreeNode parentNode = _root.leftNode;
                TreeNode curNode = _root.leftNode;
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
        if (_root.data < _key)
        {
            Delete(ref _root.rightNode, _key);
        }
        else
        {
            Delete(ref _root.leftNode, _key);
        }
    }

    public static void ResetIndex(TreeNode _bsTree)
    {
        if (_bsTree == null)
        {
            return;
        }
        if (_bsTree.leftNode != null)
        {
            _bsTree.leftNode.index = _bsTree.index * 2;
            ResetIndex(_bsTree.leftNode);
        }
        if (_bsTree.rightNode != null)
        {
            _bsTree.rightNode.index = _bsTree.index * 2 + 1;
            ResetIndex(_bsTree.rightNode);
        }
    }

    public static void ResetLayer(TreeNode _bsTree, ref int _maxLayer)
    {
        if (_bsTree == null)
        {
            return;
        }
        if (_bsTree.leftNode != null)
        {
            _bsTree.leftNode.layer = _bsTree.layer + 1;
            _maxLayer = Mathf.Max(_maxLayer, _bsTree.leftNode.layer);
            ResetLayer(_bsTree.leftNode, ref _maxLayer);
        }
        if (_bsTree.rightNode != null)
        {
            _bsTree.rightNode.layer = _bsTree.layer + 1;
            _maxLayer = Mathf.Max(_maxLayer, _bsTree.rightNode.layer);
            ResetLayer(_bsTree.rightNode, ref _maxLayer);
        }
    }

    public static void DLR(TreeNode _bsTree)
    {
        if (_bsTree == null)
        {
            return;
        }
        DLR(_bsTree.leftNode);
        DLR(_bsTree.rightNode);
    }
}
