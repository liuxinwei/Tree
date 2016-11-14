using UnityEngine;
using System.Collections;

public class TreeUtils
{
    public bool SearchBST(TreeNode _t, int _key, TreeNode f, out TreeNode _p)
    {
        if(_t == null)
        {
            _p = f;
            return false;
        }
        else if(_key == _t.data)
        {
            _p = _t;
            return true;
        }
        else if(_key < _t.data)
        {
            return SearchBST(_t.leftNode, _key, _t, out _p);
        }
        else
        {
            return SearchBST(_t.rightNode, _key, _t, out _p);
        }
    }

    public bool InsertBST(TreeNode _t, int _key)
    {
        TreeNode p, s;
        if (!SearchBST(_t, _key, null, out p))
        {
            s = new TreeNode();
            s.data = _key;
            s.leftNode = s.rightNode = null;

            if(p == null)
            {
                _t = s;
                return true;
            }
            else if(_key < p.data)
            {
                p.leftNode = s;
            }
            else if(_key > p.data)
            {
                p.rightNode = s;
            }
            return true;
        }
        return false;
    }


}
