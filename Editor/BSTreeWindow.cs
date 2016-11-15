using UnityEngine;
using System.Collections;
using UnityEditor;
using DS.BLL;
using System.Collections.Generic;

public class BSTreeWindow : EditorWindow
{
    BSTree root;
    int maxLayer;
    int maxNodeCount;
    void OnGUI()
    {
        if(GUILayout.Button("生成树"))
        {
            List<int> list = new List<int> { 50, 30, 70, 10 };
            root = BSTreeBLL.Create(list);
            Debug.LogError(root.layer);
            BSTreeBLL.ResetIndex(root);
            maxLayer = 0;
            BSTreeBLL.ResetLayer(root, ref maxLayer);
            Debug.LogError(maxLayer);
            maxNodeCount = (int)Mathf.Pow(2, maxLayer - 1);
            BSTreeBLL.LDR(root);
            Debug.LogError(maxNodeCount);
        }
        ShowGUI(root);
    }

    void ShowGUI(BSTree _root)
    {
        if(_root == null)
        {
            return;
        }
        _root.ShowGUI(maxNodeCount);
        ShowGUI(_root.Left);
        ShowGUI(_root.Right);
    }

}
